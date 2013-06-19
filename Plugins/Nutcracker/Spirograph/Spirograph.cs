using System;
using System.Drawing;
using System.Windows.Forms;
using CommonUtils;
using VixenPlus;

namespace Spirograph {
    public partial class Spirograph : UserControl, INutcrackerEffect {
        public Spirograph() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirograph"; }
        }

        public string Notes {
            get { throw new NotImplementedException(); }
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
            OnControlChanged(this, new EventArgs());
        }
    }
}
