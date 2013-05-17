using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CommonControls {
    public class Toolbox : UserControl {
        private readonly IContainer components = null;
        private readonly ToolboxCategoryCollection.OnItemsChange _categoryCollectionChange;
        private readonly Font _categoryFont;
        private readonly Font _categoryItemFont;
        private object _hoveredOver;


        public Toolbox() {
            InitializeComponent();
            BackColor = Color.White;
            _categoryCollectionChange = CategoryChange;
            _categoryFont = new Font("Arial", 8f, FontStyle.Bold);
            _categoryItemFont = new Font("Arial", 8f);
            Categories = new ToolboxCategoryCollection();
            Categories.ItemsChange += _categoryCollectionChange;
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }


        private void CategoryChange() {
            Recalc();
            Refresh();
        }


        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            Categories.ItemsChange -= _categoryCollectionChange;
            base.Dispose(disposing);
        }


        private object FindObjectAt(Point point) {
            var num = 0;
            for (var i = 0; i < Categories.Count; i++) {
                num += 16;
                var category = Categories[i];
                if (point.Y < num) {
                    return category;
                }
                if (!category.Expanded) {
                    continue;
                }
                foreach (ToolboxItem item in category.Items) {
                    num += 30;
                    if (point.Y < num) {
                        return item;
                    }
                }
            }
            return null;
        }


        private void InitializeComponent() {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "Toolbox";
            ResumeLayout(false);
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            var hoveredOver = _hoveredOver as ToolboxCategory;
            if (hoveredOver != null) {
                if (hoveredOver.ButtonBounds.Contains(e.Location)) {
                    if (hoveredOver.Expanded) {
                        hoveredOver.Collapse();
                    }
                    else {
                        hoveredOver.Expand();
                    }
                }
            }
            //else {
            //    var toolBoxItem = _hoveredOver as ToolboxItem;
            //    if (toolBoxItem != null)
            //    {
            //        var item = toolBoxItem;
            //    }
            //}
            base.OnMouseDown(e);
        }


        protected override void OnMouseLeave(EventArgs e) {
            _hoveredOver = null;
            Refresh();
            base.OnMouseLeave(e);
        }


        protected override void OnMouseMove(MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != MouseButtons.None) && (_hoveredOver is ToolboxItem)) {
                DoDragDrop(_hoveredOver, DragDropEffects.Move);
            }
            else {
                var obj2 = FindObjectAt(e.Location);
                if (obj2 != _hoveredOver) {
                    _hoveredOver = obj2;
                    Refresh();
                }
            }
            base.OnMouseMove(e);
        }


        protected override void OnPaint(PaintEventArgs e) {
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            Rectangle clientRectangle = ClientRectangle;
            graphics.FillRectangle(new SolidBrush(BackColor), clientRectangle);
            clientRectangle.Width--;
            clientRectangle.Height--;
            graphics.DrawRectangle(Pens.Black, clientRectangle);
            var y = 0;
            for (var i = 0; (y < ClientRectangle.Height) && (i < Categories.Count); i++) {
                var category = Categories[i];
                var bounds = category.Bounds;
                var brush = new LinearGradientBrush(bounds, Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                graphics.FillRectangle(brush, bounds);
                graphics.DrawString(category.Name, _categoryFont, Brushes.White, 20f, bounds.Y);
                var buttonBounds = category.ButtonBounds;
                graphics.FillRectangle(Brushes.White, buttonBounds);
                graphics.DrawRectangle(Pens.Black, buttonBounds);
                graphics.DrawLine(Pens.Black, buttonBounds.X + 2, buttonBounds.Y + 5, buttonBounds.Right - 2, buttonBounds.Y + 5);
                if (!(category.Expanded || (category.Items.Count <= 0))) {
                    graphics.DrawLine(Pens.Black, buttonBounds.X + 5, buttonBounds.Y + 2, buttonBounds.X + 5, buttonBounds.Bottom - 2);
                }
                y += 16;
                if (!category.Expanded) {
                    continue;
                }
                for (var j = 0; (y < ClientRectangle.Height) && (j < category.Items.Count); j++) {
                    var item = category[j];
                    item.Bounds = new Rectangle(3, y, ClientRectangle.Width - 7, 30);
                    if (item == _hoveredOver) {
                        graphics.FillRectangle(Brushes.LightSteelBlue, item.Bounds);
                        graphics.DrawRectangle(Pens.SteelBlue, item.Bounds);
                    }
                    if (item.Image != null) {
                        graphics.DrawImage(item.Image, item.Bounds.Left + 3, y + 3, 24, 24);
                    }
                    graphics.DrawString(item.Name, _categoryItemFont, Brushes.Black, 40f, y + 7);
                    y += 30;
                }
            }
        }


        protected override void OnResize(EventArgs e) {
            Recalc();
            Refresh();
            base.OnResize(e);
        }


        private void Recalc() {
            var rectangle = new Rectangle(1, 0, ClientRectangle.Width - 2, 0x10);
            var rectangle2 = new Rectangle(3, 3, 10, 10);
            var y = 0;
            for (var i = 0; (y < ClientRectangle.Height) && (i < Categories.Count); i++) {
                var category = Categories[i];
                rectangle.Y = y;
                category.Bounds = rectangle;
                rectangle2.Y = category.Bounds.Y + 3;
                category.ButtonBounds = rectangle2;
                y += 0x10;
                if (!category.Expanded) {
                    continue;
                }
                for (var j = 0; (y < ClientRectangle.Height) && (j < category.Items.Count); j++) {
                    var item = category[j];
                    item.Bounds = new Rectangle(3, y, ClientRectangle.Width - 7, 30);
                    y += 30;
                }
            }
        }


        [DefaultValue(typeof (Color), "White")]
        public override sealed Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolboxCategoryCollection Categories { get; set; }
    }
}
