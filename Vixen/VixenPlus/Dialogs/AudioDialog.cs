using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using FMOD;

namespace VixenPlus.Dialogs
{
	public partial class AudioDialog : Form
	{
		private readonly EventSequence _eventSequence;
		private readonly fmod _fmod;
		private readonly int[] _keyMap = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
		private readonly bool[] _keyStates;
		private readonly Audio _originalAudio;
		private readonly Stopwatch _stopwatch;
		private readonly System.Timers.Timer _timer;
		private string _audioFilename = string.Empty;
		private DateTime _countdownEnd;
		private int _lastIndex = -1;
		private int _maxTime;
		private byte[,] _newEventValues;
		private bool _paused;
		private bool _playing;
		private float _smallChange;
		private SoundChannel _soundChannel;
		private int _timeOffset;

		public AudioDialog(EventSequence sequence, bool autoSize, int deviceIndex)
		{
			InitializeComponent();
			_fmod = (deviceIndex > 0) ? fmod.GetInstance(deviceIndex) : fmod.GetInstance(-1);
			_timer = new System.Timers.Timer(10.0);
			_timer.Elapsed += TimerElapsed;
			_eventSequence = sequence;
			_keyStates = new bool[_eventSequence.ChannelCount];
			_stopwatch = new Stopwatch();
			_newEventValues = new byte[_eventSequence.EventValues.GetLength(0),_eventSequence.EventValues.GetLength(1)];
			listBoxChannels.Items.AddRange(_eventSequence.Channels.ToArray());
			_originalAudio = sequence.Audio;
			if (sequence.Audio != null)
			{
				if (LoadAudio(_eventSequence.Audio.FileName) == null)
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
			var items = (_eventSequence.Channels.ToArray());
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
			channel1ToolStripMenuItem.SelectedIndex = Math.Min(0, _eventSequence.ChannelCount - 1);
			channel2ToolStripMenuItem.SelectedIndex = Math.Min(1, _eventSequence.ChannelCount - 1);
			channel3ToolStripMenuItem.SelectedIndex = Math.Min(2, _eventSequence.ChannelCount - 1);
			channel4ToolStripMenuItem.SelectedIndex = Math.Min(3, _eventSequence.ChannelCount - 1);
			channel5ToolStripMenuItem.SelectedIndex = Math.Min(4, _eventSequence.ChannelCount - 1);
			channel6ToolStripMenuItem.SelectedIndex = Math.Min(5, _eventSequence.ChannelCount - 1);
			channel7ToolStripMenuItem.SelectedIndex = Math.Min(6, _eventSequence.ChannelCount - 1);
			channel8ToolStripMenuItem.SelectedIndex = Math.Min(7, _eventSequence.ChannelCount - 1);
			channel9ToolStripMenuItem.SelectedIndex = Math.Min(8, _eventSequence.ChannelCount - 1);
			channel0ToolStripMenuItem.SelectedIndex = Math.Min(9, _eventSequence.ChannelCount - 1);
			comboBoxAudioDevice.Items.Add("Use application's default device");
			comboBoxAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
			comboBoxAudioDevice.SelectedIndex = _eventSequence.AudioDeviceIndex + 1;
		}

		private void AudioDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			Stop();
			_fmod.ReleaseSound(_soundChannel);
			_fmod.Shutdown();
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
			_eventSequence.Audio = _originalAudio;
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < _newEventValues.GetLength(1); i++)
			{
				_newEventValues[listBoxChannels.SelectedIndex, i] = 0;
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
				_eventSequence.Audio = LoadAudio(openFileDialog1.FileName);
				UpdateRecordableLength();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if ((_eventSequence.Audio != null) &&
			    (checkBoxAutoSize.Checked || (_eventSequence.Audio.Duration > _eventSequence.Time)))
			{
				_eventSequence.Time = _eventSequence.Audio.Duration;
			}
			_eventSequence.AudioDeviceIndex = comboBoxAudioDevice.SelectedIndex - 1;
		}

