using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using CommonControls;

using VixenPlus.Properties;

using ColorDialog = CommonControls.ColorDialog;
 
//todo some things I want to do, keep selected cells between refresh of dgv, refactor massively, compartmentalize controls/tabpages

namespace VixenPlus.Dialogs {
    public partial class FrmProfileManager : Form {

        #region ClassMembers

        private readonly Preference2 _pref = Preference2.GetInstance();

        private readonly bool _suppressErrors;
        private readonly int _dgvWidthDiff;
        private readonly int _dgvHeightDiff;
        private readonly Size _borderSize = SystemInformation.BorderSize;
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

        private const string Warning =
            "\n\nNOTE: While most functions can be undone by pressing cancel in the Profile Manager dialog, this one cannot.";

        private readonly IList<Rules> _ruleEngines = new List<Rules>();

        #endregion

        #region Constructor

        public FrmProfileManager(IExecutable defaultProfile = null) {
            InitializeComponent();
            Icon = Resources.VixenPlus;

            _suppressErrors = Preference2.GetInstance().GetBoolean("SilenceProfileErrors");
            _dgvWidthDiff = Width - dgvChannels.Width;
            _dgvHeightDiff = Height - dgvChannels.Height;
            InitializeControls();

            for (var i = TabPlugins; i <= TabNutcracker; i++) {
                tcProfile.TabPages.Remove(tcProfile.TabPages[1]);
            }

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

            var enable = button.Text.Equals("Enable");
            var rows = GetSelectedRows().ToList();
            foreach (var row in rows) {
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


        private void btnCopyProfile_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(false);
        }


        private void btnDeleteProfile_Click(object sender, EventArgs e) {
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
            DialogResult = DialogResult.OK;
        }


        private void btnRenameProfile_Click(object sender, EventArgs e) {
            RenameOrCopyProfile(true);
        }


        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            //todo need to save any changes before changing index
            dgvChannels.Rows.Clear();

            dgvChannels.SuspendLayout();

            if (0 == cbProfiles.SelectedIndex) {
                _contextProfile = null;
                DoButtonManagement();
                return;
            }

            _contextProfile = (Profile) cbProfiles.SelectedItem;
            DoButtonManagement();

            AddRows(_contextProfile.FullChannels);

            dgvChannels.ResumeLayout();
            dgvChannels.Focus();
        }


        private void AddRows(IEnumerable<Channel> channels, int startCh = 1) {
            Debug.Print(DateTime.Now + "");
            dgvChannels.SelectionChanged -= dgvChannels_SelectionChanged;
            dgvChannels.SuspendLayout();
            foreach (var ch in channels) {
                AddRow(ch, startCh++);
            }
            dgvChannels.ResumeLayout();
            dgvChannels.SelectionChanged += dgvChannels_SelectionChanged;
            Debug.Print(DateTime.Now + "");
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
            tcControlArea.SelectTab(1);
            LoadTemplates();
            DoButtonManagement();
            DoRuleButtons();
        }


        private void btnGeneratorButton_Click(object sender, EventArgs e) {
            tcControlArea.SelectTab(1);
            if (((Button) sender).Text == btnChGenOk.Text) {
                dgvChannels.Rows.Clear();
                foreach (var c in GenerateChannels()) {
                    _contextProfile.AddChannelObject(c);
                }
                AddRows(_contextProfile.FullChannels);
            }
            DoButtonManagement();
        }


        private void LoadTemplates() {
            cbChGenTemplate.Items.Clear();
            foreach (var fileName in
                Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.TemplateExtension).Where(file => file.EndsWith(Vendor.TemplateExtension))
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
            var hasEnabledChannels =
                (from DataGridViewRow x in selectedRows where (bool.Parse(x.Cells[ChannelEnabledCol].Value.ToString())) select x).Any();
            var hasDisabledChannels =
                (from DataGridViewRow x in selectedRows where (!bool.Parse(x.Cells[ChannelEnabledCol].Value.ToString())) select x).Any();

            btnChAddMulti.Enabled = isProfileLoaded;
            btnChAddOne.Enabled = isProfileLoaded; // todo
            btnChColorMulti.Enabled = isProfileLoaded && !oneRowSelected; // todo
            btnChColorOne.Enabled = isProfileLoaded && cellsSelected; // todo
            btnChDisable.Enabled = isProfileLoaded && cellsSelected && hasEnabledChannels;
            btnChEnable.Enabled = isProfileLoaded && cellsSelected && hasDisabledChannels;
            btnChDelete.Enabled = isProfileLoaded && cellsSelected;
            btnChExport.Enabled = isProfileLoaded;
            btnChImport.Enabled = isProfileLoaded;
        }


