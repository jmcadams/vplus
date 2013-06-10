using System.Windows.Forms;

using VixenPlus;

namespace Tree {
    public partial class Tree : UserControl, INutcrackerEffect {
        public Tree() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Tree"; }
        }

        public byte[] EffectData {
            get { throw new System.NotImplementedException(); }
        }

        public void Startup() {
            throw new System.NotImplementedException();
        }


        public void ShutDown() {
            throw new System.NotImplementedException();
        }
    }
}
