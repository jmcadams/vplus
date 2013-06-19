using System;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace Bars {
    public partial class Bars : UserControl, INutcrackerEffect {
        public Bars() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Bars"; }
        }

        public string Notes {
            get { throw new NotImplementedException(); }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var barCount = tbRepeat.Value * palette.Length;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var barHeight = bufferHeight / barCount + 1;
            var halfHeight = bufferHeight / 2;
            var blockHeight = palette.Length * barHeight;
            var rowOffset = eventToRender / 4 % blockHeight;
            for (var row = 0; row < bufferHeight; row++) {
                var isMovingDown = false; // default to UP even if undefined
                switch (cbDirection.SelectedIndex) {
                    case 1: // DOWN
                        isMovingDown = true;
                        break;
                    case 2: // EXPAND
                        isMovingDown = (row <= halfHeight);
                        break;
                    case 3: // COMPRESS
                        isMovingDown = (row > halfHeight);
                        break;
                }
                var n = isMovingDown ? row + rowOffset : row - rowOffset + blockHeight;

                var hsv = HSVUtils.ColorToHSV(palette[(n % blockHeight) / barHeight]);
                if (cbHighlight.Checked && (isMovingDown ? n % barHeight == 0 : n % barHeight == barHeight - 1)) {
                    hsv.Saturation = 0f;
                }
                if (cb3D.Checked) {
                    hsv.Value *= (float) (isMovingDown ? barHeight - n % barHeight - 1 : n % barHeight) / barHeight;
                }
                for (var column = 0; column < bufferWidth; column++) {
                    buffer[row, column] = HSVUtils.HSVtoColor(hsv);
                }
            }
            return buffer;
        }


        private void Bars_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}