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
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnCopyChannels = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnRemoveChannels = new System.Windows.Forms.Button();
            this.tvGroups = new CommonControls.MultiSelectTreeview();
            this.lblChannels = new System.Windows.Forms.Label();
            this.lblGroups = new System.Windows.Forms.Label();
            this.btnAddChild = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(608, 406);
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
            this.btnCancel.Location = new System.Drawing.Point(608, 435);
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
            this.lbChannels.Location = new System.Drawing.Point(15, 25);
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
            this.btnAddRoot.Location = new System.Drawing.Point(608, 25);
            this.btnAddRoot.Name = "btnAddRoot";
            this.btnAddRoot.Size = new System.Drawing.Size(75, 23);
            this.btnAddRoot.TabIndex = 2;
            this.btnAddRoot.Text = "Add &Parent";
            this.btnAddRoot.UseVisualStyleBackColor = true;
            this.btnAddRoot.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(608, 83);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Re&move Group";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(608, 199);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(608, 228);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "Move &Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnColor
            // 
            this.btnColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColor.Location = new System.Drawing.Point(608, 141);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(75, 23);
            this.btnColor.TabIndex = 5;
            this.btnColor.Text = "&Set Color";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnGroupColor_Click);
            // 
            // btnCopyChannels
            // 
            this.btnCopyChannels.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCopyChannels.Location = new System.Drawing.Point(221, 199);
            this.btnCopyChannels.Name = "btnCopyChannels";
            this.btnCopyChannels.Size = new System.Drawing.Size(75, 23);
            this.btnCopyChannels.TabIndex = 9;
            this.btnCopyChannels.Text = "&Add ->";
            this.btnCopyChannels.UseVisualStyleBackColor = true;
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRename.Location = new System.Drawing.Point(608, 112);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(75, 23);
            this.btnRename.TabIndex = 4;
            this.btnRename.Text = "Re&name";
            this.btnRename.UseVisualStyleBackColor = true;
            // 
            // btnRemoveChannels
            // 
            this.btnRemoveChannels.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRemoveChannels.Location = new System.Drawing.Point(221, 228);
            this.btnRemoveChannels.Name = "btnRemoveChannels";
            this.btnRemoveChannels.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveChannels.TabIndex = 10;
            this.btnRemoveChannels.Text = "<- &Remove";
            this.btnRemoveChannels.UseVisualStyleBackColor = true;
            // 
            // tvGroups
            // 
            this.tvGroups.AllowDrop = true;
            this.tvGroups.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvGroups.Location = new System.Drawing.Point(302, 25);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new System.Drawing.Size(300, 433);
            this.tvGroups.TabIndex = 1;
            this.tvGroups.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvGroups_DrawNode);
            this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
            this.tvGroups.Leave += new System.EventHandler(this.tvGroups_Leave);
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(12, 9);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(68, 13);
            this.lblChannels.TabIndex = 13;
            this.lblChannels.Text = "All Channels:";
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new System.Drawing.Point(299, 9);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new System.Drawing.Size(81, 13);
            this.lblGroups.TabIndex = 14;
            this.lblGroups.Text = "Current Groups:";
            // 
            // btnAddChild
            // 
            this.btnAddChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddChild.Location = new System.Drawing.Point(608, 54);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new System.Drawing.Size(75, 23);
            this.btnAddChild.TabIndex = 15;
            this.btnAddChild.Text = "Add &Child";
            this.btnAddChild.UseVisualStyleBackColor = true;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.Location = new System.Drawing.Point(608, 286);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(75, 23);
            this.btnExpand.TabIndex = 16;
            this.btnExpand.Text = "&Expand All";
            this.btnExpand.UseVisualStyleBackColor = true;
            // 
            // btnCollapse
            // 
            this.btnCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapse.Location = new System.Drawing.Point(608, 315);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(75, 23);
            this.btnCollapse.TabIndex = 17;
            this.btnCollapse.Text = "C&ollapse All";
            this.btnCollapse.UseVisualStyleBackColor = true;
            // 
            // GroupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 470);
            this.Controls.Add(this.btnCollapse);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.btnAddChild);
            this.Controls.Add(this.lblGroups);
            this.Controls.Add(this.lblChannels);
            this.Controls.Add(this.btnRemoveChannels);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnCopyChannels);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddRoot);
            this.Controls.Add(this.tvGroups);
            this.Controls.Add(this.lbChannels);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GroupDialog";
            this.ResizeBegin += new System.EventHandler(this.GroupDialog_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.GroupDialog_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.GroupDialog_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbChannels;
        private System.Windows.Forms.Button btnAddRoot;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Button btnCopyChannels;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnRemoveChannels;
        private CommonControls.MultiSelectTreeview tvGroups;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.Button btnAddChild;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnCollapse;
    }
}