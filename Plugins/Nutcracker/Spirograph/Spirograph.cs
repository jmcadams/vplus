using System.Windows.Forms;

using VixenPlus;

namespace Spirograph {
    public partial class Spirograph : UserControl, INutcrackerEffect {
        public Spirograph() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Spirograph"; }
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
