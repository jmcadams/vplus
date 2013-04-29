using System.Drawing;
using System.Globalization;

namespace VixenEditor {
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using VixenPlus;

    internal partial class TestChannelsDialog : Form {
        private readonly byte[] _channelLevels;
        private readonly List<Channel> _channels;
        private readonly int _executionContextHandle;
        private readonly IExecution _executionInterface;
        private bool _internal;
        private readonly EventSequence _sequence;

        public TestChannelsDialog(EventSequence sequence, IExecution executionInterface) {
            InitializeComponent();
            _executionInterface = executionInterface;
            _sequence = sequence;
            _channels = sequence.Channels;
            if (_channels != null) {
                listBoxChannels.Items.AddRange(_channels.ToArray());
            }
            trackBar.Maximum = ((ISystem)Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ActualLevels") ? 0xff : 100;
            _channelLevels = new byte[sequence.ChannelCount];
            _executionContextHandle = _executionInterface.RequestContext(false, true, null);
            _executionInterface.SetAsynchronousContext(_executionContextHandle, _sequence);
            BringToFront();
            trackBar.Value = trackBar.Maximum;
        }

        private void buttonAllOff_Click(object sender, EventArgs e) {
            _internal = true;
            listBoxChannels.BeginUpdate();
            for (int i = 0; i < listBoxChannels.Items.Count; i++) {
                listBoxChannels.SetSelected(i, false);
                _channelLevels[_channels[i].OutputChannel] = 0;
            }
            listBoxChannels.EndUpdate();
            _internal = false;
            UpdateOutput();
        }

        private void buttonAllOn_Click(object sender, EventArgs e) {
            _internal = true;
            listBoxChannels.BeginUpdate();
            for (var i = 0; i < listBoxChannels.Items.Count; i++) {
                listBoxChannels.SetSelected(i, true);
                _channelLevels[_channels[i].OutputChannel] = LevelFromTrackBar();
            }
            listBoxChannels.EndUpdate();
            _internal = false;
            UpdateOutput();
        }

        private void buttonDone_Click(object sender, EventArgs e) {
            Close();
        }

        private byte LevelFromTrackBar() {
            return ((trackBar.Maximum == 0xff) ? ((byte)trackBar.Value) : ((byte)Math.Round(trackBar.Value * 255f / 100f)));
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
            if (!_internal) {
                UpdateAllChannels();
            }
        }

        private void TestChannelsDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _executionInterface.ReleaseContext(_executionContextHandle);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e) {
            labelLevel.Text = trackBar.Value.ToString(CultureInfo.InvariantCulture);
            var num = (trackBar.Maximum == 255) ? ((byte)trackBar.Value) : ((byte)Math.Round(trackBar.Value * 255f / 100f));
            foreach (int num2 in listBoxChannels.SelectedIndices) {
                _channelLevels[_channels[num2].OutputChannel] = num;
            }
            UpdateOutput();
        }

        private void UpdateAllChannels() {
            for (var i = 0; i < _channelLevels.Length; i++) {
                _channelLevels[_channels[i].OutputChannel] = listBoxChannels.GetSelected(i) ? LevelFromTrackBar() : ((byte)0);
            }
            UpdateOutput();
        }

        private void UpdateOutput() {
            _executionInterface.SetChannelStates(_executionContextHandle, _channelLevels);
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            var selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            using (var brush = new SolidBrush(_channels[e.Index].Color)) {
                var g = e.Graphics;
                var contrastingBrush = CommonUtils.Utils.GetTextColor(brush.Color);
                g.FillRectangle(brush, e.Bounds);

                g.DrawString(_channels[e.Index].Name, e.Font, contrastingBrush, listBoxChannels.GetItemRectangle(e.Index).Location);
            
                if (selected) {
                    g.DrawString("\u2714", e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
                }
            }

            e.DrawFocusRectangle();
        }
    }
}