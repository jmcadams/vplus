namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal partial class CurveFileImportExportDialog : Form
    {
        private string m_fileName = "";
        private ImportExport m_importExport;
        private CurveLibraryRecord m_selectedCurve = null;


        public CurveFileImportExportDialog(ImportExport importExport)
        {
            this.InitializeComponent();
            this.m_importExport = importExport;
            this.openFileDialog.InitialDirectory = Paths.ImportExportPath;
            this.openFileDialog.DefaultExt = Path.GetExtension("library.xml");
            this.openFileDialog.Filter = string.Format("{0} file|*{0}", Path.GetExtension("library.xml"));
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            if (this.m_importExport == ImportExport.Import)
            {
                this.openFileDialog.CheckFileExists = true;
                this.openFileDialog.Title = "Select a file to import from";
            }
            else
            {
                this.openFileDialog.CheckFileExists = false;
                this.openFileDialog.Title = "Specify a file to export to";
            }
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.labelFile.Text = this.FilePath = this.openFileDialog.FileName;
                try
                {
                    if (this.m_importExport == ImportExport.Import)
                    {
                        this.LoadRecords(this.FilePath, this.listViewCurvesImport);
                        this.groupBoxImport.Enabled = this.FilePath != "";
                    }
                    else
                    {
                        this.LoadRecords(null, this.listViewCurvesExport);
                        this.groupBoxExport.Enabled = this.FilePath != "";
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.m_importExport == ImportExport.Import)
            {
                this.m_selectedCurve = this.comboBoxSelectedCurve.SelectedItem as CurveLibraryRecord;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    List<CurveLibraryRecord> list = new List<CurveLibraryRecord>();
                    foreach (ListViewItem item in this.listViewCurvesExport.SelectedItems)
                    {
                        list.Add(item.Tag as CurveLibraryRecord);
                    }
                    this.ExportRecordsToFile(list.ToArray(), this.FilePath);
                    MessageBox.Show("Export completed successfully.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error occurred while exporting:\n" + exception.Message, "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void checkBoxSelectCurveToUse_Click(object sender, EventArgs e)
        {
        }

        private void comboBoxSelectedCurve_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.Index >= 0) && (e.Index < this.comboBoxSelectedCurve.Items.Count))
            {
                ComboBox box = sender as ComboBox;
                CurveLibraryRecord record = (CurveLibraryRecord)box.Items[e.Index];
                int width = e.Bounds.Height - 2;
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(record.Color)))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds.X + 1, e.Bounds.Y + 1, width, width);
                    e.Graphics.DrawRectangle(Pens.Black, (int)(e.Bounds.X + 1), (int)(e.Bounds.Y + 1), (int)(width - 1), (int)(width - 1));
                }
                e.Graphics.DrawString(string.Format("{0}, {1}, {2}", record.Manufacturer, record.LightCount, record.Controller), this.comboBoxSelectedCurve.Font, Brushes.Black, (float)((e.Bounds.X + width) + 5), (float)e.Bounds.Y);
            }
        }

        private void CurveFileImportExportDialog_Load(object sender, EventArgs e)
        {
            this.groupBoxImport.Visible = this.m_importExport == ImportExport.Import;
            this.groupBoxExport.Visible = this.m_importExport == ImportExport.Export;
        }



        private void ExportRecordsToFile(CurveLibraryRecord[] records, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (CurveLibrary library = new CurveLibrary(filePath))
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
            if (this.listViewCurvesExport.SelectedItems.Count == 1)
            {
                try
                {
                    this.ExportRecordsToFile(new CurveLibraryRecord[] { this.listViewCurvesExport.SelectedItems[0].Tag as CurveLibraryRecord }, this.FilePath);
                    base.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error occurred while exporting:\n" + exception.Message, "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void listViewCurvesImport_Leave(object sender, EventArgs e)
        {
            CurveLibraryRecord selectedItem = this.comboBoxSelectedCurve.SelectedItem as CurveLibraryRecord;
            this.comboBoxSelectedCurve.BeginUpdate();
            this.comboBoxSelectedCurve.Items.Clear();
            foreach (ListViewItem item in this.listViewCurvesImport.SelectedItems)
            {
                this.comboBoxSelectedCurve.Items.Add(item.Tag as CurveLibraryRecord);
            }
            if ((selectedItem != null) && this.comboBoxSelectedCurve.Items.Contains(selectedItem))
            {
                this.comboBoxSelectedCurve.SelectedItem = selectedItem;
            }
            this.comboBoxSelectedCurve.EndUpdate();
        }

        private void listViewCurvesImport_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listViewCurvesImport.SelectedItems.Count == 1)
            {
                CurveLibraryRecord tag = (CurveLibraryRecord)this.listViewCurvesImport.SelectedItems[0].Tag;
                this.m_selectedCurve = tag;
                base.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void listViewCurvesImport_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadRecords(string fileName, ListView listView)
        {
            CurveLibrary library = null;
            this.Cursor = Cursors.WaitCursor;
            listView.BeginUpdate();
            listView.Items.Clear();
            try
            {
                Exception exception;
                if (fileName == null)
                {
                    library = new CurveLibrary();
                }
                else
                {
                    library = new CurveLibrary(fileName);
                }
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
                        listView.Items.Add(new ListViewItem(new string[] { record.Manufacturer, record.LightCount, record.Color.ToString(), record.Controller })).Tag = record;
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
                this.Cursor = Cursors.Default;
                if (library != null)
                {
                    library.Dispose();
                }
            }
        }

        private string FilePath
        {
            get
            {
                return this.m_fileName;
            }
            set
            {
                this.m_fileName = value;
            }
        }

        public CurveLibraryRecord SelectedCurve
        {
            get
            {
                return this.m_selectedCurve;
            }
        }

        public enum ImportExport
        {
            Import,
            Export
        }
    }
}

