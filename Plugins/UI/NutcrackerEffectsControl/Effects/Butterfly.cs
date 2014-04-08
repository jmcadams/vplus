using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VixenPlus.Annotations;

using VixenPlusCommon;

namespace Nutcracker.Effects {
    [UsedImplicitly]
    public partial class Butterfly : UserControl, INutcrackerEffect {

        private readonly bool _initializing = true;
        private const string ButterflyChunks = "ID_SLIDER_Butterfly{0}_Chunks";
        private const string ButterflyPalette = "ID_CHOICE_Butterfly{0}_Colors";
        private const string ButterflySkip = "ID_SLIDER_Butterfly{0}_Skip";
        private const string ButterflyStyle = "ID_SLIDER_Butterfly{0}_Style";
        private const double Pi2 = Math.PI * 2;

        public Butterfly() {
            InitializeComponent();
            cbColors.SelectedIndex = 1;
            _initializing = false;
        }
        
        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Butterfly"; }
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
                ButterflyChunks + "=" + tbChunks.Value,
                ButterflyPalette + "=" + cbColors.SelectedItem,
                ButterflySkip + "=" + (tbSkip.Value),
                ButterflyStyle + "=" + (tbStyle.Value)
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var butterflyChunks = string.Format(ButterflyChunks, effectNum);
            var butterflyPalette = string.Format(ButterflyPalette, effectNum);
            var butterflySkip = string.Format(ButterflySkip, effectNum);
            var butterflyStyle = string.Format(ButterflyStyle, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(butterflyChunks)) {
                    tbChunks.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(butterflyPalette)) {
                    var index = cbColors.Items.IndexOf(keyValue[1]);
                    if (index >= 0) {
                        cbColors.SelectedIndex = index;
                    }
                }
                else if (keyValue[0].Equals(butterflySkip)) {
                    tbSkip.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(butterflyStyle)) {
                    tbStyle.Value = keyValue[1].ToInt();
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var h = 0.0;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var hsv = new HSV();
            var maxframe = bufferHeight*2;
            var frame = (int) ((bufferHeight*(double) eventToRender/200.0)%maxframe);
            var offset = eventToRender/100.0;
            for (var col = 0; col < bufferWidth; col++) {
                for (var row = 0; row < bufferHeight; row++) {
                    double x1;
                    double y1;
                    double f;
                    switch (tbStyle.Value) {
                        case 1:
                            var n =
                                Math.Abs((col*col - row*row)*
                                         Math.Sin(offset + ((col + row)*Pi2/(bufferHeight + bufferWidth))));
                            var d = col*col + row*row + 1;
                            h = n/d;
                            break;
                        case 2:
                            f = (frame < maxframe/2) ? frame + 1 : maxframe - frame;
                            x1 = (col - bufferWidth/2.0)/f;
                            y1 = (row - bufferHeight/2.0)/f;
                            h = Math.Sqrt(x1*x1 + y1*y1);
                            break;
                        case 3:
                            f = (frame < maxframe/2) ? frame + 1 : maxframe - frame;
                            f = f*0.1 + bufferHeight/60.0;
                            x1 = (col - bufferWidth/2.0)/f;
                            y1 = (row - bufferHeight/2.0)/f;
                            h = Math.Sin(x1)*Math.Cos(y1);
                            break;

                    }
                    hsv.Saturation = 1.0f;
                    hsv.Value = 1.0f;
                    var chunks = tbChunks.Value;
                    var skip = tbSkip.Value;
                    if (chunks > 1 && ((int) (h*chunks))%skip == 0) {
                        continue;
                    }
                    Color color;
                    if (cbColors.SelectedIndex == 0) {
                        hsv.Hue = (float) h;
                        color = hsv.ToColor();
                    }
                    else {
                        color = palette.GetMultiColorBlend(h, false);
                    }

                    buffer[row, col] = color;
                }
            }
            return buffer;
        }


        private void Butterfly_ControlChanged(object sender, EventArgs e) {
            if (_initializing) return;
            OnControlChanged(this, e);
        }
    }
}