        private void SetGeneralButtons(bool isProfileLoaded) {
            var isChannelPanel = panelChButtons.Visible;
            btnCancel.Enabled = isChannelPanel;
            btnOkay.Enabled = isChannelPanel;
            btnAddProfile.Enabled = isChannelPanel;
            btnCopyProfile.Enabled = isProfileLoaded && isChannelPanel;
            btnDeleteProfile.Enabled = isProfileLoaded && isChannelPanel;
            btnRenameProfile.Enabled = isProfileLoaded && isChannelPanel;
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
            if (Clipboard.ContainsData(DataFormats.Text)) {
                var s = (string) Clipboard.GetData(DataFormats.Text);
                Clipboard.SetData(DataFormats.Text, s);
                var csv = s.Split(new[] {"\r\n"}, StringSplitOptions.None).ToList();
                foreach (var cc in csv.SelectMany(c => c.Split('\t').ToList())) {
                    Debug.Print("'" + cc + "'");
                }
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
                from DataGridViewCell x in dgvChannels.SelectedCells where !hashSet.Contains(x.OwningRow) select x) {
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
            colorPaletteMulti.Visible = cbRuleColors.Checked && cbRuleColors.Visible;
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
        }


        private void nudRuleEnd_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).End = (int) nudRuleEnd.Value;
        }


