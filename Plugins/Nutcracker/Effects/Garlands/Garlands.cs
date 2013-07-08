using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;
using VixenPlus;

namespace Garlands {
    public partial class Garlands : UserControl, INutcrackerEffect {

        private const string GarlandsType = "ID_SLIDER_Garlands{0}_Type";
        private const string GarlandsSpacing = "ID_SLIDER_Garlands{0}_Spacing";

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

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string>();
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var garlandsSpacing = string.Format(GarlandsSpacing, effectNum);
            var garlandsType = string.Format(GarlandsType, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(garlandsSpacing)) {
                    tbSpacing.Value = Utils.GetParsedValue(keyValue[1]);
                }
                else if (keyValue[0].Equals(garlandsType)) {
                    tbGarlandType.Value = Utils.GetParsedValue(keyValue[1]) + 1;
                }
            }
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
            OnControlChanged(this, e);
        }

    }
}
