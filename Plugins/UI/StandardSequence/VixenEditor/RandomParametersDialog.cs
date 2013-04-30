using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

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
            udMin.Minimum = udMax.Minimum = actualLevels ? minLevel : CommonUtils.Utils.ToPercentage(minLevel);
            udMin.Maximum = udMax.Maximum = actualLevels ? maxLevel : CommonUtils.Utils.ToPercentage(maxLevel);
            udMax.Value = udMax.Maximum;
            checkBoxUseSaturation_CheckedChanged(null, null);
            checkBoxIntensityLevel_CheckedChanged(null, null);
        }


        private void UpDownEnter(object sender, EventArgs e) {
            ((NumericUpDown) sender).Select(0, ((NumericUpDown) sender).Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        public int IntensityMax {
            get { return (int) (_actualLevels ? udMax.Value : CommonUtils.Utils.ToValue((int) udMax.Value)); }
        }

        public int IntensityMin {
            get { return (int) (_actualLevels ? udMin.Value : CommonUtils.Utils.ToValue((int) udMin.Value)); }
        }

        public int PeriodLength {
            get { return (int) udPeriods.Value; }
        }

        public float SaturationLevel {
            get { return (((float) udSaturation.Value) / 100f); }
        }

        public bool UseSaturation {
            get { return checkBoxUseSaturation.Checked; }
        }

        public bool VaryIntensity {
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


        private void pbExample_Paint(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.Clear(pbExample.BackColor);

            for (var i = 0; i <= pbExample.Height; i += CellSize) {
                g.DrawLine(Pens.Black, i, 0, i, pbExample.Width);
                g.DrawLine(Pens.Black, 0, i, pbExample.Height, i);
            }
            var random = new Random();
            var affectedRows = new List<int>();
            var eventValues = new byte[CellWidthAndHeight,CellWidthAndHeight];
            for (var columns = 0; columns < CellWidthAndHeight; columns += PeriodLength) {
                var saturation = UseSaturation
                                     ? random.Next(2) > 2
                                           ? (int) Math.Max(1, Math.Ceiling(CellWidthAndHeight * SaturationLevel - 0.1))
                                           : (int) Math.Max(1, Math.Floor(CellWidthAndHeight * SaturationLevel))
                                     : random.Next(1, CellWidthAndHeight + 1);

                for (var rows = 0; rows < CellWidthAndHeight; rows++) {
                    affectedRows.Add(rows);
                }
                while (saturation-- > 0) {
                    var index = random.Next(affectedRows.Count);
                    var drawingLevel = (byte) (VaryIntensity ? random.Next(IntensityMin, IntensityMax + 1) : udMax.Maximum);
                    drawingLevel = (byte)(_actualLevels ? drawingLevel : CommonUtils.Utils.ToValue(drawingLevel));
                    for (var i = 0; (i < PeriodLength && columns + i < CellWidthAndHeight); i++) {
                        eventValues[affectedRows[index], columns + i] = drawingLevel;
                    }
                    affectedRows.RemoveAt(index);
                }
            }

            var rowColors = new Color[]
            {Color.Red, Color.Blue, Color.Green, Color.White};

            using (var brush = new SolidBrush(Color.Black)) {
                var y = 1;
                for (var rows = 0; rows < CellWidthAndHeight; rows++) {
                    var x = 1;
                    for (var columns = 0; columns < CellWidthAndHeight; columns++) {
                        brush.Color = Color.FromArgb(_isGradient ? eventValues[rows, columns] : 255, rowColors[rows % rowColors.Length]);
                        var height = _isGradient ? CellSize : (int) (CellSize * ((float) eventValues[rows, columns] / 255));
                        g.FillRectangle(brush, x, y + CellSize - height, CellSize - 1, height - 1);
                        x += CellSize;
                    }
                    y += CellSize;
                }
            }
        }


        private void ud_ValueChanged(object sender, EventArgs e) {
            if (sender.Equals(udMin)) {
                if (udMin == udMax) {
                    return;
                }
                if (udMin.Value > udMax.Value) {
                    udMax.Value = udMin.Value;
                }
            }
            if (sender.Equals(udMax)) {
                if (udMax == udMin) {
                    return;
                }
                if (udMax.Value < udMin.Value) {
                    udMin.Value = udMax.Value;
                }
            }
            pbExample.Refresh();
        }
    }
}
