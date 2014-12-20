using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Controllers.E131.Controls;

namespace Controllers.E131 {
    public partial class E131SetupForm {
        private IContainer components;

        #region Windows Form Designer generated code

        private DataGridViewNumbered univDGVN;
        private DataGridViewCellEventArgs univDGVNCellEventArgs = null;
        private DataGridViewCheckBoxColumn activeColumn;
        private DataGridViewTextBoxColumn universeColumn, startColumn, sizeColumn, ttlColumn;
        private DataGridViewComboBoxColumn destinationColumn;
        private CheckBox warningsCheckBox, statisticsCheckBox;
        private TextBox eventRepeatCountTextBox;
        private Button okButton;
        private ContextMenuStrip rowManipulationContextMenuStrip = new ContextMenuStrip();
        private ToolTip destinationToolTip = null;
        private ContextMenuStrip destinationContextMenuStrip = null;


        private void InitializeComponent() {
            this.components = new Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            this.label = new Label();
            this.rowManipulationContextMenuStrip = new ContextMenuStrip(this.components);
            this.univDGVN = new DataGridViewNumbered();
            this.activeColumn = new DataGridViewCheckBoxColumn();
            this.universeColumn = new DataGridViewTextBoxColumn();
            this.startColumn = new DataGridViewTextBoxColumn();
            this.sizeColumn = new DataGridViewTextBoxColumn();
            this.destinationColumn = new DataGridViewComboBoxColumn();
            this.ttlColumn = new DataGridViewTextBoxColumn();
            this.warningsCheckBox = new CheckBox();
            this.statisticsCheckBox = new CheckBox();
            this.eventRepeatCountTextBox = new TextBox();
            this.okButton = new Button();
            this.btnAbout = new Button();
            this.btnSysInfo = new Button();
            ((ISupportInitialize)(this.univDGVN)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Location = new Point(652, 56);
            this.label.Name = "label";
            this.label.Size = new Size(284, 66);
            this.label.TabIndex = 5;
            this.label.Text = "Event Repeat Count: Set to 0 to send all events to each universe, set to > 0 to s" +
    "kip \'x\' events if data is unchanged on a per universe basis.";
            // 
            // rowManipulationContextMenuStrip
            // 
            this.rowManipulationContextMenuStrip.Name = "rowManipulationContextMenuStrip";
            this.rowManipulationContextMenuStrip.Size = new Size(61, 4);
            this.rowManipulationContextMenuStrip.Opening += new CancelEventHandler(this.rowManipulationContextMenuStrip_Opening);
            // 
            // univDGVN
            // 
            this.univDGVN.AllowUserToDeleteRows = false;
            this.univDGVN.BackgroundColor = this.BackColor;
            this.univDGVN.BorderStyle = BorderStyle.None;
            this.univDGVN.Columns.AddRange(new DataGridViewColumn[] {
            this.activeColumn,
            this.universeColumn,
            this.startColumn,
            this.sizeColumn,
            this.destinationColumn,
            this.ttlColumn});
            this.univDGVN.Location = new Point(10, 10);
            this.univDGVN.Name = "univDGVN";
            this.univDGVN.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.univDGVN.Size = new Size(600, 223);
            this.univDGVN.TabIndex = 1;
            this.univDGVN.CellEndEdit += new DataGridViewCellEventHandler(this.univDGVN_CellEndEdit);
            this.univDGVN.CellEnter += new DataGridViewCellEventHandler(this.univDGVN_CellEnter);
            this.univDGVN.CellMouseClick += new DataGridViewCellMouseEventHandler(this.univDGVN_CellMouseClick);
            this.univDGVN.CellMouseEnter += new DataGridViewCellEventHandler(this.univDGVN_CellMouseEnter);
            this.univDGVN.CellValidating += new DataGridViewCellValidatingEventHandler(this.univDGVN_CellValidating);
            this.univDGVN.DefaultValuesNeeded += new DataGridViewRowEventHandler(this.univDGVN_DefaultValuesNeeded);
            this.univDGVN.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.univDGVN_EditingControlShowing);
            // 
            // activeColumn
            // 
            this.activeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            this.activeColumn.HeaderText = "Act";
            this.activeColumn.Name = "activeColumn";
            this.activeColumn.Resizable = DataGridViewTriState.False;
            this.activeColumn.Width = 25;
            // 
            // universeColumn
            // 
            this.universeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.universeColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.universeColumn.HeaderText = "Universe";
            this.universeColumn.MaxInputLength = 5;
            this.universeColumn.Name = "universeColumn";
            this.universeColumn.Resizable = DataGridViewTriState.False;
            this.universeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.universeColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.universeColumn.Width = 60;
            // 
            // startColumn
            // 
            this.startColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.startColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.startColumn.HeaderText = "Start";
            this.startColumn.MaxInputLength = 5;
            this.startColumn.Name = "startColumn";
            this.startColumn.Resizable = DataGridViewTriState.False;
            this.startColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.startColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.startColumn.Width = 60;
            // 
            // sizeColumn
            // 
            this.sizeColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.sizeColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.sizeColumn.HeaderText = "Size";
            this.sizeColumn.MaxInputLength = 3;
            this.sizeColumn.Name = "sizeColumn";
            this.sizeColumn.Resizable = DataGridViewTriState.False;
            this.sizeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.sizeColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.sizeColumn.Width = 60;
            // 
            // destinationColumn
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.destinationColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.destinationColumn.HeaderText = "Destination";
            this.destinationColumn.Name = "destinationColumn";
            this.destinationColumn.Resizable = DataGridViewTriState.False;
            this.destinationColumn.ToolTipText = "Sort (LeftClick = Ascending, RightClick = Descending)";
            this.destinationColumn.Width = 300;
            // 
            // ttlColumn
            // 
            this.ttlColumn.ContextMenuStrip = this.rowManipulationContextMenuStrip;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.ttlColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.ttlColumn.HeaderText = "TTL";
            this.ttlColumn.MaxInputLength = 2;
            this.ttlColumn.Name = "ttlColumn";
            this.ttlColumn.Resizable = DataGridViewTriState.False;
            this.ttlColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ttlColumn.Width = 30;
            // 
            // warningsCheckBox
            // 
            this.warningsCheckBox.AutoSize = true;
            this.warningsCheckBox.Location = new Point(616, 10);
            this.warningsCheckBox.Name = "warningsCheckBox";
            this.warningsCheckBox.Size = new Size(241, 17);
            this.warningsCheckBox.TabIndex = 2;
            this.warningsCheckBox.Text = "Display ALL Warnings/Errors and wait For OK";
            // 
            // statisticsCheckBox
            // 
            this.statisticsCheckBox.AutoSize = true;
            this.statisticsCheckBox.Location = new Point(616, 33);
            this.statisticsCheckBox.Name = "statisticsCheckBox";
            this.statisticsCheckBox.Size = new Size(240, 17);
            this.statisticsCheckBox.TabIndex = 3;
            this.statisticsCheckBox.Text = "Gather statistics and display at end of session";
            // 
            // eventRepeatCountTextBox
            // 
            this.eventRepeatCountTextBox.Location = new Point(616, 56);
            this.eventRepeatCountTextBox.MaxLength = 2;
            this.eventRepeatCountTextBox.Name = "eventRepeatCountTextBox";
            this.eventRepeatCountTextBox.Size = new Size(30, 20);
            this.eventRepeatCountTextBox.TabIndex = 4;
            this.eventRepeatCountTextBox.TextAlign = HorizontalAlignment.Right;
            this.eventRepeatCountTextBox.KeyPress += new KeyPressEventHandler(this.NumTextBox_KeyPress);
            this.eventRepeatCountTextBox.Validating += new CancelEventHandler(this.eventRepeatCountTextBox_Validating);
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.Location = new Point(616, 210);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(75, 23);
            this.okButton.TabIndex = 101;
            this.okButton.Text = "&OK";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new Point(699, 210);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new Size(75, 23);
            this.btnAbout.TabIndex = 102;
            this.btnAbout.Text = "About...";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new EventHandler(this.btnAbout_Click);
            // 
            // btnSysInfo
            // 
            this.btnSysInfo.Location = new Point(780, 210);
            this.btnSysInfo.Name = "btnSysInfo";
            this.btnSysInfo.Size = new Size(75, 23);
            this.btnSysInfo.TabIndex = 103;
            this.btnSysInfo.Text = "Sys Info";
            this.btnSysInfo.UseVisualStyleBackColor = true;
            this.btnSysInfo.Click += new EventHandler(this.btnSysInfo_Click);
            // 
            // E131SetupForm
            // 
            this.Controls.Add(this.univDGVN);
            this.Controls.Add(this.btnSysInfo);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.warningsCheckBox);
            this.Controls.Add(this.eventRepeatCountTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.statisticsCheckBox);
            this.Controls.Add(this.label);
            this.Name = "E131SetupForm";
            this.Size = new Size(952, 236);
            ((ISupportInitialize)(this.univDGVN)).EndInit();
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
        private Button btnAbout;
    }
}

