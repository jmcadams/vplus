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
            for (var channel = 0; channel < sortOrder.Count; channel++) {
                channels[channel] = sequence.Channels[sortOrder[channel]];
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
            var channel = _sortOrder[comboBoxSourceChannel.SelectedIndex];
            for (var column = 0; column < _eventSequence.TotalEventPeriods; column++) {
                _sequenceData[0, column] = _eventSequence.EventValues[channel, column];
            }
            _affectGridDelegate(comboBoxDestinationChannel.SelectedIndex, 0, _sequenceData);
        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
