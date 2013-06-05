using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using CommonControls;

using VixenPlus;

namespace VixenEditor {
    public sealed partial class GroupDialog : Form {

        private class GroupTreeData {
            public Color NodeColor { get; set; }
            public bool IsChannel { get; set; }
        }

        private readonly EventSequence _seq;


        public GroupDialog(EventSequence sequence, bool constrainToGroup) {
            InitializeComponent();
            MinimumSize = Size;
            SetButtons();
            _seq = sequence;
            foreach (var c in constrainToGroup ? _seq.Channels : _seq.FullChannels) {
                lbChannels.Items.Add(c.Name);
            }
            if (sequence.Groups == null) {
                return;
            }
            foreach (var g in sequence.Groups) {
                var thisNode = tvGroups.Nodes.Add(g.Key);
                AddSubNodes(g.Value.GroupChannels, thisNode);
                thisNode.Tag = new GroupTreeData {NodeColor = g.Value.GroupColor, IsChannel = false};
            }
        }


        private void AddSubNodes(string nodeData, TreeNode parentNode) {
            if (nodeData.Contains(Group.GroupTextDivider.ToString(CultureInfo.InvariantCulture))) {
                var groupData = nodeData.Split(Group.GroupTextDivider);
                foreach (var group in groupData[0].Split(new[] {','})) {
                    var thisNode = parentNode.Nodes.Add(group);
                    thisNode.Tag = new GroupTreeData { NodeColor = _seq.Groups[group].GroupColor, IsChannel = false };
                    AddSubNodes(_seq.Groups[group].GroupChannels, thisNode);
                }
                AddChannelNodes(groupData[1], parentNode);
            }
            else {
                AddChannelNodes(nodeData, parentNode);

            }
        }


        private void AddChannelNodes(string nodeData, TreeNode parentNode) {
            if (nodeData == "") {
                return;
            }
            foreach (var channel in nodeData.Split(new[] { ',' })) {
                var thisNode = parentNode.Nodes.Add(_seq.FullChannels[int.Parse(channel)].Name);
                thisNode.Tag = new GroupTreeData {NodeColor = _seq.FullChannels[int.Parse(channel)].Color, IsChannel = true};
            }   
        }


        private void SetButtons() {
            var isChannelSelected = lbChannels.SelectedItems.Count > 0;
            var activeNode = tvGroups.SelectedNode;
            var isAnyGroupNodeSelected = activeNode != null;
            var isRootNode = isAnyGroupNodeSelected && activeNode.Parent == null;
            var isLeafNode = isAnyGroupNodeSelected && activeNode.Nodes.Count == 0 && !((GroupTreeData)activeNode.Tag).IsChannel;

            // These two will need to be more complex since we can only move up or down so far, basically to the top
            // or bottom of the parent.
            var topNode = isAnyGroupNodeSelected && tvGroups.SelectedNode == tvGroups.Nodes[0];
            var bottomNode = isAnyGroupNodeSelected && tvGroups.SelectedNode == tvGroups.Nodes[tvGroups.Nodes.Count - 1];

            //Group side buttons
            btnAddRoot.Enabled = true;
            btnAddChild.Enabled = isAnyGroupNodeSelected && !isLeafNode;
            btnRemoveGroup.Enabled = !isLeafNode;
            btnRenameGroup.Enabled = isAnyGroupNodeSelected && !isLeafNode;
            btnColorGroup.Enabled = isRootNode;
            btnUp.Enabled = isAnyGroupNodeSelected && !topNode;
            btnDown.Enabled = isAnyGroupNodeSelected && !bottomNode;

            btnAddChannels.Enabled = isChannelSelected;
            btnRemoveChannels.Enabled = isLeafNode;
        }


        private void lbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _seq.FullChannels[e.Index]);
        }


        private void tvGroups_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            var treeView = sender as MultiSelectTreeview;
            Channel.DrawItem(treeView, e, ((GroupTreeData)e.Node.Tag).NodeColor);
        }


        private void lbChannels_MouseUp(object sender, MouseEventArgs e) {
            Cursor.Current = Cursors.Default;
        }


        private void lbChannels_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right)
                return;

            Cursor.Current = Cursors.Hand;
        }


        private void btnGroupColor_Click(object sender, EventArgs e) {
            using (var color = new ColorDialog()) {
                if (color.ShowDialog() != DialogResult.OK) {
                    return;
                }

                var affectedNodeText = tvGroups.SelectedNode.Text;
                tvGroups.BeginUpdate();
                foreach (TreeNode treeNode in tvGroups.Nodes) {
                    SetColorByName(treeNode, color.Color, affectedNodeText);
                }
                tvGroups.EndUpdate();
            }
        }


        private void SetColorByName(TreeNode treeNode, Color color, String name) {
            if (treeNode.Text == name && treeNode.Nodes.Count != 0) {
                SetNodeColor(treeNode, color);
            }

            foreach (TreeNode node in treeNode.Nodes) {
                if (node.Text == name && node.Nodes.Count != 0) {
                    SetNodeColor(node, color);
                }
                SetColorByName(node, color, name);
            }
        }

        private void SetNodeColor(TreeNode node, Color color) {
            ((GroupTreeData)node.Tag).NodeColor = color;
            tvGroups.InvalidateNode(node);
        }


        private void tvGroups_AfterSelect(object sender, TreeViewEventArgs e) {
            SetButtons();
        }

        private void lbChannels_SelectedIndexChanged(object sender, EventArgs e) {
            SetButtons();
        }

        private void lbChannels_Leave(object sender, EventArgs e) {
            SetButtons();
        }

        private void tvGroups_Leave(object sender, EventArgs e) {
            SetButtons();
        }

        private void GroupDialog_SizeChanged(object sender, EventArgs e) {

            const int heightOffset = 75;
            lbChannels.Height = Height - heightOffset;

            const int allButtonAndMarginWidths = 208;
            const double channelPct = 0.4;
            var availableArea = Size.Width - allButtonAndMarginWidths;
            lbChannels.Width = (int)(availableArea * channelPct);

            const int channelButtonMargin = 6;
            var channelButtonX = lbChannels.Location.X + lbChannels.Width + channelButtonMargin;
            btnAddChannels.Location = new Point(channelButtonX, btnAddChannels.Location.Y);
            btnRemoveChannels.Location = new Point(channelButtonX, btnRemoveChannels.Location.Y);

            const double groupPct = 0.6;
            tvGroups.Location = new Point(btnAddChannels.Location.X + btnAddChannels.Width + channelButtonMargin, tvGroups.Location.Y);
            tvGroups.Height = Height - heightOffset;
            tvGroups.Width = (int) (availableArea * groupPct);

            const int labelOffset = 3;
            lblGroups.Location = new Point(tvGroups.Location.X - labelOffset, lblGroups.Location.Y);
        }

        private void btnAddGroup_Click(object sender, EventArgs e) {

        }

        private void btnExpand_Click(object sender, EventArgs e) {
            tvGroups.ExpandAll();
        }

        private void btnCollapse_Click(object sender, EventArgs e) {
            tvGroups.CollapseAll();

            if (tvGroups.SelectedNodes == null) {
                return;
            }

            foreach (var node in tvGroups.SelectedNodes) node.EnsureVisible();
        }
    }
}
