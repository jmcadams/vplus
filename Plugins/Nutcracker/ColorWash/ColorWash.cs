using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;
using VixenPlus;

namespace ColorWash {
    public partial class ColorWash : UserControl, INutcrackerEffect {
        public ColorWash() {
            InitializeComponent();
        }
        
        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Color Wash"; }
        }

        public string Notes {
            get { return String.Empty; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public XmlElement Settings {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            const int speedFactor = 200;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);

            Color color;
            var hsv2 = new HSVUtils();
            var colorcnt = palette.Length;
            var cycleLen = colorcnt * speedFactor;
            var count = tbCount.Value;
            if (eventToRender > (colorcnt - 1) * speedFactor * count && count < 10) {
                color = HSVUtils.GetMultiColorBlend(count % 2, false, palette);
            } else {
                color = HSVUtils.GetMultiColorBlend(eventToRender % cycleLen / (double)cycleLen, true, palette);
            }
            var hsv = HSVUtils.ColorToHSV(color);
            var halfHeight = (bufferHeight - 1) / 2.0;
            var halfWidth = (bufferWidth - 1) / 2.0;
            for (var col = 0; col < bufferWidth; col++) {
                for (var row = 0; row < bufferHeight; row++) {
                    hsv2.SetToHSV(hsv);
                    if (chkBoxHFade.Checked) hsv2.Value *= (float)(1.0 - Math.Abs(halfWidth - col) / halfWidth);
                    if (chkBoxVFade.Checked) hsv2.Value *= (float)(1.0 - Math.Abs(halfHeight - row) / halfHeight);
                    buffer[row, col] = HSVUtils.HSVtoColor(hsv2);
                }
            }
            return buffer;
        }

        private void ColorWash_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
