using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs {
    public partial class FrmProfileManager : Form {
        #region ClassMembers


        private readonly bool _suppressErrors = Preference2.GetInstance().GetBoolean("SilenceProfileErrors");
        private readonly int _dgvWidthDiff;
        private readonly int _dgvHeightDiff;
        private Profile _contextProfile;

        private const int ChannelEnabledCol = 0;
        private const int ChannelNumCol = 1;
        private const int ChannelNameCol = 2;
        private const int OutputChannelCol = 3;
        private const int ChannelColorCol = 4;

        private const int TabChannels = 0;
        private const int TabPlugins = 1;
        private const int TabGroups = 2;
        private const int TabSorts = 3;
        private const int TabNutcracker = 4;

        private const string Warning = "\n\nNOTE: While most functions can be undone by pressing cancel in the Profile Manager dialog, this one cannot.";
        #endregion

        #region Constructor

        //TODO Can this really be an IExecutable?
        public FrmProfileManager(Profile defaultProfile = null) {
            InitializeComponent();
            Icon = Resources.VixenPlus;

            _dgvWidthDiff = Width - dgvChannels.Width;
            _dgvHeightDiff = Height - dgvChannels.Height;

            InitializeControls();
            if (null != defaultProfile) {
                SetProfileIndex(defaultProfile.Name);
            }
            else {
                cbProfiles.SelectedIndex = 0;
            }
        }

        #endregion

        #region Events

        #region "Generic" Events

        private void frmProfileManager_Resize(object sender, EventArgs e) {
            dgvChannels.Width = Width - _dgvWidthDiff;
            dgvChannels.Height = Height - _dgvHeightDiff;
        }

        private void tcProfile_SelectedIndexChanged(object sender, EventArgs e) {
            DoButtonManagement();
        }

        #region Channel Tab Events

        private void btnChannelOutputs_Click(object sender, EventArgs e) {
            foreach (var z in GetSelectedRows().Reverse()) {
                Debug.Print(z.Cells["ChannelNum"].Value.ToString());
            }
        }

        private void dgvChannels_SelectionChanged(object sender, EventArgs e) {
            DoButtonManagement();
        }

        private void btnEnableDisable_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (null == button) {
                return;
            }

            var enable = button.Text.Equals("Enable");
            foreach (var row in GetSelectedRows()) {
                row.Cells[ChannelEnabledCol].Value = enable;
            }
            DoButtonManagement();
        }

        #endregion

        #endregion

        #region Import/Export Buttons

        private void btnImport_Click(object sender, EventArgs e) {
            using (var file = File.OpenText(Paths.ProfilePath + "\\Testing.csv")) {
                string line;
                var count = 0;
                var valid = true;
                while ((line = file.ReadLine()) != null) {
                    var cols = line.Replace("\"", "").Split(',');
                    if (0 == count) {
                        if (cols.Count() != dgvChannels.ColumnCount) {
                            valid = false;
                        }
                        else {
                            for (var i = 0; i < dgvChannels.ColumnCount; i++) {
                                valid &= String.Compare(dgvChannels.Columns[i].Name, cols[i], StringComparison.OrdinalIgnoreCase) == 0;
                            }
                        }
                        if (!valid) {
                            MessageBox.Show("Import file not valid, cannot import.");
                            return;
                        }
                        dgvChannels.Rows.Clear();
                        count++;
                    }
                    else {
                        var row =
                            dgvChannels.Rows.Add(new object[] {
                                cols[ChannelEnabledCol] == "True", 
                                cols[ChannelNumCol].ToInt(), 
                                cols[ChannelNameCol], 
                                cols[OutputChannelCol].ToInt(), 
                                cols[ChannelColorCol]
                            });
                        dgvChannels.Rows[row].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(cols[ChannelColorCol]);
                        dgvChannels.Rows[row].DefaultCellStyle.ForeColor = dgvChannels.Rows[row].DefaultCellStyle.BackColor.GetForeColor();
                    }
                }
            }
        }

        
        private void btnExport_Click(object sender, EventArgs e) {
            using (var file = File.CreateText(Paths.ProfilePath + "\\Testing.csv")) {
                var data = new StringBuilder();
                foreach (DataGridViewColumn col in dgvChannels.Columns) {
                    data.Append(col.Name).Append(",");
                }
                file.WriteLine(data.ToString(0, data.Length - 1));
                foreach (DataGridViewRow row in dgvChannels.Rows) {
                    data.Length = 0;
                    for (var i = 0; i < dgvChannels.ColumnCount; i++) {
                        data.Append(row.Cells[i].Value).Append(",");
                    }
                    file.WriteLine(data.ToString(0, data.Length - 1));
                }
            }
        }

        #endregion

        #region Profile Group Box Events
        
        private void btnAddProfile_Click(object sender, EventArgs e) {
            var newName = GetNewName("Create");
            if(string.Empty == newName) {
                return;
            }

            var root = Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath;
            DeleteIfExists(Path.Combine(root, newName + Vendor.GroupExtension));
            DeleteIfExists(Path.Combine(root, newName + Vendor.ProfileExtension));

            var pro = new Profile {Name = newName};
            pro.SaveToFile();
            RefreshProfileComboBox(newName);
        }


        private void btnCopyProfile_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(false);
        }


        private void btnDeleteProfile_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this profile and group, if one exists?" + Warning, "Delete Profile", MessageBoxButtons.YesNo) != DialogResult.Yes) {
                return;
            }

            DeleteIfExists(_contextProfile.FileName);
            DeleteIfExists(Path.Combine(Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath, _contextProfile.Name + Vendor.GroupExtension));
            InitializeChannelTab();
            cbProfiles.SelectedIndex = 0;
        }


        private void btnOkay_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }


        private void btnRenameProfile_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(true);
        }


        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            //todo need to save any changes before changing index
            dgvChannels.Rows.Clear();

            if (0 == cbProfiles.SelectedIndex) {
                _contextProfile = null;
                DoButtonManagement();
                return;
            }

            _contextProfile = (Profile)cbProfiles.SelectedItem;
            DoButtonManagement();

            var count = 1;
            foreach (var ch in _contextProfile.FullChannels) {
                var row = dgvChannels.Rows.Add(new object[] { ch.Enabled, count++, ch.Name, ch.OutputChannel + 1, ColorTranslator.ToHtml(ch.Color) });
                dgvChannels.Rows[row].DefaultCellStyle.BackColor = ch.Color;
                dgvChannels.Rows[row].DefaultCellStyle.ForeColor = ch.Color.GetForeColor();
            }
        }

        #endregion
        
        #endregion

        #region Support Methods

        #region Button management
        private void DoButtonManagement() {
            var isProfileLoaded = _contextProfile != null;
            SetGeneralButtons(isProfileLoaded);
 
            switch (tcProfile.SelectedIndex) {
                case TabChannels:
                    SetChannelTabButtons(isProfileLoaded);
                    break;
                case TabPlugins:
                    SetPluginsTabButtons(isProfileLoaded);
                    break;
                case TabGroups:
                    SetGroupTabButtons(isProfileLoaded);
                    break;
                case TabSorts:
                    SetSortsTabButtons(isProfileLoaded);
                    break;
                case TabNutcracker:
                    SetNutcrackerTabButtons(isProfileLoaded);
                    break;
            }
        }


        private void SetChannelTabButtons(bool isProfileLoaded) {
            var selectedRows = GetSelectedRows().ToList();
            var cellsSelected = selectedRows.Any();
            var oneRowSelected = selectedRows.Count() == 1;
            var hasEnabledChannels = (from DataGridViewRow x in selectedRows where (bool.Parse(x.Cells[ChannelEnabledCol].Value.ToString())) select x).Any();
            var hasDisabledChannels = (from DataGridViewRow x in selectedRows where (!bool.Parse(x.Cells[ChannelEnabledCol].Value.ToString())) select x).Any();

            btnChAddMulti.Enabled = isProfileLoaded; // todo
            btnChAddOne.Enabled = isProfileLoaded; // todo
            btnChColorMulti.Enabled = isProfileLoaded && !oneRowSelected; // todo
            btnChColorOne.Enabled = isProfileLoaded && cellsSelected; // todo
            btnChDisable.Enabled = isProfileLoaded && cellsSelected && hasEnabledChannels;
            btnChEnable.Enabled = isProfileLoaded && cellsSelected && hasDisabledChannels;
            btnChExport.Enabled = isProfileLoaded;
            btnChImport.Enabled = isProfileLoaded;
            btnChMapAbove.Enabled = isProfileLoaded && cellsSelected; //todo
            btnChMapBelow.Enabled = isProfileLoaded && cellsSelected; //todo
        }


        private void SetGeneralButtons(bool isProfileLoaded) {
            btnChannelOutputs.Enabled = isProfileLoaded;
            
            btnCopyProfile.Enabled = isProfileLoaded;
            btnDeleteProfile.Enabled = isProfileLoaded;
            btnRenameProfile.Enabled = isProfileLoaded;
        }


        private void SetGroupTabButtons(bool isProfileLoaded) {
            btnGraButton.Enabled = isProfileLoaded;
        }


        private void SetPluginsTabButtons(bool isProfileLoaded) {
            btnPiaButton.Enabled = isProfileLoaded;
        }


        private void SetNutcrackerTabButtons(bool isProfileLoaded) {
            btnNcaButton.Enabled = isProfileLoaded;
        }


        private void SetSortsTabButtons(bool isProfileLoaded) {
            btnSrtDelete.Enabled = isProfileLoaded;
            btnSrtSave.Enabled = isProfileLoaded;
            cbSrtOrders.Enabled = isProfileLoaded;
        }

        #endregion

        #region Helper Methods
        private static void CopyFile(string originalFile, string newFile) {
            DeleteIfExists(newFile);
            File.Copy(originalFile, newFile);
        }


        private static void DeleteIfExists(string fileName) {
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
        }


        private string GetNewName(string buttonText) {
            var newName = string.Empty;
            using (var dialog = new TextQueryDialog("Profile Name?", Resources.NameThisProfile + Warning, _contextProfile != null ? _contextProfile.Name : "", buttonText)) {
                var showDialog = true;
                while (showDialog) {
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        newName = dialog.Response;
                        showDialog = false;
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


        private IEnumerable<DataGridViewRow> GetSelectedRows() {
            var hashSet = new HashSet<DataGridViewRow>();
            foreach (var cell in from DataGridViewCell x in dgvChannels.SelectedCells where !hashSet.Contains(x.OwningRow) select x) {
                hashSet.Add(cell.OwningRow);
            }

            return hashSet;
        } 


        private void InitializeControls() {
            InitializeChannelTab();
        }


        private void InitializeChannelTab(bool reload = false) {
            var errors = new StringBuilder();
            cbProfiles.Items.Clear();
            cbProfiles.Items.Add("Select or add a profile");
            foreach (var profileFile in
                Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.ProfileExtension).Where(
                    profileFile => Path.GetExtension(profileFile) == Vendor.ProfileExtension)) {
                try {
                    cbProfiles.Items.Add(new Profile(profileFile));
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
            DeleteIfExists(newFile);
            File.Move(oldFile, newFile);
        }


        private void RefreshProfileComboBox(string newName) {
            InitializeChannelTab(true);
            SetProfileIndex(newName);
        }


        private void RenameOrCopyProfile(bool isRename) {
            var newName = GetNewName(isRename ? "Rename" : "Copy");

            if (String.Empty == newName) {
                return;
            }

            var root = Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath;

            var oldGroup = Path.Combine(root, _contextProfile.Name + Vendor.GroupExtension);
            var newGroup = Path.Combine(root, newName + Vendor.GroupExtension);

            if (isRename) {
                RenameFile(oldGroup, newGroup);
            }
            else {
                CopyFile(oldGroup, newGroup);
            }

            var oldProfile = Path.Combine(root, _contextProfile.Name + Vendor.ProfileExtension);
            var newProfile = Path.Combine(root, newName + Vendor.ProfileExtension);
            
            if (isRename) {
                RenameFile(oldProfile, newProfile);
            }
            else {
                CopyFile(oldProfile, newProfile);
            }
            
            _contextProfile.Name = newName;

            RefreshProfileComboBox(newName);
        }


        private void SetProfileIndex(string profileName) {
            foreach (var profile in cbProfiles.Items.Cast<object>().OfType<Profile>().Where(profile => profile.Name == profileName)) {
                cbProfiles.SelectedIndex = cbProfiles.Items.IndexOf(profile);
            }
        }

        #endregion

        #endregion
    }
}