using System.Windows.Forms;

using VixenPlus;

namespace Garlands {
    public partial class Garlands : UserControl, INutcrackerEffect {
        public Garlands() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Garlands"; }
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
