namespace Preview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class SetupDialog : Form
    {
        private ToolStripMenuItem allChannelsToolStripMenuItem;
        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxRedirectOutputs;
        private IContainer components;
        private Label labelBrightness;
        private Label labelChannel;
        private string m_backgroundImageFileName;
        private int m_cellSize;
        private Dictionary<int, List<uint>> m_channelDictionary;
        private List<Channel> m_channels;
        private bool m_controlDown;
        private bool m_dirty;
        private Image m_originalBackground;
        private bool m_resizing;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private int m_startChannel;
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

        public SetupDialog(SetupData setupData, XmlNode setupNode, List<Channel> channels, int startChannel)
        {
            int num;
            this.m_setupNode = null;
            this.m_setupData = null;
            this.m_backgroundImageFileName = string.Empty;
            this.m_dirty = false;
            this.m_channelDictionary = new Dictionary<int, List<uint>>();
            this.m_channels = null;
            this.m_controlDown = false;
            this.m_originalBackground = null;
            this.m_resizing = false;
            this.components = null;
            this.InitializeComponent();
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_startChannel = startChannel;
            this.toolStripComboBoxPixelSize.SelectedIndex = 7;
            this.toolStripComboBoxChannels.BeginUpdate();
            for (num = this.m_startChannel; num < channels.Count; num++)
            {
                this.toolStripComboBoxChannels.Items.Add(string.Format("{0}: {1}", num + 1, channels[num].Name));
            }
            this.toolStripComboBoxChannels.EndUpdate();
            this.m_channels = new List<Channel>();
            for (num = this.m_startChannel; num < channels.Count; num++)
            {
                this.m_channels.Add(channels[num]);
            }
            this.UpdateFromSetupNode();
        }

        private void allChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_channelDictionary.Clear();
            this.pictureBoxSetupGrid.Refresh();
            this.m_dirty = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.UpdateSetup();
            this.m_dirty = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_originalBackground != null)
            {
                this.m_originalBackground.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SetupDialog));
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
            ((ISupportInitialize) this.pictureBoxSetupGrid).BeginInit();
            this.panel1.SuspendLayout();
            this.trackBarBrightness.BeginInit();
            this.panelPictureBoxContainer.SuspendLayout();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x196, 0x20);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x1e7, 0x20);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.labelChannel.AutoSize = true;
            this.labelChannel.ForeColor = SystemColors.ControlText;
            this.labelChannel.Location = new Point(5, 11);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new Size(0, 13);
            this.labelChannel.TabIndex = 6;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripLabel1, this.toolStripTextBoxResolutionX, this.toolStripLabel2, this.toolStripTextBoxResolutionY, this.toolStripSeparator1, this.toolStripLabel3, this.toolStripComboBoxPixelSize, this.toolStripDropDownButtonUpdate, this.toolStripSeparator3, this.toolStripButtonResetSize });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x240, 0x19);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x4b, 0x16);
            this.toolStripLabel1.Text = "Resolution:   ";
            this.toolStripTextBoxResolutionX.MaxLength = 3;
            this.toolStripTextBoxResolutionX.Name = "toolStripTextBoxResolutionX";
            this.toolStripTextBoxResolutionX.Size = new Size(40, 0x19);
            this.toolStripTextBoxResolutionX.Text = "64";
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new Size(20, 0x16);
            this.toolStripLabel2.Text = "by";
            this.toolStripTextBoxResolutionY.MaxLength = 3;
            this.toolStripTextBoxResolutionY.Name = "toolStripTextBoxResolutionY";
            this.toolStripTextBoxResolutionY.Size = new Size(40, 0x19);
            this.toolStripTextBoxResolutionY.Text = "32";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new Size(0x41, 0x16);
            this.toolStripLabel3.Text = "Pixel size:   ";
            this.toolStripComboBoxPixelSize.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBoxPixelSize.DropDownWidth = 0x4b;
            this.toolStripComboBoxPixelSize.Items.AddRange(new object[] { "1 pixel", "2 pixels", "3 pixels", "4 pixels", "5 pixels", "6 pixels", "7 pixels", "8 pixels", "9 pixels", "10 pixels" });
            this.toolStripComboBoxPixelSize.MaxDropDownItems = 10;
            this.toolStripComboBoxPixelSize.Name = "toolStripComboBoxPixelSize";
            this.toolStripComboBoxPixelSize.Size = new Size(0x4b, 0x19);
            this.toolStripDropDownButtonUpdate.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonUpdate.Image = (Image) manager.GetObject("toolStripDropDownButtonUpdate.Image");
            this.toolStripDropDownButtonUpdate.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButtonUpdate.Name = "toolStripDropDownButtonUpdate";
            this.toolStripDropDownButtonUpdate.Size = new Size(0x31, 0x16);
            this.toolStripDropDownButtonUpdate.Text = "Update";
            this.toolStripDropDownButtonUpdate.Click += new EventHandler(this.toolStripDropDownButtonUpdate_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.toolStripButtonResetSize.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonResetSize.Image = (Image) manager.GetObject("toolStripButtonResetSize.Image");
            this.toolStripButtonResetSize.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonResetSize.Name = "toolStripButtonResetSize";
            this.toolStripButtonResetSize.Size = new Size(0x73, 0x16);
            this.toolStripButtonResetSize.Text = "Reset to picture size";
            this.toolStripButtonResetSize.Click += new EventHandler(this.toolStripButtonResetSize_Click);
            this.saveFileDialog.AddExtension = false;
            this.toolStrip2.Items.AddRange(new ToolStripItem[] { this.toolStripComboBoxChannels, this.toolStripDropDownButtonClear, this.toolStripSeparator2, this.toolStripButtonLoadImage, this.toolStripButtonClearImage, this.toolStripButtonSaveImage, this.toolStripSeparator4, this.toolStripButtonReorder });
            this.toolStrip2.Location = new Point(0, 0x19);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new Size(0x240, 0x19);
            this.toolStrip2.TabIndex = 12;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStripComboBoxChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBoxChannels.Name = "toolStripComboBoxChannels";
            this.toolStripComboBoxChannels.Size = new Size(200, 0x19);
            this.toolStripComboBoxChannels.SelectedIndexChanged += new EventHandler(this.toolStripComboBoxChannels_SelectedIndexChanged);
            this.toolStripDropDownButtonClear.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonClear.DropDownItems.AddRange(new ToolStripItem[] { this.allChannelsToolStripMenuItem, this.selectedChannelToolStripMenuItem });
            this.toolStripDropDownButtonClear.Image = (Image) manager.GetObject("toolStripDropDownButtonClear.Image");
            this.toolStripDropDownButtonClear.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButtonClear.Name = "toolStripDropDownButtonClear";
            this.toolStripDropDownButtonClear.Size = new Size(0x2f, 0x16);
            this.toolStripDropDownButtonClear.Text = "Clear";
            this.allChannelsToolStripMenuItem.Name = "allChannelsToolStripMenuItem";
            this.allChannelsToolStripMenuItem.Size = new Size(0xa3, 0x16);
            this.allChannelsToolStripMenuItem.Text = "All channels";
            this.allChannelsToolStripMenuItem.Click += new EventHandler(this.allChannelsToolStripMenuItem_Click);
            this.selectedChannelToolStripMenuItem.Name = "selectedChannelToolStripMenuItem";
            this.selectedChannelToolStripMenuItem.Size = new Size(0xa3, 0x16);
            this.selectedChannelToolStripMenuItem.Text = "Selected channel";
            this.selectedChannelToolStripMenuItem.Click += new EventHandler(this.selectedChannelToolStripMenuItem_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.toolStripButtonLoadImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLoadImage.Image = (Image) manager.GetObject("toolStripButtonLoadImage.Image");
            this.toolStripButtonLoadImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonLoadImage.Name = "toolStripButtonLoadImage";
            this.toolStripButtonLoadImage.Size = new Size(0x49, 0x16);
            this.toolStripButtonLoadImage.Text = "Load image";
            this.toolStripButtonLoadImage.Click += new EventHandler(this.toolStripButtonLoadImage_Click);
            this.toolStripButtonClearImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClearImage.Image = (Image) manager.GetObject("toolStripButtonClearImage.Image");
            this.toolStripButtonClearImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonClearImage.Name = "toolStripButtonClearImage";
            this.toolStripButtonClearImage.Size = new Size(0x4a, 0x16);
            this.toolStripButtonClearImage.Text = "Clear image";
            this.toolStripButtonClearImage.Click += new EventHandler(this.toolStripButtonClearImage_Click);
            this.toolStripButtonSaveImage.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveImage.Enabled = false;
            this.toolStripButtonSaveImage.Image = (Image) manager.GetObject("toolStripButtonSaveImage.Image");
            this.toolStripButtonSaveImage.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonSaveImage.Name = "toolStripButtonSaveImage";
            this.toolStripButtonSaveImage.Size = new Size(0x68, 0x16);
            this.toolStripButtonSaveImage.Text = "Save image to file";
            this.toolStripButtonSaveImage.Click += new EventHandler(this.toolStripButtonSaveImage_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.toolStripSeparator4.Visible = false;
            this.toolStripButtonReorder.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonReorder.Image = (Image) manager.GetObject("toolStripButtonReorder.Image");
            this.toolStripButtonReorder.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonReorder.Name = "toolStripButtonReorder";
            this.toolStripButtonReorder.Size = new Size(0x41, 0x13);
            this.toolStripButtonReorder.Text = "Copy cells";
            this.toolStripButtonReorder.Visible = false;
            this.toolStripButtonReorder.Click += new EventHandler(this.toolStripButtonReorder_Click);
            this.pictureBoxSetupGrid.BackColor = Color.Transparent;
            this.pictureBoxSetupGrid.BackgroundImageLayout = ImageLayout.None;
            this.pictureBoxSetupGrid.Location = new Point(0x15, 0x17);
            this.pictureBoxSetupGrid.Name = "pictureBoxSetupGrid";
            this.pictureBoxSetupGrid.Size = new Size(0xea, 0x97);
            this.pictureBoxSetupGrid.TabIndex = 13;
            this.pictureBoxSetupGrid.TabStop = false;
            this.pictureBoxSetupGrid.MouseMove += new MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
            this.pictureBoxSetupGrid.MouseDown += new MouseEventHandler(this.pictureBoxSetupGrid_MouseEvent);
            this.pictureBoxSetupGrid.Paint += new PaintEventHandler(this.pictureBoxSetupGrid_Paint);
            this.panel1.BackColor = SystemColors.ControlLight;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.checkBoxRedirectOutputs);
            this.panel1.Controls.Add(this.trackBarBrightness);
            this.panel1.Controls.Add(this.labelBrightness);
            this.panel1.Controls.Add(this.labelChannel);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x240, 0x45);
            this.panel1.TabIndex = 14;
            this.checkBoxRedirectOutputs.Location = new Point(5, 0x21);
            this.checkBoxRedirectOutputs.Name = "checkBoxRedirectOutputs";
            this.checkBoxRedirectOutputs.Size = new Size(0x90, 0x1f);
            this.checkBoxRedirectOutputs.TabIndex = 9;
            this.checkBoxRedirectOutputs.Text = "Respect channel outputs during playback";
            this.checkBoxRedirectOutputs.UseVisualStyleBackColor = true;
            this.trackBarBrightness.LargeChange = 1;
            this.trackBarBrightness.Location = new Point(0xf8, 0x1f);
            this.trackBarBrightness.Maximum = 20;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new Size(0x90, 0x2d);
            this.trackBarBrightness.TabIndex = 8;
            this.trackBarBrightness.TickStyle = TickStyle.TopLeft;
            this.trackBarBrightness.Value = 10;
            this.trackBarBrightness.Visible = false;
            this.trackBarBrightness.ValueChanged += new EventHandler(this.trackBarBrightness_ValueChanged);
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new Point(0x9b, 0x29);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new Size(0x57, 13);
            this.labelBrightness.TabIndex = 7;
            this.labelBrightness.Text = "Image brightness";
            this.labelBrightness.Visible = false;
            this.panelPictureBoxContainer.AutoScroll = true;
            this.panelPictureBoxContainer.Controls.Add(this.pictureBoxSetupGrid);
            this.panelPictureBoxContainer.Dock = DockStyle.Fill;
            this.panelPictureBoxContainer.Location = new Point(0, 50);
            this.panelPictureBoxContainer.Name = "panelPictureBoxContainer";
            this.panelPictureBoxContainer.Size = new Size(0x240, 0x120);
            this.panelPictureBoxContainer.TabIndex = 15;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x240, 0x197);
            base.Controls.Add(this.panelPictureBoxContainer);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.toolStrip2);
            base.Controls.Add(this.toolStrip1);
            base.KeyPreview = true;
            this.MinimumSize = new Size(20, 0x49);
            base.Name = "SetupDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup for sequence preview";
            base.ResizeBegin += new EventHandler(this.SetupDialog_ResizeBegin);
            base.KeyUp += new KeyEventHandler(this.SetupDialog_KeyUp);
            base.FormClosing += new FormClosingEventHandler(this.SetupDialog_FormClosing);
            base.Resize += new EventHandler(this.SetupDialog_Resize);
            base.KeyDown += new KeyEventHandler(this.SetupDialog_KeyDown);
            base.ResizeEnd += new EventHandler(this.SetupDialog_ResizeEnd);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((ISupportInitialize) this.pictureBoxSetupGrid).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.trackBarBrightness.EndInit();
            this.panelPictureBoxContainer.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void pictureBoxSetupGrid_MouseEvent(object sender, MouseEventArgs e)
        {
            if ((e.X >= 0) && (e.Y >= 0))
            {
                int x = e.X / (this.m_cellSize + 1);
                int y = e.Y / (this.m_cellSize + 1);
                if ((x < this.pictureBoxSetupGrid.Width) && (y < this.pictureBoxSetupGrid.Height))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        this.SetPixelChannelReference(this.toolStripComboBoxChannels.SelectedIndex, x, y);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (!this.m_controlDown)
                        {
                            this.ResetPixelChannelReference(this.toolStripComboBoxChannels.SelectedIndex, x, y);
                        }
                        else
                        {
                            this.ResetPixelChannelReference(-1, x, y);
                        }
                    }
                    else
                    {
                        uint item = (uint) ((x << 0x10) | y);
                        StringBuilder builder = new StringBuilder();
                        foreach (int num4 in this.m_channelDictionary.Keys)
                        {
                            if ((num4 < this.m_channels.Count) && this.m_channelDictionary[num4].Contains(item))
                            {
                                builder.AppendFormat("{0}, ", this.toolStripComboBoxChannels.Items[num4]);
                            }
                        }
                        if (builder.Length > 0)
                        {
                            string str = builder.ToString();
                            this.labelChannel.Text = str.Substring(0, str.Length - 2);
                        }
                        else
                        {
                            this.labelChannel.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void pictureBoxSetupGrid_Paint(object sender, PaintEventArgs e)
        {
            int num;
            int num2;
            bool flag = this.m_originalBackground != null;
            SolidBrush brush = new SolidBrush(Color.Black);
            int selectedIndex = this.toolStripComboBoxChannels.SelectedIndex;
            if (flag)
            {
                e.Graphics.FillRectangle(Brushes.Transparent, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Black, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
            }
            foreach (int num4 in this.m_channelDictionary.Keys)
            {
                if ((num4 != selectedIndex) && (num4 < this.m_channels.Count))
                {
                    brush.Color = Color.FromArgb(0x80, this.m_channels[num4].Color);
                    foreach (uint num5 in this.m_channelDictionary[num4])
                    {
                        num = (int) ((num5 >> 0x10) * (this.m_cellSize + 1));
                        num2 = (int) ((num5 & 0xffff) * (this.m_cellSize + 1));
                        if (e.ClipRectangle.Contains(num, num2))
                        {
                            e.Graphics.FillRectangle(brush, num, num2, this.m_cellSize, this.m_cellSize);
                        }
                    }
                }
            }
            if (this.m_channelDictionary.ContainsKey(selectedIndex))
            {
                Channel channel = this.m_channels[selectedIndex];
                foreach (uint num5 in this.m_channelDictionary[selectedIndex])
                {
                    num = (int) ((num5 >> 0x10) * (this.m_cellSize + 1));
                    num2 = (int) ((num5 & 0xffff) * (this.m_cellSize + 1));
                    if (e.ClipRectangle.Contains(num, num2))
                    {
                        e.Graphics.FillRectangle(channel.Brush, num, num2, this.m_cellSize, this.m_cellSize);
                    }
                }
            }
            brush.Dispose();
        }

        private void ResetPixelChannelReference(int channelIndex, int x, int y)
        {
            if (channelIndex == -1)
            {
                uint item = (uint) ((x << 0x10) | y);
                foreach (List<uint> list in this.m_channelDictionary.Values)
                {
                    list.Remove(item);
                }
                this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
            }
            else
            {
                List<uint> list2;
                if (this.m_channelDictionary.TryGetValue(channelIndex, out list2))
                {
                    list2.Remove((uint) ((x << 0x10) | y));
                    this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
                }
            }
            this.m_dirty = true;
        }

        private void selectedChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxChannels.SelectedIndex != -1)
            {
                this.m_channelDictionary.Remove(this.toolStripComboBoxChannels.SelectedIndex);
                this.pictureBoxSetupGrid.Refresh();
                this.m_dirty = true;
            }
        }

        private void SetBackground(Bitmap bitmap)
        {
            if (this.m_originalBackground != null)
            {
                this.m_originalBackground.Dispose();
            }
            this.m_originalBackground = bitmap;
            this.labelBrightness.Visible = this.trackBarBrightness.Visible = this.toolStripButtonSaveImage.Enabled = bitmap != null;
            this.toolStripTextBoxResolutionX.Enabled = this.toolStripTextBoxResolutionY.Enabled = bitmap == null;
            if (bitmap != null)
            {
                this.trackBarBrightness.Value = 10;
            }
        }

        private void SetBrightness(float value)
        {
            if (this.m_originalBackground != null)
            {
                Image image = new Bitmap(this.m_originalBackground);
                float[][] newColorMatrix = new float[5][];
                float[] numArray2 = new float[5];
                numArray2[0] = 1f;
                newColorMatrix[0] = numArray2;
                numArray2 = new float[5];
                numArray2[1] = 1f;
                newColorMatrix[1] = numArray2;
                numArray2 = new float[5];
                numArray2[2] = 1f;
                newColorMatrix[2] = numArray2;
                numArray2 = new float[5];
                numArray2[3] = 1f;
                newColorMatrix[3] = numArray2;
                numArray2 = new float[5];
                numArray2[0] = value;
                numArray2[1] = value;
                numArray2[2] = value;
                numArray2[4] = 1f;
                newColorMatrix[4] = numArray2;
                ColorMatrix matrix = new ColorMatrix(newColorMatrix);
                Graphics graphics = Graphics.FromImage(image);
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorMatrix(matrix);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                graphics.Dispose();
                imageAttr.Dispose();
                this.pictureBoxSetupGrid.BackgroundImage = image;
            }
        }

        private void SetPictureBoxSize(int width, int height)
        {
            this.pictureBoxSetupGrid.Width = width;
            this.pictureBoxSetupGrid.Height = height;
            this.UpdatePosition();
            this.pictureBoxSetupGrid.Refresh();
        }

        private void SetPixelChannelReference(int channelIndex, int x, int y)
        {
            if (channelIndex != -1)
            {
                List<uint> list;
                if (!this.m_channelDictionary.TryGetValue(channelIndex, out list))
                {
                    list = new List<uint>();
                    this.m_channelDictionary[channelIndex] = list;
                }
                uint item = (uint) ((x << 0x10) | y);
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
                this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
                this.m_dirty = true;
            }
        }

        private void SetupDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_dirty)
            {
                switch (MessageBox.Show("Keep changes?", "Vixen preview", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    case DialogResult.Yes:
                        this.UpdateSetup();
                        break;
                }
            }
        }

        private void SetupDialog_KeyDown(object sender, KeyEventArgs e)
        {
            this.m_controlDown = e.Control;
        }

        private void SetupDialog_KeyUp(object sender, KeyEventArgs e)
        {
            this.m_controlDown = e.Control;
        }

        private void SetupDialog_Resize(object sender, EventArgs e)
        {
            if (!this.m_resizing)
            {
                this.UpdatePosition();
            }
        }

        private void SetupDialog_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void SetupDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
            this.UpdatePosition();
        }

        private void toolStripButtonClearImage_Click(object sender, EventArgs e)
        {
            this.pictureBoxSetupGrid.BackgroundImage = null;
            this.SetBackground(null);
            this.m_backgroundImageFileName = string.Empty;
            this.m_dirty = true;
        }

        private void toolStripButtonLoadImage_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = new FileStream(this.openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
                stream.Close();
                stream.Dispose();
                MemoryStream stream2 = new MemoryStream(buffer);
                Bitmap bitmap = new Bitmap(stream2);
                stream2.Close();
                stream2.Dispose();
                this.SetPictureBoxSize(bitmap.Width, bitmap.Height);
                this.SetBackground(bitmap);
                this.SetBrightness(0f);
                this.m_backgroundImageFileName = this.openFileDialog.FileName;
                this.m_dirty = true;
            }
        }

        private void toolStripButtonReorder_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonResetSize_Click(object sender, EventArgs e)
        {
            if (this.m_originalBackground != null)
            {
                this.SetPictureBoxSize(this.m_originalBackground.Width, this.m_originalBackground.Height);
                this.UpdateSizeUI();
            }
        }

        private void toolStripButtonSaveImage_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                new Bitmap(this.m_originalBackground).Save(Path.ChangeExtension(this.saveFileDialog.FileName, ".jpg"), ImageFormat.Jpeg);
                MessageBox.Show("File saved.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void toolStripComboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBoxSetupGrid.Refresh();
        }

        private void toolStripDropDownButtonUpdate_Click(object sender, EventArgs e)
        {
            int width;
            int height;
            this.m_cellSize = this.toolStripComboBoxPixelSize.SelectedIndex + 1;
            try
            {
                width = int.Parse(this.toolStripTextBoxResolutionX.Text) * (this.m_cellSize + 1);
            }
            catch
            {
                width = this.pictureBoxSetupGrid.Width;
            }
            try
            {
                height = int.Parse(this.toolStripTextBoxResolutionY.Text) * (this.m_cellSize + 1);
            }
            catch
            {
                height = this.pictureBoxSetupGrid.Height;
            }
            this.SetPictureBoxSize(width, height);
        }

        private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
        {
            this.SetBrightness(((float) (this.trackBarBrightness.Value - 10)) / 10f);
        }

        private void UpdateFromSetupNode()
        {
            int num;
            int num2;
            this.checkBoxRedirectOutputs.Checked = this.m_setupData.GetBoolean(this.m_setupNode, "RedirectOutputs", false);
            XmlNode setupDataNode = this.m_setupNode.SelectSingleNode("Display");
            if (setupDataNode != null)
            {
                num2 = Convert.ToInt32(setupDataNode.SelectSingleNode("Height").InnerText);
                num = Convert.ToInt32(setupDataNode.SelectSingleNode("Width").InnerText);
                this.m_cellSize = Convert.ToInt32(setupDataNode.SelectSingleNode("PixelSize").InnerText);
            }
            else
            {
                num2 = 0x20;
                num = 0x40;
                this.m_cellSize = 8;
            }
            this.SetPictureBoxSize(num * (this.m_cellSize + 1), num2 * (this.m_cellSize + 1));
            this.UpdateSizeUI();
            byte[] buffer = Convert.FromBase64String(this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText);
            if (buffer.Length > 0)
            {
                MemoryStream stream = new MemoryStream(buffer);
                this.SetBackground(new Bitmap(stream));
                stream.Dispose();
            }
            else
            {
                this.SetBackground(null);
            }
            if (setupDataNode != null)
            {
                this.trackBarBrightness.Value = this.m_setupData.GetInteger(setupDataNode, "Brightness", 10);
                this.trackBarBrightness_ValueChanged(null, null);
            }
            foreach (XmlNode node2 in this.m_setupNode.SelectNodes("Channels/Channel"))
            {
                int num3 = Convert.ToInt32(node2.Attributes["number"].Value);
                if (num3 >= this.m_startChannel)
                {
                    List<uint> list = new List<uint>();
                    byte[] buffer2 = Convert.FromBase64String(node2.InnerText);
                    for (int i = 0; i < buffer2.Length; i += 4)
                    {
                        list.Add(BitConverter.ToUInt32(buffer2, i));
                    }
                    this.m_channelDictionary[num3 - this.m_startChannel] = list;
                }
            }
            this.pictureBoxSetupGrid.Refresh();
        }

        private void UpdatePosition()
        {
            Point point = new Point();
            if (this.pictureBoxSetupGrid.Width > this.panelPictureBoxContainer.Width)
            {
                point.X = 0;
            }
            else
            {
                point.X = ((this.panelPictureBoxContainer.Width - this.pictureBoxSetupGrid.Width) / 2) + base.ClientRectangle.Left;
            }
            if (this.pictureBoxSetupGrid.Height > this.panelPictureBoxContainer.Height)
            {
                point.Y = 0;
            }
            else
            {
                point.Y = ((this.panelPictureBoxContainer.Height - this.pictureBoxSetupGrid.Height) / 2) + base.ClientRectangle.Top;
            }
            this.pictureBoxSetupGrid.Location = point;
            int num = this.trackBarBrightness.Right - this.labelBrightness.Left;
            int num2 = this.panel1.Width / 2;
            this.labelBrightness.Left = num2 - (num / 2);
            this.trackBarBrightness.Left = (this.labelBrightness.Left + num) - this.trackBarBrightness.Width;
        }

        private void UpdateSetup()
        {
            XmlNode contextNode = Preview.Xml.SetValue(this.m_setupNode, "Display", string.Empty);
            Preview.Xml.SetValue(contextNode, "Height", this.toolStripTextBoxResolutionY.Text);
            Preview.Xml.SetValue(contextNode, "Width", this.toolStripTextBoxResolutionX.Text);
            Preview.Xml.SetValue(contextNode, "PixelSize", this.m_cellSize.ToString());
            Preview.Xml.SetValue(contextNode, "Brightness", this.trackBarBrightness.Value.ToString());
            if (this.m_originalBackground != null)
            {
                if (this.m_backgroundImageFileName != string.Empty)
                {
                    FileStream stream = new FileStream(this.m_backgroundImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int) stream.Length);
                    this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText = Convert.ToBase64String(buffer);
                    stream.Close();
                    stream.Dispose();
                }
            }
            else
            {
                this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText = Convert.ToBase64String(new byte[0]);
            }
            XmlNode emptyNodeAlways = Preview.Xml.GetEmptyNodeAlways(this.m_setupNode, "Channels");
            List<byte> list = new List<byte>();
            foreach (int num in this.m_channelDictionary.Keys)
            {
                list.Clear();
                XmlNode node = Preview.Xml.SetNewValue(emptyNodeAlways, "Channel", string.Empty);
                Preview.Xml.SetAttribute(node, "number", (num + this.m_startChannel).ToString());
                foreach (uint num2 in this.m_channelDictionary[num])
                {
                    list.AddRange(BitConverter.GetBytes(num2));
                }
                node.InnerText = Convert.ToBase64String(list.ToArray());
            }
            this.m_setupData.SetBoolean(this.m_setupNode, "RedirectOutputs", this.checkBoxRedirectOutputs.Checked);
        }

        private void UpdateSizeUI()
        {
            this.toolStripTextBoxResolutionX.Text = (this.pictureBoxSetupGrid.Width / (this.m_cellSize + 1)).ToString();
            this.toolStripTextBoxResolutionY.Text = (this.pictureBoxSetupGrid.Height / (this.m_cellSize + 1)).ToString();
            this.toolStripComboBoxPixelSize.SelectedIndex = this.m_cellSize - 1;
        }
    }
}

