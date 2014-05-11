using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class Matrix : NutcrackerModelBase {
        private const string XmlAttrVertical = "Vertical";

        public Matrix() {
            InitializeComponent();
        }

        public override string EffectName {
            get { return "Matrix"; }
        }

        public override XDocument Settings {
            get {
                return
                    new XDocument(
                        new XElement(TypeName, 
                            new XAttribute(XmlAttrType, EffectName), 
                            new XAttribute(XmlAttrStrings, nudStringCount.Value),
                            new XAttribute(XmlAttrNodes, nudNodeCount.Value), 
                            new XAttribute(XmlAttrStrands, nudStrandCount.Value),
                            new XAttribute(XmlAttrVertical, rbVertical.Checked)));
            }
            set {
                if (null == value) {
                    return;
                }

                var root = value.Element(TypeName);
                nudStringCount.Value = int.Parse(FindAttribute(root, XmlAttrStrings));
                nudNodeCount.Value = int.Parse(FindAttribute(root, XmlAttrNodes));
                nudStrandCount.Value = int.Parse(FindAttribute(root, XmlAttrStrands));

                if (bool.Parse(FindAttribute(root, XmlAttrVertical))) {
                    rbVertical.Checked = true;
                }
                else {
                    rbHorizontal.Checked = true;
                }
            }
        }


        public override void InitializePreview(Rectangle rect) {
            var strandsPerString = (int)nudStrandCount.Value;

            var numStrands = Cols;
            var pixelsPerStrand = Rows;
            var xFactor = (rect.Width - (XyOffset * 2)) / Cols;
            var yFactor = (rect.Height - (XyOffset * 2)) / Rows;
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % Rows;
                    var x = index / Rows;
                    var ptX = IsLtoR ? strand : numStrands - strand - 1;
                    var ptY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    Nodes[y, x].Model = new Point(ptX * xFactor, ptY * yFactor );
                    index++;
                }
            }
        }


        private void control_ValueChanged(object sender, EventArgs e) {
            ResetNodes();
            InitializePreview(pbPreview.Bounds);
            pbPreview.Invalidate();
        }


        internal override void ResetNodes() {
            var nodesPerString = (int) nudNodeCount.Value / (int) nudStrandCount.Value;
            var totalStringCount = (int) nudStringCount.Value * (int) nudStrandCount.Value;
            Rows = rbVertical.Checked ? nodesPerString : totalStringCount;
            Cols = rbVertical.Checked ? totalStringCount : nodesPerString;

            base.ResetNodes();
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