        private void nudRuleIncr_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).Increment = (int) nudRuleIncr.Value;
        }


        private void cbRuleEndNum_CheckedChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).IsLimited = cbRuleEndNum.Checked;
            nudRuleEnd.Enabled = cbRuleEndNum.Checked;
        }


        private void tbRuleWords_TextChanged(object sender, EventArgs e) {
            ((ProfileManagerWords) lbRules.SelectedItem).Words = tbRuleWords.Text;
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

            var newName = GetFileName("Template", Paths.ProfilePath, new[] {Vendor.TemplateExtension}, name, "OK");

            if (string.IsNullOrEmpty(newName)) {
                return;
            }

            CreateTemplate().Save(Path.Combine(Paths.ProfilePath, newName + Vendor.TemplateExtension));
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
                palette = colorPaletteMulti.Palette;
            }

            var template = new XElement("Template", new XElement("Channels", nudChGenChannels.Value),
                new XElement("ChannelNameFormat", tbChGenNameFormat.Text), rules, palette);

            return template;
        }


        private void tbChGenNameFormat_TextChanged(object sender, EventArgs e) {
            DoRuleButtons();
        }


        private void cbChGenTemplate_SelectedIndexChanged(object sender, EventArgs e) {
            var template = XElement.Load(Path.Combine(Paths.ProfilePath, cbChGenTemplate.SelectedItem + Vendor.TemplateExtension));
            var element = template.Element("Channels");
            nudChGenChannels.Value = element != null ? int.Parse(element.Value) : 1;

            element = template.Element("ChannelNameFormat");
            tbChGenNameFormat.Text = element != null ? element.Value : string.Empty;

            element = template.Element(ColorPalette.PaletteElement);
            if (element != null && element.Elements().Any()) {
                cbRuleColors.Checked = true;
                colorPaletteMulti.Palette = element;
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
            dgvChannels.SuspendLayout();
            dgvChannels.Rows.Clear();
            AddRows(GenerateChannels(), _contextProfile.FullChannels.Count() + 1);
            dgvChannels.ResumeLayout();
        }


        private IEnumerable<Channel> GenerateChannels() {
            _ruleEngines.Clear();
            foreach (Rules item in lbRules.Items) {
                _ruleEngines.Add(item);
            }

            var generatedNames = GenerateNames(1, tbChGenNameFormat.Text, 0, (int) nudChGenChannels.Value).ToList();
            var generatedChannels = new List<Channel>();
            var startChannelNum = _contextProfile.FullChannels.Count();
            var colors = GetColorList();

            for (var count = 0; count < generatedNames.Count(); count++) {
                generatedChannels.Add(new Channel(generatedNames[count], startChannelNum + count) {Color = colors[count % colors.Count]});
            }

            return generatedChannels;
        }


        private List<Color> GetColorList() {
            var colors = new List<Color>();

            if (cbRuleColors.Checked) {
                colors.AddRange(colorPaletteMulti.Colors.Where(c => c != Color.Transparent).Select(c => c));
            }

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


        //todo refactor to take point instead of control for positioning.
        private bool GetColor(Control ctrl, Color initialColor, out Color resultColor, bool showNone = true) {
            var result = false;
            resultColor = Color.Black;

            var location = ctrl.PointToScreen(new Point(0, 0));

            using (var dialog = new ColorDialog(initialColor, showNone)) {
                dialog.Location = new Point(Math.Max(_borderSize.Width * 4, location.X - dialog.Width - _borderSize.Width),
                    Math.Max(_borderSize.Height * 4, location.Y - dialog.Height - _borderSize.Height));

                dialog.CustomColors = _pref.GetString(Preference2.CustomColorsPreference);
                dialog.ShowDialog();
                _pref.SetString(Preference2.CustomColorsPreference, dialog.CustomColors);

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


        private void btnChAddOne_Click(object sender, EventArgs e) {
            var chNum = _contextProfile.FullChannels.Count;
            var ch = new Channel(string.Format("Channel {0}", chNum + 1), Color.White, chNum);
            _contextProfile.AddChannelObject(ch);
            AddRow(_contextProfile.FullChannels[chNum], chNum + 1);
        }


        private void btnChDelete_Click(object sender, EventArgs e) {
            var rows = GetSelectedRows().ToList();
            var channels = rows.Select(row => _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1]).ToList();

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
            if (_dragDropBox != Rectangle.Empty && !_dragDropBox.Contains(e.X, e.Y)) {

                // Proceed with the drag and drop, passing in the list item.                    
                dgvChannels.DoDragDrop(GetSelectedRows(), DragDropEffects.Move);
            }
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
            e.Effect = DragDropEffects.Move;
        }


        private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            var clientPoint = dgvChannels.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            var rowIndexOfItemUnderMouseToDrop = dgvChannels.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect != DragDropEffects.Move) {
                return;
            }

            var rows = e.Data.GetData(typeof(HashSet<DataGridViewRow>)) as HashSet<DataGridViewRow>;
            if (rows == null) {
                return;
            }

            foreach (var row in rows) {
                dgvChannels.Rows.RemoveAt(dgvChannels.Rows.IndexOf(row));
                dgvChannels.Rows.Insert(rowIndexOfItemUnderMouseToDrop, row);
            }
        }

        private void btnChColorOne_Click(object sender, EventArgs e) {
            var rows = GetSelectedRows().ToList();
            var color = rows.First().Cells[ChannelColorCol].Value.ToString().FromHTML();
            
            if (!GetColor(sender as Button, color, out color, false)) {
                return;
            }

            var htmlColor = color.ToHTML();
            var foreColor = color.GetForeColor();

            dgvChannels.SuspendLayout();
            foreach (var row in rows) {
                row.Cells[ChannelColorCol].Value = htmlColor;
                row.DefaultCellStyle.BackColor = color;
                row.DefaultCellStyle.ForeColor = foreColor;
            }
            dgvChannels.ResumeLayout();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            //todo check if there are changes and then warn if we're going to dump changes.
            DialogResult = DialogResult.Cancel;
        }


        private void dgvChannels_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex != ChannelColorCol) {
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

            var htmlColor = color.ToHTML();
            var foreColor = color.GetForeColor();

            dgvChannels.SuspendLayout();
            foreach (var row in rows) {
                row.Cells[ChannelColorCol].Value = htmlColor;
                row.DefaultCellStyle.BackColor = color;
                row.DefaultCellStyle.ForeColor = foreColor;
            }
            dgvChannels.ResumeLayout();
        }

        private void btnChColorMulti_Click(object sender, EventArgs e) {

        }
    }
}