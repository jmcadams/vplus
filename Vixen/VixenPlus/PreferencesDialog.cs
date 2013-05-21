using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using FMOD;
using Properties;

namespace VixenPlus {
    internal partial class PreferencesDialog : Form {
        private readonly Preference2 _preferences;
        private readonly IUIPlugIn[] _uiPlugins;

        private enum RootNodes {
            General = 0,
            ScreenAndColor,
            NewSequence,
            SequenceEditing,
            SequenceExecution,
            Background,
            RemoteExecution,
            Engine
        }


        public PreferencesDialog(IUIPlugIn[] uiPlugins) {
            InitializeComponent();
            _preferences = Preference2.GetInstance();
            _uiPlugins = uiPlugins;
            foreach (var plugIn in uiPlugins) {
                comboBoxSequenceType.Items.Add(plugIn.FileTypeDescription);
            }
            treeView.Nodes["nodeGeneral"].Tag = generalTab;
            treeView.Nodes["nodeScreen"].Tag = screenTab;
            treeView.Nodes["nodeNewSequenceSettings"].Tag = newSequenceSettingsTab;
            treeView.Nodes["nodeSequenceEditing"].Tag = sequenceEditingTab;
            treeView.Nodes["nodeSequenceExecution"].Tag = sequenceExecutionTab;
            treeView.Nodes["nodeBackgroundItems"].Tag = backgroundItemsTab;
            treeView.Nodes["nodeAdvanced"].Nodes["nodeRemoteExecution"].Tag = remoteExecutionTab;
            treeView.Nodes["nodeAdvanced"].Nodes["nodeEngine"].Tag = engineTab;
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


        private void buttonEngine_Click(object sender, EventArgs e) {
            openFileDialog.Filter = Resources.Filter_EngineAssembly + @"|*.dll";
            openFileDialog.FileName = string.Empty;
            openFileDialog.InitialDirectory = Paths.BinaryPath;
            openFileDialog.Title = Resources.Title_SelectEngine;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                textBoxEngine.Text = Path.GetFileName(openFileDialog.FileName);
            }
        }


        //TODO This is not robust at all, it does not check for valid paths.
        private void buttonLogFilePath_Click(object sender, EventArgs e) {
            var path = string.Empty;
            try {
                path = Path.GetDirectoryName(textBoxLogFilePath.Text);
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch {}
            // ReSharper restore EmptyGeneralCatchClause
            var fileName = Path.GetFileName(textBoxLogFilePath.Text);
            if (path != null && Directory.Exists(path)) {
                folderBrowserDialog.SelectedPath = path;
            }
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                textBoxLogFilePath.Text = Path.Combine(folderBrowserDialog.SelectedPath, string.IsNullOrEmpty(fileName) ? "audio.log" : fileName);
            }
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            WritePreferences(tabControl.SelectedIndex);
            _preferences.SaveSettings();
        }


        private void PopulateAudioDeviceList() {
            comboBoxDefaultAudioDevice.Items.Add("Use application's default device");
            // ReSharper disable CoVariantArrayConversion
            comboBoxDefaultAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
            // ReSharper restore CoVariantArrayConversion
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

                case (int) RootNodes.Background:
                    WirteBackgroundNodes();
                    return;

                case (int) RootNodes.RemoteExecution:
                    WriteRemoteExecutionNodes();
                    return;

                case (int) RootNodes.Engine:
                    WirteEngineNodes();
                    return;
            }
        }


