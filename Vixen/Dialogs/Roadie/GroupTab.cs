using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public sealed partial class GroupDialog : UserControl {

        private readonly Dictionary<string, GroupData> _groups;
        private readonly List<Channel> _fullChannels;
        private List<Channel> _channels = new List<Channel>();

        public TreeView GetResults {
            get { return tvGroups; }
        }

        private readonly bool _useCheckmark = Preference2.GetInstance().GetBoolean("UseCheckmark");

        private enum Sorts {
            Natural,
            ChannelColor,
            ChannelName,
            ChannelColorThenName,
            ChannelNameThenColor
        }


        public GroupDialog(IExecutable iExecutable, bool constrainToGroup) {
            List<Channel> channelSubset;
            InitializeComponent();
            MinimumSize = Size;

            var profile = iExecutable as Profile;
            if (profile != null) {
                _groups = profile.Groups;
                _fullChannels = profile.FullChannels;
                channelSubset = profile.Channels;
            }
            else {
                var sequence = iExecutable as EventSequence;
                if (sequence != null) {
                    _groups = sequence.Groups;
                    _fullChannels = sequence.FullChannels;
                    channelSubset = sequence.Channels;
                }
                else {
                    throw new ArgumentException("Did not pass a valid IExecutable type to Group Dialog");
                }
            }

            foreach (var c in constrainToGroup ? channelSubset : _fullChannels) {
                lbChannels.Items.Add(c.Name);
                _channels.Add(c);
            }
            cbSort.SelectedItem = cbSort.Items[0];
            SetButtons();
            if (_groups == null) {
                return;
            }
            foreach (var g in _groups) {
                var thisNode = tvGroups.Nodes.Add(g.Key);
                AddSubNodes(g.Value.GroupChannels, thisNode);
                thisNode.Name = g.Key;
                thisNode.Tag = new GroupTagData
                {NodeColor = g.Value.GroupColor, IsLeafNode = false, Zoom = g.Value.Zoom, IsSortOrder = g.Value.IsSortOrder};
            }
            UpdateStats();
        }


        private void AddSubNodes(string nodeData, TreeNode parentNode) {
            foreach (var node in nodeData.Split(',').Where(node => node != "")) {
                if (node.StartsWith(Group.GroupTextDivider)) {
                    var groupNode = node.TrimStart(Group.GroupTextDivider.ToCharArray());
                    var thisNode = parentNode.Nodes.Add(groupNode);
                    thisNode.Name = groupNode;
                    thisNode.Tag = new GroupTagData
                    {NodeColor = _groups[groupNode].GroupColor, IsLeafNode = false, Zoom = "100%", IsSortOrder = false};
                    AddSubNodes(_groups[groupNode].GroupChannels, thisNode);
                }
                else {
                    var chNum = int.Parse(node);

                    // NOTE: Somehow in Vixen 2.x bad sort order channel numbers can seep in, just ignore them.
                    if (chNum >= _fullChannels.Count) {
                        continue;
                    }

                    var channel = _fullChannels[chNum];
                    var thisNode = parentNode.Nodes.Add(channel.Name);
                    thisNode.Name = channel.Name;
                    thisNode.Tag = new GroupTagData {NodeColor = channel.Color, IsLeafNode = true, UnderlyingChannel = node, IsSortOrder = false};
                }
            }
        }


        // ReSharper disable once FunctionComplexityOverflow
        private void SetButtons() {
            var activeNode = tvGroups.SelectedNode;
            var isNodeActive = activeNode != null;
            var isSingleNode = tvGroups.SelectedNodes.Count == 1;
            var isRootNode = isNodeActive && activeNode.Parent == null;
            var isLeafNode = isNodeActive && ((GroupTagData) activeNode.Tag).IsLeafNode;
            var isChannelSelected = lbChannels.SelectedItems.Count > 0;

            var topNode = isNodeActive && tvGroups.SelectedNode.Index == 0;
            var bottomNode = isNodeActive &&
                             tvGroups.SelectedNode.Index ==
                             (isRootNode ? tvGroups.GetNodeCount(false) : tvGroups.SelectedNode.Parent.GetNodeCount(false)) - 1;

            var isOnlyLeafNodes = true;
            var isOnlyNonLeafNodes = true;
            var isParentAtRoot = true;
            var isEditable = true;
            foreach (var n in tvGroups.SelectedNodes) {
                isOnlyLeafNodes &= ((GroupTagData) n.Tag).IsLeafNode;
                isOnlyNonLeafNodes &= !((GroupTagData) n.Tag).IsLeafNode;
                isParentAtRoot &= (n.Parent == null || !n.Parent.FullPath.Contains("\\"));
                isEditable &= n.FullPath.Split('\\').Length - 1 < 2;
            }


            btnAddChild.Enabled = isRootNode && isSingleNode;
            btnRemoveGroup.Enabled = isNodeActive && !isLeafNode && isEditable;
            btnRenameGroup.Enabled = isRootNode && isSingleNode;
            btnColorGroup.Enabled = isRootNode;
            btnUp.Enabled = isNodeActive && !topNode && isSingleNode && isParentAtRoot;
            btnDown.Enabled = isNodeActive && !bottomNode && isSingleNode && isParentAtRoot;
            btnAddChannels.Enabled = isChannelSelected && isOnlyNonLeafNodes && isRootNode;
            btnRemoveChannels.Enabled = isNodeActive && isOnlyLeafNodes && isEditable;
        }


        private void UpdateStats() {
            var groupCount = tvGroups.GetNodeCount(false);
            var subNodeCount = tvGroups.GetNodeCount(true) - groupCount;
            var groupSuffix = groupCount != 1 ? "s" : "";
            var subNodeSuffix = subNodeCount != 1 ? "s" : "";
            var groupPrefix = groupCount != 1 ? "those" : "that";
            lblStats.Text = String.Format("{0} group{1} && {2} channel{3}/sub-group{3} in {4} group{1}", groupCount, groupSuffix, subNodeCount,
                subNodeSuffix, groupPrefix);
        }


        private void lbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _channels[e.Index], _useCheckmark);
        }


        private void tvGroups_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            var treeView = sender as MultiSelectTreeview;
            e.DrawItem(((GroupTagData) e.Node.Tag).NodeColor, treeView, _useCheckmark);
        }


        private void btnGroupColor_Click(object sender, EventArgs e) {
            using (var color = new ColorDialog {AllowFullOpen = true, AnyColor = true, FullOpen = true}) {
                color.CustomColors = Preference2.GetInstance().CustomColors;

                if (color.ShowDialog() != DialogResult.OK) {
                    return;
                }
                tvGroups.BeginUpdate();
                foreach (var s in tvGroups.SelectedNodes) {
                    var affectedNodeText = s.Text;
                    foreach (TreeNode treeNode in tvGroups.Nodes) {
                        SetColorByName(treeNode, color.Color, affectedNodeText);
                    }
                }
                tvGroups.EndUpdate();
                Preference2.GetInstance().CustomColors = color.CustomColors;

            }
        }


        private void SetColorByName(TreeNode treeNode, Color color, String name) {
            if (treeNode.Text == name && !((GroupTagData) treeNode.Tag).IsLeafNode) {
                SetNodeColor(treeNode, color);
            }

            foreach (TreeNode node in treeNode.Nodes) {
                if (node.Text == name && !((GroupTagData) treeNode.Tag).IsLeafNode) {
                    SetNodeColor(node, color);
                }
                SetColorByName(node, color, name);
            }
        }


        private void SetNodeColor(TreeNode node, Color color) {
            ((GroupTagData) node.Tag).NodeColor = color;
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
            lbChannels.Width = (int) (availableArea * channelPct);

            const int defaultMargin = 6;
            var channelButtonX = lbChannels.Location.X + lbChannels.Width + defaultMargin;
            btnAddChannels.Location = new Point(channelButtonX, btnAddChannels.Location.Y);
            btnRemoveChannels.Location = new Point(channelButtonX, btnRemoveChannels.Location.Y);

            const double groupPct = 0.6;
            tvGroups.Location = new Point(btnAddChannels.Location.X + btnAddChannels.Width + defaultMargin, tvGroups.Location.Y);
            tvGroups.Height = Height - heightOffset;
            tvGroups.Width = (int) (availableArea * groupPct);

            const int labelOffset = 3;
            lblGroups.Location = new Point(tvGroups.Location.X - labelOffset, lblGroups.Location.Y);
            lblStats.Location = new Point(lblGroups.Location.X + lblGroups.Width + defaultMargin, lblStats.Location.Y);
        }


        private void btnAddGroup_Click(object sender, EventArgs e) {
            var groupName = GetName("Group Name", "What do you want to name your new group?", "");
            if (string.IsNullOrEmpty(groupName)) {
                return;
            }

            CreateGroup(groupName);
            UpdateStats();
            tvGroups.Refresh();
        }


        private void CreateGroup(string groupName) {
            var thisNode = tvGroups.Nodes.Add(groupName);
            thisNode.Tag = new GroupTagData {IsLeafNode = false, NodeColor = Color.White, Zoom = "100%", IsSortOrder = false};
            thisNode.Name = groupName;
            thisNode.EnsureVisible();
        }


        private void btnExpand_Click(object sender, EventArgs e) {
            tvGroups.BeginUpdate();
            tvGroups.ExpandAll();
            tvGroups.EndUpdate();
        }


        private void btnCollapse_Click(object sender, EventArgs e) {
            tvGroups.BeginUpdate();
            tvGroups.CollapseAll();

            if (tvGroups.SelectedNodes == null) {
                return;
            }

            foreach (var node in tvGroups.SelectedNodes) node.EnsureVisible();
            tvGroups.EndUpdate();
        }


        private void btnRenameGroup_Click(object sender, EventArgs e) {
            var currentName = tvGroups.SelectedNode.Text;
            var newName = GetName("Rename Group", "What do you want to rename this group?", currentName);

            if (string.IsNullOrEmpty(newName)) {
                return;
            }

            foreach (var g in tvGroups.Nodes.Find(currentName, true)) {
                g.Name = newName;
                g.Text = newName;
            }
            tvGroups.Refresh();
        }


        private string GetName(string heading, string prompt, string value) {
            var result = string.Empty;

            using (var groupNameDialog = new TextQueryDialog(heading, prompt, value)) {
                var validName = false;
                while (!validName) {
                    groupNameDialog.ShowDialog();
                    validName = true;
                    if (groupNameDialog.DialogResult != DialogResult.OK) {
                        continue;
                    }

                    var response = groupNameDialog.Response;
                    var msg = string.Empty;
                    if (string.IsNullOrEmpty(response)) {
                        msg = "A group must have a unique name and cannot be blank.";
                    }
                    else if (tvGroups.Nodes.Cast<TreeNode>().Any(n => n.Text == response)) {
                        msg = String.Format("A group with the name {0} already exists and group names must be unique.", response);
                    }
                    else if (response.Contains(Group.GroupTextDivider) || response.Contains(",")) {
                        msg = string.Format("A group name can not contain a {0} or a ,", Group.GroupTextDivider);
                    }

                    if (msg != String.Empty) {
                        if (MessageBox.Show(msg + @"\nClick OK to try again.", @"Group Naming Error", MessageBoxButtons.OKCancel) != DialogResult.OK) {
                            continue;
                        }
                        groupNameDialog.Response = value;
                        validName = false;
                    }
                    else {
                        result = response;
                    }
                }
            }

            return result;
        }


        private void btnAddChild_Click(object sender, EventArgs e) {
            var existingNodes = GetAllNodesFor(tvGroups.SelectedNode);
            var childNodes = tvGroups.Nodes.Cast<TreeNode>().Where(n => !existingNodes.Contains(n.Name)).ToList();

            if (childNodes.Count == 0) {
                MessageBox.Show(@"All possible child nodes have been added", @"No more nodes", MessageBoxButtons.OK);
                return;
            }

            using (var child = new GroupPickerDialog(childNodes)) {
                child.ShowDialog();
                if (child.DialogResult != DialogResult.OK) {
                    return;
                }
                var items = child.SelectedItems;
                var excludedItems = (from item in items
                    let node = tvGroups.Nodes.Find(item, false)[0]
                    let excluded = GetAllNodesFor(node)
                    from exclude in excluded
                    where item != exclude && items.Contains(exclude)
                    select exclude).ToList();
                var rootNode = tvGroups.Nodes.Find(tvGroups.SelectedNode.Name, false)[0];
                foreach (var node in from item in items where !excludedItems.Contains(item) select tvGroups.Nodes.Find(item, false)[0]) {
                    rootNode.Nodes.Insert(rootNode.GetNodeCount(false), (TreeNode) node.Clone());
                }
            }
            UpdateStats();
            tvGroups.Invalidate();
        }


        private static List<string> GetAllNodesFor(TreeNode name) {
            var result = new List<string> {name.Name};
            foreach (TreeNode n in name.Nodes) {
                AddChildren(result, n);
            }
            return result;
        }


        private static void AddChildren(ICollection<string> nodeList, TreeNode node) {
            if (!((GroupTagData) node.Tag).IsLeafNode) {
                nodeList.Add(node.Name);
            }
            foreach (TreeNode n in node.Nodes) {
                AddChildren(nodeList, n);
            }
        }


        private void btnRemoveGroup_Click(object sender, EventArgs e) {
            tvGroups.BeginUpdate();
            foreach (var n in tvGroups.SelectedNodes) {
                n.Remove();
                var affectedNodeText = n.Text;
                if (n.Parent != null) {
                    continue;
                }
                foreach (TreeNode treeNode in tvGroups.Nodes) {
                    RemoveNode(treeNode, affectedNodeText);
                }
            }
            tvGroups.SelectedNode = null;
            UpdateStats();
            tvGroups.EndUpdate();
            tvGroups.Refresh();
        }


        private static void RemoveNode(TreeNode treeNode, String name) {
            foreach (TreeNode node in treeNode.Nodes) {
                if (node.GetNodeCount(false) > 0) {
                    RemoveNode(node, name);
                }
                if (node.Text == name) {
                    node.Remove();
                }
            }
        }


        private void btnAddChannels_Click(object sender, EventArgs e) {
            tvGroups.BeginUpdate();
            foreach (int index in lbChannels.SelectedIndices) {
                var channel = _channels[index];
                foreach (var newNode in
                    tvGroups.SelectedNodes.Select(node => node.Nodes.Add(channel.Name)).Where(
                        newNode => newNode.Nodes.Find(channel.Name, false).Length == 0)) {
                    newNode.Tag = new GroupTagData {
                        IsLeafNode = true, NodeColor = channel.Color,
                        UnderlyingChannel = _fullChannels.IndexOf(channel).ToString(CultureInfo.InvariantCulture)
                    };
                    newNode.Name = channel.Name;
                    AddReferencedNode(newNode);
                }
            }
            UpdateStats();
            tvGroups.EndUpdate();
            tvGroups.Refresh();
        }


        private void AddReferencedNode(TreeNode newNode) {
            foreach (
                var node in
                    tvGroups.Nodes.Find(newNode.Parent.Name, true).Where(
                        node => node.Parent != null && node.Name == newNode.Parent.Name && node.Parent != newNode.Parent).Where(
                            node => node.Nodes.Find(newNode.Name, false).Length == 0)) {
                node.Nodes.Add((TreeNode) newNode.Clone());
            }
        }


        private void btnRemoveChannels_Click(object sender, EventArgs e) {
            tvGroups.BeginUpdate();
            foreach (var node in tvGroups.SelectedNodes) {
                var parent = node.Parent;
                node.Remove();
                RemoveReferencedNodes(parent, node);
            }
            tvGroups.SelectedNode = null;
            UpdateStats();
            tvGroups.EndUpdate();
            tvGroups.Refresh();
        }


        private void RemoveReferencedNodes(TreeNode parent, TreeNode nodeToRemove) {
            foreach (
                var node in
                    from node in tvGroups.Nodes.Find(nodeToRemove.Name, true).Where(node => node.Parent != null && node.Parent.Name == parent.Name)
                    let nodeRemoveTag = nodeToRemove.Tag as GroupTagData
                    let nodeTag = node.Tag as GroupTagData
                    where nodeRemoveTag != null && (nodeTag != null && nodeTag.UnderlyingChannel.Equals(nodeRemoveTag.UnderlyingChannel))
                    select node) {
                node.Remove();
            }
        }


        private void btnUp_Click(object sender, EventArgs e) {
            SwapNodes(-1);
        }


        private void btnDown_Click(object sender, EventArgs e) {
            SwapNodes(1);
        }


        private void SwapNodes(int direction) {
            var root = tvGroups.SelectedNode.Parent == null ? tvGroups.Nodes : tvGroups.SelectedNode.Parent.Nodes;
            var currentPos = root.IndexOf(tvGroups.SelectedNode);
            var currentNode = root[currentPos];
            currentNode.Remove();
            root.Insert(currentPos + direction, currentNode);
            currentNode.EnsureVisible();
            tvGroups.Refresh();
            SetButtons();
        }


        private void cbSort_SelectedIndexChanged(object sender, EventArgs e) {
            // reset to natural order first otherwise the other sorts don't seem
            // to work right, since it sorts on the last sort result
            var result = _channels.OrderBy(x => x.OutputChannel);
            _channels = result.ToList();

            switch ((Sorts) cbSort.SelectedIndex) {
                case Sorts.ChannelColor:
                    result = _channels.OrderBy(x => x.Color.ToArgb());
                    break;
                case Sorts.ChannelName:
                    result = _channels.OrderBy(x => x.Name);
                    break;
                case Sorts.ChannelColorThenName:
                    result = _channels.OrderBy(x => x.Color.ToArgb()).ThenBy(x => x.Name);
                    break;
                case Sorts.ChannelNameThenColor:
                    result = _channels.OrderBy(x => x.Name).ThenBy(x => x.Color.ToArgb());
                    break;
            }

            lbChannels.Items.Clear();
            if ((Sorts) cbSort.SelectedIndex != Sorts.Natural) _channels = result.ToList();
            foreach (var c in _channels) {
                lbChannels.Items.Add(c.Name);
            }
        }


        private void btnAddMutli_Click(object sender, EventArgs e) {
            using (var dialog = new GroupDialogMultiAdd()) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                var name = dialog.GroupName;
                var count = dialog.GroupCount;
                var added = 1;
                var skipped = 0;
                while (added < count + 1) {
                    var groupName = name + (added + skipped);
                    if (tvGroups.Nodes.Find(groupName, false).Length != 0) {
                        skipped++;
                        continue;
                    }
                    CreateGroup(groupName);
                    added++;
                }
            }
            tvGroups.Refresh();
            UpdateStats();
        }


        private void lbChannels_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right || !btnAddChannels.Enabled)
                return;

            lbChannels.DoDragDrop(lbChannels.SelectedItems, DragDropEffects.Copy);
        }


        private void tvGroups_DragDrop(object sender, DragEventArgs e) {
            btnAddChannels_Click(null, null);
        }


        private void tvGroups_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }


        private void tvGroups_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
