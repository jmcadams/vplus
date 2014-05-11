using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using Nutcracker.Effects;

namespace Nutcracker.Models {
    public class NutcrackerModelBase : UserControl {
        public const string TypeName = "NutcrackerModelBase";
        protected const int XyOffset = 3;

        protected const string XmlAttrType = "Type";
        protected const string XmlAttrNodes = "Nodes";
        protected const string XmlAttrStrings = "Strings";
        protected const string XmlAttrStrands = "Strands";

        public virtual string EffectName {
            get { return string.Empty; }
        }

        public virtual string Notes { get { return string.Empty; } }

        public virtual XDocument Settings {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public virtual void InitializePreview(Rectangle r) {
            throw new NotImplementedException();
        }

        internal static string FindAttribute(XElement e, string attribute) {
            var xAttribute = e.Attribute(attribute);

            if (xAttribute != null) {
                return xAttribute.Value;
            }

            throw new XmlException(string.Format("Missing {0} attribute", attribute));
        }


        public bool IsLtoR { protected get; set; }

        internal int Rows { get; set; }
        internal int Cols { get; set; }
        internal NutcrackerNodes[,] Nodes;

        internal virtual void ResetNodes() {
            Nodes = new NutcrackerNodes[Rows, Cols];
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    Nodes[row, col] = new NutcrackerNodes();
                }
            }
        }


        internal void Draw(Control c, Graphics g) {
            var b = new Bitmap(c.Width, c.Height, g);
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    b.SetPixel(Nodes[row, col].Model.X, Nodes[row, col].Model.Y, Color.White);
                }
            }
            g.DrawImage(b, XyOffset, XyOffset);
        }
    }
}