using System;
using System.Drawing;
using System.Windows.Forms;

using Nutcracker.Effects;
using Nutcracker.Properties;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class WindowFrame : NutcrackerModelBase {
        public WindowFrame() {
            InitializeComponent();
        }

        public override string EffectName {
            get { return "Window Frame"; }
        }

        private void InitFrame() {
            var top = (int)nudTopCount.Value;
            var sides = (int)nudSideCount.Value;
            var bottom = (int)nudBottomCount.Value;

            Cols = Math.Max(top, bottom) + 2;
            Rows = sides;
            Nodes = new NutcrackerNodes[Rows, Cols];
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    Nodes[row, col] = new NutcrackerNodes();
                }
            }

            int x,y;
            //SetNodeCount(top+2*sides+bottom);
            if (top >= bottom) {
                // first node is bottom left and we count up the left side, across the top, down the right and then across the bottom
                var frameWidth = top + 2; // allow for left/right columns

                // up side 1
                x = IsLtoR ? 0 : frameWidth - 1;
                for (y = 0; y < sides; y++) {
                    Nodes[y, x].Model = new Point(x, y);
                }
                // across top
                y = sides - 1;
                for (x = 0; x < top; x++) {
                    Nodes[y, x].Model = new Point(IsLtoR ? x + 1 : top - x, y);
                }
                // down side 2
                x = IsLtoR ? frameWidth - 1 : 0;
                for (y = sides - 1; y >= 0; y--) {
                    Nodes[y, x].Model = new Point(x, y);
                }
                // across bottom
                y = 0;
                for (x = 0; x < bottom; x++) {
                    Nodes[y, x].Model = new Point(IsLtoR ? top - x : x + 1, y);
                }
            }
            else {
                // first node is top left and we count down the left side, across the bottom, up the right and then across the top
                var frameWidth = bottom + 2;

                // down side 1
                x = IsLtoR ? 0 : frameWidth - 1;
                for (y = sides - 1; y >= 0; y--) {
                    Nodes[y, x].Model = new Point(x, y);
                }
                // across bottom
                y = 0;
                for (x = 0; x < bottom; x++) {
                    Nodes[y, x].Model = new Point(IsLtoR ? x + 1 : bottom - x, y);
                }
                // up side 2
                x = IsLtoR ? frameWidth - 1 : 0;
                for (y = 0; y < sides; y++) {
                    Nodes[y, x].Model = new Point(x, y);
                }
                // across top
                y = sides - 1;
                for (x = 0; x < top; x++) {
                    Nodes[y, x].Model = new Point(IsLtoR ? bottom - x : x + 1, y);
                }
            }
        }

        private void control_ValueChanged(object sender, EventArgs e) {
            if (pbLink.BackgroundImage == Resources.Link) {
                var c = sender as NumericUpDown;
                if (c != null && (c.Name == nudTopCount.Name || c.Name == nudBottomCount.Name)) {
                    if (c.Name == nudTopCount.Name) {
                        nudBottomCount.Value = nudTopCount.Value;
                    }
                    else {
                        nudTopCount.Value = nudBottomCount.Value;
                    }
                }
            }
            InitFrame();
            pbPreview.Invalidate();
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e) {
            if (Rows == 0 || Cols == 0) {
                InitFrame();
            }

            Draw(pbPreview, e.Graphics);
        }

        private void pbLink_Click(object sender, EventArgs e) {
            pbLink.BackgroundImage = pbLink.BackgroundImage == Resources.Unlink ? Resources.Link : Resources.Unlink;
        }
    }
}
