using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FMOD;
using Vixen.Dialogs;

namespace Vixen
{
	internal partial class PreferencesDialog : Form
	{
		private readonly Preference2 m_preferences;
		private readonly IUIPlugIn[] m_uiPlugins;

		public PreferencesDialog(Preference2 preferences, IUIPlugIn[] uiPlugins)
		{
			InitializeComponent();
			m_preferences = Preference2.GetInstance();
			m_uiPlugins = uiPlugins;
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

		private void buttonCreateProfile_Click(object sender, EventArgs e)
		{
			var dialog = new ProfileManagerDialog(null);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				PopulateProfileLists();
			}
			dialog.Dispose();
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
			if (Directory.Exists(path))
			{
				folderBrowserDialog.SelectedPath = path;
			}
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				textBoxLogFilePath.Text = Path.Combine(folderBrowserDialog.SelectedPath, fileName != string.Empty ? fileName : "audio.log");
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			PopulateFrom(tabControl.SelectedIndex);
			m_preferences.Flush();
		}

		private void buttonPluginSetup_Click(object sender, EventArgs e)
		{
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PreferencesDialog));
		//this.label25.Text = manager.GetString("label25.Text");


		private void PopulateAudioDeviceList()
		{
			comboBoxDefaultAudioDevice.Items.Add("Use application's default device");
			comboBoxDefaultAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
		}

