namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class CurveConflictResolutionDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader7;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private ListView listView;
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
            this.groupBox1 = new GroupBox();
            this.listView = new ListView();
            this.columnHeader7 = new ColumnHeader();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1ab, 290);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conflicts";
            this.listView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new ColumnHeader[] { this.columnHeader7, this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new Point(20, 0x55);
            this.listView.Name = "listView";
            this.listView.Size = new Size(0x187, 0xbb);
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = View.Details;
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 0x19;
            this.columnHeader1.Text = "Manufacturer";
            this.columnHeader1.Width = 150;
            this.columnHeader2.Text = "Count";
            this.columnHeader3.Text = "Controller";
            this.columnHeader3.Width = 130;
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(0x11, 0x19);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x18a, 0x20);
            this.label1.TabIndex = 0;
            this.label1.Text = "The target library already has definitions for these listed configurations.  Select which ones you want to overwrite with your changes.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x11b, 0x134);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x16c, 0x134);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1c3, 0x157);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CurveConflictResolutionDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Curve Conflict Resolution";
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
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

