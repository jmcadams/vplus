using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

using Controllers.E131.Controls;

namespace Controllers.E131 {
    public partial class E131SetupForm {
        private System.ComponentModel.IContainer components;

        #region Windows Form Designer generated code

        private DataGridViewNumbered univDGVN;
        private DataGridViewCellEventArgs univDGVNCellEventArgs = null;
        private DataGridViewCheckBoxColumn activeColumn;
        private DataGridViewTextBoxColumn universeColumn, startColumn, sizeColumn, ttlColumn;
        private DataGridViewComboBoxColumn destinationColumn;
        private CheckBox warningsCheckBox, statisticsCheckBox;
        private TextBox eventRepeatCountTextBox;
        private Button btnValidate;
        private ContextMenuStrip rowManipulationContextMenuStrip = new ContextMenuStrip();
        private ToolTip destinationToolTip = null;
        private ContextMenuStrip destinationContextMenuStrip = null;


        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label = new System.Windows.Forms.Label();
            this.rowManipulationContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.univDGVN = new Controllers.E131.Controls.DataGridViewNumbered();
            this.activeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.universeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destinationColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ttlColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warningsCheckBox = new System.Windows.Forms.CheckBox();
            this.statisticsCheckBox = new System.Windows.Forms.CheckBox();
            this.eventRepeatCountTextBox = new System.Windows.Forms.TextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnSysInfo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.univDGVN)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(652, 56);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(284, 66);
            this.label.TabIndex = 5;
            this.label.Text = "Event Repeat Count: Set to 0 to send all events to each universe, set to > 0 to s" +
                "kip \'x\' events if data is unchanged on a per universe basis.";
            // 
            // rowManipulationContextMenuStrip
            // 
            this.rowManipulationContextMenuStrip.Name = "rowManipulationContextMenuStrip";
            this.rowManipulationContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.rowManipulationContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.rowManipulationContextMenuStrip_Opening);
            // 
            // univDGVN
            // 
            this.univDGVN.AllowUserToDeleteRows = false;
            this.univDGVN.BackgroundColor = this.BackColor;
            this.univDGVN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.univDGVN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.activeColumn,
            this.universeColumn,
            this.startColumn,
            this.sizeColumn,
            this.destinationColumn,
            this.ttlColumn});
            this.univDGVN.Location = new System.Drawing.Point(10, 10);
            this.univDGVN.Name = "univDGVN";
            this.univDGVN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.univDGVN.Size = new System.Drawing.Size(600, 223);
            this.univDGVN.TabIndex = 1;
            this.univDGVN.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.univDGVN_CellEndEdit);
            this.univDGVN.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.univDGVN_CellEnter);
            this.univDGVN.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.univDGVN_CellMouseClick);
            this.univDGVN.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.univDGVN_CellMouseEnter);
            this.univDGVN.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.univDGVN_CellValidating);
            this.univDGVN.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.univDGVN_DefaultValuesNeeded);
            this.univDGVN.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.univDGVN_EditingControlShowing);
            // 
            // activeColumn
            // 
            this.activeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            this.activeColumn.HeaderText = "Act";
            this.activeColumn.Name = "activeColumn";
            this.activeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.activeColumn.Width = 25;
            // 
            // universeColumn
            // 
            this.universeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.universeColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.universeColumn.HeaderText = "Universe";
            this.universeColumn.MaxInputLength = 5;
            this.universeColumn.Name = "universeColumn";
            this.universeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.universeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.universeColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.universeColumn.Width = 60;
            // 
            // startColumn
            // 
            this.startColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.startColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.startColumn.HeaderText = "Start";
            this.startColumn.MaxInputLength = 5;
            this.startColumn.Name = "startColumn";
            this.startColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.startColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.startColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.startColumn.Width = 60;
            // 
            // sizeColumn
            // 
            this.sizeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.sizeColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.sizeColumn.HeaderText = "Size";
            this.sizeColumn.MaxInputLength = 3;
            this.sizeColumn.Name = "sizeColumn";
            this.sizeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sizeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sizeColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.sizeColumn.Width = 60;
            // 
            // destinationColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.destinationColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.destinationColumn.HeaderText = "Destination";
            this.destinationColumn.Name = "destinationColumn";
            this.destinationColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.destinationColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.destinationColumn.Width = 300;
            // 
            // ttlColumn
            // 
            this.ttlColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ttlColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.ttlColumn.HeaderText = "TTL";
            this.ttlColumn.MaxInputLength = 2;
            this.ttlColumn.Name = "ttlColumn";
            this.ttlColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ttlColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ttlColumn.Width = 30;
            // 
            // warningsCheckBox
            // 
            this.warningsCheckBox.AutoSize = true;
            this.warningsCheckBox.Location = new System.Drawing.Point(616, 10);
            this.warningsCheckBox.Name = "warningsCheckBox";
            this.warningsCheckBox.Size = new System.Drawing.Size(241, 17);
            this.warningsCheckBox.TabIndex = 2;
            this.warningsCheckBox.Text = "Display ALL Warnings/Errors and wait For OK";
            // 
            // statisticsCheckBox
            // 
            this.statisticsCheckBox.AutoSize = true;
            this.statisticsCheckBox.Location = new System.Drawing.Point(616, 33);
            this.statisticsCheckBox.Name = "statisticsCheckBox";
            this.statisticsCheckBox.Size = new System.Drawing.Size(240, 17);
            this.statisticsCheckBox.TabIndex = 3;
            this.statisticsCheckBox.Text = "Gather statistics and display at end of session";
            // 
            // eventRepeatCountTextBox
            // 
            this.eventRepeatCountTextBox.Location = new System.Drawing.Point(616, 56);
            this.eventRepeatCountTextBox.MaxLength = 2;
            this.eventRepeatCountTextBox.Name = "eventRepeatCountTextBox";
            this.eventRepeatCountTextBox.Size = new System.Drawing.Size(30, 20);
            this.eventRepeatCountTextBox.TabIndex = 4;
            this.eventRepeatCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.eventRepeatCountTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumTextBox_KeyPress);
            this.eventRepeatCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.eventRepeatCountTextBox_Validating);
            // 
            // btnValidate
            // 
            this.btnValidate.AutoSize = true;
            this.btnValidate.Location = new System.Drawing.Point(780, 210);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 101;
            this.btnValidate.Text = "&Validate";
            this.btnValidate.Click += new System.EventHandler(this.okButton_Click);
            // 
            // btnSysInfo
            // 
            this.btnSysInfo.Location = new System.Drawing.Point(861, 210);
            this.btnSysInfo.Name = "btnSysInfo";
            this.btnSysInfo.Size = new System.Drawing.Size(75, 23);
            this.btnSysInfo.TabIndex = 103;
            this.btnSysInfo.Text = "Sys Info";
            this.btnSysInfo.UseVisualStyleBackColor = true;
            this.btnSysInfo.Click += new System.EventHandler(this.btnSysInfo_Click);
            // 
            // E131SetupForm
            // 
            this.Controls.Add(this.univDGVN);
            this.Controls.Add(this.btnSysInfo);
            this.Controls.Add(this.warningsCheckBox);
            this.Controls.Add(this.eventRepeatCountTextBox);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.statisticsCheckBox);
            this.Controls.Add(this.label);
            this.Name = "E131SetupForm";
            this.Size = new System.Drawing.Size(952, 236);
            ((System.ComponentModel.ISupportInitialize)(this.univDGVN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private Label label;
        private Button btnSysInfo;
    }
}

