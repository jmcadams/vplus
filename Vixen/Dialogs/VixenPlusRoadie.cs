using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using VixenPlus.Properties;

using VixenPlusCommon;

//todo some things I want to do:
//Check that DoButtonManagement is really needed everywhere it is used.
//Refactor tab pages into their own controls.
//Implement remember selected cell
//Finish Implementing Event when a colorpalette color changes (and the live view idea)
//Move panels to thier own class
using VixenPlusCommon.Annotations;

namespace VixenPlus.Dialogs {
    public partial class VixenPlusRoadie : Form {

        #region ClassMembers

        private readonly bool _suppressErrors;
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
        private const int TabSorts = 2;
        private const int TabGroups = 3;
        private const int TabNutcracker = 4;

        private const int ControlTabNormal = 0;
        private const int ControlTabMultiChannel = 1;
        private const int ControlTabMultiColor = 2;

        private const string Warning =
            "\n\nNOTE: While most functions can be undone by pressing cancel in the Profile Manager dialog, this one cannot.";

        private readonly IList<Rules> _ruleEngines = new List<Rules>();

        #endregion

        #region Constructor

        public VixenPlusRoadie(IExecutable defaultProfile = null) {
            InitializeComponent();
            Text = Vendor.ProductName + " - " + Vendor.ModuleManager;
            Icon = Resources.VixenPlus;
            MinimumSize = Size;

            _suppressErrors = Preference2.GetInstance().GetBoolean("SilenceProfileErrors");
            _dgvWidthDiff = Width - dgvChannels.Width;
            _dgvHeightDiff = Height - dgvChannels.Height;
            InitializeControls();

            for (var i = TabGroups; i <= TabNutcracker; i++) {
                tcProfile.TabPages.Remove(tcProfile.TabPages[TabGroups]);
            }

            if (null != defaultProfile) {
                SetProfileIndex(defaultProfile.Name);
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

        #endregion

        #region Events

        #region "Generic" Events

        private void FrmProfileManager_KeyDown(object sender, KeyEventArgs e) {
            switch (tcProfile.SelectedIndex) {
                case TabChannels:
                    DoChannelKeys(e);
                    break;
                case TabPlugins:
                    DoPluginsKeys(e);
                    break;
                case TabGroups:
                    DoGroupKeys(e);
                    break;
                case TabSorts:
                    DoSortsKeys(e);
                    break;
                case TabNutcracker:
                    DoNutcrackerKeys(e);
                    break;
            }
        }


        private void frmProfileManager_Resize(object sender, EventArgs e) {
            dgvChannels.Width = Width - _dgvWidthDiff;
            dgvChannels.Height = Height - _dgvHeightDiff;

            // HACK:  Had to do this to get the columns dgv to resize 
            // not the same issue but found it here: http://stackoverflow.com/questions/296418/datagridview-column-resize-problem
            dgvChannels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvChannels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }


        private void FrmProfileManager_Shown(object sender, EventArgs e) {
            DoButtonManagement();
        }


        private void tcProfile_SelectedIndexChanged(object sender, EventArgs e) {
            DoButtonManagement();
        }

        #region Channel Tab Events

        private void dgvChannels_SelectionChanged(object sender, EventArgs e) {
            DoButtonManagement();
        }


        private void btnEnableDisable_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (null == button) {
                return;
            }

            var enable = button.Text.Equals(btnChEnable.Text);
            var rows = GetSelectedRows().ToList();
            foreach (var row in rows.Where(r => bool.Parse(r.Cells[ChannelEnabledCol].Value.ToString()) != enable)) {
                row.Cells[ChannelEnabledCol].Value = enable;
            }
            dgvChannels.Refresh();
            _contextProfile.IsDirty = true;
            DoButtonManagement();
        }

        #endregion

        #endregion

        #region Import/Export Buttons

        private void btnImport_Click(object sender, EventArgs e) {
            using (
                var dialog = new OpenFileDialog {
                    AddExtension = true, CheckFileExists = true, CheckPathExists = true, DefaultExt = Vendor.CsvExtension,
                    Filter = "(CSV Files)|" + Vendor.All + Vendor.CsvExtension, InitialDirectory = Paths.ImportExportPath,
                    Multiselect = false}) {

                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                using (var file = File.OpenText(dialog.FileName)) {
                    string line;
                    var count = 0;
                    var valid = true;
                    dgvChannels.SuspendLayout();
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
                                break;
                            }
                            dgvChannels.Rows.Clear();
                            _contextProfile.IsDirty = true;
                            count++;
                        }
                        else {
                            var row =
                                dgvChannels.Rows.Add(new object[] {
                                    cols[ChannelEnabledCol] == "True", cols[ChannelNumCol].ToInt(), cols[ChannelNameCol], cols[OutputChannelCol].ToInt(),
                                    cols[ChannelColorCol]
                                });
                            dgvChannels.Rows[row].DefaultCellStyle.BackColor = cols[ChannelColorCol].FromHTML();
                            dgvChannels.Rows[row].DefaultCellStyle.ForeColor = dgvChannels.Rows[row].DefaultCellStyle.BackColor.GetForeColor();
                        }
                    }
                    dgvChannels.ResumeLayout();
                    SetGeneralButtons();
                }
            }
        }


        private void btnExport_Click(object sender, EventArgs e) {
            using (var dialog = new SaveFileDialog {
                    InitialDirectory = Paths.ImportExportPath, Filter = "(CSV File)|" + Vendor.All + Vendor.CsvExtension, OverwritePrompt = true,
                    DefaultExt = Vendor.CsvExtension}) {

                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                using (var file = File.CreateText(dialog.FileName)) {
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
        }

        #endregion

        #region Profile Group Box Events

        private void btnProfileAdd_Click(object sender, EventArgs e) {
            var newName = GetFileName("Profile/Group Name", Paths.ProfilePath, new[] {Vendor.ProfileExtension, Vendor.GroupExtension}, "", "Create");

            if (string.Empty == newName) {
                return;
            }

            if (null != _contextProfile) {
                var root = Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath;
                DeleteIfExists(Path.Combine(root, newName + Vendor.GroupExtension));
                DeleteIfExists(Path.Combine(root, newName + Vendor.ProfileExtension));
            }

            var pro = new Profile {Name = newName};
            pro.SaveToFile();
            RefreshProfileComboBox(newName);
        }


        private void btnProfileCopy_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(false);
        }


        private void btnProfileDelete_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show("Are you sure you want to delete this profile and group, if one exists?" + Warning, "Delete Profile",
                    MessageBoxButtons.YesNo) != DialogResult.Yes) {
                return;
            }

            DeleteIfExists(_contextProfile.FileName);
            DeleteIfExists(Path.Combine(Path.GetDirectoryName(_contextProfile.FileName) ?? Paths.ProfilePath,
                _contextProfile.Name + Vendor.GroupExtension));
            InitializeChannelTab();
            cbProfiles.SelectedIndex = 0;
        }


        private void btnOkay_Click(object sender, EventArgs e) {
            SaveChangedProfiles();
            DialogResult = DialogResult.OK;
        }


        private void btnProfileRename_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(true);
        }


        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            SaveProfileFromRows();
            colorPaletteChannel.ControlChanged -= UpdateColors;
            tcControlArea.SelectTab(ControlTabNormal);
            tcProfile.TabPages[0].Text = "Channels";
            dgvChannels.Rows.Clear();

            dgvChannels.SuspendLayout();

            if (0 == cbProfiles.SelectedIndex) {
                _contextProfile = null;
                DoButtonManagement();
                return;
            }

            _contextProfile = (Profile) cbProfiles.SelectedItem;

            AddRows(_contextProfile.FullChannels);

            dgvChannels.ResumeLayout();
            dgvChannels.Focus();
            DoButtonManagement();
        }


        private void SaveProfileFromRows() {
            if (null == _contextProfile) {
                return;
            }

            _contextProfile.ClearChannels();
            foreach (var ch in from DataGridViewRow row in dgvChannels.Rows
                select
                    new Channel(row.Cells[ChannelNameCol].Value.ToString(), row.DefaultCellStyle.BackColor,
                        int.Parse(row.Cells[OutputChannelCol].Value.ToString())) 
                        {Enabled = bool.Parse(row.Cells[ChannelEnabledCol].Value.ToString())}
                ) {
                _contextProfile.AddChannelObject(ch);
            }
        }


        private void AddRows(IEnumerable<Channel> channels, int startCh = 1) {
            dgvChannels.SelectionChanged -= dgvChannels_SelectionChanged;
            dgvChannels.SuspendLayout();
            foreach (var ch in channels) {
                AddRow(ch, startCh++);
            }
            dgvChannels.ResumeLayout();
            dgvChannels.SelectionChanged += dgvChannels_SelectionChanged;
        }


        private void AddRow(Channel ch, int chNum) {
            var row = dgvChannels.Rows.Add(new object[] { ch.Enabled, chNum, ch.Name, ch.OutputChannel + 1, ch.Color.ToHTML() });
            dgvChannels.Rows[row].DefaultCellStyle.BackColor = ch.Color;
            dgvChannels.Rows[row].DefaultCellStyle.ForeColor = ch.Color.GetForeColor();
        }

        #endregion

        #endregion

        #region Support Methods

        #region Button management

        #region Channel Generator

        private void btnChAddMulti_Click(object sender, EventArgs e) {
            dgvChannels.Rows.Clear();
            colorPaletteChannel.ControlChanged += UpdateColors;
            tcControlArea.SelectTab(ControlTabMultiChannel);
            tcProfile.TabPages[0].Text = "Preview";
            LoadTemplates();
            DoButtonManagement();
            DoRuleButtons();
        }


        private void UpdateColors(object sender, EventArgs e) {
            PreviewChannels();
        }


        private void btnMultiChannelButton_Click(object sender, EventArgs e) {
            colorPaletteChannel.ControlChanged -= UpdateColors;
            tcControlArea.SelectTab(ControlTabNormal);
            tcProfile.TabPages[0].Text = "Channels";
            dgvChannels.Rows.Clear();
            if (((Button) sender).Text == btnMultiChannelOk.Text) {
                foreach (var c in GenerateChannels()) {
                    _contextProfile.AddChannelObject(c);
                }
            }
            AddRows(_contextProfile.FullChannels);
            _contextProfile.IsDirty = true;
            SelectLastRow();
            DoButtonManagement();
        }


        private void LoadTemplates() {
            cbChGenTemplate.Items.Clear();
            foreach (var fileName in
                Directory.GetFiles(Paths.ProfileGeneration, Vendor.All + Vendor.TemplateExtension).Where(file => file.EndsWith(Vendor.TemplateExtension))
                    .Select(Path.GetFileNameWithoutExtension)) {
                cbChGenTemplate.Items.Add(fileName);
            }
            ShowNumbers(false, "");
        }

        #endregion

        #region Profile Channels

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
            var hasEnabledChannels = selectedRows.Any(r => bool.Parse(r.Cells[ChannelEnabledCol].Value.ToString()));
            var hasDisabledChannels = selectedRows.Any(r => !bool.Parse(r.Cells[ChannelEnabledCol].Value.ToString()));

            btnChAddMulti.Enabled = isProfileLoaded;
            btnChAddOne.Enabled = isProfileLoaded;
            btnChColorMulti.Enabled = isProfileLoaded && !oneRowSelected;
            btnChColorOne.Enabled = isProfileLoaded && cellsSelected;
            btnChDisable.Enabled = isProfileLoaded && cellsSelected && hasEnabledChannels;
            btnChEnable.Enabled = isProfileLoaded && cellsSelected && hasDisabledChannels;
            btnChDelete.Enabled = isProfileLoaded && cellsSelected;
            btnChExport.Enabled = isProfileLoaded;
            btnChImport.Enabled = isProfileLoaded;
        }


        private void SetGeneralButtons(bool isProfileLoaded = true) {
            tcProfile.Visible = isProfileLoaded;
            var isChannelPanel = tcControlArea.SelectedTab == tpChannelControl;
            btnCancel.Enabled = isChannelPanel;
            btnOkay.Enabled = isChannelPanel;
            btnProfileAdd.Enabled = isChannelPanel;
            btnProfileCopy.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileDelete.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileRename.Enabled = isProfileLoaded && isChannelPanel;
            btnProfileSave.Enabled = _contextProfile != null && _contextProfile.IsDirty;
        }


        private void SetGroupTabButtons(bool isProfileLoaded) {
            btnGraButton.Enabled = isProfileLoaded;
        }


        private void SetPluginsTabButtons(bool isProfileLoaded) {
            Debug.Print(isProfileLoaded.ToString());
            //todo Implement
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

        #endregion

        #region Key Press Management

        #region Group Key Management

        private void DoGroupKeys(KeyEventArgs e) {
            Debug.Print(e.KeyCode.ToString());
        }

        #endregion

        #region Channel Key Management

        private void DoChannelKeys(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.C:
                    if (!e.Control) {
                        return;
                    }

                    DoChannelCopy();
                    e.Handled = true;
                    break;

                case Keys.V:
                    if (!e.Control) {
                        return;
                    }

                    DoChannelPaste();
                    _contextProfile.IsDirty = true;
                    e.Handled = true;
                    break;
            }
        }


        private void DoChannelCopy() {
            if (dgvChannels.GetCellCount(DataGridViewElementStates.Selected) <= 0) {
                return;
            }

            var data = dgvChannels.GetClipboardContent();
            if (null != data) {
                Clipboard.SetDataObject(data);
            }
        }


        private static void DoChannelPaste() {
            if (!Clipboard.ContainsData(DataFormats.Text)) {
                return;
            }

            var s = (string) Clipboard.GetData(DataFormats.Text);
            Clipboard.SetData(DataFormats.Text, s);
            var csv = s.Split(new[] {"\r\n"}, StringSplitOptions.None).ToList();
            foreach (var cc in csv.SelectMany(c => c.Split('\t').ToList())) {
                Debug.Print("'" + cc + "'"); // TODO Need to paste the data.
            }
        }

        #endregion

        private void DoNutcrackerKeys(KeyEventArgs e) {
            Debug.Print(e.KeyCode.ToString());
        }


        private void DoPluginsKeys(KeyEventArgs e) {
            Debug.Print(e.KeyCode.ToString());
        }


        private void DoSortsKeys(KeyEventArgs e) {
            Debug.Print(e.KeyCode.ToString());
        }

        #endregion

        #region Helper Methods

        private static void CopyFile(string originalFile, string newFile) {
            if (!File.Exists(originalFile)) {
                return;
            }

            DeleteIfExists(newFile);
            File.Copy(originalFile, newFile);
        }


        private static void DeleteIfExists(string fileName) {
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
        }


        private IEnumerable<DataGridViewRow> GetSelectedRows() {
            var hashSet = new HashSet<DataGridViewRow>();
            foreach (var cell in
                from DataGridViewCell cell in dgvChannels.SelectedCells where !hashSet.Contains(cell.OwningRow) select cell) {
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
            if (!File.Exists(oldFile)) {
                return;
            }

            DeleteIfExists(newFile);
            File.Move(oldFile, newFile);
        }


        private void RefreshProfileComboBox(string newName) {
            InitializeChannelTab(true);
            SetProfileIndex(newName);
        }


        private void RenameOrCopyProfile(bool isRename) {
            var newName = GetFileName("Profile/Group Name", Paths.ProfilePath, new[] {Vendor.ProfileExtension, Vendor.GroupExtension}, "",
                isRename ? "Rename" : "Copy");

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
            _contextProfile.IsDirty = true;

            RefreshProfileComboBox(newName);
        }


        private void SetProfileIndex(string profileName) {
            foreach (var profile in
                cbProfiles.Items.Cast<object>().OfType<Profile>().Where(profile => profile.Name == profileName)) {
                cbProfiles.SelectedIndex = cbProfiles.Items.IndexOf(profile);
            }
        }

        #endregion

        private void cbRuleColors_CheckedChanged(object sender, EventArgs e) {
            colorPaletteChannel.Visible = cbRuleColors.Checked && cbRuleColors.Visible;
            PreviewChannels();
        }

        #endregion

        #region Added during MultiChannelAdd - need to refactor

        private void btnRuleAdd_Click(object sender, EventArgs e) {
            if (cbRuleRules.SelectedIndex == -1) {
                return;
            }
            switch (cbRuleRules.SelectedItem.ToString()) {
                case "Numbers":
                    lbRules.Items.Add(new ProfileManagerNumbers {Start = 1, IsLimited = true, End = 1, Increment = 1});
                    break;
                case "Words":
                    lbRules.Items.Add(new ProfileManagerWords {Words = String.Empty});
                    break;
            }
            FormatRuleItems();
        }


        private void lbRules_SelectedIndexChanged(object sender, EventArgs e) {
            var rule = lbRules.SelectedItem as Rules;
            if (null == rule) {
                ShowNumbers(false, string.Empty);
                DoRuleButtons();
                return;
            }

            if (rule is ProfileManagerNumbers) {
                var numbers = rule as ProfileManagerNumbers;
                ShowNumbers(true, ProfileManagerNumbers.Prompt);
                nudRuleStart.Value = numbers.Start;
                nudRuleEnd.Value = numbers.End;
                nudRuleEnd.Enabled = numbers.IsLimited;
                cbRuleEndNum.Checked = numbers.IsLimited;
                nudRuleIncr.Value = numbers.Increment;
            }
            else if (rule is ProfileManagerWords) {
                var words = rule as ProfileManagerWords;
                ShowNumbers(false, ProfileManagerWords.Prompt);
                tbRuleWords.Text = words.Words;
            }
            DoRuleButtons();
        }


        private void ShowNumbers(bool isNumbers, string prompt) {
            var isItemSelected = lbRules.SelectedIndex != -1;
            lblRulePrompt.Text = prompt;
            lblRulePrompt.Visible = isItemSelected;
            tbRuleWords.Visible = isItemSelected && !isNumbers;
            lblRuleStartNum.Visible = isItemSelected && isNumbers;
            cbRuleEndNum.Visible = isItemSelected && isNumbers;
            lblRuleIncr.Visible = isItemSelected && isNumbers;
            nudRuleStart.Visible = isItemSelected && isNumbers;
            nudRuleEnd.Visible = isItemSelected && isNumbers;
            nudRuleIncr.Visible = isItemSelected && isNumbers;
            PreviewChannels();
        }


        private void nudRuleStart_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).Start = (int) nudRuleStart.Value;
            PreviewChannels();
        }


        private void nudRuleEnd_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).End = (int) nudRuleEnd.Value;
            PreviewChannels();
        }


        private void nudRuleIncr_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).Increment = (int) nudRuleIncr.Value;
            PreviewChannels();
        }


        private void cbRuleEndNum_CheckedChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).IsLimited = cbRuleEndNum.Checked;
            nudRuleEnd.Enabled = cbRuleEndNum.Checked;
            PreviewChannels();
        }


        private void tbRuleWords_TextChanged(object sender, EventArgs e) {
            ((ProfileManagerWords) lbRules.SelectedItem).Words = tbRuleWords.Text;
            PreviewChannels();
        }


        private void btnRuleUp_Click(object sender, EventArgs e) {
            var selected = lbRules.SelectedIndex;
            if (selected > 0) {
                SwapRules(selected, selected - 1);
            }
        }


        private void btnRuleDown_Click(object sender, EventArgs e) {
            var selected = lbRules.SelectedIndex;
            if (selected < lbRules.Items.Count) {
                SwapRules(selected, selected + 1);
            }
        }


        private void SwapRules(int originalIndex, int newIndex) {
            var totalCount = lbRules.Items.Count - 1;
            if (originalIndex > totalCount || originalIndex < 0 || newIndex > totalCount || newIndex < 0) {
                return;
            }

            var holdItem = lbRules.Items[newIndex];
            lbRules.Items[newIndex] = lbRules.Items[originalIndex];
            lbRules.Items[originalIndex] = holdItem;
            lbRules.SelectedIndex = newIndex;
            FormatRuleItems();
        }


        private void FormatRuleItems() {
            var count = 0;
            foreach (Rules rule in lbRules.Items) {
                rule.Name = string.Format("{0} {{{1}}}", rule.BaseName, count);
                count++;
            }
            lbRules.DisplayMember = "";
            lbRules.DisplayMember = "Name";
            DoRuleButtons();
            PreviewChannels();
        }


        private void btnRuleDelete_Click(object sender, EventArgs e) {
            if (lbRules.SelectedIndex < 0) {
                return;
            }

            lbRules.Items.RemoveAt(lbRules.SelectedIndex);
            FormatRuleItems();
        }


        private void DoRuleButtons() {
            var index = lbRules.SelectedIndex;
            var count = lbRules.Items.Count;
            btnRuleUp.Enabled = index > 0;
            btnRuleDown.Enabled = index != -1 && index != count - 1;
            btnRuleAdd.Enabled = cbRuleRules.SelectedIndex != -1;
            btnRuleDelete.Enabled = index != -1;

            btnChGenSaveTemplate.Enabled = !string.IsNullOrEmpty(tbChGenNameFormat.Text) && count > 0;
        }


        private void cbRuleRules_SelectedIndexChanged(object sender, EventArgs e) {
            DoRuleButtons();
        }


        private void lbRules_KeyDown(object sender, KeyEventArgs e) {
            if (lbRules.SelectedIndex != -1 && e.KeyCode == Keys.Delete) {
                btnRuleDelete_Click(null, null);
            }
        }


        private void btnChGenSaveTemplate_Click(object sender, EventArgs e) {
            var name = (cbChGenTemplate.SelectedIndex != -1) ? cbChGenTemplate.SelectedItem.ToString() : "";

            var newName = GetFileName("Template", Paths.ProfileGeneration, new[] {Vendor.TemplateExtension}, name, "OK");

            if (string.IsNullOrEmpty(newName)) {
                return;
            }

            CreateTemplate().Save(Path.Combine(Paths.ProfileGeneration, newName + Vendor.TemplateExtension));
            LoadTemplates();
            cbChGenTemplate.SelectedIndex = cbChGenTemplate.Items.IndexOf(newName);
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


        private XElement CreateTemplate() {
            var rules = (new XElement("Rules"));
            foreach (Rules rule in lbRules.Items) {
                rules.Add(rule.RuleData);
            }

            var palette = new XElement(ColorPalette.PaletteElement);
            if (cbRuleColors.Checked) {
                palette = colorPaletteChannel.Palette;
            }

            var template = new XElement("Template", new XElement("Channels", nudChGenChannels.Value),
                new XElement("ChannelNameFormat", tbChGenNameFormat.Text), rules, palette);

            return template;
        }


        private void cbChGenTemplate_SelectedIndexChanged(object sender, EventArgs e) {
            var template = XElement.Load(Path.Combine(Paths.ProfileGeneration, cbChGenTemplate.SelectedItem + Vendor.TemplateExtension));
            var element = template.Element("Channels");
            nudChGenChannels.Value = element != null ? int.Parse(element.Value) : 1;

            element = template.Element("ChannelNameFormat");
            tbChGenNameFormat.Text = element != null ? element.Value : string.Empty;

            element = template.Element(ColorPalette.PaletteElement);
            if (element != null && element.Elements().Any()) {
                cbRuleColors.Checked = true;
                colorPaletteChannel.Palette = element;
            }
            else {
                cbRuleColors.Checked = false;
            }

            var rules = template.Element("Rules");

            lbRules.Items.Clear();
            if (null == rules) {
                return;
            }

            foreach (var ele in rules.Elements(Rules.RuleDataElement)) {
                if (null == ele) {
                    continue;
                }

                var attr = ele.Attribute(Rules.RuleAttribute);
                if (null == attr) {
                    continue;
                }

                switch (attr.Value) {
                    case "Numbers":
                        lbRules.Items.Add(new ProfileManagerNumbers {RuleData = ele});
                        break;
                    case "Words":
                        lbRules.Items.Add(new ProfileManagerWords {RuleData = ele});
                        break;
                }
            }
            FormatRuleItems();
            DoRuleButtons();
            ShowNumbers(false, "");
        }


        private void PreviewChannels() {
            if (!cbPreview.Checked) {
                return;
            }

            if (previewTimer.Enabled) {
                previewTimer.Stop();
            }
            previewTimer.Start();
        }


        private IEnumerable<Channel> GenerateChannels() {
            _ruleEngines.Clear();
            foreach (Rules item in lbRules.Items) {
                _ruleEngines.Add(item);
            }

            var generatedNames = GenerateNames(1, tbChGenNameFormat.Text, 0, (int) nudChGenChannels.Value).ToList();
            var generatedChannels = new List<Channel>();
            var startChannelNum = _contextProfile.FullChannels.Count();
            var colors = cbRuleColors.Checked ? GetColorList(colorPaletteChannel) : new List<Color> {Color.White};

            for (var count = 0; count < generatedNames.Count(); count++) {
                generatedChannels.Add(new Channel(generatedNames[count], startChannelNum + count) {Color = colors[count % colors.Count]});
            }

            return generatedChannels;
        }


        private static List<Color> GetColorList(ColorPalette palette) {
            var colors = new List<Color>(palette.Colors.Where(c => c != Color.Transparent).Select(c => c));

            if (colors.Count == 0) {
                colors.Add(Color.White);
            }

            return colors;
        }


        private IEnumerable<string> GenerateNames(int ruleNum, string nameFormat, int currentChannel, int totalChannels) {
            var names = new List<string>();

            if (lbRules.Items.Count < ruleNum || currentChannel > totalChannels) {
                return names;
            }

            var ruleEngine = _ruleEngines[ruleNum - 1];
            var generatedNames = new List<string>(ruleEngine.GenerateNames());

            for (var i = 0; (i < ruleEngine.Iterations || ruleEngine.IsUnlimited) && currentChannel + names.Count < totalChannels; i++) {
                var parts = new Regex("{" + (ruleNum - 1) + "[:]?[a-zA-Z0-9]*}").Match(nameFormat).ToString().Split(':');
                var format = parts.Count() == 2 ? "{0:" + parts[1] : "{0}";
                var replace = parts.Count() == 2 ? "{" + (ruleNum - 1) + ":" + parts[1] : "{" + (ruleNum - 1) + "}";
                var replacementValue = ruleEngine.IsUnlimited ? ruleEngine.GenerateName(i) : generatedNames[i];
                int numericReplacement;
                var formattingResult = nameFormat.Replace(replace,
                    int.TryParse(replacementValue, out numericReplacement) ? string.Format(format, numericReplacement) : replacementValue);

                // Is this the last rule?
                if (ruleNum >= _ruleEngines.Count) {
                    names.Add(formattingResult);
                }
                else {
                    names.AddRange(GenerateNames(ruleNum + 1, formattingResult, currentChannel + names.Count, totalChannels).ToList());
                }
            }

            return names;
        }


        private static bool GetColor(Control ctrl, Color initialColor, out Color resultColor, bool showNone = true) {
            var result = false;
            resultColor = Color.Black;
            const int offset = 6;

            using (var dialog = new ColorPicker(initialColor, showNone)) {
                dialog.Location = dialog.GetBestLocation(ctrl.PointToScreen(new Point(0, 0)), offset);
                dialog.ShowDialog();

                switch (dialog.DialogResult) {
                    case DialogResult.OK:
                        resultColor = dialog.GetColor();
                        result = true;
                        break;
                    case DialogResult.No:
                        resultColor = Color.Transparent;
                        result = true;
                        break;
                }
            }

            return result;
        }

        #endregion

        #region Final stuff added for Channels tab - need to refactor

        private void btnChAddOne_Click(object sender, EventArgs e) {
            var chNum = _contextProfile.FullChannels.Count;
            var ch = new Channel(string.Format("Channel {0}", chNum + 1), Color.White, chNum);
            _contextProfile.AddChannelObject(ch);
            AddRow(_contextProfile.FullChannels[chNum], chNum + 1);
            _contextProfile.IsDirty = true;
            SelectLastRow();
        }


        private void SelectLastRow() {
            dgvChannels.ClearSelection();
            var lastRow = dgvChannels.RowCount - 1;
            dgvChannels.Rows[lastRow].Selected = true;
            dgvChannels.FirstDisplayedScrollingRowIndex = lastRow;
        }


        private void btnChDelete_Click(object sender, EventArgs e) {
            var rows = GetSelectedRows().ToList();
            var channels = rows.Select(row => _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1]).ToList();

            _contextProfile.IsDirty = true;

            foreach (var c in channels) {
                _contextProfile.RemoveChannel(c);
            }

            foreach (var r in rows) {
                dgvChannels.Rows.Remove(r);
            }

            dgvChannels.SuspendLayout();
            dgvChannels.Rows.Clear();
            AddRows(_contextProfile.FullChannels);
            dgvChannels.ResumeLayout();
        }


        private Rectangle _dragDropBox;
        private int _dragDropRowIndex;


        private void dataGridView1_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Right) != MouseButtons.Right) {
                return;
            }

            // If the mouse moves outside the rectangle, start the drag.
            if (_dragDropBox == Rectangle.Empty || _dragDropBox.Contains(e.X, e.Y)) {
                return;
            }

            // Proceed with the drag and drop, passing in the list item.                    
            dgvChannels.DoDragDrop(GetSelectedRows(), DragDropEffects.Move);
        }


        private void dataGridView1_MouseDown(object sender, MouseEventArgs e) {
            // Get the index of the item the mouse is below.
            _dragDropRowIndex = dgvChannels.HitTest(e.X, e.Y).RowIndex;
            if (_dragDropRowIndex != -1) {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                var dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragDropBox = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragDropBox = Rectangle.Empty;
            }
        }


        private void dataGridView1_DragOver(object sender, DragEventArgs e) {
            // Check that it is a valid drag drop
            var cols = (from DataGridViewCell c in dgvChannels.SelectedCells select c.ColumnIndex).Distinct().ToList();
            e.Effect = (cols.Count() != 1 || cols[0] < ChannelNumCol || cols[0] > OutputChannelCol) ? DragDropEffects.None : DragDropEffects.Move;
        }


        private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
            // If it is not a move, then exit since it is an invalid D&D
            if (e.Effect != DragDropEffects.Move) {
                return;
            }

            // If it is not the right type of data, exit.
            var rows = e.Data.GetData(typeof (HashSet<DataGridViewRow>)) as HashSet<DataGridViewRow>;
            if (rows == null) {
                return;
            }

            // It is a valid D&D, so where did the drop occur
            var dropPoint = dgvChannels.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is over. 
            var dropRowIndex = dgvChannels.HitTest(dropPoint.X, dropPoint.Y).RowIndex;

            // Get the column the D&D impacts
            var dropColumn = (from DataGridViewCell c in dgvChannels.SelectedCells select c.ColumnIndex).Distinct().ToList()[0];
            
            // If it is the Channel Name column, do the channel number first and the the Output Number second
            var valueColumn = dropColumn == ChannelNameCol ? ChannelNumCol : dropColumn;
            
            // Get the data for that column on the dropped row
            var initialValue = int.Parse(dgvChannels.Rows[dropRowIndex].Cells[valueColumn].Value.ToString());

            // Number of rows impacted, first row impacted and last row impacted
            var impactedRowCount = rows.Count - 1;
            var firstRow = Math.Min(initialValue, rows.Min(r => int.Parse(r.Cells[valueColumn].Value.ToString())));
            var lastRow = Math.Max(initialValue, rows.Max(r => int.Parse(r.Cells[valueColumn].Value.ToString())));

            Debug.Print("First: {0} - Last: {1}", firstRow, lastRow);

            //move the channels and renumber the appropriate column
            foreach (var row in rows) {
                dgvChannels.Rows.RemoveAt(dgvChannels.Rows.IndexOf(row));
                row.Cells[valueColumn].Value = firstRow + impactedRowCount--;
                dgvChannels.Rows.Insert(dropRowIndex, row);
            }

            // Renumber the appropriate column
            RenumberChannels(rows, firstRow, lastRow, valueColumn);

            // If it is the name column, do the output channel column renumbering now, passing an empty list.
            if (dropColumn == ChannelNameCol) {
                RenumberChannels(new List<DataGridViewRow>(), firstRow, lastRow, OutputChannelCol);
            }

            _contextProfile.IsDirty = true;

            // Select the cell where the data was dropped
            dgvChannels.CurrentCell = dgvChannels.Rows[dropRowIndex].Cells[dropColumn];
        }


        private void RenumberChannels(ICollection<DataGridViewRow> rows, int firstRow, int lastRow, int col) {
            var rowCounter = rows.Count;

            foreach (var row in from DataGridViewRow row in dgvChannels.Rows 
                                let channelNum = int.Parse(row.Cells[col].Value.ToString()) 
                                where channelNum >= firstRow && channelNum <= lastRow && !rows.Contains(row)
                                select row) {
                row.Cells[col].Value = firstRow + rowCounter++;
            }
        }


        private void btnChColorOne_Click(object sender, EventArgs e) {
            var rows = GetSelectedRows().ToList();
            var color = rows.First().Cells[ChannelColorCol].Value.ToString().FromHTML();
            
            if (!GetColor(sender as Button, color, out color, false)) {
                return;
            }

            SetSingleColor(color, rows);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (AnyDirtyProfiles()) {
                QuerySaveChanges();
            }

            DialogResult = DialogResult.Cancel;
        }


        private bool AnyDirtyProfiles() {
            return cbProfiles.Items.OfType<Profile>().Any(p => p.IsDirty);
        }


        private void dgvChannels_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex != ChannelColorCol || e.RowIndex == -1) {
                return;
            }

            var rows = GetSelectedRows().ToList();
            var color = rows.First().Cells[ChannelColorCol].Value.ToString().FromHTML();
            var cl = dgvChannels.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var cc = new Control(dgvChannels, "hold", cl.Left, cl.Top, cl.Width, cl.Height) {Visible = false};

            try {
                if (!GetColor(cc, color, out color, false)) {
                    return;
                }
            }
            finally {
                dgvChannels.Controls.Remove(cc);
            }
            SetSingleColor(color, rows);
        }


        private void SetSingleColor(Color color, IEnumerable<DataGridViewRow> rows ) {
            var htmlColor = color.ToHTML();
            var foreColor = color.GetForeColor();

            dgvChannels.SuspendLayout();
            foreach (var row in rows.Where(r => r.Cells[ChannelColorCol].Value.ToString() != htmlColor)) {
                row.Cells[ChannelColorCol].Value = htmlColor;
                row.DefaultCellStyle.BackColor = color;
                row.DefaultCellStyle.ForeColor = foreColor;
            }
            dgvChannels.ResumeLayout();
            SetGeneralButtons();
            _contextProfile.IsDirty = true;
        }

        
        private void btnChColorMulti_Click(object sender, EventArgs e) {
            tcControlArea.SelectTab(ControlTabMultiColor);
        }

        
        private void btnMultiColor_Click(object sender, EventArgs e) {
            colorPaletteChannel.ControlChanged -= UpdateColors;
            tcControlArea.SelectTab(ControlTabNormal);
            tcProfile.TabPages[0].Text = "Channels";
            if (((Button)sender).Text == btnMultiColorOk.Text) {
                var count = 0;
                var colors = GetColorList(colorPaletteColor);
                foreach (var row in GetSelectedRows().Reverse()) {
                    var color = colors[count++ % colors.Count];
                    row.Cells[ChannelColorCol].Value = color.ToHTML();
                    row.DefaultCellStyle.BackColor = color;
                    row.DefaultCellStyle.ForeColor = color.GetForeColor();
                }
                _contextProfile.IsDirty = true;
            }
            DoButtonManagement();
        }

        //private void dgvChannels_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
        //    //_contextProfile.IsDirty = true;
        //    //if (e.ColumnIndex == ChannelNameCol && ModifierKeys == Keys.Control) {
        //    //    //TODO Add channel renaming templating.
        //    //}
        //}

        private void PreviewChannelEvent(object sender, EventArgs e) {
            btnUpdatePreview.Enabled = !cbPreview.Checked;
            if (cbPreview.Checked) {
                PreviewChannels();
            }
        }

        private void previewTimer_Tick(object sender, EventArgs e) {
            previewTimer.Stop();
            dgvChannels.SuspendLayout();
            dgvChannels.Rows.Clear();
            AddRows(GenerateChannels(), _contextProfile.FullChannels.Count() + 1);
            dgvChannels.ResumeLayout();
        }

        private void tbChGenNameFormat_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = e.KeyCode == Keys.Enter;
        }

        private void btnUpdatePreview_Click(object sender, EventArgs e) {
            previewTimer_Tick(null, null);
        }


        private void VixenPlusRoadie_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason != CloseReason.UserClosing || !AnyDirtyProfiles()) {
                return;
            }

            QuerySaveChanges();
        }


        private void QuerySaveChanges() {
            if (MessageBox.Show("Save changes before closing?", "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                SaveChangedProfiles();
            }
        }

        private void SaveChangedProfiles() {
            SaveProfileFromRows();
            foreach (var p in cbProfiles.Items.OfType<Profile>().Where(p => p.IsDirty)) {
                p.SaveToFile();
            }
        }

        private void dgvChannels_KeyDown(object sender, KeyEventArgs e) {
            if (btnChDelete.Enabled && e.KeyCode == Keys.Delete) {
                btnChDelete_Click(null, null);
            }
        }
        
        private void dgvChannels_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) {
                return;
            }
            _contextProfile.IsDirty = true;
        }

        private void btnProfileSave_Click(object sender, EventArgs e) {
            SaveProfileFromRows();
            _contextProfile.SaveToFile();
            btnProfileSave.Enabled = false;
        }

