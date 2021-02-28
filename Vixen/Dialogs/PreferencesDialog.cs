using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using FMOD;

using VixenPlusCommon;
using VixenPlusCommon.Properties;

namespace VixenPlus.Dialogs {
    internal partial class PreferencesDialog : Form {
        private readonly Preference2 _preferences;

        private enum RootNodes {
            General = 0,
            ScreenAndColor,
            NewSequence,
            SequenceEditing,
            SequenceExecution
        }


        public PreferencesDialog() {
            InitializeComponent();

            UpdateFolderLabel();

            Icon = Resources.VixenPlus;
            _preferences = Preference2.GetInstance();
            treeView.Nodes["nodeGeneral"].Tag = generalTab;
            treeView.Nodes["nodeScreen"].Tag = screenTab;
            treeView.Nodes["nodeNewSequenceSettings"].Tag = newSequenceSettingsTab;
            treeView.Nodes["nodeSequenceEditing"].Tag = sequenceEditingTab;
            treeView.Nodes["nodeSequenceExecution"].Tag = sequenceExecutionTab;
            tabControl.SelectedTab = generalTab;
            ReadPreferences(tabControl.TabPages.IndexOf(generalTab));
            PopulateProfileLists();
            PopulateScreens();
            PopulateColors();
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe"))) {
                labelAutoShutdownTime.Enabled = false;
                dateTimePickerAutoShutdownTime.Checked = false;
                dateTimePickerAutoShutdownTime.Enabled = false;
            }
            PopulateAudioDeviceList();
        }

        private void UpdateFolderLabel() {
            using (var file = new StreamReader(Paths.DataDir)) {
                lblCurrentFolder.Text = file.ReadLine();
            }
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            WritePreferences(tabControl.SelectedIndex);
            _preferences.SaveSettings();
        }


        private void PopulateAudioDeviceList() {
            comboBoxDefaultAudioDevice.Items.Add("Use application's default device");
            // ReSharper disable once CoVariantArrayConversion
            comboBoxDefaultAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
        }


        private void WritePreferences(int tabIndex) {
            if (_preferences == null) {
                return;
            }

            switch (tabIndex) {
                case (int) RootNodes.General: {
                    WriteGeneralNodes();
                    return;
                }
                case (int) RootNodes.ScreenAndColor:
                    WirteScreenNodes();
                    return;

                case (int) RootNodes.NewSequence:
                    WriteNewSequenceNodes();
                    return;

                case (int) RootNodes.SequenceEditing: {
                    WirteSequenceEditingNodes();
                    return;
                }
                case (int) RootNodes.SequenceExecution:
                    WriteSequenceExecutionNodes();
                    return;
            }
        }


        private void WriteGeneralNodes() {
            _preferences.SetString("MouseWheelVerticalDelta", textBoxMouseWheelVertical.Text);
            _preferences.SetString("MouseWheelHorizontalDelta", textBoxMouseWheelHorizontal.Text);
            _preferences.SetString("ShutdownTime",
                                   !dateTimePickerAutoShutdownTime.Checked
                                       ? string.Empty
                                       : dateTimePickerAutoShutdownTime.Value.ToString("h:mm tt"));
            _preferences.SetString(Vendor.DomainLS, tbUpdateDomain.Text);
            _preferences.SetString("AutoUpdateCheckFreq", cbUpdateFrequency.SelectedItem.ToString());
            _preferences.SetInteger("HistoryImages", (int) numericUpDownHistoryImages.Value);
            _preferences.SetInteger("RecentFiles", (int) nudRecentFiles.Value);
            _preferences.SetBoolean("AutoSaveToolbars", cbToolbarAutoSave.Checked);
        }


        private void WirteScreenNodes() {
            _preferences.SetString("PrimaryDisplay", cbScreens.SelectedItem.ToString());
            _preferences.SetBoolean("UseCheckmark", cbUseCheckmark.Checked);
        }


        private void WriteNewSequenceNodes() {
            _preferences.SetString("EventPeriod", textBoxEventPeriod.Text);
            _preferences.SetInteger("MinimumLevel", (int) numericUpDownMinimumLevel.Value);
            _preferences.SetInteger("MaximumLevel", (int) numericUpDownMaximumLevel.Value);
            _preferences.SetBoolean("WizardForNewSequences", checkBoxWizardForNewSequences.Checked);
            _preferences.SetString("DefaultProfile",
                                   comboBoxDefaultProfile.SelectedIndex != 0
                                       ? comboBoxDefaultProfile.SelectedItem.ToString()
                                       : string.Empty);
            _preferences.SetInteger("DefaultSequenceAudioDevice", comboBoxDefaultAudioDevice.SelectedIndex - 1);
        }


