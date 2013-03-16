namespace Prop2SeqGen {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;

	internal partial class frmAddin : Form {
		private string m_filePath;

		public frmAddin(List<AudioSelection> audioOptions) {
			this.InitializeComponent();
			this.comboBoxAudioDevice.Items.AddRange(audioOptions.ToArray());
			if (this.comboBoxAudioDevice.Items.Count > 0) {
				this.comboBoxAudioDevice.SelectedIndex = 0;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.textBoxName.Text.Length == 0) {
				MessageBox.Show("Please specify a file name.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = DialogResult.None;
			}
			else if (this.comboBoxAudioDevice.SelectedIndex == -1) {
				MessageBox.Show("Please select an audio device.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = DialogResult.None;
			}
			else {
				this.saveFileDialog.FileName = Path.ChangeExtension(this.textBoxName.Text, ".bs2");
				if (this.saveFileDialog.ShowDialog() == DialogResult.OK) {
					this.m_filePath = Path.ChangeExtension(this.saveFileDialog.FileName, ".bs2");
				}
				else {
					base.DialogResult = DialogResult.None;
				}
			}
		}

		public int AudioDeviceIndex {
			get {
				return this.comboBoxAudioDevice.SelectedIndex;
			}
		}

		public string FileName {
			get {
				return this.m_filePath;
			}
		}

		public bool OpenFile {
			get {
				return this.checkBoxOpenFile.Checked;
			}
		}

		public int Threshold {
			get {
				return (int)this.numericUpDownThreshold.Value;
			}
		}

		public bool TriggerLevelHigh {
			get {
				return this.radioButtonActiveHigh.Checked;
			}
		}
	}
}