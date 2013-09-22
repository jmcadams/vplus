using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonUtils;

using Properties;

using VixenPlus;

namespace VixenEditor {
    public partial class ChannelMapper : Form {
        private Profile _destinationProfile;
        
        #region Class properties, variables and constants.

        private const int MaxChannels = 23;
        private const int YOffset = 0;
        private const int VscrollOffset = 2;
        private const int WheelDelta = 120; // This is defined by MS, but it is a private const in the Control class.
        private const string TextboxNamePrefix = "tb";
        private const string EditButtonTooltip = "Return to editing your destination map";
        private const string EditButtonText = "Edit Map";
        private const string DestHeaderPreviewText = "Destination Channel(s)";

        private readonly string _destinationHeaderText;
        private readonly Label[] _labels = new Label[MaxChannels];
        private readonly string _previewButtonText;
        private readonly string _previewButtonToolTip;
        private readonly int _sourceChannelCount;
        private readonly Profile _sourceProfile;
        private readonly char[] _splitChar = { ',' };
        private readonly TextBox[] _textBoxes = new TextBox[MaxChannels];

        private int _currentTopChannelIndex;
        private string[] _destinationTextBoxText;
        private bool _isPreview;
        private int _maxDestinationCount;
        private int _totalChannelsDisplayed;
        private bool _isDirty;

        public bool IsMapValid { get; set; }

        #endregion


        public ChannelMapper(EventSequence sequence) {
            var sourceSequence = sequence;

            InitializeComponent();

            _sourceProfile = sourceSequence.Profile;
            _sourceChannelCount = sourceSequence.FullChannelCount;
            InitializeDropDownList();
            InitializeScrollbar();

            GetDestinationProfile();

            BuildDynamicComponents();
            InitializeDynamicComponents();
            IsMapValid = false;
            MouseWheel += MapperMouseWheel;
            _previewButtonToolTip = toolTips.GetToolTip(btnPreviewEdit);
            _previewButtonText = btnPreviewEdit.Text;
            _destinationHeaderText = lblDestChannels.Text;
            TogglePreviewElements(true);

            UpdateButtons();
        }

        #region new stuff
        private void InitializeDropDownList() {
            cbSortSrc.Items.Add("Original Order");
            cbSortSrc.Items.Add("Name only");
            cbSortSrc.Items.Add("Color only");
            cbSortSrc.Items.Add("Name, then Color");
            cbSortSrc.Items.Add("Color, then Name");

            cbSortSrc.SelectedIndex = 0;

            cbSortDest.Items.Add("Original Order");
            cbSortDest.Items.Add("Name only");
            cbSortDest.Items.Add("Color only");
            cbSortDest.Items.Add("Name, then Color");
            cbSortDest.Items.Add("Color, then Name");

            cbSortDest.SelectedIndex = 0;
        }

        private void MapperMouseWheel(object sender, MouseEventArgs e) {
            var delta = vsb.Value - (e.Delta / WheelDelta);

            if (delta < vsb.Minimum) {
                delta = vsb.Minimum;
            }
            else if (delta > vsb.Maximum - MaxChannels + VscrollOffset) {
                delta = vsb.Maximum - MaxChannels + VscrollOffset;
            }

            vsb.Value = delta;
        }

        private void BuildDynamicComponents() {
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _labels[i] = NewDefaultLabel(i * (lblEx.Size.Height + YOffset));
                panel1.Controls.Add(_labels[i]);

                _textBoxes[i] = NewDefaultTextBox(i * (tbEx.Size.Height + YOffset));
                _textBoxes[i].Name = TextboxNamePrefix + i;
                panel1.Controls.Add(_textBoxes[i]);
            }
        }

        private void MapperKeyDown(object sender, KeyEventArgs e) {
            var isSuppressed = true;
            var k = e.KeyCode;

            if (((ModifierKeys & Keys.Shift) != Keys.Shift)
                && ((k >= Keys.D0 && k <= Keys.D9)
                    || (k >= Keys.NumPad0 && k <= Keys.NumPad9)
                    || k == Keys.Oemcomma
                    || k == Keys.Left
                    || k == Keys.Right
                    || k == Keys.Delete
                    || k == Keys.Back)
                ) {
                isSuppressed = false;
                if (k != Keys.Right && k != Keys.Left) {
                    _isDirty = true;
                }
            }
            else if ((sender is TextBox) && (k == Keys.Tab || k == Keys.Enter)) {
                HandleEnterTabKeys(((TextBox)sender).Name);
            }

            e.SuppressKeyPress = isSuppressed;
        }

