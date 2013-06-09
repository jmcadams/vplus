using System.Windows.Forms;
using VixenPlus;

namespace Butterfly {
    public partial class Butterfly : UserControl, INutcrackerEffect {
        public Butterfly() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Butterfly"; }
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