        private void WriteGeneralNodes() {
            _preferences.SetString("TimerCheckFrequency", textBoxTimerCheckFrequency.Text);
            _preferences.SetString("MouseWheelVerticalDelta", textBoxMouseWheelVertical.Text);
            _preferences.SetString("MouseWheelHorizontalDelta", textBoxMouseWheelHorizontal.Text);
            _preferences.SetString("ClientName", textBoxClientName.Text);
            _preferences.SetBoolean("ResetAtStartup", checkBoxResetAtStartup.Checked);
            _preferences.SetString("PreferredSequenceType", _uiPlugins[comboBoxSequenceType.SelectedIndex].FileExtension);
            _preferences.SetString("ShutdownTime",
                                   !dateTimePickerAutoShutdownTime.Checked
                                       ? string.Empty
                                       : dateTimePickerAutoShutdownTime.Value.ToString("h:mm tt"));
            var path = Path.Combine(Paths.DataPath, "no.update");
            if (checkBoxDisableAutoUpdate.Checked) {
                if (!File.Exists(path)) {
                    File.Create(path).Close();
                }
            }
            else if (File.Exists(path)) {
                File.Delete(path);
            }
            _preferences.SetInteger("HistoryImages", (int) numericUpDownHistoryImages.Value);
        }


        private void WirteScreenNodes() {
            _preferences.SetString("PrimaryDisplay", cbScreens.SelectedItem.ToString());
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
            _preferences.SetString("RemoteLibraryHTTPURL", textBoxCurveLibraryHttpUrl.Text);
            _preferences.SetString("RemoteLibraryFTPURL", textBoxCurveLibraryFtpUrl.Text);
            _preferences.SetString("RemoteLibraryFileName", textBoxCurveLibraryFileName.Text);
            _preferences.SetString("DefaultSequenceDirectory", textBoxDefaultSequenceSaveDirectory.Text);
            _preferences.SetBoolean("ShowWaveformZeroLine", cbWavefromZeroLine.Checked);
        }


        private void WriteSequenceExecutionNodes() {
            _preferences.SetBoolean("ShowPositionMarker", checkBoxShowPositionMarker.Checked);
            _preferences.SetBoolean("AutoScrolling", checkBoxAutoScrolling.Checked);
            _preferences.SetBoolean("SavePlugInDialogPositions", checkBoxSavePlugInDialogPositions.Checked);
            _preferences.SetBoolean("ClearAtEndOfSequence", checkBoxClearAtEndOfSequence.Checked);
            _preferences.SetBoolean("LogAudioManual", checkBoxLogManual.Checked);
            _preferences.SetBoolean("LogAudioScheduled", checkBoxLogScheduled.Checked);
            _preferences.SetBoolean("LogAudioMusicPlayer", checkBoxLogMusicPlayer.Checked);
            _preferences.SetString("AudioLogFilePath", textBoxLogFilePath.Text);
        }


        private void WirteBackgroundNodes() {
            _preferences.SetBoolean("EnableBackgroundSequence", checkBoxEnableBackgroundSequence.Checked);
            _preferences.SetString("BackgroundSequenceDelay", textBoxBackgroundSequenceDelay.Text);
            _preferences.SetBoolean("EnableBackgroundMusic", checkBoxEnableBackgroundMusic.Checked);
            _preferences.SetString("BackgroundMusicDelay", textBoxBackgroundMusicDelay.Text);
            _preferences.SetBoolean("EnableMusicFade", checkBoxEnableMusicFade.Checked);
            _preferences.SetString("MusicFadeDuration", textBoxMusicFadeDuration.Text);
        }


        private void WriteRemoteExecutionNodes() {
            if (!radioButtonSyncEmbeddedData.Checked) {
                if (radioButtonSyncDefaultProfileData.Checked || (comboBoxSyncProfile.SelectedItem == null)) {
                    _preferences.SetString("SynchronousData", "Default");
                }
                else {
                    _preferences.SetString("SynchronousData", comboBoxSyncProfile.SelectedItem.ToString());
                }
            }
            else {
                _preferences.SetString("SynchronousData", "Embedded");
            }
            if (radioButtonAsyncSyncObject.Checked) {
                _preferences.SetString("AsynchronousData", "Sync");
            }
            else if (radioButtonAsyncDefaultProfileData.Checked || (comboBoxAsyncProfile.SelectedItem == null)) {
                _preferences.SetString("AsynchronousData", "Default");
            }
            else {
                _preferences.SetString("AsynchronousData", comboBoxAsyncProfile.SelectedItem.ToString());
            }
        }


