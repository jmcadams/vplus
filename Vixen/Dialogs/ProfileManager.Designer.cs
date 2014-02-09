namespace VixenPlus.Dialogs
{
    partial class FrmProfileManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.gbProfiles = new System.Windows.Forms.GroupBox();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnRenameProfile = new System.Windows.Forms.Button();
            this.btnCopyProfile = new System.Windows.Forms.Button();
            this.btnAddProfile = new System.Windows.Forms.Button();
            this.tcProfile = new System.Windows.Forms.TabControl();
            this.tpChannels = new System.Windows.Forms.TabPage();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgvChannels = new System.Windows.Forms.DataGridView();
            this.ChannelEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChannelNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnChannelColors = new System.Windows.Forms.Button();
            this.btnChannelMask = new System.Windows.Forms.Button();
            this.btnChannelOutputs = new System.Windows.Forms.Button();
            this.btnAddProfileChannel = new System.Windows.Forms.Button();
            this.btnRemoveProfileChannels = new System.Windows.Forms.Button();
            this.lblAddMultiChannels = new System.Windows.Forms.Label();
            this.tbProfileChannelCount = new System.Windows.Forms.TextBox();
            this.btnAddMultipleProfileChannels = new System.Windows.Forms.Button();
            this.tpPlugins = new System.Windows.Forms.TabPage();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.tpSortOrders = new System.Windows.Forms.TabPage();
            this.btnDeleteChannelOrder = new System.Windows.Forms.Button();
            this.btnSaveChannelOrder = new System.Windows.Forms.Button();
            this.cbChannelOrder = new System.Windows.Forms.ComboBox();
            this.gbProfiles.SuspendLayout();
            this.tcProfile.SuspendLayout();
            this.tpChannels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.tpSortOrders.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProfiles
            // 
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(10, 19);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(281, 21);
            this.cbProfiles.TabIndex = 0;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // gbProfiles
            // 
            this.gbProfiles.Controls.Add(this.btnDeleteProfile);
            this.gbProfiles.Controls.Add(this.btnRenameProfile);
            this.gbProfiles.Controls.Add(this.btnCopyProfile);
            this.gbProfiles.Controls.Add(this.btnAddProfile);
            this.gbProfiles.Controls.Add(this.cbProfiles);
            this.gbProfiles.Location = new System.Drawing.Point(12, 12);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(621, 52);
            this.gbProfiles.TabIndex = 1;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Profile";
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(540, 19);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteProfile.TabIndex = 4;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
            // 
            // btnRenameProfile
            // 
            this.btnRenameProfile.Location = new System.Drawing.Point(378, 19);
            this.btnRenameProfile.Name = "btnRenameProfile";
            this.btnRenameProfile.Size = new System.Drawing.Size(75, 23);
            this.btnRenameProfile.TabIndex = 3;
            this.btnRenameProfile.Text = "Rename";
            this.btnRenameProfile.UseVisualStyleBackColor = true;
            this.btnRenameProfile.Click += new System.EventHandler(this.btnRenameProfile_Click);
            // 
            // btnCopyProfile
            // 
            this.btnCopyProfile.Location = new System.Drawing.Point(459, 19);
            this.btnCopyProfile.Name = "btnCopyProfile";
            this.btnCopyProfile.Size = new System.Drawing.Size(75, 23);
            this.btnCopyProfile.TabIndex = 2;
            this.btnCopyProfile.Text = "Copy";
            this.btnCopyProfile.UseVisualStyleBackColor = true;
            this.btnCopyProfile.Click += new System.EventHandler(this.btnCopyProfile_Click);
            // 
            // btnAddProfile
            // 
            this.btnAddProfile.Location = new System.Drawing.Point(297, 19);
            this.btnAddProfile.Name = "btnAddProfile";
            this.btnAddProfile.Size = new System.Drawing.Size(75, 23);
            this.btnAddProfile.TabIndex = 1;
            this.btnAddProfile.Text = "Add";
            this.btnAddProfile.UseVisualStyleBackColor = true;
            this.btnAddProfile.Click += new System.EventHandler(this.btnAddProfile_Click);
            // 
            // tcProfile
            // 
            this.tcProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProfile.Controls.Add(this.tpChannels);
            this.tcProfile.Controls.Add(this.tpPlugins);
            this.tcProfile.Controls.Add(this.tpGroups);
            this.tcProfile.Controls.Add(this.tpSortOrders);
            this.tcProfile.HotTrack = true;
            this.tcProfile.Location = new System.Drawing.Point(12, 70);
            this.tcProfile.Name = "tcProfile";
            this.tcProfile.SelectedIndex = 0;
            this.tcProfile.Size = new System.Drawing.Size(760, 480);
            this.tcProfile.TabIndex = 2;
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.btnImport);
            this.tpChannels.Controls.Add(this.btnExport);
            this.tpChannels.Controls.Add(this.dgvChannels);
            this.tpChannels.Controls.Add(this.btnChannelColors);
            this.tpChannels.Controls.Add(this.btnChannelMask);
            this.tpChannels.Controls.Add(this.btnChannelOutputs);
            this.tpChannels.Controls.Add(this.btnAddProfileChannel);
            this.tpChannels.Controls.Add(this.btnRemoveProfileChannels);
            this.tpChannels.Controls.Add(this.lblAddMultiChannels);
            this.tpChannels.Controls.Add(this.tbProfileChannelCount);
            this.tpChannels.Controls.Add(this.btnAddMultipleProfileChannels);
            this.tpChannels.Location = new System.Drawing.Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannels.Size = new System.Drawing.Size(752, 454);
            this.tpChannels.TabIndex = 0;
            this.tpChannels.Text = "Channels";
            this.tpChannels.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(669, 35);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import CSV";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(669, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgvChannels
            // 
            this.dgvChannels.AllowUserToAddRows = false;
            this.dgvChannels.AllowUserToDeleteRows = false;
            this.dgvChannels.AllowUserToOrderColumns = true;
            this.dgvChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelEnabled,
            this.ChannelNum,
            this.ChannelName,
            this.OutputChannel,
            this.Color});
            this.dgvChannels.Location = new System.Drawing.Point(6, 6);
            this.dgvChannels.Name = "dgvChannels";
            this.dgvChannels.RowHeadersWidth = 25;
            this.dgvChannels.Size = new System.Drawing.Size(517, 442);
            this.dgvChannels.TabIndex = 0;
            // 
            // ChannelEnabled
            // 
            this.ChannelEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelEnabled.HeaderText = "Enabled";
            this.ChannelEnabled.Name = "ChannelEnabled";
            this.ChannelEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ChannelEnabled.Width = 71;
            // 
            // ChannelNum
            // 
            this.ChannelNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelNum.HeaderText = "Ch #";
            this.ChannelNum.Name = "ChannelNum";
            this.ChannelNum.ReadOnly = true;
            this.ChannelNum.Width = 55;
            // 
            // ChannelName
            // 
            this.ChannelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChannelName.HeaderText = "Name";
            this.ChannelName.Name = "ChannelName";
            // 
            // OutputChannel
            // 
            this.OutputChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OutputChannel.HeaderText = "Mapped to";
            this.OutputChannel.Name = "OutputChannel";
            this.OutputChannel.ReadOnly = true;
            this.OutputChannel.Width = 83;
            // 
            // Color
            // 
            this.Color.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Color.HeaderText = "Color";
            this.Color.Name = "Color";
            this.Color.ReadOnly = true;
            this.Color.Width = 56;
            // 
            // btnChannelColors
            // 
            this.btnChannelColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChannelColors.Location = new System.Drawing.Point(542, 64);
            this.btnChannelColors.Name = "btnChannelColors";
            this.btnChannelColors.Size = new System.Drawing.Size(75, 23);
            this.btnChannelColors.TabIndex = 3;
            this.btnChannelColors.Text = "Ch. Colors";
            this.btnChannelColors.UseVisualStyleBackColor = true;
            // 
            // btnChannelMask
            // 
            this.btnChannelMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChannelMask.Location = new System.Drawing.Point(542, 35);
            this.btnChannelMask.Name = "btnChannelMask";
            this.btnChannelMask.Size = new System.Drawing.Size(75, 23);
            this.btnChannelMask.TabIndex = 2;
            this.btnChannelMask.Text = "Ch. Mask";
            this.btnChannelMask.UseVisualStyleBackColor = true;
            // 
            // btnChannelOutputs
            // 
            this.btnChannelOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChannelOutputs.Location = new System.Drawing.Point(542, 6);
            this.btnChannelOutputs.Name = "btnChannelOutputs";
            this.btnChannelOutputs.Size = new System.Drawing.Size(75, 23);
            this.btnChannelOutputs.TabIndex = 1;
            this.btnChannelOutputs.Text = "Ch. Outputs";
            this.btnChannelOutputs.UseVisualStyleBackColor = true;
            // 
            // btnAddProfileChannel
            // 
            this.btnAddProfileChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddProfileChannel.Location = new System.Drawing.Point(542, 93);
            this.btnAddProfileChannel.Name = "btnAddProfileChannel";
            this.btnAddProfileChannel.Size = new System.Drawing.Size(111, 23);
            this.btnAddProfileChannel.TabIndex = 4;
            this.btnAddProfileChannel.Text = "Add one channel";
            this.btnAddProfileChannel.UseVisualStyleBackColor = true;
            // 
            // btnRemoveProfileChannels
            // 
            this.btnRemoveProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveProfileChannels.Enabled = false;
            this.btnRemoveProfileChannels.Location = new System.Drawing.Point(542, 153);
            this.btnRemoveProfileChannels.Name = "btnRemoveProfileChannels";
            this.btnRemoveProfileChannels.Size = new System.Drawing.Size(111, 23);
            this.btnRemoveProfileChannels.TabIndex = 7;
            this.btnRemoveProfileChannels.Text = "Remove Channel";
            this.btnRemoveProfileChannels.UseVisualStyleBackColor = true;
            // 
            // lblAddMultiChannels
            // 
            this.lblAddMultiChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddMultiChannels.AutoSize = true;
            this.lblAddMultiChannels.Location = new System.Drawing.Point(649, 129);
            this.lblAddMultiChannels.Name = "lblAddMultiChannels";
            this.lblAddMultiChannels.Size = new System.Drawing.Size(50, 13);
            this.lblAddMultiChannels.TabIndex = 10;
            this.lblAddMultiChannels.Text = "channels";
            // 
            // tbProfileChannelCount
            // 
            this.tbProfileChannelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProfileChannelCount.Location = new System.Drawing.Point(605, 126);
            this.tbProfileChannelCount.Name = "tbProfileChannelCount";
            this.tbProfileChannelCount.Size = new System.Drawing.Size(38, 20);
            this.tbProfileChannelCount.TabIndex = 6;
            this.tbProfileChannelCount.Text = "10";
            // 
            // btnAddMultipleProfileChannels
            // 
            this.btnAddMultipleProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMultipleProfileChannels.Location = new System.Drawing.Point(542, 124);
            this.btnAddMultipleProfileChannels.Name = "btnAddMultipleProfileChannels";
            this.btnAddMultipleProfileChannels.Size = new System.Drawing.Size(57, 23);
            this.btnAddMultipleProfileChannels.TabIndex = 5;
            this.btnAddMultipleProfileChannels.Text = "Add";
            this.btnAddMultipleProfileChannels.UseVisualStyleBackColor = true;
            // 
            // tpPlugins
            // 
            this.tpPlugins.Location = new System.Drawing.Point(4, 22);
            this.tpPlugins.Name = "tpPlugins";
            this.tpPlugins.Padding = new System.Windows.Forms.Padding(3);
            this.tpPlugins.Size = new System.Drawing.Size(752, 454);
            this.tpPlugins.TabIndex = 1;
            this.tpPlugins.Text = "Plugins";
            this.tpPlugins.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(685, 12);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(685, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tpGroups
            // 
            this.tpGroups.Location = new System.Drawing.Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Size = new System.Drawing.Size(752, 454);
            this.tpGroups.TabIndex = 2;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // tpSortOrders
            // 
            this.tpSortOrders.Controls.Add(this.btnDeleteChannelOrder);
            this.tpSortOrders.Controls.Add(this.btnSaveChannelOrder);
            this.tpSortOrders.Controls.Add(this.cbChannelOrder);
            this.tpSortOrders.Location = new System.Drawing.Point(4, 22);
            this.tpSortOrders.Name = "tpSortOrders";
            this.tpSortOrders.Size = new System.Drawing.Size(752, 454);
            this.tpSortOrders.TabIndex = 3;
            this.tpSortOrders.Text = "Sort Orders";
            this.tpSortOrders.UseVisualStyleBackColor = true;
            // 
            // btnDeleteChannelOrder
            // 
            this.btnDeleteChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteChannelOrder.Location = new System.Drawing.Point(674, 32);
            this.btnDeleteChannelOrder.Name = "btnDeleteChannelOrder";
            this.btnDeleteChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteChannelOrder.TabIndex = 65;
            this.btnDeleteChannelOrder.Text = "Delete Order";
            this.btnDeleteChannelOrder.UseVisualStyleBackColor = true;
            // 
            // btnSaveChannelOrder
            // 
            this.btnSaveChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChannelOrder.Location = new System.Drawing.Point(674, 3);
            this.btnSaveChannelOrder.Name = "btnSaveChannelOrder";
            this.btnSaveChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChannelOrder.TabIndex = 64;
            this.btnSaveChannelOrder.Text = "Save Order";
            this.btnSaveChannelOrder.UseVisualStyleBackColor = true;
            // 
            // cbChannelOrder
            // 
            this.cbChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbChannelOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannelOrder.FormattingEnabled = true;
            this.cbChannelOrder.Items.AddRange(new object[] {
            "Define new order...",
            "Restore natural order..."});
            this.cbChannelOrder.Location = new System.Drawing.Point(547, 5);
            this.cbChannelOrder.Name = "cbChannelOrder";
            this.cbChannelOrder.Size = new System.Drawing.Size(121, 21);
            this.cbChannelOrder.TabIndex = 63;
            // 
            // FrmProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.tcProfile);
            this.Controls.Add(this.gbProfiles);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmProfileManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProfileManager";
            this.Resize += new System.EventHandler(this.frmProfileManager_Resize);
            this.gbProfiles.ResumeLayout(false);
            this.tcProfile.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            this.tpChannels.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).EndInit();
            this.tpSortOrders.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.GroupBox gbProfiles;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Button btnRenameProfile;
        private System.Windows.Forms.Button btnCopyProfile;
        private System.Windows.Forms.Button btnAddProfile;
        private System.Windows.Forms.TabControl tcProfile;
        private System.Windows.Forms.TabPage tpChannels;
        private System.Windows.Forms.TabPage tpPlugins;
        private System.Windows.Forms.DataGridView dgvChannels;
        private System.Windows.Forms.Button btnChannelColors;
        private System.Windows.Forms.Button btnChannelMask;
        private System.Windows.Forms.Button btnChannelOutputs;
        private System.Windows.Forms.Button btnAddProfileChannel;
        private System.Windows.Forms.Button btnRemoveProfileChannels;
        private System.Windows.Forms.Label lblAddMultiChannels;
        private System.Windows.Forms.TextBox tbProfileChannelCount;
        private System.Windows.Forms.Button btnAddMultipleProfileChannels;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChannelEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Color;
        private System.Windows.Forms.TabPage tpGroups;
        private System.Windows.Forms.TabPage tpSortOrders;
        private System.Windows.Forms.Button btnDeleteChannelOrder;
        private System.Windows.Forms.Button btnSaveChannelOrder;
        private System.Windows.Forms.ComboBox cbChannelOrder;
    }
}