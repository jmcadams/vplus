namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class CurveLibraryDialog : Form
    {
        private const string COLOR_PREPEND = "C";
        private byte[] m_curve = null;
        private bool m_internal = false;
        private CurveLibrary m_localLibrary;
        private ListViewItemSorter m_sorter;

        public CurveLibraryDialog()
        {
            this.InitializeComponent();
            this.listViewRecords.Columns[0].Name = "Manufacturer";
            this.listViewRecords.Columns[1].Name = "LightCount";
            this.listViewRecords.Columns[2].Name = "Color";
            this.listViewRecords.Columns[3].Name = "Controller";
            this.m_localLibrary = new CurveLibrary();
            this.m_internal = true;
            this.comboBoxManufacturer.SelectedIndex = 0;
            this.comboBoxCount.SelectedIndex = 0;
            this.comboBoxColor.SelectedIndex = 0;
            this.comboBoxController.SelectedIndex = 0;
            this.m_internal = false;
            this.comboBoxSource.SelectedIndex = 0;
            this.listViewRecords.ListViewItemSorter = this.m_sorter = new ListViewItemSorter();
            ListViewSortIcons.SetSortIcon(this.listViewRecords, 0, /* this.listViewRecords.Columns[0].Tag = */ System.Windows.Forms.SortOrder.Ascending);
        }

        private void buttonChangeRemoteLocation_Click(object sender, EventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.listViewRecords.SelectedItems.Count > 0)
            {
                this.SetSelectedCurve((CurveLibraryRecord) this.listViewRecords.Items[this.listViewRecords.SelectedIndices[0]].Tag);
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
        }

        private string ColorFromString(string mangledString)
        {
            return mangledString.Substring("C".Length);
        }

        private void comboBoxColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox box = (ComboBox) sender;
            if ((e.State & DrawItemState.Selected) != DrawItemState.None)
            {
                e.DrawBackground();
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
            if ((e.State & DrawItemState.Focus) != DrawItemState.None)
            {
                e.DrawFocusRectangle();
            }
            if (e.Index == 0)
            {
                e.Graphics.DrawString(box.Items[e.Index].ToString(), box.Font, Brushes.Black, e.Bounds);
            }
            else if ((e.Index >= 0) && (e.Index < box.Items.Count))
            {
                Rectangle bounds = e.Bounds;
                bounds.Inflate(-16, -2);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(int.Parse((box.Items[e.Index] as string).Substring(1)))))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                    e.Graphics.DrawRectangle(Pens.Black, bounds);
                }
            }
        }

        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRecords();
        }

        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRecords();
        }

        private void CurveLibraryDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_localLibrary.Dispose();
        }

        

        

        private void listViewRecords_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if ((this.listViewRecords.Columns[e.Column].Tag == null) || (((System.Windows.Forms.SortOrder) this.listViewRecords.Columns[e.Column].Tag) == System.Windows.Forms.SortOrder.Descending))
            {
                ListViewSortIcons.SetSortIcon(this.listViewRecords, e.Column, System.Windows.Forms.SortOrder.Ascending);
                this.listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Ascending;
                this.m_localLibrary.SortOrder = new CurveLibrary.Sort(this.listViewRecords.Columns[e.Column].Name, CurveLibrary.Sort.Direction.Asc);
                this.LoadRecords();
            }
            else
            {
                ListViewSortIcons.SetSortIcon(this.listViewRecords, e.Column, System.Windows.Forms.SortOrder.Descending);
                this.listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Descending;
                this.m_localLibrary.SortOrder = new CurveLibrary.Sort(this.listViewRecords.Columns[e.Column].Name, CurveLibrary.Sort.Direction.Desc);
                this.LoadRecords();
            }
        }

        private void listViewRecords_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewRecords_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                Rectangle bounds = e.Bounds;
                bounds.Inflate(-16, -2);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((e.Item.Tag as CurveLibraryRecord).Color)))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                    e.Graphics.DrawRectangle(Pens.Black, bounds);
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listViewRecords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listViewRecords.SelectedItems.Count > 0)
            {
                this.SetSelectedCurve((CurveLibraryRecord) this.listViewRecords.Items[this.listViewRecords.SelectedIndices[0]].Tag);
            }
            base.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void listViewRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnOkay.Enabled = this.listViewRecords.SelectedItems.Count > 0;
        }

        private void LoadRecords()
        {
            if (!this.m_internal)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.m_localLibrary.ManufacturerFilter = (this.comboBoxManufacturer.SelectedIndex == 0) ? null : new CurveLibrary.Filter[] { new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals, this.comboBoxManufacturer.SelectedItem.ToString()) };
                    this.m_localLibrary.LightCountFilter = (this.comboBoxCount.SelectedIndex == 0) ? null : new CurveLibrary.Filter[] { new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals, this.comboBoxCount.SelectedItem.ToString()) };
                    this.m_localLibrary.ColorFilter = (this.comboBoxColor.SelectedIndex == 0) ? null : new CurveLibrary.Filter[] { new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals, this.ColorFromString(this.comboBoxColor.SelectedItem.ToString())) };
                    this.m_localLibrary.ControllerFilter = (this.comboBoxController.SelectedIndex == 0) ? null : new CurveLibrary.Filter[] { new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals, this.comboBoxController.SelectedItem.ToString()) };
                    this.listViewRecords.BeginUpdate();
                    this.listViewRecords.Items.Clear();
                    this.btnOkay.Enabled = false;
                    try
                    {
                        foreach (CurveLibraryRecord record in this.m_localLibrary.Read())
                        {
                            this.listViewRecords.Items.Add(new ListViewItem(new string[] { record.Manufacturer, record.LightCount, record.Color.ToString(), record.Controller })).Tag = record;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        this.listViewRecords.EndUpdate();
                    }
                    if (!this.m_localLibrary.IsFiltered)
                    {
                        this.PopulateFilter(this.comboBoxManufacturer, this.m_localLibrary.GetAllManufacturers());
                        this.PopulateFilter(this.comboBoxCount, this.m_localLibrary.GetAllLightCounts());
                        this.PopulateFilter(this.comboBoxColor, this.m_localLibrary.GetAllLightColors());
                        this.PopulateFilter(this.comboBoxController, this.m_localLibrary.GetAllControllers());
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void m_library_Message(string message)
        {
            this.labelDownloadMessage.Text = message;
            this.labelDownloadMessage.Refresh();
        }

        private void PopulateFilter(ComboBox comboBox, string[] items)
        {
            int selectedIndex = comboBox.SelectedIndex;
            comboBox.Items.Clear();
            comboBox.Items.Add("(All)");
            foreach (string str2 in items)
            {
                string str;
                if (comboBox != this.comboBoxColor)
                {
                    str = str2;
                }
                else
                {
                    str = this.StringFromColor(str2);
                }
                if (!comboBox.Items.Contains(str))
                {
                    comboBox.Items.Add(str);
                }
            }
            this.m_internal = true;
            comboBox.SelectedIndex = selectedIndex;
            this.m_internal = false;
        }

        private void SetSelectedCurve(CurveLibraryRecord clr)
        {
            if (this.comboBoxSource.SelectedIndex == 1)
            {
                this.m_localLibrary.Import(clr);
            }
            this.m_curve = clr.CurveData;
        }

        private string StringFromColor(string colorString)
        {
            return ("C" + colorString);
        }

        public byte[] SelectedCurve
        {
            get
            {
                return this.m_curve;
            }
        }
    }
}

