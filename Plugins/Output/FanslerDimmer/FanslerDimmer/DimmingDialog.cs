namespace FanslerDimmer {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;

	public partial class DimmingDialog : Form {
		private int m_from = 0;
		private XmlElement m_portElement;
		private int m_to = 0;

		public DimmingDialog() {
			this.InitializeComponent();
		}

		private void buttonAll_Click(object sender, EventArgs e) {
			this.checkedListBoxChannels.BeginUpdate();
			for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++) {
				this.checkedListBoxChannels.SetItemChecked(i, true);
			}
			this.checkedListBoxChannels.EndUpdate();
		}

		private void buttonNone_Click(object sender, EventArgs e) {
			this.checkedListBoxChannels.BeginUpdate();
			for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++) {
				this.checkedListBoxChannels.SetItemChecked(i, false);
			}
			this.checkedListBoxChannels.EndUpdate();
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			StringBuilder builder = new StringBuilder();
			foreach (int num in this.checkedListBoxChannels.CheckedIndices) {
				builder.AppendFormat("{0},", num + this.m_from);
			}
			XmlNode node = this.m_portElement.SelectSingleNode("dimming");
			if (builder.Length > 0) {
				node.InnerText = builder.ToString().Substring(0, builder.Length - 1);
			}
			else {
				node.InnerText = string.Empty;
			}
		}

		private void buttonRange_Click(object sender, EventArgs e) {
			RangeDialog dialog = new RangeDialog(this.m_from, this.m_to);
			if (dialog.ShowDialog() == DialogResult.OK) {
				bool selected = dialog.Selected;
				this.checkedListBoxChannels.BeginUpdate();
				for (int i = dialog.From; i <= dialog.To; i++) {
					this.checkedListBoxChannels.SetItemChecked(i - this.m_from, selected);
				}
				this.checkedListBoxChannels.EndUpdate();
			}
		}

		public XmlElement PortElement {
			set {
				this.m_portElement = value;
				this.m_from = Convert.ToInt32(this.m_portElement.Attributes["from"].Value);
				this.m_to = Convert.ToInt32(this.m_portElement.Attributes["to"].Value);
				this.checkedListBoxChannels.BeginUpdate();
				this.checkedListBoxChannels.Items.Clear();
				for (int i = this.m_from; i <= this.m_to; i++) {
					this.checkedListBoxChannels.Items.Add("Channel " + i.ToString());
				}
				this.checkedListBoxChannels.EndUpdate();
				XmlNode node = this.m_portElement.SelectSingleNode("dimming");
				foreach (string str in node.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
					int num2 = Convert.ToInt32(str);
					if (((num2 - this.m_from) >= 0) && ((num2 - this.m_from) < this.checkedListBoxChannels.Items.Count)) {
						this.checkedListBoxChannels.SetItemChecked(num2 - this.m_from, true);
					}
				}
			}
		}
	}
}