namespace VixenPlus.Dialogs{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    public partial class ProfileManagerDialog{
        private IContainer components;

        #region Windows Form Designer generated code
        private Button buttonAddMultipleProfileChannels;
        private Button buttonAddProfileChannel;
        private Button buttonCancel;
        private Button buttonChangeProfileName;
        private Button buttonDone;
        private Button buttonPlugins;
        private Button buttonRemoveProfileChannels;
        private ComboBox comboBoxChannelOrder;
        private Label label1;
        private Label label4;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelProfileName;
        private ListBox listBoxProfiles;
        private PictureBox m_hoveredButton = null;
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
        private VixenPlus.TabControl tabControl;
        private TabPage tabEditProfile;
        private TabPage tabProfiles;
        private TextBox textBoxProfileChannelCount;
        private ToolTip toolTip;
        private TreeView treeViewProfile;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileManagerDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new VixenPlus.TabControl(this.components);
            this.tabProfiles = new System.Windows.Forms.TabPage();
            this.pictureBoxRemoveProfile = new System.Windows.Forms.PictureBox();
            this.pictureBoxEditProfile = new System.Windows.Forms.PictureBox();
            this.pictureBoxAddProfile = new System.Windows.Forms.PictureBox();
            this.listBoxProfiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabEditProfile = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDeleteChannelOrder = new System.Windows.Forms.Button();
            this.btnSaveChannelOrder = new System.Windows.Forms.Button();
            this.pictureBoxProfileDeleteChannelOrder = new System.Windows.Forms.PictureBox();
            this.pictureBoxProfileSaveChannelOrder = new System.Windows.Forms.PictureBox();
            this.comboBoxChannelOrder = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBoxProfileChannelColors = new System.Windows.Forms.PictureBox();
            this.pictureBoxProfileChannelOutputMask = new System.Windows.Forms.PictureBox();
            this.pictureBoxProfileChannelOutputs = new System.Windows.Forms.PictureBox();
            this.buttonPlugins = new System.Windows.Forms.Button();
            this.treeViewProfile = new System.Windows.Forms.TreeView();
            this.buttonChangeProfileName = new System.Windows.Forms.Button();
            this.labelProfileName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAddProfileChannel = new System.Windows.Forms.Button();
            this.pictureBoxReturnFromProfileEdit = new System.Windows.Forms.PictureBox();
            this.buttonRemoveProfileChannels = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxProfileChannelCount = new System.Windows.Forms.TextBox();
            this.buttonAddMultipleProfileChannels = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabProfiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRemoveProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddProfile)).BeginInit();
            this.tabEditProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileDeleteChannelOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileSaveChannelOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelColors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelOutputMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelOutputs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReturnFromProfileEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonDone);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 424);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(598, 40);
            this.panel1.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(511, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new System.Drawing.Point(430, 6);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 9;
            this.buttonDone.Text = "OK";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabProfiles);
            this.tabControl.Controls.Add(this.tabEditProfile);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HideTabs = true;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.OurMultiline = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(598, 424);
            this.tabControl.TabIndex = 2;
            // 
            // tabProfiles
            // 
            this.tabProfiles.BackColor = System.Drawing.Color.Transparent;
            this.tabProfiles.Controls.Add(this.pictureBoxRemoveProfile);
            this.tabProfiles.Controls.Add(this.pictureBoxEditProfile);
            this.tabProfiles.Controls.Add(this.pictureBoxAddProfile);
            this.tabProfiles.Controls.Add(this.listBoxProfiles);
            this.tabProfiles.Controls.Add(this.label1);
            this.tabProfiles.Location = new System.Drawing.Point(0, 0);
            this.tabProfiles.Name = "tabProfiles";
            this.tabProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabProfiles.Size = new System.Drawing.Size(598, 424);
            this.tabProfiles.TabIndex = 0;
            this.tabProfiles.Text = "tabProfiles";
            this.tabProfiles.UseVisualStyleBackColor = true;
            // 
            // pictureBoxRemoveProfile
            // 
            this.pictureBoxRemoveProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxRemoveProfile.Enabled = false;
            this.pictureBoxRemoveProfile.Location = new System.Drawing.Point(434, 58);
            this.pictureBoxRemoveProfile.Name = "pictureBoxRemoveProfile";
            this.pictureBoxRemoveProfile.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxRemoveProfile.TabIndex = 6;
            this.pictureBoxRemoveProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxRemoveProfile, "Remove selected profile");
            this.pictureBoxRemoveProfile.Click += new System.EventHandler(this.pictureBoxRemoveProfile_Click);
            this.pictureBoxRemoveProfile.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxRemoveProfile_Paint);
            this.pictureBoxRemoveProfile.MouseEnter += new System.EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxRemoveProfile.MouseLeave += new System.EventHandler(this.pictureBoxAddProfile_MouseLeave);
            // 
            // pictureBoxEditProfile
            // 
            this.pictureBoxEditProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxEditProfile.Enabled = false;
            this.pictureBoxEditProfile.Location = new System.Drawing.Point(408, 58);
            this.pictureBoxEditProfile.Name = "pictureBoxEditProfile";
            this.pictureBoxEditProfile.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxEditProfile.TabIndex = 5;
            this.pictureBoxEditProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxEditProfile, "Edit selected profile");
            this.pictureBoxEditProfile.Click += new System.EventHandler(this.pictureBoxEditProfile_Click);
            this.pictureBoxEditProfile.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxEditProfile_Paint);
            this.pictureBoxEditProfile.MouseEnter += new System.EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxEditProfile.MouseLeave += new System.EventHandler(this.pictureBoxAddProfile_MouseLeave);
            // 
            // pictureBoxAddProfile
            // 
            this.pictureBoxAddProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxAddProfile.Location = new System.Drawing.Point(382, 58);
            this.pictureBoxAddProfile.Name = "pictureBoxAddProfile";
            this.pictureBoxAddProfile.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxAddProfile.TabIndex = 4;
            this.pictureBoxAddProfile.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxAddProfile, "Add new profile");
            this.pictureBoxAddProfile.Click += new System.EventHandler(this.pictureBoxAddProfile_Click);
            this.pictureBoxAddProfile.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxAddProfile_Paint);
            this.pictureBoxAddProfile.MouseEnter += new System.EventHandler(this.pictureBoxAddProfile_MouseEnter);
            this.pictureBoxAddProfile.MouseLeave += new System.EventHandler(this.pictureBoxAddProfile_MouseLeave);
            // 
            // listBoxProfiles
            // 
            this.listBoxProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listBoxProfiles.FormattingEnabled = true;
            this.listBoxProfiles.Location = new System.Drawing.Point(222, 58);
            this.listBoxProfiles.Name = "listBoxProfiles";
            this.listBoxProfiles.Size = new System.Drawing.Size(154, 264);
            this.listBoxProfiles.TabIndex = 2;
            this.listBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.listBoxProfiles_SelectedIndexChanged);
            this.listBoxProfiles.DoubleClick += new System.EventHandler(this.listBoxProfiles_DoubleClick);
            this.listBoxProfiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxProfiles_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profiles";
            // 
            // tabEditProfile
            // 
            this.tabEditProfile.BackColor = System.Drawing.Color.Transparent;
            this.tabEditProfile.Controls.Add(this.button4);
            this.tabEditProfile.Controls.Add(this.button3);
            this.tabEditProfile.Controls.Add(this.button2);
            this.tabEditProfile.Controls.Add(this.button1);
            this.tabEditProfile.Controls.Add(this.btnDeleteChannelOrder);
            this.tabEditProfile.Controls.Add(this.btnSaveChannelOrder);
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
            this.tabEditProfile.Location = new System.Drawing.Point(0, 0);
            this.tabEditProfile.Name = "tabEditProfile";
            this.tabEditProfile.Size = new System.Drawing.Size(598, 424);
            this.tabEditProfile.TabIndex = 2;
            this.tabEditProfile.Text = "tabEditProfile";
            this.tabEditProfile.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 43;
            this.button3.Text = "Ch. Colors";
            this.toolTip.SetToolTip(this.button3, "Set all channel colors");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.pictureBoxProfileChannelColors_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "Ch. Mask";
            this.toolTip.SetToolTip(this.button2, "Enable/disable channels for this profile");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputMask_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 41;
            this.button1.Text = "Ch. Outputs";
            this.toolTip.SetToolTip(this.button1, "Change the channel outputs");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputs_Click);
            // 
            // btnDeleteChannelOrder
            // 
            this.btnDeleteChannelOrder.Location = new System.Drawing.Point(476, 253);
            this.btnDeleteChannelOrder.Name = "btnDeleteChannelOrder";
            this.btnDeleteChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteChannelOrder.TabIndex = 40;
            this.btnDeleteChannelOrder.Text = "Delete Order";
            this.toolTip.SetToolTip(this.btnDeleteChannelOrder, "Delete the current channel order");
            this.btnDeleteChannelOrder.UseVisualStyleBackColor = true;
            this.btnDeleteChannelOrder.Click += new System.EventHandler(this.pictureBoxProfileDeleteChannelOrder_Click);
            // 
            // btnSaveChannelOrder
            // 
            this.btnSaveChannelOrder.Location = new System.Drawing.Point(476, 224);
            this.btnSaveChannelOrder.Name = "btnSaveChannelOrder";
            this.btnSaveChannelOrder.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChannelOrder.TabIndex = 39;
            this.btnSaveChannelOrder.Text = "Save Order";
            this.toolTip.SetToolTip(this.btnSaveChannelOrder, "Save the current channel order");
            this.btnSaveChannelOrder.UseVisualStyleBackColor = true;
            this.btnSaveChannelOrder.Click += new System.EventHandler(this.pictureBoxProfileSaveChannelOrder_Click);
            // 
            // pictureBoxProfileDeleteChannelOrder
            // 
            this.pictureBoxProfileDeleteChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxProfileDeleteChannelOrder.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
            this.pictureBoxProfileDeleteChannelOrder.Location = new System.Drawing.Point(378, 253);
            this.pictureBoxProfileDeleteChannelOrder.Name = "pictureBoxProfileDeleteChannelOrder";
            this.pictureBoxProfileDeleteChannelOrder.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProfileDeleteChannelOrder.TabIndex = 38;
            this.pictureBoxProfileDeleteChannelOrder.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileDeleteChannelOrder, "Delete the current channel order");
            this.pictureBoxProfileDeleteChannelOrder.Visible = false;
            this.pictureBoxProfileDeleteChannelOrder.Click += new System.EventHandler(this.pictureBoxProfileDeleteChannelOrder_Click);
            // 
            // pictureBoxProfileSaveChannelOrder
            // 
            this.pictureBoxProfileSaveChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxProfileSaveChannelOrder.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBoxProfileSaveChannelOrder.Location = new System.Drawing.Point(356, 253);
            this.pictureBoxProfileSaveChannelOrder.Name = "pictureBoxProfileSaveChannelOrder";
            this.pictureBoxProfileSaveChannelOrder.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProfileSaveChannelOrder.TabIndex = 37;
            this.pictureBoxProfileSaveChannelOrder.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileSaveChannelOrder, "Save the current channel order");
            this.pictureBoxProfileSaveChannelOrder.Visible = false;
            this.pictureBoxProfileSaveChannelOrder.Click += new System.EventHandler(this.pictureBoxProfileSaveChannelOrder_Click);
            // 
            // comboBoxChannelOrder
            // 
            this.comboBoxChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChannelOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannelOrder.FormattingEnabled = true;
            this.comboBoxChannelOrder.Items.AddRange(new object[] {
            "Define new order...",
            "Restore natural order..."});
            this.comboBoxChannelOrder.Location = new System.Drawing.Point(349, 226);
            this.comboBoxChannelOrder.Name = "comboBoxChannelOrder";
            this.comboBoxChannelOrder.Size = new System.Drawing.Size(121, 21);
            this.comboBoxChannelOrder.TabIndex = 23;
            this.comboBoxChannelOrder.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannelOrder_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(346, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Channel order";
            // 
            // pictureBoxProfileChannelColors
            // 
            this.pictureBoxProfileChannelColors.BackColor = System.Drawing.Color.Gray;
            this.pictureBoxProfileChannelColors.Location = new System.Drawing.Point(87, 301);
            this.pictureBoxProfileChannelColors.Name = "pictureBoxProfileChannelColors";
            this.pictureBoxProfileChannelColors.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProfileChannelColors.TabIndex = 32;
            this.pictureBoxProfileChannelColors.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelColors, "Set all channel colors");
            this.pictureBoxProfileChannelColors.Visible = false;
            this.pictureBoxProfileChannelColors.Click += new System.EventHandler(this.pictureBoxProfileChannelColors_Click);
            // 
            // pictureBoxProfileChannelOutputMask
            // 
            this.pictureBoxProfileChannelOutputMask.BackColor = System.Drawing.Color.DimGray;
            this.pictureBoxProfileChannelOutputMask.Location = new System.Drawing.Point(87, 277);
            this.pictureBoxProfileChannelOutputMask.Name = "pictureBoxProfileChannelOutputMask";
            this.pictureBoxProfileChannelOutputMask.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProfileChannelOutputMask.TabIndex = 31;
            this.pictureBoxProfileChannelOutputMask.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelOutputMask, "Enable/disable channels for this profile");
            this.pictureBoxProfileChannelOutputMask.Visible = false;
            this.pictureBoxProfileChannelOutputMask.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputMask_Click);
            // 
            // pictureBoxProfileChannelOutputs
            // 
            this.pictureBoxProfileChannelOutputs.BackColor = System.Drawing.Color.Black;
            this.pictureBoxProfileChannelOutputs.Location = new System.Drawing.Point(87, 253);
            this.pictureBoxProfileChannelOutputs.Name = "pictureBoxProfileChannelOutputs";
            this.pictureBoxProfileChannelOutputs.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProfileChannelOutputs.TabIndex = 30;
            this.pictureBoxProfileChannelOutputs.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProfileChannelOutputs, "Change the channel outputs");
            this.pictureBoxProfileChannelOutputs.Visible = false;
            this.pictureBoxProfileChannelOutputs.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputs_Click);
            // 
            // buttonPlugins
            // 
            this.buttonPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlugins.Location = new System.Drawing.Point(349, 365);
            this.buttonPlugins.Name = "buttonPlugins";
            this.buttonPlugins.Size = new System.Drawing.Size(111, 23);
            this.buttonPlugins.TabIndex = 29;
            this.buttonPlugins.Text = "Output plugins";
            this.buttonPlugins.UseVisualStyleBackColor = true;
            this.buttonPlugins.Click += new System.EventHandler(this.buttonPlugins_Click);
            // 
            // treeViewProfile
            // 
            this.treeViewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewProfile.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewProfile.Location = new System.Drawing.Point(109, 53);
            this.treeViewProfile.Name = "treeViewProfile";
            this.treeViewProfile.Size = new System.Drawing.Size(230, 335);
            this.treeViewProfile.TabIndex = 27;
            this.treeViewProfile.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewProfile_DrawNode);
            this.treeViewProfile.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProfile_AfterSelect);
            this.treeViewProfile.DoubleClick += new System.EventHandler(this.treeViewProfile_DoubleClick);
            this.treeViewProfile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewProfile_KeyDown);
            // 
            // buttonChangeProfileName
            // 
            this.buttonChangeProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeProfileName.Location = new System.Drawing.Point(349, 70);
            this.buttonChangeProfileName.Name = "buttonChangeProfileName";
            this.buttonChangeProfileName.Size = new System.Drawing.Size(111, 23);
            this.buttonChangeProfileName.TabIndex = 26;
            this.buttonChangeProfileName.Text = "Change Name";
            this.buttonChangeProfileName.UseVisualStyleBackColor = true;
            this.buttonChangeProfileName.Click += new System.EventHandler(this.buttonChangeProfileName_Click);
            // 
            // labelProfileName
            // 
            this.labelProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProfileName.AutoSize = true;
            this.labelProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProfileName.Location = new System.Drawing.Point(390, 54);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Size = new System.Drawing.Size(75, 13);
            this.labelProfileName.TabIndex = 25;
            this.labelProfileName.Text = "ProfileName";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(346, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Name:";
            // 
            // buttonAddProfileChannel
            // 
            this.buttonAddProfileChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddProfileChannel.Location = new System.Drawing.Point(349, 126);
            this.buttonAddProfileChannel.Name = "buttonAddProfileChannel";
            this.buttonAddProfileChannel.Size = new System.Drawing.Size(111, 23);
            this.buttonAddProfileChannel.TabIndex = 17;
            this.buttonAddProfileChannel.Text = "Add one channel";
            this.buttonAddProfileChannel.UseVisualStyleBackColor = true;
            this.buttonAddProfileChannel.Click += new System.EventHandler(this.buttonAddProfileChannel_Click);
            // 
            // pictureBoxReturnFromProfileEdit
            // 
            this.pictureBoxReturnFromProfileEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxReturnFromProfileEdit.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxReturnFromProfileEdit.Image")));
            this.pictureBoxReturnFromProfileEdit.Location = new System.Drawing.Point(28, 253);
            this.pictureBoxReturnFromProfileEdit.Name = "pictureBoxReturnFromProfileEdit";
            this.pictureBoxReturnFromProfileEdit.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxReturnFromProfileEdit.TabIndex = 22;
            this.pictureBoxReturnFromProfileEdit.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxReturnFromProfileEdit, "Return to previous");
            this.pictureBoxReturnFromProfileEdit.Visible = false;
            this.pictureBoxReturnFromProfileEdit.Click += new System.EventHandler(this.pictureBoxReturnFromChannelGroupEdit_Click);
            // 
            // buttonRemoveProfileChannels
            // 
            this.buttonRemoveProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveProfileChannels.Enabled = false;
            this.buttonRemoveProfileChannels.Location = new System.Drawing.Point(349, 184);
            this.buttonRemoveProfileChannels.Name = "buttonRemoveProfileChannels";
            this.buttonRemoveProfileChannels.Size = new System.Drawing.Size(111, 23);
            this.buttonRemoveProfileChannels.TabIndex = 21;
            this.buttonRemoveProfileChannels.Text = "Remove Channel";
            this.buttonRemoveProfileChannels.UseVisualStyleBackColor = true;
            this.buttonRemoveProfileChannels.Click += new System.EventHandler(this.buttonRemoveProfileChannels_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(456, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "channels";
            // 
            // textBoxProfileChannelCount
            // 
            this.textBoxProfileChannelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProfileChannelCount.Location = new System.Drawing.Point(412, 157);
            this.textBoxProfileChannelCount.Name = "textBoxProfileChannelCount";
            this.textBoxProfileChannelCount.Size = new System.Drawing.Size(38, 20);
            this.textBoxProfileChannelCount.TabIndex = 19;
            this.textBoxProfileChannelCount.Text = "10";
            // 
            // buttonAddMultipleProfileChannels
            // 
            this.buttonAddMultipleProfileChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddMultipleProfileChannels.Location = new System.Drawing.Point(349, 155);
            this.buttonAddMultipleProfileChannels.Name = "buttonAddMultipleProfileChannels";
            this.buttonAddMultipleProfileChannels.Size = new System.Drawing.Size(57, 23);
            this.buttonAddMultipleProfileChannels.TabIndex = 18;
            this.buttonAddMultipleProfileChannels.Text = "Add";
            this.buttonAddMultipleProfileChannels.UseVisualStyleBackColor = true;
            this.buttonAddMultipleProfileChannels.Click += new System.EventHandler(this.buttonAddMultipleProfileChannels_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Edit a profile";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 395);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(139, 23);
            this.button4.TabIndex = 44;
            this.button4.Text = "Back to Profile Selection";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.pictureBoxReturnFromChannelGroupEdit_Click);
            // 
            // ProfileManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(598, 464);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.Icon = global::Properties.Resources.VixenPlus;
            this.Name = "ProfileManagerDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Profiles and Channel Groups";
            this.panel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabProfiles.ResumeLayout(false);
            this.tabProfiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRemoveProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddProfile)).EndInit();
            this.tabEditProfile.ResumeLayout(false);
            this.tabEditProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileDeleteChannelOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileSaveChannelOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelColors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelOutputMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileChannelOutputs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReturnFromProfileEdit)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this._pictureFont.Dispose();
            this._picturePen.Dispose();
            this._pictureBrush.Dispose();
            if (this.m_hoveredButton != null)
            {
                this.m_hoveredButton.Dispose();
            }
            base.Dispose(disposing);
        }

        private Button btnSaveChannelOrder;
        private Button button3;
        private Button button2;
        private Button button1;
        private Button btnDeleteChannelOrder;
        private Button button4;
    }
}