		private void buttonPlayPause_Click(object sender, EventArgs e)
		{
			if (_playing)
			{
				buttonPlayPause.Image = _paused ? pictureBoxPause.Image : pictureBoxPlayBlue.Image;
				if (_soundChannel != null)
				{
					_soundChannel.Paused = !_paused;
				}
				_timer.Enabled = _paused;
				_paused = !_paused;
				if (!(_paused || !progressBarCountdown.Visible))
				{
					_countdownEnd = DateTime.Now + TimeSpan.FromSeconds((((progressBarCountdown.Value)/100f)*5f));
				}
			}
			else
			{
				_countdownEnd = DateTime.Now.Add(TimeSpan.FromSeconds(5.0));
				progressBarCountdown.Value = 100;
				progressBarCountdown.Visible = true;
				_playing = true;
				labelTime.Text = "Countdown...";
				labelTotalTime.Text = string.Empty;
				trackBarPosition.Enabled = false;
				_timer.Start();
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
			if (box != null && box.SelectedIndex != -1)
			{
				int index = Convert.ToInt32(box.Tag) - 1;
				_keyMap[index] = box.SelectedIndex;
			}
		}

		private void checkBoxAutoSize_CheckedChanged(object sender, EventArgs e)
		{
			UpdateRecordableLength();
		}

		private void ClearAudio()
		{
			_eventSequence.Audio = null;
			labelAudioFileName.Text = "This event sequence does not have audio assigned";
			labelAudioLength.Text = string.Empty;
			labelAudioName.Text = string.Empty;
			_soundChannel = null;
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
			if ((_soundChannel != null) && _soundChannel.IsPlaying)
			{
				return _soundChannel.Position;
			}
			return (uint) (_stopwatch.ElapsedMilliseconds + _timeOffset);
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
				_soundChannel = _fmod.LoadSound(Path.Combine(Paths.AudioPath, fileName), _soundChannel);
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error loading audio:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
				return null;
			}
			if (_soundChannel == null)
			{
				return null;
			}
			_audioFilename = fileName;
			labelAudioFileName.Text = Path.GetFileName(_audioFilename);
			var audio = new Audio
				{
					FileName = labelAudioFileName.Text,
					Name = _soundChannel.SoundName,
					Duration = (int) _soundChannel.SoundLength
				};
			labelAudioLength.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", audio.Duration/60000, (audio.Duration%60000)/1000,
			                                      audio.Duration%1000);
			labelAudioName.Text = string.Format("\"{0}\"", audio.Name);
			UpdateAudioButtons();
			return audio;
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (progressBarCountdown.Visible)
			{
				if (progressBarCountdown.Value <= 0)
				{
					UpdateTotalTime();
					var num = (int) (((int) Invoke(new TrackBarValueDelegate(TrackBarValue)))*_smallChange);
					_timeOffset = num;
					if ((_soundChannel != null) && (num < _soundChannel.SoundLength))
					{
						_fmod.Play(_soundChannel, true);
						_soundChannel.Position = (uint) num;
						_soundChannel.Paused = false;
					}
					_stopwatch.Reset();
					_stopwatch.Start();
					Invoke(new ProgressBarVisibleDelegate(ProgressBarVisible), new object[] {false});
				}
				else
				{
					MethodInvoker method = delegate
						{
							TimeSpan span = (_countdownEnd - DateTime.Now);
							progressBarCountdown.Value = (int) ((span.TotalMilliseconds*100.0)/5000.0);
						};
					BeginInvoke(method);
				}
			}
			else
			{
				uint position = GetPosition();
				if (position >= _maxTime)
				{
					Stop();
				}
				else
				{
					var num2 = (int) ((position)/(_eventSequence.EventPeriod));
					if (num2 != _lastIndex)
					{
						_lastIndex = num2;
						for (int i = 0; i < _eventSequence.ChannelCount; i++)
						{
							if (_keyStates[i])
							{
								_newEventValues[i, num2] = _eventSequence.MaximumLevel;
								if (radioButtonSingleEvent.Checked)
								{
									_keyStates[i] = false;
								}
							}
						}
					}
					MethodInvoker invoker = delegate
						{
							labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", position/60000, (position%60000)/1000, position%1000);
							trackBarPosition.Value = (int) ((position)/_smallChange);
						};
					BeginInvoke(invoker);
				}
			}
		}

