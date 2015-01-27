using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    internal partial class CurveFileImportExportDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button btnFile;
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
        private GroupBox groupBoxExport;
        private GroupBox groupBoxImport;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelFile;
        private ListView listViewCurvesExport;
        private ListView listViewCurvesImport;
        private OpenFileDialog openFileDialog;


        private void InitializeComponent() {
            this.btnFile = new Button();
            this.labelFile = new Label();
            this.groupBoxImport = new GroupBox();
            this.label3 = new Label();
            this.comboBoxSelectedCurve = new ComboBox();
            this.label1 = new Label();
            this.listViewCurvesImport = new ListView();
            this.columnHeader1 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader2 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader7 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader3 = ((ColumnHeader) (new ColumnHeader()));
            this.groupBoxExport = new GroupBox();
            this.label2 = new Label();
            this.listViewCurvesExport = new ListView();
            this.columnHeader4 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader5 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader8 = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeader6 = ((ColumnHeader) (new ColumnHeader()));
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.groupBoxImport.SuspendLayout();
            this.groupBoxExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Location = new Point(15, 19);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new Size(49, 23);
            this.btnFile.TabIndex = 0;
            this.btnFile.Text = "File:";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new EventHandler(this.buttonFile_Click);
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new Point(76, 25);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new Size(37, 13);
            this.labelFile.TabIndex = 1;
            this.labelFile.Text = "(none)";
            // 
            // groupBoxImport
            // 
            this.groupBoxImport.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.groupBoxImport.Controls.Add(this.label3);
            this.groupBoxImport.Controls.Add(this.comboBoxSelectedCurve);
            this.groupBoxImport.Controls.Add(this.label1);
            this.groupBoxImport.Controls.Add(this.listViewCurvesImport);
            this.groupBoxImport.Enabled = false;
            this.groupBoxImport.Location = new Point(15, 53);
            this.groupBoxImport.Name = "groupBoxImport";
            this.groupBoxImport.Size = new Size(464, 263);
            this.groupBoxImport.TabIndex = 2;
            this.groupBoxImport.TabStop = false;
            this.groupBoxImport.Text = "Import Dimming Curves";
            // 
            // label3
            // 
            this.label3.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new Point(19, 225);
            this.label3.Name = "label3";
            this.label3.Size = new Size(208, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From those, use this curve for the channel:";
            // 
            // comboBoxSelectedCurve
            // 
            this.comboBoxSelectedCurve.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.comboBoxSelectedCurve.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxSelectedCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSelectedCurve.FormattingEnabled = true;
            this.comboBoxSelectedCurve.Location = new Point(233, 222);
            this.comboBoxSelectedCurve.Name = "comboBoxSelectedCurve";
            this.comboBoxSelectedCurve.Size = new Size(212, 21);
            this.comboBoxSelectedCurve.TabIndex = 3;
            this.comboBoxSelectedCurve.DrawItem += new DrawItemEventHandler(this.comboBoxSelectedCurve_DrawItem);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(302, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select one or more curves to import from the file into the library.";
            // 
            // listViewCurvesImport
            // 
            this.listViewCurvesImport.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.listViewCurvesImport.Columns.AddRange(new ColumnHeader[]
            {this.columnHeader1, this.columnHeader2, this.columnHeader7, this.columnHeader3});
            this.listViewCurvesImport.FullRowSelect = true;
            this.listViewCurvesImport.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesImport.HideSelection = false;
            this.listViewCurvesImport.Location = new Point(18, 43);
            this.listViewCurvesImport.Name = "listViewCurvesImport";
            this.listViewCurvesImport.OwnerDraw = true;
            this.listViewCurvesImport.Size = new Size(427, 161);
            this.listViewCurvesImport.TabIndex = 0;
            this.listViewCurvesImport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesImport.View = View.Details;
            this.listViewCurvesImport.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesImport.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.listViewCurvesImport.SelectedIndexChanged += new EventHandler(this.listViewCurvesImport_SelectedIndexChanged);
            this.listViewCurvesImport.Leave += new EventHandler(this.listViewCurvesImport_Leave);
            this.listViewCurvesImport.MouseDoubleClick += new MouseEventHandler(this.listViewCurvesImport_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Manufacturer";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Count";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Color";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Controller";
            this.columnHeader3.Width = 110;
            // 
            // groupBoxExport
            // 
            this.groupBoxExport.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.groupBoxExport.Controls.Add(this.label2);
            this.groupBoxExport.Controls.Add(this.listViewCurvesExport);
            this.groupBoxExport.Enabled = false;
            this.groupBoxExport.Location = new Point(15, 53);
            this.groupBoxExport.Name = "groupBoxExport";
            this.groupBoxExport.Size = new Size(464, 263);
            this.groupBoxExport.TabIndex = 3;
            this.groupBoxExport.TabStop = false;
            this.groupBoxExport.Text = "Export Dimming Curves";
            this.groupBoxExport.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 16);
            this.label2.Name = "label2";
            this.label2.Size = new Size(295, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select one or more curves to export to the file from the library.";
            // 
            // listViewCurvesExport
            // 
            this.listViewCurvesExport.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.listViewCurvesExport.Columns.AddRange(new ColumnHeader[]
            {this.columnHeader4, this.columnHeader5, this.columnHeader8, this.columnHeader6});
            this.listViewCurvesExport.FullRowSelect = true;
            this.listViewCurvesExport.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesExport.HideSelection = false;
            this.listViewCurvesExport.Location = new Point(18, 43);
            this.listViewCurvesExport.Name = "listViewCurvesExport";
            this.listViewCurvesExport.OwnerDraw = true;
            this.listViewCurvesExport.Size = new Size(427, 195);
            this.listViewCurvesExport.TabIndex = 0;
            this.listViewCurvesExport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesExport.View = View.Details;
            this.listViewCurvesExport.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesExport.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.listViewCurvesExport.MouseDoubleClick += new MouseEventHandler(this.listViewCurvesExport_MouseDoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Manufacturer";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Count";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Color";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Controller";
            this.columnHeader6.Width = 110;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(323, 322);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(404, 322);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // CurveFileImportExportDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(491, 357);
            this.Controls.Add(this.groupBoxImport);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxExport);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.btnFile);
            this.MinimizeBox = false;
            this.Name = "CurveFileImportExportDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Dimming Curve File Import/Export";
            this.Load += new EventHandler(this.CurveFileImportExportDialog_Load);
            this.groupBoxImport.ResumeLayout(false);
            this.groupBoxImport.PerformLayout();
            this.groupBoxExport.ResumeLayout(false);
            this.groupBoxExport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
