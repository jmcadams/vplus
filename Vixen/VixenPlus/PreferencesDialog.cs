using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FMOD;

namespace VixenPlus
{
	internal partial class PreferencesDialog : Form
	{
		private readonly Preference2 _preferences;
		private readonly IUIPlugIn[] _uiPlugins;

		public PreferencesDialog(IUIPlugIn[] uiPlugins)
		{
			InitializeComponent();
			_preferences = Preference2.GetInstance();
			_uiPlugins = uiPlugins;
			foreach (IUIPlugIn @in in uiPlugins)
			{
				comboBoxSequenceType.Items.Add(@in.FileTypeDescription);
			}
			treeView.Nodes["nodeGeneral"].Tag = generalTab;
			treeView.Nodes["nodeNewSequenceSettings"].Tag = newSequenceSettingsTab;
			treeView.Nodes["nodeSequenceEditing"].Tag = sequenceEditingTab;
			treeView.Nodes["nodeSequenceExecution"].Tag = sequenceExecutionTab;
			treeView.Nodes["nodeBackgroundItems"].Tag = backgroundItemsTab;
			treeView.Nodes["nodeAdvanced"].Nodes["nodeRemoteExecution"].Tag = remoteExecutionTab;
			treeView.Nodes["nodeAdvanced"].Nodes["nodeEngine"].Tag = engineTab;
			tabControl.SelectedTab = generalTab;
			PopulateTo(tabControl.TabPages.IndexOf(generalTab));
			PopulateProfileLists();
			if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe")))
			{
				labelAutoShutdownTime.Enabled = false;
				dateTimePickerAutoShutdownTime.Checked = false;
				dateTimePickerAutoShutdownTime.Enabled = false;
			}
			PopulateAudioDeviceList();
		}

		private void buttonEngine_Click(object sender, EventArgs e)
		{
			openFileDialog.Filter = "Engine assembly|*.dll";
			openFileDialog.FileName = string.Empty;
			openFileDialog.InitialDirectory = Paths.BinaryPath;
			openFileDialog.Title = "Select an engine assembly";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxEngine.Text = Path.GetFileName(openFileDialog.FileName);
			}
		}