#endregion

        #region Plugins tab - initial cut

        private  List<Channel> _channels;
        private  IExecutable _executableObject;
        private  Dictionary<string, Dictionary<int, OutputPort>> _outputPorts;
        private  List<IHardwarePlugin> _sequencePlugins;
        private  SetupData _setupData;
        private Rectangle _collapsedRelativeBounds;
        private Rectangle _expandedRelativeBounds;
        private bool _internalUpdate;
        private int _itemAffectedIndex;
        private int _lastIndex = -1;


        private void PluginListDialog(IExecutable executableObject) {
            _setupData = executableObject.PlugInData;
            _executableObject = executableObject;
            _channels = executableObject.Channels;
            _sequencePlugins = new List<IHardwarePlugin>();
            _outputPorts = new Dictionary<string, Dictionary<int, OutputPort>>();
            Cursor = Cursors.WaitCursor;
            try {
                listViewPlugins.Columns[0].Width = listViewPlugins.Width - 25;
                var group = listViewPlugins.Groups["listViewGroupOutput"];
                var list = OutputPlugins.LoadPlugins();
                if (list != null) {
                    foreach (var item in list.Select(plugin => new ListViewItem(plugin.Name, @group) {Tag = plugin})) {
                        listViewPlugins.Items.Add(item);
                    }
                }
                listViewPlugins.Enabled = listViewPlugins.Items.Count > 0;
                OutputPlugins.VerifyPlugIns(_executableObject);
                _collapsedRelativeBounds = new Rectangle(listViewOutputPorts.Columns[2].Width - (pictureBoxPlus.Width * 2),
                                                         (14 - pictureBoxPlus.Height) / 2, pictureBoxPlus.Width, pictureBoxPlus.Height);
                _expandedRelativeBounds = new Rectangle(listViewOutputPorts.Columns[2].Width - (pictureBoxMinus.Width * 2),
                                                        (14 - pictureBoxMinus.Height) / 2, pictureBoxMinus.Width, pictureBoxMinus.Height);
            }
            finally {
                Cursor = Cursors.Default;
            }
        }


        // Used in Standard Sequence to get all mapped plugins - move?
        [UsedImplicitly]
        public IEnumerable<object> MappedPluginList {
            get {
                return (from XmlNode node in _setupData.GetAllPluginData()
                        let attributes = node.Attributes
                        where attributes != null
                        select new object[] {string.Format("{0} ({1}-{2})", attributes["name"].Value, attributes["from"].Value, attributes["to"].Value), bool.Parse(attributes["enabled"].Value), Convert.ToInt32(attributes["id"].Value)}).ToArray();
            }
        }


        private void buttonPluginSetup_Click(object sender, EventArgs e) {
            PluginSetup();
        }


        private void buttonRemove_Click(object sender, EventArgs e) {
            RemoveSelectedPlugIn();
        }


        private void buttonUse_Click(object sender, EventArgs e) {
            UsePlugin();
        }


        private void checkedListBoxSequencePlugins_DoubleClick(object sender, EventArgs e) {
            if (checkedListBoxSequencePlugins.SelectedIndex == -1) {
                return;
            }

            if (_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex].SupportsLivePreview()) {
                MessageBox.Show("Yup");
            }
            else {
                PluginSetup();
            }
        }


        private void checkedListBoxSequencePlugins_ItemCheck(object sender, ItemCheckEventArgs e) {
            if (e.Index == -1) {
                return;
            }

            var pluginData = _setupData.GetPlugInData(e.Index.ToString(CultureInfo.InvariantCulture)).Attributes;
            if (pluginData != null) {
                pluginData["enabled"].Value = (e.NewValue == CheckState.Checked).ToString();
            }
            UpdateDictionary();
        }


        private void InitializePlugin(IHardwarePlugin plugin, XmlNode setupNode) {
            var eventDrivenOutputPlugIn = plugin as IEventDrivenOutputPlugIn;
            if (eventDrivenOutputPlugIn != null) {
                eventDrivenOutputPlugIn.Initialize(_executableObject, _setupData, setupNode);
            }
        }


        private void listBoxSequencePlugins_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                RemoveSelectedPlugIn();
            }
        }


        private void listBoxSequencePlugins_SelectedIndexChanged(object sender, EventArgs e) {
            if ((_lastIndex != -1) && (checkedListBoxSequencePlugins.SelectedIndex != -1)) {
                UpdatePlugInNodeChannelRanges(_lastIndex.ToString(CultureInfo.InvariantCulture));
            }
            var selectedIndex = checkedListBoxSequencePlugins.SelectedIndex;
            buttonPluginSetup.Enabled = selectedIndex != -1;
            buttonRemove.Enabled = selectedIndex != -1;
            if (selectedIndex != -1) {
                var plugInData = _setupData.GetPlugInData(selectedIndex.ToString(CultureInfo.InvariantCulture));
                if (plugInData.Attributes != null) {
                    textBoxChannelFrom.Text = plugInData.Attributes["from"].Value;
                    textBoxChannelTo.Text = plugInData.Attributes["to"].Value;
                }
            }
            _lastIndex = selectedIndex;
        }


        private void listViewOutputPorts_DrawItem(object sender, DrawListViewItemEventArgs e) {
            e.DrawDefault = false;
        }


        private void listViewOutputPorts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            if ((e.ColumnIndex == 2) && (e.Item.Tag != null)) {
                var tag = (OutputPort) e.Item.Tag;
                if (tag.ReferencingPlugins.Count > 1) {
                    //Image image = tag.IsExpanded ? pictureBoxMinus.Image : pictureBoxPlus.Image;
                    //var point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y);
                    //point.Offset(tag.IsExpanded ? _expandedRelativeBounds.Location : _collapsedRelativeBounds.Location);
                    //e.Graphics.DrawImage(image, point);
                }
            }
            else if (e.ColumnIndex != 0) {
                e.DrawDefault = true;
            }
        }


        private void listViewOutputPorts_MouseDown(object sender, MouseEventArgs e) {
            var info = listViewOutputPorts.HitTest(e.Location);
            if ((info.Item == null) || (info.Item.Tag == null)) {
                return;
            }
            var tag = (OutputPort) info.Item.Tag;
            if ((tag.ReferencingPlugins.Count <= 1) || (info.Item.SubItems.IndexOf(info.SubItem) != 2)) {
                return;
            }
            var pt = new Point(e.Location.X, e.Location.Y);
            pt.Offset(-info.SubItem.Bounds.Location.X, -info.SubItem.Bounds.Location.Y);
            _itemAffectedIndex = info.Item.Index;
            if (tag.IsExpanded) {
                if (!_expandedRelativeBounds.Contains(pt)) {
                    return;
                }
                tag.IsExpanded = false;
                UpdateConfigDisplay();
            }
            else if (_collapsedRelativeBounds.Contains(pt)) {
                tag.IsExpanded = true;
                UpdateConfigDisplay();
            }
        }


        private void listViewPlugins_DoubleClick(object sender, EventArgs e) {
            if (listViewPlugins.SelectedItems.Count > 0) {
                UsePlugin();
            }
        }


        private void listViewPlugins_SelectedIndexChanged(object sender, EventArgs e) {
            buttonUse.Enabled = listViewPlugins.SelectedItems.Count > 0;
        }


        private void PluginListDialog_FormClosing(object sender, EventArgs e) {
            listBoxSequencePlugins_SelectedIndexChanged(null, null);
        }


        private void PluginListDialog_Load(object sender, EventArgs e) {
            PluginListDialog(_contextProfile);
            Cursor = Cursors.WaitCursor;
            try {
                _internalUpdate = true;
                // ReSharper disable PossibleNullReferenceException
                foreach (XmlNode node in _setupData.GetAllPluginData()) {
                    var plugin = (node.Attributes["type"] != null && node.Attributes["type"].Value == SetupData.PluginType.Input.ToString())
                                     ? null //InputPlugins.FindPlugin(node.Attributes["name"].Value, true)
                                     : OutputPlugins.FindPlugin(node.Attributes["name"].Value, true);

                    if (plugin != null) {
                        InitializePlugin(plugin, node);
                        checkedListBoxSequencePlugins.Items.Add(plugin.Name, bool.Parse(node.Attributes["enabled"].Value));
                        _sequencePlugins.Add(plugin);
                    }
                    _internalUpdate = false;
                    UpdateDictionary();
                }
                // ReSharper restore PossibleNullReferenceException
            }
            finally {
                Cursor = Cursors.Default;
            }
        }


        private void PluginSetup() {
            if (checkedListBoxSequencePlugins.SelectedItem == null) {
                return;
            }
            UpdatePlugInNodeChannelRanges(checkedListBoxSequencePlugins.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            try {
                _sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex].Setup();
                UpdateDictionary();
            }
            catch (Exception exception) {
                MessageBox.Show(Resources.PluginInitError + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void RemoveSelectedPlugIn() {
            if ((_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex] == null) || (checkedListBoxSequencePlugins.SelectedIndex == -1)) {
                return;
            }
            _setupData.RemovePlugInData(checkedListBoxSequencePlugins.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            _sequencePlugins.RemoveAt(checkedListBoxSequencePlugins.SelectedIndex);
            checkedListBoxSequencePlugins.Items.RemoveAt(checkedListBoxSequencePlugins.SelectedIndex);
            buttonRemove.Enabled = checkedListBoxSequencePlugins.SelectedIndex != -1;
            UpdateDictionary();
        }


        private void UpdateConfigDisplay() {
            listViewOutputPorts.BeginUpdate();
            listViewOutputPorts.Items.Clear();
            var list = new List<int>();
            foreach (var str in _outputPorts.Keys) {
                var group = listViewOutputPorts.Groups.Add(str, str);
                var dictionary = _outputPorts[str];
                list.Clear();
                list.AddRange(dictionary.Keys);
                list.Sort();
                foreach (var num in list) {
                    ListViewItem item;
                    var port = dictionary[num];
                    if (port.ReferencingPlugins.Count == 1) {
                        item =
                            new ListViewItem(
                                new[] {string.Empty, port.Index.ToString("d"), string.Empty, port.ReferencingPlugins[0].Name}, group);
                    }
                    else if (port.IsExpanded) {
                        item = new ListViewItem(new[] {string.Empty, port.Index.ToString("d"), string.Empty, Resources.Multiple}, group);
                        item.SubItems[3].ForeColor = Color.Pink;
                    }
                    else {
                        item = new ListViewItem(new[] {string.Empty, port.Index.ToString("d"), string.Empty, Resources.Multiple}, group);
                        item.SubItems[3].ForeColor = Color.Red;
                    }
                    item.Tag = port;
                    listViewOutputPorts.Items.Add(item);
                    if (!port.IsExpanded) {
                        continue;
                    }
                    foreach (var plugin in port.ReferencingPlugins) {
                        listViewOutputPorts.Items.Add(new ListViewItem(new[] {string.Empty, string.Empty, string.Empty, plugin.Name}, @group));
                    }
                }
            }
            listViewOutputPorts.EndUpdate();
            if (listViewOutputPorts.Items.Count <= 0) {
                return;
            }
            listViewOutputPorts.EnsureVisible(listViewOutputPorts.Items.Count - 1);
            listViewOutputPorts.EnsureVisible(_itemAffectedIndex);
        }


        private void UpdateDictionary() {
            if (_internalUpdate) {
                return;
            }
            _outputPorts.Clear();
            var num = 0;
            foreach (var plugin in _sequencePlugins) {
                var pluginData = _setupData.GetPlugInData(num.ToString(CultureInfo.InvariantCulture)).Attributes;
                if (pluginData != null && bool.Parse(pluginData["enabled"].Value)) {
                    foreach (var map in plugin.HardwareMap) {
                        Dictionary<int, OutputPort> dictionary;
                        OutputPort port;
                        var key = map.PortTypeName.ToLower().Trim();
                        key = char.ToUpper(key[0]) + key.Substring(1);
                        if (!_outputPorts.TryGetValue(key, out dictionary)) {
                            _outputPorts[key] = dictionary = new Dictionary<int, OutputPort>();
                        }
                        if (!dictionary.TryGetValue(map.PortTypeIndex, out port)) {
                            dictionary[map.PortTypeIndex] = port = new OutputPort(map.PortTypeIndex);
                        }
                        port.ReferencingPlugins.Add(plugin);
                    }
                }
                num++;
            }
            _itemAffectedIndex = 0;
            UpdateConfigDisplay();
        }


        private void UpdatePlugInNodeChannelRanges(string pluginID) {
            int count;
            var plugInData = _setupData.GetPlugInData(pluginID);
            try {
                count = Convert.ToInt32(textBoxChannelFrom.Text);
            }
            catch {
                count = 1;
            }
            if (plugInData.Attributes != null) {
                plugInData.Attributes["from"].Value = count.ToString(CultureInfo.InvariantCulture);
            }
            try {
                count = Convert.ToInt32(textBoxChannelTo.Text);
            }
            catch {
                count = _channels.Count;
            }
            if (plugInData.Attributes != null) {
                plugInData.Attributes["to"].Value = count.ToString(CultureInfo.InvariantCulture);
            }
        }


        private void UsePlugin() {
            if (listViewPlugins.SelectedItems.Count == 0) {
                return;
            }
            var plugIn = OutputPlugins.FindPlugin(((IHardwarePlugin) listViewPlugins.SelectedItems[0].Tag).Name, true);
            var node = _setupData.CreatePlugInData(plugIn);
            Xml.SetAttribute(node, "from", "1");
            Xml.SetAttribute(node, "to", _channels.Count.ToString(CultureInfo.InvariantCulture));
            Cursor = Cursors.WaitCursor;
            try {
                InitializePlugin(plugIn, node);
            }
            catch (Exception exception) {
                MessageBox.Show(string.Format(Resources.PluginSetupErrorInvalidStatePossible, exception.Message), Vendor.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
                Cursor = Cursors.Default;
            }
            checkedListBoxSequencePlugins.Items.Add(plugIn.Name, true);
            _sequencePlugins.Add(plugIn);
        }
        #endregion
    }
}