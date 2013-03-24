using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using FMOD;

namespace Vixen.Dialogs
{
	public partial class AudioDialog : Form
	{
		private const int COUNTDOWN_SECONDS = 5;
		private readonly fmod m_fmod;
		private readonly int[] m_keyMap = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
		private readonly bool[] m_keyStates;
		private readonly Audio m_originalAudio;
		private readonly EventSequence m_sequence;
		private readonly Stopwatch m_stopwatch;
		private readonly System.Timers.Timer m_timer;
		private string m_audioFilename = string.Empty;
		private DateTime m_countdownEnd;
		private int m_lastIndex = -1;
		private int m_maxTime;
		private byte[,] m_newEventValues;
		private bool m_paused;
		private bool m_playing;
		private float m_smallChange;
		private SoundChannel m_soundChannel;
		private int m_timeOffset;

		public AudioDialog(EventSequence sequence, bool autoSize, int deviceIndex)
		{
			InitializeComponent();
			m_fmod = (deviceIndex > 0) ? fmod.GetInstance(deviceIndex) : fmod.GetInstance(-1);
			m_timer = new System.Timers.Timer(10.0);
			m_timer.Elapsed += m_timer_Elapsed;
			m_sequence = sequence;
			m_keyStates = new bool[m_sequence.ChannelCount];
			m_stopwatch = new Stopwatch();
			m_newEventValues = new byte[m_sequence.EventValues.GetLength(0),m_sequence.EventValues.GetLength(1)];
			listBoxChannels.Items.AddRange(m_sequence.Channels.ToArray());
			m_originalAudio = sequence.Audio;
			if (sequence.Audio != null)
			{
				if (LoadAudio(m_sequence.Audio.FileName) == null)
				{
					sequence.Audio = null;
					buttonRemoveAudio.Enabled = false;
					ClearAudio();
				}
				else
				{
					buttonRemoveAudio.Enabled = true;
				}
			}
			else
			{
				buttonRemoveAudio.Enabled = false;
				ClearAudio();
			}
			checkBoxAutoSize.Checked = autoSize;
			if (!autoSize)
			{
				UpdateRecordableLength();
			}
			Channel[] items = m_sequence.Channels.ToArray();
			channel1ToolStripMenuItem.Items.AddRange(items);
			channel2ToolStripMenuItem.Items.AddRange(items);
			channel3ToolStripMenuItem.Items.AddRange(items);
			channel4ToolStripMenuItem.Items.AddRange(items);
			channel5ToolStripMenuItem.Items.AddRange(items);
			channel6ToolStripMenuItem.Items.AddRange(items);
			channel7ToolStripMenuItem.Items.AddRange(items);
			channel8ToolStripMenuItem.Items.AddRange(items);
			channel9ToolStripMenuItem.Items.AddRange(items);
			channel0ToolStripMenuItem.Items.AddRange(items);
			channel1ToolStripMenuItem.SelectedIndex = Math.Min(0, m_sequence.ChannelCount - 1);
			channel2ToolStripMenuItem.SelectedIndex = Math.Min(1, m_sequence.ChannelCount - 1);
			channel3ToolStripMenuItem.SelectedIndex = Math.Min(2, m_sequence.ChannelCount - 1);
			channel4ToolStripMenuItem.SelectedIndex = Math.Min(3, m_sequence.ChannelCount - 1);
			channel5ToolStripMenuItem.SelectedIndex = Math.Min(4, m_sequence.ChannelCount - 1);
			channel6ToolStripMenuItem.SelectedIndex = Math.Min(5, m_sequence.ChannelCount - 1);
			channel7ToolStripMenuItem.SelectedIndex = Math.Min(6, m_sequence.ChannelCount - 1);
			channel8ToolStripMenuItem.SelectedIndex = Math.Min(7, m_sequence.ChannelCount - 1);
			channel9ToolStripMenuItem.SelectedIndex = Math.Min(8, m_sequence.ChannelCount - 1);
			channel0ToolStripMenuItem.SelectedIndex = Math.Min(9, m_sequence.ChannelCount - 1);
			comboBoxAudioDevice.Items.Add("Use application's default device");
			comboBoxAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
			comboBoxAudioDevice.SelectedIndex = m_sequence.AudioDeviceIndex + 1;
		}

