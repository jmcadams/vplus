namespace Waveform {
	using FMOD;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Windows.Forms;
	using VixenPlus;

	public partial class ParamsDialog : Form {
		private int m_bits = 0;
		private int m_channels = 0;
		private bool m_completed = false;
		private int m_highestValue = 0;
		private uint m_len1 = 0;
		private uint m_len2 = 0;
		private uint m_lenbytes = 0;
		private uint m_lenMilliseconds = 0;
		private short[] m_levels;
		private IntPtr m_ptr1 = IntPtr.Zero;
		private IntPtr m_ptr2 = IntPtr.Zero;
		private float m_rate = 0f;
		private RunState m_run = RunState.Stopped;
		private float m_scale;
		private EventSequence m_sequence;
		private Sound m_sound = null;
		private _System m_system = null;

		public ParamsDialog(EventSequence sequence) {
			this.InitializeComponent();
			this.m_sequence = sequence;
			this.m_stepInvoker = new MethodInvoker(this.progressBar1.PerformStep);
			this.listBoxChannels.Items.AddRange(sequence.Channels.ToArray());
			Factory.System_Create(ref this.m_system);
			this.m_system.init(2, INITFLAG.NORMAL, IntPtr.Zero);
		}

		private void buttonStart_Click(object sender, EventArgs e) {
			if (this.buttonStart.Text == "Start") {
				if (this.listBoxChannels.SelectedItems.Count == 0) {
					MessageBox.Show("There are no channels selected", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if ((!this.radioButtonChannelRange.Checked || (this.listBoxChannels.SelectedItems.Count != 1)) || (MessageBox.Show("You selected to use multiple channels but only selected one channel.\nIs this really what you want?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)) {
					RESULT result;
					this.buttonStart.Enabled = false;
					SOUND_TYPE rAW = SOUND_TYPE.RAW;
					SOUND_FORMAT nONE = SOUND_FORMAT.NONE;
					float volume = 0f;
					float pan = 0f;
					int priority = 0;
					this.label1.Text = "Loading audio, please wait...";
					this.label1.Refresh();
					if ((result = this.m_system.createSound(Path.Combine(Paths.AudioPath, this.m_sequence.Audio.FileName), MODE.ACCURATETIME | MODE._2D | MODE.HARDWARE | MODE.CREATESAMPLE, ref this.m_sound)) != RESULT.OK) {
						MessageBox.Show(string.Format("Sound system error ({0})\n\n{1}", result, Error.String(result)), "Waveform", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						this.buttonStart.Enabled = true;
						this.label1.Text = string.Empty;
					}
					else {
						this.m_sound.getFormat(ref rAW, ref nONE, ref this.m_channels, ref this.m_bits);
						this.m_sound.getDefaults(ref this.m_rate, ref volume, ref pan, ref priority);
						this.m_sound.getLength(ref this.m_lenbytes, TIMEUNIT.PCMBYTES);
						this.m_sound.getLength(ref this.m_lenMilliseconds, TIMEUNIT.MS);
						this.buttonStart.Enabled = true;
						this.progressBar1.Maximum = (int)(((long)this.m_lenMilliseconds) / ((long)this.m_sequence.EventPeriod));
						this.progressBar1.Value = this.progressBar1.Minimum;
						this.progressBar1.Visible = true;
						this.label1.Text = "Calculating...";
						this.buttonStart.Text = "Stop";
						this.m_highestValue = 0;
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.ParsePCM));
					}
				}
			}
			else {
				this.RequestStop();
			}
		}

		private void checkBoxAutoScale_CheckedChanged(object sender, EventArgs e) {
			this.labelScale.Enabled = this.textBoxScale.Enabled = !this.checkBoxAutoScale.Checked;
		}

		private void Done() {
			this.ReleaseAllocations();
			if (!this.checkBoxAutoScale.Checked) {
				try {
					this.m_scale = Convert.ToSingle(this.textBoxScale.Text);
				}
				catch {
					MessageBox.Show(this.textBoxScale.Text + " is not a valid number.\nReverting to a default scale of 1.0", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.m_scale = 1f;
				}
			}
			else {
				this.m_scale = 32767f / ((float)this.m_highestValue);
			}
			int num = (1 << (this.m_bits & 0x1f)) >> 1;
			int num3 = 0;
			int num4 = 0;
			int selectedIndex = 0;
			int num6 = 0;
			int index = 0;
			try {
				float num2;
				if (this.radioButtonChannelRange.Checked) {
					num3 = this.listBoxChannels.SelectedIndices[0];
					num4 = num3;
					selectedIndex = 1;
					while ((selectedIndex < this.listBoxChannels.SelectedIndices.Count) && (this.listBoxChannels.SelectedIndices[selectedIndex++] == (num4 + 1))) {
						num4++;
					}
					num4++;
					int num8 = num4 - num3;
					for (index = 0; index < this.m_levels.Length; index++) {
						num2 = ((float)this.m_levels[index]) / ((float)num);
						num2 = Math.Min((float)(num2 * this.m_scale), (float)1f);
						num6 = ((int)Math.Round((double)(num8 * num2), MidpointRounding.AwayFromZero)) + num3;
						selectedIndex = num3;
						while (selectedIndex < num6) {
							this.m_sequence.EventValues[selectedIndex, index] = this.m_sequence.MaximumLevel;
							selectedIndex++;
						}
						while (selectedIndex < num4) {
							this.m_sequence.EventValues[selectedIndex, index] = this.m_sequence.MinimumLevel;
							selectedIndex++;
						}
					}
				}
				else {
					selectedIndex = this.listBoxChannels.SelectedIndex;
					index = 0;
					while (index < this.m_levels.Length) {
						num2 = ((float)this.m_levels[index]) / ((float)num);
						num2 = Math.Min((float)(num2 * this.m_scale), (float)1f);
						this.m_sequence.EventValues[selectedIndex, index] = Math.Min(this.m_sequence.MaximumLevel, (byte)(num2 * 255f));
						index++;
					}
				}
			}
			catch (Exception exception) {
				throw new Exception(string.Format("Exception: {0}\n\nStart channel = {1}\nEnd channel = {2}\nUpper channel = {3}, Channel index = {4}, m_levels[i] = {5}", new object[] { exception.Message, num3, num4, num6, selectedIndex, this.m_levels[index] }));
			}
			this.label1.Text = "Completed";
			this.m_completed = true;
			MessageBox.Show("Done.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
			base.Close();
		}

		private short FindHighValue(int startSample, int endSample) {
			int num7;
			int num8;
			short num = -32768;
			int num2 = (this.m_bits >> 3) * this.m_channels;
			int num3 = startSample * num2;
			int num4 = endSample * num2;
			int num5 = endSample - startSample;
			int length = num2 * num5;
			byte[] destination = new byte[length];
			this.m_sound.@lock((uint)num3, (uint)length, ref this.m_ptr1, ref this.m_ptr2, ref this.m_len1, ref this.m_len2);
			Marshal.Copy(this.m_ptr1, destination, 0, length);
			num3 = 0;
			if (this.m_bits == 0x10) {
				for (num8 = 0; num8 < num5; num8++) {
					num7 = 0;
					while (num7 < this.m_channels) {
						num = Math.Max(num, BitConverter.ToInt16(destination, num3 + (num7 * 2)));
						num7++;
					}
					num3 += num2;
				}
			}
			else if (this.m_bits == 8) {
				for (num8 = 0; num8 < num5; num8++) {
					for (num7 = 0; num7 < this.m_channels; num7++) {
						num = Math.Max(num, (sbyte)destination[num3 + num7]);
					}
					num3 += num2;
				}
			}
			this.m_sound.unlock(this.m_ptr1, this.m_ptr2, this.m_len1, this.m_len2);
			return num;
		}

		private void ParamsDialog_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.m_run != RunState.Stopped) {
				if (MessageBox.Show("Do you want to stop?", "Waveform", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
					e.Cancel = true;
				}
				else {
					this.RequestStop();
				}
			}
			this.ReleaseAllocations();
		}

		private void ParsePCM(object obj) {
			double num = 1000.0 / ((double)this.m_sequence.EventPeriod);
			double num2 = (((double)this.m_lenbytes) / ((double)this.m_channels)) / ((double)(this.m_bits / 8));
			int num3 = (int)(((long)this.m_lenMilliseconds) / ((long)this.m_sequence.EventPeriod));
			double num4 = num2 / ((double)num3);
			this.m_levels = new short[num3];
			this.m_run = RunState.Running;
			this.m_completed = false;
			int index = 0;
			while ((index < num3) && (this.m_run == RunState.Running)) {
				int startSample = (int)Math.Round((double)(index * num4), MidpointRounding.AwayFromZero);
				int endSample = (int)Math.Round((double)((index + 1) * num4), MidpointRounding.AwayFromZero);
				short num7 = this.FindHighValue(startSample, endSample);
				this.m_highestValue = Math.Max(num7, this.m_highestValue);
				this.m_levels[index] = num7;
				base.BeginInvoke(this.m_stepInvoker);
				index++;
			}
			this.m_run = RunState.Stopped;
			if (index == num3) {
				base.BeginInvoke(new MethodInvoker(this.Done));
			}
		}

		private void radioButtonChannelRange_CheckedChanged(object sender, EventArgs e) {
			this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
		}

		private void radioButtonSingleChannel_CheckedChanged(object sender, EventArgs e) {
			this.listBoxChannels.SelectionMode = SelectionMode.One;
		}

		private void ReleaseAllocations() {
			if (this.m_sound != null) {
				this.m_sound.release();
				this.m_sound = null;
			}
			if (this.m_system != null) {
				this.m_system.close();
				this.m_system.release();
				this.m_system = null;
			}
		}

		private void RequestStop() {
			this.label1.Text = "Stopping...";
			this.buttonStart.Enabled = false;
			this.m_run = RunState.StopRequested;
			while (this.m_run != RunState.Stopped) {
			}
			this.progressBar1.Value = 0;
			this.progressBar1.Visible = false;
			this.label1.Text = "Stopped";
			this.buttonStart.Text = "Start";
			this.buttonStart.Enabled = true;
			if (this.m_sound != null) {
				this.m_sound.release();
				this.m_sound = null;
			}
		}

		public bool Completed {
			get {
				return this.m_completed;
			}
		}

		private enum RunState {
			Stopped,
			Running,
			StopRequested
		}
	}
}