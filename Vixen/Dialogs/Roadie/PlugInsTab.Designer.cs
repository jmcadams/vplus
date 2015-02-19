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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cbAvailablePlugIns = new System.Windows.Forms.ComboBox();
            this.btnRemovePlugIn = new System.Windows.Forms.Button();
            this.btnAddPlugIn = new System.Windows.Forms.Button();
            this.pSetup = new System.Windows.Forms.Panel();
            this.dgvPlugIns = new System.Windows.Forms.DataGridView();
            this.colPlugInName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlugInEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPlugInStartChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlugInEndChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlugInConfiguration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlugInSetup = new VixenPlus.Dialogs.DataGridViewDisableButtonColumn();
            this.gbSetup = new System.Windows.Forms.GroupBox();
            this.dataGridViewDisableButtonColumn1 = new VixenPlus.Dialogs.DataGridViewDisableButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugIns)).BeginInit();
            this.gbSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAvailablePlugIns
            // 
            this.cbAvailablePlugIns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAvailablePlugIns.FormattingEnabled = true;
            this.cbAvailablePlugIns.Location = new System.Drawing.Point(3, 3);
            this.cbAvailablePlugIns.Name = "cbAvailablePlugIns";
            this.cbAvailablePlugIns.Size = new System.Drawing.Size(280, 21);
            this.cbAvailablePlugIns.TabIndex = 41;
            this.cbAvailablePlugIns.SelectedIndexChanged += new System.EventHandler(this.cbAvailablePlugIns_SelectedIndexChanged);
            // 
            // btnRemovePlugIn
            // 
            this.btnRemovePlugIn.Enabled = false;
            this.btnRemovePlugIn.Location = new System.Drawing.Point(532, 2);
            this.btnRemovePlugIn.Name = "btnRemovePlugIn";
            this.btnRemovePlugIn.Size = new System.Drawing.Size(75, 23);
            this.btnRemovePlugIn.TabIndex = 38;
            this.btnRemovePlugIn.Text = "Remove";
            this.btnRemovePlugIn.UseVisualStyleBackColor = true;
            this.btnRemovePlugIn.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // btnAddPlugIn
            // 
            this.btnAddPlugIn.Enabled = false;
            this.btnAddPlugIn.Location = new System.Drawing.Point(289, 2);
            this.btnAddPlugIn.Name = "btnAddPlugIn";
            this.btnAddPlugIn.Size = new System.Drawing.Size(75, 23);
            this.btnAddPlugIn.TabIndex = 37;
            this.btnAddPlugIn.Text = "Add";
            this.btnAddPlugIn.UseVisualStyleBackColor = true;
            this.btnAddPlugIn.Click += new System.EventHandler(this.buttonUse_Click);
            // 
            // pSetup
            // 
            this.pSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pSetup.Location = new System.Drawing.Point(6, 19);
            this.pSetup.Name = "pSetup";
            this.pSetup.Size = new System.Drawing.Size(929, 236);
            this.pSetup.TabIndex = 0;
            // 
            // dgvPlugIns
            // 
            this.dgvPlugIns.AllowUserToAddRows = false;
            this.dgvPlugIns.AllowUserToDeleteRows = false;
            this.dgvPlugIns.AllowUserToOrderColumns = true;
            this.dgvPlugIns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlugIns.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPlugIns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlugIns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPlugInName,
            this.colPlugInEnabled,
            this.colPlugInStartChannel,
            this.colPlugInEndChannel,
            this.colPlugInConfiguration,
            this.colPlugInSetup});
            this.dgvPlugIns.Location = new System.Drawing.Point(2, 32);
            this.dgvPlugIns.MultiSelect = false;
            this.dgvPlugIns.Name = "dgvPlugIns";
            this.dgvPlugIns.RowHeadersVisible = false;
            this.dgvPlugIns.Size = new System.Drawing.Size(941, 234);
            this.dgvPlugIns.TabIndex = 40;
            this.dgvPlugIns.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlugIns_CellClick);
            this.dgvPlugIns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlugIns_CellValueChanged);
            this.dgvPlugIns.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlugIns_RowEnter);
            this.dgvPlugIns.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlugIns_RowLeave);
            this.dgvPlugIns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DoPluginsKeys);
            // 
            // colPlugInName
            // 
            this.colPlugInName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colPlugInName.FillWeight = 150F;
            this.colPlugInName.HeaderText = "Plug In Name";
            this.colPlugInName.Name = "colPlugInName";
            this.colPlugInName.ReadOnly = true;
            this.colPlugInName.Width = 88;
            // 
            // colPlugInEnabled
            // 
            this.colPlugInEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
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
            this.colPlugInConfiguration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colPlugInConfiguration.DefaultCellStyle = dataGridViewCellStyle1;
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
            this.gbSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSetup.Controls.Add(this.pSetup);
            this.gbSetup.Location = new System.Drawing.Point(2, 272);
            this.gbSetup.Name = "gbSetup";
            this.gbSetup.Size = new System.Drawing.Size(941, 261);
            this.gbSetup.TabIndex = 39;
            this.gbSetup.TabStop = false;
            this.gbSetup.Text = "Inline Setup";
            // 
            // dataGridViewDisableButtonColumn1
            // 
            this.dataGridViewDisableButtonColumn1.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
            this.dataGridViewDisableButtonColumn1.ReadOnly = true;
            this.dataGridViewDisableButtonColumn1.Text = "Setup";
            // 
            // PlugInsTab
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.cbAvailablePlugIns);
            this.Controls.Add(this.btnRemovePlugIn);
            this.Controls.Add(this.btnAddPlugIn);
            this.Controls.Add(this.dgvPlugIns);
            this.Controls.Add(this.gbSetup);
            this.Name = "PlugInsTab";
            this.Size = new System.Drawing.Size(957, 562);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugIns)).EndInit();
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
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
    }
}