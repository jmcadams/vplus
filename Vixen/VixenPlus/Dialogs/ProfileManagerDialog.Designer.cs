namespace VixenPlus.Dialogs{
    using System.Windows.Forms;
    using System.ComponentModel;

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
        private VixenPlus.TabControl tabControl;
        private TabPage tabEditProfile;
        private TabPage tabProfiles;
        private TextBox textBoxProfileChannelCount;
        private ToolTip toolTip;
        private TreeView treeViewProfile;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnChannelColors = new System.Windows.Forms.Button();
            this.btnChannelMask = new System.Windows.Forms.Button();
            this.btnChannelOutputs = new System.Windows.Forms.Button();
            this.btnDeleteChannelOrder = new System.Windows.Forms.Button();
            this.btnSaveChannelOrder = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new VixenPlus.TabControl(this.components);
            this.tabProfiles = new System.Windows.Forms.TabPage();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.btnAddProfile = new System.Windows.Forms.Button();
            this.listBoxProfiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabEditProfile = new System.Windows.Forms.TabPage();
            this.btnReturnToProfiles = new System.Windows.Forms.Button();
            this.comboBoxChannelOrder = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonPlugins = new System.Windows.Forms.Button();
            this.treeViewProfile = new System.Windows.Forms.TreeView();
            this.buttonChangeProfileName = new System.Windows.Forms.Button();
            this.labelProfileName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAddProfileChannel = new System.Windows.Forms.Button();
            this.buttonRemoveProfileChannels = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxProfileChannelCount = new System.Windows.Forms.TextBox();
            this.buttonAddMultipleProfileChannels = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabProfiles.SuspendLayout();
            this.tabEditProfile.SuspendLayout();
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
            // btnChannelColors
            // 
            this.btnChannelColors.Location = new System.Drawing.Point(28, 114);
            this.btnChannelColors.Name = "btnChannelColors";
            this.btnChannelColors.Size = new System.Drawing.Size(75, 23);
            this.btnChannelColors.TabIndex = 43;
            this.btnChannelColors.Text = "Ch. Colors";
            this.toolTip.SetToolTip(this.btnChannelColors, "Set all channel colors");
            this.btnChannelColors.UseVisualStyleBackColor = true;
            this.btnChannelColors.Click += new System.EventHandler(this.pictureBoxProfileChannelColors_Click);
            // 
            // btnChannelMask
            // 
            this.btnChannelMask.Location = new System.Drawing.Point(28, 84);
            this.btnChannelMask.Name = "btnChannelMask";
            this.btnChannelMask.Size = new System.Drawing.Size(75, 23);
            this.btnChannelMask.TabIndex = 42;
            this.btnChannelMask.Text = "Ch. Mask";
            this.toolTip.SetToolTip(this.btnChannelMask, "Enable/disable channels for this profile");
            this.btnChannelMask.UseVisualStyleBackColor = true;
            this.btnChannelMask.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputMask_Click);
            // 
            // btnChannelOutputs
            // 
            this.btnChannelOutputs.Location = new System.Drawing.Point(28, 54);
            this.btnChannelOutputs.Name = "btnChannelOutputs";
            this.btnChannelOutputs.Size = new System.Drawing.Size(75, 23);
            this.btnChannelOutputs.TabIndex = 41;
            this.btnChannelOutputs.Text = "Ch. Outputs";
            this.toolTip.SetToolTip(this.btnChannelOutputs, "Change the channel outputs");
            this.btnChannelOutputs.UseVisualStyleBackColor = true;
            this.btnChannelOutputs.Click += new System.EventHandler(this.pictureBoxProfileChannelOutputs_Click);
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
            this.tabProfiles.Controls.Add(this.btnRemoveProfile);
            this.tabProfiles.Controls.Add(this.btnEditProfile);
            this.tabProfiles.Controls.Add(this.btnAddProfile);
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
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.Enabled = false;
            this.btnRemoveProfile.Location = new System.Drawing.Point(383, 118);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveProfile.TabIndex = 9;
            this.btnRemoveProfile.Text = "Remove";
            this.btnRemoveProfile.UseVisualStyleBackColor = true;
            this.btnRemoveProfile.Click += new System.EventHandler(this.pictureBoxRemoveProfile_Click);
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.Enabled = false;
            this.btnEditProfile.Location = new System.Drawing.Point(383, 88);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(75, 23);
            this.btnEditProfile.TabIndex = 8;
            this.btnEditProfile.Text = "Edit Profile";
            this.btnEditProfile.UseVisualStyleBackColor = true;
            this.btnEditProfile.Click += new System.EventHandler(this.pictureBoxEditProfile_Click);
            // 
            // btnAddProfile
            // 
            this.btnAddProfile.Location = new System.Drawing.Point(382, 58);
            this.btnAddProfile.Name = "btnAddProfile";
            this.btnAddProfile.Size = new System.Drawing.Size(75, 23);
            this.btnAddProfile.TabIndex = 7;
            this.btnAddProfile.Text = "Add Profile";
            this.btnAddProfile.UseVisualStyleBackColor = true;
            this.btnAddProfile.Click += new System.EventHandler(this.pictureBoxAddProfile_Click);
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
            this.tabEditProfile.Controls.Add(this.btnReturnToProfiles);
            this.tabEditProfile.Controls.Add(this.btnChannelColors);
            this.tabEditProfile.Controls.Add(this.btnChannelMask);
            this.tabEditProfile.Controls.Add(this.btnChannelOutputs);
            this.tabEditProfile.Controls.Add(this.btnDeleteChannelOrder);
            this.tabEditProfile.Controls.Add(this.btnSaveChannelOrder);
            this.tabEditProfile.Controls.Add(this.comboBoxChannelOrder);
            this.tabEditProfile.Controls.Add(this.label7);
            this.tabEditProfile.Controls.Add(this.buttonPlugins);
            this.tabEditProfile.Controls.Add(this.treeViewProfile);
            this.tabEditProfile.Controls.Add(this.buttonChangeProfileName);
            this.tabEditProfile.Controls.Add(this.labelProfileName);
            this.tabEditProfile.Controls.Add(this.label8);
            this.tabEditProfile.Controls.Add(this.buttonAddProfileChannel);
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
            // btnReturnToProfiles
            // 
            this.btnReturnToProfiles.Location = new System.Drawing.Point(12, 395);
            this.btnReturnToProfiles.Name = "btnReturnToProfiles";
            this.btnReturnToProfiles.Size = new System.Drawing.Size(139, 23);
            this.btnReturnToProfiles.TabIndex = 44;
            this.btnReturnToProfiles.Text = "Back to Profile Selection";
            this.btnReturnToProfiles.UseVisualStyleBackColor = true;
            this.btnReturnToProfiles.Click += new System.EventHandler(this.pictureBoxReturnFromChannelGroupEdit_Click);
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
            this.tabEditProfile.ResumeLayout(false);
            this.tabEditProfile.PerformLayout();
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
        private Button btnChannelColors;
        private Button btnChannelMask;
        private Button btnChannelOutputs;
        private Button btnDeleteChannelOrder;
        private Button btnReturnToProfiles;
        private Button btnRemoveProfile;
        private Button btnEditProfile;
        private Button btnAddProfile;
    }
}
