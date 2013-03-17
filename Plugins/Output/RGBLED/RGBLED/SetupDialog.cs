namespace RGBLED {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen.Dialogs;

	public partial class SetupDialog : Form {

		private SerialPort m_selectedPort;

		public SetupDialog(SerialPort selectedPort, XmlNode setupNode) {
			this.InitializeComponent();
			this.m_selectedPort = selectedPort;
			foreach (XmlNode node in setupNode.SelectSingleNode("Controllers").SelectNodes("Controller")) {
				this.AddController(node.Attributes["config"].Value);
			}
		}

		private void AddController(string configuration) {
			RGBLEDController controller;
			this.panelContainer.Controls.Add(controller = new RGBLEDController(this.panelContainer.Controls.Count + 1));
			controller.Dock = DockStyle.Top;
			controller.IndexChange += new RGBLEDController.OnIndexChange(this.controller_IndexChange);
			controller.Configuration = configuration;
			this.panelContainer.Controls.SetChildIndex(controller, 0);
		}

		private void buttonAdd_Click(object sender, EventArgs e) {
			this.AddController("software");
		}

		private void buttonChangeID_Click(object sender, EventArgs e) {
			int num;
			int num2;
			try {
				num = Convert.ToInt32(this.textBoxOldID.Text);
			}
			catch {
				MessageBox.Show("Value for old ID is not a valid number.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			try {
				num2 = Convert.ToInt32(this.textBoxNewID.Text);
			}
			catch {
				MessageBox.Show("Value for new ID is not a valid number.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			SerialPort port = new SerialPort(this.m_selectedPort.PortName, this.m_selectedPort.BaudRate, this.m_selectedPort.Parity, this.m_selectedPort.DataBits, this.m_selectedPort.StopBits);
			port.Open();
			try {
				port.Write(string.Format("#{0:X2}F0{1:X2}0D{1:X2}F10D", num, num2));
			}
			finally {
				port.Close();
			}
			MessageBox.Show("ID has been updated", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void buttonSerial_Click(object sender, EventArgs e) {
			SerialSetupDialog dialog = new SerialSetupDialog(this.m_selectedPort);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_selectedPort = dialog.SelectedPort;
			}
		}

		private void controller_IndexChange() {
			this.RenumberControls(null, null);
		}

		private void RenumberControls(object sender, ControlEventArgs e) {
			int count = this.panelContainer.Controls.Count;
			foreach (RGBLEDController controller in this.panelContainer.Controls) {
				controller.ID = count--;
			}
		}

		public string[] Controllers {
			get {
				string[] strArray = new string[this.panelContainer.Controls.Count];
				int index = 0;
				for (int i = this.panelContainer.Controls.Count - 1; i >= 0; i--) {
					if (((RGBLEDController)this.panelContainer.Controls[i]).Configuration.ToLower().Contains("hardware")) {
						strArray[index] = "hardware";
					}
					else {
						strArray[index] = "software";
					}
					index++;
				}
				return strArray;
			}
		}

		public SerialPort SelectedPort {
			get {
				return this.m_selectedPort;
			}
		}
	}
}