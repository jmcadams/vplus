namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class CurveConflictResolutionDialog : Form
    {
        private CurveLibraryRecord[] m_selectedRecords = null;

        public CurveConflictResolutionDialog(CurveLibraryRecord[] records)
        {
            this.InitializeComponent();
            foreach (CurveLibraryRecord record in records)
            {
                ListViewItem item;
                this.listView.Items.Add(item = new ListViewItem(new string[] { "", record.Manufacturer, record.LightCount, record.Controller }));
                item.Tag = record;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.m_selectedRecords = new CurveLibraryRecord[this.listView.CheckedItems.Count];
            for (int i = 0; i < this.m_selectedRecords.Length; i++)
            {
                this.m_selectedRecords[i] = (CurveLibraryRecord) this.listView.CheckedItems[i].Tag;
            }
        }

        

        

        public CurveLibraryRecord[] SelectedRecords
        {
            get
            {
                return this.m_selectedRecords;
            }
        }
    }
}

