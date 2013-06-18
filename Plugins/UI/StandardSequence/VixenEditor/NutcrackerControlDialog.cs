using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form
    {
        private readonly Stopwatch _sw = new Stopwatch();

        public NutcrackerControlDialog()
        {
            InitializeComponent();
        }


        private void cbRender_CheckedChanged(object sender, EventArgs e) {
            var control = (INutcrackerEffect) nutcrackerEffectControl1.GetCurrentEffectControl();
            if (!cbRender.Checked || control == null) {
                return;
            }

            var rows = nudRows.Value;
            var cols = nudColumns.Value;
            var buffer = new Color[(int) nudRows.Value,(int) nudColumns.Value];

            var i = 0;
            while (cbRender.Checked) {
                _sw.Start();
                for (var row = 0; row < rows; row++) {
                    for (var col = 0; col < cols; col++) {
                        buffer[row, col] = Color.Transparent;
                    }
                }
                Render(control.RenderEffect(buffer, nutcrackerEffectControl1.GetPalette(), i));
                tbInfo.Text = _sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                if (_sw.ElapsedMilliseconds < 50) Thread.Sleep(50 - (int)(_sw.ElapsedMilliseconds % 50));
                _sw.Reset();
                Application.DoEvents();
                i += nutcrackerEffectControl1.GetSpeed();
            }
            _sw.Reset();
        }


        private void Render(Color[,] buffer) {
            var rows = buffer.GetLength(Utils.IndexRowsOrHeight);
            var cols = buffer.GetLength(Utils.IndexColsOrWidth);
            using (var g = pbPreview.CreateGraphics()) {
                // Bitmap is width (col) then height (row), we pass data like Vixen+, hight (row) then width (col)
                var bitmap = new Bitmap(cols, rows, g);
                for (var row = 0; row < rows; row++) {
                    for (var column = 0; column < cols; column++) {
                        var color = buffer[row, column];
                        bitmap.SetPixel(column, rows - 1 - row, color == Color.Transparent ? Color.Black : color);
                    }
                }
                g.DrawImage(bitmap, new Point(10, 10));
            }
        }

        private void nudRows_ValueChanged(object sender, EventArgs e) {
        }

        private void nudColumns_ValueChanged(object sender, EventArgs e) {
        }

        private void nudFrames_ValueChanged(object sender, EventArgs e) {
        }
    }
}
