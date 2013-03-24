namespace TriggerResponse {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus;

	internal partial class TriggerResponseTestDialog : Form {
		private MappedTriggerResponse[] m_responsesRegistered;

		public TriggerResponseTestDialog(MappedTriggerResponse[] responses, ITrigger triggerInterface) {
			this.InitializeComponent();
			this.m_triggerInterface = triggerInterface;
			List<string> list = new List<string>();
			foreach (MappedTriggerResponse response in responses) {
				if (!list.Contains(response.Key)) {
					this.comboBoxTriggerResponses.Items.Add(response);
					list.Add(response.Key);
				}
				response.EcHandle = this.m_triggerInterface.RegisterResponse(response.TriggerType, response.TriggerIndex, response.SequenceFile);
			}
			this.m_responsesRegistered = responses;
		}

		private void buttonTest_Click(object sender, EventArgs e) {
			MappedTriggerResponse selectedItem = (MappedTriggerResponse)this.comboBoxTriggerResponses.SelectedItem;
			this.m_triggerInterface.ActivateTrigger(selectedItem.TriggerType, selectedItem.TriggerIndex);
		}

		private void comboBoxResponses_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonTest.Enabled = this.comboBoxTriggerResponses.SelectedIndex != -1;
		}

		private void ResponseTestDialog_FormClosing(object sender, FormClosingEventArgs e) {
			foreach (MappedTriggerResponse response in this.m_responsesRegistered) {
				this.m_triggerInterface.UnregisterResponse(response.TriggerType, response.TriggerIndex, response.EcHandle);
				response.EcHandle = 0;
			}
		}

		private void ResponseTestDialog_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
		}
	}
}