using System;
using System.Drawing;
using System.Windows.Forms;
using CommonUtils;
using VixenPlus;

//TODO: Add steps and strobe effects from NC 3.0.14
namespace Twinkle {
    public partial class Twinkle : UserControl, INutcrackerEffect {
        public Twinkle() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Twinkle"; }
        }

        public string Notes {
            get { throw new NotImplementedException(); }
        }

        private readonly Random _random = new Random(2271965);


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);

            var lights = Convert.ToInt32((bufferHeight * bufferWidth) * (tbLightCount.Value / 100.0));
            var step = (bufferHeight * bufferWidth) / lights;
            if (step < 1) {
                step = 1;
            }
            var i = 0;

            for (var y = 0; y < bufferHeight; y++) {
                for (var x = 0; x < bufferWidth; x++) {
                    i++;
                    if (i % step != 0) {
                        continue;
                    }

                    var hsv = HSVUtils.ColorToHSV(palette[_random.Next() % palette.Length]);
                    switch ((eventToRender / 4 + _random.Next()) % 9) {
                        case 8:
                        case 0:
                            hsv.Value = 0.1f;
                            break;
                        case 7:
                        case 1:
                            hsv.Value = 0.3f;
                            break;
                        case 6:
                        case 2:
                            hsv.Value = 0.5f;
                            break;
                        case 5:
                        case 3:
                            hsv.Value = 0.7f;
                            break;
                        case 4:
                            hsv.Value = 1.0f;
                            break;
                    }
                    buffer[y, x] = HSVUtils.HSVtoColor(hsv);
                }
            }
            return buffer;
        }


        private void Twinkle_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
