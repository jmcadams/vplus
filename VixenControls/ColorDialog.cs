using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonControls {
    public partial class ColorDialog : Form {
        public ColorDialog(Color color) {
            InitializeComponent();
            pbOriginalColor.BackColor = color;
            colorEditor1.Color = color;
        }

        private void colorEditor1_ColorChanged(object sender, EventArgs e) {
            pbNewColor.BackColor = colorEditor1.Color;
        }

        public Color GetColor() {
            return pbNewColor.BackColor;
        }
    }
}
