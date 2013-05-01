using System.Drawing;

using CommonUtils;

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
        private readonly Channel[] _channels;

        public ChannelCopyDialog(AffectGridDelegate affectGridDelegate, EventSequence sequence, List<int> sortOrder) {
            InitializeComponent();
            _channels = new Channel[sequence.ChannelCount];
            for (var channel = 0; channel < sortOrder.Count; channel++) {
                _channels[channel] = sequence.Channels[sortOrder[channel]];
            }
            // ReSharper disable CoVariantArrayConversion
            comboBoxSourceChannel.Items.AddRange(_channels);
            comboBoxDestinationChannel.Items.AddRange(_channels);
            // ReSharper restore CoVariantArrayConversion
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


        private void comboBox_DrawItem(object sender, DrawItemEventArgs e) {
            var comboBox = sender as ComboBox;
            if (comboBox == null) {
                return;
            }

            e.DrawBackground();

            using (var backgroundBrush = new SolidBrush(_channels[e.Index].Color))
            using (var g = e.Graphics) {
                g.FillRectangle(backgroundBrush, e.Bounds);
                var contrastingBrush = Utils.GetTextColor(backgroundBrush.Color);
                g.DrawString(_channels[e.Index].Name, e.Font, contrastingBrush, new RectangleF(e.Bounds.Location, e.Bounds.Size));
            }

            e.DrawFocusRectangle();
        }
    }
}
