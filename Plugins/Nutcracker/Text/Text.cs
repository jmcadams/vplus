using System.Windows.Forms;

using VixenPlus;

namespace Text {
    public partial class Text : UserControl, INutcrackerEffect {
        public Text() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Text"; }
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
