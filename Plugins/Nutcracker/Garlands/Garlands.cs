using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Garlands {
    public partial class Garlands : UserControl, INutcrackerEffect {
        public Garlands() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Garlands"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }


        private void Garlands_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }

    }
}
