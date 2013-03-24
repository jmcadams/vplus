using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class ChannelPropertyDialog : Form
	{
		private readonly Preference2 m_preferences;
		private Channel m_currentChannel;
		private bool m_internalChange;

		public ChannelPropertyDialog(List<Channel> channels, Channel currentChannel, bool showOutputChannel)
		{
			InitializeComponent();
			m_internalChange = true;
			comboBoxChannels.Items.AddRange(channels.ToArray());
			m_internalChange = false;
			label3.Visible = labelOutputChannel.Visible = showOutputChannel;
			GotoChannel(currentChannel);
			m_preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
			string[] strArray = m_preferences.GetString("CustomColors").Split(new[] {','});
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
			colorDialog.Color = m_currentChannel.Color;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				buttonColor.BackColor = colorDialog.Color;
				var strArray = new string[colorDialog.CustomColors.Length];
				for (int i = 0; i < strArray.Length; i++)
				{
					strArray[i] = colorDialog.CustomColors[i].ToString();
				}
				m_preferences.SetString("CustomColors", string.Join(",", strArray));
			}
		}

		private void buttonDimmingCurve_Click(object sender, EventArgs e)
		{
			var dialog = new DimmingCurveDialog(null, m_currentChannel);
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
			GotoChannel((Channel) comboBoxChannels.Items[comboBoxChannels.Items.IndexOf(m_currentChannel) - 1]);
		}

		private void ChannelPropertyDialog_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar == '\r') && buttonNext.Enabled)
			{
				NextChannel();
				if (base.ActiveControl == textBoxName)
				{
					textBoxName.SelectAll();
				}
				e.Handled = true;
			}
		}

		private void ChannelPropertyDialog_Load(object sender, EventArgs e)
		{
			base.ActiveControl = buttonNext;
		}

		private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_internalChange)
			{
				ToChannel();
				GotoChannel((Channel) comboBoxChannels.SelectedItem);
			}
		}

		private void FromChannel()
		{
			textBoxName.Text = m_currentChannel.Name;
			buttonColor.BackColor = m_currentChannel.Color;
			labelOutputChannel.Text = (m_currentChannel.OutputChannel + 1).ToString();
			checkBoxEnabled.Checked = m_currentChannel.Enabled;
		}

		private void GotoChannel(Channel channel)
		{
			m_internalChange = true;
			m_currentChannel = channel;
			comboBoxChannels.SelectedItem = m_currentChannel;
			buttonPrev.Enabled = comboBoxChannels.Items.IndexOf(m_currentChannel) > 0;
			buttonNext.Enabled = comboBoxChannels.Items.IndexOf(m_currentChannel) < (comboBoxChannels.Items.Count - 1);
			m_internalChange = false;
			FromChannel();
		}

		private void NextChannel()
		{
			ToChannel();
			GotoChannel((Channel) comboBoxChannels.Items[comboBoxChannels.Items.IndexOf(m_currentChannel) + 1]);
		}

		private void ToChannel()
		{
			string name = m_currentChannel.Name;
			m_currentChannel.Name = textBoxName.Text;
			if (name != textBoxName.Text)
			{
				int selectedIndex = comboBoxChannels.SelectedIndex;
				int index = comboBoxChannels.Items.IndexOf(m_currentChannel);
				m_internalChange = true;
				comboBoxChannels.BeginUpdate();
				comboBoxChannels.Items.RemoveAt(index);
				comboBoxChannels.Items.Insert(index, m_currentChannel);
				if (selectedIndex != comboBoxChannels.SelectedIndex)
				{
					comboBoxChannels.SelectedIndex = selectedIndex;
				}
				comboBoxChannels.EndUpdate();
				m_internalChange = false;
			}
			m_currentChannel.Color = buttonColor.BackColor;
			m_currentChannel.Enabled = checkBoxEnabled.Checked;
		}
	}
}