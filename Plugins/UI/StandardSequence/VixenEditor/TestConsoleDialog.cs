using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Properties;

using VixenPlus;

namespace VixenEditor {

    internal partial class TestConsoleDialog : Form {
        private readonly List<ConsoleTrackBar> _channelControls = new List<ConsoleTrackBar>();
        private readonly byte[] _channelLevels;
        private readonly Channel[] _channels;
        private readonly int _executionContextHandle;
        private readonly IExecution _executionInterface;


        public TestConsoleDialog(EventSequence sequence, IExecution executionInterface) {
            InitializeComponent();
            _executionInterface = executionInterface;
            _executionContextHandle = _executionInterface.RequestContext(false, true, null);
            _executionInterface.SetAsynchronousContext(_executionContextHandle, sequence);
            _channelLevels = new byte[sequence.ChannelCount];
            _channels = new Channel[sequence.ChannelCount + 1];
            _channels[0] = new Channel(Resources.Unassigned, Color.WhiteSmoke, 0);

            for (var channel = 1; channel <= sequence.ChannelCount; channel++) {
                _channels[channel] = sequence.Channels[channel - 1];
            }

            foreach (var control in groupBox2.Controls) {
                var trackBar = control as ConsoleTrackBar;
                if ((trackBar == null) || (trackBar.Master == null)) {
                    continue;
                }

                trackBar.TextStrings = _channels;
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
            _channelLevels[_channels[channelNaturalIndex].OutputChannel] = value;
        }


        private void UpdateOutput() {
            _executionInterface.SetChannelStates(_executionContextHandle, _channelLevels);
        }
    }
}