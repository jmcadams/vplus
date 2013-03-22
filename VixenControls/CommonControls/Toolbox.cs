namespace VixenControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class Toolbox : UserControl
    {
        private IContainer components = null;
        private ToolboxCategoryCollection m_categories;
        private ToolboxCategoryCollection.OnItemsChange m_categoryCollectionChange;
        private Font m_categoryFont;
        private Font m_categoryItemFont;
        private object m_hoveredOver = null;

        public Toolbox()
        {
            this.InitializeComponent();
            this.BackColor = Color.White;
            this.m_categoryCollectionChange = new ToolboxCategoryCollection.OnItemsChange(this.CategoryChange);
            this.m_categoryFont = new Font("Arial", 8f, FontStyle.Bold);
            this.m_categoryItemFont = new Font("Arial", 8f);
            this.m_categories = new ToolboxCategoryCollection();
            this.m_categories.ItemsChange += this.m_categoryCollectionChange;
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        private void CategoryChange()
        {
            this.Recalc();
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_categories.ItemsChange -= this.m_categoryCollectionChange;
            base.Dispose(disposing);
        }

        private object FindObjectAt(Point point)
        {
            int num = 0;
            for (int i = 0; i < this.m_categories.Count; i++)
            {
                num += 0x10;
                ToolboxCategory category = this.m_categories[i];
                if (point.Y < num)
                {
                    return category;
                }
                if (category.Expanded)
                {
                    foreach (ToolboxItem item in category.Items)
                    {
                        num += 30;
                        if (point.Y < num)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "Toolbox";
            base.ResumeLayout(false);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.m_hoveredOver is ToolboxCategory)
            {
                ToolboxCategory hoveredOver = (ToolboxCategory) this.m_hoveredOver;
                if (hoveredOver.ButtonBounds.Contains(e.Location))
                {
                    if (hoveredOver.Expanded)
                    {
                        hoveredOver.Collapse();
                    }
                    else
                    {
                        hoveredOver.Expand();
                    }
                }
            }
            else if (this.m_hoveredOver is ToolboxItem)
            {
                ToolboxItem item = (ToolboxItem) this.m_hoveredOver;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_hoveredOver = null;
            this.Refresh();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (((e.Button & MouseButtons.Left) != MouseButtons.None) && (this.m_hoveredOver is ToolboxItem))
            {
                base.DoDragDrop(this.m_hoveredOver, DragDropEffects.Move);
            }
            else
            {
                object obj2 = this.FindObjectAt(e.Location);
                if (obj2 != this.m_hoveredOver)
                {
                    this.m_hoveredOver = obj2;
                    this.Refresh();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            System.Drawing.Rectangle clientRectangle = base.ClientRectangle;
            graphics.FillRectangle(new SolidBrush(this.BackColor), clientRectangle);
            clientRectangle.Width--;
            clientRectangle.Height--;
            graphics.DrawRectangle(Pens.Black, clientRectangle);
            int y = 0;
            for (int i = 0; (y < base.ClientRectangle.Height) && (i < this.m_categories.Count); i++)
            {
                ToolboxCategory category = this.m_categories[i];
                System.Drawing.Rectangle bounds = category.Bounds;
                LinearGradientBrush brush = new LinearGradientBrush(bounds, Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                graphics.FillRectangle(brush, bounds);
                graphics.DrawString(category.Name, this.m_categoryFont, Brushes.White, 20f, (float) bounds.Y);
                System.Drawing.Rectangle buttonBounds = category.ButtonBounds;
                graphics.FillRectangle(Brushes.White, buttonBounds);
                graphics.DrawRectangle(Pens.Black, buttonBounds);
                graphics.DrawLine(Pens.Black, (int) (buttonBounds.X + 2), (int) (buttonBounds.Y + 5), (int) (buttonBounds.Right - 2), (int) (buttonBounds.Y + 5));
                if (!(category.Expanded || (category.Items.Count <= 0)))
                {
                    graphics.DrawLine(Pens.Black, (int) (buttonBounds.X + 5), (int) (buttonBounds.Y + 2), (int) (buttonBounds.X + 5), (int) (buttonBounds.Bottom - 2));
                }
                y += 0x10;
                if (category.Expanded)
                {
                    for (int j = 0; (y < base.ClientRectangle.Height) && (j < category.Items.Count); j++)
                    {
                        ToolboxItem item = category[j];
                        item.Bounds = new System.Drawing.Rectangle(3, y, base.ClientRectangle.Width - 7, 30);
                        if (item == this.m_hoveredOver)
                        {
                            graphics.FillRectangle(Brushes.LightSteelBlue, item.Bounds);
                            graphics.DrawRectangle(Pens.SteelBlue, item.Bounds);
                        }
                        if (item.Image != null)
                        {
                            graphics.DrawImage(item.Image, item.Bounds.Left + 3, y + 3, 0x18, 0x18);
                        }
                        graphics.DrawString(item.Name, this.m_categoryItemFont, Brushes.Black, 40f, (float) (y + 7));
                        y += 30;
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Recalc();
            this.Refresh();
            base.OnResize(e);
        }

        private void Recalc()
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(1, 0, base.ClientRectangle.Width - 2, 0x10);
            System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(3, 3, 10, 10);
            int y = 0;
            for (int i = 0; (y < base.ClientRectangle.Height) && (i < this.m_categories.Count); i++)
            {
                ToolboxCategory category = this.m_categories[i];
                rectangle.Y = y;
                category.Bounds = rectangle;
                rectangle2.Y = category.Bounds.Y + 3;
                category.ButtonBounds = rectangle2;
                y += 0x10;
                if (category.Expanded)
                {
                    for (int j = 0; (y < base.ClientRectangle.Height) && (j < category.Items.Count); j++)
                    {
                        ToolboxItem item = category[j];
                        item.Bounds = new System.Drawing.Rectangle(3, y, base.ClientRectangle.Width - 7, 30);
                        y += 30;
                    }
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolboxCategoryCollection Categories
        {
            get
            {
                return this.m_categories;
            }
            set
            {
                this.m_categories = value;
            }
        }
    }
}

