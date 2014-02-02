using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Properties;

namespace Dialogs
{
    public partial class ChannelPropertyDialog : Form
    {
        private readonly Preference2 _preferences;
        private Channel _currentChannel;
        private bool _internalChange;
        private readonly List<Channel> _channels;


        public ChannelPropertyDialog(List<Channel> channels, Channel currentChannel, bool showOutputChannel)
        {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            _preferences = Preference2.GetInstance();
            _channels = channels;
            _internalChange = true;
            comboBoxChannels.Items.AddRange(channels.ToArray());
            _internalChange = false;
            label3.Visible = labelOutputChannel.Visible = showOutputChannel;
            GotoChannel(currentChannel);

            colorDialog.CustomColors = _preferences.CustomColors;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            ToChannel();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = _currentChannel.Color;
            if (colorDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            buttonColor.BackColor = colorDialog.Color;
            _preferences.CustomColors = colorDialog.CustomColors;
            ToChannel();
            comboBoxChannels.Refresh();
        }

        private void buttonDimmingCurve_Click(object sender, EventArgs e)
        {
            var dialog = new DimmingCurveDialog(null, _currentChannel);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextChannel();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            ToChannel();
            GotoChannel((Channel) comboBoxChannels.Items[comboBoxChannels.Items.IndexOf(_currentChannel) - 1]);
        }

        private void ChannelPropertyDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != '\r') || !buttonNext.Enabled) {
                return;
            }
            NextChannel();
            if (ActiveControl == textBoxName)
            {
                textBoxName.SelectAll();
            }
            e.Handled = true;
        }

        private void ChannelPropertyDialog_Load(object sender, EventArgs e)
        {
            ActiveControl = buttonNext;
        }

        private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalChange) {
                return;
            }
            ToChannel();
            GotoChannel((Channel) comboBoxChannels.SelectedItem);
        }

        private void FromChannel()
        {
            textBoxName.Text = _currentChannel.Name;
            buttonColor.BackColor = _currentChannel.Color;
            labelOutputChannel.Text = (_currentChannel.OutputChannel + 1).ToString(CultureInfo.InvariantCulture);
            checkBoxEnabled.Checked = _currentChannel.Enabled;
        }

        private void GotoChannel(Channel channel)
        {
            _internalChange = true;
            _currentChannel = channel;
            comboBoxChannels.SelectedItem = _currentChannel;
            buttonPrev.Enabled = comboBoxChannels.Items.IndexOf(_currentChannel) > 0;
            buttonNext.Enabled = comboBoxChannels.Items.IndexOf(_currentChannel) < (comboBoxChannels.Items.Count - 1);
            buttonDimmingCurve.Visible = _currentChannel.CanDoDimming;
            _internalChange = false;
            FromChannel();
        }

        private void NextChannel()
        {
            ToChannel();
            GotoChannel((Channel) comboBoxChannels.Items[comboBoxChannels.Items.IndexOf(_currentChannel) + 1]);
        }

        private void ToChannel()
        {
            var name = _currentChannel.Name;
            _currentChannel.Name = textBoxName.Text;
            if (name != textBoxName.Text)
            {
                var selectedIndex = comboBoxChannels.SelectedIndex;
                var index = comboBoxChannels.Items.IndexOf(_currentChannel);
                _internalChange = true;
                comboBoxChannels.BeginUpdate();
                comboBoxChannels.Items.RemoveAt(index);
                comboBoxChannels.Items.Insert(index, _currentChannel);
                // ReSharper disable RedundantCheckBeforeAssignment
                if (selectedIndex != comboBoxChannels.SelectedIndex) {
                    comboBoxChannels.SelectedIndex = selectedIndex;
                }
                // ReSharper restore RedundantCheckBeforeAssignment
                comboBoxChannels.EndUpdate();
                _internalChange = false;
            }
            _currentChannel.Color = buttonColor.BackColor;
            _currentChannel.Enabled = checkBoxEnabled.Checked;
        }

        private void comboBoxChannels_DrawItem(object sender, DrawItemEventArgs e) {
            Channel.DrawItem(e, _channels[e.Index]);
        }
    }
}
