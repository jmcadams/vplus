using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class CurveFileImportExportDialog : Form
	{
		public enum ImportExport
		{
			Import,
			Export
		}

		private readonly ImportExport _importExport;
		private string _fileName = "";
		private CurveLibraryRecord _selectedCurve;


		public CurveFileImportExportDialog(ImportExport importExport)
		{
			InitializeComponent();
			_importExport = importExport;
			openFileDialog.InitialDirectory = Paths.ImportExportPath;
			openFileDialog.DefaultExt = Path.GetExtension("library.xml");
			openFileDialog.Filter = string.Format("{0} file|*{0}", Path.GetExtension("library.xml"));
		}

		private string FilePath
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public CurveLibraryRecord SelectedCurve
		{
			get { return _selectedCurve; }
		}

		private void buttonFile_Click(object sender, EventArgs e)
		{
			if (_importExport == ImportExport.Import)
			{
				openFileDialog.CheckFileExists = true;
				openFileDialog.Title = "Select a file to import from";
			}
			else
			{
				openFileDialog.CheckFileExists = false;
				openFileDialog.Title = "Specify a file to export to";
			}
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				labelFile.Text = FilePath = openFileDialog.FileName;
				try
				{
					if (_importExport == ImportExport.Import)
					{
						LoadRecords(FilePath, listViewCurvesImport);
						groupBoxImport.Enabled = FilePath != "";
					}
					else
					{
						LoadRecords(null, listViewCurvesExport);
						groupBoxExport.Enabled = FilePath != "";
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (_importExport == ImportExport.Import)
			{
				_selectedCurve = comboBoxSelectedCurve.SelectedItem as CurveLibraryRecord;
			}
			else
			{
				Cursor = Cursors.WaitCursor;
				try
				{
					var list = new List<CurveLibraryRecord>();
					foreach (ListViewItem item in listViewCurvesExport.SelectedItems)
					{
						list.Add(item.Tag as CurveLibraryRecord);
					}
					ExportRecordsToFile(list.ToArray(), FilePath);
					MessageBox.Show("Export completed successfully.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				catch (Exception exception)
				{
					MessageBox.Show("Error occurred while exporting:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Exclamation);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		private void comboBoxSelectedCurve_DrawItem(object sender, DrawItemEventArgs e)
		{
			if ((e.Index >= 0) && (e.Index < comboBoxSelectedCurve.Items.Count))
			{
				var box = sender as ComboBox;
				if (box != null)
				{
					var record = (CurveLibraryRecord) box.Items[e.Index];
					int width = e.Bounds.Height - 2;
					using (var brush = new SolidBrush(Color.FromArgb(record.Color)))
					{
						e.Graphics.FillRectangle(brush, e.Bounds.X + 1, e.Bounds.Y + 1, width, width);
						e.Graphics.DrawRectangle(Pens.Black, (e.Bounds.X + 1), (e.Bounds.Y + 1), (width - 1), (width - 1));
					}
					e.Graphics.DrawString(string.Format("{0}, {1}, {2}", record.Manufacturer, record.LightCount, record.Controller),
					                      comboBoxSelectedCurve.Font, Brushes.Black, ((e.Bounds.X + width) + 5), e.Bounds.Y);
				}
			}
		}

		private void CurveFileImportExportDialog_Load(object sender, EventArgs e)
		{
			groupBoxImport.Visible = _importExport == ImportExport.Import;
			groupBoxExport.Visible = _importExport == ImportExport.Export;
		}


		private void ExportRecordsToFile(IEnumerable<CurveLibraryRecord> records, string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			using (var library = new CurveLibrary(filePath))
			{
				foreach (CurveLibraryRecord record in records)
				{
					library.Import(record);
				}
				library.Save();
			}
		}


		private void listViewCurvesExport_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (listViewCurvesExport.SelectedItems.Count == 1)
			{
				try
				{
					ExportRecordsToFile(new[] {listViewCurvesExport.SelectedItems[0].Tag as CurveLibraryRecord}, FilePath);
					DialogResult = DialogResult.OK;
				}
				catch (Exception exception)
				{
					MessageBox.Show("Error occurred while exporting:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Exclamation);
				}
			}
		}

		private void listViewCurvesImport_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void listViewCurvesImport_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
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

		private void listViewCurvesImport_Leave(object sender, EventArgs e)
		{
			var selectedItem = comboBoxSelectedCurve.SelectedItem as CurveLibraryRecord;
			comboBoxSelectedCurve.BeginUpdate();
			comboBoxSelectedCurve.Items.Clear();
			foreach (ListViewItem item in listViewCurvesImport.SelectedItems)
			{
				if (item != null)
				{
					comboBoxSelectedCurve.Items.Add(new object[] {item.Tag as CurveLibraryRecord}); //This may have been one of the new object[] that was not needed
				}
			}
			if ((selectedItem != null) && comboBoxSelectedCurve.Items.Contains(selectedItem))
			{
				comboBoxSelectedCurve.SelectedItem = selectedItem;
			}
			comboBoxSelectedCurve.EndUpdate();
		}

		private void listViewCurvesImport_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (listViewCurvesImport.SelectedItems.Count == 1)
			{
				var tag = (CurveLibraryRecord) listViewCurvesImport.SelectedItems[0].Tag;
				_selectedCurve = tag;
				DialogResult = DialogResult.OK;
			}
		}

		private void listViewCurvesImport_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void LoadRecords(string fileName, ListView listView)
		{
			CurveLibrary library = null;
			Cursor = Cursors.WaitCursor;
			listView.BeginUpdate();
			listView.Items.Clear();
			try
			{
				Exception exception;
				library = fileName == null ? new CurveLibrary() : new CurveLibrary(fileName);
				try
				{
					library.Load(false);
				}
				catch (Exception exception1)
				{
					exception = exception1;
					throw new Exception("Error occurred while loading the file:\n" + exception.Message);
				}
				int num = 1;
				try
				{
					foreach (CurveLibraryRecord record in library.Read())
					{
						listView.Items.Add(
							new ListViewItem(new[]
								{record.Manufacturer, record.LightCount, record.Color.ToString(CultureInfo.InvariantCulture), record.Controller}))
						        .Tag
							= record;
						num++;
					}
				}
				catch (Exception exception2)
				{
					exception = exception2;
					throw new Exception(string.Format("Error in file at line {0}:\n{1}", num, exception.Message));
				}
			}
			finally
			{
				listView.EndUpdate();
				Cursor = Cursors.Default;
				if (library != null)
				{
					library.Dispose();
				}
			}
		}
	}
}