using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Controllers.E131.Controls {
    //-------------------------------------------------------------
    //
    //	DataGridViewNumbered - a class that automatically adds row numbers
    //
    //		based on example by:
    //
    //			AUTHOR: Daniel S. Soper
    //			URL: http://www.danielsoper.com
    //			DATE: 20 February 2007
    //			LICENSE: Public Domain. Enjoy!   :-)
    //
    //-------------------------------------------------------------

    public class DataGridViewNumbered : DataGridView {
        private const int WidthAdjustment = 20;
        private const int RowXOffset = 15;

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e) {
            //this method overrides the DataGridView's RowPostPaint event 
            //in order to automatically draw numbers on the row header cells
            //and to automatically adjust the width of the column containing
            //the row header cells so that it can accommodate the new row
            //numbers.

            //store a string representation of the row number in 'strRowNumber'
            var rowNumber = (e.RowIndex + 1).ToString(CultureInfo.InvariantCulture);

            //prepend leading zeros to the string if necessary to improve
            //appearance. For example, if there are ten rows in the grid,
            //row seven will be numbered as "07" instead of "7". Similarly, if 
            //there are 100 rows in the grid, row seven will be numbered as "007".
            while (rowNumber.Length < RowCount.ToString(CultureInfo.InvariantCulture).Length) {
                rowNumber = "0" + rowNumber;
            }

            //determine the display size of the row number string using
            //the DataGridView's current font.
            var rowNumberSize = e.Graphics.MeasureString(rowNumber, Font);

            //adjust the width of the column that contains the row header cells 
            //if necessary
            if (RowHeadersWidth < (int) (rowNumberSize.Width + WidthAdjustment)) {
                RowHeadersWidth = (int) (rowNumberSize.Width + WidthAdjustment);
            }

            //draw the row number string on the current row header cell using
            //the brush defined above and the DataGridView's default font
            e.Graphics.DrawString(rowNumber, Font, SystemBrushes.ControlText, e.RowBounds.Location.X + RowXOffset,
                e.RowBounds.Location.Y + ((e.RowBounds.Height - rowNumberSize.Height)/2));

            //call the base object's OnRowPostPaint method
            base.OnRowPostPaint(e);
        }
    }
}