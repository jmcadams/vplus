namespace CometConversion {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class OptionsDialog : Form {
		public OptionsDialog() {
			this.InitializeComponent();
		}

		private void buttonColor_Click(object sender, EventArgs e) {
			if (this.colorDialog.ShowDialog() == DialogResult.OK) {
				this.buttonColor.BackColor = this.colorDialog.Color;
			}
		}

		public Color BlackReplacement {
			get {
				return Color.FromArgb(-16777216 | this.buttonColor.BackColor.ToArgb());
			}
		}

		public int EventPeriod {
			get {
				try {
					return Convert.ToInt32(this.textBoxEventPeriodLength.Text);
				}
				catch {
					return 100;
				}
			}
		}
	}
}