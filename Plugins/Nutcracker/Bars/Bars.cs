using System.Windows.Forms;

using VixenPlus;

namespace Bars {
    public partial class Bars : UserControl, INutcrackerEffect {
        public Bars() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Bars"; }
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
