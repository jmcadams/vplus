using System;
using System.Drawing;
using System.Windows.Forms;
using VixenPlus;

namespace VixenPlus
{
	internal partial class CurveLibraryDialog : Form
	{
		private const string COLOR_PREPEND = "C";
		private readonly CurveLibrary m_localLibrary;
		private byte[] m_curve;
		private bool m_internal;
		private ListViewItemSorter m_sorter;

		public CurveLibraryDialog()
		{
			InitializeComponent();
			listViewRecords.Columns[0].Name = "Manufacturer";
			listViewRecords.Columns[1].Name = "LightCount";
			listViewRecords.Columns[2].Name = "Color";
			listViewRecords.Columns[3].Name = "Controller";
			m_localLibrary = new CurveLibrary();
			m_internal = true;
			comboBoxManufacturer.SelectedIndex = 0;
			comboBoxCount.SelectedIndex = 0;
			comboBoxColor.SelectedIndex = 0;
			comboBoxController.SelectedIndex = 0;
			m_internal = false;
			comboBoxSource.SelectedIndex = 0;
			listViewRecords.ListViewItemSorter = m_sorter = new ListViewItemSorter();
			ListViewSortIcons.SetSortIcon(listViewRecords, 0, /* this.listViewRecords.Columns[0].Tag = */
			                              System.Windows.Forms.SortOrder.Ascending);
		}

		public byte[] SelectedCurve
		{
			get { return m_curve; }
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
				using (var brush = new SolidBrush(Color.FromArgb(int.Parse((box.Items[e.Index] as string).Substring(1)))))
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
			LoadRecords();
		}

		private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadRecords();
		}

		private void CurveLibraryDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_localLibrary.Dispose();
		}


		private void listViewRecords_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if ((listViewRecords.Columns[e.Column].Tag == null) ||
			    (((System.Windows.Forms.SortOrder) listViewRecords.Columns[e.Column].Tag) ==
			     System.Windows.Forms.SortOrder.Descending))
			{
				ListViewSortIcons.SetSortIcon(listViewRecords, e.Column, System.Windows.Forms.SortOrder.Ascending);
				listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Ascending;
				m_localLibrary.SortOrder = new CurveLibrary.Sort(listViewRecords.Columns[e.Column].Name,
				                                                 CurveLibrary.Sort.Direction.Asc);
				LoadRecords();
			}
			else
			{
				ListViewSortIcons.SetSortIcon(listViewRecords, e.Column, System.Windows.Forms.SortOrder.Descending);
				listViewRecords.Columns[e.Column].Tag = System.Windows.Forms.SortOrder.Descending;
				m_localLibrary.SortOrder = new CurveLibrary.Sort(listViewRecords.Columns[e.Column].Name,
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
				using (var brush = new SolidBrush(Color.FromArgb((e.Item.Tag as CurveLibraryRecord).Color)))
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
			if (listViewRecords.SelectedItems.Count > 0)
			{
				SetSelectedCurve((CurveLibraryRecord) listViewRecords.Items[listViewRecords.SelectedIndices[0]].Tag);
			}
			base.DialogResult = DialogResult.OK;
		}

		private void listViewRecords_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnOkay.Enabled = listViewRecords.SelectedItems.Count > 0;
		}

		private void LoadRecords()
		{
			if (!m_internal)
			{
				Cursor = Cursors.WaitCursor;
				try
				{
					m_localLibrary.ManufacturerFilter = (comboBoxManufacturer.SelectedIndex == 0)
						                                    ? null
						                                    : new[]
							                                    {
								                                    new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
								                                                            comboBoxManufacturer.SelectedItem.ToString())
							                                    };
					m_localLibrary.LightCountFilter = (comboBoxCount.SelectedIndex == 0)
						                                  ? null
						                                  : new[]
							                                  {
								                                  new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
								                                                          comboBoxCount.SelectedItem.ToString())
							                                  };
					m_localLibrary.ColorFilter = (comboBoxColor.SelectedIndex == 0)
						                             ? null
						                             : new[]
							                             {
								                             new CurveLibrary.Filter(CurveLibrary.Filter.Operator.Equals,
								                                                     ColorFromString(comboBoxColor.SelectedItem.ToString()))
							                             };
					m_localLibrary.ControllerFilter = (comboBoxController.SelectedIndex == 0)
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
						foreach (CurveLibraryRecord record in m_localLibrary.Read())
						{
							listViewRecords.Items.Add(
								new ListViewItem(new[] {record.Manufacturer, record.LightCount, record.Color.ToString(), record.Controller}))
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
					if (!m_localLibrary.IsFiltered)
					{
						PopulateFilter(comboBoxManufacturer, m_localLibrary.GetAllManufacturers());
						PopulateFilter(comboBoxCount, m_localLibrary.GetAllLightCounts());
						PopulateFilter(comboBoxColor, m_localLibrary.GetAllLightColors());
						PopulateFilter(comboBoxController, m_localLibrary.GetAllControllers());
					}
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		private void m_library_Message(string message)
		{
			labelDownloadMessage.Text = message;
			labelDownloadMessage.Refresh();
		}

		private void PopulateFilter(ComboBox comboBox, string[] items)
		{
			int selectedIndex = comboBox.SelectedIndex;
			comboBox.Items.Clear();
			comboBox.Items.Add("(All)");
			foreach (string str2 in items)
			{
				string str;
				str = comboBox != comboBoxColor ? str2 : StringFromColor(str2);
				if (!comboBox.Items.Contains(str))
				{
					comboBox.Items.Add(str);
				}
			}
			m_internal = true;
			comboBox.SelectedIndex = selectedIndex;
			m_internal = false;
		}

		private void SetSelectedCurve(CurveLibraryRecord clr)
		{
			if (comboBoxSource.SelectedIndex == 1)
			{
				m_localLibrary.Import(clr);
			}
			m_curve = clr.CurveData;
		}

		private string StringFromColor(string colorString)
		{
			return ("C" + colorString);
		}
	}
}