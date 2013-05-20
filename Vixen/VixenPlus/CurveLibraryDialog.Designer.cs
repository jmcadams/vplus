using System.Windows.Forms;
using System.ComponentModel;

namespace VixenPlus {
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
            this.btnOkay = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.comboBoxManufacturer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxController = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.buttonReload = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonChangeRemoteLocation = new System.Windows.Forms.Button();
            this.listViewRecords = new System.Windows.Forms.ListView();
            this.columnHeaderManufacturer = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCount = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderColor = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderController = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.labelDownloadMessage = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Enabled = false;
            this.btnOkay.Location = new System.Drawing.Point(413, 303);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(494, 303);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            this.label1.Visible = false;
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Items.AddRange(new object[] {"Local", "Remote"});
            this.comboBoxSource.Location = new System.Drawing.Point(62, 22);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.Visible = false;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxSource_SelectedIndexChanged);
            // 
            // comboBoxManufacturer
            // 
            this.comboBoxManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Items.AddRange(new object[] {"(All)"});
            this.comboBoxManufacturer.Location = new System.Drawing.Point(97, 24);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new System.Drawing.Size(156, 21);
            this.comboBoxManufacturer.Sorted = true;
            this.comboBoxManufacturer.TabIndex = 1;
            this.comboBoxManufacturer.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Manufacturer:";
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Items.AddRange(new object[] {"(All)"});
            this.comboBoxCount.Location = new System.Drawing.Point(97, 51);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(156, 21);
            this.comboBoxCount.Sorted = true;
            this.comboBoxCount.TabIndex = 3;
            this.comboBoxCount.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Count:";
            // 
            // comboBoxController
            // 
            this.comboBoxController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxController.FormattingEnabled = true;
            this.comboBoxController.Items.AddRange(new object[] {"(All)"});
            this.comboBoxController.Location = new System.Drawing.Point(97, 105);
            this.comboBoxController.Name = "comboBoxController";
            this.comboBoxController.Size = new System.Drawing.Size(156, 21);
            this.comboBoxController.Sorted = true;
            this.comboBoxController.TabIndex = 7;
            this.comboBoxController.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Controller:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 285);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light string criteria";
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {"(All)"});
            this.comboBoxColor.Location = new System.Drawing.Point(97, 78);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(156, 21);
            this.comboBoxColor.Sorted = true;
            this.comboBoxColor.TabIndex = 5;
            this.comboBoxColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxColor_DrawItem);
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new System.Drawing.Point(266, 8);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(135, 23);
            this.buttonReload.TabIndex = 2;
            this.buttonReload.Text = "Reload current source";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Visible = false;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Color:";
            // 
            // buttonChangeRemoteLocation
            // 
            this.buttonChangeRemoteLocation.Location = new System.Drawing.Point(407, 8);
            this.buttonChangeRemoteLocation.Name = "buttonChangeRemoteLocation";
            this.buttonChangeRemoteLocation.Size = new System.Drawing.Size(141, 23);
            this.buttonChangeRemoteLocation.TabIndex = 3;
            this.buttonChangeRemoteLocation.Text = "Change remote location";
            this.buttonChangeRemoteLocation.UseVisualStyleBackColor = true;
            this.buttonChangeRemoteLocation.Visible = false;
            this.buttonChangeRemoteLocation.Click += new System.EventHandler(this.buttonChangeRemoteLocation_Click);
            // 
            // listViewRecords
            // 
            this.listViewRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {this.columnHeaderManufacturer, this.columnHeaderCount, this.columnHeaderColor, this.columnHeaderController});
            this.listViewRecords.FullRowSelect = true;
            this.listViewRecords.Location = new System.Drawing.Point(21, 158);
            this.listViewRecords.MultiSelect = false;
            this.listViewRecords.Name = "listViewRecords";
            this.listViewRecords.OwnerDraw = true;
            this.listViewRecords.Size = new System.Drawing.Size(514, 111);
            this.listViewRecords.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRecords.TabIndex = 8;
            this.listViewRecords.UseCompatibleStateImageBehavior = false;
            this.listViewRecords.View = System.Windows.Forms.View.Details;
            this.listViewRecords.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewRecords_ColumnClick);
            this.listViewRecords.DrawColumnHeader +=
                new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewRecords_DrawColumnHeader);
            this.listViewRecords.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewRecords_DrawSubItem);
            this.listViewRecords.SelectedIndexChanged += new System.EventHandler(this.listViewRecords_SelectedIndexChanged);
            this.listViewRecords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewRecords_MouseDoubleClick);
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
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDownloadMessage.AutoSize = true;
            this.labelDownloadMessage.Location = new System.Drawing.Point(12, 316);
            this.labelDownloadMessage.Name = "labelDownloadMessage";
            this.labelDownloadMessage.Size = new System.Drawing.Size(0, 13);
            this.labelDownloadMessage.TabIndex = 7;
            // 
            // CurveLibraryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(581, 338);
            this.Controls.Add(this.labelDownloadMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.btnOkay);
            this.Icon = global::Properties.Resources.VixenPlus;
            this.Name = "CurveLibraryDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve Library";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurveLibraryDialog_FormClosing);
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
