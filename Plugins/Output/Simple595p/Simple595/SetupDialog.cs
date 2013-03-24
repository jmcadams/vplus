namespace Simple595 {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus.Dialogs;

	public partial class SetupDialog : Form {

		private int m_portAddress = 0;

		public SetupDialog(int portAddress, int pulseWidth) {
			this.InitializeComponent();
			this.m_portAddress = portAddress;
			if (pulseWidth != 0) {
				this.numericUpDownPulseWidth.Value = pulseWidth;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
		}

		private void buttonSetupPort_Click(object sender, EventArgs e) {
			ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_portAddress = dialog.PortAddress;
			}
		}

		private void numericUpDownPulseWidth_ValueChanged(object sender, EventArgs e) {
			this.labelPulses.Text = (this.numericUpDownPulseWidth.Value > 1M) ? "pulses." : "pulse.";
		}

		public int PortAddress {
			get {
				return this.m_portAddress;
			}
		}

		public int PulseWidth {
			get {
				return (int)this.numericUpDownPulseWidth.Value;
			}
		}
	}
}