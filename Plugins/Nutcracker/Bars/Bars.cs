using System.Windows.Forms;

using VixenPlus;

namespace NutcrackerEffects {
    public partial class Bars : UserControl, INutcrackerEffect {
        public Bars() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Bars"; }
        }

        public byte[] EffectData { get; private set; }


        public void Startup() {
            throw new System.NotImplementedException();
        }


        public void ShutDown() {
            throw new System.NotImplementedException();
        }
    }
}
