namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	public partial class ParallelSetupDialog : Form {
		private int m_otherAddressIndex;

		public ParallelSetupDialog(int portAddress) {
			this.InitializeComponent();
			this.m_otherAddressIndex = 3;
			switch (portAddress) {
				case 0x278:
					this.comboBoxPort.SelectedIndex = 1;
					break;

				case 0x378:
					this.comboBoxPort.SelectedIndex = 0;
					break;

				case 0x3bc:
					this.comboBoxPort.SelectedIndex = 2;
					break;

				case 0:
					this.comboBoxPort.SelectedIndex = 0;
					break;

				default:
					this.textBoxPort.Text = portAddress.ToString("X4");
					this.comboBoxPort.SelectedIndex = 3;
					break;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.comboBoxPort.SelectedIndex == this.m_otherAddressIndex) {
				try {
					Convert.ToUInt16(this.textBoxPort.Text, 0x10);
				}
				catch {
					MessageBox.Show("The port number is not a valid hexadecimal number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					base.DialogResult = System.Windows.Forms.DialogResult.None;
				}
			}
		}

		private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e) {
			this.textBoxPort.Enabled = this.comboBoxPort.SelectedIndex == this.m_otherAddressIndex;
		}





		public ushort PortAddress {
			get {
				switch (this.comboBoxPort.SelectedIndex) {
					case 0:
						return 0x378;

					case 1:
						return 0x278;

					case 2:
						return 0x3bc;
				}
				return Convert.ToUInt16(this.textBoxPort.Text, 0x10);
			}
		}
	}
}

