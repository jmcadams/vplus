using System.Windows.Forms;

using VixenPlus;

namespace Snowflakes {
    public partial class Snowflakes : UserControl, INutcrackerEffect {
        public Snowflakes() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Snowflakes"; }
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
