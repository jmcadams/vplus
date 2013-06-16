using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Fireworks {
    public partial class Fireworks : UserControl, INutcrackerEffect {
        public Fireworks() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Fireworks"; }
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

        private void Fireworks_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
