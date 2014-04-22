using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public class DataGridViewDisableButtonCell : DataGridViewButtonCell {
        private bool _visible;

        public new bool Visible {
            private get { return _visible; }
            set { _visible = value; }
        }

        // Override the Clone method so that the Enabled property is copied. 
        public override object Clone() {
            var cell = (DataGridViewDisableButtonCell) base.Clone();
            cell.Visible = Visible;
            return cell;
        }


        // By default, enable the button cell. 
        public DataGridViewDisableButtonCell() {
            _visible = true;
        }


        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts) {

            if (_visible) {
                // Llet the base class handle the painting. 
                base.Paint(g, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
            else {

                // Draw the cell background, if specified. 
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background) {
                    using (var cellBackground = new SolidBrush(cellStyle.BackColor)) {
                        g.FillRectangle(cellBackground, cellBounds);
                    }
                }

                // Draw the cell borders, if specified. 
                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border) {
                    PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                }

                // Calculate the area in which to draw the cell.
                var cellArea = cellBounds;
                var borderWidths = BorderWidths(advancedBorderStyle);
                cellArea.X += borderWidths.X;
                cellArea.Y += borderWidths.Y;
                cellArea.Height -= borderWidths.Height;
                cellArea.Width -= borderWidths.Width;


                // Draw the disabled button text.  
                var s = FormattedValue as string;
                if (s != null) {
                    TextRenderer.DrawText(g, s, DataGridView.Font, cellArea, SystemColors.GrayText);
                }
            }
        }
    }
}