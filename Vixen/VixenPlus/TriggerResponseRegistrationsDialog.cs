using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class TriggerResponseRegistrationsDialog : Form
	{
		private readonly IExecution m_executionInterface;
		private readonly Dictionary<string, List<RegisteredResponse>> m_registrations;
		private readonly ITrigger m_triggerInterface;

		public TriggerResponseRegistrationsDialog(Dictionary<string, List<RegisteredResponse>> responseRegistrations)
		{
			InitializeComponent();
			m_registrations = responseRegistrations;
			m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			m_triggerInterface = (ITrigger) Interfaces.Available["ITrigger"];
		}

		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			GetRegistrations();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			var list = new List<ListViewItem>();
			listViewResponses.BeginUpdate();
			foreach (ListViewItem item in listViewResponses.SelectedItems)
			{
				var tag = (RegisteredResponse) item.Tag;
				m_triggerInterface.UnregisterResponse(tag.InterfaceTypeName, tag.Line, tag.EcHandle);
				list.Add(item);
			}
			foreach (ListViewItem item in list)
			{
				listViewResponses.Items.Remove(item);
			}
			listViewResponses.EndUpdate();
		}


		private void GetRegistrations()
		{
			listViewResponses.BeginUpdate();
			listViewResponses.Items.Clear();
			buttonRemove.Enabled = false;
			foreach (var list in m_registrations.Values)
			{
				foreach (RegisteredResponse response in list)
				{
					IExecutable objectInContext = m_executionInterface.GetObjectInContext(response.EcHandle);
					var item =
						new ListViewItem(new[]
							{
								response.InterfaceTypeName, response.Line.ToString(),
								(objectInContext == null) ? "(none)" : objectInContext.Name
							});
					item.Tag = response;
					listViewResponses.Items.Add(item);
				}
			}
			listViewResponses.EndUpdate();
		}


		private void listViewResponses_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonRemove.Enabled = listViewResponses.SelectedItems.Count > 0;
		}

		private void TriggerResponseRegistrationsDialog_Load(object sender, EventArgs e)
		{
			GetRegistrations();
		}
	}
}