using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Snowstorm {
    public partial class Snowstorm : UserControl, INutcrackerEffect {
        public Snowstorm() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Snow Storm"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void SnowStorm_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
