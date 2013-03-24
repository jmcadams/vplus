namespace StandardScript {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	public partial class TextListDialog : Form {
		private string m_childName;
		private XmlNode m_contextNode;
		private bool m_suppressBlanks;

		public TextListDialog(XmlNode contextNode, string childName, bool suppressBlanks) {
			this.InitializeComponent();
			this.m_contextNode = contextNode;
			this.m_childName = childName;
			this.m_suppressBlanks = suppressBlanks;
			List<string> list = new List<string>();
			foreach (XmlNode node in contextNode.SelectNodes(childName)) {
				if (!(this.m_suppressBlanks && (!this.m_suppressBlanks || (node.InnerText.Trim().Length == 0)))) {
					list.Add(node.InnerText);
				}
			}
			this.textBoxItems.Lines = list.ToArray();
			base.ActiveControl = this.textBoxItems;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.m_contextNode.RemoveAll();
			foreach (string str in this.textBoxItems.Lines) {
				if (!(this.m_suppressBlanks && (!this.m_suppressBlanks || (str.Trim().Length == 0)))) {
					Xml.SetNewValue(this.m_contextNode, this.m_childName, str);
				}
			}
		}
	}
}