using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus.Dialogs
{
    sealed partial class GroupDialog
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
            this.lbChannels = new ListBox();
            this.btnAddRoot = new Button();
            this.btnRemoveGroup = new Button();
            this.btnUp = new Button();
            this.btnDown = new Button();
            this.btnColorGroup = new Button();
            this.btnAddChannels = new Button();
            this.btnRenameGroup = new Button();
            this.btnRemoveChannels = new Button();
            this.lblChannels = new Label();
            this.lblGroups = new Label();
            this.btnAddChild = new Button();
            this.btnExpand = new Button();
            this.btnCollapse = new Button();
            this.tvGroups = new MultiSelectTreeview();
            this.cbSort = new ComboBox();
            this.btnAddMutli = new Button();
            this.lblStats = new Label();
            this.SuspendLayout();
            // 
            // lbChannels
            // 
            this.lbChannels.AllowDrop = true;
            this.lbChannels.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbChannels.FormattingEnabled = true;
            this.lbChannels.IntegralHeight = false;
            this.lbChannels.Location = new Point(12, 33);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.SelectionMode = SelectionMode.MultiExtended;
            this.lbChannels.Size = new Size(200, 425);
            this.lbChannels.TabIndex = 0;
            this.lbChannels.DrawItem += new DrawItemEventHandler(this.lbChannels_DrawItem);
            this.lbChannels.SelectedIndexChanged += new EventHandler(this.lbChannels_SelectedIndexChanged);
            this.lbChannels.Leave += new EventHandler(this.lbChannels_Leave);
            this.lbChannels.MouseDown += new MouseEventHandler(this.lbChannels_MouseDown);
            // 
            // btnAddRoot
            // 
            this.btnAddRoot.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnAddRoot.Location = new Point(605, 33);
            this.btnAddRoot.Name = "btnAddRoot";
            this.btnAddRoot.Size = new Size(75, 23);
            this.btnAddRoot.TabIndex = 2;
            this.btnAddRoot.Text = "Add &Group";
            this.btnAddRoot.UseVisualStyleBackColor = true;
            this.btnAddRoot.Click += new EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnRemoveGroup.Location = new Point(606, 120);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new Size(75, 23);
            this.btnRemoveGroup.TabIndex = 3;
            this.btnRemoveGroup.Text = "Dele&te";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            this.btnRemoveGroup.Click += new EventHandler(this.btnRemoveGroup_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnUp.Location = new Point(605, 236);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnDown.Location = new Point(605, 265);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "Move &Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnDown_Click);
            // 
            // btnColorGroup
            // 
            this.btnColorGroup.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnColorGroup.Location = new Point(606, 178);
            this.btnColorGroup.Name = "btnColorGroup";
            this.btnColorGroup.Size = new Size(75, 23);
            this.btnColorGroup.TabIndex = 5;
            this.btnColorGroup.Text = "&Set Color";
            this.btnColorGroup.UseVisualStyleBackColor = true;
            this.btnColorGroup.Click += new EventHandler(this.btnGroupColor_Click);
            // 
            // btnAddChannels
            // 
            this.btnAddChannels.Anchor = AnchorStyles.Top;
            this.btnAddChannels.Location = new Point(218, 236);
            this.btnAddChannels.Name = "btnAddChannels";
            this.btnAddChannels.Size = new Size(75, 23);
            this.btnAddChannels.TabIndex = 9;
            this.btnAddChannels.Text = "&Add ->";
            this.btnAddChannels.UseVisualStyleBackColor = true;
            this.btnAddChannels.Click += new EventHandler(this.btnAddChannels_Click);
            // 
            // btnRenameGroup
            // 
            this.btnRenameGroup.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnRenameGroup.Location = new Point(606, 149);
            this.btnRenameGroup.Name = "btnRenameGroup";
            this.btnRenameGroup.Size = new Size(75, 23);
            this.btnRenameGroup.TabIndex = 4;
            this.btnRenameGroup.Text = "Re&name";
            this.btnRenameGroup.UseVisualStyleBackColor = true;
            this.btnRenameGroup.Click += new EventHandler(this.btnRenameGroup_Click);
            // 
            // btnRemoveChannels
            // 
            this.btnRemoveChannels.Anchor = AnchorStyles.Top;
            this.btnRemoveChannels.Location = new Point(218, 265);
            this.btnRemoveChannels.Name = "btnRemoveChannels";
            this.btnRemoveChannels.Size = new Size(75, 23);
            this.btnRemoveChannels.TabIndex = 10;
            this.btnRemoveChannels.Text = "<- &Remove";
            this.btnRemoveChannels.UseVisualStyleBackColor = true;
            this.btnRemoveChannels.Click += new EventHandler(this.btnRemoveChannels_Click);
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new Point(9, 9);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new Size(68, 13);
            this.lblChannels.TabIndex = 13;
            this.lblChannels.Text = "All Channels:";
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new Point(296, 9);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new Size(81, 13);
            this.lblGroups.TabIndex = 14;
            this.lblGroups.Text = "Current Groups:";
            // 
            // btnAddChild
            // 
            this.btnAddChild.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnAddChild.Location = new Point(606, 91);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new Size(75, 23);
            this.btnAddChild.TabIndex = 15;
            this.btnAddChild.Text = "&Copy Group";
            this.btnAddChild.UseVisualStyleBackColor = true;
            this.btnAddChild.Click += new EventHandler(this.btnAddChild_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnExpand.Location = new Point(605, 323);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new Size(75, 23);
            this.btnExpand.TabIndex = 16;
            this.btnExpand.Text = "&Expand All";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new EventHandler(this.btnExpand_Click);
            // 
            // btnCollapse
            // 
            this.btnCollapse.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCollapse.Location = new Point(605, 352);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new Size(75, 23);
            this.btnCollapse.TabIndex = 17;
            this.btnCollapse.Text = "C&ollapse All";
            this.btnCollapse.UseVisualStyleBackColor = true;
            this.btnCollapse.Click += new EventHandler(this.btnCollapse_Click);
            // 
            // tvGroups
            // 
            this.tvGroups.AllowDrop = true;
            this.tvGroups.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvGroups.Location = new Point(299, 33);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new Size(300, 425);
            this.tvGroups.TabIndex = 1;
            this.tvGroups.DrawNode += new DrawTreeNodeEventHandler(this.tvGroups_DrawNode);
            this.tvGroups.AfterSelect += new TreeViewEventHandler(this.tvGroups_AfterSelect);
            this.tvGroups.DragDrop += new DragEventHandler(this.tvGroups_DragDrop);
            this.tvGroups.DragEnter += new DragEventHandler(this.tvGroups_DragEnter);
            this.tvGroups.DragOver += new DragEventHandler(this.tvGroups_DragOver);
            this.tvGroups.Leave += new EventHandler(this.tvGroups_Leave);
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
            this.cbSort.Location = new Point(106, 6);
            this.cbSort.Name = "cbSort";
            this.cbSort.Size = new Size(106, 21);
            this.cbSort.TabIndex = 18;
            this.cbSort.SelectedIndexChanged += new EventHandler(this.cbSort_SelectedIndexChanged);
            // 
            // btnAddMutli
            // 
            this.btnAddMutli.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnAddMutli.Location = new Point(605, 62);
            this.btnAddMutli.Name = "btnAddMutli";
            this.btnAddMutli.Size = new Size(76, 23);
            this.btnAddMutli.TabIndex = 19;
            this.btnAddMutli.Text = "Add &Multi";
            this.btnAddMutli.UseVisualStyleBackColor = true;
            this.btnAddMutli.Click += new EventHandler(this.btnAddMutli_Click);
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Location = new Point(383, 9);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new Size(187, 13);
            this.lblStats.TabIndex = 20;
            this.lblStats.Text = "0 groups && 0 channels in those groups";
            // 
            // GroupDialog
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
            this.Name = "GroupDialog";
            this.Size = new Size(692, 470);
            this.SizeChanged += new EventHandler(this.GroupDialog_SizeChanged);
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