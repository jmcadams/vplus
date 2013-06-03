namespace VixenEditor
{
    partial class GroupDialog
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
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.btnRemoveGroup = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnGroupColor = new System.Windows.Forms.Button();
            this.btnNewFromChannels = new System.Windows.Forms.Button();
            this.btnCopyChannels = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnRemoveChannels = new System.Windows.Forms.Button();
            this.tvGroups = new CommonControls.MultiSelectTreeview();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(622, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(541, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbChannels
            // 
            this.lbChannels.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbChannels.FormattingEnabled = true;
            this.lbChannels.IntegralHeight = false;
            this.lbChannels.Location = new System.Drawing.Point(12, 12);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbChannels.Size = new System.Drawing.Size(195, 433);
            this.lbChannels.TabIndex = 4;
            this.lbChannels.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChannels_DrawItem);
            this.lbChannels.SelectedIndexChanged += new System.EventHandler(this.lbChannels_SelectedIndexChanged);
            this.lbChannels.Leave += new System.EventHandler(this.lbChannels_Leave);
            this.lbChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChannels_MouseDown);
            this.lbChannels.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbChannels_MouseUp);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(228, 32);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(112, 23);
            this.btnAddGroup.TabIndex = 8;
            this.btnAddGroup.Text = "Add New Group";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.Location = new System.Drawing.Point(228, 62);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new System.Drawing.Size(112, 23);
            this.btnRemoveGroup.TabIndex = 9;
            this.btnRemoveGroup.Text = "Remove Group";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(228, 179);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(112, 23);
            this.btnUp.TabIndex = 10;
            this.btnUp.Text = "Move Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(228, 208);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(112, 23);
            this.btnDown.TabIndex = 11;
            this.btnDown.Text = "Move Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnGroupColor
            // 
            this.btnGroupColor.Location = new System.Drawing.Point(228, 120);
            this.btnGroupColor.Name = "btnGroupColor";
            this.btnGroupColor.Size = new System.Drawing.Size(112, 23);
            this.btnGroupColor.TabIndex = 12;
            this.btnGroupColor.Text = "Set Group Color";
            this.btnGroupColor.UseVisualStyleBackColor = true;
            this.btnGroupColor.Click += new System.EventHandler(this.btnGroupColor_Click);
            // 
            // btnNewFromChannels
            // 
            this.btnNewFromChannels.Location = new System.Drawing.Point(228, 266);
            this.btnNewFromChannels.Name = "btnNewFromChannels";
            this.btnNewFromChannels.Size = new System.Drawing.Size(112, 46);
            this.btnNewFromChannels.TabIndex = 13;
            this.btnNewFromChannels.Text = "Create Group From Selected Channels";
            this.btnNewFromChannels.UseVisualStyleBackColor = true;
            // 
            // btnCopyChannels
            // 
            this.btnCopyChannels.Location = new System.Drawing.Point(228, 318);
            this.btnCopyChannels.Name = "btnCopyChannels";
            this.btnCopyChannels.Size = new System.Drawing.Size(112, 46);
            this.btnCopyChannels.TabIndex = 14;
            this.btnCopyChannels.Text = "Copy Channels to Selected Group";
            this.btnCopyChannels.UseVisualStyleBackColor = true;
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(228, 91);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(112, 23);
            this.btnRename.TabIndex = 15;
            this.btnRename.Text = "Rename Group";
            this.btnRename.UseVisualStyleBackColor = true;
            // 
            // btnRemoveChannels
            // 
            this.btnRemoveChannels.Location = new System.Drawing.Point(228, 371);
            this.btnRemoveChannels.Name = "btnRemoveChannels";
            this.btnRemoveChannels.Size = new System.Drawing.Size(112, 46);
            this.btnRemoveChannels.TabIndex = 16;
            this.btnRemoveChannels.Text = "Remove Channels From Group";
            this.btnRemoveChannels.UseVisualStyleBackColor = true;
            // 
            // tvGroups
            // 
            this.tvGroups.AllowDrop = true;
            this.tvGroups.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvGroups.Location = new System.Drawing.Point(361, 12);
            this.tvGroups.Name = "tvGroups";
            //this.tvGroups.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject("tvGroups.SelectedNodes")));
            this.tvGroups.Size = new System.Drawing.Size(336, 433);
            this.tvGroups.TabIndex = 7;
            this.tvGroups.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvGroups_DrawNode);
            this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
            this.tvGroups.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvGroups_NodeMouseClick);
            this.tvGroups.Leave += new System.EventHandler(this.tvGroups_Leave);
            // 
            // GroupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 484);
            this.Controls.Add(this.btnRemoveChannels);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnCopyChannels);
            this.Controls.Add(this.btnNewFromChannels);
            this.Controls.Add(this.btnGroupColor);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRemoveGroup);
            this.Controls.Add(this.btnAddGroup);
            this.Controls.Add(this.tvGroups);
            this.Controls.Add(this.lbChannels);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GroupDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbChannels;
        //private System.Windows.Forms.TreeView tvGroups;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.Button btnRemoveGroup;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnGroupColor;
        private System.Windows.Forms.Button btnNewFromChannels;
        private System.Windows.Forms.Button btnCopyChannels;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnRemoveChannels;
        private CommonControls.MultiSelectTreeview tvGroups;
    }
}