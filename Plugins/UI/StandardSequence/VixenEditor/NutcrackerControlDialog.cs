using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form
    {
        private Stopwatch _sw = new Stopwatch();

        public NutcrackerControlDialog()
        {
            InitializeComponent();
        }


        private void cbRender_CheckedChanged(object sender, EventArgs e) {
            var control = (INutcrackerEffect) nutcrackerEffectControl1.GetCurrentEffectControl();
            if (!cbRender.Checked || control == null) {
                return;
            }

            _sw.Start();
            var buffer = new Color[(int) nudRows.Value,(int) nudColumns.Value];
            while (cbRender.Checked) {
                for (var i = 0; i < nudFrames.Value; i++) {
                    _sw.Start();
                    //control.RenderEffect(buffer, new[] { Color.Red, Color.Green, Color.Blue }, i);
                    Render(control.RenderEffect(buffer, new[] { Color.Red, Color.Green, Color.Blue }, i));
                    //Thread.Sleep(20 - (int) (_sw.ElapsedMilliseconds % 20));
                    tbInfo.Text = _sw.ElapsedMilliseconds.ToString();
                    _sw.Reset();
                    Application.DoEvents();
                }
            }
            _sw.Reset();
        }


        private void Render(Color[,] buffer) {
            var rows = buffer.GetLength(Utils.IndexRowsOrHeight);
            var cols = buffer.GetLength(Utils.IndexColsOrWidth);
            using (var g = pbPreview.CreateGraphics()) {
                //g.Clear(Color.Black);
                var bitmap = new Bitmap(rows, cols, g);
                for (var row = 0; row < rows; row++) {
                    for (var column = 0; column < cols; column++) {
                        bitmap.SetPixel(rows - 1 - row, column, buffer[row, column]);
                    }
                }
                g.DrawImage(bitmap, new Point(10, 10));
            }
        }

        private void nudRows_ValueChanged(object sender, EventArgs e) {
            UpdateRender();
        }

        private void nudColumns_ValueChanged(object sender, EventArgs e) {
            UpdateRender();
        }

        private void nudFrames_ValueChanged(object sender, EventArgs e) {
            UpdateRender();
        }

        private void UpdateRender() {
            cbRender.Enabled = nudRows.Value != 0 && nudColumns.Value != 0 && nudFrames.Value != 0;
        }
    }
}
