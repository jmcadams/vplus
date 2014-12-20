using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VixenPlus.Annotations;

using VixenPlusCommon;

namespace Nutcracker.Effects {
    [UsedImplicitly]
    public partial class ColorWash : UserControl, INutcrackerEffect {

        private const string ColorwashCount = "ID_SLIDER_ColorWash{0}_Count";
        private const string ColorwashHorzFade = "ID_CHECKBOX_ColorWash{0}_HFade";
        private const string ColorwashVertFade = "ID_CHECKBOX_ColorWash{0}_VFade";

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

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string> {
                ColorwashCount + "=" + tbCount.Value,
                ColorwashHorzFade + "=" + (chkBoxHFade.Checked ? "1" : "0"),
                ColorwashVertFade + "=" + (chkBoxVFade.Checked ? "1" : "0")
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var colorwashCount = string.Format(ColorwashCount, effectNum);
            var colorwashHorzFade = string.Format(ColorwashHorzFade, effectNum);
            var colorwashVertFade = string.Format(ColorwashVertFade, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split('='))) {
                if (keyValue[0].Equals(colorwashCount)) {
                    tbCount.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(colorwashHorzFade)) {
                    chkBoxHFade.Checked = keyValue[1].Equals("1");
                }
                else if (keyValue[0].Equals(colorwashVertFade)) {
                    chkBoxVFade.Checked = keyValue[1].Equals("1");
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            const int speedFactor = 200;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);

            Color color;
            var hsv2 = new HSV();
            var colorcnt = palette.Length;
            var cycleLen = colorcnt * speedFactor;
            var count = tbCount.Value;
            if (eventToRender > (colorcnt - 1) * speedFactor * count && count < 10) {
                color = palette.GetMultiColorBlend(count % 2, false);
            } else {
                color = palette.GetMultiColorBlend(eventToRender % cycleLen / (double)cycleLen, true);
            }
            var hsv = color.ToHSV();
            var halfHeight = (bufferHeight - 1) / 2.0;
            var halfWidth = (bufferWidth - 1) / 2.0;
            for (var col = 0; col < bufferWidth; col++) {
                for (var row = 0; row < bufferHeight; row++) {
                    hsv2.SetToHSV(hsv);
                    if (chkBoxHFade.Checked) hsv2.Value *= (float)(1.0 - Math.Abs(halfWidth - col) / halfWidth);
                    if (chkBoxVFade.Checked) hsv2.Value *= (float)(1.0 - Math.Abs(halfHeight - row) / halfHeight);
                    buffer[row, col] = hsv2.ToColor();
                }
            }
            return buffer;
        }

        private void ColorWash_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
