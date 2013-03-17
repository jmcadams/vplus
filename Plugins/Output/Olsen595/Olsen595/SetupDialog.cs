namespace Olsen595 {
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
			XmlNode node = this.m_setupNode.SelectSingleNode("parallel1");
			this.textBoxParallel1From.Text = node.Attributes["from"].Value;
			this.textBoxParallel1To.Text = node.Attributes["to"].Value;
			node = this.m_setupNode.SelectSingleNode("parallel2");
			this.textBoxParallel2From.Text = node.Attributes["from"].Value;
			this.textBoxParallel2To.Text = node.Attributes["to"].Value;
			node = this.m_setupNode.SelectSingleNode("parallel3");
			this.textBoxParallel3From.Text = node.Attributes["from"].Value;
			this.textBoxParallel3To.Text = node.Attributes["to"].Value;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			XmlNode node = this.m_setupNode.SelectSingleNode("parallel1");
			node.Attributes["from"].Value = this.textBoxParallel1From.Text;
			node.Attributes["to"].Value = this.textBoxParallel1To.Text;
			node = this.m_setupNode.SelectSingleNode("parallel2");
			node.Attributes["from"].Value = this.textBoxParallel2From.Text;
			node.Attributes["to"].Value = this.textBoxParallel2To.Text;
			node = this.m_setupNode.SelectSingleNode("parallel3");
			node.Attributes["from"].Value = this.textBoxParallel3From.Text;
			node.Attributes["to"].Value = this.textBoxParallel3To.Text;
		}
	}
}