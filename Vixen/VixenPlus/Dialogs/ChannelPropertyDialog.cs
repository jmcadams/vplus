namespace Vixen.Dialogs {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	public partial class ChannelPropertyDialog : Form {
		private Channel m_currentChannel;
		private bool m_internalChange = false;
		private Preference2 m_preferences;

		public ChannelPropertyDialog(List<Channel> channels, Channel currentChannel, bool showOutputChannel) {
			this.InitializeComponent();
			this.m_internalChange = true;
			this.comboBoxChannels.Items.AddRange(channels.ToArray());
			this.m_internalChange = false;
			this.label3.Visible = this.labelOutputChannel.Visible = showOutputChannel;
			this.GotoChannel(currentChannel);
			this.m_preferences = ((ISystem)Interfaces.Available["ISystem"]).UserPreferences;
			string[] strArray = this.m_preferences.GetString("CustomColors").Split(new char[] { ',' });
			int[] numArray = new int[strArray.Length];
			for (int i = 0; i < strArray.Length; i++) {
				numArray[i] = int.Parse(strArray[i]);
			}
			this.colorDialog.CustomColors = numArray;
		}

		private void buttonClose_Click(object sender, EventArgs e) {
			this.ToChannel();
		}

		private void buttonColor_Click(object sender, EventArgs e) {
			this.colorDialog.Color = this.m_currentChannel.Color;
			if (this.colorDialog.ShowDialog() == DialogResult.OK) {
				this.buttonColor.BackColor = this.colorDialog.Color;
				string[] strArray = new string[this.colorDialog.CustomColors.Length];
				for (int i = 0; i < strArray.Length; i++) {
					strArray[i] = this.colorDialog.CustomColors[i].ToString();
				}
				this.m_preferences.SetString("CustomColors", string.Join(",", strArray));
			}
		}

		private void buttonDimmingCurve_Click(object sender, EventArgs e) {
			DimmingCurveDialog dialog = new DimmingCurveDialog(null, this.m_currentChannel);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonNext_Click(object sender, EventArgs e) {
			this.NextChannel();
		}

		private void buttonPrev_Click(object sender, EventArgs e) {
			this.ToChannel();
			this.GotoChannel((Channel)this.comboBoxChannels.Items[this.comboBoxChannels.Items.IndexOf(this.m_currentChannel) - 1]);
		}

		private void ChannelPropertyDialog_KeyPress(object sender, KeyPressEventArgs e) {
			if ((e.KeyChar == '\r') && this.buttonNext.Enabled) {
				this.NextChannel();
				if (base.ActiveControl == this.textBoxName) {
					this.textBoxName.SelectAll();
				}
				e.Handled = true;
			}
		}

		private void ChannelPropertyDialog_Load(object sender, EventArgs e) {
			base.ActiveControl = this.buttonNext;
		}

		private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
			if (!this.m_internalChange) {
				this.ToChannel();
				this.GotoChannel((Channel)this.comboBoxChannels.SelectedItem);
			}
		}
		
		private void FromChannel() {
			this.textBoxName.Text = this.m_currentChannel.Name;
			this.buttonColor.BackColor = this.m_currentChannel.Color;
			this.labelOutputChannel.Text = (this.m_currentChannel.OutputChannel + 1).ToString();
			this.checkBoxEnabled.Checked = this.m_currentChannel.Enabled;
		}

		private void GotoChannel(Channel channel) {
			this.m_internalChange = true;
			this.m_currentChannel = channel;
			this.comboBoxChannels.SelectedItem = this.m_currentChannel;
			this.buttonPrev.Enabled = this.comboBoxChannels.Items.IndexOf(this.m_currentChannel) > 0;
			this.buttonNext.Enabled = this.comboBoxChannels.Items.IndexOf(this.m_currentChannel) < (this.comboBoxChannels.Items.Count - 1);
			this.m_internalChange = false;
			this.FromChannel();
		}
		
		private void NextChannel() {
			this.ToChannel();
			this.GotoChannel((Channel)this.comboBoxChannels.Items[this.comboBoxChannels.Items.IndexOf(this.m_currentChannel) + 1]);
		}

		private void ToChannel() {
			string name = this.m_currentChannel.Name;
			this.m_currentChannel.Name = this.textBoxName.Text;
			if (name != this.textBoxName.Text) {
				int selectedIndex = this.comboBoxChannels.SelectedIndex;
				int index = this.comboBoxChannels.Items.IndexOf(this.m_currentChannel);
				this.m_internalChange = true;
				this.comboBoxChannels.BeginUpdate();
				this.comboBoxChannels.Items.RemoveAt(index);
				this.comboBoxChannels.Items.Insert(index, this.m_currentChannel);
				if (selectedIndex != this.comboBoxChannels.SelectedIndex) {
					this.comboBoxChannels.SelectedIndex = selectedIndex;
				}
				this.comboBoxChannels.EndUpdate();
				this.m_internalChange = false;
			}
			this.m_currentChannel.Color = this.buttonColor.BackColor;
			this.m_currentChannel.Enabled = this.checkBoxEnabled.Checked;
		}
	}
}

