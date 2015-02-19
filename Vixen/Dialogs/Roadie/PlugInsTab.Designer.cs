using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    partial class PlugInsTab {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            this.cbAvailablePlugIns = new ComboBox();
            this.btnRemovePlugIn = new Button();
            this.btnAddPlugIn = new Button();
            this.pSetup = new Panel();
            this.dgvPlugIns = new DataGridView();
            this.colPlugInName = new DataGridViewTextBoxColumn();
            this.colPlugInEnabled = new DataGridViewCheckBoxColumn();
            this.colPlugInStartChannel = new DataGridViewTextBoxColumn();
            this.colPlugInEndChannel = new DataGridViewTextBoxColumn();
            this.colPlugInConfiguration = new DataGridViewTextBoxColumn();
            this.colPlugInSetup = new DataGridViewDisableButtonColumn();
            this.gbSetup = new GroupBox();
            ((ISupportInitialize)(this.dgvPlugIns)).BeginInit();
            this.gbSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAvailablePlugIns
            // 
            this.cbAvailablePlugIns.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAvailablePlugIns.FormattingEnabled = true;
            this.cbAvailablePlugIns.Location = new Point(3, 3);
            this.cbAvailablePlugIns.Name = "cbAvailablePlugIns";
            this.cbAvailablePlugIns.Size = new Size(280, 21);
            this.cbAvailablePlugIns.TabIndex = 41;
            this.cbAvailablePlugIns.SelectedIndexChanged += new EventHandler(this.cbAvailablePlugIns_SelectedIndexChanged);
            // 
            // btnRemovePlugIn
            // 
            this.btnRemovePlugIn.Enabled = false;
            this.btnRemovePlugIn.Location = new Point(532, 2);
            this.btnRemovePlugIn.Name = "btnRemovePlugIn";
            this.btnRemovePlugIn.Size = new Size(75, 23);
            this.btnRemovePlugIn.TabIndex = 38;
            this.btnRemovePlugIn.Text = "Remove";
            this.btnRemovePlugIn.UseVisualStyleBackColor = true;
            this.btnRemovePlugIn.Click += new EventHandler(this.buttonRemove_Click);
            // 
            // btnAddPlugIn
            // 
            this.btnAddPlugIn.Enabled = false;
            this.btnAddPlugIn.Location = new Point(289, 2);
            this.btnAddPlugIn.Name = "btnAddPlugIn";
            this.btnAddPlugIn.Size = new Size(75, 23);
            this.btnAddPlugIn.TabIndex = 37;
            this.btnAddPlugIn.Text = "Add";
            this.btnAddPlugIn.UseVisualStyleBackColor = true;
            this.btnAddPlugIn.Click += new EventHandler(this.buttonUse_Click);
            // 
            // pSetup
            // 
            this.pSetup.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.pSetup.Location = new Point(6, 19);
            this.pSetup.Name = "pSetup";
            this.pSetup.Size = new Size(929, 236);
            this.pSetup.TabIndex = 0;
            // 
            // dgvPlugIns
            // 
            this.dgvPlugIns.AllowUserToAddRows = false;
            this.dgvPlugIns.AllowUserToDeleteRows = false;
            this.dgvPlugIns.AllowUserToOrderColumns = true;
            this.dgvPlugIns.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.dgvPlugIns.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPlugIns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlugIns.Columns.AddRange(new DataGridViewColumn[] {
            this.colPlugInName,
            this.colPlugInEnabled,
            this.colPlugInStartChannel,
            this.colPlugInEndChannel,
            this.colPlugInConfiguration,
            this.colPlugInSetup});
            this.dgvPlugIns.Location = new Point(2, 32);
            this.dgvPlugIns.MultiSelect = false;
            this.dgvPlugIns.Name = "dgvPlugIns";
            this.dgvPlugIns.RowHeadersVisible = false;
            this.dgvPlugIns.Size = new Size(941, 234);
            this.dgvPlugIns.TabIndex = 40;
            this.dgvPlugIns.CellClick += new DataGridViewCellEventHandler(this.dgvPlugIns_CellClick);
            this.dgvPlugIns.CellValueChanged += new DataGridViewCellEventHandler(this.dgvPlugIns_CellValueChanged);
            this.dgvPlugIns.RowEnter += new DataGridViewCellEventHandler(this.dgvPlugIns_RowEnter);
            this.dgvPlugIns.RowLeave += new DataGridViewCellEventHandler(this.dgvPlugIns_RowLeave);
            // 
            // colPlugInName
            // 
            this.colPlugInName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colPlugInName.FillWeight = 150F;
            this.colPlugInName.HeaderText = "Plug In Name";
            this.colPlugInName.Name = "colPlugInName";
            this.colPlugInName.ReadOnly = true;
            this.colPlugInName.Width = 88;
            // 
            // colPlugInEnabled
            // 
            this.colPlugInEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPlugInEnabled.HeaderText = "Enabled";
            this.colPlugInEnabled.Name = "colPlugInEnabled";
            this.colPlugInEnabled.Width = 52;
            // 
            // colPlugInStartChannel
            // 
            this.colPlugInStartChannel.HeaderText = "Start Channel";
            this.colPlugInStartChannel.Name = "colPlugInStartChannel";
            // 
            // colPlugInEndChannel
            // 
            this.colPlugInEndChannel.HeaderText = "End Channel";
            this.colPlugInEndChannel.Name = "colPlugInEndChannel";
            // 
            // colPlugInConfiguration
            // 
            this.colPlugInConfiguration.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            this.colPlugInConfiguration.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPlugInConfiguration.HeaderText = "Current Configuration";
            this.colPlugInConfiguration.Name = "colPlugInConfiguration";
            this.colPlugInConfiguration.ReadOnly = true;
            // 
            // colPlugInSetup
            // 
            this.colPlugInSetup.HeaderText = "Setup";
            this.colPlugInSetup.Name = "colPlugInSetup";
            this.colPlugInSetup.ReadOnly = true;
            this.colPlugInSetup.Text = "Setup";
            // 
            // gbSetup
            // 
            this.gbSetup.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.gbSetup.Controls.Add(this.pSetup);
            this.gbSetup.Location = new Point(2, 272);
            this.gbSetup.Name = "gbSetup";
            this.gbSetup.Size = new Size(941, 261);
            this.gbSetup.TabIndex = 39;
            this.gbSetup.TabStop = false;
            this.gbSetup.Text = "Inline Setup";
            // 
            // PlugInDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.cbAvailablePlugIns);
            this.Controls.Add(this.btnRemovePlugIn);
            this.Controls.Add(this.btnAddPlugIn);
            this.Controls.Add(this.dgvPlugIns);
            this.Controls.Add(this.gbSetup);
            this.Name = "PlugInsTab";
            this.Size = new Size(957, 562);
            ((ISupportInitialize)(this.dgvPlugIns)).EndInit();
            this.gbSetup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox cbAvailablePlugIns;
        private Button btnRemovePlugIn;
        private Button btnAddPlugIn;
        private Panel pSetup;
        private DataGridView dgvPlugIns;
        private DataGridViewTextBoxColumn colPlugInName;
        private DataGridViewCheckBoxColumn colPlugInEnabled;
        private DataGridViewTextBoxColumn colPlugInStartChannel;
        private DataGridViewTextBoxColumn colPlugInEndChannel;
        private DataGridViewTextBoxColumn colPlugInConfiguration;
        private DataGridViewDisableButtonColumn colPlugInSetup;
        private GroupBox gbSetup;
    }
}