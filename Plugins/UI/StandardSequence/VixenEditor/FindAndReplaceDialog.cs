namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class FindAndReplaceDialog : Form {
		private bool m_actualLevels;

		public FindAndReplaceDialog(byte minimum, byte maximum, bool actualLevels) {
			int num;
			int num2;
			this.components = null;
			this.InitializeComponent();
			this.m_actualLevels = actualLevels;
			if (actualLevels) {
				num = minimum;
				num2 = maximum;
			}
			else {
				num = (int)Math.Round((double)((((float)minimum) / 255f) * 100f), MidpointRounding.AwayFromZero);
				num2 = (int)Math.Round((double)((((float)maximum) / 255f) * 100f), MidpointRounding.AwayFromZero);
			}
			this.numericUpDownFind.Minimum = num;
			this.numericUpDownReplaceWith.Minimum = num;
			this.numericUpDownFind.Maximum = num2;
			this.numericUpDownReplaceWith.Maximum = num2;
		}

		private void numericUpDownFind_Enter(object sender, EventArgs e) {
			this.numericUpDownFind.Select(0, this.numericUpDownFind.Value.ToString().Length);
		}

		private void numericUpDownReplaceWith_Enter(object sender, EventArgs e) {
			this.numericUpDownReplaceWith.Select(0, this.numericUpDownReplaceWith.Value.ToString().Length);
		}

		public byte FindValue {
			get {
				if (this.m_actualLevels) {
					return (byte)this.numericUpDownFind.Value;
				}
				return (byte)this.numericUpDownFind.Value;
			}
		}

		public byte ReplaceWithValue {
			get {
				if (this.m_actualLevels) {
					return (byte)this.numericUpDownReplaceWith.Value;
				}
				return (byte)this.numericUpDownReplaceWith.Value;
			}
		}
	}
}