using System.Collections.Generic;
using System.Windows.Forms;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs {
    public partial class ChannelOutputMaskDialog : Form {
        public ChannelOutputMaskDialog(IEnumerable<Channel> channels) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            foreach (var channel in channels) {
                checkedListBoxChannels.Items.Add(channel, channel.Enabled);
            }
        }


        public IEnumerable<int> DisabledChannels {
            get {
                var disabledChannels = new List<int>();
                for (var i = 0; i < checkedListBoxChannels.Items.Count; i++) {
                    disabledChannels.Add(i);
                }
                foreach (int index in checkedListBoxChannels.CheckedIndices) {
                    disabledChannels.Remove(index);
                }
                return disabledChannels;
            }
        }
    }
}
