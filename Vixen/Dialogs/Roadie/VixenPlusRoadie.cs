using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using VixenPlusCommon;
using VixenPlusCommon.Properties;

namespace VixenPlus.Dialogs {
    public partial class VixenPlusRoadie : Form {
        private readonly bool _suppressErrors;
        private bool _isPluginsOnly;

        private IExecutable _contextProfile;

        private const string TabChannels = "tpChannels";
        private const string TabPlugins = "tpPlugins";
        private const string TabGroups = "tpGroups";

        private const string Warning =
            "\n\nNOTE: While most functions can be undone by pressing cancel in the Profile Manager dialog, this one cannot.";


        public VixenPlusRoadie(IExecutable iExecutable = null, bool pluginsOnly = false) {
            InitializeComponent();
            Text = Vendor.ProductName + " - " + Vendor.ModuleManager;
            Icon = Resources.VixenPlus;
            MinimumSize = Size;

            _suppressErrors = Preference2.GetInstance().GetBoolean("SilenceProfileErrors");

            _isPluginsOnly = pluginsOnly;

            if (!_isPluginsOnly && null != iExecutable && string.IsNullOrEmpty(iExecutable.Name)) {
                AddProfile((Profile) iExecutable);
            }

            InitializeControls();

            if (_isPluginsOnly) {
                tcProfile.TabPages.RemoveByKey(TabChannels);
                _contextProfile = iExecutable;
                gbProfiles.Visible = false;
            }

            if (null != iExecutable && !_isPluginsOnly) {
                SetProfileIndex(iExecutable.Name);
            }
            else {
                cbProfiles.SelectedIndex = 0;
            }
        }


        public override sealed string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }


        public override sealed Size MinimumSize {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        public string ProfileFileName {
            get { return _contextProfile == null ? null : _contextProfile.FileName; }
        }


        private void frmProfileManager_Resize(object sender, EventArgs e) {
            var clientSize = tcProfile.ClientSize;

            ResizePanel(pChannels, clientSize);
            ResizePanel(pPlugIns, clientSize);
            ResizePanel(pGroups, clientSize);
        }


        private void ResizePanel(Control panel, Size clientSize) {
            panel.Size = clientSize;
            foreach (Control c in panel.Controls) {
                c.Size = clientSize;
            }
        }


        private void tcProfile_SelectedIndexChanged(object sender, EventArgs e) {
            InitializeTabData();
            DoButtonManagement();
        }


        private void btnProfileAdd_Click(object sender, EventArgs e) {
            AddProfile();
        }


        private void AddProfile(Profile profileData = null) {
            var isNew = profileData == null;

            var newName = GetFileName("Profile Name", Paths.ProfilePath, new[] {Vendor.ProfileExtension}, "", "Create");

            if (string.Empty == newName) {
                return;
            }

            var root = (null != _contextProfile && _contextProfile.FileName != null)
                ? Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath : Paths.ProfilePath;

            var newFileName = Path.Combine(root, newName + Vendor.ProfileExtension);

            DeleteIfExists(newFileName);

            var profile = isNew ? new Profile() : profileData;
            profile.FileName = newFileName;
            profile.Name = newName;
            profile.FileIOHandler = FileIOHelper.GetNativeHelper();
            SaveProfile(profile);

            RefreshProfileComboBox(newName);
        }


        private static void SaveProfile(Profile p) {
            p.FileIOHandler.SaveProfile(p);

        }


        private void btnProfileCopy_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(false);
        }


