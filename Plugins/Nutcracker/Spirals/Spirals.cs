using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Spirals {
    public partial class Spirals : UserControl, INutcrackerEffect {
        public Spirals() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirals"; }
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

        private void Spirals_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