        private void WirteSequenceEditingNodes() {
            _preferences.SetString("MaxColumnWidth", textBoxMaxColumnWidth.Text);
            _preferences.SetString("MaxRowHeight", textBoxMaxRowHeight.Text);
            var index = textBoxIntensityLargeDelta.Text.Trim().IndexOf('%');
            if (index != -1) {
                textBoxIntensityLargeDelta.Text = textBoxIntensityLargeDelta.Text.Substring(0, index).Trim();
            }
            _preferences.SetString("IntensityLargeDelta", textBoxIntensityLargeDelta.Text);
            _preferences.SetBoolean("EventSequenceAutoSize", checkBoxEventSequenceAutoSize.Checked);
            _preferences.SetBoolean("SaveZoomLevels", checkBoxSaveZoomLevels.Checked);
            _preferences.SetBoolean("ShowSaveConfirmation", checkBoxShowSaveConfirmation.Checked);
            _preferences.SetBoolean("ShowNaturalChannelNumber", checkBoxShowNaturalChannelNumber.Checked);
            _preferences.SetBoolean("FlipScrollBehavior", checkBoxFlipMouseScroll.Checked);
            _preferences.SetString("DefaultSequenceDirectory", textBoxDefaultSequenceSaveDirectory.Text);
            _preferences.SetBoolean("ShowWaveformZeroLine", cbWavefromZeroLine.Checked);
            _preferences.SetBoolean("SilenceProfileErrors", cbProfileErrors.Checked);
        }


        private void WriteSequenceExecutionNodes() {
            _preferences.SetBoolean("ShowPositionMarker", checkBoxShowPositionMarker.Checked);
            _preferences.SetBoolean("AutoScrolling", checkBoxAutoScrolling.Checked);
            _preferences.SetBoolean("SavePlugInDialogPositions", checkBoxSavePlugInDialogPositions.Checked);
            _preferences.SetBoolean("ClearAtEndOfSequence", checkBoxClearAtEndOfSequence.Checked);
            _preferences.SetBoolean("ChannelHighlight", cbChannelHighlight.Checked);
        }


        private void PopulateProfileLists() {
            var boxArray = new[] {comboBoxDefaultProfile};
            foreach (var box in boxArray) {
                var selectedIndex = box.SelectedIndex;
                box.BeginUpdate();
                box.Items.Clear();
                box.Items.AddRange(Directory.GetFiles(Paths.ProfilePath, Vendor.All + Vendor.ProfileExtension).Select(Path.GetFileNameWithoutExtension).ToArray());
                if (selectedIndex < box.Items.Count) {
                    box.SelectedIndex = selectedIndex;
                }
                box.EndUpdate();
            }
            comboBoxDefaultProfile.Items.Insert(0, "None");
        }


        private void PopulateScreens() {
            foreach (var screen in Screen.AllScreens) {
                cbScreens.Items.Add(Preference2.FixDeviceName(screen.DeviceName));
            }
            cbScreens.SelectedIndex = 0;
        }


        private void PopulateColors() {
            lbColorizableItems.Items.Add("Channel Background");
            lbColorizableItems.Items.Add("Crosshair");
            lbColorizableItems.Items.Add("Grid Background");
            lbColorizableItems.Items.Add("Grid Lines");
            lbColorizableItems.Items.Add("Mouse Caret");
            lbColorizableItems.Items.Add("Routine Bitmap");
            lbColorizableItems.Items.Add("Waveform");
            lbColorizableItems.Items.Add("Waveform Background");
            lbColorizableItems.Items.Add("Waveform Zero Line");
        }


        private void ReadPreferences(int tabIndex) {
            if (_preferences == null) {
                return;
            }

            switch (tabIndex) {
                case (int) RootNodes.General:
                    ReadGeneralNodes();
                    return;

                case (int) RootNodes.ScreenAndColor:
                    ReadScreenNodes();
                    return;

                case (int) RootNodes.NewSequence:
                    ReadNewSequenceNodes();
                    return;

                case (int) RootNodes.SequenceEditing:
                    ReadSequenceEditingNodes();
                    return;

                case (int) RootNodes.SequenceExecution:
                    ReadSequenceExecutionNodes();
                    return;
            }
        }


        private void ReadGeneralNodes() {
            textBoxMouseWheelVertical.Text = _preferences.GetString("MouseWheelVerticalDelta");
            textBoxMouseWheelHorizontal.Text = _preferences.GetString("MouseWheelHorizontalDelta");
            var s = _preferences.GetString("ShutdownTime");
            if (s != string.Empty) {
                dateTimePickerAutoShutdownTime.Checked = true;
                dateTimePickerAutoShutdownTime.Value = DateTime.Parse(s);
            }
            tbUpdateDomain.Text = _preferences.GetString(Vendor.DomainLS);
            cbUpdateFrequency.SelectedItem  = _preferences.GetString("AutoUpdateCheckFreq");
            numericUpDownHistoryImages.Value = _preferences.GetInteger("HistoryImages");
            nudRecentFiles.Value = _preferences.GetInteger("RecentFiles");
            cbToolbarAutoSave.Checked = _preferences.GetBoolean("AutoSaveToolbars");
        }


