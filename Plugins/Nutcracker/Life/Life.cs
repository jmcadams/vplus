using System.Windows.Forms;

using VixenPlus;

namespace Life {
    public partial class Life : UserControl, INutcrackerEffect {
        public Life() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Life"; }
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
