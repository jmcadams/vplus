using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VixenPlus;

namespace VixenEditor {

    //TODO: Convert this to simple renard tester

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
            _channels = sequence.Channels.ToArray();
            var strArray = new string[sequence.ChannelCount + 1];
            for (int i = 1; i <= sequence.ChannelCount; i++) {
                strArray[i] = sequence.Channels[i - 1].Name;
            }
            strArray[0] = " -- unassigned -- ";
            foreach (Control control in groupBox2.Controls) {
                var item = control as ConsoleTrackBar;
                if ((item != null) &&
                    (item.Master != null)) {
                    item.TextStrings = strArray;
                    item.ResetIndex = 0;
                    _channelControls.Add(item);
                }
            }
        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private void consoleTrackBar_ValueChanged(object sender) {
            var bar = sender as ConsoleTrackBar;
            if (bar != null &&
                bar.SelectedTextIndex > 0) {
                UpdateChannelFrom(sender as ConsoleTrackBar);
                UpdateOutput();
            }
        }

        private void consoleTrackBarMaster_ValueChanged(object sender) {
            foreach (var bar in _channelControls) {
                UpdateChannelFrom(bar);
            }
            UpdateOutput();
        }

        private void TestConsoleDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _executionInterface.ReleaseContext(_executionContextHandle);
        }

        private void UpdateChannelFrom(ConsoleTrackBar consoleTrackBar) {
            if ((consoleTrackBar != null) &&
                (consoleTrackBar.SelectedTextIndex > 0)) {
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