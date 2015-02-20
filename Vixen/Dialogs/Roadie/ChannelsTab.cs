using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public partial class ChannelsTab : UserControl {

        private readonly Profile _contextProfile;
        private readonly VixenPlusRoadie _parentControl;
        private const int ChannelEnabledCol = 0;
        private const int ChannelNumCol = 1;
        private const int ChannelNameCol = 2;
        private const int OutputChannelCol = 3;
        private const int ChannelColorCol = 4;

        private readonly IList<Rules> _ruleEngines = new List<Rules>();

        private readonly Size _controlPanelSzie;
        private bool _internalUpdate;

        public ChannelsTab(IExecutable iExecutable, VixenPlusRoadie parent) {
            _internalUpdate = true;
            InitializeComponent();

            _parentControl = parent;
            _contextProfile = iExecutable as Profile;
            _controlPanelSzie = new Size(Width - dgvChannels.Width, Height - dgvChannels.Height);

            ShowPanel(pChannels);
            SetParentText("Channels");
            InitializeChannels();
            _internalUpdate = false;
        }


        private void ShowPanel(IDisposable panel) {
            pMultiAdd.Visible = panel.Equals(pMultiAdd);
            pMultiColor.Visible = panel.Equals(pMultiColor);
            pChannels.Visible = panel.Equals(pChannels);
        }


        private void SetParentText(string text) {
            if (_parentControl.InvokeRequired) {
                _parentControl.Invoke(_parentControl.SetText(text));
            }
            else {
                _parentControl.SetText(text);
            }
        }


        private void InitializeChannels() {
            dgvChannels.SuspendLayout();
            dgvChannels.Rows.Clear();
            AddRows(_contextProfile.FullChannels);
            dgvChannels.ResumeLayout();
            dgvChannels.Focus();
        }


        private void AddRows(IEnumerable<Channel> channels, int startCh = 1) {
            dgvChannels.SelectionChanged -= dgvChannels_SelectionChanged;
            _internalUpdate = true;
            dgvChannels.SuspendLayout();
            foreach (var ch in channels) {
                AddRow(ch, startCh++);
            }
            dgvChannels.ResumeLayout();
            _internalUpdate = false;
            dgvChannels.SelectionChanged += dgvChannels_SelectionChanged;
        }


        private void AddRow(Channel ch, int chNum) {
            var row = dgvChannels.Rows.Add(ch.Enabled, chNum, ch.Name, ch.OutputChannel + 1, ch.Color.ToHTML());
            dgvChannels.Rows[row].DefaultCellStyle.BackColor = ch.Color;
            dgvChannels.Rows[row].DefaultCellStyle.ForeColor = ch.Color.GetForeColor();
        }


        private void ChannelsTab_SizeChanged(object sender, EventArgs e) {
            dgvChannels.Width = Width - _controlPanelSzie.Width;
            dgvChannels.Height = Height - _controlPanelSzie.Height;
        }


        private void btnEnableDisable_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (null == button) {
                return;
            }

            var enable = button.Text.Equals(btnChEnable.Text);
            var rows = GetSelectedRows().ToList();
            foreach (var row in rows) {
                row.Cells[ChannelEnabledCol].Value = enable;
                _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1].Enabled = enable;
            }
            dgvChannels.Refresh();
            _contextProfile.IsDirty = true;
            SetChannelTabButtons();
        }


        private IEnumerable<DataGridViewRow> GetSelectedRows() {
            var hashSet = new HashSet<DataGridViewRow>();
            foreach (var cell in
                from DataGridViewCell cell in dgvChannels.SelectedCells where !hashSet.Contains(cell.OwningRow) select cell) {
                hashSet.Add(cell.OwningRow);
            }

            return hashSet;
        }


        private void btnImport_Click(object sender, EventArgs e) {
            using (
                var dialog = new OpenFileDialog {
                    AddExtension = true, CheckFileExists = true, CheckPathExists = true, DefaultExt = Vendor.CsvExtension,
                    Filter = "(CSV Files)|" + Vendor.All + Vendor.CsvExtension, InitialDirectory = Paths.ImportExportPath, Multiselect = false
                }) {

                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                try {
                    using (var file = File.OpenText(dialog.FileName)) {
                        string line;
                        var count = 0;
                        var valid = true;
                        dgvChannels.SuspendLayout();
                        _internalUpdate = true;
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
                                var row = dgvChannels.Rows.Add(cols[ChannelEnabledCol] == bool.TrueString, cols[ChannelNumCol].ToInt(),
                                    cols[ChannelNameCol], cols[OutputChannelCol].ToInt(), cols[ChannelColorCol]);
                                dgvChannels.Rows[row].DefaultCellStyle.BackColor = cols[ChannelColorCol].FromHTML();
                                dgvChannels.Rows[row].DefaultCellStyle.ForeColor = dgvChannels.Rows[row].DefaultCellStyle.BackColor.GetForeColor();
                            }
                        }
                        if (valid) {
                            RefreshContextProfile();
                        }
                        _internalUpdate = false;
                        dgvChannels.ResumeLayout();
                    }
                }
                catch (IOException ioe) {
                    ioe.Message.ShowIoError("Error importing file");
                }
            }
        }


        private void btnExport_Click(object sender, EventArgs e) {
            using (
                var dialog = new SaveFileDialog {
                    InitialDirectory = Paths.ImportExportPath, Filter = "(CSV File)|" + Vendor.All + Vendor.CsvExtension, OverwritePrompt = true,
                    DefaultExt = Vendor.CsvExtension
                }) {

                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }


                try {
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
                catch (IOException ioe) {
                    ioe.Message.ShowIoError("Error exporting file");
                }
            }
        }


        private void btnChAddMulti_Click(object sender, EventArgs e) {
            dgvChannels.Rows.Clear();
            colorPaletteChannel.ControlChanged += UpdateColors;
            ShowPanel(pMultiAdd);
            SetParentText("Preview");
            LoadTemplates();
            SetChannelTabButtons();
            DoRuleButtons();
        }


        private void UpdateColors(object sender, EventArgs e) {
            PreviewChannels();
        }


        private void dgvChannels_SelectionChanged(object sender, EventArgs e) {
            SetChannelTabButtons();
        }



        private void SelectLastRow() {
            dgvChannels.ClearSelection();
            var lastRow = dgvChannels.RowCount - 1;
            if (lastRow < 0) {
                return;
            }
            dgvChannels.Rows[lastRow].Selected = true;
            dgvChannels.FirstDisplayedScrollingRowIndex = lastRow;
        }


        private void LoadTemplates() {
            cbChGenTemplate.Items.Clear();
            foreach (var fileName in
                Directory.GetFiles(Paths.ProfileGeneration, Vendor.All + Vendor.TemplateExtension).Where(
                    file => file.EndsWith(Vendor.TemplateExtension)).Select(Path.GetFileNameWithoutExtension)) {
                cbChGenTemplate.Items.Add(fileName);
            }
            ShowNumbers(false, "");
        }


        private void btnRuleAdd_Click(object sender, EventArgs e) {
            if (cbRuleRules.SelectedIndex == -1) {
                return;
            }
            switch (cbRuleRules.SelectedItem.ToString()) {
                case "Numbers":
                    lbRules.Items.Add(new ProfileManagerNumbers {Start = 1, IsLimited = true, End = 1, Increment = 1, Bounce = false});
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
                cbBounce.Checked = numbers.Bounce;
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
            cbBounce.Visible = isItemSelected && isNumbers;
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

        private const string HoldIndex = "___hold___";

        private void SwapRules(int originalIndex, int newIndex) {
            var totalCount = lbRules.Items.Count - 1;
            if (originalIndex > totalCount || originalIndex < 0 || newIndex > totalCount || newIndex < 0) {
                return;
            }

            var holdItem = lbRules.Items[newIndex];
            lbRules.Items[newIndex] = lbRules.Items[originalIndex];
            lbRules.Items[originalIndex] = holdItem;
            lbRules.SelectedIndex = newIndex;

            if (cbMatchInFormat.Checked && !string.IsNullOrEmpty(tbChGenNameFormat.Text)) {
                var currentText = tbChGenNameFormat.Text;
                tbChGenNameFormat.Text = currentText
                    .Replace(string.Format("{{{0}}}", originalIndex), string.Format("{{{0}}}", HoldIndex))
                    .Replace(string.Format("{{{0}}}", newIndex), string.Format("{{{0}}}", originalIndex))
                    .Replace(string.Format("{{{0}}}", HoldIndex), string.Format("{{{0}}}", newIndex));
            }

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
            if (cbChGenTemplate.SelectedIndex == -1) return;

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
            var resultingNames = new List<string>();

            if (lbRules.Items.Count < ruleNum || currentChannel > totalChannels) {
                return resultingNames;
            }

            var ruleEngine = _ruleEngines[ruleNum - 1];
            if (ruleEngine.Iterations != 0) {
                var generatedNames = new List<string>(ruleEngine.GenerateNames());

                for (var i = 0; (i < ruleEngine.Iterations || ruleEngine.IsUnlimited) && currentChannel + resultingNames.Count < totalChannels; i++) {
                    var parts = new Regex("{" + (ruleNum - 1) + "[:]?[a-zA-Z0-9]*}").Match(nameFormat).ToString().Split(':');
                    var format = parts.Count() == 2 ? "{0:" + parts[1] : "{0}";
                    var replace = parts.Count() == 2 ? "{" + (ruleNum - 1) + ":" + parts[1] : "{" + (ruleNum - 1) + "}";
                    var replacementValue = ruleEngine.IsUnlimited ? ruleEngine.GenerateName(i) : generatedNames[i];
                    int numericReplacement;
                    var formattingResult = nameFormat.Replace(replace,
                        int.TryParse(replacementValue, out numericReplacement) ? string.Format(format, numericReplacement) : replacementValue);

                    // Is this the last rule?
                    if (ruleNum >= _ruleEngines.Count) {
                        resultingNames.Add(formattingResult);
                    }
                    else {
                        resultingNames.AddRange(
                            GenerateNames(ruleNum + 1, formattingResult, currentChannel + resultingNames.Count, totalChannels).ToList());
                    }
                }
            }
            else {
                resultingNames.Add(ruleEngine.GenerateDefaultName());
            }

            return resultingNames;
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



        private void SetChannelTabButtons(bool isProfileLoaded = true) {
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


        private void btnChAddOne_Click(object sender, EventArgs e) {
            var chNum = _contextProfile.FullChannels.Count;
            var ch = new Channel(string.Format("Channel {0}", chNum + 1), Color.White, chNum);
            _contextProfile.AddChannelObject(ch);
            _contextProfile.Freeze();
            _internalUpdate = true;
            AddRow(_contextProfile.FullChannels[chNum], chNum + 1);
            _internalUpdate = false;
            _contextProfile.IsDirty = true;
            SelectLastRow();
        }


        private void btnChDelete_Click(object sender, EventArgs e) {
            var rows = GetSelectedRows().ToList();
            var channels = rows.Select(row => _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1]).ToList();

            _contextProfile.IsDirty = true;

            if (null != _contextProfile) {
                foreach (var c in channels) {
                    _contextProfile.RemoveChannel(c);
                }
                _contextProfile.Freeze();
            }

            dgvChannels.SuspendLayout();
            _internalUpdate = true;
            foreach (var r in rows) {
                dgvChannels.Rows.Remove(r);
            }
            _internalUpdate = false;
            dgvChannels.ResumeLayout();
        }


        private Rectangle _dragDropBox;
        private int _dragDropRowIndex;


        private void dataGridView1_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Right) != MouseButtons.Right || _dragCanceled) {
                return;
            }

            // Start the drag when it moves outside of the initial rectangle.
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
                _dragCanceled = false;
                _dragDropScrollSpeed = DragDropDefaultSpeed;
            }
            else {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragDropBox = Rectangle.Empty;
            }
        }


        private DateTime _lastUpdate = DateTime.Now;
        private const int DragDropMinSpeed = 20; // mills
        private const int DragDropDefaultSpeed = 35; // mills
        private  int _dragDropScrollSpeed = DragDropDefaultSpeed; // mills
        private bool _dragCanceled;


        private void dataGridView1_DragOver(object sender, DragEventArgs e) {
            if (_dragCanceled) return;

            // Check that it is a valid drag drop
            var r = dgvChannels.Bounds;
            var m = dgvChannels.PointToClient(Cursor.Position);

            e.Effect = r.Contains(m.X, m.Y) ? DragDropEffects.Move : DragDropEffects.None;
            var delayTime = DateTime.Now - _lastUpdate;

            // We don't want to scroll too fast, so is it time to scroll again?
            if (delayTime.Milliseconds <= _dragDropScrollSpeed) {
                if (_dragDropScrollSpeed > DragDropMinSpeed) {
                    _dragDropScrollSpeed = Math.Min(DragDropMinSpeed, _dragDropScrollSpeed - (_dragDropScrollSpeed / 3));
                }
                return;
            }

            _lastUpdate = DateTime.Now;
            var rect = Rectangle.Inflate(r, 0, -dgvChannels.Rows[0].Height);
            var firstRowIndex = dgvChannels.FirstDisplayedScrollingRowIndex;
            if (m.Y < rect.Top && firstRowIndex > 0) {
                dgvChannels.FirstDisplayedScrollingRowIndex--;
            }
            else if (m.Y > rect.Bottom && firstRowIndex + dgvChannels.DisplayedRowCount(false) < dgvChannels.RowCount) {
                dgvChannels.FirstDisplayedScrollingRowIndex++;
            }
        }


        private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
            // If it is not a move, then exit since it is an invalid D&D
            if (_dragCanceled || e.Effect != DragDropEffects.Move) {
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
            if (dropRowIndex < 0) {
                dropRowIndex = 0;
            }

            // Get the column the D&D impacts
            var impactedColumn = (from DataGridViewCell c in dgvChannels.SelectedCells select c.ColumnIndex).Distinct().ToList()[0];

            // If it is the Channel Name column, do the channel number first and the the Output Number second
            var valueColumn = impactedColumn == ChannelNameCol ? ChannelNumCol : impactedColumn;

            // Get the data for that column on the dropped row
            var initialValue = int.Parse(dgvChannels.Rows[dropRowIndex].Cells[valueColumn].Value.ToString());

            // Number of rows impacted, first row impacted and last row impacted
            var impactedRowCount = rows.Count - 1;
            var firstRow = Math.Min(initialValue, rows.Min(r => int.Parse(r.Cells[valueColumn].Value.ToString())));
            var lastRow = Math.Max(initialValue, rows.Max(r => int.Parse(r.Cells[valueColumn].Value.ToString())));

            //move the channels and renumber the appropriate column
            foreach (var row in rows) {
                dgvChannels.Rows.RemoveAt(dgvChannels.Rows.IndexOf(row));
                row.Cells[valueColumn].Value = firstRow + impactedRowCount--;
                dgvChannels.Rows.Insert(dropRowIndex, row);
            }

            // Renumber the appropriate column
            RenumberChannels(rows, firstRow, lastRow, valueColumn);

            // If it is the name column, do the output channel column renumbering now, passing an empty list.
            if (impactedColumn == ChannelNameCol) {
                RenumberChannels(new List<DataGridViewRow>(), firstRow, lastRow, OutputChannelCol);
            }

            //now update the _contextProfile
            RefreshContextProfile();

            // Select the cell where the data was dropped
            dgvChannels.CurrentCell = dgvChannels.Rows[dropRowIndex].Cells[impactedColumn];
        }


        private void RefreshContextProfile() {
            _contextProfile.ClearChannels();
            foreach (var ch in from DataGridViewRow row in dgvChannels.Rows
                select
                    new Channel(row.Cells[ChannelNameCol].Value.ToString(), row.DefaultCellStyle.BackColor,
                        int.Parse(row.Cells[OutputChannelCol].Value.ToString()))) {
                _contextProfile.AddChannelObject(ch);
            }
            _contextProfile.Freeze();
            _contextProfile.IsDirty = true;
        }


        private void dgvChannels_QueryContinueDrag(object sender, QueryContinueDragEventArgs e) {
            if (!e.EscapePressed) {
                return;
            }

            e.Action = DragAction.Cancel;
            _dragCanceled = true;
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


        private void SetSingleColor(Color color, IEnumerable<DataGridViewRow> rows) {
            var htmlColor = color.ToHTML();
            var foreColor = color.GetForeColor();

            dgvChannels.SuspendLayout();
            _internalUpdate = true;
            foreach (var row in rows) {
                row.Cells[ChannelColorCol].Value = htmlColor;
                row.DefaultCellStyle.BackColor = color;
                row.DefaultCellStyle.ForeColor = foreColor;
                _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1].Color = color;
            }
            _internalUpdate = false;
            dgvChannels.ResumeLayout();
            _contextProfile.IsDirty = true;
        }


        private void btnChColorMulti_Click(object sender, EventArgs e) {
            ShowPanel(pMultiColor);
        }


        private void btnMultiColor_Click(object sender, EventArgs e) {
            colorPaletteChannel.ControlChanged -= UpdateColors;
            ShowPanel(pChannels);
            SetParentText("Channels");
            if (((Button) sender).Text == btnMultiColorOk.Text) {
                var count = 0;
                var colors = GetColorList(colorPaletteColor);
                foreach (var row in GetSelectedRows().Reverse()) {
                    var color = colors[count++ % colors.Count];
                    row.Cells[ChannelColorCol].Value = color.ToHTML();
                    row.DefaultCellStyle.BackColor = color;
                    row.DefaultCellStyle.ForeColor = color.GetForeColor();
                    _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1].Color = color;
                }
                _contextProfile.IsDirty = true;
            }
            SetChannelTabButtons();
        }


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
            AddRows(GenerateChannels(), _contextProfile.FullChannels.Count + 1);
            dgvChannels.ResumeLayout();
        }


        private void tbChGenNameFormat_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = e.KeyCode == Keys.Enter;
            if (!e.SuppressKeyPress) previewTimer.Start();
        }


        private void btnUpdatePreview_Click(object sender, EventArgs e) {
            previewTimer_Tick(null, null);
        }



        private void dgvChannels_KeyDown(object sender, KeyEventArgs e) {
            if (btnChDelete.Enabled && e.KeyCode == Keys.Delete) {
                btnChDelete_Click(null, null);
            }
        }


        private void dgvChannels_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (_internalUpdate || e.RowIndex == -1 || e.ColumnIndex == -1) {
                return;
            }

            var row = dgvChannels.Rows[e.RowIndex];
            var channel = _contextProfile.FullChannels[int.Parse(row.Cells[ChannelNumCol].Value.ToString()) - 1];

            switch (e.ColumnIndex) {
                case ChannelEnabledCol:
                    channel.Enabled = bool.Parse(row.Cells[ChannelEnabledCol].Value.ToString());
                    _contextProfile.IsDirty = true;
                    break;

                case ChannelNameCol:
                    channel.Name = row.Cells[ChannelNameCol].Value.ToString();
                    _contextProfile.IsDirty = true;
                    break; 

                case ChannelNumCol:
                    //do nothing - immutable
                    break;

                case OutputChannelCol:
                case ChannelColorCol:
                    //do nothing - changes saved elsewhere
                    break;
            }
        }


        private void cbRuleColors_CheckedChanged(object sender, EventArgs e) {
            colorPaletteChannel.Visible = cbRuleColors.Checked && cbRuleColors.Visible;
            PreviewChannels();
        }

        private void btnMultiChannelButton_Click(object sender, EventArgs e) {
            colorPaletteChannel.ControlChanged -= UpdateColors;
            ShowPanel(pChannels);
            SetParentText("Channels");
            _internalUpdate = true;
            dgvChannels.Rows.Clear();
            var addingChannels = null != _contextProfile && ((Button)sender).Text == btnMultiChannelOk.Text;
            if (addingChannels) {
                var generateChannels = GenerateChannels();
                foreach (var channel in generateChannels) {
                    _contextProfile.AddChannelObject(channel);
                }
            }
            if (null != _contextProfile) {
                _contextProfile.Freeze();
                AddRows(_contextProfile.FullChannels);
                if (addingChannels) {
                    SelectLastRow();
                }
            }
            _internalUpdate = false;
            SetChannelTabButtons();
        }

        private void btnClearSettings_Click(object sender, EventArgs e) {
            cbPreview.Checked = true;
            cbChGenTemplate.SelectedIndex = -1;
            nudChGenChannels.Value = 1;
            tbChGenNameFormat.Text = string.Empty;
            cbRuleRules.SelectedIndex = -1;
            lbRules.Items.Clear();
            PreviewChannels();
        }

        private void cbBounce_CheckedChanged(object sender, EventArgs e) {
            ((ProfileManagerNumbers)lbRules.SelectedItem).Bounce = cbBounce.Checked;
            PreviewChannels();
        }
    }
}