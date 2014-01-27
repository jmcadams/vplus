using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;

namespace Twinkle {
    public partial class Twinkle : UserControl, INutcrackerEffect {

        private const string TwinkleCount = "ID_SLIDER_Twinkle{0}_Count";
        private const string TwinkleSteps = "ID_SLIDER_Twinkle{0}_Steps";
        private const string TwinkleStrobe = "ID_CHECKBOX_Twinkle{0}_Strobe";

        public Twinkle() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Twinkle"; }
        }

        public string Notes {
            get { return "Does not use speed control."; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return false; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string> {
                TwinkleCount + "=" + tbLightCount.Value,
                TwinkleSteps + "=" + tbSteps.Value,
                TwinkleStrobe + "=" + (chkBoxStrobe.Checked ? "1" : "0")
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var twinkleCount = string.Format(TwinkleCount, effectNum);
            var twinkleSteps = string.Format(TwinkleSteps, effectNum);
            var twinkleStrobe = string.Format(TwinkleStrobe, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(twinkleCount)) {
                    tbLightCount.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(twinkleSteps)) {
                    tbSteps.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(twinkleStrobe)) {
                    chkBoxStrobe.Checked = keyValue[1].Equals("1");
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var random = new Random(chkBoxStrobe.Checked ? DateTime.Now.Millisecond : 2271965);
            var steps = tbSteps.Value;
            var stepsHalf = steps / 2.0f;
            var lights = Convert.ToInt32((bufferHeight * bufferWidth) * (tbLightCount.Value / 100.0));
            var step = Math.Max(1, (bufferHeight * bufferWidth) / lights);

            for (var y = 0; y < bufferHeight; y++) {
                for (var x = 0; x < bufferWidth; x++) {
                    if ((y * bufferHeight + x + 1) % step != 1 && step != 1) {
                        continue;
                    }

                    var hsv = palette[random.Next() % palette.Length].ToHSV();

                    var randomStep = (eventToRender + random.Next()) % steps;

                    hsv.Value = chkBoxStrobe.Checked ? ((randomStep == (int)(stepsHalf + 0.5f)) ? 1.0f : 0.0f) :
                        Math.Max(0.0f, ((randomStep <= stepsHalf ? randomStep : steps - randomStep) / stepsHalf));

                    buffer[y, x] = hsv.ToColor();
                }
            }
            return buffer;
        }


        private void Twinkle_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
