using System.Windows.Forms;

using VixenPlus;

namespace Snowstorm {
    public partial class Snowstorm : UserControl, INutcrackerEffect {
        public Snowstorm() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Snow Storm"; }
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
