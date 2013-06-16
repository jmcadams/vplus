using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Text {
    public partial class Text : UserControl, INutcrackerEffect {
        public Text() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Text"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Text_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
