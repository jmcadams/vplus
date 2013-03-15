namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;

    public class ProfileManagerDialog : Form
    {
        private Button buttonAddMultipleProfileChannels;
        private Button buttonAddProfileChannel;
        private Button buttonCancel;
        private Button buttonChangeProfileName;
        private Button buttonDone;
        private Button buttonPlugins;
        private Button buttonRemoveProfileChannels;
        private ComboBox comboBoxChannelOrder;
        private IContainer components = null;
        private Label label1;
        private Label label4;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelProfileName;
        private ListBox listBoxProfiles;
        private List<int> m_channelOrderMapping;
        private Profile m_contextProfile = null;
        private PictureBox m_hoveredButton = null;
        private Color m_pictureBoxHoverColor = Color.FromArgb(80, 80, 0xff);
        private SolidBrush m_pictureBrush = new SolidBrush(Color.Black);
        private Color m_pictureDisabledColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
        private Color m_pictureEnabledColor = Color.FromArgb(0x80, 0x80, 0xff);
        private Font m_pictureFont = new Font("Arial", 13f, FontStyle.Bold);
        private Pen m_picturePen = new Pen(Color.Black, 2f);
        private OpenFileDialog openFileDialog;
        private Panel panel1;
        private PictureBox pictureBoxAddProfile;
        private PictureBox pictureBoxEditProfile;
        private PictureBox pictureBoxProfileChannelColors;
        private PictureBox pictureBoxProfileChannelOutputMask;
        private PictureBox pictureBoxProfileChannelOutputs;
        private PictureBox pictureBoxProfileDeleteChannelOrder;
        private PictureBox pictureBoxProfileSaveChannelOrder;
        private PictureBox pictureBoxRemoveProfile;
        private PictureBox pictureBoxReturnFromProfileEdit;
        private Vixen.TabControl tabControl;
        private TabPage tabEditProfile;
        private TabPage tabProfiles;
        private TextBox textBoxProfileChannelCount;
        private ToolTip toolTip;
        private TreeView treeViewProfile;

        public ProfileManagerDialog(object objectInContext)
        {
            this.InitializeComponent();
            foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro"))
            {
                try
                {
                    this.listBoxProfiles.Items.Add(new Profile(str));
                }
                catch
                {
                }
            }
            Bitmap bitmap = new Bitmap(this.pictureBoxReturnFromProfileEdit.Image);
            bitmap.MakeTransparent();
            this.pictureBoxReturnFromProfileEdit.Image = bitmap;
            this.m_channelOrderMapping = new List<int>();
            if (objectInContext is Profile)
            {
                if (this.listBoxProfiles.Items.IndexOf(objectInContext) == -1)
                {
                    this.listBoxProfiles.Items.Add((Profile) objectInContext);
                }
                this.EditProfile((Profile) objectInContext);
            }
            else
            {
                this.tabControl.SelectedTab = this.tabProfiles;
            }
        }

        private void AddProfileChannel()
        {
            int num = this.treeViewProfile.Nodes.Count + 1;
            Channel channelObject = new Channel("Channel " + num.ToString(), 0);
            this.m_contextProfile.AddChannelObject(channelObject);
            this.treeViewProfile.Nodes.Add(channelObject.Name).Tag = channelObject;
            this.m_channelOrderMapping.Add(this.m_channelOrderMapping.Count);
        }

        private void buttonAddMultipleProfileChannels_Click(object sender, EventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(this.textBoxProfileChannelCount.Text);
            }
            catch
            {
                MessageBox.Show("Need to have a valid number for the number of channels to add.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            this.treeViewProfile.BeginUpdate();
            while (num-- > 0)
            {
                this.AddProfileChannel();
            }
            this.treeViewProfile.EndUpdate();
        }

        private void buttonAddProfileChannel_Click(object sender, EventArgs e)
        {
            this.AddProfileChannel();
        }

        private void buttonChangeProfileName_Click(object sender, EventArgs e)
        {
            if (this.ChangeProfileName())
            {
                this.labelProfileName.Text = this.m_contextProfile.Name;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            foreach (Profile profile in this.listBoxProfiles.Items)
            {
                if (!(!(profile.FileName == string.Empty) || this.ChangeProfileName()))
                {
                    base.DialogResult = System.Windows.Forms.DialogResult.None;
                    break;
                }
                profile.SaveToFile();
            }
        }

        private void buttonPlugins_Click(object sender, EventArgs e)
        {
            PluginListDialog dialog = new PluginListDialog(this.m_contextProfile);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonRemoveProfileChannels_Click(object sender, EventArgs e)
        {
            this.RemoveSelectedProfileChannelObjects();
        }

        private bool ChangeProfileName()
        {
            TextQueryDialog dialog = new TextQueryDialog("Profile Name", "Name for this profile", this.m_contextProfile.Name);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_contextProfile.Name = dialog.Response;
                dialog.Dispose();
                return true;
            }
            dialog.Dispose();
            return false;
        }

        private void comboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxChannelOrder.SelectedIndex != -1)
            {
                List<Channel> channels = this.m_contextProfile.Channels;
                if (this.comboBoxChannelOrder.SelectedIndex == 0)
                {
                    if (channels.Count == 0)
                    {
                        MessageBox.Show("There are no channels to reorder.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
                    this.comboBoxChannelOrder.SelectedIndex = -1;
                    List<Channel> channelList = this.m_contextProfile.Channels;
                    ChannelOrderDialog dialog = new ChannelOrderDialog(channelList, this.m_channelOrderMapping);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.m_channelOrderMapping.Clear();
                        foreach (Channel channel in dialog.ChannelMapping)
                        {
                            this.m_channelOrderMapping.Add(channelList.IndexOf(channel));
                        }
                    }
                    dialog.Dispose();
                }
                else if (this.comboBoxChannelOrder.SelectedIndex == (this.comboBoxChannelOrder.Items.Count - 1))
                {
                    this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
                    this.comboBoxChannelOrder.SelectedIndex = -1;
                    this.m_channelOrderMapping.Clear();
                    for (int i = 0; i < channels.Count; i++)
                    {
                        this.m_channelOrderMapping.Add(i);
                    }
                    this.m_contextProfile.LastSort = -1;
                }
                else
                {
                    this.m_channelOrderMapping.Clear();
                    this.m_channelOrderMapping.AddRange(((Vixen.SortOrder) this.comboBoxChannelOrder.SelectedItem).ChannelIndexes);
                    this.m_contextProfile.LastSort = this.comboBoxChannelOrder.SelectedIndex;
                    this.pictureBoxProfileDeleteChannelOrder.Enabled = true;
                }
                this.ReloadProfileChannelObjects();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_pictureFont.Dispose();
            this.m_picturePen.Dispose();
            this.m_pictureBrush.Dispose();
            if (this.m_hoveredButton != null)
            {
                this.m_hoveredButton.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditProfile(Profile profile)
        {
            this.m_contextProfile = profile;
            this.labelProfileName.Text = profile.Name;
            this.UpdateSortList();
            this.ReloadProfileChannelObjects();
            this.comboBoxChannelOrder.SelectedIndex = this.m_contextProfile.LastSort;
            this.tabEditProfile.Tag = this.tabControl.SelectedTab;
            this.tabControl.SelectedTab = this.tabEditProfile;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ProfileManagerDialog));
            this.panel1 = new Panel();
            this.buttonDone = new Button();
            this.toolTip = new ToolTip(this.components);
            this.pictureBoxRemoveProfile = new PictureBox();
            this.pictureBoxEditProfile = new PictureBox();
            this.pictureBoxAddProfile = new PictureBox();
            this.pictureBoxProfileDeleteChannelOrder = new PictureBox();
            this.pictureBoxProfileSaveChannelOrder = new PictureBox();
            this.pictureBoxProfileChannelColors = new PictureBox();
            this.pictureBoxProfileChannelOutputMask = new PictureBox();
            this.pictureBoxProfileChannelOutputs = new PictureBox();
            this.pictureBoxReturnFromProfileEdit = new PictureBox();
            this.openFileDialog = new OpenFileDialog();
            this.tabControl = new Vixen.TabControl(this.components);
            this.tabProfiles = new TabPage();
            this.listBoxProfiles = new ListBox();
            this.label1 = new Label();
            this.tabEditProfile = new TabPage();
            this.comboBoxChannelOrder = new ComboBox();
            this.label7 = new Label();
            this.buttonPlugins = new Button();
            this.treeViewProfile = new TreeView();
            this.buttonChangeProfileName = new Button();
            this.labelProfileName = new Label();
            this.label8 = new Label();
            this.buttonAddProfileChannel = new Button();
            this.buttonRemoveProfileChannels = new Button();
            this.label9 = new Label();
            this.textBoxProfileChannelCount = new TextBox();
            this.buttonAddMultipleProfileChannels = new Button();
            this.label4 = new Label();
            this.buttonCancel = new Button();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxRemoveProfile).BeginInit();
            ((ISupportInitialize) this.pictureBoxEditProfile).BeginInit();
            ((ISupportInitialize) this.pictureBoxAddProfile).BeginInit();
            ((ISupportInitialize) this.pictureBoxProfileDeleteChannelOrder).BeginInit();
            ((ISupportInitialize) this.pictureBoxProfileSaveChannelOrder).BeginInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelColors).BeginInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelOutputMask).BeginInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelOutputs).BeginInit();
            ((ISupportInitialize) this.pictureBoxReturnFromProfileEdit).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabProfiles.SuspendLayout();
            this.tabEditProfile.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonDone);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x1a8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x256, 40);
            this.panel1.TabIndex = 1;
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new Point(430, 6);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 9;
            this.buttonDone.Text = "OK";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
            this.pictureBoxRemoveProfile.Anchor = AnchorStyles.Top;
            this.pictureBoxRemoveProfile.Enabled = false;
            this.pictureBoxRemoveProfile.Location = new Point(0x1b2, 0x3a);
            this.pictureBoxRemoveProfile.Name = "pictureBoxRemoveProfile";
            this.pictureBoxRemoveProfile.Size = new Size(20, 20);
            this.pictureBoxRemoveProfile.TabIndex = 6;
            this.pictureBoxRemoveProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxRemoveProfile, "Remove selected profile");
            this.pictureBoxRemoveProfile.MouseLeave += new EventHandler(this.pictureBoxAddProfile_MouseLeave);
            this.pictureBoxRemoveProfile.Click += new EventHandler(this.pictureBoxRemoveProfile_Click);
            this.pictureBoxRemoveProfile.Paint += new PaintEventHandler(this.pictureBoxRemoveProfile_Paint);
            this.pictureBoxRemoveProfile.MouseEnter += new EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxEditProfile.Anchor = AnchorStyles.Top;
            this.pictureBoxEditProfile.Enabled = false;
            this.pictureBoxEditProfile.Location = new Point(0x198, 0x3a);
            this.pictureBoxEditProfile.Name = "pictureBoxEditProfile";
            this.pictureBoxEditProfile.Size = new Size(20, 20);
            this.pictureBoxEditProfile.TabIndex = 5;
            this.pictureBoxEditProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxEditProfile, "Edit selected profile");
            this.pictureBoxEditProfile.MouseLeave += new EventHandler(this.pictureBoxAddProfile_MouseLeave);
            this.pictureBoxEditProfile.Click += new EventHandler(this.pictureBoxEditProfile_Click);
            this.pictureBoxEditProfile.Paint += new PaintEventHandler(this.pictureBoxEditProfile_Paint);
            this.pictureBoxEditProfile.MouseEnter += new EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxAddProfile.Anchor = AnchorStyles.Top;
            this.pictureBoxAddProfile.Location = new Point(0x17e, 0x3a);
            this.pictureBoxAddProfile.Name = "pictureBoxAddProfile";
            this.pictureBoxAddProfile.Size = new Size(20, 20);
            this.pictureBoxAddProfile.TabIndex = 4;
            this.pictureBoxAddProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxAddProfile, "Add new profile");
            this.pictureBoxAddProfile.MouseLeave += new EventHandler(this.pictureBoxAddProfile_MouseLeave);
            this.pictureBoxAddProfile.Click += new EventHandler(this.pictureBoxAddProfile_Click);
            this.pictureBoxAddProfile.Paint += new PaintEventHandler(this.pictureBoxAddProfile_Paint);
            this.pictureBoxAddProfile.MouseEnter += new EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxProfileDeleteChannelOrder.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
            this.pictureBoxProfileDeleteChannelOrder.Image = (Image) manager.GetObject("pictureBoxProfileDeleteChannelOrder.Image");
            this.pictureBoxProfileDeleteChannelOrder.Location = new Point(0x1f2, 0x139);
            this.pictureBoxProfileDeleteChannelOrder.Name = "pictureBoxProfileDeleteChannelOrder";
            this.pictureBoxProfileDeleteChannelOrder.Size = new Size(0x10, 0x10);
            this.pictureBoxProfileDeleteChannelOrder.TabIndex = 0x26;
            this.pictureBoxProfileDeleteChannelOrder.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileDeleteChannelOrder, "Delete the current channel order");
            this.pictureBoxProfileDeleteChannelOrder.Click += new EventHandler(this.pictureBoxProfileDeleteChannelOrder_Click);
            this.pictureBoxProfileSaveChannelOrder.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxProfileSaveChannelOrder.Image = (Image) manager.GetObject("pictureBoxProfileSaveChannelOrder.Image");
            this.pictureBoxProfileSaveChannelOrder.Location = new Point(0x1dc, 0x139);
            this.pictureBoxProfileSaveChannelOrder.Name = "pictureBoxProfileSaveChannelOrder";
            this.pictureBoxProfileSaveChannelOrder.Size = new Size(0x10, 0x10);
            this.pictureBoxProfileSaveChannelOrder.TabIndex = 0x25;
            this.pictureBoxProfileSaveChannelOrder.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileSaveChannelOrder, "Save the current channel order");
            this.pictureBoxProfileSaveChannelOrder.Click += new EventHandler(this.pictureBoxProfileSaveChannelOrder_Click);
            this.pictureBoxProfileChannelColors.Image = (Image) manager.GetObject("pictureBoxProfileChannelColors.Image");
            this.pictureBoxProfileChannelColors.Location = new Point(0x57, 0x65);
            this.pictureBoxProfileChannelColors.Name = "pictureBoxProfileChannelColors";
            this.pictureBoxProfileChannelColors.Size = new Size(0x10, 0x10);
            this.pictureBoxProfileChannelColors.TabIndex = 0x20;
            this.pictureBoxProfileChannelColors.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelColors, "Set all channel colors");
            this.pictureBoxProfileChannelColors.Click += new EventHandler(this.pictureBoxProfileChannelColors_Click);
            this.pictureBoxProfileChannelOutputMask.Image = (Image) manager.GetObject("pictureBoxProfileChannelOutputMask.Image");
            this.pictureBoxProfileChannelOutputMask.Location = new Point(0x57, 0x4d);
            this.pictureBoxProfileChannelOutputMask.Name = "pictureBoxProfileChannelOutputMask";
            this.pictureBoxProfileChannelOutputMask.Size = new Size(0x10, 0x10);
            this.pictureBoxProfileChannelOutputMask.TabIndex = 0x1f;
            this.pictureBoxProfileChannelOutputMask.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelOutputMask, "Enable/disable channels for this profile");
            this.pictureBoxProfileChannelOutputMask.Click += new EventHandler(this.pictureBoxProfileChannelOutputMask_Click);
            this.pictureBoxProfileChannelOutputs.Image = (Image) manager.GetObject("pictureBoxProfileChannelOutputs.Image");
            this.pictureBoxProfileChannelOutputs.Location = new Point(0x57, 0x35);
            this.pictureBoxProfileChannelOutputs.Name = "pictureBoxProfileChannelOutputs";
            this.pictureBoxProfileChannelOutputs.Size = new Size(0x10, 0x10);
            this.pictureBoxProfileChannelOutputs.TabIndex = 30;
            this.pictureBoxProfileChannelOutputs.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelOutputs, "Change the channel outputs");
            this.pictureBoxProfileChannelOutputs.Click += new EventHandler(this.pictureBoxProfileChannelOutputs_Click);
            this.pictureBoxReturnFromProfileEdit.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.pictureBoxReturnFromProfileEdit.Image = (Image) manager.GetObject("pictureBoxReturnFromProfileEdit.Image");
            this.pictureBoxReturnFromProfileEdit.Location = new Point(11, 0x15d);
            this.pictureBoxReturnFromProfileEdit.Name = "pictureBoxReturnFromProfileEdit";
            this.pictureBoxReturnFromProfileEdit.Size = new Size(0x4a, 0x27);
            this.pictureBoxReturnFromProfileEdit.TabIndex = 0x16;
            this.pictureBoxReturnFromProfileEdit.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxReturnFromProfileEdit, "Return to previous");
            this.pictureBoxReturnFromProfileEdit.Click += new EventHandler(this.pictureBoxReturnFromChannelGroupEdit_Click);
            this.tabControl.Controls.Add(this.tabProfiles);
            this.tabControl.Controls.Add(this.tabEditProfile);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.HideTabs = true;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x256, 0x1a8);
            this.tabControl.TabIndex = 2;
            this.tabProfiles.BackColor = Color.Transparent;
            this.tabProfiles.Controls.Add(this.pictureBoxRemoveProfile);
            this.tabProfiles.Controls.Add(this.pictureBoxEditProfile);
            this.tabProfiles.Controls.Add(this.pictureBoxAddProfile);
            this.tabProfiles.Controls.Add(this.listBoxProfiles);
            this.tabProfiles.Controls.Add(this.label1);
            this.tabProfiles.Location = new Point(0, 0);
            this.tabProfiles.Name = "tabProfiles";
            this.tabProfiles.Padding = new Padding(3);
            this.tabProfiles.Size = new Size(0x256, 0x1a8);
            this.tabProfiles.TabIndex = 0;
            this.tabProfiles.Text = "tabProfiles";
            this.tabProfiles.UseVisualStyleBackColor = true;
            this.listBoxProfiles.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxProfiles.FormattingEnabled = true;
            this.listBoxProfiles.Location = new Point(0xde, 0x3a);
            this.listBoxProfiles.Name = "listBoxProfiles";
            this.listBoxProfiles.Size = new Size(0x9a, 0x108);
            this.listBoxProfiles.TabIndex = 2;
            this.listBoxProfiles.SelectedIndexChanged += new EventHandler(this.listBoxProfiles_SelectedIndexChanged);
            this.listBoxProfiles.DoubleClick += new EventHandler(this.listBoxProfiles_DoubleClick);
            this.listBoxProfiles.KeyDown += new KeyEventHandler(this.listBoxProfiles_KeyDown);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Arial", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x56, 0x18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profiles";
            this.tabEditProfile.BackColor = Color.Transparent;
            this.tabEditProfile.Controls.Add(this.pictureBoxProfileDeleteChannelOrder);
            this.tabEditProfile.Controls.Add(this.pictureBoxProfileSaveChannelOrder);
            this.tabEditProfile.Controls.Add(this.comboBoxChannelOrder);
            this.tabEditProfile.Controls.Add(this.label7);
            this.tabEditProfile.Controls.Add(this.pictureBoxProfileChannelColors);
            this.tabEditProfile.Controls.Add(this.pictureBoxProfileChannelOutputMask);
            this.tabEditProfile.Controls.Add(this.pictureBoxProfileChannelOutputs);
            this.tabEditProfile.Controls.Add(this.buttonPlugins);
            this.tabEditProfile.Controls.Add(this.treeViewProfile);
            this.tabEditProfile.Controls.Add(this.buttonChangeProfileName);
            this.tabEditProfile.Controls.Add(this.labelProfileName);
            this.tabEditProfile.Controls.Add(this.label8);
            this.tabEditProfile.Controls.Add(this.buttonAddProfileChannel);
            this.tabEditProfile.Controls.Add(this.pictureBoxReturnFromProfileEdit);
            this.tabEditProfile.Controls.Add(this.buttonRemoveProfileChannels);
            this.tabEditProfile.Controls.Add(this.label9);
            this.tabEditProfile.Controls.Add(this.textBoxProfileChannelCount);
            this.tabEditProfile.Controls.Add(this.buttonAddMultipleProfileChannels);
            this.tabEditProfile.Controls.Add(this.label4);
            this.tabEditProfile.Location = new Point(0, 0);
            this.tabEditProfile.Name = "tabEditProfile";
            this.tabEditProfile.Size = new Size(0x256, 0x1a8);
            this.tabEditProfile.TabIndex = 2;
            this.tabEditProfile.Text = "tabEditProfile";
            this.tabEditProfile.UseVisualStyleBackColor = true;
            this.comboBoxChannelOrder.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.comboBoxChannelOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxChannelOrder.FormattingEnabled = true;
            this.comboBoxChannelOrder.Items.AddRange(new object[] { "Define new order...", "Restore natural order..." });
            this.comboBoxChannelOrder.Location = new Point(0x15d, 0x134);
            this.comboBoxChannelOrder.Name = "comboBoxChannelOrder";
            this.comboBoxChannelOrder.Size = new Size(0x79, 0x15);
            this.comboBoxChannelOrder.TabIndex = 0x17;
            this.comboBoxChannelOrder.SelectedIndexChanged += new EventHandler(this.comboBoxChannelOrder_SelectedIndexChanged);
            this.label7.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x15a, 0x124);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x49, 13);
            this.label7.TabIndex = 0x16;
            this.label7.Text = "Channel order";
            this.buttonPlugins.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonPlugins.Location = new Point(0x15d, 0x16d);
            this.buttonPlugins.Name = "buttonPlugins";
            this.buttonPlugins.Size = new Size(0x6f, 0x17);
            this.buttonPlugins.TabIndex = 0x1d;
            this.buttonPlugins.Text = "Output plugins";
            this.buttonPlugins.UseVisualStyleBackColor = true;
            this.buttonPlugins.Click += new EventHandler(this.buttonPlugins_Click);
            this.treeViewProfile.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewProfile.Location = new Point(0x6d, 0x35);
            this.treeViewProfile.Name = "treeViewProfile";
            this.treeViewProfile.Size = new Size(230, 0x14f);
            this.treeViewProfile.TabIndex = 0x1b;
            this.treeViewProfile.DoubleClick += new EventHandler(this.treeViewProfile_DoubleClick);
            this.treeViewProfile.AfterSelect += new TreeViewEventHandler(this.treeViewProfile_AfterSelect);
            this.treeViewProfile.KeyDown += new KeyEventHandler(this.treeViewProfile_KeyDown);
            this.buttonChangeProfileName.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonChangeProfileName.Location = new Point(0x15d, 70);
            this.buttonChangeProfileName.Name = "buttonChangeProfileName";
            this.buttonChangeProfileName.Size = new Size(0x4b, 0x17);
            this.buttonChangeProfileName.TabIndex = 0x1a;
            this.buttonChangeProfileName.Text = "Change";
            this.buttonChangeProfileName.UseVisualStyleBackColor = true;
            this.buttonChangeProfileName.Click += new EventHandler(this.buttonChangeProfileName_Click);
            this.labelProfileName.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.labelProfileName.AutoSize = true;
            this.labelProfileName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelProfileName.Location = new Point(390, 0x36);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Size = new Size(0x29, 13);
            this.labelProfileName.TabIndex = 0x19;
            this.labelProfileName.Text = "label7";
            this.label8.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x15a, 0x36);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x26, 13);
            this.label8.TabIndex = 0x18;
            this.label8.Text = "Name:";
            this.buttonAddProfileChannel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonAddProfileChannel.Location = new Point(0x15d, 0x7e);
            this.buttonAddProfileChannel.Name = "buttonAddProfileChannel";
            this.buttonAddProfileChannel.Size = new Size(0x6f, 0x17);
            this.buttonAddProfileChannel.TabIndex = 0x11;
            this.buttonAddProfileChannel.Text = "Add one channel";
            this.buttonAddProfileChannel.UseVisualStyleBackColor = true;
            this.buttonAddProfileChannel.Click += new EventHandler(this.buttonAddProfileChannel_Click);
            this.buttonRemoveProfileChannels.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonRemoveProfileChannels.Enabled = false;
            this.buttonRemoveProfileChannels.Location = new Point(0x15d, 0xe7);
            this.buttonRemoveProfileChannels.Name = "buttonRemoveProfileChannels";
            this.buttonRemoveProfileChannels.Size = new Size(0x4b, 0x17);
            this.buttonRemoveProfileChannels.TabIndex = 0x15;
            this.buttonRemoveProfileChannels.Text = "Remove";
            this.buttonRemoveProfileChannels.UseVisualStyleBackColor = true;
            this.buttonRemoveProfileChannels.Click += new EventHandler(this.buttonRemoveProfileChannels_Click);
            this.label9.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x1c8, 160);
            this.label9.Name = "label9";
            this.label9.Size = new Size(50, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "channels";
            this.textBoxProfileChannelCount.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.textBoxProfileChannelCount.Location = new Point(0x19c, 0x9d);
            this.textBoxProfileChannelCount.Name = "textBoxProfileChannelCount";
            this.textBoxProfileChannelCount.Size = new Size(0x26, 20);
            this.textBoxProfileChannelCount.TabIndex = 0x13;
            this.textBoxProfileChannelCount.Text = "10";
            this.buttonAddMultipleProfileChannels.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonAddMultipleProfileChannels.Location = new Point(0x15d, 0x9b);
            this.buttonAddMultipleProfileChannels.Name = "buttonAddMultipleProfileChannels";
            this.buttonAddMultipleProfileChannels.Size = new Size(0x39, 0x17);
            this.buttonAddMultipleProfileChannels.TabIndex = 0x12;
            this.buttonAddMultipleProfileChannels.Text = "Add";
            this.buttonAddMultipleProfileChannels.UseVisualStyleBackColor = true;
            this.buttonAddMultipleProfileChannels.Click += new EventHandler(this.buttonAddMultipleProfileChannels_Click);
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Arial", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(7, 7);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x88, 0x18);
            this.label4.TabIndex = 1;
            this.label4.Text = "Edit a profile";
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x1ff, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonDone;
            base.ClientSize = new Size(0x256, 0x1d0);
            base.Controls.Add(this.tabControl);
            base.Controls.Add(this.panel1);
            base.Name = "ProfileManagerDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Profiles and Channel Groups";
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxRemoveProfile).EndInit();
            ((ISupportInitialize) this.pictureBoxEditProfile).EndInit();
            ((ISupportInitialize) this.pictureBoxAddProfile).EndInit();
            ((ISupportInitialize) this.pictureBoxProfileDeleteChannelOrder).EndInit();
            ((ISupportInitialize) this.pictureBoxProfileSaveChannelOrder).EndInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelColors).EndInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelOutputMask).EndInit();
            ((ISupportInitialize) this.pictureBoxProfileChannelOutputs).EndInit();
            ((ISupportInitialize) this.pictureBoxReturnFromProfileEdit).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabProfiles.ResumeLayout(false);
            this.tabProfiles.PerformLayout();
            this.tabEditProfile.ResumeLayout(false);
            this.tabEditProfile.PerformLayout();
            base.ResumeLayout(false);
        }

        private void listBoxProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBoxProfiles.SelectedIndex != -1)
            {
                this.EditProfile((Profile) this.listBoxProfiles.SelectedItem);
            }
        }

        private void listBoxProfiles_KeyDown(object sender, KeyEventArgs e)
        {
            if ((this.listBoxProfiles.SelectedIndex != -1) && (e.KeyCode == Keys.Delete))
            {
                this.RemoveProfile((Profile) this.listBoxProfiles.SelectedItem);
            }
        }

        private void listBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBoxEditProfile.Enabled = this.pictureBoxRemoveProfile.Enabled = this.listBoxProfiles.SelectedIndex != -1;
        }

        private void PictureBase(PictureBox pb, Graphics g)
        {
            Color color = (this.m_hoveredButton == pb) ? this.m_pictureBoxHoverColor : (pb.Enabled ? this.m_pictureEnabledColor : this.m_pictureDisabledColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle clientRectangle = pb.ClientRectangle;
            clientRectangle.Inflate(-2, -2);
            this.m_picturePen.Color = color;
            this.m_pictureBrush.Color = color;
            g.FillEllipse(Brushes.White, clientRectangle);
            g.DrawEllipse(this.m_picturePen, clientRectangle);
        }

        private void pictureBoxAddProfile_Click(object sender, EventArgs e)
        {
            TextQueryDialog dialog = new TextQueryDialog("New Profile", "Name for the new profile", string.Empty);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Profile item = new Profile();
                item.Name = dialog.Response;
                this.listBoxProfiles.Items.Add(item);
                this.EditProfile(item);
            }
            dialog.Dispose();
        }

        private void pictureBoxAddProfile_MouseEnter(object sender, EventArgs e)
        {
            this.m_hoveredButton = (PictureBox) sender;
            ((PictureBox) sender).Refresh();
        }

        private void pictureBoxAddProfile_MouseLeave(object sender, EventArgs e)
        {
            this.m_hoveredButton = null;
            ((PictureBox) sender).Refresh();
        }

        private void pictureBoxAddProfile_Paint(object sender, PaintEventArgs e)
        {
            this.PictureBase((PictureBox) sender, e.Graphics);
            e.Graphics.DrawString("+", this.m_pictureFont, this.m_pictureBrush, (float) 3f, (float) 1f);
        }

        private void pictureBoxEditProfile_Click(object sender, EventArgs e)
        {
            this.EditProfile((Profile) this.listBoxProfiles.SelectedItem);
        }

        private void pictureBoxEditProfile_Paint(object sender, PaintEventArgs e)
        {
            this.PictureBase((PictureBox) sender, e.Graphics);
            e.Graphics.DrawLine(this.m_picturePen, 6, 11, 11, 6);
            e.Graphics.DrawLine(this.m_picturePen, 9, 14, 14, 9);
        }

        private void pictureBoxProfileChannelColors_Click(object sender, EventArgs e)
        {
            List<Channel> channels = this.m_contextProfile.Channels;
            AllChannelsColorDialog dialog = new AllChannelsColorDialog(channels);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<Color> channelColors = dialog.ChannelColors;
                for (int i = 0; i < channels.Count; i++)
                {
                    channels[i].Color = channelColors[i];
                }
            }
            dialog.Dispose();
        }

        private void pictureBoxProfileChannelOutputMask_Click(object sender, EventArgs e)
        {
            List<Channel> channels = this.m_contextProfile.Channels;
            ChannelOutputMaskDialog dialog = new ChannelOutputMaskDialog(channels);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Channel channel in channels)
                {
                    channel.Enabled = true;
                }
                foreach (int num in dialog.DisabledChannels)
                {
                    channels[num].Enabled = false;
                }
            }
            dialog.Dispose();
        }

        private void pictureBoxProfileChannelOutputs_Click(object sender, EventArgs e)
        {
            ChannelOrderDialog dialog = new ChannelOrderDialog(this.m_contextProfile.OutputChannels, null, "Channel output mapping");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_contextProfile.OutputChannels = dialog.ChannelMapping;
            }
            dialog.Dispose();
        }

        private void pictureBoxProfileDeleteChannelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Delete channel order '{0}'?", this.comboBoxChannelOrder.Text), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.m_contextProfile.Sorts.Remove((Vixen.SortOrder) this.comboBoxChannelOrder.SelectedItem);
                this.comboBoxChannelOrder.Items.RemoveAt(this.comboBoxChannelOrder.SelectedIndex);
                this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
            }
        }

        private void pictureBoxProfileSaveChannelOrder_Click(object sender, EventArgs e)
        {
            Vixen.SortOrder item = null;
            TextQueryDialog dialog = new TextQueryDialog("New order", "What name would you like to give to this ordering of the channels?", string.Empty);
            DialogResult no = DialogResult.No;
            while (no == DialogResult.No)
            {
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    dialog.Dispose();
                    return;
                }
                no = DialogResult.Yes;
                foreach (Vixen.SortOrder order2 in this.m_contextProfile.Sorts)
                {
                    if (order2.Name == dialog.Response)
                    {
                        if ((no = MessageBox.Show("This name is already in use.\nDo you want to overwrite it?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Cancel)
                        {
                            dialog.Dispose();
                            return;
                        }
                        item = order2;
                        break;
                    }
                }
            }
            dialog.Dispose();
            if (item != null)
            {
                item.ChannelIndexes.Clear();
                item.ChannelIndexes.AddRange(this.m_channelOrderMapping);
                this.comboBoxChannelOrder.SelectedItem = item;
            }
            else
            {
                this.m_contextProfile.Sorts.Add(item = new Vixen.SortOrder(dialog.Response, this.m_channelOrderMapping));
                item.ChannelIndexes.Clear();
                item.ChannelIndexes.AddRange(this.m_channelOrderMapping);
                this.comboBoxChannelOrder.Items.Insert(this.comboBoxChannelOrder.Items.Count - 1, item);
                this.comboBoxChannelOrder.SelectedIndex = this.comboBoxChannelOrder.Items.Count - 2;
            }
        }

        private void pictureBoxRemoveProfile_Click(object sender, EventArgs e)
        {
            this.RemoveProfile((Profile) this.listBoxProfiles.SelectedItem);
        }

        private void pictureBoxRemoveProfile_Paint(object sender, PaintEventArgs e)
        {
            this.PictureBase((PictureBox) sender, e.Graphics);
            e.Graphics.DrawString("-", this.m_pictureFont, this.m_pictureBrush, (float) 4f, (float) 0f);
        }

        private void pictureBoxReturnFromChannelGroupEdit_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab.Tag == this.tabProfiles)
            {
                this.UpdateProfiles();
            }
            else if (this.tabControl.SelectedTab.Tag == this.tabEditProfile)
            {
                this.ReloadProfileChannelObjects();
            }
            this.tabControl.SelectedTab = (TabPage) this.tabControl.SelectedTab.Tag;
        }

        private void ReloadProfileChannelObjects()
        {
            int index = -1;
            if (this.treeViewProfile.SelectedNode != null)
            {
                if (this.treeViewProfile.SelectedNode.Level == 0)
                {
                    if (this.treeViewProfile.SelectedNode.IsExpanded)
                    {
                        index = this.treeViewProfile.SelectedNode.Index;
                    }
                }
                else if ((this.treeViewProfile.SelectedNode.Level == 1) && this.treeViewProfile.SelectedNode.Parent.IsExpanded)
                {
                    index = this.treeViewProfile.SelectedNode.Parent.Index;
                }
            }
            this.buttonRemoveProfileChannels.Enabled = false;
            this.treeViewProfile.BeginUpdate();
            this.treeViewProfile.Nodes.Clear();
            List<Channel> channels = this.m_contextProfile.Channels;
            foreach (int num2 in this.m_channelOrderMapping)
            {
                Channel channel = channels[num2];
                this.treeViewProfile.Nodes.Add(channel.Name).Tag = channel;
            }
            if (index != -1)
            {
                this.treeViewProfile.Nodes[index].Expand();
            }
            this.treeViewProfile.EndUpdate();
        }

        private void RemoveProfile(Profile profile)
        {
            if (MessageBox.Show(string.Format("Remove profile {0}?\n\nThis will affect any sequences that use this profile.", profile.Name), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                File.Delete(profile.FileName);
                this.listBoxProfiles.Items.Remove(profile);
            }
        }

        private void RemoveSelectedProfileChannelObjects()
        {
            if (this.treeViewProfile.SelectedNode.Level != 0)
            {
                this.buttonRemoveProfileChannels.Enabled = false;
            }
            else if (MessageBox.Show("Remove the selected item from this profile?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.m_channelOrderMapping.RemoveAt(this.m_contextProfile.Channels.IndexOf((Channel) this.treeViewProfile.SelectedNode.Tag));
                this.m_contextProfile.RemoveChannelObject((Channel) this.treeViewProfile.SelectedNode.Tag);
                this.treeViewProfile.Nodes.Remove(this.treeViewProfile.SelectedNode);
            }
        }

        private void treeViewProfile_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buttonRemoveProfileChannels.Enabled = (this.treeViewProfile.SelectedNode != null) && (e.Node.Level == 0);
        }

        private void treeViewProfile_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeViewProfile.SelectedNode != null)
            {
                Channel tag = (Channel) this.treeViewProfile.SelectedNode.Tag;
                List<Channel> channels = new List<Channel>();
                foreach (TreeNode node in this.treeViewProfile.Nodes)
                {
                    channels.Add((Channel) node.Tag);
                }
                ChannelPropertyDialog dialog = new ChannelPropertyDialog(channels, tag, false);
                dialog.ShowDialog();
                this.ReloadProfileChannelObjects();
                dialog.Dispose();
            }
        }

        private void treeViewProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if ((this.treeViewProfile.SelectedNode != null) && (e.KeyCode == Keys.Delete))
            {
                this.RemoveSelectedProfileChannelObjects();
            }
        }

        private void UpdateProfiles()
        {
            List<Profile> list = new List<Profile>();
            foreach (Profile profile in this.listBoxProfiles.Items)
            {
                list.Add(profile);
            }
            this.listBoxProfiles.SelectedIndex = -1;
            this.listBoxProfiles.BeginUpdate();
            this.listBoxProfiles.Items.Clear();
            foreach (Profile profile in list)
            {
                this.listBoxProfiles.Items.Add(profile);
            }
            this.listBoxProfiles.EndUpdate();
        }

        private void UpdateSortList()
        {
            this.comboBoxChannelOrder.BeginUpdate();
            string item = (string) this.comboBoxChannelOrder.Items[0];
            string str2 = (string) this.comboBoxChannelOrder.Items[this.comboBoxChannelOrder.Items.Count - 1];
            this.comboBoxChannelOrder.Items.Clear();
            this.comboBoxChannelOrder.Items.Add(item);
            foreach (Vixen.SortOrder order in this.m_contextProfile.Sorts)
            {
                this.comboBoxChannelOrder.Items.Add(order);
            }
            this.comboBoxChannelOrder.Items.Add(str2);
            this.comboBoxChannelOrder.EndUpdate();
            int count = this.m_contextProfile.Channels.Count;
            this.m_channelOrderMapping.Clear();
            for (int i = 0; i < count; i++)
            {
                this.m_channelOrderMapping.Add(i);
            }
        }
    }
}

