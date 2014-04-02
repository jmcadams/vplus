//TODO: Add support for:
// Line1 and Line2 now individually controlled and can be rotated independently and colored independently
//TODO: Fix calculating
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

using VixenPlus;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

namespace NutcrackerEffectsControl.Effects {
    [UsedImplicitly]
    public partial class Text : UserControl, INutcrackerEffect {

        private readonly bool _initializing = true;

        private const string TextLine1 = "ID_TEXTCTRL_Text{0}_Line1";
        private const string TextFont1 = "ID_TEXTCTRL_Text{0}_1_Font";
        private const string TextDirection1 = "ID_CHOICE_Text{0}_1_Dir";
        private const string TextPosition1 = "ID_SLIDER_Text{0}_1_Position";
        private const string TextRotation1 = "ID_SLIDER_Text{0}_1_TextRotation";

        private const string TextLine2 = "ID_TEXTCTRL_Text{0}_Line2";
        private const string TextFont2 = "ID_TEXTCTRL_Text{0}_2_Font";
        private const string TextDirection2 = "ID_CHOICE_Text{0}_2_Dir";
        private const string TextPosition2 = "ID_SLIDER_Text{0}_2_Position";
        private const string TextRotation2 = "ID_SLIDER_Text{0}_2_TextRotation";

        public Text() {
            InitializeComponent();
            cbDirection.SelectedIndex = 0;
            _initializing = false;
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Text"; }
        }

        public string Notes {
            get { return "Not working right"; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string> {
                TextLine1 + "=" + txtBoxLine1.Text,
                TextFont1 + "=" + lblFont.Text,
                TextDirection1 + "=" + cbDirection.SelectedItem,
                TextPosition1 + "=" + tbTop.Value,
                TextRotation1 + "=" + tbRotation.Value,
                TextLine2 + "=" + txtBoxLine2.Text,
                TextFont2 + "=" + lblFont.Text,
                TextDirection2 + "=" + cbDirection.SelectedItem,
                TextPosition2 + "=" + tbTop.Value,
                TextRotation2 + "=" + tbRotation.Value
            };
        }


        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var textLine1 = string.Format(TextLine1, effectNum);
            var textFont1 = string.Format(TextFont1, effectNum);
            var textDirection1 = string.Format(TextDirection1, effectNum);
            var textPosition1 = string.Format(TextPosition1, effectNum);
            var textRotation1 = string.Format(TextRotation1, effectNum);

            var textLine2 = string.Format(TextLine2, effectNum);
            var textFont2 = string.Format(TextFont2, effectNum);
            var textDirection2 = string.Format(TextDirection2, effectNum);
            var textPosition2 = string.Format(TextPosition2, effectNum);
            var textRotation2 = string.Format(TextRotation2, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] {'='}))) {

                if (keyValue[0].Equals(textLine1)) {
                    txtBoxLine1.Text = keyValue[1];
                }
                else if (keyValue[0].Equals(textFont1)) {
                    lblFont.Text = keyValue[1];
                }
                else if (keyValue[0].Equals(textDirection1)) {
                    var index = cbDirection.Items.IndexOf(keyValue[1]);
                    if (index >= 0) {
                        cbDirection.SelectedIndex = index;
                    }
                }
                else if (keyValue[0].Equals(textPosition1)) {
                    tbTop.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(textRotation1)) {
                    tbRotation.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(textLine2)) {
                    txtBoxLine2.Text = keyValue[1];
                }
                else if (keyValue[0].Equals(textFont2)) {
                }
                else if (keyValue[0].Equals(textDirection2)) {
                }
                else if (keyValue[0].Equals(textPosition2)) {
                }
                else if (keyValue[0].Equals(textRotation2)) {
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var bitmap = new Bitmap(bufferWidth, bufferHeight, PixelFormat.Format32bppRgb);
            var selectedColor = palette[0];

            using (var g = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(selectedColor)) {

                var line1 = txtBoxLine1.Text;
                var line2 = txtBoxLine2.Text;
                var textRotation = tbRotation.Value;
                var font = lblFont.Tag != null ? (Font)lblFont.Tag : new Font(FontFamily.GenericSansSerif, 12.0f);
                var msg = line1;

                if (line2.Length > 0) {
                    msg += "\n" + line2;
                }

                var sz1 = g.MeasureString(line1, font);
                var sz2 = g.MeasureString(line2, font);
                var maxwidth = (int)Math.Max(sz1.Width, sz2.Width);
                var maxht = (int)Math.Max(sz1.Height, sz2.Height);
                if ((textRotation > 45 && textRotation < 135) || (textRotation > 225 && textRotation < 315) ) {
                    var itmp = maxwidth;
                    maxwidth = maxht;
                    maxht = itmp;
                }
                var dctop = tbTop.Value * bufferHeight / 50 - bufferHeight / 2;
                var xlimit = (bufferWidth + maxwidth) * 8 + 1;
                var ylimit = (bufferHeight + maxht) * 8 + 1;
                var xcentered = (bufferWidth - maxwidth) / 2;
                if (textRotation > 0) {
                    g.RotateTransform(textRotation);
                }

                switch (cbDirection.SelectedIndex) {
                    case 0: // left
                        g.DrawString(msg, font, brush, new Point(bufferWidth - eventToRender % xlimit / 8, dctop));
                        break;
                    case 1: // right
                        g.DrawString(msg, font, brush, new Point(eventToRender % xlimit / 8 - bufferWidth, dctop));
                        break;
                    case 2: // up
                        g.DrawString(msg, font, brush, new Point(xcentered, bufferHeight - eventToRender % ylimit / 8));
                        break;
                    case 3: // down
                        g.DrawString(msg, font, brush, new Point(xcentered, eventToRender % ylimit / 8 - bufferHeight));
                        break;
                    default: // no movement - centered
                        g.DrawString(msg, font, brush, new Point(xcentered, dctop));
                        break;
                }


                for (var x = 0; x < bufferWidth; x++) {
                    for (var y = 0; y < bufferHeight; y++) {
                        buffer[y, x] = bitmap.GetPixel(x, bufferHeight - y - 1);
                    }
                }
            }
            bitmap.Dispose();

            return buffer;
        }

        private void Text_ControlChanged(object sender, EventArgs e) {
            if (_initializing) return;
            OnControlChanged(this, e);
        }

        private void btnFont_Click(object sender, EventArgs e) {
            using (var dialog = new FontDialog()) {
                dialog.ShowColor = true;
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                lblFont.Text = dialog.Font.FontFamily.ToString();
                lblFont.Tag = dialog.Font;
            }
        }
    }
}