		private void ProgressBarVisible(bool value)
		{
			progressBarCountdown.Visible = value;
		}

		private void Stop()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(Stop));
			}
			else
			{
				_timer.Stop();
				Thread.Sleep((int) _timer.Interval);
				if (_soundChannel != null)
				{
					_fmod.Stop(_soundChannel);
				}
				_stopwatch.Stop();
				buttonPlayPause.Image = pictureBoxPlay.Image;
				labelTime.Text = @"00:00.000";
				_playing = _paused = false;
				trackBarPosition.Enabled = true;
				trackBarPosition.Value = 0;
				progressBarCountdown.Visible = false;
			}
		}

		private void trackBarPosition_Scroll(object sender, EventArgs e)
		{
			var num = (int) (trackBarPosition.Value*_smallChange);
			labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", num/60000, (num%60000)/1000, num%1000);
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
			buttonClear.Enabled = selectedItem != null && selectedItem.Enabled;
		}

		private void UpdateKeyState(KeyEventArgs e, bool state)
		{
			if (!progressBarCountdown.Visible)
			{
				if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9))
				{
					if (e.KeyCode == Keys.D0)
					{
						_keyStates[_keyMap[9]] = state;
					}
					else
					{
						_keyStates[_keyMap[((int) e.KeyCode) - 49]] = state;
					}
				}
				else if ((e.KeyCode == Keys.ControlKey) && (listBoxChannels.SelectedItem != null))
				{
					_keyStates[listBoxChannels.SelectedIndex] = state;
				}
			}
		}

		private void UpdateRecordableLength()
		{
			int num = UpdateTotalTime();
			_maxTime = num;
			trackBarPosition.Maximum = num;
			UpdateTrackbar();
			var dest = new byte[_newEventValues.GetLength(0),(int) Math.Ceiling(((num)/((float) _eventSequence.EventPeriod)))];
			CopyArray(_newEventValues, dest);
			_newEventValues = dest;
		}

		private int UpdateTotalTime()
		{
			if (InvokeRequired)
			{
				int milliseconds = 0;
				Invoke((MethodInvoker) delegate
					{
						milliseconds = UpdateTotalTime();
					});
				return milliseconds;
			}
			int num = (_eventSequence.Audio != null)
				          ? (checkBoxAutoSize.Checked
					             ? _eventSequence.Audio.Duration
					             : Math.Max(_eventSequence.Time, _eventSequence.Audio.Duration))
				          : _eventSequence.Time;
			labelTotalTime.Text = string.Format("/ {0:d2}:{1:d2}.{2:d3}", num/60000, (num%60000)/1000, num%1000);
			return num;
		}

		private void UpdateTrackbar()
		{
			if (trackBarPosition.Maximum < 2000)
			{
				_smallChange = 100f;
			}
			else if (trackBarPosition.Maximum < 20000)
			{
				_smallChange = 1000f;
			}
			else if (trackBarPosition.Maximum < 60000)
			{
				_smallChange = 2000f;
			}
			else
			{
				_smallChange = 5000f;
			}
			trackBarPosition.Maximum =
				(int) Math.Round(((trackBarPosition.Maximum)/_smallChange), MidpointRounding.AwayFromZero);
		}

		private delegate void ProgressBarVisibleDelegate(bool value);

		private delegate int TrackBarValueDelegate();
	}
}