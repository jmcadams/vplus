using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Pictures {
    public partial class Picture : UserControl, INutcrackerEffect {
        public Picture() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Picture"; }
        }

        public byte[] EffectData {
            get { throw new NotImplementedException(); }
        }

        public void Startup() {
            throw new NotImplementedException();
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Pictures_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
