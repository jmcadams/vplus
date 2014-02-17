using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonControls {
    public partial class ColorDialog : Form {
        public ColorDialog(Color color) {
            InitializeComponent();
            pbColor.BackColor = color;
            colorEditor1.Color = color;
            colorWheel1.Color = color;
        }

        private void colorEditor1_ColorChanged(object sender, EventArgs e) {
            pbColor.BackColor = colorEditor1.Color;
            colorWheel1.Color = colorEditor1.Color;
        }

        private void colorWheel1_ColorChanged(object sender, EventArgs e) {
            pbColor.BackColor = colorWheel1.Color;
            colorEditor1.Color = colorWheel1.Color;
        }


        public Color GetColor() {
            return pbColor.BackColor;
        }
    }
}