        private void HandleEnterTabKeys(string textBoxName) {
            var textBoxIndex = int.Parse(textBoxName.Remove(0, TextboxNamePrefix.Length));

            var isShiftPressed = ((ModifierKeys & Keys.Shift) == Keys.Shift);

            var isAtTopTextBox = textBoxIndex == 00;
            var isAtTopOfList = isAtTopTextBox && _currentTopChannelIndex == 0;

            var isAtBottomTextBox = textBoxIndex == _totalChannelsDisplayed - 1;
            var isAtBottomOfList = isAtBottomTextBox &&
                                   _sourceChannelCount - _currentTopChannelIndex == _totalChannelsDisplayed;

            if (isShiftPressed) {
                if (isAtTopOfList) {
                    // Make last text box have focus and move to show bottom of list
                    textBoxIndex = _totalChannelsDisplayed - 1;
                    ShowComponents(_sourceChannelCount - _totalChannelsDisplayed);
                }
                else if (isAtTopTextBox) {
                    // Move list down one and stay in same text box
                    ShowComponents(_currentTopChannelIndex - 1);
                }
                else {
                    // Make previous text box have focus
                    textBoxIndex--;
                }
            }
            else {
                if (isAtBottomOfList) {
                    // Make first text box have focus and move to show top of list
                    textBoxIndex = 0;
                    ShowComponents(0);
                }
                else if (isAtBottomTextBox) {
                    //Move list up one and stay in same text box
                    ShowComponents(_currentTopChannelIndex + 1);
                }
                else {
                    // Make next text box have focus
                    textBoxIndex++;
                }
            }

            // Find the control and set the focus to the first matching item.
            var c = (TextBox)Controls.Find(TextboxNamePrefix + textBoxIndex, true)[0];

            c.Focus();
            c.SelectionStart = 0;
            c.SelectionLength = c.Text.Length;
        }

