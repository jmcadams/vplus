using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonControls {
    public class VectorImageStrip : UserControl {
        private IContainer components;
        private int _depth;
        private Rectangle _imageListRect;
        private int _imagesShown;
        private Rectangle _lessButtonBounds;
        private Point[] _lessButtonPoints;
        private bool _lessButtonVisible;
        private Rectangle _moreButtonBounds;
        private Point[] _moreButtonPoints;
        private bool _moreButtonVisible;
        private int _startIndex;
        private readonly VectorImageCollection.OnItemsChange _vectorImageCollectionChange;


        public VectorImageStrip() {
            InitializeComponent();
            BackColor = Color.Transparent;
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            Images = new VectorImageCollection();
            _vectorImageCollectionChange = VectorImageChange;
            Images.ItemsChange += _vectorImageCollectionChange;
            _startIndex = 0;
            _imagesShown = 0;
            _depth = 1;
        }


        private void CalcImageContainerRect() {
            int num;
            Point[] pointArray;
            const int num2 = 5;
            if (Width > Height) {
                num = _lessButtonBounds.Height >> 1;
                _imageListRect = new Rectangle(20, 0, ClientRectangle.Width - 40, ClientRectangle.Height);
                _lessButtonBounds = new Rectangle(0, 0, 10, ClientRectangle.Height - 1);
                pointArray = new[] {
                    new Point(_lessButtonBounds.Left + 2, num), new Point(_lessButtonBounds.Right - 1, num - num2),
                    new Point(_lessButtonBounds.Right - 1, num + num2)
                };
                _lessButtonPoints = pointArray;
                _moreButtonBounds = new Rectangle((ClientRectangle.Right - 10) - 1, 0, 10, ClientRectangle.Height - 1);
                pointArray = new[] {
                    new Point(_moreButtonBounds.Right - 2, num), new Point(_moreButtonBounds.Left + 2, num - num2),
                    new Point(_moreButtonBounds.Left + 2, num + num2)
                };
                _moreButtonPoints = pointArray;
            }
            else {
                num = _lessButtonBounds.Width >> 1;
                _imageListRect = new Rectangle(0, 20, ClientRectangle.Width, ClientRectangle.Height - 40);
                _lessButtonBounds = new Rectangle(0, 0, ClientRectangle.Width - 1, 10);
                pointArray = new[] {
                    new Point(num, _lessButtonBounds.Top + 2), new Point(num - num2, _lessButtonBounds.Bottom - 1),
                    new Point(num + num2, _lessButtonBounds.Bottom - 1)
                };
                _lessButtonPoints = pointArray;
                _moreButtonBounds = new Rectangle(0, (ClientRectangle.Bottom - 10) - 1, ClientRectangle.Width - 1, 10);
                pointArray = new[] {
                    new Point(num, _moreButtonBounds.Bottom - 2), new Point(num - num2, _moreButtonBounds.Top + 2),
                    new Point(num + num2, _moreButtonBounds.Top + 2)
                };
                _moreButtonPoints = pointArray;
            }
        }


        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private static void DrawVectorImage(Graphics g, VectorListItem listItem) {
            var pen = new Pen(Color.Black);
            var brush = new SolidBrush(Color.Black);
            var image = listItem.Image;
            var location = image.Location;
            g.FillPath(Brushes.White, listItem.BorderPath);
            g.DrawPath(Pens.DarkSlateBlue, listItem.BorderPath);
            foreach (var element in image.Elements) {
                VectorImage.Rectangle rectangle;
                brush.Color = pen.Color = element.Color;
                switch (element.Type) {
                    case VectorImage.PrimitiveType.Line: {
                        var line = (VectorImage.Line) element;
                        g.DrawLine(pen, location.X + line.X, location.Y + line.Y, location.X + line.X2, location.Y + line.Y2);
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


        private void InitializeComponent() {
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            if (_lessButtonVisible && _lessButtonBounds.Contains(e.Location)) {
                _startIndex--;
                Recalc();
            }
            else if (_moreButtonVisible && _moreButtonBounds.Contains(e.Location)) {
                _startIndex++;
                Recalc();
            }
        }


        protected override void OnMouseLeave(EventArgs e) {}

        protected override void OnMouseMove(MouseEventArgs e) {}


        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;
            var clipBounds = g.ClipBounds;
            g.SetClip(_imageListRect, CombineMode.Intersect);
            for (var i = 0; i < _imagesShown; i++) {
                DrawVectorImage(g, Images[_startIndex + i]);
            }
            g.SetClip(clipBounds);
            if (_lessButtonVisible) {
                g.FillRectangle(Brushes.White, _lessButtonBounds);
                g.DrawRectangle(Pens.Black, _lessButtonBounds);
                g.FillPolygon(Brushes.Gray, _lessButtonPoints);
            }
            if (!_moreButtonVisible) {
                return;
            }

            g.FillRectangle(Brushes.White, _moreButtonBounds);
            g.DrawRectangle(Pens.Black, _moreButtonBounds);
            g.FillPolygon(Brushes.Gray, _moreButtonPoints);
        }


        protected override void OnResize(EventArgs e) {
            Recalc();
            base.OnResize(e);
        }


        private void Recalc() {
            if (Images.Count == 0) {
                return;
            }

            float num;
            int left;
            int top;
            int startIndex;
            VectorListItem item;
            VectorImage.Image image;
            Size size;
            Size originalSize;
            int num8;
            CalcImageContainerRect();
            var count = Images.Count;
            if (Width > Height) {
                num8 = _imageListRect.Height / _depth;
                left = _imageListRect.Left;
                startIndex = _startIndex;
                while ((left < _imageListRect.Right) && (startIndex < count)) {
                    var num5 = 0;
                    top = 0;
                    while ((top < _imageListRect.Height) && (startIndex < count)) {
                        item = Images[startIndex];
                        image = item.Image;
                        originalSize = image.OriginalSize;
                        num = Math.Min(num8 / (originalSize.Height + 6f), _imageListRect.Width / (originalSize.Width + 6f));
                        if (num < 1f) {
                            image.ScaleTo(num);
                        }
                        size = image.Size;
                        item.ListItemBounds.X = left;
                        item.ListItemBounds.Y = top;
                        item.ListItemBounds.Width = size.Width + 6;
                        item.ListItemBounds.Height = num8 - 1;
                        image.Location.X = left + ((item.ListItemBounds.Width - size.Width) / 2);
                        image.Location.Y = top + ((item.ListItemBounds.Height - size.Height) / 2);
                        if (item.BorderPath != null) {
                            item.BorderPath.Dispose();
                        }
                        item.BorderPath = RoundedRectPath(left, top, item.ListItemBounds.Width, item.ListItemBounds.Height, 4);
                        num5 = Math.Max(num5, size.Width + 6);
                        top += num8;
                        startIndex++;
                    }
                    left += num5 + 5;
                }
                _moreButtonVisible = (Images[startIndex - 1].ListItemBounds.Right > _imageListRect.Right) || (startIndex < Images.Count);
            }
            else {
                num8 = _imageListRect.Width / _depth;
                top = _imageListRect.Top;
                startIndex = _startIndex;
                while ((top < _imageListRect.Bottom) && (startIndex < count)) {
                    var num6 = 0;
                    left = _imageListRect.Left;
                    while ((left < _imageListRect.Right) && (startIndex < count)) {
                        item = Images[startIndex];
                        image = item.Image;
                        originalSize = image.OriginalSize;
                        num = Math.Min(_imageListRect.Height / (originalSize.Height + 6f), num8 / (originalSize.Width + 6f));
                        if (num < 1f) {
                            image.ScaleTo(num);
                        }
                        size = image.Size;
                        item.ListItemBounds.X = left;
                        item.ListItemBounds.Y = top;
                        item.ListItemBounds.Width = num8 - 1;
                        item.ListItemBounds.Height = size.Height + 6;
                        image.Location.X = left + ((item.ListItemBounds.Width - size.Width) / 2);
                        image.Location.Y = top + ((item.ListItemBounds.Height - size.Height) / 2);
                        if (item.BorderPath != null) {
                            item.BorderPath.Dispose();
                        }
                        item.BorderPath = RoundedRectPath(left, top, item.ListItemBounds.Width, item.ListItemBounds.Height, 4);
                        num6 = Math.Max(num6, size.Height + 6);
                        left += num8;
                        startIndex++;
                    }
                    top += num6 + 5;
                }
                _moreButtonVisible = (Images[startIndex - 1].ListItemBounds.Bottom > _imageListRect.Bottom) || (startIndex < Images.Count);
            }
            _lessButtonVisible = _startIndex > 0;
            _imagesShown = startIndex - _startIndex;
            Refresh();
        }


        private GraphicsPath RoundedRectPath(int x, int y, int width, int height, int radius) {
            var path = new GraphicsPath();
            path.AddLine(x + radius, y, (x + width) - (radius * 2), y);
            path.AddArc((x + width) - (radius * 2), y, radius * 2, radius * 2, 270f, 90f);
            path.AddLine(x + width, y + radius, x + width, (y + height) - (radius * 2));
            path.AddArc((x + width) - (radius * 2), (y + height) - (radius * 2), radius * 2, radius * 2, 0f, 90f);
            path.AddLine((x + width) - (radius * 2), y + height, x + radius, y + height);
            path.AddArc(x, (y + height) - (radius * 2), radius * 2, radius * 2, 90f, 90f);
            path.AddLine(x, (y + height) - (radius * 2), x, y + radius);
            path.AddArc(x, y, radius * 2, radius * 2, 180f, 90f);
            path.CloseFigure();
            return path;
        }


        private void VectorImageChange() {
            Recalc();
        }


        [DefaultValue(typeof (Color), "Transparent")]
        public override sealed Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(1)]
        public int Depth {
            get { return _depth; }
            set {
                _depth = value;
                Recalc();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        public VectorImageCollection Images { get; set; }
    }
}
