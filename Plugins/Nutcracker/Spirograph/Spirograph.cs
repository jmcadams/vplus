using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Spirograph {
    public partial class Spirograph : UserControl, INutcrackerEffect {
        public Spirograph() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirograph"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Spirograph_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
