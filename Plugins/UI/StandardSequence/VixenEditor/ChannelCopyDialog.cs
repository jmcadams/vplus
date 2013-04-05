namespace VixenEditor {
	using System;
	using System.Collections.Generic;
    using System.Windows.Forms;
	using VixenPlus;

	internal partial class ChannelCopyDialog : Form {
		private readonly AffectGridDelegate _affectGridDelegate;
		private readonly EventSequence _eventSequence;
		private readonly List<int> _sortOrder;
		private readonly byte[,] _sequenceData;

		public ChannelCopyDialog(AffectGridDelegate affectGridDelegate, EventSequence sequence, List<int> sortOrder) {
			InitializeComponent();
			var channels = new Channel[sequence.ChannelCount];
			for (var i = 0; i < sortOrder.Count; i++) {
				channels[i] = sequence.Channels[sortOrder[i]];
			}
			comboBoxSourceChannel.Items.AddRange(channels);
			comboBoxDestinationChannel.Items.AddRange(channels);
			if (comboBoxSourceChannel.Items.Count > 0) {
				comboBoxSourceChannel.SelectedIndex = 0;
			}
			if (comboBoxDestinationChannel.Items.Count > 0) {
				comboBoxDestinationChannel.SelectedIndex = 0;
			}
			_eventSequence = sequence;
			_sortOrder = sortOrder;
			_sequenceData = new byte[1, sequence.TotalEventPeriods];
			_affectGridDelegate = affectGridDelegate;
		}

		private void buttonCopy_Click(object sender, EventArgs e) {
			var num = _sortOrder[comboBoxSourceChannel.SelectedIndex];
			for (var i = 0; i < _eventSequence.TotalEventPeriods; i++) {
				_sequenceData[0, i] = _eventSequence.EventValues[num, i];
			}
			_affectGridDelegate(comboBoxDestinationChannel.SelectedIndex, 0, _sequenceData);
		}

		private void buttonDone_Click(object sender, EventArgs e) {
			Close();
		}
	}
}