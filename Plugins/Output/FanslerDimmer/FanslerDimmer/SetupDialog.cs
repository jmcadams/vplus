namespace FanslerDimmer {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;

	public partial class SetupDialog : Form {

		private XmlNode m_setupNode;

		public SetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			this.m_dimmingDialog = new DimmingDialog();
			XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
			this.textBoxParallel1From.Text = node.Attributes["from"].Value;
			this.textBoxParallel1To.Text = node.Attributes["to"].Value;
			this.checkBoxParallel1Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
			node = this.m_setupNode.SelectSingleNode("Parallel2");
			this.textBoxParallel2From.Text = node.Attributes["from"].Value;
			this.textBoxParallel2To.Text = node.Attributes["to"].Value;
			this.checkBoxParallel2Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
			node = this.m_setupNode.SelectSingleNode("Parallel3");
			this.textBoxParallel3From.Text = node.Attributes["from"].Value;
			this.textBoxParallel3To.Text = node.Attributes["to"].Value;
			this.checkBoxParallel3Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
			node.Attributes["from"].Value = this.textBoxParallel1From.Text;
			node.Attributes["to"].Value = this.textBoxParallel1To.Text;
			node.Attributes["reversed"].Value = this.checkBoxParallel1Reverse.Checked ? "1" : "0";
			node = this.m_setupNode.SelectSingleNode("Parallel2");
			node.Attributes["from"].Value = this.textBoxParallel2From.Text;
			node.Attributes["to"].Value = this.textBoxParallel2To.Text;
			node.Attributes["reversed"].Value = this.checkBoxParallel2Reverse.Checked ? "1" : "0";
			node = this.m_setupNode.SelectSingleNode("Parallel3");
			node.Attributes["from"].Value = this.textBoxParallel3From.Text;
			node.Attributes["to"].Value = this.textBoxParallel3To.Text;
			node.Attributes["reversed"].Value = this.checkBoxParallel3Reverse.Checked ? "1" : "0";
		}

		private void buttonParallel1Dimming_Click(object sender, EventArgs e) {
			XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
			node.Attributes["from"].Value = this.textBoxParallel1From.Text;
			node.Attributes["to"].Value = this.textBoxParallel1To.Text;
			this.m_dimmingDialog.PortElement = (XmlElement)node;
			this.m_dimmingDialog.ShowDialog();
		}

		private void buttonParallel2Dimming_Click(object sender, EventArgs e) {
			XmlNode node = this.m_setupNode.SelectSingleNode("Parallel2");
			node.Attributes["from"].Value = this.textBoxParallel2From.Text;
			node.Attributes["to"].Value = this.textBoxParallel2To.Text;
			this.m_dimmingDialog.PortElement = (XmlElement)node;
			this.m_dimmingDialog.ShowDialog();
		}

		private void buttonParallel3Dimming_Click(object sender, EventArgs e) {
			XmlNode node = this.m_setupNode.SelectSingleNode("Parallel3");
			node.Attributes["from"].Value = this.textBoxParallel3From.Text;
			node.Attributes["to"].Value = this.textBoxParallel3To.Text;
			this.m_dimmingDialog.PortElement = (XmlElement)node;
			this.m_dimmingDialog.ShowDialog();
		}
	}
}