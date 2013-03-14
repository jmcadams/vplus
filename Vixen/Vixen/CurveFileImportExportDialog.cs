namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class CurveFileImportExportDialog : Form
    {
        private Button buttonCancel;
        private Button buttonFile;
        private Button buttonOK;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ComboBox comboBoxSelectedCurve;
        private IContainer components = null;
        private GroupBox groupBoxExport;
        private GroupBox groupBoxImport;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelFile;
        private ListView listViewCurvesExport;
        private ListView listViewCurvesImport;
        private string m_fileName = "";
        private ImportExport m_importExport;
        private CurveLibraryRecord m_selectedCurve = null;
        private OpenFileDialog openFileDialog;

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
                CurveLibraryRecord record = (CurveLibraryRecord) box.Items[e.Index];
                int width = e.Bounds.Height - 2;
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(record.Color)))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds.X + 1, e.Bounds.Y + 1, width, width);
                    e.Graphics.DrawRectangle(Pens.Black, (int) (e.Bounds.X + 1), (int) (e.Bounds.Y + 1), (int) (width - 1), (int) (width - 1));
                }
                e.Graphics.DrawString(string.Format("{0}, {1}, {2}", record.Manufacturer, record.LightCount, record.Controller), this.comboBoxSelectedCurve.Font, Brushes.Black, (float) ((e.Bounds.X + width) + 5), (float) e.Bounds.Y);
            }
        }

        private void CurveFileImportExportDialog_Load(object sender, EventArgs e)
        {
            this.groupBoxImport.Visible = this.m_importExport == ImportExport.Import;
            this.groupBoxExport.Visible = this.m_importExport == ImportExport.Export;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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

        private void InitializeComponent()
        {
            this.buttonFile = new Button();
            this.labelFile = new Label();
            this.groupBoxImport = new GroupBox();
            this.label3 = new Label();
            this.comboBoxSelectedCurve = new ComboBox();
            this.label1 = new Label();
            this.listViewCurvesImport = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader7 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.groupBoxExport = new GroupBox();
            this.label2 = new Label();
            this.listViewCurvesExport = new ListView();
            this.columnHeader4 = new ColumnHeader();
            this.columnHeader5 = new ColumnHeader();
            this.columnHeader8 = new ColumnHeader();
            this.columnHeader6 = new ColumnHeader();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.groupBoxImport.SuspendLayout();
            this.groupBoxExport.SuspendLayout();
            base.SuspendLayout();
            this.buttonFile.Location = new Point(15, 0x13);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new Size(0x31, 0x17);
            this.buttonFile.TabIndex = 0;
            this.buttonFile.Text = "File:";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new EventHandler(this.buttonFile_Click);
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new Point(0x4c, 0x19);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new Size(0x25, 13);
            this.labelFile.TabIndex = 1;
            this.labelFile.Text = "(none)";
            this.groupBoxImport.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBoxImport.Controls.Add(this.label3);
            this.groupBoxImport.Controls.Add(this.comboBoxSelectedCurve);
            this.groupBoxImport.Controls.Add(this.label1);
            this.groupBoxImport.Controls.Add(this.listViewCurvesImport);
            this.groupBoxImport.Enabled = false;
            this.groupBoxImport.Location = new Point(15, 0x35);
            this.groupBoxImport.Name = "groupBoxImport";
            this.groupBoxImport.Size = new Size(0x1d0, 0x107);
            this.groupBoxImport.TabIndex = 2;
            this.groupBoxImport.TabStop = false;
            this.groupBoxImport.Text = "Import Dimming Curves";
            this.label3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x13, 0xe1);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xd0, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From those, use this curve for the channel:";
            this.comboBoxSelectedCurve.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.comboBoxSelectedCurve.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxSelectedCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSelectedCurve.FormattingEnabled = true;
            this.comboBoxSelectedCurve.Location = new Point(0xe9, 0xde);
            this.comboBoxSelectedCurve.Name = "comboBoxSelectedCurve";
            this.comboBoxSelectedCurve.Size = new Size(0xd4, 0x15);
            this.comboBoxSelectedCurve.TabIndex = 3;
            this.comboBoxSelectedCurve.DrawItem += new DrawItemEventHandler(this.comboBoxSelectedCurve_DrawItem);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x12e, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select one or more curves to import from the file into the library.";
            this.listViewCurvesImport.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewCurvesImport.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader7, this.columnHeader3 });
            this.listViewCurvesImport.FullRowSelect = true;
            this.listViewCurvesImport.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesImport.HideSelection = false;
            this.listViewCurvesImport.Location = new Point(0x12, 0x2b);
            this.listViewCurvesImport.Name = "listViewCurvesImport";
            this.listViewCurvesImport.OwnerDraw = true;
            this.listViewCurvesImport.Size = new Size(0x1ab, 0xa1);
            this.listViewCurvesImport.TabIndex = 0;
            this.listViewCurvesImport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesImport.View = View.Details;
            this.listViewCurvesImport.MouseDoubleClick += new MouseEventHandler(this.listViewCurvesImport_MouseDoubleClick);
            this.listViewCurvesImport.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesImport.SelectedIndexChanged += new EventHandler(this.listViewCurvesImport_SelectedIndexChanged);
            this.listViewCurvesImport.Leave += new EventHandler(this.listViewCurvesImport_Leave);
            this.listViewCurvesImport.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.columnHeader1.Text = "Manufacturer";
            this.columnHeader1.Width = 150;
            this.columnHeader2.Text = "Count";
            this.columnHeader7.Text = "Color";
            this.columnHeader3.Text = "Controller";
            this.columnHeader3.Width = 110;
            this.groupBoxExport.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBoxExport.Controls.Add(this.label2);
            this.groupBoxExport.Controls.Add(this.listViewCurvesExport);
            this.groupBoxExport.Enabled = false;
            this.groupBoxExport.Location = new Point(15, 0x35);
            this.groupBoxExport.Name = "groupBoxExport";
            this.groupBoxExport.Size = new Size(0x1d0, 0x107);
            this.groupBoxExport.TabIndex = 3;
            this.groupBoxExport.TabStop = false;
            this.groupBoxExport.Text = "Export Dimming Curves";
            this.groupBoxExport.Visible = false;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x127, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select one or more curves to export to the file from the library.";
            this.listViewCurvesExport.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewCurvesExport.Columns.AddRange(new ColumnHeader[] { this.columnHeader4, this.columnHeader5, this.columnHeader8, this.columnHeader6 });
            this.listViewCurvesExport.FullRowSelect = true;
            this.listViewCurvesExport.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesExport.HideSelection = false;
            this.listViewCurvesExport.Location = new Point(0x12, 0x2b);
            this.listViewCurvesExport.Name = "listViewCurvesExport";
            this.listViewCurvesExport.OwnerDraw = true;
            this.listViewCurvesExport.Size = new Size(0x1ab, 0xc3);
            this.listViewCurvesExport.TabIndex = 0;
            this.listViewCurvesExport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesExport.View = View.Details;
            this.listViewCurvesExport.MouseDoubleClick += new MouseEventHandler(this.listViewCurvesExport_MouseDoubleClick);
            this.listViewCurvesExport.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesExport.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.columnHeader4.Text = "Manufacturer";
            this.columnHeader4.Width = 150;
            this.columnHeader5.Text = "Count";
            this.columnHeader8.Text = "Color";
            this.columnHeader6.Text = "Controller";
            this.columnHeader6.Width = 110;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x143, 0x142);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x194, 0x142);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.openFileDialog.SupportMultiDottedExtensions = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1eb, 0x165);
            base.Controls.Add(this.groupBoxImport);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBoxExport);
            base.Controls.Add(this.labelFile);
            base.Controls.Add(this.buttonFile);
            base.MinimizeBox = false;
            base.Name = "CurveFileImportExportDialog";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Dimming Curve File Import/Export";
            base.Load += new EventHandler(this.CurveFileImportExportDialog_Load);
            this.groupBoxImport.ResumeLayout(false);
            this.groupBoxImport.PerformLayout();
            this.groupBoxExport.ResumeLayout(false);
            this.groupBoxExport.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listViewCurvesExport_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listViewCurvesExport.SelectedItems.Count == 1)
            {
                try
                {
                    this.ExportRecordsToFile(new CurveLibraryRecord[] { this.listViewCurvesExport.SelectedItems[0].Tag as CurveLibraryRecord }, this.FilePath);
                    base.DialogResult = DialogResult.OK;
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
                CurveLibraryRecord tag = (CurveLibraryRecord) this.listViewCurvesImport.SelectedItems[0].Tag;
                this.m_selectedCurve = tag;
                base.DialogResult = DialogResult.OK;
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