		private void PopulateFrom(int tabIndex)
		{
			if (m_preferences != null)
			{
				switch (tabIndex)
				{
					case 0:
						{
							m_preferences.SetString("TimerCheckFrequency", textBoxTimerCheckFrequency.Text);
							m_preferences.SetString("MouseWheelVerticalDelta", textBoxMouseWheelVertical.Text);
							m_preferences.SetString("MouseWheelHorizontalDelta", textBoxMouseWheelHorizontal.Text);
							m_preferences.SetString("ClientName", textBoxClientName.Text);
							m_preferences.SetBoolean("ResetAtStartup", checkBoxResetAtStartup.Checked);
							m_preferences.SetString("PreferredSequenceType", m_uiPlugins[comboBoxSequenceType.SelectedIndex].FileExtension);
							m_preferences.SetString("ShutdownTime",
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
							m_preferences.SetInteger("HistoryImages", (int) numericUpDownHistoryImages.Value);
							return;
						}
					case 1:
						m_preferences.SetString("EventPeriod", textBoxEventPeriod.Text);
						m_preferences.SetInteger("MinimumLevel", (int) numericUpDownMinimumLevel.Value);
						m_preferences.SetInteger("MaximumLevel", (int) numericUpDownMaximumLevel.Value);
						m_preferences.SetBoolean("WizardForNewSequences", checkBoxWizardForNewSequences.Checked);
						m_preferences.SetString("DefaultProfile",
						                        comboBoxDefaultProfile.SelectedIndex != 0
							                        ? comboBoxDefaultProfile.SelectedItem.ToString()
							                        : string.Empty);
						m_preferences.SetInteger("DefaultSequenceAudioDevice", comboBoxDefaultAudioDevice.SelectedIndex - 1);
						return;

					case 2:
						{
							m_preferences.SetString("MaxColumnWidth", textBoxMaxColumnWidth.Text);
							m_preferences.SetString("MaxRowHeight", textBoxMaxRowHeight.Text);
							int index = textBoxIntensityLargeDelta.Text.Trim().IndexOf('%');
							if (index != -1)
							{
								textBoxIntensityLargeDelta.Text = textBoxIntensityLargeDelta.Text.Substring(0, index).Trim();
							}
							m_preferences.SetString("IntensityLargeDelta", textBoxIntensityLargeDelta.Text);
							m_preferences.SetBoolean("EventSequenceAutoSize", checkBoxEventSequenceAutoSize.Checked);
							m_preferences.SetBoolean("SaveZoomLevels", checkBoxSaveZoomLevels.Checked);
							m_preferences.SetBoolean("ShowSaveConfirmation", checkBoxShowSaveConfirmation.Checked);
							m_preferences.SetBoolean("ShowNaturalChannelNumber", checkBoxShowNaturalChannelNumber.Checked);
							m_preferences.SetBoolean("FlipScrollBehavior", checkBoxFlipMouseScroll.Checked);
							m_preferences.SetString("RemoteLibraryHTTPURL", textBoxCurveLibraryHttpUrl.Text);
							m_preferences.SetString("RemoteLibraryFTPURL", textBoxCurveLibraryFtpUrl.Text);
							m_preferences.SetString("RemoteLibraryFileName", textBoxCurveLibraryFileName.Text);
							m_preferences.SetString("DefaultSequenceDirectory", textBoxDefaultSequenceSaveDirectory.Text);
							return;
						}
					case 3:
						m_preferences.SetBoolean("ShowPositionMarker", checkBoxShowPositionMarker.Checked);
						m_preferences.SetBoolean("AutoScrolling", checkBoxAutoScrolling.Checked);
						m_preferences.SetBoolean("SavePlugInDialogPositions", checkBoxSavePlugInDialogPositions.Checked);
						m_preferences.SetBoolean("ClearAtEndOfSequence", checkBoxClearAtEndOfSequence.Checked);
						m_preferences.SetBoolean("LogAudioManual", checkBoxLogManual.Checked);
						m_preferences.SetBoolean("LogAudioScheduled", checkBoxLogScheduled.Checked);
						m_preferences.SetBoolean("LogAudioMusicPlayer", checkBoxLogMusicPlayer.Checked);
						m_preferences.SetString("AudioLogFilePath", textBoxLogFilePath.Text);
						return;

					case 4:
						m_preferences.SetBoolean("EnableBackgroundSequence", checkBoxEnableBackgroundSequence.Checked);
						m_preferences.SetString("BackgroundSequenceDelay", textBoxBackgroundSequenceDelay.Text);
						m_preferences.SetBoolean("EnableBackgroundMusic", checkBoxEnableBackgroundMusic.Checked);
						m_preferences.SetString("BackgroundMusicDelay", textBoxBackgroundMusicDelay.Text);
						m_preferences.SetBoolean("EnableMusicFade", checkBoxEnableMusicFade.Checked);
						m_preferences.SetString("MusicFadeDuration", textBoxMusicFadeDuration.Text);
						return;

					case 5:
						if (!radioButtonSyncEmbeddedData.Checked)
						{
							if (radioButtonSyncDefaultProfileData.Checked || (comboBoxSyncProfile.SelectedItem == null))
							{
								m_preferences.SetString("SynchronousData", "Default");
							}
							else
							{
								m_preferences.SetString("SynchronousData", comboBoxSyncProfile.SelectedItem.ToString());
							}
						}
						else
						{
							m_preferences.SetString("SynchronousData", "Embedded");
						}
						if (radioButtonAsyncSyncObject.Checked)
						{
							m_preferences.SetString("AsynchronousData", "Sync");
						}
						else if (radioButtonAsyncDefaultProfileData.Checked || (comboBoxAsyncProfile.SelectedItem == null))
						{
							m_preferences.SetString("AsynchronousData", "Default");
						}
						else
						{
							m_preferences.SetString("AsynchronousData", comboBoxAsyncProfile.SelectedItem.ToString());
						}
						return;

					case 6:
						m_preferences.SetString("SecondaryEngine", Path.GetFileName(textBoxEngine.Text));
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
				box.Items.AddRange(list.ToArray());
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
			if (m_preferences != null)
			{
				switch (tabIndex)
				{
					case 0:
						{
							textBoxTimerCheckFrequency.Text = m_preferences.GetString("TimerCheckFrequency");
							textBoxMouseWheelVertical.Text = m_preferences.GetString("MouseWheelVerticalDelta");
							textBoxMouseWheelHorizontal.Text = m_preferences.GetString("MouseWheelHorizontalDelta");
							textBoxClientName.Text = m_preferences.GetString("ClientName");
							checkBoxResetAtStartup.Checked = m_preferences.GetBoolean("ResetAtStartup");
							string str = m_preferences.GetString("PreferredSequenceType");
							for (int i = 0; i < m_uiPlugins.Length; i++)
							{
								if (m_uiPlugins[i].FileExtension == str)
								{
									comboBoxSequenceType.SelectedIndex = i;
									break;
								}
							}
							if (comboBoxSequenceType.SelectedIndex == -1)
							{
								comboBoxSequenceType.SelectedIndex = 0;
							}
							string s = m_preferences.GetString("ShutdownTime");
							if (s != string.Empty)
							{
								dateTimePickerAutoShutdownTime.Checked = true;
								dateTimePickerAutoShutdownTime.Value = DateTime.Parse(s);
							}
							checkBoxDisableAutoUpdate.Checked = File.Exists(Path.Combine(Paths.DataPath, "no.update"));
							numericUpDownHistoryImages.Value = m_preferences.GetInteger("HistoryImages");
							return;
						}
					case 1:
						{
							textBoxEventPeriod.Text = m_preferences.GetString("EventPeriod");
							numericUpDownMinimumLevel.Value = m_preferences.GetInteger("MinimumLevel");
							numericUpDownMaximumLevel.Value = m_preferences.GetInteger("MaximumLevel");
							checkBoxWizardForNewSequences.Checked = m_preferences.GetBoolean("WizardForNewSequences");
							string str3;
							if (((str3 = m_preferences.GetString("DefaultProfile")).Length != 0) &&
							    File.Exists(Path.Combine(Paths.ProfilePath, str3 + ".pro")))
							{
								comboBoxDefaultProfile.SelectedIndex = comboBoxDefaultProfile.Items.IndexOf(str3);
							}
							else
							{
								comboBoxDefaultProfile.SelectedIndex = 0;
							}
							int num2 = m_preferences.GetInteger("DefaultSequenceAudioDevice") + 1;
							comboBoxDefaultAudioDevice.SelectedIndex = num2 < comboBoxDefaultAudioDevice.Items.Count ? num2 : 0;
							return;
						}
					case 2:
						textBoxMaxColumnWidth.Text = m_preferences.GetString("MaxColumnWidth");
						textBoxMaxRowHeight.Text = m_preferences.GetString("MaxRowHeight");
						textBoxIntensityLargeDelta.Text = m_preferences.GetString("IntensityLargeDelta");
						checkBoxEventSequenceAutoSize.Checked = m_preferences.GetBoolean("EventSequenceAutoSize");
						checkBoxSaveZoomLevels.Checked = m_preferences.GetBoolean("SaveZoomLevels");
						checkBoxShowSaveConfirmation.Checked = m_preferences.GetBoolean("ShowSaveConfirmation");
						checkBoxShowNaturalChannelNumber.Checked = m_preferences.GetBoolean("ShowNaturalChannelNumber");
						checkBoxFlipMouseScroll.Checked = m_preferences.GetBoolean("FlipScrollBehavior");
						textBoxCurveLibraryHttpUrl.Text = m_preferences.GetString("RemoteLibraryHTTPURL");
						textBoxCurveLibraryFtpUrl.Text = m_preferences.GetString("RemoteLibraryFTPURL");
						textBoxCurveLibraryFileName.Text = m_preferences.GetString("RemoteLibraryFileName");
						textBoxDefaultSequenceSaveDirectory.Text = m_preferences.GetString("DefaultSequenceDirectory");
						return;

					case 3:
						checkBoxShowPositionMarker.Checked = m_preferences.GetBoolean("ShowPositionMarker");
						checkBoxAutoScrolling.Checked = m_preferences.GetBoolean("AutoScrolling");
						checkBoxSavePlugInDialogPositions.Checked = m_preferences.GetBoolean("SavePlugInDialogPositions");
						checkBoxClearAtEndOfSequence.Checked = m_preferences.GetBoolean("ClearAtEndOfSequence");
						checkBoxLogManual.Checked = m_preferences.GetBoolean("LogAudioManual");
						checkBoxLogScheduled.Checked = m_preferences.GetBoolean("LogAudioScheduled");
						checkBoxLogMusicPlayer.Checked = m_preferences.GetBoolean("LogAudioMusicPlayer");
						textBoxLogFilePath.Text = m_preferences.GetString("AudioLogFilePath");
						return;

					case 4:
						checkBoxEnableBackgroundSequence.Checked = m_preferences.GetBoolean("EnableBackgroundSequence");
						textBoxBackgroundSequenceDelay.Text = m_preferences.GetString("BackgroundSequenceDelay");
						checkBoxEnableBackgroundMusic.Checked = m_preferences.GetBoolean("EnableBackgroundMusic");
						textBoxBackgroundMusicDelay.Text = m_preferences.GetString("BackgroundMusicDelay");
						checkBoxEnableMusicFade.Checked = m_preferences.GetBoolean("EnableMusicFade");
						textBoxMusicFadeDuration.Text = m_preferences.GetString("MusicFadeDuration");
						return;

					case 5:
						{
							string str4 = m_preferences.GetString("SynchronousData");
							if (!(str4 == "Embedded"))
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
							str4 = m_preferences.GetString("AsynchronousData");
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
						textBoxEngine.Text = Path.GetFileName(m_preferences.GetString("SecondaryEngine"));
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