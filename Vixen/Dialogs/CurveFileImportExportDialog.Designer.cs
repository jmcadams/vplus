using System.ComponentModel;
using System.Windows.Forms;

namespace Dialogs {
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
            this.btnFile = new System.Windows.Forms.Button();
            this.labelFile = new System.Windows.Forms.Label();
            this.groupBoxImport = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSelectedCurve = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewCurvesImport = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.groupBoxExport = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewCurvesExport = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxImport.SuspendLayout();
            this.groupBoxExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(15, 19);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(49, 23);
            this.btnFile.TabIndex = 0;
            this.btnFile.Text = "File:";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(76, 25);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(37, 13);
            this.labelFile.TabIndex = 1;
            this.labelFile.Text = "(none)";
            // 
            // groupBoxImport
            // 
            this.groupBoxImport.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxImport.Controls.Add(this.label3);
            this.groupBoxImport.Controls.Add(this.comboBoxSelectedCurve);
            this.groupBoxImport.Controls.Add(this.label1);
            this.groupBoxImport.Controls.Add(this.listViewCurvesImport);
            this.groupBoxImport.Enabled = false;
            this.groupBoxImport.Location = new System.Drawing.Point(15, 53);
            this.groupBoxImport.Name = "groupBoxImport";
            this.groupBoxImport.Size = new System.Drawing.Size(464, 263);
            this.groupBoxImport.TabIndex = 2;
            this.groupBoxImport.TabStop = false;
            this.groupBoxImport.Text = "Import Dimming Curves";
            // 
            // label3
            // 
            this.label3.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From those, use this curve for the channel:";
            // 
            // comboBoxSelectedCurve
            // 
            this.comboBoxSelectedCurve.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxSelectedCurve.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxSelectedCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectedCurve.FormattingEnabled = true;
            this.comboBoxSelectedCurve.Location = new System.Drawing.Point(233, 222);
            this.comboBoxSelectedCurve.Name = "comboBoxSelectedCurve";
            this.comboBoxSelectedCurve.Size = new System.Drawing.Size(212, 21);
            this.comboBoxSelectedCurve.TabIndex = 3;
            this.comboBoxSelectedCurve.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxSelectedCurve_DrawItem);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select one or more curves to import from the file into the library.";
            // 
            // listViewCurvesImport
            // 
            this.listViewCurvesImport.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCurvesImport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {this.columnHeader1, this.columnHeader2, this.columnHeader7, this.columnHeader3});
            this.listViewCurvesImport.FullRowSelect = true;
            this.listViewCurvesImport.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesImport.HideSelection = false;
            this.listViewCurvesImport.Location = new System.Drawing.Point(18, 43);
            this.listViewCurvesImport.Name = "listViewCurvesImport";
            this.listViewCurvesImport.OwnerDraw = true;
            this.listViewCurvesImport.Size = new System.Drawing.Size(427, 161);
            this.listViewCurvesImport.TabIndex = 0;
            this.listViewCurvesImport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesImport.View = System.Windows.Forms.View.Details;
            this.listViewCurvesImport.DrawColumnHeader +=
                new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesImport.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.listViewCurvesImport.SelectedIndexChanged += new System.EventHandler(this.listViewCurvesImport_SelectedIndexChanged);
            this.listViewCurvesImport.Leave += new System.EventHandler(this.listViewCurvesImport_Leave);
            this.listViewCurvesImport.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewCurvesImport_MouseDoubleClick);
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
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxExport.Controls.Add(this.label2);
            this.groupBoxExport.Controls.Add(this.listViewCurvesExport);
            this.groupBoxExport.Enabled = false;
            this.groupBoxExport.Location = new System.Drawing.Point(15, 53);
            this.groupBoxExport.Name = "groupBoxExport";
            this.groupBoxExport.Size = new System.Drawing.Size(464, 263);
            this.groupBoxExport.TabIndex = 3;
            this.groupBoxExport.TabStop = false;
            this.groupBoxExport.Text = "Export Dimming Curves";
            this.groupBoxExport.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select one or more curves to export to the file from the library.";
            // 
            // listViewCurvesExport
            // 
            this.listViewCurvesExport.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCurvesExport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {this.columnHeader4, this.columnHeader5, this.columnHeader8, this.columnHeader6});
            this.listViewCurvesExport.FullRowSelect = true;
            this.listViewCurvesExport.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCurvesExport.HideSelection = false;
            this.listViewCurvesExport.Location = new System.Drawing.Point(18, 43);
            this.listViewCurvesExport.Name = "listViewCurvesExport";
            this.listViewCurvesExport.OwnerDraw = true;
            this.listViewCurvesExport.Size = new System.Drawing.Size(427, 195);
            this.listViewCurvesExport.TabIndex = 0;
            this.listViewCurvesExport.UseCompatibleStateImageBehavior = false;
            this.listViewCurvesExport.View = System.Windows.Forms.View.Details;
            this.listViewCurvesExport.DrawColumnHeader +=
                new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewCurvesImport_DrawColumnHeader);
            this.listViewCurvesExport.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewCurvesImport_DrawSubItem);
            this.listViewCurvesExport.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewCurvesExport_MouseDoubleClick);
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
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(323, 322);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(404, 322);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(491, 357);
            this.Controls.Add(this.groupBoxImport);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxExport);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.btnFile);
            this.MinimizeBox = false;
            this.Name = "CurveFileImportExportDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dimming Curve File Import/Export";
            this.Load += new System.EventHandler(this.CurveFileImportExportDialog_Load);
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
