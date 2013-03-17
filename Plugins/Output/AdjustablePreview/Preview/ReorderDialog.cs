namespace Preview {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	internal partial class ReorderDialog : Form {

		private Dictionary<int, List<uint>> m_channelDictionary = new Dictionary<int, List<uint>>();

		public ReorderDialog(List<Channel> channels, Dictionary<int, List<uint>> channelDictionary) {
			this.InitializeComponent();
			foreach (KeyValuePair<int, List<uint>> pair in channelDictionary) {
				List<uint> list;
				this.m_channelDictionary[pair.Key] = list = new List<uint>();
				list.AddRange(pair.Value);
			}
			Channel[] items = channels.ToArray();
			this.comboBoxFrom.Items.AddRange(items);
			this.comboBoxTo.Items.AddRange(items);
		}

		private void buttonClear_Click(object sender, EventArgs e) {
			if (this.comboBoxTo.SelectedIndex == -1) {
				MessageBox.Show("Please select a channel to clear.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else {
				this.m_channelDictionary.Remove(this.comboBoxTo.SelectedIndex);
				MessageBox.Show(string.Format("Channel '{0}' has been cleared.", (Channel)this.comboBoxTo.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void buttonCopy_Click(object sender, EventArgs e) {
			if ((this.comboBoxFrom.SelectedIndex == -1) || (this.comboBoxTo.SelectedIndex == -1)) {
				MessageBox.Show("Please select channels to copy from and to.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else {
				List<uint> list = null;
				if (!this.m_channelDictionary.TryGetValue(this.comboBoxFrom.SelectedIndex, out list)) {
					MessageBox.Show(string.Format("{0} has no cells drawn.", (Channel)this.comboBoxFrom.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else {
					List<uint> list2 = null;
					if (!this.m_channelDictionary.TryGetValue(this.comboBoxTo.SelectedIndex, out list2)) {
						list2 = new List<uint>();
						this.m_channelDictionary[this.comboBoxTo.SelectedIndex] = list2;
					}
					else {
						list2.Clear();
					}
					list2.AddRange(list);
					MessageBox.Show(string.Format("Channel '{0}' has been copied to channel '{1}'.", (Channel)this.comboBoxFrom.SelectedItem, (Channel)this.comboBoxTo.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		public Dictionary<int, List<uint>> ChannelDictionary {
			get {
				return this.m_channelDictionary;
			}
		}
	}
}