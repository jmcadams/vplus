namespace Vixen {
	using FMOD;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Vixen.Dialogs;

	internal partial class PreferencesDialog : Form {

		private Preference2 m_preferences = null;
		private IUIPlugIn[] m_uiPlugins;

		public PreferencesDialog(Preference2 preferences, IUIPlugIn[] uiPlugins) {
			this.InitializeComponent();
			this.m_preferences = Preference2.GetInstance();
			this.m_uiPlugins = uiPlugins;
			foreach (IUIPlugIn @in in uiPlugins) {
				this.comboBoxSequenceType.Items.Add(@in.FileTypeDescription);
			}
			this.treeView.Nodes["nodeGeneral"].Tag = this.generalTab;
			this.treeView.Nodes["nodeNewSequenceSettings"].Tag = this.newSequenceSettingsTab;
			this.treeView.Nodes["nodeSequenceEditing"].Tag = this.sequenceEditingTab;
			this.treeView.Nodes["nodeSequenceExecution"].Tag = this.sequenceExecutionTab;
			this.treeView.Nodes["nodeBackgroundItems"].Tag = this.backgroundItemsTab;
			this.treeView.Nodes["nodeAdvanced"].Nodes["nodeRemoteExecution"].Tag = this.remoteExecutionTab;
			this.treeView.Nodes["nodeAdvanced"].Nodes["nodeEngine"].Tag = this.engineTab;
			this.tabControl.SelectedTab = this.generalTab;
			this.PopulateTo(this.tabControl.TabPages.IndexOf(this.generalTab));
			this.PopulateProfileLists();
			if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe"))) {
				this.labelAutoShutdownTime.Enabled = false;
				this.dateTimePickerAutoShutdownTime.Checked = false;
				this.dateTimePickerAutoShutdownTime.Enabled = false;
			}
			this.PopulateAudioDeviceList();
		}

		private void buttonCreateProfile_Click(object sender, EventArgs e) {
			ProfileManagerDialog dialog = new ProfileManagerDialog(null);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.PopulateProfileLists();
			}
			dialog.Dispose();
		}

		private void buttonEngine_Click(object sender, EventArgs e) {
			this.openFileDialog.Filter = "Engine assembly|*.dll";
			this.openFileDialog.FileName = string.Empty;
			this.openFileDialog.InitialDirectory = Paths.BinaryPath;
			this.openFileDialog.Title = "Select an engine assembly";
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				this.textBoxEngine.Text = Path.GetFileName(this.openFileDialog.FileName);
			}
		}

		private void buttonLogFilePath_Click(object sender, EventArgs e) {
			string path = string.Empty;
			string fileName = "audio.log";
			try {
				path = Path.GetDirectoryName(this.textBoxLogFilePath.Text);
			}
			catch {
			}
			fileName = Path.GetFileName(this.textBoxLogFilePath.Text);
			if (Directory.Exists(path)) {
				this.folderBrowserDialog.SelectedPath = path;
			}
			if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK) {
				if (fileName != string.Empty) {
					this.textBoxLogFilePath.Text = Path.Combine(this.folderBrowserDialog.SelectedPath, fileName);
				}
				else {
					this.textBoxLogFilePath.Text = Path.Combine(this.folderBrowserDialog.SelectedPath, "audio.log");
				}
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.PopulateFrom(this.tabControl.SelectedIndex);
			this.m_preferences.Flush();
		}

		private void buttonPluginSetup_Click(object sender, EventArgs e) {
		}



		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PreferencesDialog));
		//this.label25.Text = manager.GetString("label25.Text");




		private void PopulateAudioDeviceList() {
			this.comboBoxDefaultAudioDevice.Items.Add("Use application's default device");
			this.comboBoxDefaultAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
		}

		private void PopulateFrom(int tabIndex) {
			if (this.m_preferences != null) {
				switch (tabIndex) {
					case 0: {
							this.m_preferences.SetString("TimerCheckFrequency", this.textBoxTimerCheckFrequency.Text);
							this.m_preferences.SetString("MouseWheelVerticalDelta", this.textBoxMouseWheelVertical.Text);
							this.m_preferences.SetString("MouseWheelHorizontalDelta", this.textBoxMouseWheelHorizontal.Text);
							this.m_preferences.SetString("ClientName", this.textBoxClientName.Text);
							this.m_preferences.SetBoolean("ResetAtStartup", this.checkBoxResetAtStartup.Checked);
							this.m_preferences.SetString("PreferredSequenceType", this.m_uiPlugins[this.comboBoxSequenceType.SelectedIndex].FileExtension);
							if (!this.dateTimePickerAutoShutdownTime.Checked) {
								this.m_preferences.SetString("ShutdownTime", string.Empty);
							}
							else {
								this.m_preferences.SetString("ShutdownTime", this.dateTimePickerAutoShutdownTime.Value.ToString("h:mm tt"));
							}
							string path = Path.Combine(Paths.DataPath, "no.update");
							if (this.checkBoxDisableAutoUpdate.Checked) {
								if (!File.Exists(path)) {
									File.Create(path).Close();
								}
							}
							else if (File.Exists(path)) {
								File.Delete(path);
							}
							this.m_preferences.SetInteger("HistoryImages", (int)this.numericUpDownHistoryImages.Value);
							return;
						}
					case 1:
						this.m_preferences.SetString("EventPeriod", this.textBoxEventPeriod.Text);
						this.m_preferences.SetInteger("MinimumLevel", (int)this.numericUpDownMinimumLevel.Value);
						this.m_preferences.SetInteger("MaximumLevel", (int)this.numericUpDownMaximumLevel.Value);
						this.m_preferences.SetBoolean("WizardForNewSequences", this.checkBoxWizardForNewSequences.Checked);
						if (this.comboBoxDefaultProfile.SelectedIndex != 0) {
							this.m_preferences.SetString("DefaultProfile", this.comboBoxDefaultProfile.SelectedItem.ToString());
						}
						else {
							this.m_preferences.SetString("DefaultProfile", string.Empty);
						}
						this.m_preferences.SetInteger("DefaultSequenceAudioDevice", this.comboBoxDefaultAudioDevice.SelectedIndex - 1);
						return;

					case 2: {
							this.m_preferences.SetString("MaxColumnWidth", this.textBoxMaxColumnWidth.Text);
							this.m_preferences.SetString("MaxRowHeight", this.textBoxMaxRowHeight.Text);
							int index = this.textBoxIntensityLargeDelta.Text.Trim().IndexOf('%');
							if (index != -1) {
								this.textBoxIntensityLargeDelta.Text = this.textBoxIntensityLargeDelta.Text.Substring(0, index).Trim();
							}
							this.m_preferences.SetString("IntensityLargeDelta", this.textBoxIntensityLargeDelta.Text);
							this.m_preferences.SetBoolean("EventSequenceAutoSize", this.checkBoxEventSequenceAutoSize.Checked);
							this.m_preferences.SetBoolean("SaveZoomLevels", this.checkBoxSaveZoomLevels.Checked);
							this.m_preferences.SetBoolean("ShowSaveConfirmation", this.checkBoxShowSaveConfirmation.Checked);
							this.m_preferences.SetBoolean("ShowNaturalChannelNumber", this.checkBoxShowNaturalChannelNumber.Checked);
							this.m_preferences.SetBoolean("FlipScrollBehavior", this.checkBoxFlipMouseScroll.Checked);
							this.m_preferences.SetString("RemoteLibraryHTTPURL", this.textBoxCurveLibraryHttpUrl.Text);
							this.m_preferences.SetString("RemoteLibraryFTPURL", this.textBoxCurveLibraryFtpUrl.Text);
							this.m_preferences.SetString("RemoteLibraryFileName", this.textBoxCurveLibraryFileName.Text);
							this.m_preferences.SetString("DefaultSequenceDirectory", this.textBoxDefaultSequenceSaveDirectory.Text);
							return;
						}
					case 3:
						this.m_preferences.SetBoolean("ShowPositionMarker", this.checkBoxShowPositionMarker.Checked);
						this.m_preferences.SetBoolean("AutoScrolling", this.checkBoxAutoScrolling.Checked);
						this.m_preferences.SetBoolean("SavePlugInDialogPositions", this.checkBoxSavePlugInDialogPositions.Checked);
						this.m_preferences.SetBoolean("ClearAtEndOfSequence", this.checkBoxClearAtEndOfSequence.Checked);
						this.m_preferences.SetBoolean("LogAudioManual", this.checkBoxLogManual.Checked);
						this.m_preferences.SetBoolean("LogAudioScheduled", this.checkBoxLogScheduled.Checked);
						this.m_preferences.SetBoolean("LogAudioMusicPlayer", this.checkBoxLogMusicPlayer.Checked);
						this.m_preferences.SetString("AudioLogFilePath", this.textBoxLogFilePath.Text);
						return;

					case 4:
						this.m_preferences.SetBoolean("EnableBackgroundSequence", this.checkBoxEnableBackgroundSequence.Checked);
						this.m_preferences.SetString("BackgroundSequenceDelay", this.textBoxBackgroundSequenceDelay.Text);
						this.m_preferences.SetBoolean("EnableBackgroundMusic", this.checkBoxEnableBackgroundMusic.Checked);
						this.m_preferences.SetString("BackgroundMusicDelay", this.textBoxBackgroundMusicDelay.Text);
						this.m_preferences.SetBoolean("EnableMusicFade", this.checkBoxEnableMusicFade.Checked);
						this.m_preferences.SetString("MusicFadeDuration", this.textBoxMusicFadeDuration.Text);
						return;

					case 5:
						if (!this.radioButtonSyncEmbeddedData.Checked) {
							if (this.radioButtonSyncDefaultProfileData.Checked || (this.comboBoxSyncProfile.SelectedItem == null)) {
								this.m_preferences.SetString("SynchronousData", "Default");
							}
							else {
								this.m_preferences.SetString("SynchronousData", this.comboBoxSyncProfile.SelectedItem.ToString());
							}
						}
						else {
							this.m_preferences.SetString("SynchronousData", "Embedded");
						}
						if (this.radioButtonAsyncSyncObject.Checked) {
							this.m_preferences.SetString("AsynchronousData", "Sync");
						}
						else if (this.radioButtonAsyncDefaultProfileData.Checked || (this.comboBoxAsyncProfile.SelectedItem == null)) {
							this.m_preferences.SetString("AsynchronousData", "Default");
						}
						else {
							this.m_preferences.SetString("AsynchronousData", this.comboBoxAsyncProfile.SelectedItem.ToString());
						}
						return;

					case 6:
						this.m_preferences.SetString("SecondaryEngine", Path.GetFileName(this.textBoxEngine.Text));
						return;
				}
			}
		}

		private void PopulateProfileLists() {
			ComboBox[] boxArray = new ComboBox[] { this.comboBoxDefaultProfile, this.comboBoxSyncProfile, this.comboBoxAsyncProfile };
			List<string> list = new List<string>();
			foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro")) {
				list.Add(Path.GetFileNameWithoutExtension(str));
			}
			foreach (ComboBox box in boxArray) {
				int selectedIndex = box.SelectedIndex;
				box.BeginUpdate();
				box.Items.Clear();
				box.Items.AddRange(list.ToArray());
				if (selectedIndex < box.Items.Count) {
					box.SelectedIndex = selectedIndex;
				}
				box.EndUpdate();
			}
			this.comboBoxDefaultProfile.Items.Insert(0, "None");
		}

		private void PopulateTo(int tabIndex) {
			if (this.m_preferences != null) {
				switch (tabIndex) {
					case 0: {
							this.textBoxTimerCheckFrequency.Text = this.m_preferences.GetString("TimerCheckFrequency");
							this.textBoxMouseWheelVertical.Text = this.m_preferences.GetString("MouseWheelVerticalDelta");
							this.textBoxMouseWheelHorizontal.Text = this.m_preferences.GetString("MouseWheelHorizontalDelta");
							this.textBoxClientName.Text = this.m_preferences.GetString("ClientName");
							this.checkBoxResetAtStartup.Checked = this.m_preferences.GetBoolean("ResetAtStartup");
							string str = this.m_preferences.GetString("PreferredSequenceType");
							for (int i = 0; i < this.m_uiPlugins.Length; i++) {
								if (this.m_uiPlugins[i].FileExtension == str) {
									this.comboBoxSequenceType.SelectedIndex = i;
									break;
								}
							}
							if (this.comboBoxSequenceType.SelectedIndex == -1) {
								this.comboBoxSequenceType.SelectedIndex = 0;
							}
							string s = this.m_preferences.GetString("ShutdownTime");
							if (s != string.Empty) {
								this.dateTimePickerAutoShutdownTime.Checked = true;
								this.dateTimePickerAutoShutdownTime.Value = DateTime.Parse(s);
							}
							this.checkBoxDisableAutoUpdate.Checked = File.Exists(Path.Combine(Paths.DataPath, "no.update"));
							this.numericUpDownHistoryImages.Value = this.m_preferences.GetInteger("HistoryImages");
							return;
						}
					case 1: {
							this.textBoxEventPeriod.Text = this.m_preferences.GetString("EventPeriod");
							this.numericUpDownMinimumLevel.Value = this.m_preferences.GetInteger("MinimumLevel");
							this.numericUpDownMaximumLevel.Value = this.m_preferences.GetInteger("MaximumLevel");
							this.checkBoxWizardForNewSequences.Checked = this.m_preferences.GetBoolean("WizardForNewSequences");
							string str3 = string.Empty;
							if (((str3 = this.m_preferences.GetString("DefaultProfile")).Length != 0) && File.Exists(Path.Combine(Paths.ProfilePath, str3 + ".pro"))) {
								this.comboBoxDefaultProfile.SelectedIndex = this.comboBoxDefaultProfile.Items.IndexOf(str3);
							}
							else {
								this.comboBoxDefaultProfile.SelectedIndex = 0;
							}
							int num2 = this.m_preferences.GetInteger("DefaultSequenceAudioDevice") + 1;
							if (num2 < this.comboBoxDefaultAudioDevice.Items.Count) {
								this.comboBoxDefaultAudioDevice.SelectedIndex = num2;
							}
							else {
								this.comboBoxDefaultAudioDevice.SelectedIndex = 0;
							}
							return;
						}
					case 2:
						this.textBoxMaxColumnWidth.Text = this.m_preferences.GetString("MaxColumnWidth");
						this.textBoxMaxRowHeight.Text = this.m_preferences.GetString("MaxRowHeight");
						this.textBoxIntensityLargeDelta.Text = this.m_preferences.GetString("IntensityLargeDelta");
						this.checkBoxEventSequenceAutoSize.Checked = this.m_preferences.GetBoolean("EventSequenceAutoSize");
						this.checkBoxSaveZoomLevels.Checked = this.m_preferences.GetBoolean("SaveZoomLevels");
						this.checkBoxShowSaveConfirmation.Checked = this.m_preferences.GetBoolean("ShowSaveConfirmation");
						this.checkBoxShowNaturalChannelNumber.Checked = this.m_preferences.GetBoolean("ShowNaturalChannelNumber");
						this.checkBoxFlipMouseScroll.Checked = this.m_preferences.GetBoolean("FlipScrollBehavior");
						this.textBoxCurveLibraryHttpUrl.Text = this.m_preferences.GetString("RemoteLibraryHTTPURL");
						this.textBoxCurveLibraryFtpUrl.Text = this.m_preferences.GetString("RemoteLibraryFTPURL");
						this.textBoxCurveLibraryFileName.Text = this.m_preferences.GetString("RemoteLibraryFileName");
						this.textBoxDefaultSequenceSaveDirectory.Text = this.m_preferences.GetString("DefaultSequenceDirectory");
						return;

					case 3:
						this.checkBoxShowPositionMarker.Checked = this.m_preferences.GetBoolean("ShowPositionMarker");
						this.checkBoxAutoScrolling.Checked = this.m_preferences.GetBoolean("AutoScrolling");
						this.checkBoxSavePlugInDialogPositions.Checked = this.m_preferences.GetBoolean("SavePlugInDialogPositions");
						this.checkBoxClearAtEndOfSequence.Checked = this.m_preferences.GetBoolean("ClearAtEndOfSequence");
						this.checkBoxLogManual.Checked = this.m_preferences.GetBoolean("LogAudioManual");
						this.checkBoxLogScheduled.Checked = this.m_preferences.GetBoolean("LogAudioScheduled");
						this.checkBoxLogMusicPlayer.Checked = this.m_preferences.GetBoolean("LogAudioMusicPlayer");
						this.textBoxLogFilePath.Text = this.m_preferences.GetString("AudioLogFilePath");
						return;

					case 4:
						this.checkBoxEnableBackgroundSequence.Checked = this.m_preferences.GetBoolean("EnableBackgroundSequence");
						this.textBoxBackgroundSequenceDelay.Text = this.m_preferences.GetString("BackgroundSequenceDelay");
						this.checkBoxEnableBackgroundMusic.Checked = this.m_preferences.GetBoolean("EnableBackgroundMusic");
						this.textBoxBackgroundMusicDelay.Text = this.m_preferences.GetString("BackgroundMusicDelay");
						this.checkBoxEnableMusicFade.Checked = this.m_preferences.GetBoolean("EnableMusicFade");
						this.textBoxMusicFadeDuration.Text = this.m_preferences.GetString("MusicFadeDuration");
						return;

					case 5: {
							string str4 = this.m_preferences.GetString("SynchronousData");
							if (!(str4 == "Embedded")) {
								if (str4 == "Default") {
									this.radioButtonSyncDefaultProfileData.Checked = true;
								}
								else {
									this.radioButtonSyncProfileData.Checked = true;
									this.comboBoxSyncProfile.SelectedIndex = this.comboBoxSyncProfile.Items.IndexOf(str4);
								}
							}
							else {
								this.radioButtonSyncEmbeddedData.Checked = true;
							}
							str4 = this.m_preferences.GetString("AsynchronousData");
							if (str4 == "Sync") {
								this.radioButtonAsyncSyncObject.Checked = true;
							}
							else if (str4 == "Default") {
								this.radioButtonAsyncDefaultProfileData.Checked = true;
							}
							else {
								this.radioButtonAsyncProfileData.Checked = true;
								this.comboBoxAsyncProfile.SelectedIndex = this.comboBoxAsyncProfile.Items.IndexOf(str4);
							}
							return;
						}
					case 6:
						this.textBoxEngine.Text = Path.GetFileName(this.m_preferences.GetString("SecondaryEngine"));
						return;
				}
			}
		}

		private void radioButtonAsyncProfileData_CheckedChanged(object sender, EventArgs e) {
			this.comboBoxAsyncProfile.Enabled = this.radioButtonAsyncProfileData.Checked;
			if (this.comboBoxAsyncProfile.Enabled && (this.comboBoxAsyncProfile.SelectedIndex == -1)) {
				this.comboBoxAsyncProfile.SelectedIndex = 0;
			}
		}

		private void radioButtonSyncProfileData_CheckedChanged(object sender, EventArgs e) {
			this.comboBoxSyncProfile.Enabled = this.radioButtonSyncProfileData.Checked;
			if (this.comboBoxSyncProfile.Enabled && (this.comboBoxSyncProfile.SelectedIndex == -1)) {
				this.comboBoxSyncProfile.SelectedIndex = 0;
			}
		}

		private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
			this.PopulateFrom(e.TabPageIndex);
		}

		private void tabControl_Selected(object sender, TabControlEventArgs e) {
			this.PopulateTo(e.TabPageIndex);
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e) {
			if (e.Node.Tag != null) {
				this.tabControl.SelectedTab = (TabPage)e.Node.Tag;
			}
		}
	}
}

