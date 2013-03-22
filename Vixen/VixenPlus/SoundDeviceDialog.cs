namespace Vixen {
	using FMOD;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class SoundDeviceDialog : Form {
		private bool m_internal = false;
		private int m_lastSelection = -1;
		private Preference2 m_preferences;

		public SoundDeviceDialog(Preference2 preferences) {
			this.InitializeComponent();
			this.m_preferences = preferences;
		}

		private void buttonSet_Click(object sender, EventArgs e) {
			this.m_preferences.SetInteger("SoundDevice", this.comboBoxDevice.SelectedIndex, 0);
			this.m_preferences.Flush();
			this.buttonSet.Enabled = false;
			MessageBox.Show("Please restart the application for this change to take effect", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e) {
			if (!this.m_internal) {
				this.buttonSet.Enabled = this.m_lastSelection != this.comboBoxDevice.SelectedIndex;
				this.m_lastSelection = this.comboBoxDevice.SelectedIndex;
			}
		}





		private void SoundDeviceDialog_Load(object sender, EventArgs e) {
			this.comboBoxDevice.Items.AddRange(fmod.GetSoundDeviceList());
			int integer = this.m_preferences.GetInteger("SoundDevice");
			if (integer < this.comboBoxDevice.Items.Count) {
				this.m_internal = true;
				this.comboBoxDevice.SelectedIndex = integer;
				this.m_internal = false;
			}
		}
	}
}

