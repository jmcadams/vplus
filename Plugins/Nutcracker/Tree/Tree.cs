using System;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace Tree {
    public partial class Tree : UserControl, INutcrackerEffect {
        public Tree() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Tree"; }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);

            var isTwoGarlands = tbGralands.Value == 2;
            var branches = tbBranchCount.Value;
            var pixelsPerBranch = (int) (0.5 + (double) bufferHeight / branches);
            var maxFrame = (branches + 1) * bufferWidth;
            var frame = (eventToRender / 4) % maxFrame;

            for (var y = 0; y < bufferHeight; y++) {
                for (var x = 0; x < bufferWidth; x++) {
                    var pixelsThisBranch = y % pixelsPerBranch;
                    if (pixelsThisBranch == 0) {
                        pixelsThisBranch = pixelsPerBranch;
                    }

                    var thisBranch = (y - 1) / pixelsPerBranch;
                    var row = pixelsPerBranch - pixelsThisBranch;
                    var branch = eventToRender / bufferWidth % branches;
                    var fMod = eventToRender / 4 % bufferWidth;
                    
                    var garlandPixel = (x % 6);
                    if (garlandPixel == 0) {
                        garlandPixel = 6;
                    }
                    var isOdd = branch % 2 == 1;
                    var hsv = HSVUtils.ColorToHSV(palette[0]);
                    hsv.Value = (float)(1 - (1.0 * pixelsThisBranch / pixelsPerBranch) * 0.70);

                    if (thisBranch <= branch && x <= frame &&
                        (((row == 3 || (isTwoGarlands && row == 6)) && (garlandPixel == 1 || garlandPixel == 6)) || 
                         ((row == 2 || (isTwoGarlands && row == 5)) && (garlandPixel == 2 || garlandPixel == 5)) ||
                         ((row == 1 || (isTwoGarlands && row == 4)) && (garlandPixel == 3 || garlandPixel == 4)))) {
                        if ((!isOdd && x <= fMod) || (isOdd && bufferWidth - x + 1 <= fMod)) {
                            hsv.Hue = (float)(thisBranch % 5 / 4.0);
                            hsv.Saturation = 1.0f;
                            hsv.Value = 1.0f;
                        }
                    }
                    buffer[y, x] = HSVUtils.HSVtoColor(hsv);
                }
            }
            return buffer;
        }


        private void Tree_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
