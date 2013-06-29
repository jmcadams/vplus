using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Matrix2 {
    public partial class Matrix : UserControl, INutcrackerModel {
        public Matrix() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Matrix"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }

        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            throw new System.NotImplementedException();
        }


        public bool SetDirection {
            set { throw new System.NotImplementedException(); }
        }
    }
}
