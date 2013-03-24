namespace JWLC {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	internal partial class SetupDialog : Form {

		private XmlNode m_setupNode;

		public SetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "Name").InnerText;
			this.numericUpDownPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "Baud").InnerText;
			this.comboBoxBaud.SelectedIndex = this.comboBoxBaud.Items.IndexOf(innerText);
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			Xml.SetValue(this.m_setupNode, "Name", "COM" + ((int)this.numericUpDownPort.Value).ToString());
			Xml.SetValue(this.m_setupNode, "Baud", this.comboBoxBaud.Text.ToString());
		}
	}
}