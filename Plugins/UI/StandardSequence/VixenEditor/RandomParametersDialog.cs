namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class RandomParametersDialog : Form {
		private bool m_actualLevels;

		public RandomParametersDialog(int minLevel, int maxLevel, bool actualLevels) {
			this.InitializeComponent();
			this.m_actualLevels = actualLevels;
			if (actualLevels) {
				this.label7.Visible = this.label8.Visible = false;
				this.numericUpDownIntensityMin.Minimum = this.numericUpDownIntensityMax.Minimum = minLevel;
				this.numericUpDownIntensityMin.Maximum = this.numericUpDownIntensityMax.Maximum = maxLevel;
			}
			else {
				this.numericUpDownIntensityMin.Minimum = this.numericUpDownIntensityMax.Minimum = (minLevel * 100) / 0xff;
				this.numericUpDownIntensityMin.Maximum = this.numericUpDownIntensityMax.Maximum = (maxLevel * 100) / 0xff;
			}
			this.numericUpDownIntensityMax.Value = this.numericUpDownIntensityMax.Maximum;
		}

		private void UpDownEnter(object sender, EventArgs e) {
			((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Value.ToString().Length);
		}

		public int IntensityMax {
			get {
				if (this.m_actualLevels) {
					return (int)this.numericUpDownIntensityMax.Value;
				}
				return ((((int)this.numericUpDownIntensityMax.Value) * 0xff) / 100);
			}
		}

		public int IntensityMin {
			get {
				if (this.m_actualLevels) {
					return (int)this.numericUpDownIntensityMin.Value;
				}
				return ((((int)this.numericUpDownIntensityMin.Value) * 0xff) / 100);
			}
		}

		public int PeriodLength {
			get {
				return (int)this.numericUpDownPeriodLength.Value;
			}
		}

		public float SaturationLevel {
			get {
				return (((float)this.numericUpDownSaturationLevel.Value) / 100f);
			}
		}

		public bool UseSaturation {
			get {
				return this.checkBoxUseSaturation.Checked;
			}
		}

		public bool VaryIntensity {
			get {
				return this.checkBoxIntensityLevel.Checked;
			}
		}
	}
}