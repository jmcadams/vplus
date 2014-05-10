using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using VixenPlusCommon.Annotations;
using VixenPlusCommon.Properties;

namespace VixenPlusCommon {
    [DefaultEvent("ControlChanged")]
    public partial class ColorPalette : UserControl {

        public const string PaletteElement = "Palette";
        private const string PbPrefix = "pbRuleColor";
        private PictureBox _currentPb;

        public ColorPalette() {
            InitializeComponent();
        }


        [UsedImplicitly]
        public event EventHandler ControlChanged;

        private void OnPaletteChanged() {
            var handler = ControlChanged;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
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
            OnPaletteChanged();
        }


        private Color GetColor(Control ctrl, Color initialColor) {
            var resultColor = initialColor;
            const int offset = 6;

            using (var dialog = new ColorPicker(initialColor)) {
                dialog.Location = dialog.GetBestLocation(ctrl.PointToScreen(new Point(0, 0)), offset);
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
