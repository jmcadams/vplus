using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using VixenPlusCommon.Properties;

namespace VixenPlusCommon {
    public partial class ColorPalette : UserControl {

        private const string PbPrefix = "pbRuleColor";
        public const string PaletteElement = "Palette";


        public ColorPalette() {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public XElement Palette {
            get {
                var palette = new XElement(PaletteElement);
                var count = 1;
                foreach (var c in GetAllPictureBoxes()) {
                    palette.Add(new XElement(String.Format("Color{0}", count++), c.BackColor.ToHTML()));
                }

                return palette;
            }
            set {
                for (var i = 1; i <= 8; i++) {
                    var color = value.Element(String.Format("Color{0}", i));
                    var control = Controls.Find(string.Format("{0}{1}", PbPrefix, i), true)[0];
                    if (null != color) {
                        FormatPaintBox(control, color);
                    }
                    else {
                        FormatPaintBox(control, Color.Transparent);
                    }
                }
            }
        }

        public IEnumerable<Color> Colors {
            get {
                return GetAllPictureBoxes().Select(c => c.BackColor).ToList();
            }
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


        private IEnumerable<Control> GetAllPictureBoxes() {
            return GetAll(this, typeof (PictureBox)).Where(c => c.Name.StartsWith(PbPrefix)).OrderBy(c => c.Name);
        }


        private static IEnumerable<Control> GetAll(Control control, Type type) {
            // ReSharper disable PossibleMultipleEnumeration
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
            // ReSharper restore PossibleMultipleEnumeration
        }

    }
}
