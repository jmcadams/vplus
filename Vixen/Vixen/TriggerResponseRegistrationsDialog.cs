namespace Vixen {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class TriggerResponseRegistrationsDialog : Form {

		private IExecution m_executionInterface;
		private Dictionary<string, List<RegisteredResponse>> m_registrations;
		private ITrigger m_triggerInterface;

		public TriggerResponseRegistrationsDialog(Dictionary<string, List<RegisteredResponse>> responseRegistrations) {
			this.InitializeComponent();
			this.m_registrations = responseRegistrations;
			this.m_executionInterface = (IExecution)Interfaces.Available["IExecution"];
			this.m_triggerInterface = (ITrigger)Interfaces.Available["ITrigger"];
		}

		private void buttonRefresh_Click(object sender, EventArgs e) {
			this.GetRegistrations();
		}

		private void buttonRemove_Click(object sender, EventArgs e) {
			List<ListViewItem> list = new List<ListViewItem>();
			this.listViewResponses.BeginUpdate();
			foreach (ListViewItem item in this.listViewResponses.SelectedItems) {
				RegisteredResponse tag = (RegisteredResponse)item.Tag;
				this.m_triggerInterface.UnregisterResponse(tag.InterfaceTypeName, tag.Line, tag.EcHandle);
				list.Add(item);
			}
			foreach (ListViewItem item in list) {
				this.listViewResponses.Items.Remove(item);
			}
			this.listViewResponses.EndUpdate();
		}



		private void GetRegistrations() {
			this.listViewResponses.BeginUpdate();
			this.listViewResponses.Items.Clear();
			this.buttonRemove.Enabled = false;
			foreach (List<RegisteredResponse> list in this.m_registrations.Values) {
				foreach (RegisteredResponse response in list) {
					IExecutable objectInContext = this.m_executionInterface.GetObjectInContext(response.EcHandle);
					ListViewItem item = new ListViewItem(new string[] { response.InterfaceTypeName, response.Line.ToString(), (objectInContext == null) ? "(none)" : objectInContext.Name });
					item.Tag = response;
					this.listViewResponses.Items.Add(item);
				}
			}
			this.listViewResponses.EndUpdate();
		}



		private void listViewResponses_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonRemove.Enabled = this.listViewResponses.SelectedItems.Count > 0;
		}

		private void TriggerResponseRegistrationsDialog_Load(object sender, EventArgs e) {
			this.GetRegistrations();
		}
	}
}

