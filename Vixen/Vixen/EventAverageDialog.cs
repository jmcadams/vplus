namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class EventAverageDialog : Form
    {
        private IContainer components = null;

        public EventAverageDialog()
        {
            this.InitializeComponent();
            this.lvData.Columns[0].Width = 0x92;
            this.lvData.Columns[1].Width = 0x57;
            this.lvData.Columns[2].Width = 0xb8;
            string debugValue = string.Empty;
            for (int i = 0; debugValue != null; i++)
            {
                debugValue = Host.GetDebugValue("event_average_" + i.ToString());
                if (debugValue != null)
                {
                    string[] strArray = debugValue.Split(new char[] { '|' });
                    ListViewItem item = new ListViewItem(new string[] { strArray[0], string.Format("{0} - {1}", strArray[1], strArray[2]), strArray[3] });
                    this.lvData.Items.Add(item);
                }
            }
        }
    }
}

