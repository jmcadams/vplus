using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;
using VixenPlus;

namespace Garlands {
    public partial class Garlands : UserControl, INutcrackerEffect {
        public Garlands() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Garlands"; }
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
            var rows = buffer.GetLength(Utils.IndexRowsOrHeight);
            var columns = buffer.GetLength(Utils.IndexColsOrWidth);
            var pixelSpacing = tbSpacing.Value * rows / 100 + 3;
            var limit = rows * pixelSpacing * 4;
            var garlandsState = (limit - (eventToRender % limit)) / 4;
            for (var ring = 0; ring < rows; ring++) {
                var ratio = ring / (double) rows;
                var color = HSVUtils.GetMultiColorBlend(ratio, false, palette);
                var intialRow = garlandsState - ring * pixelSpacing;
                for (var column = 0; column < columns; column++) {
                    var row = intialRow;
                    switch (tbGarlandType.Value) {
                        case 1:
                            switch (column % 5) {
                                case 2:
                                    row -= 2;
                                    break;
                                case 1:
                                case 3:
                                    row -= 1;
                                    break;
                            }
                            break;
                        case 2:
                            switch (column % 5) {
                                case 2:
                                    row -= 4;
                                    break;
                                case 1:
                                case 3:
                                    row -= 2;
                                    break;
                            }
                            break;
                        case 3:
                            switch (column % 6) {
                                case 3:
                                    row -= 6;
                                    break;
                                case 2:
                                case 4:
                                    row -= 4;
                                    break;
                                case 1:
                                case 5:
                                    row -= 2;
                                    break;
                            }
                            break;
                        case 4:
                            switch (column % 5) {
                                case 1:
                                case 3:
                                    row -= 2;
                                    break;
                            }
                            break;
                    }

                    if (row < rows - ring - 1) {
                        row = rows - ring - 1;
                    }
                    if (row < rows) {
                        buffer[row, column] = color;
                    }
                }
            }
            return buffer;
        }


        private void Garlands_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }

    }
}
