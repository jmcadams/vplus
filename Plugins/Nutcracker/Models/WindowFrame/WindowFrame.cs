using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace WindowFrame {
    public partial class WindowFrame : UserControl, INutcrackerModel {
        public WindowFrame() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Window Frame"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }

        public NutcrackerNodes[,] InitializeNodes { get; private set; }
    }
}
