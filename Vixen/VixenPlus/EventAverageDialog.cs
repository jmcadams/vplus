using System.ComponentModel;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class EventAverageDialog : Form
	{
		private readonly IContainer components = null;

		public EventAverageDialog()
		{
			InitializeComponent();
			lvData.Columns[0].Width = 0x92;
			lvData.Columns[1].Width = 0x57;
			lvData.Columns[2].Width = 0xb8;
			string debugValue = string.Empty;
			for (int i = 0; debugValue != null; i++)
			{
				debugValue = Host.GetDebugValue("event_average_" + i.ToString());
				if (debugValue != null)
				{
					string[] strArray = debugValue.Split(new[] {'|'});
					var item = new ListViewItem(new[] {strArray[0], string.Format("{0} - {1}", strArray[1], strArray[2]), strArray[3]});
					lvData.Items.Add(item);
				}
			}
		}
	}
}