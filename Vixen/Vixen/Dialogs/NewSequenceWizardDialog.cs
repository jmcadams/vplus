namespace Vixen.Dialogs {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using Vixen;

	public partial class NewSequenceWizardDialog : Form {
		private bool m_back = false;
		private string[,] m_explanations = new string[,] { { string.Empty, string.Empty, string.Empty, string.Empty }, { "The event period length is how long a single on/off event lasts.", "A sequence is made up of a series of events of all the same length.  At every event, the program updates the controller with new data if a change needs to be made.", "The smaller the event period, the finer the control you over the timing of the events.  Generally, you don't want to have the event period be any shorter than you need it to be in order to synchronize to your selected audio.", "100 milliseconds, or 10 updates/second, is adequate for synchronizing to most audio." }, { "A profile contains information about channels and plugins.", "To reduce the otherwise repetitive task of setting up channels and plugins for every sequence you create.", "By selecting a profile, the channel count, channel names, channel colors, channel outputs, and plugin setup will all be done for you.", "Profiles are not required, but can really help you quickly create new sequences.  Profiles can be created by hand or from existing sequences." }, { "A channel is an independently-controlled circuit of lights.", "For every area of your display that you want to control independently, you will want to create a channel.  Sometimes you may need multiple channels assigned to a specific area or structure to adequately meet power limitations.", "For every channel you define in your sequence there will be a row defined in the event grid.", "The channel count can be changed at any time.  If you try to reduce your channel count, you will be warned about the potential loss of data.  Channel names can be easily imported from a file by going to Sequence/Import, which also affects the channel count." }, { "Names for the channels defined earlier.", "For easier identification of a channel's purpose and location.", string.Empty, string.Empty }, { "The output plugins are what Vixen uses to communicate with the controllers.  They translate the data sent from Vixen into a format that the controllers can understand.", "Without specifying at least one plugin, Vixen cannot communicate with any controller.", string.Empty, "If you select plugins to use with this sequence, they need to be setup before they can be used.  The plugins and their respective setup can be changed at any time.  While Vixen supports using multiple plugins simultaneously, setting up one is adequate in most installations." }, { "Here you can assign audio and create event patterns based on the music (or without music).  As the music plays, you tap a pattern on your keyboard on a channel-by-channel basis.", "The sequence can be authored to be synchronized with audio.  The music would be played by Vixen at the same time the sequence is being executed.\nEvent patterns can help set up the initial event timings which can later be cleaned up with manual editing.  It's much easier to establish the initial synchronized timings this way than by hand.", "The event grid will be populated with the created events.  The events are on/off values only.", "The audio can be added or removed at any time during editing.  The length of the sequence can be auto-sized to exactly fit the music, or it can be longer.  The sequence length cannot be shorter than any associated music.\nDefining event patterns can also be used to mark events of significance in the music.  The event patterns can be recreated at any time by going to Sequences/Music." }, { "The length of the sequence in minutes and seconds.", string.Empty, string.Empty, "If there is audio assigned to a sequence, the sequence length cannot be shorter than the music length." } };
		private Stack<int> m_history;
		private Preference2 m_preferences;
		private EventSequence m_sequence;
		private bool m_skip = false;

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(NewSequenceWizardDialog));
		//this.label2.Text = manager.GetString("label2.Text");
		//this.label13.Text = manager.GetString("label13.Text");

		public NewSequenceWizardDialog(Preference2 preferences) {
			this.InitializeComponent();
			this.openFileDialog.InitialDirectory = Paths.SequencePath;
			this.tabControl.SelectedIndex = 0;
			this.m_preferences = preferences;
			this.m_sequence = new EventSequence(preferences);
			this.PopulateProfileList();
			string str = string.Empty;
			if ((str = preferences.GetString("DefaultProfile")).Length > 0) {
				this.comboBoxProfiles.SelectedIndex = this.comboBoxProfiles.Items.IndexOf(str);
			}
			this.m_history = new Stack<int>();
			this.UpdateExplanations(0);
		}

		private void buttonAssignAudio_Click(object sender, EventArgs e) {
			int integer = this.m_preferences.GetInteger("SoundDevice");
			AudioDialog dialog = new AudioDialog(this.m_sequence, this.m_preferences.GetBoolean("EventSequenceAutoSize"), integer);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.SetSequenceTime();
			}
			dialog.Dispose();
		}

		private void buttonImportChannels_Click(object sender, EventArgs e) {
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				EventSequence sequence = new EventSequence(this.openFileDialog.FileName);
				this.textBoxChannelCount.Text = sequence.ChannelCount.ToString();
				StringBuilder builder = new StringBuilder();
				foreach (Channel channel in sequence.Channels) {
					builder.AppendLine(channel.Name);
				}
				this.textBoxChannelNames.Text = builder.ToString();
				this.m_sequence.Channels.Clear();
				this.m_sequence.Channels.AddRange(sequence.Channels);
			}
		}

		private void buttonNext_Click(object sender, EventArgs e) {
			this.m_skip = false;
			this.m_back = false;
			if (this.tabControl.SelectedTab == this.tabPageProfile) {
				if (this.comboBoxProfiles.SelectedIndex == 0) {
					this.tabControl.SelectedTab = this.tabPageChannelCount;
				}
				else {
					this.tabControl.SelectedTab = this.tabPageAudio;
				}
			}
			else {
				this.tabControl.SelectedIndex++;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.tabControl_Deselecting(null, new TabControlCancelEventArgs(this.tabControl.SelectedTab, this.tabControl.SelectedIndex, false, TabControlAction.Deselecting));
		}

		private void buttonPrev_Click(object sender, EventArgs e) {
			this.m_back = true;
			this.tabControl.SelectedIndex = this.m_history.Pop();
			this.buttonPrev.Enabled = this.m_history.Count > 0;
		}

		private void buttonProfileManager_Click(object sender, EventArgs e) {
			ProfileManagerDialog dialog = new ProfileManagerDialog(null);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.PopulateProfileList();
			}
			dialog.Dispose();
		}

		private void buttonSetupPlugins_Click(object sender, EventArgs e) {
			PluginListDialog dialog = new PluginListDialog(this.m_sequence);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonSkip_Click(object sender, EventArgs e) {
			this.m_skip = true;
			this.m_back = false;
			this.tabControl.SelectedIndex++;
		}





		private void labelEffect_Click(object sender, EventArgs e) {
			if (this.labelEffect.Enabled) {
				this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 2];
			}
		}

		private void labelNotes_Click(object sender, EventArgs e) {
			if (this.labelNotes.Enabled) {
				this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 3];
			}
		}

		private void labelWhat_Click(object sender, EventArgs e) {
			if (this.labelWhat.Enabled) {
				this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 0];
			}
		}

		private void labelWhy_Click(object sender, EventArgs e) {
			if (this.labelWhy.Enabled) {
				this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 1];
			}
		}

		private int ParseTimeString(string text) {
			int num2;
			int num3;
			int num4;
			string s = "0";
			string str2 = string.Empty;
			string str3 = "0";
			int index = text.IndexOf(':');
			if (index != -1) {
				s = text.Substring(0, index).Trim();
				text = text.Substring(index + 1);
			}
			index = text.IndexOf('.');
			if (index != -1) {
				str3 = text.Substring(index + 1).Trim();
				text = text.Substring(0, index);
			}
			str2 = text;
			try {
				num2 = int.Parse(s);
			}
			catch {
				num2 = 0;
			}
			try {
				num3 = int.Parse(str2);
			}
			catch {
				num3 = 0;
			}
			try {
				num4 = int.Parse(str3);
			}
			catch {
				num4 = 0;
			}
			num4 = (num4 + (num3 * 0x3e8)) + (num2 * 0xea60);
			if (num4 == 0) {
				MessageBox.Show("Not a valid format for time.\nUse one of the following:\n\nSeconds\nMinutes:Seconds\nSeconds.Milliseconds\nMinutes:Seconds.Milliseconds", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return 0;
			}
			return num4;
		}

		private void PopulateProfileList() {
			int selectedIndex = this.comboBoxProfiles.SelectedIndex;
			this.comboBoxProfiles.BeginUpdate();
			this.comboBoxProfiles.Items.Clear();
			this.comboBoxProfiles.Items.Add("None");
			foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro")) {
				this.comboBoxProfiles.Items.Add(Path.GetFileNameWithoutExtension(str));
			}
			if (selectedIndex < this.comboBoxProfiles.Items.Count) {
				this.comboBoxProfiles.SelectedIndex = selectedIndex;
			}
			this.comboBoxProfiles.EndUpdate();
		}

		private void SetSequenceTime() {
			this.textBoxTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", this.m_sequence.Time / 0xea60, (this.m_sequence.Time % 0xea60) / 0x3e8, this.m_sequence.Time % 0x3e8);
		}

		private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
			if (!this.m_back) {
				this.m_history.Push(e.TabPageIndex);
				this.buttonPrev.Enabled = true;
			}
			if (this.m_skip) {
				return;
			}
			string text = string.Empty;
			switch (e.TabPageIndex) {
				case 1: {
						int num = 0;
						try {
							num = Convert.ToInt32(this.textBoxEventPeriod.Text);
							if (num < 0x19) {
								text = "While possible, event periods less than 25 ms aren't realistic or practical.";
								goto Label_0339;
							}
						}
						catch {
							text = this.textBoxChannelCount.Text + " is not a valid number for the event period length";
							goto Label_0339;
						}
						this.m_sequence.EventPeriod = num;
						goto Label_0339;
					}
				case 2:
					if (this.comboBoxProfiles.SelectedIndex == 0) {
						this.m_sequence.Profile = null;
					}
					else {
						this.m_sequence.Profile = new Profile(Path.Combine(Paths.ProfilePath, this.comboBoxProfiles.SelectedItem.ToString() + ".pro"));
					}
					goto Label_0339;

				case 3: {
						int num2 = 0;
						try {
							num2 = Convert.ToInt32(this.textBoxChannelCount.Text);
							if (num2 < 1) {
								text = "The channel count must be 1 or more";
								goto Label_0339;
							}
						}
						catch {
							text = this.textBoxChannelCount.Text + " is not a valid number for the channel count";
							goto Label_0339;
						}
						if ((num2 > 0x400) && (MessageBox.Show(string.Format("Are you sure you really want {0} channels?", num2), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)) {
							this.tabControl.TabIndex = 1;
						}
						else {
							this.Cursor = Cursors.WaitCursor;
							try {
								this.m_sequence.ChannelCount = num2;
							}
							finally {
								this.Cursor = Cursors.Default;
							}
						}
						goto Label_0339;
					}
				case 4:
					if (this.textBoxChannelNames.Lines.Length == Convert.ToInt32(this.textBoxChannelCount.Text)) {
						foreach (string str2 in this.textBoxChannelNames.Lines) {
							if (str2.Trim() == string.Empty) {
								text = "Channel names cannot be blank";
								break;
							}
						}
						break;
					}
					text = "There must be an equal number of channel names as there are channels";
					goto Label_0339;

				case 7: {
						int num4 = this.ParseTimeString(this.textBoxTime.Text);
						if (num4 != 0) {
							try {
								this.m_sequence.Time = num4;
							}
							catch (Exception exception) {
								text = exception.Message;
							}
						}
						goto Label_0339;
					}
				default:
					goto Label_0339;
			}
			this.Cursor = Cursors.WaitCursor;
			try {
				for (int i = 0; i < this.m_sequence.ChannelCount; i++) {
					this.m_sequence.Channels[i].Name = this.textBoxChannelNames.Lines[i];
				}
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		Label_0339:
			if (text != string.Empty) {
				e.Cancel = true;
				MessageBox.Show(text, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void tabControl_Selected(object sender, TabControlEventArgs e) {
			switch (e.TabPageIndex) {
				case 1:
					this.textBoxEventPeriod.Text = this.m_sequence.EventPeriod.ToString();
					break;

				case 2:
					if (this.m_sequence.Profile != null) {
						this.comboBoxProfiles.SelectedIndex = this.comboBoxProfiles.Items.IndexOf(this.m_sequence.Profile.Name);
						break;
					}
					this.comboBoxProfiles.SelectedIndex = 0;
					break;

				case 4:
					this.Cursor = Cursors.WaitCursor;
					try {
						string[] strArray;
						int num = Convert.ToInt32(this.textBoxChannelCount.Text);
						int length = this.textBoxChannelNames.Lines.Length;
						if (length < num) {
							strArray = new string[num];
							this.textBoxChannelNames.Lines.CopyTo(strArray, 0);
							while (length < num) {
								strArray[length] = string.Format("Channel {0}", length + 1);
								length++;
							}
							this.textBoxChannelNames.Lines = strArray;
						}
						else if (length > num) {
							strArray = new string[num];
							for (int i = 0; i < num; i++) {
								strArray[i] = this.textBoxChannelNames.Lines[i];
							}
							this.textBoxChannelNames.Lines = strArray;
						}
					}
					finally {
						this.Cursor = Cursors.Default;
					}
					break;

				case 7:
					this.SetSequenceTime();
					break;
			}
			this.UpdateExplanations(e.TabPageIndex);
			this.buttonSkip.Enabled = this.buttonNext.Enabled = e.TabPageIndex < (this.tabControl.TabCount - 1);
			base.AcceptButton = (e.TabPageIndex == 4) ? null : (this.buttonNext.Enabled ? ((IButtonControl)this.buttonNext) : ((IButtonControl)this.buttonOK));
		}

		private void textBoxChannelNames_TextChanged(object sender, EventArgs e) {
			this.labelNamesChannels.Text = string.Format("{0} names / {1} channels", this.textBoxChannelNames.Lines.Length, this.textBoxChannelCount.Text);
		}

		private void UpdateExplanations(int pageIndex) {
			this.labelWhat.Enabled = this.m_explanations[pageIndex, 0] != string.Empty;
			this.labelWhy.Enabled = this.m_explanations[pageIndex, 1] != string.Empty;
			this.labelEffect.Enabled = this.m_explanations[pageIndex, 2] != string.Empty;
			this.labelNotes.Enabled = this.m_explanations[pageIndex, 3] != string.Empty;
			this.labelExplanation.Text = this.m_explanations[pageIndex, 0];
		}

		public EventSequence Sequence {
			get {
				return this.m_sequence;
			}
		}
	}
}

