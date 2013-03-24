using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class TriggerResponseRegistrationsDialog : Form
	{
		private readonly IExecution _executionInterface;
		private readonly Dictionary<string, List<RegisteredResponse>> _registrations;
		private readonly ITrigger _triggerInterface;

		public TriggerResponseRegistrationsDialog(Dictionary<string, List<RegisteredResponse>> responseRegistrations)
		{
			InitializeComponent();
			_registrations = responseRegistrations;
			_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			_triggerInterface = (ITrigger) Interfaces.Available["ITrigger"];
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
				_triggerInterface.UnregisterResponse(tag.InterfaceTypeName, tag.Line, tag.EcHandle);
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
			foreach (var list in _registrations.Values)
			{
				foreach (RegisteredResponse response in list)
				{
					IExecutable objectInContext = _executionInterface.GetObjectInContext(response.EcHandle);
					var item =
						new ListViewItem(new[]
							{
								response.InterfaceTypeName, response.Line.ToString(CultureInfo.InvariantCulture),
								(objectInContext == null) ? "(none)" : objectInContext.Name
							}) {Tag = response};
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