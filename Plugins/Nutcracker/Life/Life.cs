using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Life {
    public partial class Life : UserControl, INutcrackerEffect {
        public Life() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Life"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Life_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