		private void buttonLogFilePath_Click(object sender, EventArgs e)
		{
			string path = string.Empty;
			string fileName = "audio.log"; //TODO This is overwritten below without being used.
			try
			{
				path = Path.GetDirectoryName(textBoxLogFilePath.Text);
			}
			catch
			{
			}
			fileName = Path.GetFileName(textBoxLogFilePath.Text);
			if (path != null && Directory.Exists(path))
			{
				folderBrowserDialog.SelectedPath = path;
			}
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxLogFilePath.Text = Path.Combine(folderBrowserDialog.SelectedPath,
				                                       string.IsNullOrEmpty(fileName) ? "audio.log" : fileName);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			PopulateFrom(tabControl.SelectedIndex);
			_preferences.Flush();
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PreferencesDialog));
		//this.label25.Text = manager.GetString("label25.Text");


		private void PopulateAudioDeviceList()
		{
			comboBoxDefaultAudioDevice.Items.Add("Use application's default device");
			comboBoxDefaultAudioDevice.Items.AddRange(new object[] {fmod.GetSoundDeviceList()});
		}

		private void PopulateFrom(int tabIndex)
		{
			if (_preferences != null)
			{
				switch (tabIndex)
				{
					case 0:
						{
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
							string path = Path.Combine(Paths.DataPath, "no.update");
							if (checkBoxDisableAutoUpdate.Checked)
							{
								if (!File.Exists(path))
								{
									File.Create(path).Close();
								}
							}
							else if (File.Exists(path))
							{
								File.Delete(path);
							}
							_preferences.SetInteger("HistoryImages", (int) numericUpDownHistoryImages.Value);
							return;
						}
					case 1:
						_preferences.SetString("EventPeriod", textBoxEventPeriod.Text);
						_preferences.SetInteger("MinimumLevel", (int) numericUpDownMinimumLevel.Value);
						_preferences.SetInteger("MaximumLevel", (int) numericUpDownMaximumLevel.Value);
						_preferences.SetBoolean("WizardForNewSequences", checkBoxWizardForNewSequences.Checked);
						_preferences.SetString("DefaultProfile",
						                       comboBoxDefaultProfile.SelectedIndex != 0
							                       ? comboBoxDefaultProfile.SelectedItem.ToString()
							                       : string.Empty);
						_preferences.SetInteger("DefaultSequenceAudioDevice", comboBoxDefaultAudioDevice.SelectedIndex - 1);
						return;

					case 2:
						{
							_preferences.SetString("MaxColumnWidth", textBoxMaxColumnWidth.Text);
							_preferences.SetString("MaxRowHeight", textBoxMaxRowHeight.Text);
							int index = textBoxIntensityLargeDelta.Text.Trim().IndexOf('%');
							if (index != -1)
							{
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
							return;
						}
					case 3:
						_preferences.SetBoolean("ShowPositionMarker", checkBoxShowPositionMarker.Checked);
						_preferences.SetBoolean("AutoScrolling", checkBoxAutoScrolling.Checked);
						_preferences.SetBoolean("SavePlugInDialogPositions", checkBoxSavePlugInDialogPositions.Checked);
						_preferences.SetBoolean("ClearAtEndOfSequence", checkBoxClearAtEndOfSequence.Checked);
						_preferences.SetBoolean("LogAudioManual", checkBoxLogManual.Checked);
						_preferences.SetBoolean("LogAudioScheduled", checkBoxLogScheduled.Checked);
						_preferences.SetBoolean("LogAudioMusicPlayer", checkBoxLogMusicPlayer.Checked);
						_preferences.SetString("AudioLogFilePath", textBoxLogFilePath.Text);
						return;

					case 4:
						_preferences.SetBoolean("EnableBackgroundSequence", checkBoxEnableBackgroundSequence.Checked);
						_preferences.SetString("BackgroundSequenceDelay", textBoxBackgroundSequenceDelay.Text);
						_preferences.SetBoolean("EnableBackgroundMusic", checkBoxEnableBackgroundMusic.Checked);
						_preferences.SetString("BackgroundMusicDelay", textBoxBackgroundMusicDelay.Text);
						_preferences.SetBoolean("EnableMusicFade", checkBoxEnableMusicFade.Checked);
						_preferences.SetString("MusicFadeDuration", textBoxMusicFadeDuration.Text);
						return;

					case 5:
						if (!radioButtonSyncEmbeddedData.Checked)
						{
							if (radioButtonSyncDefaultProfileData.Checked || (comboBoxSyncProfile.SelectedItem == null))
							{
								_preferences.SetString("SynchronousData", "Default");
							}
							else
							{
								_preferences.SetString("SynchronousData", comboBoxSyncProfile.SelectedItem.ToString());
							}
						}
						else
						{
							_preferences.SetString("SynchronousData", "Embedded");
						}
						if (radioButtonAsyncSyncObject.Checked)
						{
							_preferences.SetString("AsynchronousData", "Sync");
						}
						else if (radioButtonAsyncDefaultProfileData.Checked || (comboBoxAsyncProfile.SelectedItem == null))
						{
							_preferences.SetString("AsynchronousData", "Default");
						}
						else
						{
							_preferences.SetString("AsynchronousData", comboBoxAsyncProfile.SelectedItem.ToString());
						}
						return;

					case 6:
						_preferences.SetString("SecondaryEngine", Path.GetFileName(textBoxEngine.Text));
						return;
				}
			}
		}

		private void PopulateProfileLists()
		{
			var boxArray = new[] {comboBoxDefaultProfile, comboBoxSyncProfile, comboBoxAsyncProfile};
			var list = new List<string>();
			foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro"))
			{
				list.Add(Path.GetFileNameWithoutExtension(str));
			}
			foreach (ComboBox box in boxArray)
			{
				int selectedIndex = box.SelectedIndex;
				box.BeginUpdate();
				box.Items.Clear();
				box.Items.AddRange(new object[] {list.ToArray()});
				if (selectedIndex < box.Items.Count)
				{
					box.SelectedIndex = selectedIndex;
				}
				box.EndUpdate();
			}
			comboBoxDefaultProfile.Items.Insert(0, "None");
		}

		private void PopulateTo(int tabIndex)
		{
			if (_preferences != null)
			{
				switch (tabIndex)
				{
					case 0:
						{
							textBoxTimerCheckFrequency.Text = _preferences.GetString("TimerCheckFrequency");
							textBoxMouseWheelVertical.Text = _preferences.GetString("MouseWheelVerticalDelta");
							textBoxMouseWheelHorizontal.Text = _preferences.GetString("MouseWheelHorizontalDelta");
							textBoxClientName.Text = _preferences.GetString("ClientName");
							checkBoxResetAtStartup.Checked = _preferences.GetBoolean("ResetAtStartup");
							string str = _preferences.GetString("PreferredSequenceType");
							for (int i = 0; i < _uiPlugins.Length; i++)
							{
								if (_uiPlugins[i].FileExtension == str)
								{
									comboBoxSequenceType.SelectedIndex = i;
									break;
								}
							}
							if (comboBoxSequenceType.SelectedIndex == -1)
							{
								comboBoxSequenceType.SelectedIndex = 0;
							}
							string s = _preferences.GetString("ShutdownTime");
							if (s != string.Empty)
							{
								dateTimePickerAutoShutdownTime.Checked = true;
								dateTimePickerAutoShutdownTime.Value = DateTime.Parse(s);
							}
							checkBoxDisableAutoUpdate.Checked = File.Exists(Path.Combine(Paths.DataPath, "no.update"));
							numericUpDownHistoryImages.Value = _preferences.GetInteger("HistoryImages");
							return;
						}
					case 1:
						{
							textBoxEventPeriod.Text = _preferences.GetString("EventPeriod");
							numericUpDownMinimumLevel.Value = _preferences.GetInteger("MinimumLevel");
							numericUpDownMaximumLevel.Value = _preferences.GetInteger("MaximumLevel");
							checkBoxWizardForNewSequences.Checked = _preferences.GetBoolean("WizardForNewSequences");
							string str3;
							if (((str3 = _preferences.GetString("DefaultProfile")).Length != 0) &&
							    File.Exists(Path.Combine(Paths.ProfilePath, str3 + ".pro")))
							{
								comboBoxDefaultProfile.SelectedIndex = comboBoxDefaultProfile.Items.IndexOf(str3);
							}
							else
							{
								comboBoxDefaultProfile.SelectedIndex = 0;
							}
							int num2 = _preferences.GetInteger("DefaultSequenceAudioDevice") + 1;
							comboBoxDefaultAudioDevice.SelectedIndex = num2 < comboBoxDefaultAudioDevice.Items.Count ? num2 : 0;
							return;
						}
					case 2:
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
						return;

					case 3:
						checkBoxShowPositionMarker.Checked = _preferences.GetBoolean("ShowPositionMarker");
						checkBoxAutoScrolling.Checked = _preferences.GetBoolean("AutoScrolling");
						checkBoxSavePlugInDialogPositions.Checked = _preferences.GetBoolean("SavePlugInDialogPositions");
						checkBoxClearAtEndOfSequence.Checked = _preferences.GetBoolean("ClearAtEndOfSequence");
						checkBoxLogManual.Checked = _preferences.GetBoolean("LogAudioManual");
						checkBoxLogScheduled.Checked = _preferences.GetBoolean("LogAudioScheduled");
						checkBoxLogMusicPlayer.Checked = _preferences.GetBoolean("LogAudioMusicPlayer");
						textBoxLogFilePath.Text = _preferences.GetString("AudioLogFilePath");
						return;

					case 4:
						checkBoxEnableBackgroundSequence.Checked = _preferences.GetBoolean("EnableBackgroundSequence");
						textBoxBackgroundSequenceDelay.Text = _preferences.GetString("BackgroundSequenceDelay");
						checkBoxEnableBackgroundMusic.Checked = _preferences.GetBoolean("EnableBackgroundMusic");
						textBoxBackgroundMusicDelay.Text = _preferences.GetString("BackgroundMusicDelay");
						checkBoxEnableMusicFade.Checked = _preferences.GetBoolean("EnableMusicFade");
						textBoxMusicFadeDuration.Text = _preferences.GetString("MusicFadeDuration");
						return;

					case 5:
						{
							string str4 = _preferences.GetString("SynchronousData");
							if (str4 != "Embedded")
							{
								if (str4 == "Default")
								{
									radioButtonSyncDefaultProfileData.Checked = true;
								}
								else
								{
									radioButtonSyncProfileData.Checked = true;
									comboBoxSyncProfile.SelectedIndex = comboBoxSyncProfile.Items.IndexOf(str4);
								}
							}
							else
							{
								radioButtonSyncEmbeddedData.Checked = true;
							}
							str4 = _preferences.GetString("AsynchronousData");
							if (str4 == "Sync")
							{
								radioButtonAsyncSyncObject.Checked = true;
							}
							else if (str4 == "Default")
							{
								radioButtonAsyncDefaultProfileData.Checked = true;
							}
							else
							{
								radioButtonAsyncProfileData.Checked = true;
								comboBoxAsyncProfile.SelectedIndex = comboBoxAsyncProfile.Items.IndexOf(str4);
							}
							return;
						}
					case 6:
						textBoxEngine.Text = Path.GetFileName(_preferences.GetString("SecondaryEngine"));
						return;
				}
			}
		}

		private void radioButtonAsyncProfileData_CheckedChanged(object sender, EventArgs e)
		{
			comboBoxAsyncProfile.Enabled = radioButtonAsyncProfileData.Checked;
			if (comboBoxAsyncProfile.Enabled && (comboBoxAsyncProfile.SelectedIndex == -1))
			{
				comboBoxAsyncProfile.SelectedIndex = 0;
			}
		}

		private void radioButtonSyncProfileData_CheckedChanged(object sender, EventArgs e)
		{
			comboBoxSyncProfile.Enabled = radioButtonSyncProfileData.Checked;
			if (comboBoxSyncProfile.Enabled && (comboBoxSyncProfile.SelectedIndex == -1))
			{
				comboBoxSyncProfile.SelectedIndex = 0;
			}
		}

		private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e)
		{
			PopulateFrom(e.TabPageIndex);
		}

		private void tabControl_Selected(object sender, TabControlEventArgs e)
		{
			PopulateTo(e.TabPageIndex);
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag != null)
			{
				tabControl.SelectedTab = (TabPage) e.Node.Tag;
			}
		}
	}
}