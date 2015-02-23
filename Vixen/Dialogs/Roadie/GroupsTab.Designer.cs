using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus.Dialogs
{
    sealed partial class GroupsTab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tvGroups = new VixenPlusCommon.MultiSelectTreeview();
            this.cbSort = new System.Windows.Forms.ComboBox();
            this.btnAddMutli = new System.Windows.Forms.Button();
            this.lblStats = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbChannels
            // 
            this.lbChannels.AllowDrop = true;
            this.lbChannels.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbChannels.FormattingEnabled = true;
            this.lbChannels.IntegralHeight = false;
            this.lbChannels.Location = new System.Drawing.Point(12, 33);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbChannels.Size = new System.Drawing.Size(200, 425);
            this.lbChannels.TabIndex = 0;
            this.lbChannels.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChannels_DrawItem);
            this.lbChannels.SelectedIndexChanged += new System.EventHandler(this.lbChannels_SelectedIndexChanged);
            this.lbChannels.Leave += new System.EventHandler(this.lbChannels_Leave);
            this.lbChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChannels_MouseDown);
            // 
            // btnAddRoot
            // 
            this.btnAddRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRoot.Location = new System.Drawing.Point(605, 33);
            this.btnAddRoot.Name = "btnAddRoot";
            this.btnAddRoot.Size = new System.Drawing.Size(75, 23);
            this.btnAddRoot.TabIndex = 2;
            this.btnAddRoot.Text = "Add &Group";
            this.btnAddRoot.UseVisualStyleBackColor = true;
            this.btnAddRoot.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveGroup.Location = new System.Drawing.Point(606, 120);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveGroup.TabIndex = 3;
            this.btnRemoveGroup.Text = "Dele&te";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            this.btnRemoveGroup.Click += new System.EventHandler(this.btnRemoveGroup_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(605, 236);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(605, 265);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "Move &Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnColorGroup
            // 
            this.btnColorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColorGroup.Location = new System.Drawing.Point(606, 178);
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
            this.btnAddChannels.Location = new System.Drawing.Point(218, 236);
            this.btnAddChannels.Name = "btnAddChannels";
            this.btnAddChannels.Size = new System.Drawing.Size(75, 23);
            this.btnAddChannels.TabIndex = 9;
            this.btnAddChannels.Text = "&Add ->";
            this.btnAddChannels.UseVisualStyleBackColor = true;
            this.btnAddChannels.Click += new System.EventHandler(this.btnAddChannels_Click);
            // 
            // btnRenameGroup
            // 
            this.btnRenameGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameGroup.Location = new System.Drawing.Point(606, 149);
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
            this.btnRemoveChannels.Location = new System.Drawing.Point(218, 265);
            this.btnRemoveChannels.Name = "btnRemoveChannels";
            this.btnRemoveChannels.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveChannels.TabIndex = 10;
            this.btnRemoveChannels.Text = "<- &Remove";
            this.btnRemoveChannels.UseVisualStyleBackColor = true;
            this.btnRemoveChannels.Click += new System.EventHandler(this.btnRemoveChannels_Click);
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
            this.lblGroups.Location = new System.Drawing.Point(296, 9);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new System.Drawing.Size(81, 13);
            this.lblGroups.TabIndex = 14;
            this.lblGroups.Text = "Current Groups:";
            // 
            // btnAddChild
            // 
            this.btnAddChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddChild.Location = new System.Drawing.Point(606, 91);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new System.Drawing.Size(75, 23);
            this.btnAddChild.TabIndex = 15;
            this.btnAddChild.Text = "&Copy Group";
            this.btnAddChild.UseVisualStyleBackColor = true;
            this.btnAddChild.Click += new System.EventHandler(this.btnAddChild_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.Location = new System.Drawing.Point(605, 323);
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
            this.btnCollapse.Location = new System.Drawing.Point(605, 352);
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
            this.tvGroups.Location = new System.Drawing.Point(299, 33);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new System.Drawing.Size(300, 425);
            this.tvGroups.TabIndex = 1;
            this.tvGroups.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvGroups_DrawNode);
            this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
            this.tvGroups.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvGroups_DragDrop);
            this.tvGroups.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvGroups_DragEnter);
            this.tvGroups.DragOver += new System.Windows.Forms.DragEventHandler(this.tvGroups_DragOver);
            this.tvGroups.Leave += new System.EventHandler(this.tvGroups_Leave);
            // 
            // cbSort
            // 
            this.cbSort.FormattingEnabled = true;
            this.cbSort.Items.AddRange(new object[] {
            "Natural Order",
            "By Color",
            "By Name",
            "By Color Then Name",
            "By Name Then Color"});
            this.cbSort.Location = new System.Drawing.Point(106, 6);
            this.cbSort.Name = "cbSort";
            this.cbSort.Size = new System.Drawing.Size(106, 21);
            this.cbSort.TabIndex = 18;
            this.cbSort.SelectedIndexChanged += new System.EventHandler(this.cbSort_SelectedIndexChanged);
            // 
            // btnAddMutli
            // 
            this.btnAddMutli.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMutli.Location = new System.Drawing.Point(605, 62);
            this.btnAddMutli.Name = "btnAddMutli";
            this.btnAddMutli.Size = new System.Drawing.Size(76, 23);
            this.btnAddMutli.TabIndex = 19;
            this.btnAddMutli.Text = "Add &Multi";
            this.btnAddMutli.UseVisualStyleBackColor = true;
            this.btnAddMutli.Click += new System.EventHandler(this.btnAddMutli_Click);
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Location = new System.Drawing.Point(383, 9);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(187, 13);
            this.lblStats.TabIndex = 20;
            this.lblStats.Text = "0 groups && 0 channels in those groups";
            // 
            // GroupsTab
            // 
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.btnAddMutli);
            this.Controls.Add(this.cbSort);
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
            this.Name = "GroupsTab";
            this.Size = new System.Drawing.Size(692, 470);
            this.SizeChanged += new System.EventHandler(this.GroupDialog_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lbChannels;
        private Button btnAddRoot;
        private Button btnRemoveGroup;
        private Button btnUp;
        private Button btnDown;
        private Button btnColorGroup;
        private Button btnAddChannels;
        private Button btnRenameGroup;
        private Button btnRemoveChannels;
        private MultiSelectTreeview tvGroups;
        private Label lblChannels;
        private Label lblGroups;
        private Button btnAddChild;
        private Button btnExpand;
        private Button btnCollapse;
        private ComboBox cbSort;
        private Button btnAddMutli;
        private Label lblStats;
    }
}