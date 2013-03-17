namespace GenericSerial {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	internal class SetupDialog : Form {
		private XmlNode m_setupNode;
		
		public SetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "Name").InnerText;
			this.numericUpDownPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "Baud").InnerText;
			this.comboBoxBaud.SelectedIndex = this.comboBoxBaud.Items.IndexOf(innerText);
			XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Header");
			if (nodeAlways.Attributes["checked"] != null) {
				this.checkBoxHeader.Checked = bool.Parse(nodeAlways.Attributes["checked"].Value);
			}
			this.textBoxHeader.Text = nodeAlways.InnerText;
			nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Footer");
			if (nodeAlways.Attributes["checked"] != null) {
				this.checkBoxFooter.Checked = bool.Parse(nodeAlways.Attributes["checked"].Value);
			}
			this.textBoxFooter.Text = nodeAlways.InnerText;
		}
		
		private void buttonOK_Click(object sender, EventArgs e) {
			Xml.SetValue(this.m_setupNode, "Name", "COM" + ((int)this.numericUpDownPort.Value).ToString());
			Xml.SetValue(this.m_setupNode, "Baud", this.comboBoxBaud.Text.ToString());
			Xml.SetAttribute(this.m_setupNode, "Header", "checked", this.checkBoxHeader.Checked.ToString()).InnerText = this.textBoxHeader.Text;
			Xml.SetAttribute(this.m_setupNode, "Footer", "checked", this.checkBoxFooter.Checked.ToString()).InnerText = this.textBoxFooter.Text;
		}
	}
}
