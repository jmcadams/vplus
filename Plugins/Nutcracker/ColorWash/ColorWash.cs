using System.Windows.Forms;
using VixenPlus;

namespace ColorWash {
    public partial class ColorWash : UserControl, INutcrackerEffect {
        public ColorWash() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Color Wash"; }
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
