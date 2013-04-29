using System.Globalization;

namespace VixenEditor {
    using System;
    using System.Windows.Forms;

    internal partial class RandomParametersDialog : Form {
        private readonly bool _actualLevels;


        public RandomParametersDialog(int minLevel, int maxLevel, bool actualLevels) {
            InitializeComponent();
            _actualLevels = actualLevels;
            lblPctMin.Visible = lblPctMax.Visible = !actualLevels;
            udMin.Minimum = udMax.Minimum = actualLevels ? minLevel : CommonUtils.Utils.ToPercentage(minLevel);
            udMin.Maximum = udMax.Maximum = actualLevels ? maxLevel : CommonUtils.Utils.ToPercentage(maxLevel);
            udMax.Value = udMax.Maximum;
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
    }
}
