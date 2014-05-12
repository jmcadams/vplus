using System;
using System.Diagnostics;
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

        public override string Notes {
            get {
                return "Treated as for string, top/bottom/left/right.";
            }
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
        public override void InitializePreview(Rectangle rect) {
            var top = (int)nudTopCount.Value;
            var sides = (int)nudSideCount.Value;
            var bottom = (int)nudBottomCount.Value;

            Cols = Math.Max(Math.Max(top, bottom),sides);
            Rows = 4;
            Nodes = new NutcrackerNodes[Rows, Cols];
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    Nodes[row, col] = new NutcrackerNodes() {Model = new Point(-1,-1)};
                }
            }

            var spacing = Math.Min(rect.Width, rect.Height);
            var maxWidth = Math.Max(top, bottom);

            var xSpacing = spacing / (maxWidth + 2);
            var ySpacing = spacing / (sides + 2);

            var xOffset = (rect.Width - (xSpacing * (maxWidth + 3))) / 2;
            var yOffset = (rect.Height - (ySpacing * (sides + 3))) / 2;
            int x, y;

            // x = left to right
            // y = top to bottom

            // Starts at bottom left with the left side - should be the left most/bottom most pixel.
            for (y = sides - 1; y >= 0; y--) {
                Nodes[0, y].Model = new Point(xSpacing + xOffset, (y + 2) * ySpacing + yOffset);
            }
            
            // Now top from left to right
            for (x = 0; x < top; x++) {
                Nodes[1, x].Model = new Point((x + 1) * xSpacing + xOffset, ySpacing + yOffset);
            }

            // Now top right to bottom right
            x = (maxWidth + 1) * xSpacing + xOffset;
            for (y = 0; y < sides; y++) {
                Nodes[2, y].Model = new Point(x, (y + 1) * ySpacing + yOffset);
            }

            // Finally, finish along bottom
            y = (sides + 1) * ySpacing + yOffset;
            var margin = top <= bottom ? 0 : top - bottom;
            for (x = bottom - 1; x >= 0; x--) {
                Nodes[3, x].Model = new Point((x + 2 + margin) * xSpacing + xOffset, y);

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
