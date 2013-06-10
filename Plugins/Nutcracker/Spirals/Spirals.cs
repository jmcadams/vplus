using System.Windows.Forms;

using VixenPlus;

namespace Spirals {
    public partial class Spirals : UserControl, INutcrackerEffect {
        public Spirals() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Spirals"; }
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