		private void AudioDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			Stop();
			m_fmod.ReleaseSound(m_soundChannel);
			m_fmod.Shutdown();
		}

		private void AudioDialog_KeyDown(object sender, KeyEventArgs e)
		{
			UpdateKeyState(e, true);
		}

		private void AudioDialog_KeyUp(object sender, KeyEventArgs e)
		{
			UpdateKeyState(e, false);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			m_sequence.Audio = m_originalAudio;
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < m_newEventValues.GetLength(1); i++)
			{
				m_newEventValues[listBoxChannels.SelectedIndex, i] = 0;
			}
			MessageBox.Show(
				string.Format("Channel \"{0}\" cleared of new events.", ((Channel) listBoxChannels.SelectedItem).Name),
				Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = Paths.AudioPath;
			openFileDialog1.DefaultExt = "wma";
			openFileDialog1.Filter = "All supported formats | *.aiff;*.asf;*.flac;*.mp2;*.mp3;*.ogg;*.wav;*.wma;*.mid";
			openFileDialog1.FileName = string.Empty;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string path = Path.Combine(Paths.AudioPath, Path.GetFileName(openFileDialog1.FileName));
				if (!File.Exists(path))
				{
					Cursor = Cursors.WaitCursor;
					File.Copy(openFileDialog1.FileName, path);
					Cursor = Cursors.Default;
				}
				m_sequence.Audio = LoadAudio(openFileDialog1.FileName);
				UpdateRecordableLength();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if ((m_sequence.Audio != null) && (checkBoxAutoSize.Checked || (m_sequence.Audio.Duration > m_sequence.Time)))
			{
				m_sequence.Time = m_sequence.Audio.Duration;
			}
			m_sequence.AudioDeviceIndex = comboBoxAudioDevice.SelectedIndex - 1;
			int num2 = Math.Min(m_newEventValues.GetLength(0), m_sequence.EventValues.GetLength(0));
			int num3 = Math.Min(m_newEventValues.GetLength(1), m_sequence.EventValues.GetLength(1));
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					byte num1 = m_sequence.EventValues[i, j];
					num1 = (byte) (num1 | m_newEventValues[i, j]);
				}
			}
		}

		private void buttonPlayPause_Click(object sender, EventArgs e)
		{
			if (m_playing)
			{
				buttonPlayPause.Image = m_paused ? pictureBoxPause.Image : pictureBoxPlayBlue.Image;
				if (m_soundChannel != null)
				{
					m_soundChannel.Paused = !m_paused;
				}
				m_timer.Enabled = m_paused;
				m_paused = !m_paused;
				if (!(m_paused || !progressBarCountdown.Visible))
				{
					m_countdownEnd = DateTime.Now + TimeSpan.FromSeconds((((progressBarCountdown.Value)/100f)*5f));
				}
			}
			else
			{
				m_countdownEnd = DateTime.Now.Add(TimeSpan.FromSeconds(5.0));
				progressBarCountdown.Value = 100;
				progressBarCountdown.Visible = true;
				m_playing = true;
				labelTime.Text = "Countdown...";
				labelTotalTime.Text = string.Empty;
				trackBarPosition.Enabled = false;
				m_timer.Start();
				buttonPlayPause.Image = pictureBoxPause.Image;
			}
		}

		private void buttonRemoveAudio_Click(object sender, EventArgs e)
		{
			ClearAudio();
			UpdateRecordableLength();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			Stop();
		}

		private void channelMapItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			var box = sender as ToolStripComboBox;
			if (box.SelectedIndex != -1)
			{
				int index = Convert.ToInt32(box.Tag) - 1;
				m_keyMap[index] = box.SelectedIndex;
			}
		}

		private void checkBoxAutoSize_CheckedChanged(object sender, EventArgs e)
		{
			UpdateRecordableLength();
		}

		private void ClearAudio()
		{
			m_sequence.Audio = null;
			labelAudioFileName.Text = "This event sequence does not have audio assigned";
			labelAudioLength.Text = string.Empty;
			labelAudioName.Text = string.Empty;
			m_soundChannel = null;
		}

		private void CopyArray(byte[,] source, byte[,] dest)
		{
			int num = Math.Min(source.GetLength(0), dest.GetLength(0));
			int num2 = Math.Min(source.GetLength(1), dest.GetLength(1));
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					dest[i, j] = source[i, j];
				}
			}
		}


		private uint GetPosition()
		{
			if ((m_soundChannel != null) && m_soundChannel.IsPlaying)
			{
				return m_soundChannel.Position;
			}
			return (uint) (m_stopwatch.ElapsedMilliseconds + m_timeOffset);
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(AudioDialog));
		//this.pictureBoxPlay.Image = (Image)manager.GetObject("pictureBoxPlay.Image");
		//this.pictureBoxPlayBlue.Image = (Image)manager.GetObject("pictureBoxPlayBlue.Image");
		//this.pictureBoxPause.Image = (Image)manager.GetObject("pictureBoxPause.Image");
		//this.buttonStop.Image = (Image)manager.GetObject("buttonStop.Image");
		//this.buttonPlayPause.Image = (Image) manager.GetObject("buttonPlayPause.Image");


		private void linkLabelAssignedKeys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			contextMenuStrip.Show(linkLabelAssignedKeys.PointToScreen(new Point(0, linkLabelAssignedKeys.Height)));
		}

		private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAudioButtons();
		}

		private Audio LoadAudio(string fileName)
		{
			if (fileName == string.Empty)
			{
				return null;
			}
			try
			{
				m_soundChannel = m_fmod.LoadSound(Path.Combine(Paths.AudioPath, fileName), m_soundChannel);
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error loading audio:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
				return null;
			}
			if (m_soundChannel == null)
			{
				return null;
			}
			m_audioFilename = fileName;
			labelAudioFileName.Text = Path.GetFileName(m_audioFilename);
			var audio = new Audio();
			audio.FileName = labelAudioFileName.Text;
			audio.Name = m_soundChannel.SoundName;
			audio.Duration = (int) m_soundChannel.SoundLength;
			labelAudioLength.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", audio.Duration/0xea60, (audio.Duration%0xea60)/0x3e8,
			                                      audio.Duration%0x3e8);
			labelAudioName.Text = string.Format("\"{0}\"", audio.Name);
			UpdateAudioButtons();
			return audio;
		}

		private void m_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			MethodInvoker method = null;
			if (progressBarCountdown.Visible)
			{
				if (progressBarCountdown.Value <= 0)
				{
					UpdateTotalTime();
					var num = (int) (((int) base.Invoke(new TrackBarValueDelegate(TrackBarValue)))*m_smallChange);
					m_timeOffset = num;
					if ((m_soundChannel != null) && (num < m_soundChannel.SoundLength))
					{
						m_fmod.Play(m_soundChannel, true);
						m_soundChannel.Position = (uint) num;
						m_soundChannel.Paused = false;
					}
					m_stopwatch.Reset();
					m_stopwatch.Start();
					base.Invoke(new ProgressBarVisibleDelegate(ProgressBarVisible), new object[] {false});
				}
				else
				{
					if (method == null)
					{
						method = delegate
							{
								TimeSpan span = (m_countdownEnd - DateTime.Now);
								progressBarCountdown.Value = (int) ((span.TotalMilliseconds*100.0)/5000.0);
							};
					}
					base.BeginInvoke(method);
				}
			}
			else
			{
				MethodInvoker invoker = null;
				uint position = GetPosition();
				if (position >= m_maxTime)
				{
					Stop();
				}
				else
				{
					var num2 = (int) ((position)/(m_sequence.EventPeriod));
					if (num2 != m_lastIndex)
					{
						m_lastIndex = num2;
						for (int i = 0; i < m_sequence.ChannelCount; i++)
						{
							if (m_keyStates[i])
							{
								m_newEventValues[i, num2] = m_sequence.MaximumLevel;
								if (radioButtonSingleEvent.Checked)
								{
									m_keyStates[i] = false;
								}
							}
						}
					}
					if (invoker == null)
					{
						invoker = delegate
							{
								labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", position/0xea60, (position%0xea60)/0x3e8, position%0x3e8);
								trackBarPosition.Value = (int) ((position)/m_smallChange);
							};
					}
					base.BeginInvoke(invoker);
				}
			}
		}

		private void ProgressBarVisible(bool value)
		{
			progressBarCountdown.Visible = value;
		}

		private void Stop()
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new MethodInvoker(Stop));
			}
			else
			{
				m_timer.Stop();
				Thread.Sleep((int) m_timer.Interval);
				if (m_soundChannel != null)
				{
					m_fmod.Stop(m_soundChannel);
				}
				m_stopwatch.Stop();
				buttonPlayPause.Image = pictureBoxPlay.Image;
				labelTime.Text = "00:00.000";
				m_playing = m_paused = false;
				trackBarPosition.Enabled = true;
				trackBarPosition.Value = 0;
				progressBarCountdown.Visible = false;
			}
		}

		private void trackBarPosition_Scroll(object sender, EventArgs e)
		{
			var num = (int) (trackBarPosition.Value*m_smallChange);
			labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", num/0xea60, (num%0xea60)/0x3e8, num%0x3e8);
		}

		private int TrackBarValue()
		{
			return trackBarPosition.Value;
		}

		private void UpdateAudioButtons()
		{
			Channel selectedItem = null;
			if (listBoxChannels.SelectedItem != null)
			{
				selectedItem = (Channel) listBoxChannels.SelectedItem;
			}
			if (selectedItem != null)
			{
				buttonClear.Enabled = selectedItem.Enabled;
			}
			else
			{
				buttonClear.Enabled = false;
			}
		}

		private void UpdateKeyState(KeyEventArgs e, bool state)
		{
			if (!progressBarCountdown.Visible)
			{
				if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9))
				{
					if (e.KeyCode == Keys.D0)
					{
						m_keyStates[m_keyMap[9]] = state;
					}
					else
					{
						m_keyStates[m_keyMap[((int) e.KeyCode) - 0x31]] = state;
					}
				}
				else if ((e.KeyCode == Keys.ControlKey) && (listBoxChannels.SelectedItem != null))
				{
					m_keyStates[listBoxChannels.SelectedIndex] = state;
				}
			}
		}

		private void UpdateRecordableLength()
		{
			int num = UpdateTotalTime();
			m_maxTime = num;
			trackBarPosition.Maximum = num;
			UpdateTrackbar();
			var dest = new byte[m_newEventValues.GetLength(0),(int) Math.Ceiling(((num)/((float) m_sequence.EventPeriod)))];
			CopyArray(m_newEventValues, dest);
			m_newEventValues = dest;
		}

		private int UpdateTotalTime()
		{
			if (base.InvokeRequired)
			{
				int milliseconds = 0;
				base.Invoke((MethodInvoker) delegate
					{
						// TODO: Can change this to "base.Invoke(() => {" when we go to 3.x
						milliseconds = UpdateTotalTime();
					});
				return milliseconds;
			}
			int num = (m_sequence.Audio != null)
				          ? (checkBoxAutoSize.Checked
					             ? m_sequence.Audio.Duration
					             : Math.Max(m_sequence.Time, m_sequence.Audio.Duration))
				          : m_sequence.Time;
			labelTotalTime.Text = string.Format("/ {0:d2}:{1:d2}.{2:d3}", num/0xea60, (num%0xea60)/0x3e8, num%0x3e8);
			return num;
		}

		private void UpdateTrackbar()
		{
			if (trackBarPosition.Maximum < 0x7d0)
			{
				m_smallChange = 100f;
			}
			else if (trackBarPosition.Maximum < 0x4e20)
			{
				m_smallChange = 1000f;
			}
			else if (trackBarPosition.Maximum < 0xea60)
			{
				m_smallChange = 2000f;
			}
			else
			{
				m_smallChange = 5000f;
			}
			trackBarPosition.Maximum =
				(int) Math.Round(((trackBarPosition.Maximum)/m_smallChange), MidpointRounding.AwayFromZero);
		}

		private delegate void ProgressBarVisibleDelegate(bool value);

		private delegate int TrackBarValueDelegate();
	}
}