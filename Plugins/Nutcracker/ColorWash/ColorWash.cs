using System;
using System.Drawing;
using System.Windows.Forms;
using VixenPlus;

namespace ColorWash {
    public partial class ColorWash : UserControl, INutcrackerEffect {
        public ColorWash() {
            InitializeComponent();
        }
        
        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Color Wash"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void ColorWash_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
