namespace VixenPlus.Dialogs
{
    partial class ProfileManager
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNewProfile = new System.Windows.Forms.Button();
            this.btnCopyProfile = new System.Windows.Forms.Button();
            this.btnRenameProfile = new System.Windows.Forms.Button();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnChannelColors = new System.Windows.Forms.Button();
            this.btnChannelMask = new System.Windows.Forms.Button();
            this.btnChannelOutputs = new System.Windows.Forms.Button();
            this.btnDeleteChannelOrder = new System.Windows.Forms.Button();
            this.btnSaveChannelOrder = new System.Windows.Forms.Button();
            this.comboBoxChannelOrder = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.treeViewProfile = new System.Windows.Forms.TreeView();
            this.buttonAddProfileChannel = new System.Windows.Forms.Button();
            this.buttonRemoveProfileChannels = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxProfileChannelCount = new System.Windows.Forms.TextBox();
            this.buttonAddMultipleProfileChannels = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ChannelEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(181, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(223, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCurrent);
            this.groupBox1.Controls.Add(this.btnDeleteProfile);
            this.groupBox1.Controls.Add(this.btnRenameProfile);
            this.groupBox1.Controls.Add(this.btnCopyProfile);
            this.groupBox1.Controls.Add(this.btnNewProfile);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 52);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile";
            // 
            // btnNewProfile
            // 
            this.btnNewProfile.Location = new System.Drawing.Point(598, 17);
            this.btnNewProfile.Name = "btnNewProfile";
            this.btnNewProfile.Size = new System.Drawing.Size(75, 23);
            this.btnNewProfile.TabIndex = 1;
            this.btnNewProfile.Text = "New Profile";
            this.btnNewProfile.UseVisualStyleBackColor = true;
            // 
            // btnCopyProfile
            // 
            this.btnCopyProfile.Location = new System.Drawing.Point(491, 17);
            this.btnCopyProfile.Name = "btnCopyProfile";
            this.btnCopyProfile.Size = new System.Drawing.Size(75, 23);
            this.btnCopyProfile.TabIndex = 2;
            this.btnCopyProfile.Text = "Copy";
            this.btnCopyProfile.UseVisualStyleBackColor = true;
            // 
            // btnRenameProfile
            // 
            this.btnRenameProfile.Location = new System.Drawing.Point(410, 17);
            this.btnRenameProfile.Name = "btnRenameProfile";
            this.btnRenameProfile.Size = new System.Drawing.Size(75, 23);
            this.btnRenameProfile.TabIndex = 3;
            this.btnRenameProfile.Text = "Rename";
            this.btnRenameProfile.UseVisualStyleBackColor = true;
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(679, 17);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteProfile.TabIndex = 4;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(6, 22);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(169, 13);
            this.lblCurrent.TabIndex = 5;
            this.lblCurrent.Text = "Currently displaying information for:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 480);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.btnChannelColors);
            this.tabPage1.Controls.Add(this.btnChannelMask);
            this.tabPage1.Controls.Add(this.btnChannelOutputs);
            this.tabPage1.Controls.Add(this.btnDeleteChannelOrder);
            this.tabPage1.Controls.Add(this.btnSaveChannelOrder);
            this.tabPage1.Controls.Add(this.comboBoxChannelOrder);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.treeViewProfile);
            this.tabPage1.Controls.Add(this.buttonAddProfileChannel);
            this.tabPage1.Controls.Add(this.buttonRemoveProfileChannels);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.textBoxProfileChannelCount);
            this.tabPage1.Controls.Add(this.buttonAddMultipleProfileChannels);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 454);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Channels";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(752, 454);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plugins";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnChannelColors
            // 
            this.btnChannelColors.Location = new System.Drawing.Point(542, 66);
            this.btnChannelColors.Name = "btnChannelColors";
            this.btnChannelColors.Size = new System.Drawing.Size(75, 23);
            this.btnChannelColors.TabIndex = 61;
            this.btnChannelColors.Text = "Ch. Colors";
            this.btnChannelColors.UseVisualStyleBackColor = true;
            // 
            // btnChannelMask
            // 
            this.btnChannelMask.Location = new System.Drawing.Point(542, 36);
            this.btnChannelMask.Name = "btnChannelMask";
            this.btnChannelMask.Size = new System.Drawing.Size(75, 23);
            this.btnChannelMask.TabIndex = 60;
            this.btnChannelMask.Text = "Ch. Mask";
            this.btnChannelMask.UseVisualStyleBackColor = true;
            // 
            // btnChannelOutputs
            // 
            this.btnChannelOutputs.Location = new System.Drawing.Point(542, 6);
            this.btnChannelOutputs.Name = "btnChannelOutputs";
            this.btnChannelOutputs.Size = new System.Drawing.Size(75, 23);
            this.btnChannelOutputs.TabIndex = 59;
            this.btnChannelOutputs.Text = "Ch. Outputs";
            this.btnChannelOutputs.UseVisualStyleBackColor = true;
            // 
            // btnDeleteChannelOrder
            // 
            this.btnDeleteChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteChannelOrder.Location = new System.Drawing.Point(669, 222);
            this.btnDeleteChannelOrder.Name = "btnDeleteChannelOrder";
            this.btnDeleteChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteChannelOrder.TabIndex = 58;
            this.btnDeleteChannelOrder.Text = "Delete Order";
            this.btnDeleteChannelOrder.UseVisualStyleBackColor = true;
            // 
            // btnSaveChannelOrder
            // 
            this.btnSaveChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChannelOrder.Location = new System.Drawing.Point(669, 193);
            this.btnSaveChannelOrder.Name = "btnSaveChannelOrder";
            this.btnSaveChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChannelOrder.TabIndex = 57;
            this.btnSaveChannelOrder.Text = "Save Order";
            this.btnSaveChannelOrder.UseVisualStyleBackColor = true;
            // 
            // comboBoxChannelOrder
            // 
            this.comboBoxChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChannelOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannelOrder.FormattingEnabled = true;
            this.comboBoxChannelOrder.Items.AddRange(new object[] {
            "Define new order...",
            "Restore natural order..."});
            this.comboBoxChannelOrder.Location = new System.Drawing.Point(542, 195);
            this.comboBoxChannelOrder.Name = "comboBoxChannelOrder";
            this.comboBoxChannelOrder.Size = new System.Drawing.Size(121, 21);
            this.comboBoxChannelOrder.TabIndex = 51;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(539, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Channel order";
            // 
            // treeViewProfile
            // 
            this.treeViewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewProfile.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewProfile.Location = new System.Drawing.Point(542, 251);
            this.treeViewProfile.Name = "treeViewProfile";
            this.treeViewProfile.Size = new System.Drawing.Size(202, 197);
            this.treeViewProfile.TabIndex = 55;
            // 
            // buttonAddProfileChannel
            // 
            this.buttonAddProfileChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddProfileChannel.Location = new System.Drawing.Point(542, 95);
            this.buttonAddProfileChannel.Name = "buttonAddProfileChannel";
            this.buttonAddProfileChannel.Size = new System.Drawing.Size(111, 23);
            this.buttonAddProfileChannel.TabIndex = 45;
            this.buttonAddProfileChannel.Text = "Add one channel";
            this.buttonAddProfileChannel.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveProfileChannels
            // 
            this.buttonRemoveProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveProfileChannels.Enabled = false;
            this.buttonRemoveProfileChannels.Location = new System.Drawing.Point(542, 153);
            this.buttonRemoveProfileChannels.Name = "buttonRemoveProfileChannels";
            this.buttonRemoveProfileChannels.Size = new System.Drawing.Size(111, 23);
            this.buttonRemoveProfileChannels.TabIndex = 49;
            this.buttonRemoveProfileChannels.Text = "Remove Channel";
            this.buttonRemoveProfileChannels.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(649, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "channels";
            // 
            // textBoxProfileChannelCount
            // 
            this.textBoxProfileChannelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProfileChannelCount.Location = new System.Drawing.Point(605, 126);
            this.textBoxProfileChannelCount.Name = "textBoxProfileChannelCount";
            this.textBoxProfileChannelCount.Size = new System.Drawing.Size(38, 20);
            this.textBoxProfileChannelCount.TabIndex = 47;
            this.textBoxProfileChannelCount.Text = "10";
            // 
            // buttonAddMultipleProfileChannels
            // 
            this.buttonAddMultipleProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddMultipleProfileChannels.Location = new System.Drawing.Point(542, 124);
            this.buttonAddMultipleProfileChannels.Name = "buttonAddMultipleProfileChannels";
            this.buttonAddMultipleProfileChannels.Size = new System.Drawing.Size(57, 23);
            this.buttonAddMultipleProfileChannels.TabIndex = 46;
            this.buttonAddMultipleProfileChannels.Text = "Add";
            this.buttonAddMultipleProfileChannels.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelEnabled,
            this.ChannelName,
            this.OutputChannel});
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(517, 442);
            this.dataGridView1.TabIndex = 62;
            // 
            // Enabled
            // 
            this.ChannelEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ChannelEnabled.HeaderText = "Enabled";
            this.ChannelEnabled.Name = "Enabled";
            this.ChannelEnabled.Width = 52;
            // 
            // ChannelName
            // 
            this.ChannelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelName.HeaderText = "Name";
            this.ChannelName.Name = "ChannelName";
            this.ChannelName.Width = 60;
            // 
            // OutputChannel
            // 
            this.OutputChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.OutputChannel.HeaderText = "Mapped to";
            this.OutputChannel.Name = "OutputChannel";
            this.OutputChannel.Width = 83;
            // 
            // ProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProfileManager";
            this.Text = "ProfileManager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Button btnRenameProfile;
        private System.Windows.Forms.Button btnCopyProfile;
        private System.Windows.Forms.Button btnNewProfile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChannelEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputChannel;
        private System.Windows.Forms.Button btnChannelColors;
        private System.Windows.Forms.Button btnChannelMask;
        private System.Windows.Forms.Button btnChannelOutputs;
        private System.Windows.Forms.Button btnDeleteChannelOrder;
        private System.Windows.Forms.Button btnSaveChannelOrder;
        private System.Windows.Forms.ComboBox comboBoxChannelOrder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TreeView treeViewProfile;
        private System.Windows.Forms.Button buttonAddProfileChannel;
        private System.Windows.Forms.Button buttonRemoveProfileChannels;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxProfileChannelCount;
        private System.Windows.Forms.Button buttonAddMultipleProfileChannels;
    }
}