using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;



using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public partial class ProfileManagerDialog : Form {
        private readonly List<int> _channelOrderMapping;
        private readonly SolidBrush _pictureBrush = new SolidBrush(Color.Black);
        private readonly Font _pictureFont = new Font("Arial", 13f, FontStyle.Bold);
        private readonly Pen _picturePen = new Pen(Color.Black, 2f);
        private Profile _contextProfile;
        private readonly bool _useCheckmark = Preference2.GetInstance().GetBoolean("UseCheckmark");

        public ProfileManagerDialog(object objectInContext) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            tabControl.OurMultiline = true;
            foreach (var str in Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.ProfileExtension)) {
                // ReSharper disable EmptyGeneralCatchClause
                try {
                    listBoxProfiles.Items.Add(new Profile(str));
                }
                catch (Exception) {
                    // Empty catch since we don't want to bomb on attempting to add a profile.
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
            _channelOrderMapping = new List<int>();
            var profile = objectInContext as Profile;
            if (profile != null) {
                var profileListed = listBoxProfiles.Items.Cast<object>().Any(item => ((Profile) item).Name == profile.Name);
                if (!profileListed) {
                    listBoxProfiles.Items.Add(profile);
                }
                EditProfile(profile);
            }
            else {
                tabControl.SelectedTab = tabProfiles;
            }
        }


        private void AddProfileChannel() {
            var channelNum = treeViewProfile.Nodes.Count + 1;
            var channelObject = new Channel(Resources.ChannelSpace + channelNum.ToString(CultureInfo.InvariantCulture), 0);
            _contextProfile.AddChannelObject(channelObject);
            treeViewProfile.Nodes.Add(channelObject.Name).Tag = channelObject;
            _channelOrderMapping.Add(_channelOrderMapping.Count);
        }


        private void buttonAddMultipleProfileChannels_Click(object sender, EventArgs e) {
            int channelsToAdd;
            if (!Int32.TryParse(textBoxProfileChannelCount.Text, out channelsToAdd)) {
                MessageBox.Show(
                    String.Format(Resources.InvalidChannelsToAdd, textBoxProfileChannelCount.Text),
                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            treeViewProfile.BeginUpdate();
            for (var i = 0; i < channelsToAdd; i++) {
                AddProfileChannel();
            }
            treeViewProfile.EndUpdate();
        }


        private void buttonAddProfileChannel_Click(object sender, EventArgs e) {
            AddProfileChannel();
        }


        private void buttonChangeProfileName_Click(object sender, EventArgs e) {
            if (!ChangeProfileName()) {
                return;
            }

            labelProfileName.Text = _contextProfile.Name;
            _contextProfile.IsDirty = true;
        }


        private void buttonDone_Click(object sender, EventArgs e) {
            foreach (Profile profile in listBoxProfiles.Items) {
                if (!(profile.FileName != string.Empty || ChangeProfileName())) {
                    DialogResult = DialogResult.None;
                    break;
                }
                if (profile.IsDirty) {
                    profile.SaveToFile();
                }
            }
        }


        private void buttonPlugins_Click(object sender, EventArgs e) {
            using (var dialog = new PluginListDialog(_contextProfile)) {
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK) {
                    _contextProfile.IsDirty = true;
                }
            }
        }


        private void buttonRemoveProfileChannels_Click(object sender, EventArgs e) {
            RemoveSelectedProfileChannelObjects();
        }


        private bool ChangeProfileName() {
            var nameChanged = false;
            var newName = string.Empty;

            using (var dialog = new TextQueryDialog(Resources.ProfileNamePrompt, Resources.NameThisProfile, _contextProfile.Name)) {
                var showDialog = true;
                while (showDialog) {
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        newName = dialog.Response;
                        showDialog = false;
                        nameChanged = true;
                        if (!File.Exists(Path.Combine(Paths.ProfilePath, newName + Vendor.ProfileExtension)) &&
                            !File.Exists(Path.Combine(Paths.RoutinePath, newName + Vendor.GroupExtension))) {
                            continue;
                        }
                        var overwriteResult = MessageBox.Show(
                            String.Format("Profile or Group with the name {0} exists.  Overwrite this profile?", newName), "Overwrite?",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        switch (overwriteResult) {
                            case DialogResult.Yes:
                                break;
                            case DialogResult.No:
                                nameChanged = false;
                                showDialog = true;
                                break;
                            case DialogResult.Cancel:
                                nameChanged = false;
                                break;
                        }
                    }
                    else {
                        showDialog = false;
                    }
                }
            }

            if (!nameChanged) {
                return false;
            }

            if (!string.IsNullOrEmpty(_contextProfile.FileName)) {
                if (File.Exists(_contextProfile.FileName)) {
                    File.Delete(_contextProfile.FileName);
                }

                var root = Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath;

                var groupName = Path.Combine(root,
                    (Path.GetFileNameWithoutExtension(_contextProfile.FileName)) + Vendor.GroupExtension);
                if (File.Exists(groupName)) {
                    File.Delete(groupName);
                }
            }
            _contextProfile.Name = newName;
            _contextProfile.IsDirty = true;

            return true;
        }


        private void comboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBoxChannelOrder.SelectedIndex == -1) {
                return;
            }

            var channels = _contextProfile.Channels;
            if (comboBoxChannelOrder.SelectedIndex == 0) {
                if (channels.Count == 0) {
                    MessageBox.Show(Resources.NoChannelsToReorder, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                btnDeleteChannelOrder.Enabled = false;
                comboBoxChannelOrder.SelectedIndex = -1;
                using (var dialog = new ChannelOrderDialog(channels, _channelOrderMapping)) {
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        _channelOrderMapping.Clear();
                        foreach (var channel in dialog.ChannelMapping) {
                            _channelOrderMapping.Add(channels.IndexOf(channel));
                        }
                        _contextProfile.IsDirty = true;
                    }
                }
            }
            else if (comboBoxChannelOrder.SelectedIndex == (comboBoxChannelOrder.Items.Count - 1)) {
                btnDeleteChannelOrder.Enabled = false;
                comboBoxChannelOrder.SelectedIndex = -1;
                _channelOrderMapping.Clear();
                for (var i = 0; i < channels.Count; i++) {
                    _channelOrderMapping.Add(i);
                }
                _contextProfile.LastSort = -1;
                _contextProfile.IsDirty = true;
            }
            else {
                _channelOrderMapping.Clear();
                _channelOrderMapping.AddRange(((SortOrder) comboBoxChannelOrder.SelectedItem).ChannelIndexes);
                _contextProfile.LastSort = comboBoxChannelOrder.SelectedIndex;
                btnDeleteChannelOrder.Enabled = true;
                _contextProfile.IsDirty = true;
            }
            ReloadProfileChannelObjects();
        }


        private void EditProfile(Profile profile) {
            _contextProfile = profile;
            labelProfileName.Text = profile.Name;
            UpdateSortList();
            ReloadProfileChannelObjects();
            comboBoxChannelOrder.SelectedIndex = _contextProfile.LastSort;
            tabEditProfile.Tag = tabControl.SelectedTab;
            tabControl.SelectedTab = tabEditProfile;
        }


        private void listBoxProfiles_DoubleClick(object sender, EventArgs e) {
            if (listBoxProfiles.SelectedIndex != -1) {
                EditProfile((Profile) listBoxProfiles.SelectedItem);
            }
        }


        private void listBoxProfiles_KeyDown(object sender, KeyEventArgs e) {
            if ((listBoxProfiles.SelectedIndex != -1) && (e.KeyCode == Keys.Delete)) {
                RemoveProfile((Profile) listBoxProfiles.SelectedItem);
            }
        }


        private void listBoxProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            btnEditProfile.Enabled = btnRemoveProfile.Enabled = listBoxProfiles.SelectedIndex != -1;
        }


        private void pictureBoxAddProfile_Click(object sender, EventArgs e) {
            using (var dialog = new TextQueryDialog(Resources.NewProfilePrompt, Resources.NameNewProfile, string.Empty)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                var item = new Profile {Name = dialog.Response};
                listBoxProfiles.Items.Add(item);
                EditProfile(item);
            }
        }

        private void pictureBoxEditProfile_Click(object sender, EventArgs e) {
            EditProfile((Profile) listBoxProfiles.SelectedItem);
        }


        private void pictureBoxProfileChannelColors_Click(object sender, EventArgs e) {
            var channels = _contextProfile.Channels;
            using (var dialog = new AllChannelsColorDialog(channels)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                var channelColors = dialog.ChannelColors;
                for (var i = 0; i < channels.Count; i++) {
                    channels[i].Color = channelColors[i];
                }
                treeViewProfile.Refresh();
            }
        }


        private void pictureBoxProfileChannelOutputMask_Click(object sender, EventArgs e) {
            var channels = _contextProfile.Channels;
            using (var dialog = new ChannelOutputMaskDialog(channels)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                foreach (var channel in channels) {
                    channel.Enabled = true;
                }
                foreach (var channelNum in dialog.DisabledChannels) {
                    channels[channelNum].Enabled = false;
                }
            }
        }


        private void pictureBoxProfileChannelOutputs_Click(object sender, EventArgs e) {
            using (var dialog = new ChannelOrderDialog(_contextProfile.OutputChannels, null, Resources.ChannelCaption)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    _contextProfile.OutputChannels = dialog.ChannelMapping;
                }
            }
        }


        private void pictureBoxProfileDeleteChannelOrder_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show(string.Format(Resources.DeleteChannelOrder, comboBoxChannelOrder.Text), Vendor.ProductName, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }
            _contextProfile.Sorts.Remove((SortOrder) comboBoxChannelOrder.SelectedItem);
            comboBoxChannelOrder.Items.RemoveAt(comboBoxChannelOrder.SelectedIndex);
            btnDeleteChannelOrder.Enabled = false;
        }


        private void pictureBoxProfileSaveChannelOrder_Click(object sender, EventArgs e) {
            SortOrder sortOrder = null;
            using (var dialog = new TextQueryDialog(Resources.NewOrderPrompt, Resources.ChannelOrderingName, string.Empty)) {
                var dialogResult = DialogResult.No;
                while (dialogResult == DialogResult.No) {
                    if (dialog.ShowDialog() == DialogResult.Cancel) {
                        return;
                    }
                    dialogResult = DialogResult.Yes;
                    foreach (var sort in _contextProfile.Sorts) {
                        if (sort.Name != dialog.Response) {
                            continue;
                        }
                        if (
                            (dialogResult =
                             MessageBox.Show(Resources.OrderNameOverwrite, Vendor.ProductName,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Cancel) {
                            return;
                        }
                        sortOrder = sort;
                        break;
                    }
                }
                if (sortOrder != null) {
                    sortOrder.ChannelIndexes.Clear();
                    sortOrder.ChannelIndexes.AddRange(_channelOrderMapping);
                    comboBoxChannelOrder.SelectedItem = sortOrder;
                }
                else {
                    _contextProfile.Sorts.Add(sortOrder = new SortOrder(dialog.Response, _channelOrderMapping));
                    sortOrder.ChannelIndexes.Clear();
                    sortOrder.ChannelIndexes.AddRange(_channelOrderMapping);
                    comboBoxChannelOrder.Items.Insert(comboBoxChannelOrder.Items.Count - 1, sortOrder);
                    comboBoxChannelOrder.SelectedIndex = comboBoxChannelOrder.Items.Count - 2;
                }
            }
        }


        private void pictureBoxRemoveProfile_Click(object sender, EventArgs e) {
            RemoveProfile((Profile) listBoxProfiles.SelectedItem);
        }


        private void pictureBoxReturnFromChannelGroupEdit_Click(object sender, EventArgs e) {
            if (tabControl.SelectedTab.Tag == tabProfiles) {
                UpdateProfiles();
            }
            else if (tabControl.SelectedTab.Tag == tabEditProfile) {
                ReloadProfileChannelObjects();
            }
            tabControl.SelectedTab = (TabPage) tabControl.SelectedTab.Tag;
        }


        private void ReloadProfileChannelObjects() {
            var index = -1;
            if (treeViewProfile.SelectedNode != null) {
                if (treeViewProfile.SelectedNode.Level == 0) {
                    if (treeViewProfile.SelectedNode.IsExpanded) {
                        index = treeViewProfile.SelectedNode.Index;
                    }
                }
                else if ((treeViewProfile.SelectedNode.Level == 1) && treeViewProfile.SelectedNode.Parent.IsExpanded) {
                    index = treeViewProfile.SelectedNode.Parent.Index;
                }
            }
            buttonRemoveProfileChannels.Enabled = false;
            treeViewProfile.BeginUpdate();
            treeViewProfile.Nodes.Clear();
            var channels = _contextProfile.Channels;
            foreach (var channelNum in _channelOrderMapping) {
                var channel = channels[channelNum];
                treeViewProfile.Nodes.Add(channel.Name).Tag = channel;
            }
            if (index != -1) {
                treeViewProfile.Nodes[index].Expand();
            }
            treeViewProfile.EndUpdate();
        }


        private void RemoveProfile(IExecutable profile) {
            if (
                MessageBox.Show(string.Format(Resources.RemoveProfile, profile.Name),
                                Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }
            File.Delete(profile.FileName);
            var groupName = Path.GetFileNameWithoutExtension(profile.FileName) + Vendor.GroupExtension;
            if (File.Exists(groupName)) {
                File.Delete(groupName);
            }
            listBoxProfiles.Items.Remove(profile);
        }


        private void RemoveSelectedProfileChannelObjects() {
            if (treeViewProfile.SelectedNode.Level != 0) {
                buttonRemoveProfileChannels.Enabled = false;
            }
            else if (
                MessageBox.Show(Resources.RemoveProfileItem, Vendor.ProductName, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes) {
                _channelOrderMapping.RemoveAt(_contextProfile.Channels.IndexOf((Channel) treeViewProfile.SelectedNode.Tag));
                _contextProfile.RemoveChannel((Channel) treeViewProfile.SelectedNode.Tag);
                treeViewProfile.Nodes.Remove(treeViewProfile.SelectedNode);
                treeViewProfile.SelectedNode = null;
                treeViewProfile_AfterSelect(null, null);
                _contextProfile.IsDirty = true;
            }
        }


        private void treeViewProfile_AfterSelect(object sender, TreeViewEventArgs e) {
            buttonRemoveProfileChannels.Enabled = (treeViewProfile.SelectedNode != null) && (e.Node.Level == 0);
        }


        private void treeViewProfile_DoubleClick(object sender, EventArgs e) {
            if (treeViewProfile.SelectedNode == null) {
                return;
            }

            var tag = (Channel) treeViewProfile.SelectedNode.Tag;
            var channels = (from TreeNode node in treeViewProfile.Nodes
                            select (Channel) node.Tag).ToList();
            using (var dialog = new ChannelPropertyDialog(channels, tag, false)) {
                dialog.ShowDialog();
                ReloadProfileChannelObjects();
                _contextProfile.IsDirty = true;
            }
        }


        private void treeViewProfile_KeyDown(object sender, KeyEventArgs e) {
            if ((treeViewProfile.SelectedNode != null) && (e.KeyCode == Keys.Delete)) {
                RemoveSelectedProfileChannelObjects();
            }
        }

        
        private void UpdateProfiles() {
            var profiles = listBoxProfiles.Items.Cast<Profile>().ToList();
            listBoxProfiles.SelectedIndex = -1;
            listBoxProfiles.BeginUpdate();
            listBoxProfiles.Items.Clear();
            foreach (var profile in profiles) {
                listBoxProfiles.Items.Add(profile);
            }
            listBoxProfiles.EndUpdate();
        }


        private void UpdateSortList() {
            comboBoxChannelOrder.BeginUpdate();
            var defineNewOrder = (string) comboBoxChannelOrder.Items[0];
            var restoreNaturalOrder = (string) comboBoxChannelOrder.Items[comboBoxChannelOrder.Items.Count - 1];
            comboBoxChannelOrder.Items.Clear();
            comboBoxChannelOrder.Items.Add(defineNewOrder);
            foreach (var sort in _contextProfile.Sorts) {
                comboBoxChannelOrder.Items.Add(sort);
            }
            comboBoxChannelOrder.Items.Add(restoreNaturalOrder);
            comboBoxChannelOrder.EndUpdate();
            var count = _contextProfile.Channels.Count;
            _channelOrderMapping.Clear();
            for (var i = 0; i < count; i++) {
                _channelOrderMapping.Add(i);
            }
        }


        private void treeViewProfile_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            var treeView = sender as TreeView;
            e.DrawItem(_contextProfile.Channels[_contextProfile.FullChannels.IndexOf((Channel)e.Node.Tag)].Color, treeView, _useCheckmark);
        }
    }
}
