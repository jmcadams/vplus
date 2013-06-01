using System.Collections.Generic;
using System.Windows.Forms;

using VixenPlus;

namespace VixenEditor
{
    public partial class GroupDialog: Form
    {
        private readonly List<Channel> _channels;

        public GroupDialog(EventSequence sequence, bool constrainToGroup)
        {
            InitializeComponent();
            _channels = constrainToGroup ? sequence.Channels : sequence.FullChannels; 
            foreach (var c in _channels) {
                lbChannels.Items.Add(c.Name);
            }
            if (sequence.Groups == null) {
                return;
            }
            foreach (var g in sequence.Groups) {
                var thisNode = tvGroups.Nodes.Add(g.Key);
                thisNode.Nodes.Add(g.Value.GroupChannels);
            }
        }

        private void lbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _channels[e.Index]);
        }
    }
}
