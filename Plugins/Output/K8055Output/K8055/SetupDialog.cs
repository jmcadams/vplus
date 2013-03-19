namespace K8055 {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class SetupDialog : Form {

		public SetupDialog(int channelCount, int[] deviceStartChannels) {
			this.InitializeComponent();
			int num = channelCount - 7;
			this.numericUpDownDev0.Maximum = this.numericUpDownDev1.Maximum = this.numericUpDownDev2.Maximum = this.numericUpDownDev3.Maximum = num;
			this.numericUpDownDev0.Value = Math.Min(deviceStartChannels[0] + 1, num);
			this.UpdateRange(this.numericUpDownDev0, this.labelDev0ChannelRange);
			this.numericUpDownDev1.Value = Math.Min(deviceStartChannels[1] + 1, num);
			this.UpdateRange(this.numericUpDownDev1, this.labelDev1ChannelRange);
			this.numericUpDownDev2.Value = Math.Min(deviceStartChannels[2] + 1, num);
			this.UpdateRange(this.numericUpDownDev2, this.labelDev2ChannelRange);
			this.numericUpDownDev3.Value = Math.Min(deviceStartChannels[3] + 1, num);
			this.UpdateRange(this.numericUpDownDev3, this.labelDev3ChannelRange);
			this.SearchDevices();
		}

		private void buttonDriverVersion_Click(object sender, EventArgs e) {
			K8055.Version();
		}

		private void buttonSearchDevices_Click(object sender, EventArgs e) {
			this.SearchDevices();
		}

		private void numericUpDownDev0_ValueChanged(object sender, EventArgs e) {
			this.UpdateRange(this.numericUpDownDev0, this.labelDev0ChannelRange);
		}

		private void numericUpDownDev1_ValueChanged(object sender, EventArgs e) {
			this.UpdateRange(this.numericUpDownDev1, this.labelDev1ChannelRange);
		}

		private void numericUpDownDev2_ValueChanged(object sender, EventArgs e) {
			this.UpdateRange(this.numericUpDownDev2, this.labelDev2ChannelRange);
		}

		private void numericUpDownDev3_ValueChanged(object sender, EventArgs e) {
			this.UpdateRange(this.numericUpDownDev3, this.labelDev3ChannelRange);
		}

		private void SearchDevices() {
			long num = 0L;
			this.Cursor = Cursors.WaitCursor;
			try {
				num = K8055.SearchDevices();
				this.checkBoxDev0.Checked = (num & 1L) != 0L;
				this.checkBoxDev1.Checked = (num & 2L) != 0L;
				this.checkBoxDev2.Checked = (num & 4L) != 0L;
				this.checkBoxDev3.Checked = (num & 8L) != 0L;
			}
			finally {
				this.Cursor = Cursors.Default;
			}
			if ((num & 15L) == 0L) {
				MessageBox.Show("No devices were found.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void UpdateRange(NumericUpDown upDownStart, Label labelEnd) {
			labelEnd.Text = string.Format("to {0}", upDownStart.Value + 7M);
		}

		public int[] DeviceStartChannels {
			get {
				return new int[] { (((int)this.numericUpDownDev0.Value) - 1), (((int)this.numericUpDownDev1.Value) - 1), (((int)this.numericUpDownDev2.Value) - 1), (((int)this.numericUpDownDev3.Value) - 1) };
			}
		}
	}
}