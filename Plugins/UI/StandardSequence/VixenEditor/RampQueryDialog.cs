using System.Globalization;

namespace VixenEditor {
	using System;
    using System.Windows.Forms;

	internal partial class RampQueryDialog : Form {
		private readonly bool _actualLevels;

		public RampQueryDialog(int minLevel, int maxLevel, bool descending, bool actualLevels) {
			int num;
			int num2;
			components = null;
			InitializeComponent();
			_actualLevels = actualLevels;
			if (actualLevels) {
				num = minLevel;
				num2 = maxLevel;
			}
			else {
				num = (int)Math.Round(minLevel / 255f * 100f, MidpointRounding.AwayFromZero);
				num2 = (int)Math.Round(maxLevel / 255f * 100f, MidpointRounding.AwayFromZero);
			}
			numericUpDownStart.Minimum = num;
			numericUpDownEnd.Minimum = num;
			numericUpDownStart.Maximum = num2;
			numericUpDownEnd.Maximum = num2;
			if (!descending) {
				numericUpDownStart.Value = numericUpDownEnd.Minimum;
				numericUpDownEnd.Value = numericUpDownEnd.Maximum;
			}
			else {
				numericUpDownStart.Value = numericUpDownEnd.Maximum;
				numericUpDownEnd.Value = numericUpDownEnd.Minimum;
			}
		}

		private void numericUpDownEnd_Enter(object sender, EventArgs e) {
			numericUpDownEnd.Select(0, numericUpDownEnd.Value.ToString(CultureInfo.InvariantCulture).Length);
		}

		private void numericUpDownStart_Enter(object sender, EventArgs e) {
			numericUpDownStart.Select(0, numericUpDownStart.Value.ToString(CultureInfo.InvariantCulture).Length);
		}

		public int EndingLevel {
			get {
				if (_actualLevels) {
					return (int)numericUpDownEnd.Value;
				}
				return (int)((numericUpDownEnd.Value / 100M) * 255M);
			}
		}

		public int StartingLevel {
			get {
				if (_actualLevels) {
					return (int)numericUpDownStart.Value;
				}
				return (int)((numericUpDownStart.Value / 100M) * 255M);
			}
		}
	}
}