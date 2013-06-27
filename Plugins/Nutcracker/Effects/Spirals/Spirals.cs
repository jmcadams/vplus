using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;

using VixenPlus;

namespace Spirals {
    public partial class Spirals : UserControl, INutcrackerEffect {
        public Spirals() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Spirals"; }
        }

        public string Notes {
            get { return String.Empty; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public XmlElement Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private static XmlElement GetCurrentSettings() {
            return Xml.CreateXmlDocument().DocumentElement;
        }

        private static void Setup(XmlElement settings) {
            System.Diagnostics.Debug.Print(settings.ToString());
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var colorCount = palette.Length;
            var spiralCount = colorCount * tbPaletteRepeat.Value;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var deltaStrands = bufferWidth / spiralCount;
            var spiralThickness = (deltaStrands * tbThickness.Value / 100) + 1;
            long spiralState = eventToRender * tbDirection.Value;

            for (var spiral = 0; spiral < spiralCount; spiral++) {
                var strandBase = spiral * deltaStrands;
                var color = palette[spiral % colorCount];
                for (var thickness = 0; thickness < spiralThickness; thickness++) {
                    var strand = (strandBase + thickness) % bufferWidth;
                    for (var row = 0; row < bufferHeight; row++) {
                        var column = (strand + ((int)spiralState / 10) + (row * tbRotations.Value / bufferHeight)) % bufferWidth;
                        if (column < 0) {
                            column += bufferWidth;
                        }
                        if (chkBoxBlend.Checked) {
                            color = HSVUtils.GetMultiColorBlend((bufferHeight - row - 1) / (double)bufferHeight, false, palette);
                        }
                        if (chkBox3D.Checked) {
                            var hsv = HSVUtils.ColorToHSV(color);
                            hsv.Value = (float)((double)(tbRotations.Value < 0 ? thickness + 1 : spiralThickness - thickness) / spiralThickness);
                            color = HSVUtils.HSVtoColor(hsv);
                        }
                        buffer[row, column] = color;
                    }
                }
            }
            return buffer;
        }

        private void Spirals_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
