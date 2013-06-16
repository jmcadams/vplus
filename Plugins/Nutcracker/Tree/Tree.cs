using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;

namespace Tree {
    public partial class Tree : UserControl, INutcrackerEffect {
        public Tree() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Tree"; }
        }

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Tree_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
