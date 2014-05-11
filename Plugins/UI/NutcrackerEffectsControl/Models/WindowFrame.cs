using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using Nutcracker.Effects;
using Nutcracker.Properties;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class WindowFrame : NutcrackerModelBase {
        private bool _isLinked = true;

        private const string XmlAttrTop = "Top";
        private const string XmlAttrSide = "Sides";
        private const string XmlAttrBottom = "Bottom";
        private const string XmlAttrLink = "Linked";

        public WindowFrame() {
            InitializeComponent();
        }

        public override string EffectName {
            get { return "Window Frame"; }
        }

        public override XDocument Settings {
            get {
                return
                    new XDocument(
                        new XElement(TypeName,
                            // ReSharper disable AssignNullToNotNullAttribute
                            new XAttribute(XmlAttrType, XmlConvert.EncodeName(EffectName)),
                            // ReSharper restore AssignNullToNotNullAttribute
                            new XAttribute(XmlAttrTop, nudTopCount.Value),
                            new XAttribute(XmlAttrBottom, nudBottomCount.Value),
                            new XAttribute(XmlAttrSide, nudSideCount.Value),
                            new XAttribute(XmlAttrLink, _isLinked)
                        )
                    );
            }
            set {
                if (null == value) {
                    return;
                }

                var root = value.Element(TypeName);
                nudTopCount.Value = int.Parse(FindAttribute(root, XmlAttrTop));
                nudBottomCount.Value = int.Parse(FindAttribute(root, XmlAttrBottom));
                nudSideCount.Value = int.Parse(FindAttribute(root, XmlAttrSide));
                _isLinked = bool.Parse(FindAttribute(root, XmlAttrLink));
                pbLink.BackgroundImage = _isLinked ? Resources.Link : Resources.Unlink;
            }
        }
        //todo need to figure out why this doesnt draw right when the offset == 1
        //TODO implement rect centering;
        public override void InitializePreview(Rectangle rect) {
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
            if (_isLinked) {
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
            InitializePreview(pbPreview.Bounds);
            pbPreview.Invalidate();
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e) {
            if (Rows == 0 || Cols == 0) {
                InitializePreview(pbPreview.Bounds);
            }

            Draw(pbPreview, e.Graphics);
        }

        private void pbLink_Click(object sender, EventArgs e) {
            _isLinked = !_isLinked;
            pbLink.BackgroundImage = _isLinked ? Resources.Link : Resources.Unlink;
        }
    }
}
