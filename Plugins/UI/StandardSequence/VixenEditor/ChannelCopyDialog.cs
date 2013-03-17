namespace VixenEditor {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	internal partial class ChannelCopyDialog : Form {
		private AffectGridDelegate m_affectGrid;
		private EventSequence m_sequence;
		private List<int> m_sortOrder;
		private byte[,] m_values;

		public ChannelCopyDialog(AffectGridDelegate affectGrid, EventSequence sequence, List<int> sortOrder) {
			this.InitializeComponent();
			Channel[] items = new Channel[sequence.ChannelCount];
			for (int i = 0; i < sortOrder.Count; i++) {
				items[i] = sequence.Channels[sortOrder[i]];
			}
			this.comboBoxSourceChannel.Items.AddRange(items);
			this.comboBoxDestinationChannel.Items.AddRange(items);
			if (this.comboBoxSourceChannel.Items.Count > 0) {
				this.comboBoxSourceChannel.SelectedIndex = 0;
			}
			if (this.comboBoxDestinationChannel.Items.Count > 0) {
				this.comboBoxDestinationChannel.SelectedIndex = 0;
			}
			this.m_sequence = sequence;
			this.m_sortOrder = sortOrder;
			this.m_values = new byte[1, sequence.TotalEventPeriods];
			this.m_affectGrid = affectGrid;
		}

		private void buttonCopy_Click(object sender, EventArgs e) {
			int num = this.m_sortOrder[this.comboBoxSourceChannel.SelectedIndex];
			for (int i = 0; i < this.m_sequence.TotalEventPeriods; i++) {
				this.m_values[0, i] = this.m_sequence.EventValues[num, i];
			}
			this.m_affectGrid(this.comboBoxDestinationChannel.SelectedIndex, 0, this.m_values);
		}

		private void buttonDone_Click(object sender, EventArgs e) {
			base.Close();
		}
	}
}