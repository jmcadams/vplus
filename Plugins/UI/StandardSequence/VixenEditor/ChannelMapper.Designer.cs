namespace VixenEditor {
    partial class ChannelMapper {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelMapper));
            this.lbDestination = new System.Windows.Forms.ListBox();
            this.btnDestinationProfile = new System.Windows.Forms.Button();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.btnSaveMap = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnPreviewEdit = new System.Windows.Forms.Button();
            this.cbKeepUnmapped = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSortSrc = new System.Windows.Forms.Label();
            this.cbSortSrc = new System.Windows.Forms.ComboBox();
            this.lblSouceChannels = new System.Windows.Forms.Label();
            this.lblChannelMapping = new System.Windows.Forms.Label();
            this.lblDestChannels = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbEx = new System.Windows.Forms.TextBox();
            this.lblEx = new System.Windows.Forms.Label();
            this.vsb = new System.Windows.Forms.VScrollBar();
            this.cbSortDest = new System.Windows.Forms.ComboBox();
            this.lblSortDest = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDestination
            // 
            this.lbDestination.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbDestination.FormattingEnabled = true;
            this.lbDestination.ItemHeight = 20;
            this.lbDestination.Location = new System.Drawing.Point(586, 54);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.Size = new System.Drawing.Size(186, 464);
            this.lbDestination.TabIndex = 10;
            this.lbDestination.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbDestination_DrawItem);
            // 
            // btnDestinationProfile
            // 
            this.btnDestinationProfile.Location = new System.Drawing.Point(460, 532);
            this.btnDestinationProfile.Name = "btnDestinationProfile";
            this.btnDestinationProfile.Size = new System.Drawing.Size(139, 23);
            this.btnDestinationProfile.TabIndex = 0;
            this.btnDestinationProfile.Text = "New Destination Profile";
            this.btnDestinationProfile.UseVisualStyleBackColor = true;
            this.btnDestinationProfile.Click += new System.EventHandler(this.btnDestProfile_Click);
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Location = new System.Drawing.Point(204, 532);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(75, 23);
            this.btnLoadMap.TabIndex = 3;
            this.btnLoadMap.Text = "Load Map";
            this.toolTips.SetToolTip(this.btnLoadMap, "Load a mapping file.");
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.BtnLoadClick);
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.Location = new System.Drawing.Point(285, 532);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMap.TabIndex = 4;
            this.btnSaveMap.Text = "Save Map";
            this.toolTips.SetToolTip(this.btnSaveMap, "Save a mapping file.");
            this.btnSaveMap.UseVisualStyleBackColor = true;
            this.btnSaveMap.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // btnTransform
            // 
            this.btnTransform.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTransform.Location = new System.Drawing.Point(697, 532);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(75, 23);
            this.btnTransform.TabIndex = 6;
            this.btnTransform.Text = "Transform";
            this.toolTips.SetToolTip(this.btnTransform, "Transform your sequence.");
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.BtnTransformClick);
            // 
            // btnPreviewEdit
            // 
            this.btnPreviewEdit.Location = new System.Drawing.Point(366, 532);
            this.btnPreviewEdit.Name = "btnPreviewEdit";
            this.btnPreviewEdit.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewEdit.TabIndex = 16;
            this.btnPreviewEdit.Text = "Preview";
            this.toolTips.SetToolTip(this.btnPreviewEdit, "Show a Visual Preivew");
            this.btnPreviewEdit.UseVisualStyleBackColor = true;
            this.btnPreviewEdit.Click += new System.EventHandler(this.BtnPreviewClick);
            // 
            // cbKeepUnmapped
            // 
            this.cbKeepUnmapped.AutoSize = true;
            this.cbKeepUnmapped.Location = new System.Drawing.Point(12, 536);
            this.cbKeepUnmapped.Name = "cbKeepUnmapped";
            this.cbKeepUnmapped.Size = new System.Drawing.Size(186, 17);
            this.cbKeepUnmapped.TabIndex = 2;
            this.cbKeepUnmapped.Text = "Map Unmapped Source Channels";
            this.cbKeepUnmapped.UseVisualStyleBackColor = true;
            this.cbKeepUnmapped.Click += new System.EventHandler(this.cbKeepEmpty_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(616, 532);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSortSrc
            // 
            this.lblSortSrc.AutoSize = true;
            this.lblSortSrc.Location = new System.Drawing.Point(13, 13);
            this.lblSortSrc.Name = "lblSortSrc";
            this.lblSortSrc.Size = new System.Drawing.Size(44, 13);
            this.lblSortSrc.TabIndex = 11;
            this.lblSortSrc.Text = "Sort By:";
            // 
            // cbSortSrc
            // 
            this.cbSortSrc.FormattingEnabled = true;
            this.cbSortSrc.Location = new System.Drawing.Point(63, 10);
            this.cbSortSrc.Name = "cbSortSrc";
            this.cbSortSrc.Size = new System.Drawing.Size(135, 21);
            this.cbSortSrc.TabIndex = 1;
            this.cbSortSrc.SelectedIndexChanged += new System.EventHandler(this.SortSrcIndexChanged);
            // 
            // lblSouceChannels
            // 
            this.lblSouceChannels.AutoSize = true;
            this.lblSouceChannels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSouceChannels.Location = new System.Drawing.Point(9, 38);
            this.lblSouceChannels.Name = "lblSouceChannels";
            this.lblSouceChannels.Size = new System.Drawing.Size(87, 13);
            this.lblSouceChannels.TabIndex = 12;
            this.lblSouceChannels.Text = "Source Profile";
            // 
            // lblChannelMapping
            // 
            this.lblChannelMapping.AutoSize = true;
            this.lblChannelMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChannelMapping.Location = new System.Drawing.Point(201, 38);
            this.lblChannelMapping.Name = "lblChannelMapping";
            this.lblChannelMapping.Size = new System.Drawing.Size(135, 13);
            this.lblChannelMapping.TabIndex = 13;
            this.lblChannelMapping.Text = "Destination Channel(s)";
            // 
            // lblDestChannels
            // 
            this.lblDestChannels.AutoSize = true;
            this.lblDestChannels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestChannels.Location = new System.Drawing.Point(645, 38);
            this.lblDestChannels.Name = "lblDestChannels";
            this.lblDestChannels.Size = new System.Drawing.Size(111, 13);
            this.lblDestChannels.TabIndex = 14;
            this.lblDestChannels.Text = "Destination Profile";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbEx);
            this.panel1.Controls.Add(this.lblEx);
            this.panel1.Controls.Add(this.vsb);
            this.panel1.Location = new System.Drawing.Point(12, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(568, 472);
            this.panel1.TabIndex = 15;
            // 
            // tbEx
            // 
            this.tbEx.BackColor = System.Drawing.SystemColors.Window;
            this.tbEx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbEx.Location = new System.Drawing.Point(192, 1);
            this.tbEx.Margin = new System.Windows.Forms.Padding(0);
            this.tbEx.Name = "tbEx";
            this.tbEx.Size = new System.Drawing.Size(359, 20);
            this.tbEx.TabIndex = 18;
            this.tbEx.TabStop = false;
            this.tbEx.Text = "Example Text";
            this.tbEx.Visible = false;
            // 
            // lblEx
            // 
            this.lblEx.Enabled = false;
            this.lblEx.Location = new System.Drawing.Point(0, 0);
            this.lblEx.Name = "lblEx";
            this.lblEx.Size = new System.Drawing.Size(186, 20);
            this.lblEx.TabIndex = 17;
            this.lblEx.Text = "Example Label";
            this.lblEx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEx.Visible = false;
            // 
            // vsb
            // 
            this.vsb.Location = new System.Drawing.Point(551, 0);
            this.vsb.Name = "vsb";
            this.vsb.Size = new System.Drawing.Size(17, 472);
            this.vsb.TabIndex = 16;
            this.vsb.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VsbScroll);
            this.vsb.ValueChanged += new System.EventHandler(this.VsbValueChanged);
            // 
            // cbSortDest
            // 
            this.cbSortDest.FormattingEnabled = true;
            this.cbSortDest.Location = new System.Drawing.Point(637, 10);
            this.cbSortDest.Name = "cbSortDest";
            this.cbSortDest.Size = new System.Drawing.Size(135, 21);
            this.cbSortDest.TabIndex = 17;
            this.cbSortDest.SelectedIndexChanged += new System.EventHandler(this.SortDestIndexChanged);
            // 
            // lblSortDest
            // 
            this.lblSortDest.AutoSize = true;
            this.lblSortDest.Location = new System.Drawing.Point(587, 13);
            this.lblSortDest.Name = "lblSortDest";
            this.lblSortDest.Size = new System.Drawing.Size(44, 13);
            this.lblSortDest.TabIndex = 18;
            this.lblSortDest.Text = "Sort By:";
            // 
            // ChannelMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.cbSortDest);
            this.Controls.Add(this.lblSortDest);
            this.Controls.Add(this.btnPreviewEdit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDestChannels);
            this.Controls.Add(this.lblChannelMapping);
            this.Controls.Add(this.lblSouceChannels);
            this.Controls.Add(this.cbSortSrc);
            this.Controls.Add(this.lblSortSrc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTransform);
            this.Controls.Add(this.cbKeepUnmapped);
            this.Controls.Add(this.btnSaveMap);
            this.Controls.Add(this.btnLoadMap);
            this.Controls.Add(this.btnDestinationProfile);
            this.Controls.Add(this.lbDestination);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelMapper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Channel Mapper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapperFormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.ListBox lbDestination;
        private System.Windows.Forms.Button btnDestinationProfile;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.Button btnSaveMap;
        private System.Windows.Forms.CheckBox cbKeepUnmapped;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSortSrc;
        private System.Windows.Forms.ComboBox cbSortSrc;
        private System.Windows.Forms.Label lblSouceChannels;
        private System.Windows.Forms.Label lblChannelMapping;
        private System.Windows.Forms.Label lblDestChannels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbEx;
        private System.Windows.Forms.Label lblEx;
        private System.Windows.Forms.VScrollBar vsb;
        private System.Windows.Forms.Button btnPreviewEdit;
        private System.Windows.Forms.ComboBox cbSortDest;
        private System.Windows.Forms.Label lblSortDest;
    }
}