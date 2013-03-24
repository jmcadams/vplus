using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
	public partial class ChannelPropertyDialog : Form
	{
		private readonly Preference2 _preferences;
		private Channel _currentChannel;
		private bool _internalChange;

		public ChannelPropertyDialog(List<Channel> channels, Channel currentChannel, bool showOutputChannel)
		{
			InitializeComponent();
			_internalChange = true;
			comboBoxChannels.Items.AddRange(new object[] {channels.ToArray()});
			_internalChange = false;
			label3.Visible = labelOutputChannel.Visible = showOutputChannel;
			GotoChannel(currentChannel);
			_preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
			string[] strArray = _preferences.GetString("CustomColors").Split(new[] {','});
			var numArray = new int[strArray.Length];
			for (int i = 0; i < strArray.Length; i++)
			{
				numArray[i] = int.Parse(strArray[i]);
			}
			colorDialog.CustomColors = numArray;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			ToChannel();
		}

		private void buttonColor_Click(object sender, EventArgs e)
		{
			colorDialog.Color = _currentChannel.Color;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				buttonColor.BackColor = colorDialog.Color;
				var strArray = new string[colorDialog.CustomColors.Length];
				for (int i = 0; i < strArray.Length; i++)
				{
					strArray[i] = colorDialog.CustomColors[i].ToString(CultureInfo.InvariantCulture);
				}
				_preferences.SetString("CustomColors", string.Join(",", strArray));
			}
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
			if ((e.KeyChar == '\r') && buttonNext.Enabled)
			{
				NextChannel();
				if (ActiveControl == textBoxName)
				{
					textBoxName.SelectAll();
				}
				e.Handled = true;
			}
		}

		private void ChannelPropertyDialog_Load(object sender, EventArgs e)
		{
			ActiveControl = buttonNext;
		}

		private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_internalChange)
			{
				ToChannel();
				GotoChannel((Channel) comboBoxChannels.SelectedItem);
			}
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
			string name = _currentChannel.Name;
			_currentChannel.Name = textBoxName.Text;
			if (name != textBoxName.Text)
			{
				int selectedIndex = comboBoxChannels.SelectedIndex;
				int index = comboBoxChannels.Items.IndexOf(_currentChannel);
				_internalChange = true;
				comboBoxChannels.BeginUpdate();
				comboBoxChannels.Items.RemoveAt(index);
				comboBoxChannels.Items.Insert(index, _currentChannel);
				//TODO Why do we check before assigning?
				if (selectedIndex != comboBoxChannels.SelectedIndex)
				{
					comboBoxChannels.SelectedIndex = selectedIndex;
				}
				comboBoxChannels.EndUpdate();
				_internalChange = false;
			}
			_currentChannel.Color = buttonColor.BackColor;
			_currentChannel.Enabled = checkBoxEnabled.Checked;
		}
	}
}