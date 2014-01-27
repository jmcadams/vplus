using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VixenPlus.Properties;

namespace VixenEditor {

    internal partial class TestConsoleDialog : Form {
        private readonly List<ConsoleTrackBar> _channelControls = new List<ConsoleTrackBar>();
        private readonly byte[] _channelLevels;
        private readonly int _executionContextHandle;
        private readonly IExecution _executionInterface;
        private readonly EventSequence _sequence;


        public TestConsoleDialog(EventSequence sequence, IExecution executionInterface, bool constrainToGroup) {
            InitializeComponent();
            _sequence = sequence;
            _executionInterface = executionInterface;
            _executionContextHandle = _executionInterface.RequestContext(false, true, null);
            _executionInterface.SetAsynchronousContext(_executionContextHandle, sequence);
            _channelLevels = new byte[_sequence.FullChannelCount];
            var channels = new Channel[constrainToGroup ? _sequence.ChannelCount + 1 : _sequence.FullChannelCount + 1];
            channels[0] = new Channel(Resources.Unassigned, Color.Gainsboro, 0);

            for (var channel = 1; channel <= channels.Length - 1; channel++) {
                channels[channel] = constrainToGroup ? _sequence.Channels[channel - 1] : _sequence.FullChannels[channel - 1];
            }

            foreach (var trackBar in groupBox2.Controls.Cast<object>().Select(control => control as ConsoleTrackBar).Where(trackBar => (trackBar != null) && (trackBar.Master != null))) {
                trackBar.TextStrings = channels;
                trackBar.ResetIndex = 0;
                _channelControls.Add(trackBar);
            }
        }


        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }


        private void consoleTrackBar_ValueChanged(object sender) {
            var trackBar = sender as ConsoleTrackBar;
            if (trackBar == null || trackBar.SelectedTextIndex <= 0) {
                return;
            }
            UpdateChannelFrom(sender as ConsoleTrackBar);
            UpdateOutput();
        }


        private void consoleTrackBarMaster_ValueChanged(object sender) {
            foreach (var trackBar in _channelControls) {
                UpdateChannelFrom(trackBar);
            }
            UpdateOutput();
        }


        private void TestConsoleDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _executionInterface.ReleaseContext(_executionContextHandle);
        }


        private void UpdateChannelFrom(ConsoleTrackBar consoleTrackBar) {
            if ((consoleTrackBar != null) && (consoleTrackBar.SelectedTextIndex > 0)) {
                UpdateChannelValue(consoleTrackBar.SelectedTextIndex - 1, (byte) consoleTrackBar.Value);
            }
        }


        private void UpdateChannelValue(int channelNaturalIndex, byte value) {
            _channelLevels[_sequence.Channels[channelNaturalIndex].OutputChannel] = value;
        }


        private void UpdateOutput() {
            _executionInterface.SetChannelStates(_executionContextHandle, _channelLevels);
        }
    }
}