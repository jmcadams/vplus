using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus
{
    internal partial class EventAverageDialog : Form
    {
        private readonly IContainer components = null;

        public EventAverageDialog()
        {
            InitializeComponent();
            lvData.Columns[0].Width = 146;
            lvData.Columns[1].Width = 87;
            lvData.Columns[2].Width = 184;
            string debugValue = string.Empty;
            for (int i = 0; debugValue != null; i++)
            {
                debugValue = Host.GetDebugValue("event_average_" + i.ToString(CultureInfo.InvariantCulture));
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