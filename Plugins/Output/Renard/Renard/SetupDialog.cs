namespace Renard {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Windows.Forms;
	using Vixen.Dialogs;

	public partial class SetupDialog : Form {
		private SerialPort m_selectedPort;

		public SetupDialog(SerialPort selectedPort, int protocolVersion, bool holdPort) {
			this.InitializeComponent();
			this.m_selectedPort = selectedPort;
			this.comboBoxProtocolVersion.SelectedIndex = protocolVersion - 1;
			this.checkBoxHoldPort.Checked = holdPort;
		}

		private void buttonSerialSetup_Click(object sender, EventArgs e) {
			SerialSetupDialog dialog = new SerialSetupDialog(this.m_selectedPort);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_selectedPort = dialog.SelectedPort;
			}
		}

		public bool HoldPort {
			get {
				return this.checkBoxHoldPort.Checked;
			}
		}

		public int ProtocolVersion {
			get {
				return (this.comboBoxProtocolVersion.SelectedIndex + 1);
			}
		}

		public SerialPort SelectedPort {
			get {
				return this.m_selectedPort;
			}
		}
	}
}