using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using CommonUtils;

namespace VixenEditor {

    internal partial class TestChannelsDialog : Form {
        private readonly byte[] _channelLevels;
        private readonly List<Channel> _channels;
        private readonly int _executionContextHandle;
        private readonly IExecution _executionInterface;
        private bool _suppressSelectedIndexChange;
        private readonly bool _actualLevels;


        public TestChannelsDialog(EventSequence sequence, IExecution executionInterface, bool constrainToGroup) {
            InitializeComponent();
            _executionInterface = executionInterface;
            _channels = constrainToGroup ? sequence.Channels : sequence.FullChannels;
            if (_channels != null) {
                // ReSharper disable CoVariantArrayConversion
                listBoxChannels.Items.AddRange(_channels.ToArray());
                // ReSharper restore CoVariantArrayConversion
            }
            _actualLevels = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ActualLevels");
            trackBar.Maximum = _actualLevels ? Utils.Cell8BitMax : Utils.Cell8BitMax.ToPercentage();
            _channelLevels = new byte[sequence.FullChannelCount];
            _executionContextHandle = _executionInterface.RequestContext(false, true, null);
            _executionInterface.SetAsynchronousContext(_executionContextHandle, sequence);
            BringToFront();
            trackBar.Value = trackBar.Maximum;
        }


        private void buttonAllOff_Click(object sender, EventArgs e) {
            SetChannelLevel(0, false);
        }


        private void buttonAllOn_Click(object sender, EventArgs e) {
            SetChannelLevel(LevelFromTrackBar(), true);
        }


        private void SetChannelLevel(byte level, bool isSelected) {
            _suppressSelectedIndexChange = true;
            listBoxChannels.BeginUpdate();
            for (var i = 0; i < listBoxChannels.Items.Count; i++) {
                listBoxChannels.SetSelected(i, isSelected);
                _channelLevels[_channels[i].OutputChannel] = level;
            }
            listBoxChannels.EndUpdate();
            _suppressSelectedIndexChange = false;
            UpdateOutput();
        }


        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }


        private byte LevelFromTrackBar() {
            return (byte) (_actualLevels ? trackBar.Value : trackBar.Value.ToValue());
        }


        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
            if (_suppressSelectedIndexChange) {
                return;
            }

            for (var channel = 0; channel < _channelLevels.Length; channel++) {
                _channelLevels[channel] = 0;
            }

            for(var i = 0; i < listBoxChannels.Items.Count; i++) {
                _channelLevels[_channels[i].OutputChannel] = listBoxChannels.GetSelected(i) ? LevelFromTrackBar() : ((byte) 0);
            }
            UpdateOutput();
        }


        private void TestChannelsDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _executionInterface.ReleaseContext(_executionContextHandle);
        }


        private void trackBar_ValueChanged(object sender, EventArgs e) {
            labelLevel.Text = trackBar.Value.ToString(CultureInfo.InvariantCulture);
            var level = (byte) (_actualLevels ? trackBar.Value : trackBar.Value.ToValue());
            foreach (int channel in listBoxChannels.SelectedIndices) {
                _channelLevels[_channels[channel].OutputChannel] = level;
            }
            UpdateOutput();
        }


        private void UpdateOutput() {
            _executionInterface.SetChannelStates(_executionContextHandle, _channelLevels);
        }


        private void listBox_DrawItem(object sender, DrawItemEventArgs e) {
            var listBox = sender as ListBox;
            if (listBox == null) {
                return;
            }

            Channel.DrawItem(listBox, e, _channels[e.Index]);
        }
    }
}
