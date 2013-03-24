using System;
using System.Windows.Forms;
using FMOD;

namespace VixenPlus
{
	internal partial class SoundDeviceDialog : Form
	{
		private readonly Preference2 m_preferences;
		private bool m_internal;
		private int m_lastSelection = -1;

		public SoundDeviceDialog(Preference2 preferences)
		{
			InitializeComponent();
			m_preferences = preferences;
		}

		private void buttonSet_Click(object sender, EventArgs e)
		{
			m_preferences.SetInteger("SoundDevice", comboBoxDevice.SelectedIndex, 0);
			m_preferences.Flush();
			buttonSet.Enabled = false;
			MessageBox.Show("Please restart the application for this change to take effect", Vendor.ProductName,
			                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_internal)
			{
				buttonSet.Enabled = m_lastSelection != comboBoxDevice.SelectedIndex;
				m_lastSelection = comboBoxDevice.SelectedIndex;
			}
		}


		private void SoundDeviceDialog_Load(object sender, EventArgs e)
		{
			comboBoxDevice.Items.AddRange(fmod.GetSoundDeviceList());
			int integer = m_preferences.GetInteger("SoundDevice");
			if (integer < comboBoxDevice.Items.Count)
			{
				m_internal = true;
				comboBoxDevice.SelectedIndex = integer;
				m_internal = false;
			}
		}
	}
}