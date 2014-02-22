using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Common;

using CommonControls;

using VixenPlus;

namespace Spirograph {
    public partial class Spirograph : UserControl, INutcrackerEffect {

        private const string SpirographOuterR = "ID_SLIDER_Spirograph{0}_R";
        private const string SpirographInnerR = "ID_SLIDER_Spirograph{0}_r";
        private const string SpirographDiameter = "ID_SLIDER_Spirograph{0}_d";
        private const string SpirographAnimate = "ID_CHECKBOX_Spirograph{0}_Animate";

        public Spirograph() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirograph"; }
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
                SpirographAnimate + "=" + (chkBoxAnimate.Checked ? "1" : "0"),
                SpirographDiameter + "=" + tbDistance.Value,
                SpirographInnerR + "=" + tbInnerR.Value,
                SpirographOuterR + "=" + tbOuterR.Value
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var spirographAnimate = string.Format(SpirographAnimate, effectNum);
            var spirographDiameter = string.Format(SpirographDiameter, effectNum);
            var spirographInnerR = string.Format(SpirographInnerR, effectNum);
            var spirographOuterR = string.Format(SpirographOuterR, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(spirographAnimate)) {
                    chkBoxAnimate.Checked = keyValue[1].Equals("1");
                }
                else if (keyValue[0].Equals(spirographDiameter)) {
                    tbDistance.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(spirographInnerR)) {
                    tbInnerR.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(spirographOuterR)) {
                    tbOuterR.Value = keyValue[1].ToInt();
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var colorcnt = palette.Length;

            var halfWidth = bufferWidth / 2;
            var halfHeight = bufferHeight / 2;
            var outterR = (float) (halfWidth * (tbOuterR.Value / 100.0));
            var innerR = (float) (halfWidth * (tbInnerR.Value / 100.0));
            if (innerR > outterR) innerR = outterR;
            var distance = (float) (halfWidth * (tbDistance.Value / 100.0));

            var mod1440 = eventToRender % 1440;
            var originalDistance = distance;
            for (var i = 1; i <= 360; i++) {
                if (chkBoxAnimate.Checked) distance = (int) (originalDistance + eventToRender / 2.0) % 100;
                var t = (float) ((i + mod1440) * Math.PI / 180);
                var x = Convert.ToInt32((outterR - innerR) * Math.Cos(t) + distance * Math.Cos(((outterR - innerR) / innerR) * t) + halfWidth);
                var y = Convert.ToInt32((outterR - innerR) * Math.Sin(t) + distance * Math.Sin(((outterR - innerR) / innerR) * t) + halfHeight);
                var x2 = Math.Pow((x - halfWidth), 2);
                var y2 = Math.Pow((y - halfHeight), 2);
                var hyp = (Math.Sqrt(x2 + y2) / bufferWidth) * 100.0;

                if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight) {
                    buffer[y, x] = palette[(int)(hyp / (colorcnt > 0 ? bufferWidth / colorcnt : 1)) % colorcnt];
                }
            }
            return buffer;
        }


        private void Spirograph_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