        private void WirteEngineNodes() {
            _preferences.SetString("SecondaryEngine", Path.GetFileName(textBoxEngine.Text));
        }


        private void PopulateProfileLists() {
            var boxArray = new[] {comboBoxDefaultProfile, comboBoxSyncProfile, comboBoxAsyncProfile};
            var list = new List<string>();
            foreach (var str in Directory.GetFiles(Paths.ProfilePath, "*.pro")) {
                list.Add(Path.GetFileNameWithoutExtension(str));
            }
            foreach (var box in boxArray) {
                var selectedIndex = box.SelectedIndex;
                box.BeginUpdate();
                box.Items.Clear();
                // ReSharper disable CoVariantArrayConversion
                box.Items.AddRange(list.ToArray());
                // ReSharper restore CoVariantArrayConversion
                if (selectedIndex < box.Items.Count) {
                    box.SelectedIndex = selectedIndex;
                }
                box.EndUpdate();
            }
            comboBoxDefaultProfile.Items.Insert(0, "None");
        }


        private void PopulateScreens() {
            foreach (var screen in Screen.AllScreens) {
                cbScreens.Items.Add(screen.DeviceName);
            }
            cbScreens.SelectedIndex = 0;
        }


        private void PopulateColors() {
            lbColorizableItems.Items.Add("Channel Background");
            lbColorizableItems.Items.Add("Crosshair");
            lbColorizableItems.Items.Add("Grid Background");
            lbColorizableItems.Items.Add("Grid Lines");
            lbColorizableItems.Items.Add("Mouse Caret");
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

                case (int) RootNodes.Background:
                    ReadBackgroundNodes();
                    return;

                case (int) RootNodes.RemoteExecution: {
                    ReadRemoteExecutionNodes();
                    return;
                }
                case (int) RootNodes.Engine:
                    ReadEngineNodes();
                    return;
            }
        }


        private void ReadGeneralNodes() {
            textBoxTimerCheckFrequency.Text = _preferences.GetString("TimerCheckFrequency");
            textBoxMouseWheelVertical.Text = _preferences.GetString("MouseWheelVerticalDelta");
            textBoxMouseWheelHorizontal.Text = _preferences.GetString("MouseWheelHorizontalDelta");
            textBoxClientName.Text = _preferences.GetString("ClientName");
            checkBoxResetAtStartup.Checked = _preferences.GetBoolean("ResetAtStartup");
            var str = _preferences.GetString("PreferredSequenceType");
            for (var i = 0; i < _uiPlugins.Length; i++) {
                if (_uiPlugins[i].FileExtension != str) {
                    continue;
                }
                comboBoxSequenceType.SelectedIndex = i;
                break;
            }
            if (comboBoxSequenceType.SelectedIndex == -1) {
                comboBoxSequenceType.SelectedIndex = 0;
            }
            var s = _preferences.GetString("ShutdownTime");
            if (s != string.Empty) {
                dateTimePickerAutoShutdownTime.Checked = true;
                dateTimePickerAutoShutdownTime.Value = DateTime.Parse(s);
            }
            checkBoxDisableAutoUpdate.Checked = File.Exists(Path.Combine(Paths.DataPath, "no.update"));
            numericUpDownHistoryImages.Value = _preferences.GetInteger("HistoryImages");
        }


        private void ReadScreenNodes() {
            var primaryDisplay = _preferences.GetString("PrimaryDisplay");
            cbScreens.SelectedIndex = primaryDisplay.Length != 0 ? cbScreens.Items.IndexOf(primaryDisplay) : 0;
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
            textBoxCurveLibraryHttpUrl.Text = _preferences.GetString("RemoteLibraryHTTPURL");
            textBoxCurveLibraryFtpUrl.Text = _preferences.GetString("RemoteLibraryFTPURL");
            textBoxCurveLibraryFileName.Text = _preferences.GetString("RemoteLibraryFileName");
            textBoxDefaultSequenceSaveDirectory.Text = _preferences.GetString("DefaultSequenceDirectory");
            cbWavefromZeroLine.Checked = _preferences.GetBoolean("ShowWaveformZeroLine");
        }


        private void ReadSequenceExecutionNodes() {
            checkBoxShowPositionMarker.Checked = _preferences.GetBoolean("ShowPositionMarker");
            checkBoxAutoScrolling.Checked = _preferences.GetBoolean("AutoScrolling");
            checkBoxSavePlugInDialogPositions.Checked = _preferences.GetBoolean("SavePlugInDialogPositions");
            checkBoxClearAtEndOfSequence.Checked = _preferences.GetBoolean("ClearAtEndOfSequence");
            checkBoxLogManual.Checked = _preferences.GetBoolean("LogAudioManual");
            checkBoxLogScheduled.Checked = _preferences.GetBoolean("LogAudioScheduled");
            checkBoxLogMusicPlayer.Checked = _preferences.GetBoolean("LogAudioMusicPlayer");
            textBoxLogFilePath.Text = _preferences.GetString("AudioLogFilePath");
        }


        private void ReadBackgroundNodes() {
            checkBoxEnableBackgroundSequence.Checked = _preferences.GetBoolean("EnableBackgroundSequence");
            textBoxBackgroundSequenceDelay.Text = _preferences.GetString("BackgroundSequenceDelay");
            checkBoxEnableBackgroundMusic.Checked = _preferences.GetBoolean("EnableBackgroundMusic");
            textBoxBackgroundMusicDelay.Text = _preferences.GetString("BackgroundMusicDelay");
            checkBoxEnableMusicFade.Checked = _preferences.GetBoolean("EnableMusicFade");
            textBoxMusicFadeDuration.Text = _preferences.GetString("MusicFadeDuration");
        }


        private void ReadRemoteExecutionNodes() {
            var syncType = _preferences.GetString("SynchronousData");
            if (syncType != "Embedded") {
                if (syncType == "Default") {
                    radioButtonSyncDefaultProfileData.Checked = true;
                }
                else {
                    radioButtonSyncProfileData.Checked = true;
                    comboBoxSyncProfile.SelectedIndex = comboBoxSyncProfile.Items.IndexOf(syncType);
                }
            }
            else {
                radioButtonSyncEmbeddedData.Checked = true;
            }
            syncType = _preferences.GetString("AsynchronousData");
            switch (syncType) {
                case "Sync":
                    radioButtonAsyncSyncObject.Checked = true;
                    break;
                case "Default":
                    radioButtonAsyncDefaultProfileData.Checked = true;
                    break;
                default:
                    radioButtonAsyncProfileData.Checked = true;
                    comboBoxAsyncProfile.SelectedIndex = comboBoxAsyncProfile.Items.IndexOf(syncType);
                    break;
            }
        }


        private void ReadEngineNodes() {
            textBoxEngine.Text = Path.GetFileName(_preferences.GetString("SecondaryEngine"));
        }


        private void radioButtonAsyncProfileData_CheckedChanged(object sender, EventArgs e) {
            comboBoxAsyncProfile.Enabled = radioButtonAsyncProfileData.Checked;
            if (comboBoxAsyncProfile.Enabled && (comboBoxAsyncProfile.SelectedIndex == -1)) {
                comboBoxAsyncProfile.SelectedIndex = 0;
            }
        }


        private void radioButtonSyncProfileData_CheckedChanged(object sender, EventArgs e) {
            comboBoxSyncProfile.Enabled = radioButtonSyncProfileData.Checked;
            if (comboBoxSyncProfile.Enabled && (comboBoxSyncProfile.SelectedIndex == -1)) {
                comboBoxSyncProfile.SelectedIndex = 0;
            }
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
                var result = colorDialog.ShowDialog();
                if (result != DialogResult.OK) {
                    return;
                }
                pbColor.BackColor = colorDialog.Color;
                var item = lbColorizableItems.SelectedItem.ToString().Replace(" ", "");
                _preferences.SetString(item, colorDialog.Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
