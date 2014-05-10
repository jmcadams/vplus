using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class Arch : NutcrackerModelBase {
        public Arch() {
            InitializeComponent();
        }

        public override string EffectName {
            get { return "Arch"; }
        }

        public override string Notes {
            get { return "Define up to 10 arches"; }
        }

        private void control_ValueChanged(object sender, EventArgs e) {
            ResetNodes();
            InitializePreview(pbPreview.Bounds);
            pbPreview.Invalidate();
        }

        internal override void ResetNodes() {
            Rows = (int)nudNodeCount.Value; 
            Cols = (int)nudArchCount.Value; 
            base.ResetNodes();
        }

        private void InitializePreview(Rectangle previewRect) {
            var radius = ((previewRect.Size.Width / 2) - (XyOffset * 2)) / Cols;
            const double twoPi = Math.PI * 2;
            var angleStep = 0.5d / (Rows - 1);

            var centerX = radius;
            var centerY = previewRect.Size.Height - (radius / 2) + XyOffset;

            for (var c = 0; c < Cols; c++) {
                for (var ns = 0; ns < Rows; ns++) {
                    var angle = 0.5d + ns * angleStep;
                    var ptX = centerX + (int) Math.Floor(Math.Cos(angle * twoPi) * radius);
                    var ptY = centerY + (int) Math.Floor(Math.Sin(angle * twoPi) * radius);
                    Nodes[ns, c].Model = new Point(ptX, ptY);
                }
                centerX += radius * 2;
            }
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e) {
            if (Rows == 0 || Cols == 0) {
                ResetNodes();
                InitializePreview(pbPreview.Bounds);
            }

            Draw(pbPreview, e.Graphics);
        }
    }
}
