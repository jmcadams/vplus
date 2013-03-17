namespace Icicles {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Windows.Forms;
	using Vixen.Dialogs;

	public partial class SetupDialog : Form {

		private SerialPort m_selectedPort;

		public SetupDialog(SerialPort selectedPort, int version) {
			this.InitializeComponent();
			this.m_selectedPort = selectedPort;
			this.comboBoxVersion.SelectedIndex = version - 1;
		}

		private void buttonAssign_Click(object sender, EventArgs e) {
			int num = 0;
			try {
				num = Convert.ToInt32(this.textBoxBoardID.Text);
			}
			catch {
				MessageBox.Show("Please make sure the board ID is a valid decimal integer.", "Icicles", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			this.m_selectedPort.Open();
			byte[] buffer2 = new byte[] { 0x3a, 0x2a, 0x3d, 0x4a, 0x3d, 0x31, 0x65, 0 };
			buffer2[7] = (byte)num;
			byte[] buffer = buffer2;
			this.m_selectedPort.Write(buffer, 0, buffer.Length);
			this.m_selectedPort.Close();
			MessageBox.Show("Assignment has been sent", "Icicles", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void buttonSerialSetup_Click(object sender, EventArgs e) {
			SerialSetupDialog dialog = new SerialSetupDialog(this.m_selectedPort);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_selectedPort = dialog.SelectedPort;
			}
		}

		public SerialPort SelectedPort {
			get {
				return this.m_selectedPort;
			}
		}

		public int Version {
			get {
				return (this.comboBoxVersion.SelectedIndex + 1);
			}
		}
	}
}