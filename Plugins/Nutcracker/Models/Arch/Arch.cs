using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Arch {
    public partial class Arch : UserControl, INutcrackerModel {
        public Arch() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Arch"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }


        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            throw new System.NotImplementedException();
        }


        public bool IsLtoR { get; set; }

    }
}