        private void ReadScreenNodes() {
            var primaryDisplay = _preferences.GetString("PrimaryDisplay");
            var screenIndex = cbScreens.Items.IndexOf(primaryDisplay);
            cbScreens.SelectedIndex = primaryDisplay.Length != 0 ? (screenIndex != -1 ? screenIndex : 0) : 0;
            cbUseCheckmark.Checked = _preferences.GetBoolean("UseCheckmark");
        }


        private void ReadNewSequenceNodes() {
            textBoxEventPeriod.Text = _preferences.GetString("EventPeriod");
            numericUpDownMinimumLevel.Value = _preferences.GetInteger("MinimumLevel");
            numericUpDownMaximumLevel.Value = _preferences.GetInteger("MaximumLevel");
            checkBoxWizardForNewSequences.Checked = _preferences.GetBoolean("WizardForNewSequences");
            string str3;
            if (((str3 = _preferences.GetString("DefaultProfile")).Length != 0) &&
                File.Exists(Path.Combine(Paths.ProfilePath, str3 + ".pro"))) {
                comboBoxDefaultProfile.SelectedIndex = comboBoxDefaultProfile.Items.IndexOf(str3);
            }
            else {
                comboBoxDefaultProfile.SelectedIndex = 0;
            }
            var num2 = _preferences.GetInteger("DefaultSequenceAudioDevice") + 1;
            comboBoxDefaultAudioDevice.SelectedIndex = num2 < comboBoxDefaultAudioDevice.Items.Count ? num2 : 0;
        }


        private void ReadSequenceEditingNodes() {
            textBoxMaxColumnWidth.Text = _preferences.GetString("MaxColumnWidth");
            textBoxMaxRowHeight.Text = _preferences.GetString("MaxRowHeight");
            textBoxIntensityLargeDelta.Text = _preferences.GetString("IntensityLargeDelta");
            checkBoxEventSequenceAutoSize.Checked = _preferences.GetBoolean("EventSequenceAutoSize");
            checkBoxSaveZoomLevels.Checked = _preferences.GetBoolean("SaveZoomLevels");
            checkBoxShowSaveConfirmation.Checked = _preferences.GetBoolean("ShowSaveConfirmation");
            checkBoxShowNaturalChannelNumber.Checked = _preferences.GetBoolean("ShowNaturalChannelNumber");
            checkBoxFlipMouseScroll.Checked = _preferences.GetBoolean("FlipScrollBehavior");
            textBoxDefaultSequenceSaveDirectory.Text = _preferences.GetString("DefaultSequenceDirectory");
            cbWavefromZeroLine.Checked = _preferences.GetBoolean("ShowWaveformZeroLine");
            cbProfileErrors.Checked = _preferences.GetBoolean("SilenceProfileErrors");
        }


        private void ReadSequenceExecutionNodes() {
            checkBoxShowPositionMarker.Checked = _preferences.GetBoolean("ShowPositionMarker");
            checkBoxAutoScrolling.Checked = _preferences.GetBoolean("AutoScrolling");
            checkBoxSavePlugInDialogPositions.Checked = _preferences.GetBoolean("SavePlugInDialogPositions");
            checkBoxClearAtEndOfSequence.Checked = _preferences.GetBoolean("ClearAtEndOfSequence");
            cbChannelHighlight.Checked = _preferences.GetBoolean("ChannelHighlight");
        }


        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
            WritePreferences(e.TabPageIndex);
        }


        private void tabControl_Selected(object sender, TabControlEventArgs e) {
            ReadPreferences(e.TabPageIndex);
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e) {
            if (e.Node.Tag != null) {
                tabControl.SelectedTab = (TabPage) e.Node.Tag;
            }
        }


        private void lbColorizableItems_SelectedIndexChanged(object sender, EventArgs e) {
            var item = _preferences.GetString(lbColorizableItems.SelectedItem.ToString().Replace(" ", ""));
            int argb;
            if (Int32.TryParse(item, out argb)) {
                pbColor.BackColor = Color.FromArgb(argb);
            }
        }


        private void pbColor_Click(object sender, EventArgs e) {
            using (var colorDialog = new ColorDialog {AnyColor = true, Color = pbColor.BackColor, FullOpen = true}) {
                colorDialog.CustomColors = _preferences.CustomColors;
                var result = colorDialog.ShowDialog();
                if (result != DialogResult.OK) {
                    return;
                }
                pbColor.BackColor = colorDialog.Color;
                var item = lbColorizableItems.SelectedItem.ToString().Replace(" ", "");
                _preferences.SetString(item, colorDialog.Color.ToArgb().ToString(CultureInfo.InvariantCulture));

                _preferences.CustomColors = colorDialog.CustomColors;
            }
        }

        private void btnSetDataFolder_Click(object sender, EventArgs e) {
            using (var firstRunPath = new FirstRunPathDialog(false)) {
                firstRunPath.ShowDialog();
            }
            UpdateFolderLabel();
        }
    }
}
