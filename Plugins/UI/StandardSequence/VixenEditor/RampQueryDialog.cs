namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class RampQueryDialog : Form {
		private bool m_actualLevels;

		public RampQueryDialog(int minLevel, int maxLevel, bool descending, bool actualLevels) {
			int num;
			int num2;
			this.components = null;
			this.InitializeComponent();
			this.m_actualLevels = actualLevels;
			if (actualLevels) {
				num = minLevel;
				num2 = maxLevel;
			}
			else {
				num = (int)Math.Round((double)((((float)minLevel) / 255f) * 100f), MidpointRounding.AwayFromZero);
				num2 = (int)Math.Round((double)((((float)maxLevel) / 255f) * 100f), MidpointRounding.AwayFromZero);
			}
			this.numericUpDownStart.Minimum = num;
			this.numericUpDownEnd.Minimum = num;
			this.numericUpDownStart.Maximum = num2;
			this.numericUpDownEnd.Maximum = num2;
			if (!descending) {
				this.numericUpDownStart.Value = this.numericUpDownEnd.Minimum;
				this.numericUpDownEnd.Value = this.numericUpDownEnd.Maximum;
			}
			else {
				this.numericUpDownStart.Value = this.numericUpDownEnd.Maximum;
				this.numericUpDownEnd.Value = this.numericUpDownEnd.Minimum;
			}
		}

		private void numericUpDownEnd_Enter(object sender, EventArgs e) {
			this.numericUpDownEnd.Select(0, this.numericUpDownEnd.Value.ToString().Length);
		}

		private void numericUpDownStart_Enter(object sender, EventArgs e) {
			this.numericUpDownStart.Select(0, this.numericUpDownStart.Value.ToString().Length);
		}

		public int EndingLevel {
			get {
				if (this.m_actualLevels) {
					return (int)this.numericUpDownEnd.Value;
				}
				return (int)((this.numericUpDownEnd.Value / 100M) * 255M);
			}
		}

		public int StartingLevel {
			get {
				if (this.m_actualLevels) {
					return (int)this.numericUpDownStart.Value;
				}
				return (int)((this.numericUpDownStart.Value / 100M) * 255M);
			}
		}
	}
}