using System;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace Spirals {
    public partial class Spirals : UserControl, INutcrackerEffect {
        public Spirals() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirals"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var colorCount = palette.Length;
            var spiralCount = colorCount * tbPaletteRepeat.Value;
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var deltaStrands = bufferWidth / spiralCount;
            var spiralThickness = (deltaStrands * tbThickness.Value / 100) + 1;
            long spiralState = eventToRender * tbDirection.Value;

            for (var spiral = 0; spiral < spiralCount; spiral++) {
                var strandBase = spiral * deltaStrands;
                var color = palette[spiral % colorCount];
                for (var thick = 0; thick < spiralThickness; thick++) {
                    var strand = (strandBase + thick) % bufferWidth;
                    for (var row = 0; row < bufferHeight; row++) {
                        var column = (strand + ((int)spiralState / 10) + (row * tbRotations.Value / bufferHeight)) % bufferWidth;
                        if (column < 0) {
                            column += bufferWidth;
                        }
                        if (chkBoxBlend.Checked) {
                            color = HSVUtils.GetMultiColorBlend((bufferHeight - row - 1) / (double)bufferHeight, false, palette);
                        }
                        if (chkBox3D.Checked) {
                            var hsv = HSVUtils.ColorToHSV(color);
                            if (tbRotations.Value < 0) {
                                hsv.Value = (float) ((double) (thick + 1) / spiralThickness);
                            }
                            else {
                                hsv.Value = (float) ((double) (spiralThickness - thick) / spiralThickness);
                            }
                            color = HSVUtils.HSVtoColor(hsv);
                        }
                        buffer[row, column] = color;
                    }
                }
            }
            return buffer;
        }

        private void Spirals_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
