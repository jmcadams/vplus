using System;
using System.Drawing;
using System.Windows.Forms;

using CommonControls.Properties;

namespace CommonControls {
    public partial class ColorDialog : Form {
        public ColorDialog(Color color) {
            InitializeComponent();
            SetColorOrImage(pbOriginalColor, color);
            colorEditor1.Color = color;
        }


        private void colorEditor1_ColorChanged(object sender, EventArgs e) {
            SetColorOrImage(pbNewColor, colorEditor1.Color);
        }


        public Color GetColor() {
            return pbNewColor.BackColor;
        }


        private static void SetColorOrImage(PictureBox pb, Color color) {
            pb.BackColor = color;
            pb.BackgroundImage = color == Color.Transparent ? Resources.cellbackground : null;
        }
    }
}