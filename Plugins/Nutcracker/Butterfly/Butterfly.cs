using System;
using System.Drawing;
using System.Windows.Forms;
using VixenPlus;

using CommonUtils;

namespace Butterfly {
    public partial class Butterfly : UserControl, INutcrackerEffect {
        public Butterfly() {
            InitializeComponent();
        }
        
        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Butterfly"; }
        }

        private const double Pi2 = Math.PI * 2;


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var h = 0.0;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var hsv = new HSVUtils();
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
                        color = HSVUtils.HSVtoColor(hsv);
                    }
                    else {
                        color = HSVUtils.GetMultiColorBlend(h, false, palette);
                    }

                    buffer[row, col] = color;
                }
            }
            return buffer;
        }


        private void Butterfly_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
