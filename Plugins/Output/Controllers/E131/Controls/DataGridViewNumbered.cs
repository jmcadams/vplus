using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Controllers.E131.Controls
{
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

        public class DataGridViewNumbered : DataGridView
        {
            protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
            { //this method overrides the DataGridView's RowPostPaint event 
              //in order to automatically draw numbers on the row header cells
              //and to automatically adjust the width of the column containing
              //the row header cells so that it can accommodate the new row
              //numbers.

                //store a string representation of the row number in 'strRowNumber'
                var strRowNumber = (e.RowIndex + 1).ToString(CultureInfo.InvariantCulture);

                //prepend leading zeros to the string if necessary to improve
                //appearance. For example, if there are ten rows in the grid,
                //row seven will be numbered as "07" instead of "7". Similarly, if 
                //there are 100 rows in the grid, row seven will be numbered as "007".
                while (strRowNumber.Length < RowCount.ToString(CultureInfo.InvariantCulture).Length) strRowNumber = "0" + strRowNumber;

                //determine the display size of the row number string using
                //the DataGridView's current font.
                var size = e.Graphics.MeasureString(strRowNumber, Font);
            
                //adjust the width of the column that contains the row header cells 
                //if necessary
                if (RowHeadersWidth < (int)(size.Width + 20)) RowHeadersWidth = (int)(size.Width + 20);

                //this brush will be used to draw the row number string on the
                //row header cell using the system's current ControlText color
                var b = SystemBrushes.ControlText;

                //draw the row number string on the current row header cell using
                //the brush defined above and the DataGridView's default font
                e.Graphics.DrawString(strRowNumber, Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));

                //call the base object's OnRowPostPaint method
                base.OnRowPostPaint(e);
            }
        }
}
