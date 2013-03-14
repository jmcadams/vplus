namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class CurveLibraryDialog : Form
    {
        private Button buttonCancel;
        private Button buttonChangeRemoteLocation;
        private Button buttonOK;
        private Button buttonReload;
        private const string COLOR_PREPEND = "C";
        private ColumnHeader columnHeaderColor;
        private ColumnHeader columnHeaderController;
        private ColumnHeader columnHeaderCount;
        private ColumnHeader columnHeaderManufacturer;
        private ComboBox comboBoxColor;
        private ComboBox comboBoxController;
        private ComboBox comboBoxCount;
        private ComboBox comboBoxManufacturer;
        private ComboBox comboBoxSource;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labelDownloadMessage;
        private ListView listViewRecords;
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
            ListViewSortIcons.SetSortIcon(this.listViewRecords, 0, this.listViewRecords.Columns[0].Tag = System.Windows.Forms.SortOrder.Ascending);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.label1 = new Label();
            this.comboBoxSource = new ComboBox();
            this.comboBoxManufacturer = new ComboBox();
            this.label2 = new Label();
            this.comboBoxCount = new ComboBox();
            this.label3 = new Label();
            this.comboBoxController = new ComboBox();
            this.label4 = new Label();
            this.groupBox1 = new GroupBox();
            this.comboBoxColor = new ComboBox();
            this.label5 = new Label();
            this.listViewRecords = new ListView();
            this.columnHeaderManufacturer = new ColumnHeader();
            this.columnHeaderCount = new ColumnHeader();
            this.columnHeaderColor = new ColumnHeader();
            this.columnHeaderController = new ColumnHeader();
            this.buttonChangeRemoteLocation = new Button();
            this.buttonReload = new Button();
            this.labelDownloadMessage = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new Point(0x19d, 0x12f);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x1ee, 0x12f);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x19);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2c, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            this.label1.Visible = false;
            this.comboBoxSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Items.AddRange(new object[] { "Local", "Remote" });
            this.comboBoxSource.Location = new Point(0x3e, 0x16);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new Size(0x79, 0x15);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.Visible = false;
            this.comboBoxSource.SelectedIndexChanged += new EventHandler(this.comboBoxSource_SelectedIndexChanged);
            this.comboBoxManufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Items.AddRange(new object[] { "(All)" });
            this.comboBoxManufacturer.Location = new Point(0x61, 0x18);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new Size(0x9c, 0x15);
            this.comboBoxManufacturer.Sorted = true;
            this.comboBoxManufacturer.TabIndex = 1;
            this.comboBoxManufacturer.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 0x1b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x49, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Manufacturer:";
            this.comboBoxCount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Items.AddRange(new object[] { "(All)" });
            this.comboBoxCount.Location = new Point(0x61, 0x33);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new Size(0x9c, 0x15);
            this.comboBoxCount.Sorted = true;
            this.comboBoxCount.TabIndex = 3;
            this.comboBoxCount.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x12, 0x36);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x26, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Count:";
            this.comboBoxController.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxController.FormattingEnabled = true;
            this.comboBoxController.Items.AddRange(new object[] { "(All)" });
            this.comboBoxController.Location = new Point(0x61, 0x69);
            this.comboBoxController.Name = "comboBoxController";
            this.comboBoxController.Size = new Size(0x9c, 0x15);
            this.comboBoxController.Sorted = true;
            this.comboBoxController.TabIndex = 7;
            this.comboBoxController.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x12, 0x6c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x36, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Controller:";
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.comboBoxColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.listViewRecords);
            this.groupBox1.Controls.Add(this.comboBoxController);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxManufacturer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x22a, 0x11d);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light string criteria";
            this.comboBoxColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] { "(All)" });
            this.comboBoxColor.Location = new Point(0x61, 0x4e);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new Size(0x9c, 0x15);
            this.comboBoxColor.Sorted = true;
            this.comboBoxColor.TabIndex = 5;
            this.comboBoxColor.DrawItem += new DrawItemEventHandler(this.comboBoxColor_DrawItem);
            this.comboBoxColor.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x12, 0x51);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x22, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Color:";
            this.listViewRecords.Columns.AddRange(new ColumnHeader[] { this.columnHeaderManufacturer, this.columnHeaderCount, this.columnHeaderColor, this.columnHeaderController });
            this.listViewRecords.FullRowSelect = true;
            this.listViewRecords.Location = new Point(0x15, 0x9e);
            this.listViewRecords.MultiSelect = false;
            this.listViewRecords.Name = "listViewRecords";
            this.listViewRecords.OwnerDraw = true;
            this.listViewRecords.Size = new Size(0x202, 0x6f);
            this.listViewRecords.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRecords.TabIndex = 8;
            this.listViewRecords.UseCompatibleStateImageBehavior = false;
            this.listViewRecords.View = View.Details;
            this.listViewRecords.MouseDoubleClick += new MouseEventHandler(this.listViewRecords_MouseDoubleClick);
            this.listViewRecords.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewRecords_DrawColumnHeader);
            this.listViewRecords.SelectedIndexChanged += new EventHandler(this.listViewRecords_SelectedIndexChanged);
            this.listViewRecords.ColumnClick += new ColumnClickEventHandler(this.listViewRecords_ColumnClick);
            this.listViewRecords.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewRecords_DrawSubItem);
            this.columnHeaderManufacturer.Text = "Manufacturer";
            this.columnHeaderManufacturer.Width = 0xa1;
            this.columnHeaderCount.Text = "Count";
            this.columnHeaderCount.Width = 0x4b;
            this.columnHeaderColor.Text = "Color";
            this.columnHeaderColor.Width = 0x51;
            this.columnHeaderController.Text = "Controller";
            this.columnHeaderController.Width = 0x9e;
            this.buttonChangeRemoteLocation.Location = new Point(0x15b, 20);
            this.buttonChangeRemoteLocation.Name = "buttonChangeRemoteLocation";
            this.buttonChangeRemoteLocation.Size = new Size(0x8d, 0x17);
            this.buttonChangeRemoteLocation.TabIndex = 3;
            this.buttonChangeRemoteLocation.Text = "Change remote location";
            this.buttonChangeRemoteLocation.UseVisualStyleBackColor = true;
            this.buttonChangeRemoteLocation.Visible = false;
            this.buttonChangeRemoteLocation.Click += new EventHandler(this.buttonChangeRemoteLocation_Click);
            this.buttonReload.Location = new Point(0xce, 20);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new Size(0x87, 0x17);
            this.buttonReload.TabIndex = 2;
            this.buttonReload.Text = "Reload current source";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Visible = false;
            this.buttonReload.Click += new EventHandler(this.buttonReload_Click);
            this.labelDownloadMessage.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.labelDownloadMessage.AutoSize = true;
            this.labelDownloadMessage.Location = new Point(12, 0x13c);
            this.labelDownloadMessage.Name = "labelDownloadMessage";
            this.labelDownloadMessage.Size = new Size(0, 13);
            this.labelDownloadMessage.TabIndex = 7;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x245, 0x152);
            base.Controls.Add(this.labelDownloadMessage);
            base.Controls.Add(this.buttonReload);
            base.Controls.Add(this.buttonChangeRemoteLocation);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.comboBoxSource);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Name = "CurveLibraryDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve Library";
            base.FormClosing += new FormClosingEventHandler(this.CurveLibraryDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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
            base.DialogResult = DialogResult.OK;
        }

        private void listViewRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonOK.Enabled = this.listViewRecords.SelectedItems.Count > 0;
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
                    this.buttonOK.Enabled = false;
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

