namespace Preview {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class SetupDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private ToolStripMenuItem allChannelsToolStripMenuItem;
		private Button buttonCancel;
		private Button buttonOK;
		private CheckBox checkBoxRedirectOutputs;
		private Label labelBrightness;
		private Label labelChannel;
		private OpenFileDialog openFileDialog;
		private Panel panel1;
		private Panel panelPictureBoxContainer;
		private PictureBox pictureBoxSetupGrid;
		private SaveFileDialog saveFileDialog;
		private ToolStripMenuItem selectedChannelToolStripMenuItem;
		private ToolStrip toolStrip1;
		private ToolStrip toolStrip2;
		private ToolStripButton toolStripButtonClearImage;
		private ToolStripButton toolStripButtonLoadImage;
		private ToolStripButton toolStripButtonReorder;
		private ToolStripButton toolStripButtonResetSize;
		private ToolStripButton toolStripButtonSaveImage;
		private ToolStripComboBox toolStripComboBoxChannels;
		private ToolStripComboBox toolStripComboBoxPixelSize;
		private ToolStripDropDownButton toolStripDropDownButtonClear;
		private ToolStripButton toolStripDropDownButtonUpdate;
		private ToolStripLabel toolStripLabel1;
		private ToolStripLabel toolStripLabel2;
		private ToolStripLabel toolStripLabel3;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripTextBox toolStripTextBoxResolutionX;
		private ToolStripTextBox toolStripTextBoxResolutionY;
		private TrackBar trackBarBrightness;

