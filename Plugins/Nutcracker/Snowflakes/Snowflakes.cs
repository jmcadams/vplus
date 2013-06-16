using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Snowflakes {
    public partial class Snowflakes : UserControl, INutcrackerEffect {
        public Snowflakes() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Snowflakes"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Snowflakes_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
