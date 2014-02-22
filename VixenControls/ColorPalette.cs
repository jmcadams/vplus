using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

using CommonControls.Properties;

namespace CommonControls {
    public partial class ColorPalette : UserControl {
        public ColorPalette() {
            InitializeComponent();
        }

        private void pbRuleColor_DoubleClick(object sender, EventArgs e) {
            var pb = sender as PictureBox;
            if (null == pb) {
                return;
            }

            Color color;
            if (GetColor(pb, pb.BackColor, out color)) {
                FormatPaintBox(pb, color);
            }
        }

        private readonly Size _borderSize = SystemInformation.BorderSize;

        //todo refactor to take point instead of control for positioning.
        private bool GetColor(Control ctrl, Color initialColor, out Color resultColor, bool showNone = true) {
            var result = false;
            resultColor = Color.Black;

            var location = ctrl.PointToScreen(new Point(0, 0));

            using (var dialog = new ColorDialog(initialColor, showNone)) {
                dialog.Location = new Point(Math.Max(_borderSize.Width * 4, location.X - dialog.Width - _borderSize.Width),
                    Math.Max(_borderSize.Height * 4, location.Y - dialog.Height - _borderSize.Height));

                //dialog.CustomColors = _pref.GetString(Preference2.CustomColorsPreference);
                dialog.ShowDialog();
                //_pref.SetString(Preference2.CustomColorsPreference, dialog.CustomColors);

                switch (dialog.DialogResult) {
                    case DialogResult.OK:
                        resultColor = dialog.GetColor();
                        result = true;
                        break;
                    case DialogResult.No:
                        resultColor = Color.Transparent;
                        result = true;
                        break;
                }
            }

            return result;
        }

        private static void FormatPaintBox(Control pb, XElement color) {
            if (color == null) {
                return;
            }

            FormatPaintBox(pb, color.Value.FromHTML());
        }


        private static void FormatPaintBox(Control pb, Color color) {
            pb.BackColor = color;
            pb.BackgroundImage = color == Color.Transparent ? Resources.none : null;
            pb.BackgroundImageLayout = ImageLayout.Center;
        }

    }
}
