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
            this.lbSource = new System.Windows.Forms.ListBox();
            this.lbMapped = new System.Windows.Forms.ListBox();
            this.lbDestination = new System.Windows.Forms.ListBox();
            this.btnDestProfile = new System.Windows.Forms.Button();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.btnSaveMap = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnTransform = new System.Windows.Forms.Button();
            this.cbKeepUnmapped = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbDestCnt = new System.Windows.Forms.ListBox();
            this.lblSort = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblSouceChannels = new System.Windows.Forms.Label();
            this.lblChannelMapping = new System.Windows.Forms.Label();
            this.lblDestChannels = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbSource
            // 
            this.lbSource.FormattingEnabled = true;
            this.lbSource.Location = new System.Drawing.Point(12, 54);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(215, 472);
            this.lbSource.TabIndex = 7;
            // 
            // lbMapped
            // 
            this.lbMapped.FormattingEnabled = true;
            this.lbMapped.Location = new System.Drawing.Point(228, 54);
            this.lbMapped.Name = "lbMapped";
            this.lbMapped.Size = new System.Drawing.Size(280, 472);
            this.lbMapped.TabIndex = 8;
            // 
            // lbDestination
            // 
            this.lbDestination.FormattingEnabled = true;
            this.lbDestination.Location = new System.Drawing.Point(557, 54);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.Size = new System.Drawing.Size(215, 472);
            this.lbDestination.TabIndex = 10;
            // 
            // btnDestProfile
            // 
            this.btnDestProfile.Location = new System.Drawing.Point(616, 8);
            this.btnDestProfile.Name = "btnDestProfile";
            this.btnDestProfile.Size = new System.Drawing.Size(156, 23);
            this.btnDestProfile.TabIndex = 0;
            this.btnDestProfile.Text = "Load Destination Info";
            this.btnDestProfile.UseVisualStyleBackColor = true;
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Location = new System.Drawing.Point(228, 532);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(75, 23);
            this.btnLoadMap.TabIndex = 3;
            this.btnLoadMap.Text = "Load Map";
            this.toolTips.SetToolTip(this.btnLoadMap, "Load a mapping file.");
            this.btnLoadMap.UseVisualStyleBackColor = true;
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.Location = new System.Drawing.Point(309, 532);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMap.TabIndex = 4;
            this.btnSaveMap.Text = "Save Map";
            this.toolTips.SetToolTip(this.btnSaveMap, "Save a mapping file.");
            this.btnSaveMap.UseVisualStyleBackColor = true;
            // 
            // btnTransform
            // 
            this.btnTransform.Location = new System.Drawing.Point(697, 532);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(75, 23);
            this.btnTransform.TabIndex = 6;
            this.btnTransform.Text = "Transform";
            this.toolTips.SetToolTip(this.btnTransform, "Transform your sequence.");
            this.btnTransform.UseVisualStyleBackColor = true;
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
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(616, 532);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbDestCnt
            // 
            this.lbDestCnt.FormattingEnabled = true;
            this.lbDestCnt.Location = new System.Drawing.Point(520, 54);
            this.lbDestCnt.Name = "lbDestCnt";
            this.lbDestCnt.Size = new System.Drawing.Size(36, 472);
            this.lbDestCnt.TabIndex = 9;
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(13, 13);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(44, 13);
            this.lblSort.TabIndex = 11;
            this.lblSort.Text = "Sort By:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(63, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(164, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // lblSouceChannels
            // 
            this.lblSouceChannels.AutoSize = true;
            this.lblSouceChannels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSouceChannels.Location = new System.Drawing.Point(9, 38);
            this.lblSouceChannels.Name = "lblSouceChannels";
            this.lblSouceChannels.Size = new System.Drawing.Size(103, 13);
            this.lblSouceChannels.TabIndex = 12;
            this.lblSouceChannels.Text = "Source Channels";
            // 
            // lblChannelMapping
            // 
            this.lblChannelMapping.AutoSize = true;
            this.lblChannelMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChannelMapping.Location = new System.Drawing.Point(225, 38);
            this.lblChannelMapping.Name = "lblChannelMapping";
            this.lblChannelMapping.Size = new System.Drawing.Size(105, 13);
            this.lblChannelMapping.TabIndex = 13;
            this.lblChannelMapping.Text = "Channel Mapping";
            // 
            // lblDestChannels
            // 
            this.lblDestChannels.AutoSize = true;
            this.lblDestChannels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestChannels.Location = new System.Drawing.Point(645, 38);
            this.lblDestChannels.Name = "lblDestChannels";
            this.lblDestChannels.Size = new System.Drawing.Size(127, 13);
            this.lblDestChannels.TabIndex = 14;
            this.lblDestChannels.Text = "Destination Channels";
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMap.Location = new System.Drawing.Point(520, 35);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(38, 13);
            this.lblMap.TabIndex = 15;
            this.lblMap.Text = "Map?";
            // 
            // ChannelMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.lblMap);
            this.Controls.Add(this.lblDestChannels);
            this.Controls.Add(this.lblChannelMapping);
            this.Controls.Add(this.lblSouceChannels);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblSort);
            this.Controls.Add(this.lbDestCnt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTransform);
            this.Controls.Add(this.cbKeepUnmapped);
            this.Controls.Add(this.btnSaveMap);
            this.Controls.Add(this.btnLoadMap);
            this.Controls.Add(this.btnDestProfile);
            this.Controls.Add(this.lbDestination);
            this.Controls.Add(this.lbMapped);
            this.Controls.Add(this.lbSource);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelMapper";
            this.Text = "Channel Mapper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.ListBox lbSource;
        private System.Windows.Forms.ListBox lbMapped;
        private System.Windows.Forms.ListBox lbDestination;
        private System.Windows.Forms.Button btnDestProfile;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.Button btnSaveMap;
        private System.Windows.Forms.CheckBox cbKeepUnmapped;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbDestCnt;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblSouceChannels;
        private System.Windows.Forms.Label lblChannelMapping;
        private System.Windows.Forms.Label lblDestChannels;
        private System.Windows.Forms.Label lblMap;
    }
}