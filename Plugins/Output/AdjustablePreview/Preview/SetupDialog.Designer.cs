using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Preview {
    public partial class SetupDialog {
        private IContainer components = null;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SetupDialog));
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.labelChannel = new Label();
            this.openFileDialog = new OpenFileDialog();
            this.toolStrip1 = new ToolStrip();
            this.toolStripLabel1 = new ToolStripLabel();
            this.toolStripTextBoxResolutionX = new ToolStripTextBox();
            this.toolStripLabel2 = new ToolStripLabel();
            this.toolStripTextBoxResolutionY = new ToolStripTextBox();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStripLabel3 = new ToolStripLabel();
            this.toolStripComboBoxPixelSize = new ToolStripComboBox();
            this.toolStripDropDownButtonUpdate = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.toolStripButtonResetSize = new ToolStripButton();
            this.saveFileDialog = new SaveFileDialog();
            this.toolStrip2 = new ToolStrip();
            this.toolStripComboBoxChannels = new ToolStripComboBox();
            this.toolStripDropDownButtonClear = new ToolStripDropDownButton();
            this.allChannelsToolStripMenuItem = new ToolStripMenuItem();
            this.selectedChannelToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButtonLoadImage = new ToolStripButton();
            this.toolStripButtonClearImage = new ToolStripButton();
            this.toolStripButtonSaveImage = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolStripButtonReorder = new ToolStripButton();
            this.pictureBoxSetupGrid = new PictureBox();
            this.panel1 = new Panel();
            this.checkBoxRedirectOutputs = new CheckBox();
            this.trackBarBrightness = new TrackBar();
            this.labelBrightness = new Label();
            this.panelPictureBoxContainer = new Panel();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxSetupGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.panelPictureBoxContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(468, 32);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(549, 32);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelChannel
            // 
            this.labelChannel.AutoSize = true;
            this.labelChannel.ForeColor = SystemColors.ControlText;
            this.labelChannel.Location = new Point(5, 11);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new Size(0, 13);
            this.labelChannel.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new ToolStripItem[] {
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
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(638, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(75, 22);
            this.toolStripLabel1.Text = "Resolution:   ";
            // 
            // toolStripTextBoxResolutionX
            // 
            this.toolStripTextBoxResolutionX.MaxLength = 3;
            this.toolStripTextBoxResolutionX.Name = "toolStripTextBoxResolutionX";
            this.toolStripTextBoxResolutionX.Size = new Size(40, 25);
            this.toolStripTextBoxResolutionX.Text = "64";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new Size(20, 22);
            this.toolStripLabel2.Text = "by";
            // 
            // toolStripTextBoxResolutionY
            // 
            this.toolStripTextBoxResolutionY.MaxLength = 3;
            this.toolStripTextBoxResolutionY.Name = "toolStripTextBoxResolutionY";
            this.toolStripTextBoxResolutionY.Size = new Size(40, 25);
            this.toolStripTextBoxResolutionY.Text = "32";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new Size(65, 22);
            this.toolStripLabel3.Text = "Pixel size:   ";
            // 
            // toolStripComboBoxPixelSize
            // 
            this.toolStripComboBoxPixelSize.DropDownStyle = ComboBoxStyle.DropDownList;
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
            this.toolStripComboBoxPixelSize.Size = new Size(75, 25);
            // 
            // toolStripDropDownButtonUpdate
            // 
            this.toolStripDropDownButtonUpdate.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonUpdate.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButtonUpdate.Name = "toolStripDropDownButtonUpdate";
            this.toolStripDropDownButtonUpdate.Size = new Size(49, 22);
            this.toolStripDropDownButtonUpdate.Text = "Update";
            this.toolStripDropDownButtonUpdate.Click += new EventHandler(this.toolStripDropDownButtonUpdate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 25);
            // 
            // toolStripButtonResetSize
            // 
            this.toolStripButtonResetSize.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonResetSize.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonResetSize.Name = "toolStripButtonResetSize";
            this.toolStripButtonResetSize.Size = new Size(115, 22);
            this.toolStripButtonResetSize.Text = "Reset to picture size";
            this.toolStripButtonResetSize.Click += new EventHandler(this.toolStripButtonResetSize_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.AddExtension = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new ToolStripItem[] {
            this.toolStripComboBoxChannels,
            this.toolStripDropDownButtonClear,
            this.toolStripSeparator2,
            this.toolStripButtonLoadImage,
            this.toolStripButtonClearImage,
            this.toolStripButtonSaveImage,
            this.toolStripSeparator4,
            this.toolStripButtonReorder});
            this.toolStrip2.Location = new Point(0, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new Size(638, 25);
            this.toolStrip2.TabIndex = 12;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripComboBoxChannels
            // 
            this.toolStripComboBoxChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBoxChannels.Name = "toolStripComboBoxChannels";
            this.toolStripComboBoxChannels.Size = new Size(200, 25);
            this.toolStripComboBoxChannels.SelectedIndexChanged += new EventHandler(this.toolStripComboBoxChannels_SelectedIndexChanged);
            // 
            // toolStripDropDownButtonClear
            // 
            this.toolStripDropDownButtonClear.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonClear.DropDownItems.AddRange(new ToolStripItem[] {
            this.allChannelsToolStripMenuItem,
            this.selectedChannelToolStripMenuItem});
            this.toolStripDropDownButtonClear.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButtonClear.Name = "toolStripDropDownButtonClear";
            this.toolStripDropDownButtonClear.Size = new Size(47, 22);
            this.toolStripDropDownButtonClear.Text = "Clear";
            // 
            // allChannelsToolStripMenuItem
            // 
            this.allChannelsToolStripMenuItem.Name = "allChannelsToolStripMenuItem";
            this.allChannelsToolStripMenuItem.Size = new Size(163, 22);
            this.allChannelsToolStripMenuItem.Text = "All channels";
            this.allChannelsToolStripMenuItem.Click += new EventHandler(this.allChannelsToolStripMenuItem_Click);
            // 
            // selectedChannelToolStripMenuItem
            // 
            this.selectedChannelToolStripMenuItem.Name = "selectedChannelToolStripMenuItem";
            this.selectedChannelToolStripMenuItem.Size = new Size(163, 22);
            this.selectedChannelToolStripMenuItem.Text = "Selected channel";
            this.selectedChannelToolStripMenuItem.Click += new EventHandler(this.selectedChannelToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripButtonLoadImage
            // 
            this.toolStripButtonLoadImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLoadImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonLoadImage.Name = "toolStripButtonLoadImage";
            this.toolStripButtonLoadImage.Size = new Size(73, 22);
            this.toolStripButtonLoadImage.Text = "Load image";
            this.toolStripButtonLoadImage.Click += new EventHandler(this.toolStripButtonLoadImage_Click);
            // 
            // toolStripButtonClearImage
            // 
            this.toolStripButtonClearImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClearImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonClearImage.Name = "toolStripButtonClearImage";
            this.toolStripButtonClearImage.Size = new Size(74, 22);
            this.toolStripButtonClearImage.Text = "Clear image";
            this.toolStripButtonClearImage.Click += new EventHandler(this.toolStripButtonClearImage_Click);
            // 
            // toolStripButtonSaveImage
            // 
            this.toolStripButtonSaveImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveImage.Enabled = false;
            this.toolStripButtonSaveImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonSaveImage.Name = "toolStripButtonSaveImage";
            this.toolStripButtonSaveImage.Size = new Size(104, 22);
            this.toolStripButtonSaveImage.Text = "Save image to file";
            this.toolStripButtonSaveImage.Click += new EventHandler(this.toolStripButtonSaveImage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 25);
            this.toolStripSeparator4.Visible = false;
            // 
            // toolStripButtonReorder
            // 
            this.toolStripButtonReorder.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonReorder.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonReorder.Name = "toolStripButtonReorder";
            this.toolStripButtonReorder.Size = new Size(65, 22);
            this.toolStripButtonReorder.Text = "Copy cells";
            this.toolStripButtonReorder.Click += new EventHandler(this.toolStripButtonReorder_Click);
            // 
            // pictureBoxSetupGrid
            // 
            this.pictureBoxSetupGrid.BackColor = Color.Transparent;
            this.pictureBoxSetupGrid.BackgroundImageLayout = ImageLayout.None;
            this.pictureBoxSetupGrid.Location = new Point(21, 23);
            this.pictureBoxSetupGrid.Name = "pictureBoxSetupGrid";
            this.pictureBoxSetupGrid.Size = new Size(234, 151);
            this.pictureBoxSetupGrid.TabIndex = 13;
            this.pictureBoxSetupGrid.TabStop = false;
            this.pictureBoxSetupGrid.Paint += new PaintEventHandler(this.pictureBoxSetupGrid_Paint);
            this.pictureBoxSetupGrid.MouseDown += new MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
            this.pictureBoxSetupGrid.MouseMove += new MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
            // 
            // panel1
            // 
            this.panel1.BackColor = SystemColors.ControlLight;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.checkBoxRedirectOutputs);
            this.panel1.Controls.Add(this.trackBarBrightness);
            this.panel1.Controls.Add(this.labelBrightness);
            this.panel1.Controls.Add(this.labelChannel);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 338);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(638, 69);
            this.panel1.TabIndex = 14;
            // 
            // checkBoxRedirectOutputs
            // 
            this.checkBoxRedirectOutputs.Location = new Point(5, 33);
            this.checkBoxRedirectOutputs.Name = "checkBoxRedirectOutputs";
            this.checkBoxRedirectOutputs.Size = new Size(144, 31);
            this.checkBoxRedirectOutputs.TabIndex = 9;
            this.checkBoxRedirectOutputs.Text = "Respect channel outputs during playback";
            this.checkBoxRedirectOutputs.UseVisualStyleBackColor = true;
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.LargeChange = 1;
            this.trackBarBrightness.Location = new Point(248, 31);
            this.trackBarBrightness.Maximum = 20;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new Size(144, 45);
            this.trackBarBrightness.TabIndex = 8;
            this.trackBarBrightness.TickStyle = TickStyle.TopLeft;
            this.trackBarBrightness.Value = 10;
            this.trackBarBrightness.Visible = false;
            this.trackBarBrightness.ValueChanged += new EventHandler(this.trackBarBrightness_ValueChanged);
            // 
            // labelBrightness
            // 
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new Point(155, 41);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new Size(87, 13);
            this.labelBrightness.TabIndex = 7;
            this.labelBrightness.Text = "Image brightness";
            this.labelBrightness.Visible = false;
            // 
            // panelPictureBoxContainer
            // 
            this.panelPictureBoxContainer.AutoScroll = true;
            this.panelPictureBoxContainer.Controls.Add(this.pictureBoxSetupGrid);
            this.panelPictureBoxContainer.Dock = DockStyle.Fill;
            this.panelPictureBoxContainer.Location = new Point(0, 50);
            this.panelPictureBoxContainer.Name = "panelPictureBoxContainer";
            this.panelPictureBoxContainer.Size = new Size(638, 288);
            this.panelPictureBoxContainer.TabIndex = 15;
            // 
            // SetupDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(638, 407);
            this.Controls.Add(this.panelPictureBoxContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new Size(20, 73);
            this.Name = "SetupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup for sequence preview";
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += new FormClosingEventHandler(this.SetupDialog_FormClosing);
            this.ResizeBegin += new EventHandler(this.SetupDialog_ResizeBegin);
            this.ResizeEnd += new EventHandler(this.SetupDialog_ResizeEnd);
            this.KeyDown += new KeyEventHandler(this.SetupDialog_KeyDown);
            this.KeyUp += new KeyEventHandler(this.SetupDialog_KeyUp);
            this.Resize += new EventHandler(this.SetupDialog_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((ISupportInitialize)(this.pictureBoxSetupGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.panelPictureBoxContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            if (this._originalBackground != null) {
                this._originalBackground.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
