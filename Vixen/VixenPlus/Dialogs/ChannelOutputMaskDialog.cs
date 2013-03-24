using System.Collections.Generic;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class ChannelOutputMaskDialog : Form
	{
		public ChannelOutputMaskDialog(List<Channel> channels)
		{
			InitializeComponent();
			foreach (Channel channel in channels)
			{
				checkedListBoxChannels.Items.Add(channel, channel.Enabled);
			}
		}

		public List<int> DisabledChannels
		{
			get
			{
				var list = new List<int>();
				for (int i = 0; i < checkedListBoxChannels.Items.Count; i++)
				{
					list.Add(i);
				}
				foreach (int num2 in checkedListBoxChannels.CheckedIndices)
				{
					list.Remove(num2);
				}
				return list;
			}
		}
	}
}