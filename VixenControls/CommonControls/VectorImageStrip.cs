namespace VixenControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class VectorImageStrip : UserControl
    {
        private IContainer components = null;
        private int m_depth;
        private const int m_imageBorderPad = 6;
        private System.Drawing.Rectangle m_imageListRect;
        private const int m_imagePad = 5;
        private VectorImageCollection m_images;
        private int m_imagesShown;
        private System.Drawing.Rectangle m_lessButtonBounds;
        private Point[] m_lessButtonPoints;
        private bool m_lessButtonVisible = false;
        private System.Drawing.Rectangle m_moreButtonBounds;
        private Point[] m_moreButtonPoints;
        private bool m_moreButtonVisible = false;
        private const int m_navButtonSize = 10;
        private int m_startIndex;
        private VectorImageCollection.OnItemsChange m_vectorImageCollectionChange;

        public VectorImageStrip()
        {
            this.InitializeComponent();
            this.BackColor = Color.Transparent;
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            this.m_images = new VectorImageCollection();
            this.m_vectorImageCollectionChange = new VectorImageCollection.OnItemsChange(this.VectorImageChange);
            this.m_images.ItemsChange += this.m_vectorImageCollectionChange;
            this.m_startIndex = 0;
            this.m_imagesShown = 0;
            this.m_depth = 1;
        }

        private void CalcImageContainerRect()
        {
            int num;
            Point[] pointArray;
            int num2 = 5;
            if (base.Width > base.Height)
            {
                num = this.m_lessButtonBounds.Height >> 1;
                this.m_imageListRect = new System.Drawing.Rectangle(20, 0, base.ClientRectangle.Width - 40, base.ClientRectangle.Height);
                this.m_lessButtonBounds = new System.Drawing.Rectangle(0, 0, 10, base.ClientRectangle.Height - 1);
                pointArray = new Point[] { new Point(this.m_lessButtonBounds.Left + 2, num), new Point(this.m_lessButtonBounds.Right - 1, num - num2), new Point(this.m_lessButtonBounds.Right - 1, num + num2) };
                this.m_lessButtonPoints = pointArray;
                this.m_moreButtonBounds = new System.Drawing.Rectangle((base.ClientRectangle.Right - 10) - 1, 0, 10, base.ClientRectangle.Height - 1);
                pointArray = new Point[] { new Point(this.m_moreButtonBounds.Right - 2, num), new Point(this.m_moreButtonBounds.Left + 2, num - num2), new Point(this.m_moreButtonBounds.Left + 2, num + num2) };
                this.m_moreButtonPoints = pointArray;
            }
            else
            {
                num = this.m_lessButtonBounds.Width >> 1;
                this.m_imageListRect = new System.Drawing.Rectangle(0, 20, base.ClientRectangle.Width, base.ClientRectangle.Height - 40);
                this.m_lessButtonBounds = new System.Drawing.Rectangle(0, 0, base.ClientRectangle.Width - 1, 10);
                pointArray = new Point[] { new Point(num, this.m_lessButtonBounds.Top + 2), new Point(num - num2, this.m_lessButtonBounds.Bottom - 1), new Point(num + num2, this.m_lessButtonBounds.Bottom - 1) };
                this.m_lessButtonPoints = pointArray;
                this.m_moreButtonBounds = new System.Drawing.Rectangle(0, (base.ClientRectangle.Bottom - 10) - 1, base.ClientRectangle.Width - 1, 10);
                pointArray = new Point[] { new Point(num, this.m_moreButtonBounds.Bottom - 2), new Point(num - num2, this.m_moreButtonBounds.Top + 2), new Point(num + num2, this.m_moreButtonBounds.Top + 2) };
                this.m_moreButtonPoints = pointArray;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawVectorImage(Graphics g, VectorListItem listItem)
        {
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);
            VectorImage.Image image = listItem.Image;
            Point location = image.Location;
            g.FillPath(Brushes.White, listItem.BorderPath);
            g.DrawPath(Pens.DarkSlateBlue, listItem.BorderPath);
            foreach (VectorImageElement element in image.Elements)
            {
                VectorImage.Rectangle rectangle;
                brush.Color = pen.Color = element.Color;
                switch (element.Type)
                {
                    case VectorImage.PrimitiveType.Line:
                    {
                        VectorImage.Line line = (VectorImage.Line) element;
                        g.DrawLine(pen, (int) (location.X + line.X), (int) (location.Y + line.Y), (int) (location.X + line.X2), (int) (location.Y + line.Y2));
                        break;
                    }
                    case VectorImage.PrimitiveType.Rectangle:
                        rectangle = (VectorImage.Rectangle) element;
                        g.DrawRectangle(pen, location.X + rectangle.X, location.Y + rectangle.Y, rectangle.Width, rectangle.Height);
                        break;

                    case VectorImage.PrimitiveType.FilledRectangle:
                        rectangle = (VectorImage.FilledRectangle) element;
                        g.FillRectangle(brush, location.X + rectangle.X, location.Y + rectangle.Y, rectangle.Width, rectangle.Height);
                        break;

                    case VectorImage.PrimitiveType.Ellipse:
                        rectangle = (VectorImage.Ellipse) element;
                        g.DrawEllipse(pen, location.X + rectangle.X, location.Y + rectangle.Y, rectangle.Width, rectangle.Height);
                        break;

                    case VectorImage.PrimitiveType.FilledEllipse:
                        rectangle = (VectorImage.FilledEllipse) element;
                        g.FillEllipse(brush, location.X + rectangle.X, location.Y + rectangle.Y, rectangle.Width, rectangle.Height);
                        break;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.m_lessButtonVisible && this.m_lessButtonBounds.Contains(e.Location))
            {
                this.m_startIndex--;
                this.Recalc();
            }
            else if (this.m_moreButtonVisible && this.m_moreButtonBounds.Contains(e.Location))
            {
                this.m_startIndex++;
                this.Recalc();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.None)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            RectangleF clipBounds = g.ClipBounds;
            g.SetClip(this.m_imageListRect, CombineMode.Intersect);
            for (int i = 0; i < this.m_imagesShown; i++)
            {
                this.DrawVectorImage(g, this.m_images[this.m_startIndex + i]);
            }
            g.SetClip(clipBounds);
            if (this.m_lessButtonVisible)
            {
                g.FillRectangle(Brushes.White, this.m_lessButtonBounds);
                g.DrawRectangle(Pens.Black, this.m_lessButtonBounds);
                g.FillPolygon(Brushes.Gray, this.m_lessButtonPoints);
            }
            if (this.m_moreButtonVisible)
            {
                g.FillRectangle(Brushes.White, this.m_moreButtonBounds);
                g.DrawRectangle(Pens.Black, this.m_moreButtonBounds);
                g.FillPolygon(Brushes.Gray, this.m_moreButtonPoints);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Recalc();
            base.OnResize(e);
        }

        private void Recalc()
        {
            if (this.m_images.Count != 0)
            {
                float num;
                int left;
                int top;
                int startIndex;
                VectorListItem item;
                VectorImage.Image image;
                Size size;
                Size originalSize;
                int num8;
                this.CalcImageContainerRect();
                int num5 = 0;
                int num6 = 0;
                int count = this.m_images.Count;
                if (base.Width > base.Height)
                {
                    num8 = this.m_imageListRect.Height / this.m_depth;
                    left = this.m_imageListRect.Left;
                    startIndex = this.m_startIndex;
                    while ((left < this.m_imageListRect.Right) && (startIndex < count))
                    {
                        num5 = 0;
                        top = 0;
                        while ((top < this.m_imageListRect.Height) && (startIndex < count))
                        {
                            item = this.m_images[startIndex];
                            image = item.Image;
                            originalSize = image.OriginalSize;
                            num = Math.Min((float) (num8 / (originalSize.Height + 6)), ((float) this.m_imageListRect.Width) / ((float) (originalSize.Width + 6)));
                            if (num < 1f)
                            {
                                image.ScaleTo(num);
                            }
                            size = image.Size;
                            item.ListItemBounds.X = left;
                            item.ListItemBounds.Y = top;
                            item.ListItemBounds.Width = size.Width + 6;
                            item.ListItemBounds.Height = num8 - 1;
                            image.Location.X = left + ((item.ListItemBounds.Width - size.Width) / 2);
                            image.Location.Y = top + ((item.ListItemBounds.Height - size.Height) / 2);
                            if (item.BorderPath != null)
                            {
                                item.BorderPath.Dispose();
                            }
                            item.BorderPath = this.RoundedRectPath(left, top, item.ListItemBounds.Width, item.ListItemBounds.Height, 4);
                            num5 = Math.Max(num5, size.Width + 6);
                            top += num8;
                            startIndex++;
                        }
                        left += num5 + 5;
                    }
                    this.m_moreButtonVisible = (this.m_images[startIndex - 1].ListItemBounds.Right > this.m_imageListRect.Right) || (startIndex < this.m_images.Count);
                }
                else
                {
                    num8 = this.m_imageListRect.Width / this.m_depth;
                    top = this.m_imageListRect.Top;
                    startIndex = this.m_startIndex;
                    while ((top < this.m_imageListRect.Bottom) && (startIndex < count))
                    {
                        num6 = 0;
                        left = this.m_imageListRect.Left;
                        while ((left < this.m_imageListRect.Right) && (startIndex < count))
                        {
                            item = this.m_images[startIndex];
                            image = item.Image;
                            originalSize = image.OriginalSize;
                            num = Math.Min(((float) this.m_imageListRect.Height) / ((float) (originalSize.Height + 6)), (float) (num8 / (originalSize.Width + 6)));
                            if (num < 1f)
                            {
                                image.ScaleTo(num);
                            }
                            size = image.Size;
                            item.ListItemBounds.X = left;
                            item.ListItemBounds.Y = top;
                            item.ListItemBounds.Width = num8 - 1;
                            item.ListItemBounds.Height = size.Height + 6;
                            image.Location.X = left + ((item.ListItemBounds.Width - size.Width) / 2);
                            image.Location.Y = top + ((item.ListItemBounds.Height - size.Height) / 2);
                            if (item.BorderPath != null)
                            {
                                item.BorderPath.Dispose();
                            }
                            item.BorderPath = this.RoundedRectPath(left, top, item.ListItemBounds.Width, item.ListItemBounds.Height, 4);
                            num6 = Math.Max(num6, size.Height + 6);
                            left += num8;
                            startIndex++;
                        }
                        top += num6 + 5;
                    }
                    this.m_moreButtonVisible = (this.m_images[startIndex - 1].ListItemBounds.Bottom > this.m_imageListRect.Bottom) || (startIndex < this.m_images.Count);
                }
                this.m_lessButtonVisible = this.m_startIndex > 0;
                this.m_imagesShown = startIndex - this.m_startIndex;
                this.Refresh();
            }
        }

        private GraphicsPath RoundedRectPath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(x + radius, y, (x + width) - (radius * 2), y);
            path.AddArc((x + width) - (radius * 2), y, radius * 2, radius * 2, 270f, 90f);
            path.AddLine((int) (x + width), (int) (y + radius), (int) (x + width), (int) ((y + height) - (radius * 2)));
            path.AddArc((int) ((x + width) - (radius * 2)), (int) ((y + height) - (radius * 2)), (int) (radius * 2), (int) (radius * 2), 0f, 90f);
            path.AddLine((int) ((x + width) - (radius * 2)), (int) (y + height), (int) (x + radius), (int) (y + height));
            path.AddArc(x, (y + height) - (radius * 2), radius * 2, radius * 2, 90f, 90f);
            path.AddLine(x, (y + height) - (radius * 2), x, y + radius);
            path.AddArc(x, y, radius * 2, radius * 2, 180f, 90f);
            path.CloseFigure();
            return path;
        }

        private void VectorImageChange()
        {
            this.Recalc();
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [DefaultValue(1)]
        public int Depth
        {
            get
            {
                return this.m_depth;
            }
            set
            {
                this.m_depth = value;
                this.Recalc();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public VectorImageCollection Images
        {
            get
            {
                return this.m_images;
            }
            set
            {
                this.m_images = value;
            }
        }
    }
}

