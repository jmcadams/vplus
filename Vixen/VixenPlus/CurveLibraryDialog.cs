using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus
{
    internal partial class CurveLibraryDialog : Form
    {
        private readonly CurveLibrary _curveLibrary;
        private byte[] _curve;
        private bool _isInternal;

        public CurveLibraryDialog()
        {
            InitializeComponent();
            listViewRecords.Columns[0].Name = "Manufacturer";
            listViewRecords.Columns[1].Name = "LightCount";
            listViewRecords.Columns[2].Name = "Color";
            listViewRecords.Columns[3].Name = "Controller";
            _curveLibrary = new CurveLibrary();
            _isInternal = true;
            comboBoxManufacturer.SelectedIndex = 0;
            comboBoxCount.SelectedIndex = 0;
            comboBoxColor.SelectedIndex = 0;
            comboBoxController.SelectedIndex = 0;
            _isInternal = false;
            comboBoxSource.SelectedIndex = 0;
            listViewRecords.ListViewItemSorter = new ListViewItemSorter();
            ListViewSortIcons.SetSortIcon(listViewRecords, 0, /* this.listViewRecords.Columns[0].Tag = */
                                          System.Windows.Forms.SortOrder.Ascending);
        }

        public byte[] SelectedCurve
        {
            get { return _curve; }
        }

        private void buttonChangeRemoteLocation_Click(object sender, EventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listViewRecords.SelectedItems.Count > 0)
            {
                SetSelectedCurve((CurveLibraryRecord) listViewRecords.Items[listViewRecords.SelectedIndices[0]].Tag);
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
            var box = (ComboBox) sender;
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
                var boxItems = box.Items[e.Index] as string;
                if (boxItems != null)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(int.Parse(boxItems.Substring(1)))))
                    {
                        e.Graphics.FillRectangle(brush, bounds);
                        e.Graphics.DrawRectangle(Pens.Black, bounds);
                    }
                }
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void CurveLibraryDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _curveLibrary.Dispose();
        }


        private void listViewRecords_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if ((listViewRecords.Columns[e.Column].Tag == null) ||
                (((System.Windows.Forms.SortOrder) listViewRecords.Columns[e.Column].Tag) ==
                 System.Windows.Forms.SortOrder.Descending))
            {
                ListViewSortIcons.SetSortIcon(listViewRecords, e.Column, System.Windows.Forms.SortOrder.Ascending);
                listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Ascending;
                _curveLibrary.SortOrder = new CurveLibrary.Sort(listViewRecords.Columns[e.Column].Name,
                                                                CurveLibrary.Sort.Direction.Asc);
                LoadRecords();
            }
            else
            {
                ListViewSortIcons.SetSortIcon(listViewRecords, e.Column, System.Windows.Forms.SortOrder.Descending);
                listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Descending;
                _curveLibrary.SortOrder = new CurveLibrary.Sort(listViewRecords.Columns[e.Column].Name,
                                                                CurveLibrary.Sort.Direction.Desc);
                LoadRecords();
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
                var curveLibraryRecord = e.Item.Tag as CurveLibraryRecord;
                if (curveLibraryRecord != null)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(curveLibraryRecord.Color)))
                    {
                        e.Graphics.FillRectangle(brush, bounds);
                        e.Graphics.DrawRectangle(Pens.Black, bounds);
                    }
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listViewRecords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewRecords.SelectedItems.Count > 0)
            {
                SetSelectedCurve((CurveLibraryRecord) listViewRecords.Items[listViewRecords.SelectedIndices[0]].Tag);
            }
            DialogResult = DialogResult.OK;
        }

        private void listViewRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOkay.Enabled = listViewRecords.SelectedItems.Count > 0;
        }

        private void LoadRecords()
        {
            if (!_isInternal)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    _curveLibrary.ManufacturerFilter = (comboBoxManufacturer.SelectedIndex == 0)
                                                           ? null
                                                           : new[]
                                                               {
                                                                   new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
                                                                                           comboBoxManufacturer.SelectedItem.ToString())
                                                               };
                    _curveLibrary.LightCountFilter = (comboBoxCount.SelectedIndex == 0)
                                                         ? null
                                                         : new[]
                                                             {
                                                                 new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
                                                                                         comboBoxCount.SelectedItem.ToString())
                                                             };
                    _curveLibrary.ColorFilter = (comboBoxColor.SelectedIndex == 0)
                                                    ? null
                                                    : new[]
                                                        {
                                                            new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
                                                                                    ColorFromString(comboBoxColor.SelectedItem.ToString()))
                                                        };
                    _curveLibrary.ControllerFilter = (comboBoxController.SelectedIndex == 0)
                                                         ? null
                                                         : new[]
                                                             {
                                                                 new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
                                                                                         comboBoxController.SelectedItem.ToString())
                                                             };
                    listViewRecords.BeginUpdate();
                    listViewRecords.Items.Clear();
                    btnOkay.Enabled = false;
                    try
                    {
                        foreach (CurveLibraryRecord record in _curveLibrary.Read())
                        {
                            listViewRecords.Items.Add(
                                new ListViewItem(new[]
                                    {
                                        record.Manufacturer, record.LightCount, record.Color.ToString(CultureInfo.InvariantCulture), record.Controller
                                    }))
                                           .Tag = record;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        listViewRecords.EndUpdate();
                    }
                    if (!_curveLibrary.IsFiltered)
                    {
                        PopulateFilter(comboBoxManufacturer, _curveLibrary.GetAllManufacturers());
                        PopulateFilter(comboBoxCount, _curveLibrary.GetAllLightCounts());
                        PopulateFilter(comboBoxColor, _curveLibrary.GetAllLightColors());
                        PopulateFilter(comboBoxController, _curveLibrary.GetAllControllers());
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void PopulateFilter(ComboBox comboBox, IEnumerable<string> items)
        {
            int selectedIndex = comboBox.SelectedIndex;
            comboBox.Items.Clear();
            comboBox.Items.Add("(All)");
            foreach (string str2 in items)
            {
                string str = comboBox != comboBoxColor ? str2 : StringFromColor(str2);
                if (!comboBox.Items.Contains(str))
                {
                    comboBox.Items.Add(str);
                }
            }
            _isInternal = true;
            comboBox.SelectedIndex = selectedIndex;
            _isInternal = false;
        }

        private void SetSelectedCurve(CurveLibraryRecord clr)
        {
            if (comboBoxSource.SelectedIndex == 1)
            {
                _curveLibrary.Import(clr);
            }
            _curve = clr.CurveData;
        }

        private string StringFromColor(string colorString)
        {
            return ("C" + colorString);
        }
    }
}