		private void InitializeComponent() {
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelChannel = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBoxResolutionX = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBoxResolutionY = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripComboBoxPixelSize = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripDropDownButtonUpdate = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonResetSize = new System.Windows.Forms.ToolStripButton();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripComboBoxChannels = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripDropDownButtonClear = new System.Windows.Forms.ToolStripDropDownButton();
			this.allChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonLoadImage = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonClearImage = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSaveImage = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonReorder = new System.Windows.Forms.ToolStripButton();
			this.pictureBoxSetupGrid = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBoxRedirectOutputs = new System.Windows.Forms.CheckBox();
			this.trackBarBrightness = new System.Windows.Forms.TrackBar();
			this.labelBrightness = new System.Windows.Forms.Label();
			this.panelPictureBoxContainer = new System.Windows.Forms.Panel();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetupGrid)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
			this.panelPictureBoxContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(406, 32);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(487, 32);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelChannel
			// 
			this.labelChannel.AutoSize = true;
			this.labelChannel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelChannel.Location = new System.Drawing.Point(5, 11);
			this.labelChannel.Name = "labelChannel";
			this.labelChannel.Size = new System.Drawing.Size(0, 13);
			this.labelChannel.TabIndex = 6;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBoxResolutionX,
            this.toolStripLabel2,
            this.toolStripTextBoxResolutionY,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.toolStripComboBoxPixelSize,
            this.toolStripDropDownButtonUpdate,
            this.toolStripSeparator3,
            this.toolStripButtonResetSize});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(576, 25);
			this.toolStrip1.TabIndex = 8;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(75, 22);
			this.toolStripLabel1.Text = "Resolution:   ";
			// 
			// toolStripTextBoxResolutionX
			// 
			this.toolStripTextBoxResolutionX.MaxLength = 3;
			this.toolStripTextBoxResolutionX.Name = "toolStripTextBoxResolutionX";
			this.toolStripTextBoxResolutionX.Size = new System.Drawing.Size(40, 25);
			this.toolStripTextBoxResolutionX.Text = "64";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(20, 22);
			this.toolStripLabel2.Text = "by";
			// 
			// toolStripTextBoxResolutionY
			// 
			this.toolStripTextBoxResolutionY.MaxLength = 3;
			this.toolStripTextBoxResolutionY.Name = "toolStripTextBoxResolutionY";
			this.toolStripTextBoxResolutionY.Size = new System.Drawing.Size(40, 25);
			this.toolStripTextBoxResolutionY.Text = "32";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(65, 22);
			this.toolStripLabel3.Text = "Pixel size:   ";
			// 
			// toolStripComboBoxPixelSize
			// 
			this.toolStripComboBoxPixelSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxPixelSize.DropDownWidth = 75;
			this.toolStripComboBoxPixelSize.Items.AddRange(new object[] {
            "1 pixel",
            "2 pixels",
            "3 pixels",
            "4 pixels",
            "5 pixels",
            "6 pixels",
            "7 pixels",
            "8 pixels",
            "9 pixels",
            "10 pixels"});
			this.toolStripComboBoxPixelSize.MaxDropDownItems = 10;
			this.toolStripComboBoxPixelSize.Name = "toolStripComboBoxPixelSize";
			this.toolStripComboBoxPixelSize.Size = new System.Drawing.Size(75, 25);
			// 
			// toolStripDropDownButtonUpdate
			// 
			this.toolStripDropDownButtonUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButtonUpdate.Name = "toolStripDropDownButtonUpdate";
			this.toolStripDropDownButtonUpdate.Size = new System.Drawing.Size(49, 22);
			this.toolStripDropDownButtonUpdate.Text = "Update";
			this.toolStripDropDownButtonUpdate.Click += new System.EventHandler(this.toolStripDropDownButtonUpdate_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonResetSize
			// 
			this.toolStripButtonResetSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonResetSize.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonResetSize.Name = "toolStripButtonResetSize";
			this.toolStripButtonResetSize.Size = new System.Drawing.Size(115, 22);
			this.toolStripButtonResetSize.Text = "Reset to picture size";
			this.toolStripButtonResetSize.Click += new System.EventHandler(this.toolStripButtonResetSize_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.AddExtension = false;
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxChannels,
            this.toolStripDropDownButtonClear,
            this.toolStripSeparator2,
            this.toolStripButtonLoadImage,
            this.toolStripButtonClearImage,
            this.toolStripButtonSaveImage,
            this.toolStripSeparator4,
            this.toolStripButtonReorder});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(576, 25);
			this.toolStrip2.TabIndex = 12;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripComboBoxChannels
			// 
			this.toolStripComboBoxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxChannels.Name = "toolStripComboBoxChannels";
			this.toolStripComboBoxChannels.Size = new System.Drawing.Size(200, 25);
			this.toolStripComboBoxChannels.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxChannels_SelectedIndexChanged);
			// 
			// toolStripDropDownButtonClear
			// 
			this.toolStripDropDownButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButtonClear.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allChannelsToolStripMenuItem,
            this.selectedChannelToolStripMenuItem});
			this.toolStripDropDownButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButtonClear.Name = "toolStripDropDownButtonClear";
			this.toolStripDropDownButtonClear.Size = new System.Drawing.Size(47, 22);
			this.toolStripDropDownButtonClear.Text = "Clear";
			// 
			// allChannelsToolStripMenuItem
			// 
			this.allChannelsToolStripMenuItem.Name = "allChannelsToolStripMenuItem";
			this.allChannelsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.allChannelsToolStripMenuItem.Text = "All channels";
			this.allChannelsToolStripMenuItem.Click += new System.EventHandler(this.allChannelsToolStripMenuItem_Click);
			// 
			// selectedChannelToolStripMenuItem
			// 
			this.selectedChannelToolStripMenuItem.Name = "selectedChannelToolStripMenuItem";
			this.selectedChannelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.selectedChannelToolStripMenuItem.Text = "Selected channel";
			this.selectedChannelToolStripMenuItem.Click += new System.EventHandler(this.selectedChannelToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonLoadImage
			// 
			this.toolStripButtonLoadImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonLoadImage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonLoadImage.Name = "toolStripButtonLoadImage";
			this.toolStripButtonLoadImage.Size = new System.Drawing.Size(73, 22);
			this.toolStripButtonLoadImage.Text = "Load image";
			this.toolStripButtonLoadImage.Click += new System.EventHandler(this.toolStripButtonLoadImage_Click);
			// 
			// toolStripButtonClearImage
			// 
			this.toolStripButtonClearImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonClearImage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonClearImage.Name = "toolStripButtonClearImage";
			this.toolStripButtonClearImage.Size = new System.Drawing.Size(74, 22);
			this.toolStripButtonClearImage.Text = "Clear image";
			this.toolStripButtonClearImage.Click += new System.EventHandler(this.toolStripButtonClearImage_Click);
			// 
			// toolStripButtonSaveImage
			// 
			this.toolStripButtonSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonSaveImage.Enabled = false;
			this.toolStripButtonSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveImage.Name = "toolStripButtonSaveImage";
			this.toolStripButtonSaveImage.Size = new System.Drawing.Size(104, 22);
			this.toolStripButtonSaveImage.Text = "Save image to file";
			this.toolStripButtonSaveImage.Click += new System.EventHandler(this.toolStripButtonSaveImage_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			this.toolStripSeparator4.Visible = false;
			// 
			// toolStripButtonReorder
			// 
			this.toolStripButtonReorder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonReorder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonReorder.Name = "toolStripButtonReorder";
			this.toolStripButtonReorder.Size = new System.Drawing.Size(65, 19);
			this.toolStripButtonReorder.Text = "Copy cells";
			this.toolStripButtonReorder.Visible = false;
			this.toolStripButtonReorder.Click += new System.EventHandler(this.toolStripButtonReorder_Click);
			// 
			// pictureBoxSetupGrid
			// 
			this.pictureBoxSetupGrid.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxSetupGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pictureBoxSetupGrid.Location = new System.Drawing.Point(21, 23);
			this.pictureBoxSetupGrid.Name = "pictureBoxSetupGrid";
			this.pictureBoxSetupGrid.Size = new System.Drawing.Size(234, 151);
			this.pictureBoxSetupGrid.TabIndex = 13;
			this.pictureBoxSetupGrid.TabStop = false;
			this.pictureBoxSetupGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxSetupGrid_Paint);
			this.pictureBoxSetupGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
			this.pictureBoxSetupGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.checkBoxRedirectOutputs);
			this.panel1.Controls.Add(this.trackBarBrightness);
			this.panel1.Controls.Add(this.labelBrightness);
			this.panel1.Controls.Add(this.labelChannel);
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 338);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(576, 69);
			this.panel1.TabIndex = 14;
			// 
			// checkBoxRedirectOutputs
			// 
			this.checkBoxRedirectOutputs.Location = new System.Drawing.Point(5, 33);
			this.checkBoxRedirectOutputs.Name = "checkBoxRedirectOutputs";
			this.checkBoxRedirectOutputs.Size = new System.Drawing.Size(144, 31);
			this.checkBoxRedirectOutputs.TabIndex = 9;
			this.checkBoxRedirectOutputs.Text = "Respect channel outputs during playback";
			this.checkBoxRedirectOutputs.UseVisualStyleBackColor = true;
			// 
			// trackBarBrightness
			// 
			this.trackBarBrightness.LargeChange = 1;
			this.trackBarBrightness.Location = new System.Drawing.Point(248, 31);
			this.trackBarBrightness.Maximum = 20;
			this.trackBarBrightness.Name = "trackBarBrightness";
			this.trackBarBrightness.Size = new System.Drawing.Size(144, 45);
			this.trackBarBrightness.TabIndex = 8;
			this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackBarBrightness.Value = 10;
			this.trackBarBrightness.Visible = false;
			this.trackBarBrightness.ValueChanged += new System.EventHandler(this.trackBarBrightness_ValueChanged);
			// 
			// labelBrightness
			// 
			this.labelBrightness.AutoSize = true;
			this.labelBrightness.Location = new System.Drawing.Point(155, 41);
			this.labelBrightness.Name = "labelBrightness";
			this.labelBrightness.Size = new System.Drawing.Size(87, 13);
			this.labelBrightness.TabIndex = 7;
			this.labelBrightness.Text = "Image brightness";
			this.labelBrightness.Visible = false;
			// 
			// panelPictureBoxContainer
			// 
			this.panelPictureBoxContainer.AutoScroll = true;
			this.panelPictureBoxContainer.Controls.Add(this.pictureBoxSetupGrid);
			this.panelPictureBoxContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelPictureBoxContainer.Location = new System.Drawing.Point(0, 50);
			this.panelPictureBoxContainer.Name = "panelPictureBoxContainer";
			this.panelPictureBoxContainer.Size = new System.Drawing.Size(576, 288);
			this.panelPictureBoxContainer.TabIndex = 15;
			// 
			// SetupDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(576, 407);
			this.Controls.Add(this.panelPictureBoxContainer);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(20, 73);
			this.Name = "SetupDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup for sequence preview";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialog_FormClosing);
			this.ResizeBegin += new System.EventHandler(this.SetupDialog_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.SetupDialog_ResizeEnd);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetupDialog_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SetupDialog_KeyUp);
			this.Resize += new System.EventHandler(this.SetupDialog_Resize);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetupGrid)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
			this.panelPictureBoxContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			if (this.m_originalBackground != null) {
				this.m_originalBackground.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}