using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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
        }


        private void FrmProfileManager_Shown(object sender, EventArgs e) {
            DoButtonManagement();
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
                        dgvChannels.Rows[row].DefaultCellStyle.BackColor = cols[ChannelColorCol].FromHTML();
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
            dgvChannels.Focus();
        }

        #endregion
        
        #endregion

        #region Support Methods

        #region Button management

        #region Channel Generator

        private void btnChAddMulti_Click(object sender, EventArgs e) {
            panelChButtons.Visible = false;
            panelChGenerator.Visible = true;
            lblRulePrompt.Text = "";
            tbRuleWords.Visible = false;
            lblRuleStartNum.Visible = false;
            cbRuleEndNum.Visible = false;
            lblRuleIncr.Visible = false;
            nudRuleStart.Visible = false;
            nudRuleEnd.Visible = false;
            nudRuleIncr.Visible = false;
            LoadTemplates();
            DoButtonManagement();
            DoRuleButtons();
        }


        private void btnGenOk_Click(object sender, EventArgs e) {
            panelChButtons.Visible = true;
            panelChGenerator.Visible = false;
            DoButtonManagement();
        }


        private void LoadTemplates() {
            cbChGenTemplate.Items.Clear();
            foreach (var fileName in Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.TemplateExtension)
                .Where(file => file.EndsWith(Vendor.TemplateExtension)).Select(Path.GetFileNameWithoutExtension)) {
                cbChGenTemplate.Items.Add(fileName);
            }
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
            var isChannelPanel = panelChButtons.Visible;
            btnChannelOutputs.Enabled = isProfileLoaded;
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
                var s = (string)Clipboard.GetData(DataFormats.Text);
                Clipboard.SetData(DataFormats.Text, s);
                var csv = s.Split(new []{"\r\n"}, StringSplitOptions.None).ToList();
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

        private void cbRuleColors_CheckedChanged(object sender, EventArgs e) {
            var isVisible = cbRuleColors.Checked && cbRuleColors.Visible;
            pbRuleColor1.Visible = isVisible;
            pbRuleColor2.Visible = isVisible;
            pbRuleColor3.Visible = isVisible;
            pbRuleColor4.Visible = isVisible;
        }

        #endregion

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
        }

        private void nudRuleStart_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers) lbRules.SelectedItem).Start = (int)nudRuleStart.Value;
        }

        private void nudRuleEnd_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers)lbRules.SelectedItem).End = (int)nudRuleEnd.Value;
        }

        private void nudRuleIncr_ValueChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers)lbRules.SelectedItem).Increment = (int)nudRuleIncr.Value;
        }

        private void cbRuleEndNum_CheckedChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers)lbRules.SelectedItem).IsLimited = cbRuleEndNum.Checked;
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
            btnRuleDown.Enabled = index != -1 && index != count  - 1;
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

        // todo Refactor
        private void btnChGenSaveTemplate_Click(object sender, EventArgs e) {
            var newName = string.Empty;
            using (var dialog = new TextQueryDialog("Template name", "What would you like to name this template?", string.Empty)) {
                var showDialog = true;
                while (showDialog) {
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        newName = dialog.Response;
                        showDialog = false;
                        if (!File.Exists(Path.Combine(Paths.ProfilePath, newName + Vendor.TemplateExtension))) {
                            continue;
                        }
                        var overwriteResult = MessageBox.Show(
                            String.Format("Template with the name {0} exists.  Overwrite this profile?", newName), "Overwrite?",
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
                    } else {
                        showDialog = false;
                    }
                }
                
            }
            if (string.IsNullOrEmpty(newName)) {
                return;
            }

            CreateTemplate().Save(Path.Combine(Paths.ProfilePath, newName + Vendor.TemplateExtension));
        }


        private XElement CreateTemplate() {
            var rules = (new XElement("Rules"));
            foreach (Rules rule in lbRules.Items) {
                rules.Add(rule.RuleData);
            }

            var colors = new XElement("Colors");
            if (cbRuleColors.Checked) {
                colors.Add(new XElement("Color1", pbRuleColor1.BackColor.ToHTML()));
                colors.Add(new XElement("Color2", pbRuleColor2.BackColor.ToHTML()));
                colors.Add(new XElement("Color3", pbRuleColor3.BackColor.ToHTML()));
                colors.Add(new XElement("Color4", pbRuleColor4.BackColor.ToHTML()));
            }

            var template = new XElement("Template",
                new XElement("Channels", nudChGenChannels.Value),
                new XElement("ChannelNameFormat", tbChGenNameFormat.Text),
                rules,
                colors);

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

            element = template.Element("Colors");
            if (element != null && element.Elements().Any()) {
                cbRuleColors.Checked = true;
                SetColor(pbRuleColor1, element.Element("Color1"));
                SetColor(pbRuleColor2, element.Element("Color2"));
                SetColor(pbRuleColor3, element.Element("Color3"));
                SetColor(pbRuleColor4, element.Element("Color4"));
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
        }


        private static void SetColor(Control pb, XElement color) {
            if (color == null) {
                return;
            }

            pb.BackColor = color.Value.FromHTML();
            pb.BackgroundImage = pb.BackColor == Color.Transparent ? Resources.none1 : null;
            pb.BackgroundImageLayout = ImageLayout.Center;
        }

        private void pbRuleColor_DoubleClick(object sender, EventArgs e) {
            //todo bring up the color dialog and apply the color.
        }
    }
}