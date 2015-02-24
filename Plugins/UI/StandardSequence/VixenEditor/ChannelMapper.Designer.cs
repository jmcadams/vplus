using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChannelMapper));
            this.lbDestination = new ListBox();
            this.btnDestinationProfile = new Button();
            this.btnLoadMap = new Button();
            this.btnSaveMap = new Button();
            this.toolTips = new ToolTip(this.components);
            this.btnTransform = new Button();
            this.btnPreviewEdit = new Button();
            this.cbKeepUnmapped = new CheckBox();
            this.btnCancel = new Button();
            this.lblSortSrc = new Label();
            this.cbSortSrc = new ComboBox();
            this.lblSouceChannels = new Label();
            this.lblChannelMapping = new Label();
            this.lblDestChannels = new Label();
            this.panel1 = new Panel();
            this.tbEx = new TextBox();
            this.lblEx = new Label();
            this.vsb = new VScrollBar();
            this.cbSortDest = new ComboBox();
            this.lblSortDest = new Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDestination
            // 
            this.lbDestination.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbDestination.HorizontalScrollbar = true;
            this.lbDestination.ItemHeight = 20;
            this.lbDestination.Location = new Point(586, 54);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.ScrollAlwaysVisible = true;
            this.lbDestination.Size = new Size(186, 484);
            this.lbDestination.TabIndex = 10;
            this.lbDestination.DrawItem += new DrawItemEventHandler(this.lbDestination_DrawItem);
            this.lbDestination.DoubleClick += new EventHandler(this.lbDestination_DoubleClick);
            // 
            // btnDestinationProfile
            // 
            this.btnDestinationProfile.Location = new Point(460, 544);
            this.btnDestinationProfile.Name = "btnDestinationProfile";
            this.btnDestinationProfile.Size = new Size(139, 23);
            this.btnDestinationProfile.TabIndex = 0;
            this.btnDestinationProfile.Text = "New Destination Profile";
            this.btnDestinationProfile.UseVisualStyleBackColor = true;
            this.btnDestinationProfile.Click += new EventHandler(this.btnDestProfile_Click);
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Location = new Point(204, 544);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new Size(75, 23);
            this.btnLoadMap.TabIndex = 3;
            this.btnLoadMap.Text = "Load Map";
            this.toolTips.SetToolTip(this.btnLoadMap, "Load a mapping file.");
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new EventHandler(this.BtnLoadClick);
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.Location = new Point(285, 544);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new Size(75, 23);
            this.btnSaveMap.TabIndex = 4;
            this.btnSaveMap.Text = "Save Map";
            this.toolTips.SetToolTip(this.btnSaveMap, "Save a mapping file.");
            this.btnSaveMap.UseVisualStyleBackColor = true;
            this.btnSaveMap.Click += new EventHandler(this.BtnSaveClick);
            // 
            // btnTransform
            // 
            this.btnTransform.Location = new Point(697, 544);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new Size(75, 23);
            this.btnTransform.TabIndex = 6;
            this.btnTransform.Text = "Transform";
            this.toolTips.SetToolTip(this.btnTransform, "Transform your sequence.");
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new EventHandler(this.BtnTransformClick);
            // 
            // btnPreviewEdit
            // 
            this.btnPreviewEdit.Location = new Point(366, 544);
            this.btnPreviewEdit.Name = "btnPreviewEdit";
            this.btnPreviewEdit.Size = new Size(75, 23);
            this.btnPreviewEdit.TabIndex = 16;
            this.btnPreviewEdit.Text = "Preview";
            this.toolTips.SetToolTip(this.btnPreviewEdit, "Show a Visual Preivew");
            this.btnPreviewEdit.UseVisualStyleBackColor = true;
            this.btnPreviewEdit.Click += new EventHandler(this.BtnPreviewClick);
            // 
            // cbKeepUnmapped
            // 
            this.cbKeepUnmapped.AutoSize = true;
            this.cbKeepUnmapped.Location = new Point(12, 548);
            this.cbKeepUnmapped.Name = "cbKeepUnmapped";
            this.cbKeepUnmapped.Size = new Size(186, 17);
            this.cbKeepUnmapped.TabIndex = 2;
            this.cbKeepUnmapped.Text = "Map Unmapped Source Channels";
            this.cbKeepUnmapped.UseVisualStyleBackColor = true;
            this.cbKeepUnmapped.Click += new EventHandler(this.cbKeepEmpty_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(616, 544);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSortSrc
            // 
            this.lblSortSrc.AutoSize = true;
            this.lblSortSrc.Location = new Point(13, 13);
            this.lblSortSrc.Name = "lblSortSrc";
            this.lblSortSrc.Size = new Size(44, 13);
            this.lblSortSrc.TabIndex = 11;
            this.lblSortSrc.Text = "Sort By:";
            // 
            // cbSortSrc
            // 
            this.cbSortSrc.FormattingEnabled = true;
            this.cbSortSrc.Location = new Point(63, 10);
            this.cbSortSrc.Name = "cbSortSrc";
            this.cbSortSrc.Size = new Size(135, 21);
            this.cbSortSrc.TabIndex = 1;
            this.cbSortSrc.SelectedIndexChanged += new EventHandler(this.SortSrcIndexChanged);
            // 
            // lblSouceChannels
            // 
            this.lblSouceChannels.AutoSize = true;
            this.lblSouceChannels.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblSouceChannels.Location = new Point(9, 38);
            this.lblSouceChannels.Name = "lblSouceChannels";
            this.lblSouceChannels.Size = new Size(87, 13);
            this.lblSouceChannels.TabIndex = 12;
            this.lblSouceChannels.Text = "Source Profile";
            // 
            // lblChannelMapping
            // 
            this.lblChannelMapping.AutoSize = true;
            this.lblChannelMapping.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblChannelMapping.Location = new Point(201, 38);
            this.lblChannelMapping.Name = "lblChannelMapping";
            this.lblChannelMapping.Size = new Size(135, 13);
            this.lblChannelMapping.TabIndex = 13;
            this.lblChannelMapping.Text = "Destination Channel(s)";
            // 
            // lblDestChannels
            // 
            this.lblDestChannels.AutoSize = true;
            this.lblDestChannels.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblDestChannels.Location = new Point(645, 38);
            this.lblDestChannels.Name = "lblDestChannels";
            this.lblDestChannels.Size = new Size(111, 13);
            this.lblDestChannels.TabIndex = 14;
            this.lblDestChannels.Text = "Destination Profile";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbEx);
            this.panel1.Controls.Add(this.lblEx);
            this.panel1.Controls.Add(this.vsb);
            this.panel1.Location = new Point(12, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(568, 472);
            this.panel1.TabIndex = 15;
            // 
            // tbEx
            // 
            this.tbEx.BackColor = SystemColors.Window;
            this.tbEx.Font = new Font("Microsoft Sans Serif", 8.25F);
            this.tbEx.Location = new Point(192, 1);
            this.tbEx.Margin = new Padding(0);
            this.tbEx.Name = "tbEx";
            this.tbEx.Size = new Size(359, 20);
            this.tbEx.TabIndex = 18;
            this.tbEx.TabStop = false;
            this.tbEx.Text = "Example Text";
            this.tbEx.Visible = false;
            // 
            // lblEx
            // 
            this.lblEx.Enabled = false;
            this.lblEx.Location = new Point(0, 0);
            this.lblEx.Name = "lblEx";
            this.lblEx.Size = new Size(186, 20);
            this.lblEx.TabIndex = 17;
            this.lblEx.Text = "Example Label";
            this.lblEx.TextAlign = ContentAlignment.MiddleRight;
            this.lblEx.Visible = false;
            // 
            // vsb
            // 
            this.vsb.Location = new Point(551, 0);
            this.vsb.Name = "vsb";
            this.vsb.Size = new Size(17, 472);
            this.vsb.TabIndex = 16;
            this.vsb.Scroll += new ScrollEventHandler(this.VsbScroll);
            this.vsb.ValueChanged += new EventHandler(this.VsbValueChanged);
            // 
            // cbSortDest
            // 
            this.cbSortDest.FormattingEnabled = true;
            this.cbSortDest.Location = new Point(637, 10);
            this.cbSortDest.Name = "cbSortDest";
            this.cbSortDest.Size = new Size(135, 21);
            this.cbSortDest.TabIndex = 17;
            this.cbSortDest.SelectedIndexChanged += new EventHandler(this.SortDestIndexChanged);
            // 
            // lblSortDest
            // 
            this.lblSortDest.AutoSize = true;
            this.lblSortDest.Location = new Point(587, 13);
            this.lblSortDest.Name = "lblSortDest";
            this.lblSortDest.Size = new Size(44, 13);
            this.lblSortDest.TabIndex = 18;
            this.lblSortDest.Text = "Sort By:";
            // 
            // ChannelMapper
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(784, 574);
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
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelMapper";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Channel Mapper";
            this.FormClosing += new FormClosingEventHandler(this.MapperFormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolTip toolTips;
        private ListBox lbDestination;
        private Button btnDestinationProfile;
        private Button btnLoadMap;
        private Button btnSaveMap;
        private CheckBox cbKeepUnmapped;
        private Button btnTransform;
        private Button btnCancel;
        private Label lblSortSrc;
        private ComboBox cbSortSrc;
        private Label lblSouceChannels;
        private Label lblChannelMapping;
        private Label lblDestChannels;
        private Panel panel1;
        private TextBox tbEx;
        private Label lblEx;
        private VScrollBar vsb;
        private Button btnPreviewEdit;
        private ComboBox cbSortDest;
        private Label lblSortDest;
    }
}