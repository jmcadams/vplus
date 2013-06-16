using System;
using System.Drawing;
using System.Windows.Forms;
using VixenPlus;

namespace Butterfly {
    public partial class Butterfly : UserControl, INutcrackerEffect {
        public Butterfly() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Butterfly"; }
        }

        public byte[] EffectData {
            get { throw new NotImplementedException(); }
        }


        public void Startup() {
            throw new NotImplementedException();
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            throw new NotImplementedException();
        }

        private void Butterfly_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
