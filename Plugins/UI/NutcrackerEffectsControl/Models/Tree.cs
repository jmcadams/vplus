using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class Tree : NutcrackerModelBase {
        public Tree() {
            InitializeComponent();
        }


        public override string EffectName {
            get { return "Tree"; }
        }


        public override XDocument Settings {
            get {
                return new XDocument(
                    new XElement(TypeName,
                        new XElement(EffectName,
                            new XAttribute("Strings", nudStringCount.Value),
                            new XAttribute("Nodes", nudNodeCount.Value),
                            new XAttribute("Strands", nudStrandCount.Value),
                            new XAttribute("Degrees", Degrees))
                    )
                );
            }
        }

        private int Degrees {
            get { return rb360.Checked ? 360 : rb270.Checked ? 270 : rb180.Checked ? 180 : 90; }
        }


        private void InitializePreview(Rectangle previewRect) {
            InitMatrix();
            if (Cols < 2 || Rows < 1) return;
            var factor = previewRect.Height / Rows;
            var renderWi = previewRect.Width / 2;
            var radians = 2.0 * Math.PI * Degrees / 360.0;
            var radius = renderWi * 0.8;
            var startAngle = -radians / 2.0;
            var angleIncr = radians / (Cols - 1);
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    var angle = startAngle + Nodes[Rows - 1 - row, col].BufX * angleIncr;
                    var x0 = radius * Math.Sin(angle);
                    var x = (int) Math.Floor(x0 * (1.0 - (double) (Nodes[row, col].BufY) / Rows) + 0.5) + renderWi;
                    var y = Nodes[Rows - 1 - row, col].BufY * factor;
                    Nodes[Rows - 1 - row, col].Model = new Point(x, y);
                }
            }
        }


        private void InitMatrix() {
            var strandsPerString = (int) nudStrandCount.Value;

            var numStrands = Cols;
            var pixelsPerStrand = Rows;
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % Rows;
                    var x = index / Rows;
                    Nodes[y, x].BufX = IsLtoR ? strand : numStrands - strand - 1;
                    Nodes[y, x].BufY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    index++;
                }
            }
        }


        private void control_ValueChanged(object sender, EventArgs e) {
            if (sender is NumericUpDown || Rows == 0 || Cols == 0) {
                ResetNodes();
            }
            var rect = pbPreview.DisplayRectangle;
            rect.Offset(-XyOffset * 2, -XyOffset * 2);
            InitializePreview(rect);

            pbPreview.Invalidate();
        }


        internal override void ResetNodes() {
            Rows = (int) nudNodeCount.Value / (int) nudStrandCount.Value;
            Cols = (int) nudStringCount.Value * (int) nudStrandCount.Value;
            base.ResetNodes();
        }


        private void pbPreview_Paint(object sender, PaintEventArgs e) {
            if (Rows == 0 || Cols == 0) {
                ResetNodes();
                var rect = pbPreview.DisplayRectangle;
                rect.Offset(-XyOffset * 2, -XyOffset * 2);
                InitializePreview(rect);
            }

            Draw(pbPreview, e.Graphics);
        }
    }
}
