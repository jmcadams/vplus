using System;
using System.Windows.Forms;



using FMOD;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    internal partial class SoundDeviceDialog : Form {
        private readonly Preference2 _preferences;
        private bool _internal;
        private int _lastSelection = -1;


        public SoundDeviceDialog(Preference2 preferences) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            _preferences = preferences;
        }


        private void buttonSet_Click(object sender, EventArgs e) {
            _preferences.SetInteger("SoundDevice", comboBoxDevice.SelectedIndex, 0);
            _preferences.SaveSettings();
            buttonSet.Enabled = false;
            MessageBox.Show(Resources.RestartForChange, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e) {
            if (_internal) {
                return;
            }
            buttonSet.Enabled = _lastSelection != comboBoxDevice.SelectedIndex;
            _lastSelection = comboBoxDevice.SelectedIndex;
        }


        private void SoundDeviceDialog_Load(object sender, EventArgs e) {
            comboBoxDevice.Items.AddRange(fmod.GetSoundDeviceList());
            var integer = _preferences.GetInteger("SoundDevice");
            if (integer >= comboBoxDevice.Items.Count) {
                return;
            }
            _internal = true;
            comboBoxDevice.SelectedIndex = integer;
            _internal = false;
        }
    }
}
