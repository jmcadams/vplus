using System;
using System.Windows.Forms;
using FMOD;

namespace VixenPlus
{
	internal partial class SoundDeviceDialog : Form
	{
		private readonly Preference2 _preferences;
		private bool _internal;
		private int _lastSelection = -1;

		public SoundDeviceDialog(Preference2 preferences)
		{
			InitializeComponent();
			_preferences = preferences;
		}

		private void buttonSet_Click(object sender, EventArgs e)
		{
			_preferences.SetInteger("SoundDevice", comboBoxDevice.SelectedIndex, 0);
			_preferences.Flush();
			buttonSet.Enabled = false;
			MessageBox.Show("Please restart the application for this change to take effect", Vendor.ProductName,
			                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_internal)
			{
				buttonSet.Enabled = _lastSelection != comboBoxDevice.SelectedIndex;
				_lastSelection = comboBoxDevice.SelectedIndex;
			}
		}


		private void SoundDeviceDialog_Load(object sender, EventArgs e)
		{
			comboBoxDevice.Items.AddRange(new object[] {fmod.GetSoundDeviceList()});
			int integer = _preferences.GetInteger("SoundDevice");
			if (integer < comboBoxDevice.Items.Count)
			{
				_internal = true;
				comboBoxDevice.SelectedIndex = integer;
				_internal = false;
			}
		}
	}
}