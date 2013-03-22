namespace Vixen.Dialogs {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	public partial class ChannelOutputMaskDialog : Form {
		
		public ChannelOutputMaskDialog(List<Channel> channels) {
			this.InitializeComponent();
			foreach (Channel channel in channels) {
				this.checkedListBoxChannels.Items.Add(channel, channel.Enabled);
			}
		}
		
		public List<int> DisabledChannels {
			get {
				List<int> list = new List<int>();
				for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++) {
					list.Add(i);
				}
				foreach (int num2 in this.checkedListBoxChannels.CheckedIndices) {
					list.Remove(num2);
				}
				return list;
			}
		}
	}
}

