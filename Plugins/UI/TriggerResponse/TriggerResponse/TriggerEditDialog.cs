namespace TriggerResponse {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Vixen;

	internal class TriggerEditDialog : Form {
		private MappedTriggerResponse m_item;
		private string m_sequenceFile;

		public TriggerEditDialog(List<ITriggerPlugin> triggers, string[] audioDevices) {
			this.InitializeComponent();
			this.comboBoxTriggers.Items.AddRange(triggers.ToArray());
			this.openFileDialog.InitialDirectory = Paths.SequencePath;
			this.openFileDialog.Filter = ((ISystem)Interfaces.Available["ISystem"]).KnownFileTypesFilter;
			this.comboBoxAudioDevice.Items.AddRange(audioDevices);
			if (this.comboBoxAudioDevice.Items.Count > 0) {
				this.comboBoxAudioDevice.SelectedIndex = 0;
			}
		}

		private void buttonFindSequence_Click(object sender, EventArgs e) {
			this.openFileDialog.FileName = string.Empty;
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				this.textBoxSequence.Text = Path.GetFileNameWithoutExtension(this.openFileDialog.FileName);
				this.m_sequenceFile = Path.GetFileName(this.openFileDialog.FileName);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if ((((this.comboBoxTriggers.SelectedIndex == -1) || (this.comboBoxLines.SelectedIndex == -1)) || (this.textBoxDescription.Text.Length == 0)) || (this.textBoxSequence.Text.Length == 0)) {
				MessageBox.Show("All items are required.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = System.Windows.Forms.DialogResult.None;
			}
			else {
				this.m_item.TriggerType = ((ITriggerPlugin)this.comboBoxTriggers.SelectedItem).Name;
				this.m_item.TriggerIndex = (int)this.comboBoxLines.SelectedItem;
				this.m_item.Description = this.textBoxDescription.Text;
				this.m_item.SequenceFile = this.m_sequenceFile;
				if (this.comboBoxAudioDevice.SelectedIndex != this.m_item.Sequence.AudioDeviceIndex) {
					this.m_item.Sequence.AudioDeviceIndex = this.comboBoxAudioDevice.SelectedIndex;
					this.m_item.Sequence.Save();
				}
			}
		}

		private void comboBoxLines_DropDown(object sender, EventArgs e) {
			if (this.comboBoxTriggers.SelectedItem == null) {
				this.comboBoxLines.Enabled = false;
			}
			else {
				this.QueryTriggerLines();
			}
		}

		private void QueryTriggerLines() {
			this.comboBoxLines.BeginUpdate();
			this.comboBoxLines.Items.Clear();
			int triggerCount = ((ITriggerPlugin)this.comboBoxTriggers.SelectedItem).TriggerCount;
			for (int i = 0; i < triggerCount; i++) {
				this.comboBoxLines.Items.Add(i);
			}
			if (this.m_item != null) {
				this.comboBoxLines.SelectedItem = this.m_item.TriggerIndex;
			}
			this.comboBoxLines.EndUpdate();
			this.comboBoxLines.Enabled = true;
		}

		public MappedTriggerResponse SelectedItem {
			get {
				return this.m_item;
			}
			set {
				this.m_item = value;
				foreach (ITriggerPlugin plugin in this.comboBoxTriggers.Items) {
					if (plugin.InterfaceTypeName == this.m_item.TriggerType) {
						this.comboBoxTriggers.SelectedItem = plugin;
						break;
					}
				}
				if (this.comboBoxTriggers.SelectedIndex != -1) {
					this.QueryTriggerLines();
					this.textBoxDescription.Text = this.m_item.Description;
				}
				this.m_sequenceFile = this.m_item.SequenceFile;
				if (this.m_sequenceFile != string.Empty) {
					this.textBoxSequence.Text = Path.GetFileNameWithoutExtension(this.m_sequenceFile);
					if (this.m_item.Sequence.AudioDeviceIndex < this.comboBoxAudioDevice.Items.Count) {
						this.comboBoxAudioDevice.SelectedIndex = this.m_item.Sequence.AudioDeviceIndex;
					}
				}
			}
		}
	}
}