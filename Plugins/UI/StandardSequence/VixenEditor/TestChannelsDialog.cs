namespace VixenEditor {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	internal partial class TestChannelsDialog : Form {
		private byte[] m_channelLevels;
		private List<Channel> m_channels;
		private int m_executionContextHandle;
		private IExecution m_executionInterface;
		private bool m_internal = false;
		private EventSequence m_sequence;

		public TestChannelsDialog(EventSequence sequence, IExecution executionInterface) {
			this.InitializeComponent();
			this.m_executionInterface = executionInterface;
			this.m_sequence = sequence;
			this.m_channels = sequence.Channels;
			this.listBoxChannels.Items.AddRange(this.m_channels.ToArray());
			this.trackBar.Maximum = ((ISystem)Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ActualLevels") ? 0xff : 100;
			this.m_channelLevels = new byte[sequence.ChannelCount];
			this.m_executionContextHandle = this.m_executionInterface.RequestContext(false, true, null);
			this.m_executionInterface.SetAsynchronousContext(this.m_executionContextHandle, this.m_sequence);
			base.BringToFront();
			this.trackBar.Value = this.trackBar.Maximum;
		}

		private void buttonAllOff_Click(object sender, EventArgs e) {
			this.m_internal = true;
			this.listBoxChannels.BeginUpdate();
			for (int i = 0; i < this.listBoxChannels.Items.Count; i++) {
				this.listBoxChannels.SetSelected(i, false);
				this.m_channelLevels[this.m_channels[i].OutputChannel] = 0;
			}
			this.listBoxChannels.EndUpdate();
			this.m_internal = false;
			this.UpdateOutput();
		}

		private void buttonAllOn_Click(object sender, EventArgs e) {
			this.m_internal = true;
			this.listBoxChannels.BeginUpdate();
			for (int i = 0; i < this.listBoxChannels.Items.Count; i++) {
				this.listBoxChannels.SetSelected(i, true);
				this.m_channelLevels[this.m_channels[i].OutputChannel] = this.LevelFromTrackBar();
			}
			this.listBoxChannels.EndUpdate();
			this.m_internal = false;
			this.UpdateOutput();
		}

		private void buttonDone_Click(object sender, EventArgs e) {
			base.Close();
		}

		private byte LevelFromTrackBar() {
			return ((this.trackBar.Maximum == 0xff) ? ((byte)this.trackBar.Value) : ((byte)Math.Round((double)((this.trackBar.Value * 255f) / 100f))));
		}

		private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
			if (!this.m_internal) {
				this.UpdateAllChannels();
			}
		}

		private void TestChannelsDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
		}

		private void trackBar_ValueChanged(object sender, EventArgs e) {
			this.labelLevel.Text = this.trackBar.Value.ToString();
			byte num = (this.trackBar.Maximum == 0xff) ? ((byte)this.trackBar.Value) : ((byte)Math.Round((double)((this.trackBar.Value * 255f) / 100f)));
			foreach (int num2 in this.listBoxChannels.SelectedIndices) {
				this.m_channelLevels[this.m_channels[num2].OutputChannel] = num;
			}
			this.UpdateOutput();
		}

		private void UpdateAllChannels() {
			for (int i = 0; i < this.m_channelLevels.Length; i++) {
				this.m_channelLevels[this.m_channels[i].OutputChannel] = this.listBoxChannels.GetSelected(i) ? this.LevelFromTrackBar() : ((byte)0);
			}
			this.UpdateOutput();
		}

		private void UpdateOutput() {
			this.m_executionInterface.SetChannelStates(this.m_executionContextHandle, this.m_channelLevels);
		}
	}
}