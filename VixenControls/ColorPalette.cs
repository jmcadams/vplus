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

        public const string PaletteElement = "Palette";

        private const string PbPrefix = "pbRuleColor";
        private readonly Size _borderSize = SystemInformation.BorderSize;

        private PictureBox _currentPb;


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
            _currentPb = pb;

            FormatPaintBox(pb, GetColor(pb, pb.BackColor));
        }


        private Color GetColor(Control ctrl, Color initialColor) {
            var resultColor = initialColor;

            var location = ctrl.PointToScreen(new Point(0, 0));

            using (var dialog = new ColorPicker(initialColor)) {
                dialog.Location = new Point(Math.Max(_borderSize.Width * 4, location.X - dialog.Width - _borderSize.Width * 4),
                    Math.Max(_borderSize.Height * 4, location.Y - dialog.Height - _borderSize.Height * 4));

                dialog.ColorEditorColorChanged += OnColorEditorColorChanged;
                dialog.ShowDialog();
                dialog.ColorEditorColorChanged -= OnColorEditorColorChanged;

                switch (dialog.DialogResult) {
                    case DialogResult.OK:
                        resultColor = dialog.GetColor();
                        break;
                    case DialogResult.No:
                        resultColor = Color.Transparent;
                        break;
                }
            }
            return resultColor;
        }


        private void OnColorEditorColorChanged(object sender, EventArgs e) {
            FormatPaintBox(_currentPb, ((ColorEventArgs)e).Color);
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
