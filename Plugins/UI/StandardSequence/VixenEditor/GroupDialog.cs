using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using CommonControls;

using VixenPlus;

namespace VixenEditor {
    public sealed partial class GroupDialog : Form {
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
                thisNode.Tag = g.Value.GroupColor;
            }
            tvGroups.ExpandAll();
        }


        private void SetButtons() {
            //System.Diagnostics.Debug.Print( SystemInformation.MenuAccessKeysUnderlined.ToString());
            var channelSelected = lbChannels.SelectedItems.Count > 0;
            var groupSelected = tvGroups.SelectedNode != null;
            var groupIsRoot = groupSelected && tvGroups.SelectedNode.Parent == null;
            var topNode = groupSelected && tvGroups.SelectedNode == tvGroups.Nodes[0];
            var bottomNode = groupSelected && tvGroups.SelectedNode == tvGroups.Nodes[tvGroups.Nodes.Count - 1];

            btnAddRoot.Enabled = true;
            btnRemove.Enabled = groupIsRoot;
            btnRename.Enabled = groupSelected;
            btnColor.Enabled = groupSelected;

            btnUp.Enabled = groupSelected && !topNode;
            btnDown.Enabled = groupSelected && !bottomNode;

            btnCopyChannels.Enabled = channelSelected;
            btnRemoveChannels.Enabled = channelSelected;
        }

        private void AddSubNodes(string nodeData, TreeNode parentNode) {
            if (nodeData.Contains(Group.GroupTextDivider.ToString(CultureInfo.InvariantCulture))) {
                var groupData = nodeData.Split(Group.GroupTextDivider);
                foreach (var group in groupData[0].Split(new[] {','})) {
                    var thisNode = parentNode.Nodes.Add(group);
                    thisNode.Tag = _seq.Groups[group].GroupColor;
                    AddSubNodes(_seq.Groups[group].GroupChannels, thisNode);
                }
            }
            else {
                if (nodeData == "") {
                    return;
                }
                foreach (var channel in nodeData.Split(new[] {','})) {
                    var thisNode = parentNode.Nodes.Add(_seq.FullChannels[int.Parse(channel)].Name);
                    thisNode.Tag = _seq.FullChannels[int.Parse(channel)].Color;
                }
            }
        }


        private void lbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _seq.Channels[e.Index]);
        }


        private void tvGroups_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            var treeView = sender as MultiSelectTreeview;
            Channel.DrawItem(treeView, e, (Color) e.Node.Tag);
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
                if (color.ShowDialog() == DialogResult.OK) {}
            }
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
            //const int heightOffset = 92;
            //lbChannels.Height = Height - heightOffset;
            //tvGroups.Height = Height - heightOffset;

            //const int widthOffset = 26;
            //const int margin = 13;
            //lbChannels.Width = btnUp.Location.X - widthOffset;
            //tvGroups.Location = new Point(btnUp.Location.X + btnUp.Width + margin, tvGroups.Location.Y);
            //tvGroups.Width = Width - (btnUp.Location.X + btnUp.Width) - widthOffset - margin;
        }


        private void GroupDialog_ResizeBegin(object sender, EventArgs e) {
            lbChannels.BeginUpdate();
            tvGroups.BeginUpdate();
        }


        private void GroupDialog_ResizeEnd(object sender, EventArgs e) {
            lbChannels.EndUpdate();
            tvGroups.EndUpdate();
        }

        private void btnAddGroup_Click(object sender, EventArgs e) {

        }
    }
}
