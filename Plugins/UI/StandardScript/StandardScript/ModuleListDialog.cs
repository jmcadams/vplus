namespace StandardScript {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	public partial class ModuleListDialog : Form {
		private XmlNode m_projectNode;

		public ModuleListDialog(XmlNode projectNode) {
			this.InitializeComponent();
			this.m_projectNode = projectNode;
			List<string> list = new List<string>();
			foreach (string str2 in Directory.GetFiles(Paths.ScriptModulePath, "*.dll")) {
				string fileName = Path.GetFileName(str2);
				this.checkedListBoxModules.Items.Add(fileName);
				list.Add(fileName);
			}
			foreach (XmlNode node in projectNode.SelectNodes("ModuleReferences/*")) {
				this.checkedListBoxModules.SetItemChecked(list.IndexOf(node.InnerText), true);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_projectNode, "ModuleReferences");
			foreach (int num in this.checkedListBoxModules.CheckedIndices) {
				Xml.SetValue(emptyNodeAlways, "ModuleReference", (string)this.checkedListBoxModules.Items[num]);
			}
		}

		private void ModuleListDialog_Load(object sender, EventArgs e) {
			base.ActiveControl = this.checkedListBoxModules;
		}
	}
}