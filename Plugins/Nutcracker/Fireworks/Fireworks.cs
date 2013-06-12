using System.Windows.Forms;

using VixenPlus;

namespace Fireworks {
    public partial class Fireworks : UserControl, INutcrackerEffect {
        public Fireworks() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Fireworks"; }
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
