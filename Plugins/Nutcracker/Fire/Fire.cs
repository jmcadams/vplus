using System;
using System.Drawing;
using System.Windows.Forms;
using VixenPlus;

namespace Fire {
    public partial class Fire : UserControl, INutcrackerEffect {
        public Fire() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Fire"; }
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

        private void Fire_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
