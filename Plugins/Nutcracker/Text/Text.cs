using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;
using VixenPlus;

//TODO: Add support for:
// Line1 and Line2 now individually controlled and can be rotated independently and colored independently
//TODO: Fix calculating

namespace Text {
    public partial class Text : UserControl, INutcrackerEffect {
        public Text() {
            InitializeComponent();
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

        public XmlElement Settings {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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
            OnControlChanged(this, new EventArgs());
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
