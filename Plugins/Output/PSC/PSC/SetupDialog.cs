namespace PSC {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;
	using VixenPlus.Dialogs;

	public partial class SetupDialog : Form {

		private SerialPort m_serialPort;
		private XmlNode m_setupNode;

		public SetupDialog(XmlNode setupNode, SerialPort serialPort) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			this.m_serialPort = serialPort;
			this.checkBoxRamp.Checked = bool.Parse(Xml.GetNodeAlways(this.m_setupNode, "Ramps", "false").InnerText);
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			Xml.SetValue(this.m_setupNode, "Name", this.m_serialPort.PortName);
			Xml.SetValue(this.m_setupNode, "Ramps", this.checkBoxRamp.Checked.ToString());
		}

		private void buttonSerialSetup_Click(object sender, EventArgs e) {
			SerialSetupDialog dialog = new SerialSetupDialog(this.m_serialPort, true, false, false, false, false);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_serialPort = dialog.SelectedPort;
			}
		}

		public bool Ramps {
			get {
				return this.checkBoxRamp.Checked;
			}
		}

		public SerialPort SelectedPort {
			get {
				return this.m_serialPort;
			}
		}
	}
}