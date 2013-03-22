namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	public partial class SequenceSettingsDialog : Form {
		private EventSequence m_sequence = null;

		public SequenceSettingsDialog(EventSequence sequence) {
			this.InitializeComponent();
			this.m_sequence = sequence;
			this.numericUpDownMinimum.Value = sequence.MinimumLevel;
			this.numericUpDownMaximum.Value = sequence.MaximumLevel;
			this.textBoxEventPeriodLength.Text = sequence.EventPeriod.ToString();
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.numericUpDownMinimum.Value >= this.numericUpDownMaximum.Value) {
				MessageBox.Show("Minimum must be less than the maximum.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = System.Windows.Forms.DialogResult.None;
			}
			else {
				this.m_sequence.MinimumLevel = (byte)this.numericUpDownMinimum.Value;
				this.m_sequence.MaximumLevel = (byte)this.numericUpDownMaximum.Value;
				this.Cursor = Cursors.WaitCursor;
				try {
					int num = Convert.ToInt32(this.textBoxEventPeriodLength.Text);
					this.m_sequence.EventPeriod = num;
				}
				catch {
				}
				finally {
					this.Cursor = Cursors.Default;
				}
			}
		}




	}
}