        private void btnProfileDelete_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this profile?" + Warning, "Delete Profile", MessageBoxButtons.YesNo) !=
                DialogResult.Yes) {
                return;
            }

            DeleteIfExists(_contextProfile.FileName);
            InitializeProfiles();
            cbProfiles.SelectedIndex = 0;
        }


        private void btnOkay_Click(object sender, EventArgs e) {
            //ClearSetup();
            SaveChangedProfiles();
            DialogResult = DialogResult.OK;
        }


        private void btnProfileRename_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(true);
        }


        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            PersistProfileInfo();
            tcProfile.Visible = cbProfiles.SelectedIndex > 0 || _isPluginsOnly;

            if (0 == cbProfiles.SelectedIndex && !_isPluginsOnly) {
                _contextProfile = null;
                _isPluginsOnly = false;
                return;
            }

            if (!_isPluginsOnly) {
                _contextProfile = (Profile) cbProfiles.SelectedItem;
            }

            InitializeTabData();
            DoButtonManagement();
        }


        private void InitializeTabData() {
            switch (tcProfile.SelectedTab.Name) {
                case TabChannels:
                    InitializeChannelTab();
                    break;
                case TabPlugins:
                    InitializePlugInTab();
                    break;
                case TabGroups:
                    InitializeGroupsTab();
                    break;
            }
        }


        private void PersistProfileInfo() {
            if (_isPluginsOnly) {
                var pro = _contextProfile as EventSequence;
                if (pro == null) {
                    return;
                }

                foreach (var dialog in pGroups.Controls.OfType<GroupsTab>()) {
                    pro.Groups = Group.GetGroups(dialog.GetResults);
                }
                return;
            }

            var cp = _contextProfile as Profile;
            if (null == cp) {
                return;
            }

            // Process channels - this should be being done already...
            //cp.ClearChannels();
            //foreach (var ch in from DataGridViewRow row in dgvChannels.Rows
            //    select
            //        new Channel(row.Cells[ChannelNameCol].Value.ToString(), row.DefaultCellStyle.BackColor,
            //            int.Parse(row.Cells[OutputChannelCol].Value.ToString()) - 1)
            //        {Enabled = bool.Parse(row.Cells[ChannelEnabledCol].Value.ToString())}) {
            //    cp.AddChannelObject(ch);
            //}

            // Process Groups
            foreach (var dialog in pGroups.Controls.OfType<GroupsTab>()) {
                cp.Groups = Group.GetGroups(dialog.GetResults);
            }
        }



        private void DoButtonManagement() {
            var isProfileLoaded = _contextProfile != null;
            SetGeneralButtons(isProfileLoaded);
        }


        private void SetGeneralButtons(bool isProfileLoaded = true) {
            var isChannelPanel = pChannels.Controls.Count > 0;
            btnCancel.Enabled = isChannelPanel;
            btnOkay.Enabled = isChannelPanel;
            btnProfileAdd.Enabled = isChannelPanel;
            btnProfileCopy.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileDelete.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileRename.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileSave.Enabled = !_isPluginsOnly && _contextProfile != null && ((Profile) _contextProfile).IsDirty;
        }


        private static void CopyFile(string originalFile, string newFile) {
            if (!File.Exists(originalFile)) {
                return;
            }

            DeleteIfExists(newFile);
            try {
                File.Copy(originalFile, newFile);
            }
            catch (Exception e) {
                e.Message.ShowIoError("Error copying file");
            }
        }


        private static void DeleteIfExists(string fileName) {
            if (!File.Exists(fileName)) {
                return;
            }

            try {
                File.Delete(fileName);
            }
            catch (Exception e) {
                e.Message.ShowIoError("Error deleting file");
            }
        }



        private void InitializeControls() {
            InitializeProfiles();
        }


        private void InitializeProfiles(bool reload = false) {
            var errors = new StringBuilder();
            cbProfiles.Items.Clear();
            cbProfiles.Items.Add("Select or add a profile");
            foreach (var profileFile in
                Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.ProfileExtension).Where(
                    profileFile => Path.GetExtension(profileFile) == Vendor.ProfileExtension)) {
                try {
                    var nativeIO = FileIOHelper.GetNativeHelper();
                    cbProfiles.Items.Add(nativeIO.OpenProfile(profileFile));
                }
                catch (XmlException e) {
                    errors.AppendLine(string.Format("{0}\nFailed to load because: {1}\n", profileFile, e.Message));
                }
            }

            if (errors.Length <= 0 || _suppressErrors || reload) {
                return;
            }

            errors.AppendLine(
                "You will continue to see this message until the offending files are fixed, moved or deleted. You can also supress this message in preferences by selecting 'Silence profile editor errors'");
            MessageBox.Show(errors.ToString(), "Errors loading some profiles");
        }


        private static void RenameFile(string oldFile, string newFile) {
            if (!File.Exists(oldFile)) {
                return;
            }

            DeleteIfExists(newFile);
            try {
                File.Move(oldFile, newFile);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message, "Error renaming file");
            }
        }


        private void RefreshProfileComboBox(string newName) {
            InitializeProfiles(true);
            SetProfileIndex(newName);
        }


        private void RenameOrCopyProfile(bool isRename) {
            var newName = GetFileName("Profile", Paths.ProfilePath, new[] {Vendor.ProfileExtension}, "", isRename ? "Rename" : "Copy");

            if (String.Empty == newName) {
                return;
            }

            var root = Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath;
            var oldProfile = Path.Combine(root, _contextProfile.Name + Vendor.ProfileExtension);
            var newProfile = Path.Combine(root, newName + Vendor.ProfileExtension);

            if (isRename) {
                RenameFile(oldProfile, newProfile);
            }
            else {
                CopyFile(oldProfile, newProfile);
            }

            ((Profile) _contextProfile).Name = newName;
            SetContextDirtyFlag(true);

            RefreshProfileComboBox(newName);
        }


        private void SetProfileIndex(string profileName) {
            foreach (var profile in
                cbProfiles.Items.Cast<object>().OfType<Profile>().Where(profile => profile.Name == profileName)) {
                cbProfiles.SelectedIndex = cbProfiles.Items.IndexOf(profile);
            }
        }


        private static string GetFileName(string fileType, string filePath, IList<string> extension, string defaultResponse, string buttonText) {
            var newName = string.Empty;
            var caption = string.Format("{0} name", fileType);
            var query = string.Format("What would you like to name this {0}?", fileType);

            using (var dialog = new TextQueryDialog(caption, query, defaultResponse, buttonText)) {
                var showDialog = true;
                while (showDialog) {
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        newName = dialog.Response;
                        showDialog = false;

                        if (!extension.Aggregate(false, (current, ext) => current | File.Exists(Path.Combine(filePath, newName + ext)))) {
                            continue;
                        }

                        var msg = String.Format("{0} with the name {1} exists.  Overwrite this {0}?", fileType, newName);
                        var overwriteResult = MessageBox.Show(msg, "Overwrite?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        switch (overwriteResult) {
                            case DialogResult.Yes:
                                break;
                            case DialogResult.No:
                                newName = string.Empty;
                                showDialog = true;
                                break;
                            case DialogResult.Cancel:
                                newName = string.Empty;
                                break;
                        }
                    }
                    else {
                        showDialog = false;
                    }
                }
            }

            return newName;
        }


        private void btnCancel_Click(object sender, EventArgs e) {
            //ClearSetup();
            DialogResult = DialogResult.Cancel;

            if (!AnyDirtyProfiles()) {
                return;
            }

            if (QuerySaveChanges()) {
                DialogResult = DialogResult.OK;
            }
        }


        private bool AnyDirtyProfiles() {
            return cbProfiles.Items.OfType<Profile>().Any(p => p.IsDirty);
        }


        private void VixenPlusRoadie_FormClosing(object sender, FormClosingEventArgs e) {
            //ClearSetup();
            DialogResult = DialogResult.OK;

            if (e.CloseReason != CloseReason.UserClosing || !AnyDirtyProfiles()) {
                return;
            }

            if (!QuerySaveChanges()) {
                DialogResult = DialogResult.Cancel;
            }
        }


        private bool QuerySaveChanges() {
            var result = MessageBox.Show("Save changes before closing?", "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) {
                return false;
            }
            SaveChangedProfiles();

            return true;
        }


        private void SaveChangedProfiles() {
            PersistProfileInfo();
            foreach (var p in cbProfiles.Items.OfType<Profile>().Where(p => p.IsDirty)) {
                SaveProfile(p);
            }
        }


        private void btnProfileSave_Click(object sender, EventArgs e) {
            PersistProfileInfo();
            SaveProfile((Profile) _contextProfile);
            btnProfileSave.Enabled = false;
        }


        private void SetContextDirtyFlag(bool value) {
            if (_isPluginsOnly) {
                return;
            }

            ((Profile) _contextProfile).IsDirty = value;

        }


        public Delegate SetText(string text) {
            tcProfile.TabPages[0].Text = text;
            return null;
        }


        private void InitializeChannelTab() {
            pChannels.Size = tcProfile.ClientSize;
            pChannels.Controls.Clear();
            var control = new ChannelsTab(_contextProfile, this) {Size = pChannels.Size};
            pChannels.Controls.Add(control);
        }


        private void InitializePlugInTab() {
            pPlugIns.Size = tcProfile.ClientSize;
            pPlugIns.Controls.Clear();
            var control = new PlugInsTab(_contextProfile) {Size = pPlugIns.Size};
            pPlugIns.Controls.Add(control);
        }


        private void InitializeGroupsTab() {
            pGroups.Size = tcProfile.ClientSize;
            pGroups.Controls.Clear();
            var control = new GroupsTab(_contextProfile, false) {Size = pGroups.Size};
            pGroups.Controls.Add(control);
        }
    }
}