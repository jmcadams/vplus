using System.Windows.Forms;

using VixenPlus;


namespace Meteors {
    public partial class Meteors : UserControl, INutcrackerEffect {
        public Meteors() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Meteors"; }
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
