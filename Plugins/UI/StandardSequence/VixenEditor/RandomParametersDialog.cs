using System.Globalization;

namespace VixenEditor {
	using System;
    using System.Windows.Forms;

	internal partial class RandomParametersDialog : Form {
		private readonly bool _actualLevels;

		public RandomParametersDialog(int minLevel, int maxLevel, bool actualLevels) {
			InitializeComponent();
			_actualLevels = actualLevels;
			if (actualLevels) {
				label7.Visible = label8.Visible = false;
				numericUpDownIntensityMin.Minimum = numericUpDownIntensityMax.Minimum = minLevel;
				numericUpDownIntensityMin.Maximum = numericUpDownIntensityMax.Maximum = maxLevel;
			}
			else {
				numericUpDownIntensityMin.Minimum = numericUpDownIntensityMax.Minimum = minLevel * 100 / 255;
				numericUpDownIntensityMin.Maximum = numericUpDownIntensityMax.Maximum = maxLevel * 100 / 255;
			}
			numericUpDownIntensityMax.Value = numericUpDownIntensityMax.Maximum;
		}

		private void UpDownEnter(object sender, EventArgs e) {
			((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Value.ToString(CultureInfo.InvariantCulture).Length);
		}

		public int IntensityMax {
			get {
				if (_actualLevels) {
					return (int)numericUpDownIntensityMax.Value;
				}
				return ((((int)numericUpDownIntensityMax.Value) * 255) / 100);
			}
		}

		public int IntensityMin {
			get {
				if (_actualLevels) {
					return (int)numericUpDownIntensityMin.Value;
				}
				return ((((int)numericUpDownIntensityMin.Value) * 255) / 100);
			}
		}

		public int PeriodLength {
			get {
				return (int)numericUpDownPeriodLength.Value;
			}
		}

		public float SaturationLevel {
			get {
				return (((float)numericUpDownSaturationLevel.Value) / 100f);
			}
		}

		public bool UseSaturation {
			get {
				return checkBoxUseSaturation.Checked;
			}
		}

		public bool VaryIntensity {
			get {
				return checkBoxIntensityLevel.Checked;
			}
		}
	}
}