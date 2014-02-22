using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;



using VixenPlusCommon;

namespace VixenEditor {

    internal partial class RandomParametersDialog : Form {
        private readonly bool _actualLevels;
        private readonly bool _isGradient;
        private const int CellSize = 27;
        private const int CellWidthAndHeight = 10;


        public RandomParametersDialog(int minLevel, int maxLevel, bool actualLevels, bool isGradient) {
            InitializeComponent();
            _actualLevels = actualLevels;
            _isGradient = isGradient;
            lblPctMin.Visible = lblPctMax.Visible = !actualLevels;
            udMin.Minimum = udMax.Minimum = actualLevels ? minLevel : minLevel.ToPercentage();
            udMin.Maximum = udMax.Maximum = actualLevels ? maxLevel : maxLevel.ToPercentage();
            udMax.Value = udMax.Maximum;
            checkBoxUseSaturation_CheckedChanged(null, null);
            checkBoxIntensityLevel_CheckedChanged(null, null);
        }


        private void UpDownEnter(object sender, EventArgs e) {
            ((NumericUpDown) sender).Select(0, ((NumericUpDown) sender).Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        private int IntensityMax {
            get { return (int) (_actualLevels ? udMax.Value : ((int) udMax.Value).ToValue()); }
        }

        private int IntensityMin {
            get { return (int) (_actualLevels ? udMin.Value : ((int) udMin.Value).ToValue()); }
        }

        private int PeriodLength {
            get { return (int) udPeriods.Value; }
        }

        private float SaturationLevel {
            get { return ((float) udSaturation.Value / 100f); }
        }

        private bool UseSaturation {
            get { return checkBoxUseSaturation.Checked; }
        }

        private bool VaryIntensity {
            get { return checkBoxIntensityLevel.Checked; }
        }


        private void checkBoxUseSaturation_CheckedChanged(object sender, EventArgs e) {
            udSaturation.Enabled = checkBoxUseSaturation.Checked;
            pbExample.Refresh();
        }


        private void checkBoxIntensityLevel_CheckedChanged(object sender, EventArgs e) {
            udMin.Enabled = udMax.Enabled = checkBoxIntensityLevel.Checked;
            pbExample.Refresh();
        }


        private void ud_ValueChanged(object sender, EventArgs e) {
            if (sender.Equals(udMin) && udMin.Value > udMax.Value) {
                udMax.Value = udMin.Value;
            }
            if (sender.Equals(udMax) && udMax.Value < udMin.Value) {
                udMin.Value = udMax.Value;
            }
            pbExample.Refresh();
        }


        private void pbExample_Paint(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.Clear(pbExample.BackColor);

            for (var i = 0; i <= pbExample.Height; i += CellSize) {
                g.DrawLine(Pens.Black, i, 0, i, pbExample.Width);
                g.DrawLine(Pens.Black, 0, i, pbExample.Height, i);
            }

            var rowColors = new[] {Color.Red, Color.Blue, Color.Green, Color.White};
            var eventValues = GetEventValues(CellWidthAndHeight, CellWidthAndHeight);

            using (var brush = new SolidBrush(Color.Black)) {
                for (var rows = 0; rows < CellWidthAndHeight; rows++) {
                    for (var columns = 0; columns < CellWidthAndHeight; columns++) {
                        brush.Color = Color.FromArgb(_isGradient ? eventValues[rows, columns] : 255, rowColors[rows % rowColors.Length]);
                        var height = _isGradient ? CellSize : (int) (CellSize * ((float) eventValues[rows, columns] / 255));
                        g.FillRectangle(brush, columns * CellSize + 1, (rows + 1) * CellSize - height + 1, CellSize - 1, height - 1);
                    }
                }
            }
        }


        public byte[,] GetEventValues(int rows, int columns) {
            var random = new Random();
            var eventValues = new byte[rows,columns];
            var satTarget = (int) (UseSaturation ? SaturationLevel * (rows * columns) : random.Next(1, rows * columns));
            var computedSaturation = 0;

            while (computedSaturation < satTarget) {
                var row = random.Next(rows);
                var column = random.Next(columns);
                if (eventValues[row, column] != 0) {
                    continue;
                }

                var drawingLevel = (byte) (VaryIntensity ? random.Next(IntensityMin, IntensityMax + 1) : udMax.Maximum);
                for (var i = 0; (i < PeriodLength && column + i < columns); i++) {
                    if (eventValues[row, column + i] != 0) computedSaturation--;
                    eventValues[row, column + i] = drawingLevel;
                    computedSaturation++;
                }
            }
            return eventValues;
        }

    }
}
