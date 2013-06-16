using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

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
            if (!cbRender.Checked) {
                return;
            }

            _sw.Start();
            while (cbRender.Checked) {
                for (var i = 0; i < nudFrames.Value; i++) {
                    var buffer = new Color[(int) nudRows.Value,(int) nudColumns.Value];
                    var control = (INutcrackerEffect) nutcrackerEffectControl1.GetCurrentEffectControl();
                    Render(control.RenderEffect(buffer, new[] {Color.Red, Color.Green, Color.Blue}, i));
                }
                Application.DoEvents();
                Thread.Sleep(20 - (int)(_sw.ElapsedMilliseconds % 20));
            }
            _sw.Stop();
            _sw.Reset();
        }

        private void Render(Color[,] buffer) {
            var rows = buffer.GetLength(0);
            var cols = buffer.GetLength(1);
            using (var g = pbPreview.CreateGraphics()) {
                var bm = new Bitmap(rows, cols, g);
                for (var c = 0; c < cols; c++) {
                    for (var r = 0; r < rows; r++) {
                        bm.SetPixel(r,c,buffer[r,c]);
                    }
                }
                g.DrawImage(bm, new Point(10,10));
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
