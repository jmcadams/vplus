using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Tree {
    public partial class Tree : UserControl, INutcrackerModel {
        public Tree() {
            InitializeComponent();
        }
        
        public string EffectName {
            get { return "Tree"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }
        
        public NutcrackerNodes[,] InitializeNodes { get; private set; }
    }
}
