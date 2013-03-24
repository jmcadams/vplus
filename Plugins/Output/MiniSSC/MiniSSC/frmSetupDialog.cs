namespace MiniSSC {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	internal partial class frmSetupDialog : Form {

		private XmlNode m_setupNode;

		public frmSetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
			this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "baud").InnerText;
			this.comboBoxBaud.SelectedIndex = this.comboBoxBaud.Items.IndexOf(innerText);
			innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMin").InnerText;
			this.nudAnaMin.Value = (innerText.Length > 0) ? int.Parse(innerText) : 100;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMax").InnerText;
			this.nudAnaMax.Value = (innerText.Length > 0) ? int.Parse(innerText) : 200;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			Xml.SetValue(this.m_setupNode, "name", "COM" + ((int)this.nudPort.Value).ToString());
			Xml.SetValue(this.m_setupNode, "baud", this.comboBoxBaud.Text.ToString());
			Xml.SetValue(this.m_setupNode, "analogMin", ((int)this.nudAnaMin.Value).ToString());
			Xml.SetValue(this.m_setupNode, "analogMax", ((int)this.nudAnaMax.Value).ToString());
		}
	}
}