using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;


namespace Meteors {
    public partial class Meteors : UserControl, INutcrackerEffect {
        public Meteors() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Meteors"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Meteors_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
