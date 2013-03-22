namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Text;
	using System.Windows.Forms;
	using Vixen;

	public partial class SerialSetupDialog : Form {
		public SerialSetupDialog(SerialPort serialPort) {
			this.components = null;
			this.InitializeComponent();
			this.comboBoxPortName.Items.AddRange(SerialPort.GetPortNames());
			this.Init(serialPort);
		}

		public SerialSetupDialog(SerialPort serialPort, bool allowPortEdit, bool allowBaudEdit, bool allowParityEdit, bool allowDataEdit, bool allowStopEdit) {
			this.components = null;
			this.InitializeComponent();
			this.comboBoxPortName.Items.AddRange(SerialPort.GetPortNames());
			this.comboBoxPortName.Enabled = allowPortEdit;
			this.comboBoxBaudRate.Enabled = allowBaudEdit;
			this.comboBoxParity.Enabled = allowParityEdit;
			this.textBoxData.Enabled = allowDataEdit;
			this.comboBoxStop.Enabled = allowStopEdit;
			this.Init(serialPort);
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			base.DialogResult = System.Windows.Forms.DialogResult.None;
			StringBuilder builder = new StringBuilder();
			if (this.comboBoxPortName.SelectedIndex == -1) {
				builder.AppendLine("* Port name has not been selected.");
			}
			if (this.comboBoxBaudRate.SelectedIndex == -1) {
				builder.AppendLine("* Baud rate has not been selected.");
			}
			if (this.comboBoxParity.SelectedIndex == -1) {
				builder.AppendLine("* Parity has not been selected.");
			}
			int result = 0;
			if (!int.TryParse(this.textBoxData.Text, out result)) {
				builder.AppendLine("* Invalid numeric value for data bits.");
			}
			if (this.comboBoxStop.SelectedIndex == -1) {
				builder.AppendLine("* Stop bits have not been selected.");
			}
			if (builder.Length > 0) {
				MessageBox.Show("The following items need to be resolved:\n\n" + builder.ToString(), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else {
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}



		private void Init(SerialPort serialPort) {
			this.comboBoxParity.Items.Add(Parity.Even);
			this.comboBoxParity.Items.Add(Parity.Mark);
			this.comboBoxParity.Items.Add(Parity.None);
			this.comboBoxParity.Items.Add(Parity.Odd);
			this.comboBoxParity.Items.Add(Parity.Space);
			this.comboBoxStop.Items.Add(StopBits.None);
			this.comboBoxStop.Items.Add(StopBits.One);
			this.comboBoxStop.Items.Add(StopBits.OnePointFive);
			this.comboBoxStop.Items.Add(StopBits.Two);
			if (serialPort == null) {
				serialPort = new SerialPort("COM1", 0x9600, Parity.None, 8, StopBits.One);
			}
			this.comboBoxPortName.SelectedIndex = this.comboBoxPortName.Items.IndexOf(serialPort.PortName);
			this.comboBoxBaudRate.SelectedItem = serialPort.BaudRate.ToString();
			this.comboBoxParity.SelectedItem = serialPort.Parity;
			this.textBoxData.Text = serialPort.DataBits.ToString();
			this.comboBoxStop.SelectedItem = serialPort.StopBits;
		}



		public SerialPort SelectedPort {
			get {
				return new SerialPort(this.comboBoxPortName.SelectedItem.ToString(), int.Parse(this.comboBoxBaudRate.SelectedItem.ToString()), (Parity)this.comboBoxParity.SelectedItem, int.Parse(this.textBoxData.Text), (StopBits)this.comboBoxStop.SelectedItem);
			}
		}
	}
}

