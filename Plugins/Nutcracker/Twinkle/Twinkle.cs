using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Twinkle {
    public partial class Twinkle : UserControl, INutcrackerEffect {
        public Twinkle() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Twinkle"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Twinkle_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
