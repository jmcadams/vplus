using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonControls;

using CommonUtils;

using VixenPlus;
using VixenPlus.Dialogs;

namespace VixenEditor {
    public sealed partial class GroupDialog : Form {

        private readonly EventSequence _seq;


        public TreeView GetResults { get { return tvGroups; } }


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
                thisNode.Name = g.Key;
                thisNode.Tag = new GroupTagData {NodeColor = g.Value.GroupColor, IsLeafNode = false};
            }
        }


        private void AddSubNodes(string nodeData, TreeNode parentNode) {
            foreach (var node in nodeData.Split(new[] {','})) {
                if (node.StartsWith(Group.GroupTextDivider)) {
                    var groupNode = node.TrimStart(Group.GroupTextDivider.ToCharArray());
                    var thisNode = parentNode.Nodes.Add(groupNode);
                    thisNode.Name = groupNode;
                    thisNode.Tag = new GroupTagData { NodeColor = _seq.Groups[groupNode].GroupColor, IsLeafNode = false };
                    AddSubNodes(_seq.Groups[groupNode].GroupChannels, thisNode);
                }
                else {
                    var channel = _seq.FullChannels[int.Parse(node)];
                    var thisNode = parentNode.Nodes.Add(channel.Name);
                    thisNode.Name = channel.Name;
                    thisNode.Tag = new GroupTagData { NodeColor = channel.Color, IsLeafNode = true };
                }
            }
        }


        private void SetButtons() {
            var activeNode = tvGroups.SelectedNode;
            var isNodeActive = activeNode != null;
            var isSingleNode = tvGroups.SelectedNodes.Count == 1;
            var isRootNode = isNodeActive && activeNode.Parent == null;
            var isLeafNode = isNodeActive && ((GroupTagData)activeNode.Tag).IsLeafNode;
            var isChannelSelected = lbChannels.SelectedItems.Count > 0;

            // These two will need to be more complex since we can only move up or down so far, basically to the top
            // or bottom of the top? parent.
            var topNode = isNodeActive && tvGroups.SelectedNode == tvGroups.Nodes[0];
            var bottomNode = isNodeActive && tvGroups.SelectedNode == tvGroups.Nodes[tvGroups.Nodes.Count - 1];

            var isOnlyLeafNodes = true;
            var isParentAtRoot = true;
            var isRemovable = true;
            foreach (var n in tvGroups.SelectedNodes) {
                isOnlyLeafNodes &= ((GroupTagData) n.Tag).IsLeafNode;
                isParentAtRoot &= (n.Parent == null || !n.Parent.FullPath.Contains("\\"));
                isRemovable &= n.FullPath.Split(new[] {'\\'}).Length - 1 < 2;
            }


            // Add Root is always available
            btnAddRoot.Enabled = true;

            // Add child is only availabe to a root node
            btnAddChild.Enabled = isRootNode && isSingleNode;

            // Removed group available to any non leaf node/nodes - cascade to lower level non leaf node if root node removed
            btnRemoveGroup.Enabled = isNodeActive && !isLeafNode && isRemovable; 

            // Rename group at root node - cascade to other lowel level non leaf nodes
            btnRenameGroup.Enabled = isRootNode && isSingleNode;

            // Color group at root node/nodes
            btnColorGroup.Enabled = isRootNode;

            // For any node/nodes if not at the top of its hierarchy and are all part of the same (top parent?)
            btnUp.Enabled = isNodeActive && !topNode && isParentAtRoot;

            // For any node/nodes if not at the bottom of its hierarchy and are all part of the same (top parent?)
            btnDown.Enabled = isNodeActive && !bottomNode && isParentAtRoot;

            // If a channel/channels are selected
            btnAddChannels.Enabled = isChannelSelected;

            // If only channels are selected and parent is a root node
            btnRemoveChannels.Enabled = isOnlyLeafNodes && isParentAtRoot;
        }


        private void lbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _seq.FullChannels[e.Index], true);
        }


        private void tvGroups_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            var treeView = sender as MultiSelectTreeview;
            Utils.DrawItem(treeView, e, ((GroupTagData)e.Node.Tag).NodeColor);
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
                tvGroups.BeginUpdate();
                foreach (var s in tvGroups.SelectedNodes) {
                    var affectedNodeText = s.Text;
                    foreach (TreeNode treeNode in tvGroups.Nodes) {
                        SetColorByName(treeNode, color.Color, affectedNodeText);
                    }
                }
                tvGroups.EndUpdate();
            }
        }


        private void SetColorByName(TreeNode treeNode, Color color, String name) {
            if (treeNode.Text == name && !((GroupTagData)treeNode.Tag).IsLeafNode) {
                SetNodeColor(treeNode, color);
            }

            foreach (TreeNode node in treeNode.Nodes) {
                if (node.Text == name && !((GroupTagData)treeNode.Tag).IsLeafNode) {
                    SetNodeColor(node, color);
                }
                SetColorByName(node, color, name);
            }
        }


        private void SetNodeColor(TreeNode node, Color color) {
            ((GroupTagData)node.Tag).NodeColor = color;
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
            var result = GetName("Group Name", "What do you want to name your new group?", "");
            if (string.IsNullOrEmpty(result)) {
                return;
            }

            var thisNode = tvGroups.Nodes.Add(result);
            thisNode.Tag = new GroupTagData {
                IsLeafNode = false,
                NodeColor = Color.White
            };
            thisNode.Name = result;
            thisNode.EnsureVisible();
            tvGroups.Refresh();
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

            using (var name = new TextQueryDialog(heading, prompt, value)) {
                var done = false;
                while (!done) {
                    name.ShowDialog();
                    done = true;
                    if (name.DialogResult != DialogResult.OK) {
                        continue;
                    }

                    var newName = name.Response;
                    var msg = string.Empty;
                    if (string.IsNullOrEmpty(newName)) {
                        msg = "A group must have a unique name and cannot be blank.";
                    }
                    else {
                        if (tvGroups.Nodes.Cast<TreeNode>().Any(n => n.Text == newName)) {
                            msg = String.Format("A node with the name {0} already exists and group names must be unique.", newName);
                        }
                    }

                    if (msg != String.Empty) {
                        MessageBox.Show(msg, "Group Naming Error", MessageBoxButtons.OK);
                        name.Response = value;
                        done = false;
                    }
                    else {
                        result = newName;
                    }
                }
            }

            return result;
        }


        private void btnAddChild_Click(object sender, EventArgs e) {
            var existingNodes = GetAllNodesFor(tvGroups.SelectedNode);
            var childNodes = tvGroups.Nodes.Cast<TreeNode>().Where(n => !existingNodes.Contains(n.Name)).ToList();

            if (childNodes.Count == 0) {
                MessageBox.Show("All possible child nodes have been added", "No more nodes", MessageBoxButtons.OK);
                return;
            }

            using (var child = new GroupPickerDialog(childNodes)) {
                child.ShowDialog();
                if (child.DialogResult != DialogResult.OK) {
                    return;
                }
                var items = child.SelectedItems;
                //var excludedItems = new List<string>();
                //foreach (var item in items) {
                //    var node = tvGroups.Nodes.Find(item, false)[0];
                //    var excluded = GetAllNodesFor(node);
                //    foreach (var exclude in excluded) {
                //        if (item != exclude && items.Contains(exclude)) {
                //            excludedItems.Add(exclude);
                //        }
                //    }
                //}
                //var rootNode = tvGroups.Nodes.Find(tvGroups.SelectedNode.Name, false)[0];
                //foreach (var item in items) {
                //    if (excludedItems.Contains(item)) {
                //        continue;
                //    }
                var excludedItems = (from item in items
                                     let node = tvGroups.Nodes.Find(item, false)[0]
                                     let excluded = GetAllNodesFor(node)
                                     from exclude in excluded
                                     where item != exclude && items.Contains(exclude)
                                     select exclude).ToList();
                var rootNode = tvGroups.Nodes.Find(tvGroups.SelectedNode.Name, false)[0];
                foreach (var node in from item in items
                                     where !excludedItems.Contains(item)
                                     select tvGroups.Nodes.Find(item, false)[0]) {
                    rootNode.Nodes.Insert(rootNode.GetNodeCount(false), (TreeNode)node.Clone());
                }
            }
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
            foreach (TreeNode n in node.Nodes)
            {
                AddChildren(nodeList, n);
            }
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e) {
            //tvGroups.BeginUpdate();
            //foreach (var n in tvGroups.SelectedNodes) {
            //    n.Remove();
            //    var affectedNodeText = n.Text;
            //    foreach (TreeNode treeNode in tvGroups.Nodes) {
            //        SetColorByName(treeNode, color.Color, affectedNodeText);
            //    }
            //}
            //tvGroups.EndUpdate();
            //tvGroups.Refresh();
        }

        private void RemoveNode(TreeNode treeNode, String name) {
            //if (treeNode.Text == name && !((GroupTagData)treeNode.Tag).IsLeafNode) {
            //    SetNodeColor(treeNode, color);
            //}

            //foreach (TreeNode node in treeNode.Nodes) {
            //    if (node.Text == name && !((GroupTagData)treeNode.Tag).IsLeafNode) {
            //        SetNodeColor(node, color);
            //    }
            //    SetColorByName(node, color, name);
            //}
        }
    }
}
