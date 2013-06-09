namespace VixenEditor
{
    sealed partial class GroupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbChannels = new System.Windows.Forms.ListBox();
            this.btnAddRoot = new System.Windows.Forms.Button();
            this.btnRemoveGroup = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnColorGroup = new System.Windows.Forms.Button();
            this.btnAddChannels = new System.Windows.Forms.Button();
            this.btnRenameGroup = new System.Windows.Forms.Button();
            this.btnRemoveChannels = new System.Windows.Forms.Button();
            this.lblChannels = new System.Windows.Forms.Label();
            this.lblGroups = new System.Windows.Forms.Label();
            this.btnAddChild = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.tvGroups = new CommonControls.MultiSelectTreeview();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(605, 406);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(605, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbChannels
            // 
            this.lbChannels.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbChannels.FormattingEnabled = true;
            this.lbChannels.IntegralHeight = false;
            this.lbChannels.Location = new System.Drawing.Point(12, 25);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbChannels.Size = new System.Drawing.Size(200, 433);
            this.lbChannels.TabIndex = 0;
            this.lbChannels.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChannels_DrawItem);
            this.lbChannels.SelectedIndexChanged += new System.EventHandler(this.lbChannels_SelectedIndexChanged);
            this.lbChannels.Leave += new System.EventHandler(this.lbChannels_Leave);
            this.lbChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChannels_MouseDown);
            this.lbChannels.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbChannels_MouseUp);
            // 
            // btnAddRoot
            // 
            this.btnAddRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRoot.Location = new System.Drawing.Point(605, 25);
            this.btnAddRoot.Name = "btnAddRoot";
            this.btnAddRoot.Size = new System.Drawing.Size(75, 23);
            this.btnAddRoot.TabIndex = 2;
            this.btnAddRoot.Text = "Add &Parent";
            this.btnAddRoot.UseVisualStyleBackColor = true;
            this.btnAddRoot.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveGroup.Location = new System.Drawing.Point(605, 83);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveGroup.TabIndex = 3;
            this.btnRemoveGroup.Text = "Re&move Group";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            this.btnRemoveGroup.Click += new System.EventHandler(this.btnRemoveGroup_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(605, 199);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(605, 228);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "Move &Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnColorGroup
            // 
            this.btnColorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColorGroup.Location = new System.Drawing.Point(605, 141);
            this.btnColorGroup.Name = "btnColorGroup";
            this.btnColorGroup.Size = new System.Drawing.Size(75, 23);
            this.btnColorGroup.TabIndex = 5;
            this.btnColorGroup.Text = "&Set Color";
            this.btnColorGroup.UseVisualStyleBackColor = true;
            this.btnColorGroup.Click += new System.EventHandler(this.btnGroupColor_Click);
            // 
            // btnAddChannels
            // 
            this.btnAddChannels.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAddChannels.Location = new System.Drawing.Point(218, 199);
            this.btnAddChannels.Name = "btnAddChannels";
            this.btnAddChannels.Size = new System.Drawing.Size(75, 23);
            this.btnAddChannels.TabIndex = 9;
            this.btnAddChannels.Text = "&Add ->";
            this.btnAddChannels.UseVisualStyleBackColor = true;
            // 
            // btnRenameGroup
            // 
            this.btnRenameGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameGroup.Location = new System.Drawing.Point(605, 112);
            this.btnRenameGroup.Name = "btnRenameGroup";
            this.btnRenameGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRenameGroup.TabIndex = 4;
            this.btnRenameGroup.Text = "Re&name";
            this.btnRenameGroup.UseVisualStyleBackColor = true;
            this.btnRenameGroup.Click += new System.EventHandler(this.btnRenameGroup_Click);
            // 
            // btnRemoveChannels
            // 
            this.btnRemoveChannels.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRemoveChannels.Location = new System.Drawing.Point(218, 228);
            this.btnRemoveChannels.Name = "btnRemoveChannels";
            this.btnRemoveChannels.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveChannels.TabIndex = 10;
            this.btnRemoveChannels.Text = "<- &Remove";
            this.btnRemoveChannels.UseVisualStyleBackColor = true;
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(9, 9);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(68, 13);
            this.lblChannels.TabIndex = 13;
            this.lblChannels.Text = "All Channels:";
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new System.Drawing.Point(297, 9);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new System.Drawing.Size(81, 13);
            this.lblGroups.TabIndex = 14;
            this.lblGroups.Text = "Current Groups:";
            // 
            // btnAddChild
            // 
            this.btnAddChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddChild.Location = new System.Drawing.Point(605, 54);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new System.Drawing.Size(75, 23);
            this.btnAddChild.TabIndex = 15;
            this.btnAddChild.Text = "Add &Child";
            this.btnAddChild.UseVisualStyleBackColor = true;
            this.btnAddChild.Click += new System.EventHandler(this.btnAddChild_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.Location = new System.Drawing.Point(605, 286);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(75, 23);
            this.btnExpand.TabIndex = 16;
            this.btnExpand.Text = "&Expand All";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnCollapse
            // 
            this.btnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapse.Location = new System.Drawing.Point(605, 315);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(75, 23);
            this.btnCollapse.TabIndex = 17;
            this.btnCollapse.Text = "C&ollapse All";
            this.btnCollapse.UseVisualStyleBackColor = true;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // tvGroups
            // 
            this.tvGroups.AllowDrop = true;
            this.tvGroups.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvGroups.Location = new System.Drawing.Point(299, 25);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new System.Drawing.Size(300, 433);
            this.tvGroups.TabIndex = 1;
            this.tvGroups.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvGroups_DrawNode);
            this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
            this.tvGroups.Leave += new System.EventHandler(this.tvGroups_Leave);
            // 
            // GroupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 470);
            this.Controls.Add(this.btnCollapse);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.btnAddChild);
            this.Controls.Add(this.lblGroups);
            this.Controls.Add(this.lblChannels);
            this.Controls.Add(this.btnRemoveChannels);
            this.Controls.Add(this.btnRenameGroup);
            this.Controls.Add(this.btnAddChannels);
            this.Controls.Add(this.btnColorGroup);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRemoveGroup);
            this.Controls.Add(this.btnAddRoot);
            this.Controls.Add(this.tvGroups);
            this.Controls.Add(this.lbChannels);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "GroupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GroupDialog";
            this.SizeChanged += new System.EventHandler(this.GroupDialog_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbChannels;
        private System.Windows.Forms.Button btnAddRoot;
        private System.Windows.Forms.Button btnRemoveGroup;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnColorGroup;
        private System.Windows.Forms.Button btnAddChannels;
        private System.Windows.Forms.Button btnRenameGroup;
        private System.Windows.Forms.Button btnRemoveChannels;
        private CommonControls.MultiSelectTreeview tvGroups;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.Button btnAddChild;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnCollapse;
    }
}