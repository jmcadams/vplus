namespace Vixen.Dialogs {
	using FMOD;
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Timers;
	using System.Windows.Forms;
	using Vixen;

	public partial class AudioDialog : Form {
		private const int COUNTDOWN_SECONDS = 5;
		private string m_audioFilename = string.Empty;
		private DateTime m_countdownEnd;
		private fmod m_fmod;
		private int[] m_keyMap = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private bool[] m_keyStates;
		private int m_lastIndex = -1;
		private int m_maxTime;
		private byte[,] m_newEventValues;
		private Audio m_originalAudio;
		private bool m_paused = false;
		private bool m_playing = false;
		private EventSequence m_sequence = null;
		private float m_smallChange;
		private SoundChannel m_soundChannel = null;
		private Stopwatch m_stopwatch;
		private int m_timeOffset;
		private System.Timers.Timer m_timer;

		public AudioDialog(EventSequence sequence, bool autoSize, int deviceIndex) {
			this.InitializeComponent();
			this.m_fmod = (deviceIndex > 0) ? fmod.GetInstance(deviceIndex) : fmod.GetInstance(-1);
			this.m_timer = new System.Timers.Timer(10.0);
			this.m_timer.Elapsed += new ElapsedEventHandler(this.m_timer_Elapsed);
			this.m_sequence = sequence;
			this.m_keyStates = new bool[this.m_sequence.ChannelCount];
			this.m_stopwatch = new Stopwatch();
			this.m_newEventValues = new byte[this.m_sequence.EventValues.GetLength(0), this.m_sequence.EventValues.GetLength(1)];
			this.listBoxChannels.Items.AddRange(this.m_sequence.Channels.ToArray());
			this.m_originalAudio = sequence.Audio;
			if (sequence.Audio != null) {
				if (this.LoadAudio(this.m_sequence.Audio.FileName) == null) {
					sequence.Audio = null;
					this.buttonRemoveAudio.Enabled = false;
					this.ClearAudio();
				}
				else {
					this.buttonRemoveAudio.Enabled = true;
				}
			}
			else {
				this.buttonRemoveAudio.Enabled = false;
				this.ClearAudio();
			}
			this.checkBoxAutoSize.Checked = autoSize;
			if (!autoSize) {
				this.UpdateRecordableLength();
			}
			Vixen.Channel[] items = this.m_sequence.Channels.ToArray();
			this.channel1ToolStripMenuItem.Items.AddRange(items);
			this.channel2ToolStripMenuItem.Items.AddRange(items);
			this.channel3ToolStripMenuItem.Items.AddRange(items);
			this.channel4ToolStripMenuItem.Items.AddRange(items);
			this.channel5ToolStripMenuItem.Items.AddRange(items);
			this.channel6ToolStripMenuItem.Items.AddRange(items);
			this.channel7ToolStripMenuItem.Items.AddRange(items);
			this.channel8ToolStripMenuItem.Items.AddRange(items);
			this.channel9ToolStripMenuItem.Items.AddRange(items);
			this.channel0ToolStripMenuItem.Items.AddRange(items);
			this.channel1ToolStripMenuItem.SelectedIndex = Math.Min(0, this.m_sequence.ChannelCount - 1);
			this.channel2ToolStripMenuItem.SelectedIndex = Math.Min(1, this.m_sequence.ChannelCount - 1);
			this.channel3ToolStripMenuItem.SelectedIndex = Math.Min(2, this.m_sequence.ChannelCount - 1);
			this.channel4ToolStripMenuItem.SelectedIndex = Math.Min(3, this.m_sequence.ChannelCount - 1);
			this.channel5ToolStripMenuItem.SelectedIndex = Math.Min(4, this.m_sequence.ChannelCount - 1);
			this.channel6ToolStripMenuItem.SelectedIndex = Math.Min(5, this.m_sequence.ChannelCount - 1);
			this.channel7ToolStripMenuItem.SelectedIndex = Math.Min(6, this.m_sequence.ChannelCount - 1);
			this.channel8ToolStripMenuItem.SelectedIndex = Math.Min(7, this.m_sequence.ChannelCount - 1);
			this.channel9ToolStripMenuItem.SelectedIndex = Math.Min(8, this.m_sequence.ChannelCount - 1);
			this.channel0ToolStripMenuItem.SelectedIndex = Math.Min(9, this.m_sequence.ChannelCount - 1);
			this.comboBoxAudioDevice.Items.Add("Use application's default device");
			this.comboBoxAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
			this.comboBoxAudioDevice.SelectedIndex = this.m_sequence.AudioDeviceIndex + 1;
		}

		private void AudioDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.Stop();
			this.m_fmod.ReleaseSound(this.m_soundChannel);
			this.m_fmod.Shutdown();
		}

		private void AudioDialog_KeyDown(object sender, KeyEventArgs e) {
			this.UpdateKeyState(e, true);
		}

		private void AudioDialog_KeyUp(object sender, KeyEventArgs e) {
			this.UpdateKeyState(e, false);
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			this.m_sequence.Audio = this.m_originalAudio;
		}

		private void buttonClear_Click(object sender, EventArgs e) {
			for (int i = 0; i < this.m_newEventValues.GetLength(1); i++) {
				this.m_newEventValues[this.listBoxChannels.SelectedIndex, i] = 0;
			}
			MessageBox.Show(string.Format("Channel \"{0}\" cleared of new events.", ((Vixen.Channel)this.listBoxChannels.SelectedItem).Name), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void buttonLoad_Click(object sender, EventArgs e) {
			this.openFileDialog1.InitialDirectory = Paths.AudioPath;
			this.openFileDialog1.DefaultExt = "wma";
			this.openFileDialog1.Filter = "All supported formats | *.aiff;*.asf;*.flac;*.mp2;*.mp3;*.ogg;*.wav;*.wma;*.mid";
			this.openFileDialog1.FileName = string.Empty;
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK) {
				string path = Path.Combine(Paths.AudioPath, Path.GetFileName(this.openFileDialog1.FileName));
				if (!File.Exists(path)) {
					this.Cursor = Cursors.WaitCursor;
					File.Copy(this.openFileDialog1.FileName, path);
					this.Cursor = Cursors.Default;
				}
				this.m_sequence.Audio = this.LoadAudio(this.openFileDialog1.FileName);
				this.UpdateRecordableLength();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if ((this.m_sequence.Audio != null) && (this.checkBoxAutoSize.Checked || (this.m_sequence.Audio.Duration > this.m_sequence.Time))) {
				this.m_sequence.Time = this.m_sequence.Audio.Duration;
			}
			this.m_sequence.AudioDeviceIndex = this.comboBoxAudioDevice.SelectedIndex - 1;
			int num2 = Math.Min(this.m_newEventValues.GetLength(0), this.m_sequence.EventValues.GetLength(0));
			int num3 = Math.Min(this.m_newEventValues.GetLength(1), this.m_sequence.EventValues.GetLength(1));
			for (int i = 0; i < num2; i++) {
				for (int j = 0; j < num3; j++) {
					byte num1 = this.m_sequence.EventValues[i, j];
					num1 = (byte)(num1 | this.m_newEventValues[i, j]);
				}
			}
		}

		private void buttonPlayPause_Click(object sender, EventArgs e) {
			if (this.m_playing) {
				this.buttonPlayPause.Image = this.m_paused ? this.pictureBoxPause.Image : this.pictureBoxPlayBlue.Image;
				if (this.m_soundChannel != null) {
					this.m_soundChannel.Paused = !this.m_paused;
				}
				this.m_timer.Enabled = this.m_paused;
				this.m_paused = !this.m_paused;
				if (!(this.m_paused || !this.progressBarCountdown.Visible)) {
					this.m_countdownEnd = DateTime.Now + TimeSpan.FromSeconds((double)((((float)this.progressBarCountdown.Value) / 100f) * 5f));
				}
			}
			else {
				this.m_countdownEnd = DateTime.Now.Add(TimeSpan.FromSeconds(5.0));
				this.progressBarCountdown.Value = 100;
				this.progressBarCountdown.Visible = true;
				this.m_playing = true;
				this.labelTime.Text = "Countdown...";
				this.labelTotalTime.Text = string.Empty;
				this.trackBarPosition.Enabled = false;
				this.m_timer.Start();
				this.buttonPlayPause.Image = this.pictureBoxPause.Image;
			}
		}

		private void buttonRemoveAudio_Click(object sender, EventArgs e) {
			this.ClearAudio();
			this.UpdateRecordableLength();
		}

		private void buttonStop_Click(object sender, EventArgs e) {
			this.Stop();
		}

		private void channelMapItem_SelectedIndexChanged(object sender, EventArgs e) {
			ToolStripComboBox box = sender as ToolStripComboBox;
			if (box.SelectedIndex != -1) {
				int index = Convert.ToInt32(box.Tag) - 1;
				this.m_keyMap[index] = box.SelectedIndex;
			}
		}

		private void checkBoxAutoSize_CheckedChanged(object sender, EventArgs e) {
			this.UpdateRecordableLength();
		}

		private void ClearAudio() {
			this.m_sequence.Audio = null;
			this.labelAudioFileName.Text = "This event sequence does not have audio assigned";
			this.labelAudioLength.Text = string.Empty;
			this.labelAudioName.Text = string.Empty;
			this.m_soundChannel = null;
		}

		private void CopyArray(byte[,] source, byte[,] dest) {
			int num = Math.Min(source.GetLength(0), dest.GetLength(0));
			int num2 = Math.Min(source.GetLength(1), dest.GetLength(1));
			for (int i = 0; i < num; i++) {
				for (int j = 0; j < num2; j++) {
					dest[i, j] = source[i, j];
				}
			}
		}



		private uint GetPosition() {
			if ((this.m_soundChannel != null) && this.m_soundChannel.IsPlaying) {
				return this.m_soundChannel.Position;
			}
			return (uint)(this.m_stopwatch.ElapsedMilliseconds + this.m_timeOffset);
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(AudioDialog));
		//this.pictureBoxPlay.Image = (Image)manager.GetObject("pictureBoxPlay.Image");
		//this.pictureBoxPlayBlue.Image = (Image)manager.GetObject("pictureBoxPlayBlue.Image");
		//this.pictureBoxPause.Image = (Image)manager.GetObject("pictureBoxPause.Image");
		//this.buttonStop.Image = (Image)manager.GetObject("buttonStop.Image");
		//this.buttonPlayPause.Image = (Image) manager.GetObject("buttonPlayPause.Image");




		private void linkLabelAssignedKeys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			this.contextMenuStrip.Show(this.linkLabelAssignedKeys.PointToScreen(new Point(0, this.linkLabelAssignedKeys.Height)));
		}

		private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
			this.UpdateAudioButtons();
		}

		private Audio LoadAudio(string fileName) {
			if (fileName == string.Empty) {
				return null;
			}
			try {
				this.m_soundChannel = this.m_fmod.LoadSound(Path.Combine(Paths.AudioPath, fileName), this.m_soundChannel);
			}
			catch (Exception exception) {
				MessageBox.Show("Error loading audio:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
			}
			if (this.m_soundChannel == null) {
				return null;
			}
			this.m_audioFilename = fileName;
			this.labelAudioFileName.Text = Path.GetFileName(this.m_audioFilename);
			Audio audio = new Audio();
			audio.FileName = this.labelAudioFileName.Text;
			audio.Name = this.m_soundChannel.SoundName;
			audio.Duration = (int)this.m_soundChannel.SoundLength;
			this.labelAudioLength.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", audio.Duration / 0xea60, (audio.Duration % 0xea60) / 0x3e8, audio.Duration % 0x3e8);
			this.labelAudioName.Text = string.Format("\"{0}\"", audio.Name);
			this.UpdateAudioButtons();
			return audio;
		}

		private void m_timer_Elapsed(object sender, ElapsedEventArgs e) {
			MethodInvoker method = null;
			if (this.progressBarCountdown.Visible) {
				if (this.progressBarCountdown.Value <= 0) {
					this.UpdateTotalTime();
					int num = (int)(((int)base.Invoke(new TrackBarValueDelegate(this.TrackBarValue))) * this.m_smallChange);
					this.m_timeOffset = num;
					if ((this.m_soundChannel != null) && (num < this.m_soundChannel.SoundLength)) {
						this.m_fmod.Play(this.m_soundChannel, true);
						this.m_soundChannel.Position = (uint)num;
						this.m_soundChannel.Paused = false;
					}
					this.m_stopwatch.Reset();
					this.m_stopwatch.Start();
					base.Invoke(new ProgressBarVisibleDelegate(this.ProgressBarVisible), new object[] { false });
				}
				else {
					if (method == null) {
						method = delegate {
							TimeSpan span = (TimeSpan)(this.m_countdownEnd - DateTime.Now);
							this.progressBarCountdown.Value = (int)((span.TotalMilliseconds * 100.0) / 5000.0);
						};
					}
					base.BeginInvoke(method);
				}
			}
			else {
				MethodInvoker invoker = null;
				uint position = this.GetPosition();
				if (position >= this.m_maxTime) {
					this.Stop();
				}
				else {
					int num2 = (int)(((long)position) / ((long)this.m_sequence.EventPeriod));
					if (num2 != this.m_lastIndex) {
						this.m_lastIndex = num2;
						for (int i = 0; i < this.m_sequence.ChannelCount; i++) {
							if (this.m_keyStates[i]) {
								this.m_newEventValues[i, num2] = this.m_sequence.MaximumLevel;
								if (this.radioButtonSingleEvent.Checked) {
									this.m_keyStates[i] = false;
								}
							}
						}
					}
					if (invoker == null) {
						invoker = delegate {
							this.labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", position / 0xea60, (position % 0xea60) / 0x3e8, position % 0x3e8);
							this.trackBarPosition.Value = (int)(((float)position) / this.m_smallChange);
						};
					}
					base.BeginInvoke(invoker);
				}
			}
		}

		private void ProgressBarVisible(bool value) {
			this.progressBarCountdown.Visible = value;
		}

		private void Stop() {
			if (base.InvokeRequired) {
				base.BeginInvoke(new MethodInvoker(this.Stop));
			}
			else {
				this.m_timer.Stop();
				Thread.Sleep((int)this.m_timer.Interval);
				if (this.m_soundChannel != null) {
					this.m_fmod.Stop(this.m_soundChannel);
				}
				this.m_stopwatch.Stop();
				this.buttonPlayPause.Image = this.pictureBoxPlay.Image;
				this.labelTime.Text = "00:00.000";
				this.m_playing = this.m_paused = false;
				this.trackBarPosition.Enabled = true;
				this.trackBarPosition.Value = 0;
				this.progressBarCountdown.Visible = false;
			}
		}

		private void trackBarPosition_Scroll(object sender, EventArgs e) {
			int num = (int)(this.trackBarPosition.Value * this.m_smallChange);
			this.labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
		}

		private int TrackBarValue() {
			return this.trackBarPosition.Value;
		}

		private void UpdateAudioButtons() {
			Vixen.Channel selectedItem = null;
			if (this.listBoxChannels.SelectedItem != null) {
				selectedItem = (Vixen.Channel)this.listBoxChannels.SelectedItem;
			}
			if (selectedItem != null) {
				this.buttonClear.Enabled = selectedItem.Enabled;
			}
			else {
				this.buttonClear.Enabled = false;
			}
		}

		private void UpdateKeyState(KeyEventArgs e, bool state) {
			if (!this.progressBarCountdown.Visible) {
				if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) {
					if (e.KeyCode == Keys.D0) {
						this.m_keyStates[this.m_keyMap[9]] = state;
					}
					else {
						this.m_keyStates[this.m_keyMap[((int)e.KeyCode) - 0x31]] = state;
					}
				}
				else if ((e.KeyCode == Keys.ControlKey) && (this.listBoxChannels.SelectedItem != null)) {
					this.m_keyStates[this.listBoxChannels.SelectedIndex] = state;
				}
			}
		}

		private void UpdateRecordableLength() {
			int num = this.UpdateTotalTime();
			this.m_maxTime = num;
			this.trackBarPosition.Maximum = num;
			this.UpdateTrackbar();
			byte[,] dest = new byte[this.m_newEventValues.GetLength(0), (int)Math.Ceiling((double)(((float)num) / ((float)this.m_sequence.EventPeriod)))];
			this.CopyArray(this.m_newEventValues, dest);
			this.m_newEventValues = dest;
		}

		private int UpdateTotalTime() {
			if (base.InvokeRequired) {
				int milliseconds = 0;
				base.Invoke((MethodInvoker)delegate { // TODO: Can change this to "base.Invoke(() => {" when we go to 3.x
					milliseconds = this.UpdateTotalTime();
				});
				return milliseconds;
			}
			int num = (this.m_sequence.Audio != null) ? (this.checkBoxAutoSize.Checked ? this.m_sequence.Audio.Duration : Math.Max(this.m_sequence.Time, this.m_sequence.Audio.Duration)) : this.m_sequence.Time;
			this.labelTotalTime.Text = string.Format("/ {0:d2}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
			return num;
		}

		private void UpdateTrackbar() {
			if (this.trackBarPosition.Maximum < 0x7d0) {
				this.m_smallChange = 100f;
			}
			else if (this.trackBarPosition.Maximum < 0x4e20) {
				this.m_smallChange = 1000f;
			}
			else if (this.trackBarPosition.Maximum < 0xea60) {
				this.m_smallChange = 2000f;
			}
			else {
				this.m_smallChange = 5000f;
			}
			this.trackBarPosition.Maximum = (int)Math.Round((double)(((float)this.trackBarPosition.Maximum) / this.m_smallChange), MidpointRounding.AwayFromZero);
		}

		private delegate void ProgressBarVisibleDelegate(bool value);

		private delegate int TrackBarValueDelegate();
	}
}