        private static void MapperPreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            // In order to consume the tab key we have to set IsInputKey to true.
            e.IsInputKey = (e.KeyCode == Keys.Tab);
        }

        private void InitializeDynamicComponents() {
            vsb.Height = (lblEx.Height + YOffset) * MaxChannels;
            _destinationTextBoxText = new string[_sourceChannelCount];
            for (var i = 0; i < _sourceChannelCount; i++) {
                _destinationTextBoxText[i] = "";
            }
            ShowComponents(0);
        }

        private void InitializeScrollbar() {
            if (_sourceChannelCount > MaxChannels) {
                _totalChannelsDisplayed = MaxChannels;
                vsb.Enabled = true;
                vsb.Maximum = _sourceChannelCount - VscrollOffset;
                vsb.LargeChange = MaxChannels - 1;
            }
            else {
                _totalChannelsDisplayed = _sourceChannelCount;
                vsb.Enabled = false;
            }
        }

        private void ShowComponents(int start, bool save = true) {
            if (save) SaveCurrentValues();

            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                var source = _sourceProfile.FullChannels[start + row];
                _labels[row].Text = source.Name;
                _labels[row].BackColor = source.Color;
                _labels[row].ForeColor = _labels[row].BackColor.GetForeColor();

                _textBoxes[row].Text = _destinationTextBoxText[source.OutputChannel];

                if (!_isPreview) continue;

                var s = _textBoxes[row].Text.Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
                var col = 0;
                foreach (var channel in s) {
                    var dest = _destinationProfile.FullChannels[int.Parse(channel)];
                    var c = Controls.Find("r" + row + "c" + col, true)[0];
                    c.Text = dest.Name;
                    c.BackColor = dest.Color;
                    c.ForeColor = c.BackColor.GetForeColor();
                    c.Visible = true;
                    col++;
                }
                for (; col < _maxDestinationCount; col++) {
                    Controls.Find("r" + row + "c" + col, true)[0].Visible = false;
                }
            }

            _currentTopChannelIndex = start;
            vsb.Value = start;
        }

        private void SaveCurrentValues() {
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _destinationTextBoxText[_sourceProfile.FullChannels[_currentTopChannelIndex + i].OutputChannel] = _textBoxes[i].Text;
            }
        }

        private Label NewDefaultLabel(int y) {
            return new Label {
                Text = lblEx.Text,
                Size = lblEx.Size,
                Enabled = true,
                Visible = true,
                TextAlign = lblEx.TextAlign,
                Location = new Point(lblEx.Location.X, y),
                AutoSize = lblEx.AutoSize
            };
        }

        private TextBox NewDefaultTextBox(int y) {
            var textBox = new TextBox {
                Text = "",
                Size = tbEx.Size,
                Location = new Point(tbEx.Location.X, y),
                Enabled = true,
                Visible = true
            };

            // Need these events so that we can catch the Tab and Enter key to do special list movement.
            textBox.PreviewKeyDown += MapperPreviewKeyDown;
            textBox.KeyDown += MapperKeyDown;

            return textBox;
        }

        private void VsbScroll(object sender, ScrollEventArgs e) {
            ScrollTo(e.NewValue);
        }

        private void MapperFormClosing(object sender, FormClosingEventArgs e) {
            // Remove all of our event handlers.
            MouseWheel -= MapperMouseWheel;
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].PreviewKeyDown -= MapperPreviewKeyDown;
                _textBoxes[i].KeyDown -= MapperKeyDown;
            }
        }


        private string GetMap() {
            SaveCurrentValues();

            var sortOrder = cbSortSrc.SelectedIndex;
            SetSortOrder(_sourceProfile.Channels, 0);

            var theMap = new StringBuilder();

            for (var i = 0; i < _sourceChannelCount; i++) {
                var src = _sourceProfile.FullChannels[i].OutputChannel;
                theMap.Append(i + ":");
                if (cbKeepUnmapped.Checked && _destinationTextBoxText[src].Equals("")) {
                    theMap.Append(i + " ");
                }
                else {
                    var values = _destinationTextBoxText[src].Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var channel in values) {
                        theMap.Append(channel.Trim() + " ");
                    }
                }
                theMap.Append(';');
            }

            SetSortOrder(_sourceProfile.Channels, sortOrder);

            return theMap.ToString();
        }

        private void VsbValueChanged(object sender, EventArgs e) {
            ScrollTo(vsb.Value);
        }

        private void ScrollTo(int newValue) {
            if (newValue + _totalChannelsDisplayed > _sourceChannelCount) {
                ShowComponents(_sourceChannelCount - _totalChannelsDisplayed);
            }
            else {
                ShowComponents(newValue);
            }
        }

        private void BtnTransformClick(object sender, EventArgs e) {
            SaveCurrentValues();
            SaveToFileIfDirty();

            var destCount = new int[_destinationProfile.FullChannels.Count];

            var badChannels = FindBadChannels(destCount);
            var dupeChannels = FindDupeChannels(destCount);

            if (string.IsNullOrEmpty(badChannels) && string.IsNullOrEmpty(dupeChannels)) {
                IsMapValid = true;
                if (ActiveForm != null) {
                    ActiveForm.DialogResult = DialogResult.OK;
                    if (ActiveForm != null) ActiveForm.Close();
                }
            }
            else {
                IsMapValid = false;
                ShowErrors(dupeChannels, badChannels);
            }
        }

        private string FindDupeChannels(int[] destCount) {
            var destChannelCount = _destinationProfile.FullChannels.Count;

            var dupeChannels = new StringBuilder();
            for (var i = 0; i < destChannelCount; i++) {
                if (destCount[i] <= 1) continue;
                dupeChannels.Append(i).Append(" (" + destCount[i] + @" times), ");
            }

            return dupeChannels.ToString();
        }

        private string FindBadChannels(int[] destCount) {
            var destChannelCount = _destinationProfile.FullChannels.Count;

            var currentChannel = 0;

            var badChannels = new StringBuilder();
            for (var i = 0; i < _sourceChannelCount; i++) {
                var process = true;

                var destChannels = _destinationTextBoxText[_sourceProfile.FullChannels[i].OutputChannel].Split(_splitChar);

                foreach (var channel in destChannels) {
                    if (channel.Equals("") && cbKeepUnmapped.Checked) {
                        currentChannel = i;
                    }

                    if (channel.Equals("") && !cbKeepUnmapped.Checked) {
                        process = false;
                    }

                    if (!channel.Equals("")) {
                        currentChannel = int.Parse(channel);
                    }

                    if (!process) continue;

                    if (currentChannel < destChannelCount) {
                        destCount[currentChannel]++;
                    }
                    else {
                        badChannels.Append(channel).Append(" (from ch " + i + "), ");
                    }
                }
            }

            return badChannels.ToString();
        }


        private void SaveToFileIfDirty() {
            if (_isDirty) {
                if (MessageBox.Show(@"Would you like to save your mapping data before transforming your sequence?",
                                    @"Save Changes?",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question)
                        .Equals(DialogResult.Yes)) {
                    BtnSaveClick(this, new EventArgs());
                }
            }
        }

        private static void ShowErrors(string dupes, string bad) {
            var msg = new StringBuilder(@"Transformation cannot take place because of the follow errors:");
            msg.Append(Environment.NewLine + Environment.NewLine);

            if (dupes.Length > 0) {
                msg.AppendLine(@"Destination channels cannot come from more than one source channel: ").AppendLine(
                    dupes.Remove(dupes.Length - 2)).AppendLine();
            }

            if (bad.Length > 0) {
                msg.AppendLine(@"Bad (out of range) destination channels: ").AppendLine(bad.Remove(bad.Length - 2)).
                    AppendLine();
            }

            msg.Append(@"Please correct these errors and try again.");

            MessageBox.Show(msg.ToString(), @"Errors in map. Can't transform.", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }


        private void BtnSaveClick(object sender, EventArgs e) {
            using (var saveMap = new SaveFileDialog()) {
                if (!saveMap.ShowDialog().Equals(DialogResult.OK)) return;
                using (TextWriter tw = new StreamWriter(saveMap.FileName)) {
                    tw.WriteLine(GetMap());
                    tw.Close();
                    _isDirty = false;
                }
            }
        }


        private void BtnLoadClick(object sender, EventArgs e) {
            using (var openMap = new OpenFileDialog()) {
                openMap.InitialDirectory = Paths.ProfilePath + "Mappers\\";

                if (!openMap.ShowDialog().Equals(DialogResult.OK)) return;

                using (TextReader tr = new StreamReader(openMap.FileName)) {
                    LoadMap(tr.ReadLine());
                    tr.Close();
                    _isDirty = false;
                }
            }
        }


        private void LoadMap(string map) {
            var sortOrder = cbSortSrc.SelectedIndex;
            SetSortOrder(_sourceProfile.Channels, 0);

            var splitChannelChar = new[] { ';' };
            var channels = map.Split(splitChannelChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var channel in channels) {
                var elements = channel.Split(':');

                var sourceChannel = int.Parse(elements[0]);
                _destinationTextBoxText[_sourceProfile.FullChannels[sourceChannel].OutputChannel] = elements.Length > 1
                                                             ? elements[1].Trim().Replace(' ', _splitChar[0])
                                                             : "";
            }

            SetSortOrder(_sourceProfile.Channels, sortOrder);

            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].Text = _destinationTextBoxText[_sourceProfile.FullChannels[i + _currentTopChannelIndex].OutputChannel];
            }
        }

        private void BtnPreviewClick(object sender, EventArgs e) {
            SaveCurrentValues();
            if (btnPreviewEdit.Text.Equals(EditButtonText)) {
                _isPreview = false;
                btnPreviewEdit.Text = _previewButtonText;
                toolTips.SetToolTip(btnPreviewEdit, _previewButtonToolTip);
                lblDestChannels.Text = _destinationHeaderText;

                TeardownPreviewGrid();
            }
            else {
                _isPreview = true;
                btnPreviewEdit.Text = EditButtonText;
                toolTips.SetToolTip(btnPreviewEdit, EditButtonTooltip);
                lblDestChannels.Text = DestHeaderPreviewText;

                ConstructPreviewGrid();
            }
            TogglePreviewElements(!_isPreview);
            ShowComponents(_currentTopChannelIndex);
        }


        private void ConstructPreviewGrid() {
            if (cbKeepUnmapped.Checked) {
                LoadMap(GetMap());
            }

            SetMaxDestinationCount();

            var width = tbEx.Width / _maxDestinationCount;

            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                for (var col = 0; col < _maxDestinationCount; col++) {
                    panel1.Controls.Add(new Label {
                        Name = "r" + row + "c" + col,
                        Height = lblEx.Size.Height,
                        Width = width,
                        Location =
                            new Point(tbEx.Location.X + width * col, row * (lblEx.Height + YOffset)),
                        Text = Name,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Visible = false
                    }
                        );
                }
            }
        }

        private void SetMaxDestinationCount() {
            _maxDestinationCount = 0;
            for (var i = 0; i < _sourceChannelCount; i++) {
                var elementCount = _destinationTextBoxText[_sourceProfile.FullChannels[i].OutputChannel].Split(_splitChar).Length;

                if (_maxDestinationCount < elementCount) {
                    _maxDestinationCount = elementCount;
                }
            }
        }

        private void TeardownPreviewGrid() {
            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                for (var col = 0; col < _maxDestinationCount; col++) {
                    var cc = panel1.Controls.Find("r" + row + "c" + col, true);
                    foreach (var c in cc) {
                        panel1.Controls.Remove(c);
                    }
                }
            }
        }

        private void TogglePreviewElements(bool isVisible) {
            btnLoadMap.Visible = isVisible;
            btnSaveMap.Visible = isVisible;
            cbKeepUnmapped.Visible = _destinationProfile != null && _sourceProfile.FullChannels.Count <= _destinationProfile.FullChannels.Count && isVisible;
            btnCancel.Visible = isVisible;
            btnTransform.Visible = isVisible;
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].Visible = isVisible;
            }
        }

        private void SortSrcIndexChanged(object sender, EventArgs e) {
            SaveCurrentValues();
            SetSortOrder(_sourceProfile.Channels, cbSortSrc.SelectedIndex);
            ShowComponents(_currentTopChannelIndex, false);
        }


        private void SortDestIndexChanged(object sender, EventArgs e) {
            if (_destinationProfile == null) return;

            SaveCurrentValues();
            SetSortOrder(_destinationProfile.Channels, cbSortDest.SelectedIndex);
            PopulateDestinationList();
        }

        private void cbKeepEmpty_CheckedChanged(object sender, EventArgs e) {
            _isDirty = true;
        }
        #endregion

        #region old stuff
        private void PopulateDestinationList() {
            lbDestination.Items.Clear();
            foreach (var channel in _destinationProfile.FullChannels.Sort()) {
                lbDestination.Items.Add(channel);
            }
        }

        private void GetDestinationProfile() {
            using (var openProfile = new OpenFileDialog()) {
                openProfile.Filter = Resources.Profile + @"|*" + Vendor.ProfilExtension;
                openProfile.DefaultExt = Vendor.ProfilExtension.Replace(".", "");
                openProfile.CheckFileExists = true;
                openProfile.Title = @"Select a destination profile";
                openProfile.InitialDirectory = Paths.ProfilePath;
                openProfile.FileName = string.Empty;
                if (openProfile.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _destinationProfile = new Profile(openProfile.FileName);
                PopulateDestinationList();
            }
        }


        private void btnDestProfile_Click(object sender, EventArgs e) {
            GetDestinationProfile();
            UpdateButtons();
        }


        private void UpdateButtons() {
            var enabled = _destinationProfile != null;
            btnLoadMap.Enabled = enabled;
            btnSaveMap.Enabled = enabled;
            btnTransform.Enabled = enabled;
        }


        private void lbDestination_DrawItem(object sender, DrawItemEventArgs e) {
            var listBox = sender as ListBox;
            if (e.Index == -1 || listBox == null) {
                return;
            }

            Channel.DrawItem(listBox, e, (Channel)lbDestination.Items[e.Index]);
        }


        private static void SetSortOrder(List<Channel> channels, int sortType) {
            switch (sortType) {
                case 0:
                    channels.Sort((lhs, rhs) => lhs.OutputChannel.CompareTo(rhs.OutputChannel));
                    break;
                case 1:
                    channels.Sort((lhs, rhs) => String.Compare(lhs.Name, rhs.Name, StringComparison.Ordinal));
                    break;
                case 2:
                    channels.Sort((lhs, rhs) => lhs.Color.ToArgb().CompareTo(rhs.Color.ToArgb()));
                    break;
                case 3:
                    channels.Sort((lhs, rhs) => String.Compare((lhs.Name + lhs.Color.ToString()), (rhs.Name + rhs.Color.ToString()), StringComparison.Ordinal));
                    break;
                case 4:
                    channels.Sort((lhs, rhs) => String.Compare((lhs.Color.ToString() + lhs.Name), (rhs.Color.ToString() + rhs.Name), StringComparison.Ordinal));
                    break;
            }
        }
        #endregion

    }
}
