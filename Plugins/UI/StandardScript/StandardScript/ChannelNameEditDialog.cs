namespace StandardScript {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus;

	public partial class ChannelNameEditDialog : Form {
		public ChannelNameEditDialog(EventSequence sequence, int itemIndex) {
			this.InitializeComponent();
			List<string> list = new List<string>();
			int start = 0;
			int length = 0;
			int num3 = 0;
			foreach (Channel channel in sequence.Channels) {
				if (num3 == itemIndex) {
					length = channel.Name.Length;
				}
				else if (num3 < itemIndex) {
					start += channel.Name.Length + 2;
				}
				num3++;
				list.Add(channel.Name);
			}
			this.textBoxChannels.Lines = list.ToArray();
			this.textBoxChannels.Select(start, length);
		}

		private void ChannelNameEditDialog_Activated(object sender, EventArgs e) {
			base.ActiveControl = this.textBoxChannels;
			this.textBoxChannels.ScrollToCaret();
		}

		public string[] ChannelNames {
			get {
				return this.textBoxChannels.Lines;
			}
		}
	}
}