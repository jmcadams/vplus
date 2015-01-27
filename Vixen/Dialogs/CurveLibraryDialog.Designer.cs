using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    internal partial class CurveLibraryDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonChangeRemoteLocation;
        private Button btnOkay;
        private Button buttonReload;
        private ColumnHeader columnHeaderColor;
        private ColumnHeader columnHeaderController;
        private ColumnHeader columnHeaderCount;
        private ColumnHeader columnHeaderManufacturer;
        private ComboBox comboBoxColor;
        private ComboBox comboBoxController;
        private ComboBox comboBoxCount;
        private ComboBox comboBoxManufacturer;
        private ComboBox comboBoxSource;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labelDownloadMessage;
        private ListView listViewRecords;


        private void InitializeComponent() {
            this.btnOkay = new Button();
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
            this.buttonReload = new Button();
            this.label5 = new Label();
            this.buttonChangeRemoteLocation = new Button();
            this.listViewRecords = new ListView();
            this.columnHeaderManufacturer = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeaderCount = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeaderColor = ((ColumnHeader) (new ColumnHeader()));
            this.columnHeaderController = ((ColumnHeader) (new ColumnHeader()));
            this.labelDownloadMessage = new Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnOkay.DialogResult = DialogResult.OK;
            this.btnOkay.Enabled = false;
            this.btnOkay.Location = new Point(413, 303);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(494, 303);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            this.label1.Visible = false;
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Items.AddRange(new object[] {"Local", "Remote"});
            this.comboBoxSource.Location = new Point(62, 22);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new Size(121, 21);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.Visible = false;
            this.comboBoxSource.SelectedIndexChanged += new EventHandler(this.comboBoxSource_SelectedIndexChanged);
            // 
            // comboBoxManufacturer
            // 
            this.comboBoxManufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Items.AddRange(new object[] {"(All)"});
            this.comboBoxManufacturer.Location = new Point(97, 24);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new Size(156, 21);
            this.comboBoxManufacturer.Sorted = true;
            this.comboBoxManufacturer.TabIndex = 1;
            this.comboBoxManufacturer.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(18, 27);
            this.label2.Name = "label2";
            this.label2.Size = new Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Manufacturer:";
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Items.AddRange(new object[] {"(All)"});
            this.comboBoxCount.Location = new Point(97, 51);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new Size(156, 21);
            this.comboBoxCount.Sorted = true;
            this.comboBoxCount.TabIndex = 3;
            this.comboBoxCount.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(18, 54);
            this.label3.Name = "label3";
            this.label3.Size = new Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Count:";
            // 
            // comboBoxController
            // 
            this.comboBoxController.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxController.FormattingEnabled = true;
            this.comboBoxController.Items.AddRange(new object[] {"(All)"});
            this.comboBoxController.Location = new Point(97, 105);
            this.comboBoxController.Name = "comboBoxController";
            this.comboBoxController.Size = new Size(156, 21);
            this.comboBoxController.Sorted = true;
            this.comboBoxController.TabIndex = 7;
            this.comboBoxController.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(18, 108);
            this.label4.Name = "label4";
            this.label4.Size = new Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Controller:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor =
                ((AnchorStyles)
                 (((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxColor);
            this.groupBox1.Controls.Add(this.buttonReload);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonChangeRemoteLocation);
            this.groupBox1.Controls.Add(this.listViewRecords);
            this.groupBox1.Controls.Add(this.comboBoxController);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxManufacturer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(554, 285);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light string criteria";
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {"(All)"});
            this.comboBoxColor.Location = new Point(97, 78);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new Size(156, 21);
            this.comboBoxColor.Sorted = true;
            this.comboBoxColor.TabIndex = 5;
            this.comboBoxColor.DrawItem += new DrawItemEventHandler(this.comboBoxColor_DrawItem);
            this.comboBoxColor.SelectedIndexChanged += new EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new Point(266, 8);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new Size(135, 23);
            this.buttonReload.TabIndex = 2;
            this.buttonReload.Text = "Reload current source";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Visible = false;
            this.buttonReload.Click += new EventHandler(this.buttonReload_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(18, 81);
            this.label5.Name = "label5";
            this.label5.Size = new Size(34, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Color:";
            // 
            // buttonChangeRemoteLocation
            // 
            this.buttonChangeRemoteLocation.Location = new Point(407, 8);
            this.buttonChangeRemoteLocation.Name = "buttonChangeRemoteLocation";
            this.buttonChangeRemoteLocation.Size = new Size(141, 23);
            this.buttonChangeRemoteLocation.TabIndex = 3;
            this.buttonChangeRemoteLocation.Text = "Change remote location";
            this.buttonChangeRemoteLocation.UseVisualStyleBackColor = true;
            this.buttonChangeRemoteLocation.Visible = false;
            this.buttonChangeRemoteLocation.Click += new EventHandler(this.buttonChangeRemoteLocation_Click);
            // 
            // listViewRecords
            // 
            this.listViewRecords.Columns.AddRange(new ColumnHeader[]
            {this.columnHeaderManufacturer, this.columnHeaderCount, this.columnHeaderColor, this.columnHeaderController});
            this.listViewRecords.FullRowSelect = true;
            this.listViewRecords.Location = new Point(21, 158);
            this.listViewRecords.MultiSelect = false;
            this.listViewRecords.Name = "listViewRecords";
            this.listViewRecords.OwnerDraw = true;
            this.listViewRecords.Size = new Size(514, 111);
            this.listViewRecords.Sorting = SortOrder.Ascending;
            this.listViewRecords.TabIndex = 8;
            this.listViewRecords.UseCompatibleStateImageBehavior = false;
            this.listViewRecords.View = View.Details;
            this.listViewRecords.ColumnClick += new ColumnClickEventHandler(this.listViewRecords_ColumnClick);
            this.listViewRecords.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler(this.listViewRecords_DrawColumnHeader);
            this.listViewRecords.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewRecords_DrawSubItem);
            this.listViewRecords.SelectedIndexChanged += new EventHandler(this.listViewRecords_SelectedIndexChanged);
            this.listViewRecords.MouseDoubleClick += new MouseEventHandler(this.listViewRecords_MouseDoubleClick);
            // 
            // columnHeaderManufacturer
            // 
            this.columnHeaderManufacturer.Text = "Manufacturer";
            this.columnHeaderManufacturer.Width = 161;
            // 
            // columnHeaderCount
            // 
            this.columnHeaderCount.Text = "Count";
            this.columnHeaderCount.Width = 75;
            // 
            // columnHeaderColor
            // 
            this.columnHeaderColor.Text = "Color";
            this.columnHeaderColor.Width = 81;
            // 
            // columnHeaderController
            // 
            this.columnHeaderController.Text = "Controller";
            this.columnHeaderController.Width = 158;
            // 
            // labelDownloadMessage
            // 
            this.labelDownloadMessage.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.labelDownloadMessage.AutoSize = true;
            this.labelDownloadMessage.Location = new Point(12, 316);
            this.labelDownloadMessage.Name = "labelDownloadMessage";
            this.labelDownloadMessage.Size = new Size(0, 13);
            this.labelDownloadMessage.TabIndex = 7;
            // 
            // CurveLibraryDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(581, 338);
            this.Controls.Add(this.labelDownloadMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.btnOkay);
            this.Name = "CurveLibraryDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve Library";
            this.FormClosing += new FormClosingEventHandler(this.CurveLibraryDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
