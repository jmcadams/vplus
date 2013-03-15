namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Vixen;

    public class NewSequenceWizardDialog : Form
    {
        private Button buttonAssignAudio;
        private Button buttonCancel;
        private Button buttonImportChannels;
        private Button buttonNext;
        private Button buttonOK;
        private Button buttonPrev;
        private Button buttonProfileManager;
        private Button buttonSetupPlugins;
        private Button buttonSkip;
        private ComboBox comboBoxProfiles;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelEffect;
        private Label labelExplanation;
        private Label labelNamesChannels;
        private Label labelNotes;
        private Label labelWhat;
        private Label labelWhy;
        private bool m_back = false;
        private string[,] m_explanations = new string[,] { { string.Empty, string.Empty, string.Empty, string.Empty }, { "The event period length is how long a single on/off event lasts.", "A sequence is made up of a series of events of all the same length.  At every event, the program updates the controller with new data if a change needs to be made.", "The smaller the event period, the finer the control you over the timing of the events.  Generally, you don't want to have the event period be any shorter than you need it to be in order to synchronize to your selected audio.", "100 milliseconds, or 10 updates/second, is adequate for synchronizing to most audio." }, { "A profile contains information about channels and plugins.", "To reduce the otherwise repetitive task of setting up channels and plugins for every sequence you create.", "By selecting a profile, the channel count, channel names, channel colors, channel outputs, and plugin setup will all be done for you.", "Profiles are not required, but can really help you quickly create new sequences.  Profiles can be created by hand or from existing sequences." }, { "A channel is an independently-controlled circuit of lights.", "For every area of your display that you want to control independently, you will want to create a channel.  Sometimes you may need multiple channels assigned to a specific area or structure to adequately meet power limitations.", "For every channel you define in your sequence there will be a row defined in the event grid.", "The channel count can be changed at any time.  If you try to reduce your channel count, you will be warned about the potential loss of data.  Channel names can be easily imported from a file by going to Sequence/Import, which also affects the channel count." }, { "Names for the channels defined earlier.", "For easier identification of a channel's purpose and location.", string.Empty, string.Empty }, { "The output plugins are what Vixen uses to communicate with the controllers.  They translate the data sent from Vixen into a format that the controllers can understand.", "Without specifying at least one plugin, Vixen cannot communicate with any controller.", string.Empty, "If you select plugins to use with this sequence, they need to be setup before they can be used.  The plugins and their respective setup can be changed at any time.  While Vixen supports using multiple plugins simultaneously, setting up one is adequate in most installations." }, { "Here you can assign audio and create event patterns based on the music (or without music).  As the music plays, you tap a pattern on your keyboard on a channel-by-channel basis.", "The sequence can be authored to be synchronized with audio.  The music would be played by Vixen at the same time the sequence is being executed.\nEvent patterns can help set up the initial event timings which can later be cleaned up with manual editing.  It's much easier to establish the initial synchronized timings this way than by hand.", "The event grid will be populated with the created events.  The events are on/off values only.", "The audio can be added or removed at any time during editing.  The length of the sequence can be auto-sized to exactly fit the music, or it can be longer.  The sequence length cannot be shorter than any associated music.\nDefining event patterns can also be used to mark events of significance in the music.  The event patterns can be recreated at any time by going to Sequences/Music." }, { "The length of the sequence in minutes and seconds.", string.Empty, string.Empty, "If there is audio assigned to a sequence, the sequence length cannot be shorter than the music length." } };
        private Stack<int> m_history;
        private Preference2 m_preferences;
        private EventSequence m_sequence;
        private bool m_skip = false;
        private OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl tabControl;
        private TabPage tabPageAudio;
        private TabPage tabPageChannelCount;
        private TabPage tabPageChannelNames;
        private TabPage tabPageEventPeriod;
        private TabPage tabPagePlugin;
        private TabPage tabPageProfile;
        private TabPage tabPageStart;
        private TabPage tabPageTime;
        private TextBox textBoxChannelCount;
        private TextBox textBoxChannelNames;
        private TextBox textBoxEventPeriod;
        private TextBox textBoxTime;

        public NewSequenceWizardDialog(Preference2 preferences)
        {
            this.InitializeComponent();
            this.openFileDialog.InitialDirectory = Paths.SequencePath;
            this.tabControl.SelectedIndex = 0;
            this.m_preferences = preferences;
            this.m_sequence = new EventSequence(preferences);
            this.PopulateProfileList();
            string str = string.Empty;
            if ((str = preferences.GetString("DefaultProfile")).Length > 0)
            {
                this.comboBoxProfiles.SelectedIndex = this.comboBoxProfiles.Items.IndexOf(str);
            }
            this.m_history = new Stack<int>();
            this.UpdateExplanations(0);
        }

        private void buttonAssignAudio_Click(object sender, EventArgs e)
        {
            int integer = this.m_preferences.GetInteger("SoundDevice");
            AudioDialog dialog = new AudioDialog(this.m_sequence, this.m_preferences.GetBoolean("EventSequenceAutoSize"), integer);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetSequenceTime();
            }
            dialog.Dispose();
        }

        private void buttonImportChannels_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventSequence sequence = new EventSequence(this.openFileDialog.FileName);
                this.textBoxChannelCount.Text = sequence.ChannelCount.ToString();
                StringBuilder builder = new StringBuilder();
                foreach (Channel channel in sequence.Channels)
                {
                    builder.AppendLine(channel.Name);
                }
                this.textBoxChannelNames.Text = builder.ToString();
                this.m_sequence.Channels.Clear();
                this.m_sequence.Channels.AddRange(sequence.Channels);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.m_skip = false;
            this.m_back = false;
            if (this.tabControl.SelectedTab == this.tabPageProfile)
            {
                if (this.comboBoxProfiles.SelectedIndex == 0)
                {
                    this.tabControl.SelectedTab = this.tabPageChannelCount;
                }
                else
                {
                    this.tabControl.SelectedTab = this.tabPageAudio;
                }
            }
            else
            {
                this.tabControl.SelectedIndex++;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.tabControl_Deselecting(null, new TabControlCancelEventArgs(this.tabControl.SelectedTab, this.tabControl.SelectedIndex, false, TabControlAction.Deselecting));
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            this.m_back = true;
            this.tabControl.SelectedIndex = this.m_history.Pop();
            this.buttonPrev.Enabled = this.m_history.Count > 0;
        }

        private void buttonProfileManager_Click(object sender, EventArgs e)
        {
            ProfileManagerDialog dialog = new ProfileManagerDialog(null);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.PopulateProfileList();
            }
            dialog.Dispose();
        }

        private void buttonSetupPlugins_Click(object sender, EventArgs e)
        {
            PluginListDialog dialog = new PluginListDialog(this.m_sequence);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            this.m_skip = true;
            this.m_back = false;
            this.tabControl.SelectedIndex++;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(NewSequenceWizardDialog));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageStart = new TabPage();
            this.label2 = new Label();
            this.label1 = new Label();
            this.tabPageEventPeriod = new TabPage();
            this.label12 = new Label();
            this.textBoxEventPeriod = new TextBox();
            this.label7 = new Label();
            this.tabPageProfile = new TabPage();
            this.comboBoxProfiles = new ComboBox();
            this.label16 = new Label();
            this.label14 = new Label();
            this.buttonProfileManager = new Button();
            this.label15 = new Label();
            this.tabPageChannelCount = new TabPage();
            this.buttonImportChannels = new Button();
            this.label9 = new Label();
            this.textBoxChannelCount = new TextBox();
            this.label3 = new Label();
            this.tabPageChannelNames = new TabPage();
            this.labelNamesChannels = new Label();
            this.textBoxChannelNames = new TextBox();
            this.label10 = new Label();
            this.label4 = new Label();
            this.tabPagePlugin = new TabPage();
            this.label13 = new Label();
            this.buttonSetupPlugins = new Button();
            this.label8 = new Label();
            this.tabPageAudio = new TabPage();
            this.buttonAssignAudio = new Button();
            this.label5 = new Label();
            this.tabPageTime = new TabPage();
            this.label11 = new Label();
            this.textBoxTime = new TextBox();
            this.label6 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1 = new GroupBox();
            this.labelExplanation = new Label();
            this.labelNotes = new Label();
            this.labelEffect = new Label();
            this.labelWhy = new Label();
            this.labelWhat = new Label();
            this.buttonPrev = new Button();
            this.buttonNext = new Button();
            this.buttonSkip = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.tabPageStart.SuspendLayout();
            this.tabPageEventPeriod.SuspendLayout();
            this.tabPageProfile.SuspendLayout();
            this.tabPageChannelCount.SuspendLayout();
            this.tabPageChannelNames.SuspendLayout();
            this.tabPagePlugin.SuspendLayout();
            this.tabPageAudio.SuspendLayout();
            this.tabPageTime.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControl.Controls.Add(this.tabPageStart);
            this.tabControl.Controls.Add(this.tabPageEventPeriod);
            this.tabControl.Controls.Add(this.tabPageProfile);
            this.tabControl.Controls.Add(this.tabPageChannelCount);
            this.tabControl.Controls.Add(this.tabPageChannelNames);
            this.tabControl.Controls.Add(this.tabPagePlugin);
            this.tabControl.Controls.Add(this.tabPageAudio);
            this.tabControl.Controls.Add(this.tabPageTime);
            this.tabControl.Location = new Point(0, -22);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x1c0, 0x10f);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.Deselecting += new TabControlCancelEventHandler(this.tabControl_Deselecting);
            this.tabPageStart.Controls.Add(this.label2);
            this.tabPageStart.Controls.Add(this.label1);
            this.tabPageStart.Location = new Point(4, 0x16);
            this.tabPageStart.Name = "tabPageStart";
            this.tabPageStart.Size = new Size(440, 0xf5);
            this.tabPageStart.TabIndex = 7;
            this.tabPageStart.Text = "tabPageStart";
            this.tabPageStart.UseVisualStyleBackColor = true;
            this.label2.Location = new Point(0x19, 60);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x187, 0x72);
            this.label2.TabIndex = 1;
            this.label2.Text = manager.GetString("label2.Text");
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.label1.Location = new Point(0x18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xb5, 0x13);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Sequence Wizard";
            this.tabPageEventPeriod.Controls.Add(this.label12);
            this.tabPageEventPeriod.Controls.Add(this.textBoxEventPeriod);
            this.tabPageEventPeriod.Controls.Add(this.label7);
            this.tabPageEventPeriod.Location = new Point(4, 0x16);
            this.tabPageEventPeriod.Name = "tabPageEventPeriod";
            this.tabPageEventPeriod.Size = new Size(440, 0xf5);
            this.tabPageEventPeriod.TabIndex = 8;
            this.tabPageEventPeriod.Text = "tabPageEventPeriod";
            this.tabPageEventPeriod.UseVisualStyleBackColor = true;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0xd8, 0x73);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x3f, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "milliseconds";
            this.textBoxEventPeriod.Location = new Point(0xaf, 0x70);
            this.textBoxEventPeriod.Name = "textBoxEventPeriod";
            this.textBoxEventPeriod.Size = new Size(0x23, 20);
            this.textBoxEventPeriod.TabIndex = 4;
            this.textBoxEventPeriod.Text = "100";
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.label7.Location = new Point(0x18, 20);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x6b, 0x13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Event Period";
            this.tabPageProfile.Controls.Add(this.comboBoxProfiles);
            this.tabPageProfile.Controls.Add(this.label16);
            this.tabPageProfile.Controls.Add(this.label14);
            this.tabPageProfile.Controls.Add(this.buttonProfileManager);
            this.tabPageProfile.Controls.Add(this.label15);
            this.tabPageProfile.Location = new Point(4, 0x16);
            this.tabPageProfile.Name = "tabPageProfile";
            this.tabPageProfile.Size = new Size(440, 0xf5);
            this.tabPageProfile.TabIndex = 9;
            this.tabPageProfile.Text = "tabPageProfile";
            this.tabPageProfile.UseVisualStyleBackColor = true;
            this.comboBoxProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxProfiles.FormattingEnabled = true;
            this.comboBoxProfiles.Location = new Point(0x8d, 0x69);
            this.comboBoxProfiles.Name = "comboBoxProfiles";
            this.comboBoxProfiles.Size = new Size(0x9f, 0x15);
            this.comboBoxProfiles.TabIndex = 2;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x3e, 0xba);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x14c, 0x1a);
            this.label16.TabIndex = 4;
            this.label16.Text = "If you don't want to use a profile at this point, you can manually\r\ndefine channels and setup output plugins by clicking the Skip button.";
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x3e, 0x33);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x138, 0x27);
            this.label14.TabIndex = 1;
            this.label14.Text = "If you have a default profile defined in the user preferences, this\r\nsequence is already using it.  You may edit or verify it at this point\r\nif you would like, or create a new one altogether.";
            this.buttonProfileManager.Location = new Point(0xac, 0x84);
            this.buttonProfileManager.Name = "buttonProfileManager";
            this.buttonProfileManager.Size = new Size(0x61, 0x17);
            this.buttonProfileManager.TabIndex = 3;
            this.buttonProfileManager.Text = "Profile Manager";
            this.buttonProfileManager.UseVisualStyleBackColor = true;
            this.buttonProfileManager.Click += new EventHandler(this.buttonProfileManager_Click);
            this.label15.AutoSize = true;
            this.label15.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label15.Location = new Point(0x18, 20);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x3a, 0x13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Profile";
            this.tabPageChannelCount.Controls.Add(this.buttonImportChannels);
            this.tabPageChannelCount.Controls.Add(this.label9);
            this.tabPageChannelCount.Controls.Add(this.textBoxChannelCount);
            this.tabPageChannelCount.Controls.Add(this.label3);
            this.tabPageChannelCount.Location = new Point(4, 0x16);
            this.tabPageChannelCount.Name = "tabPageChannelCount";
            this.tabPageChannelCount.Padding = new Padding(3);
            this.tabPageChannelCount.Size = new Size(440, 0xf5);
            this.tabPageChannelCount.TabIndex = 1;
            this.tabPageChannelCount.Text = "tabPageChannelCount";
            this.tabPageChannelCount.UseVisualStyleBackColor = true;
            this.buttonImportChannels.Location = new Point(170, 0x86);
            this.buttonImportChannels.Name = "buttonImportChannels";
            this.buttonImportChannels.Size = new Size(100, 0x17);
            this.buttonImportChannels.TabIndex = 7;
            this.buttonImportChannels.Text = "Import channels";
            this.buttonImportChannels.UseVisualStyleBackColor = true;
            this.buttonImportChannels.Click += new EventHandler(this.buttonImportChannels_Click);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xd8, 90);
            this.label9.Name = "label9";
            this.label9.Size = new Size(50, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "channels";
            this.textBoxChannelCount.Location = new Point(0xaf, 0x57);
            this.textBoxChannelCount.Name = "textBoxChannelCount";
            this.textBoxChannelCount.Size = new Size(0x23, 20);
            this.textBoxChannelCount.TabIndex = 2;
            this.textBoxChannelCount.Text = "16";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x18, 20);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x7c, 0x13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Channel Count";
            this.tabPageChannelNames.Controls.Add(this.labelNamesChannels);
            this.tabPageChannelNames.Controls.Add(this.textBoxChannelNames);
            this.tabPageChannelNames.Controls.Add(this.label10);
            this.tabPageChannelNames.Controls.Add(this.label4);
            this.tabPageChannelNames.Location = new Point(4, 0x16);
            this.tabPageChannelNames.Name = "tabPageChannelNames";
            this.tabPageChannelNames.Size = new Size(440, 0xf5);
            this.tabPageChannelNames.TabIndex = 2;
            this.tabPageChannelNames.Text = "tabPageChannelNames";
            this.tabPageChannelNames.UseVisualStyleBackColor = true;
            this.labelNamesChannels.AutoSize = true;
            this.labelNamesChannels.Location = new Point(0x90, 190);
            this.labelNamesChannels.Name = "labelNamesChannels";
            this.labelNamesChannels.Size = new Size(0, 13);
            this.labelNamesChannels.TabIndex = 5;
            this.textBoxChannelNames.Location = new Point(0x93, 0x4e);
            this.textBoxChannelNames.Multiline = true;
            this.textBoxChannelNames.Name = "textBoxChannelNames";
            this.textBoxChannelNames.ScrollBars = ScrollBars.Vertical;
            this.textBoxChannelNames.Size = new Size(0x8f, 0x69);
            this.textBoxChannelNames.TabIndex = 4;
            this.textBoxChannelNames.TextChanged += new EventHandler(this.textBoxChannelNames_TextChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x90, 0x35);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x92, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Edit this list of channel names";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x18, 20);
            this.label4.Name = "label4";
            this.label4.Size = new Size(130, 0x13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Channel Names";
            this.tabPagePlugin.Controls.Add(this.label13);
            this.tabPagePlugin.Controls.Add(this.buttonSetupPlugins);
            this.tabPagePlugin.Controls.Add(this.label8);
            this.tabPagePlugin.Location = new Point(4, 0x16);
            this.tabPagePlugin.Name = "tabPagePlugin";
            this.tabPagePlugin.Size = new Size(440, 0xf5);
            this.tabPagePlugin.TabIndex = 6;
            this.tabPagePlugin.Text = "tabPagePlugin";
            this.tabPagePlugin.UseVisualStyleBackColor = true;
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x3e, 0x33);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x142, 0x34);
            this.label13.TabIndex = 7;
            this.label13.Text = manager.GetString("label13.Text");
            this.buttonSetupPlugins.Location = new Point(0xac, 0x73);
            this.buttonSetupPlugins.Name = "buttonSetupPlugins";
            this.buttonSetupPlugins.Size = new Size(0x61, 0x17);
            this.buttonSetupPlugins.TabIndex = 6;
            this.buttonSetupPlugins.Text = "Setup Plugins";
            this.buttonSetupPlugins.UseVisualStyleBackColor = true;
            this.buttonSetupPlugins.Click += new EventHandler(this.buttonSetupPlugins_Click);
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label8.Location = new Point(0x18, 20);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0xa3, 0x13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Output Plugin Setup";
            this.tabPageAudio.Controls.Add(this.buttonAssignAudio);
            this.tabPageAudio.Controls.Add(this.label5);
            this.tabPageAudio.Location = new Point(4, 0x16);
            this.tabPageAudio.Name = "tabPageAudio";
            this.tabPageAudio.Size = new Size(440, 0xf5);
            this.tabPageAudio.TabIndex = 3;
            this.tabPageAudio.Text = "tabPageAudio";
            this.tabPageAudio.UseVisualStyleBackColor = true;
            this.buttonAssignAudio.Location = new Point(0x6f, 0x73);
            this.buttonAssignAudio.Name = "buttonAssignAudio";
            this.buttonAssignAudio.Size = new Size(0xda, 0x17);
            this.buttonAssignAudio.TabIndex = 3;
            this.buttonAssignAudio.Text = "Assign audio / Define event patterns";
            this.buttonAssignAudio.UseVisualStyleBackColor = true;
            this.buttonAssignAudio.Click += new EventHandler(this.buttonAssignAudio_Click);
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0x18, 20);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xcb, 0x13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Audio and Event Patterns";
            this.tabPageTime.Controls.Add(this.label11);
            this.tabPageTime.Controls.Add(this.textBoxTime);
            this.tabPageTime.Controls.Add(this.label6);
            this.tabPageTime.Location = new Point(4, 0x16);
            this.tabPageTime.Name = "tabPageTime";
            this.tabPageTime.Size = new Size(440, 0xf5);
            this.tabPageTime.TabIndex = 4;
            this.tabPageTime.Text = "tabPageTime";
            this.tabPageTime.UseVisualStyleBackColor = true;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x8e, 110);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x9d, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "minutes : seconds . milliseconds";
            this.textBoxTime.Location = new Point(0xbb, 0x57);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new Size(0x42, 20);
            this.textBoxTime.TabIndex = 3;
            this.textBoxTime.Text = "5:00.000";
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.Location = new Point(0x18, 20);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x7f, 0x13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Sequence Time";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x117, 0x1d2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "Create It";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(360, 0x1d2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.labelExplanation);
            this.groupBox1.Controls.Add(this.labelNotes);
            this.groupBox1.Controls.Add(this.labelEffect);
            this.groupBox1.Controls.Add(this.labelWhy);
            this.groupBox1.Controls.Add(this.labelWhat);
            this.groupBox1.Location = new Point(12, 0x12d);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1a5, 0x8b);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Explanations";
            this.labelExplanation.Location = new Point(90, 0x1a);
            this.labelExplanation.Name = "labelExplanation";
            this.labelExplanation.Size = new Size(0x13c, 100);
            this.labelExplanation.TabIndex = 4;
            this.labelNotes.AutoSize = true;
            this.labelNotes.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelNotes.Location = new Point(15, 0x72);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new Size(0x33, 13);
            this.labelNotes.TabIndex = 3;
            this.labelNotes.Text = "Notes >";
            this.labelNotes.Click += new EventHandler(this.labelNotes_Click);
            this.labelEffect.AutoSize = true;
            this.labelEffect.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelEffect.Location = new Point(14, 0x55);
            this.labelEffect.Name = "labelEffect";
            this.labelEffect.Size = new Size(0x34, 13);
            this.labelEffect.TabIndex = 2;
            this.labelEffect.Text = "Effect >";
            this.labelEffect.Click += new EventHandler(this.labelEffect_Click);
            this.labelWhy.AutoSize = true;
            this.labelWhy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelWhy.Location = new Point(15, 0x38);
            this.labelWhy.Name = "labelWhy";
            this.labelWhy.Size = new Size(0x33, 13);
            this.labelWhy.TabIndex = 1;
            this.labelWhy.Text = "Why   >";
            this.labelWhy.Click += new EventHandler(this.labelWhy_Click);
            this.labelWhat.AutoSize = true;
            this.labelWhat.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelWhat.Location = new Point(14, 0x1b);
            this.labelWhat.Name = "labelWhat";
            this.labelWhat.Size = new Size(0x34, 13);
            this.labelWhat.TabIndex = 0;
            this.labelWhat.Text = "What  >";
            this.labelWhat.Click += new EventHandler(this.labelWhat_Click);
            this.buttonPrev.Enabled = false;
            this.buttonPrev.Location = new Point(0xc6, 0xff);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new Size(0x4b, 0x17);
            this.buttonPrev.TabIndex = 1;
            this.buttonPrev.Text = "<  &Prev";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new EventHandler(this.buttonPrev_Click);
            this.buttonNext.Location = new Point(360, 0xff);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new Size(0x4b, 0x17);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "&Next  >";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new EventHandler(this.buttonNext_Click);
            this.buttonSkip.Location = new Point(0x117, 0xff);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new Size(0x4b, 0x17);
            this.buttonSkip.TabIndex = 2;
            this.buttonSkip.Text = "Skip";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new EventHandler(this.buttonSkip_Click);
            this.openFileDialog.DefaultExt = "vix";
            this.openFileDialog.Filter = "Vixen event sequence | *.vix";
            base.AcceptButton = this.buttonNext;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1bf, 0x1f5);
            base.Controls.Add(this.buttonSkip);
            base.Controls.Add(this.buttonNext);
            base.Controls.Add(this.buttonPrev);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.tabControl);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "NewSequenceWizardDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "New Sequence Wizard";
            this.tabControl.ResumeLayout(false);
            this.tabPageStart.ResumeLayout(false);
            this.tabPageStart.PerformLayout();
            this.tabPageEventPeriod.ResumeLayout(false);
            this.tabPageEventPeriod.PerformLayout();
            this.tabPageProfile.ResumeLayout(false);
            this.tabPageProfile.PerformLayout();
            this.tabPageChannelCount.ResumeLayout(false);
            this.tabPageChannelCount.PerformLayout();
            this.tabPageChannelNames.ResumeLayout(false);
            this.tabPageChannelNames.PerformLayout();
            this.tabPagePlugin.ResumeLayout(false);
            this.tabPagePlugin.PerformLayout();
            this.tabPageAudio.ResumeLayout(false);
            this.tabPageAudio.PerformLayout();
            this.tabPageTime.ResumeLayout(false);
            this.tabPageTime.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void labelEffect_Click(object sender, EventArgs e)
        {
            if (this.labelEffect.Enabled)
            {
                this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 2];
            }
        }

        private void labelNotes_Click(object sender, EventArgs e)
        {
            if (this.labelNotes.Enabled)
            {
                this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 3];
            }
        }

        private void labelWhat_Click(object sender, EventArgs e)
        {
            if (this.labelWhat.Enabled)
            {
                this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 0];
            }
        }

        private void labelWhy_Click(object sender, EventArgs e)
        {
            if (this.labelWhy.Enabled)
            {
                this.labelExplanation.Text = this.m_explanations[this.tabControl.SelectedIndex, 1];
            }
        }

        private int ParseTimeString(string text)
        {
            int num2;
            int num3;
            int num4;
            string s = "0";
            string str2 = string.Empty;
            string str3 = "0";
            int index = text.IndexOf(':');
            if (index != -1)
            {
                s = text.Substring(0, index).Trim();
                text = text.Substring(index + 1);
            }
            index = text.IndexOf('.');
            if (index != -1)
            {
                str3 = text.Substring(index + 1).Trim();
                text = text.Substring(0, index);
            }
            str2 = text;
            try
            {
                num2 = int.Parse(s);
            }
            catch
            {
                num2 = 0;
            }
            try
            {
                num3 = int.Parse(str2);
            }
            catch
            {
                num3 = 0;
            }
            try
            {
                num4 = int.Parse(str3);
            }
            catch
            {
                num4 = 0;
            }
            num4 = (num4 + (num3 * 0x3e8)) + (num2 * 0xea60);
            if (num4 == 0)
            {
                MessageBox.Show("Not a valid format for time.\nUse one of the following:\n\nSeconds\nMinutes:Seconds\nSeconds.Milliseconds\nMinutes:Seconds.Milliseconds", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            return num4;
        }

        private void PopulateProfileList()
        {
            int selectedIndex = this.comboBoxProfiles.SelectedIndex;
            this.comboBoxProfiles.BeginUpdate();
            this.comboBoxProfiles.Items.Clear();
            this.comboBoxProfiles.Items.Add("None");
            foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro"))
            {
                this.comboBoxProfiles.Items.Add(Path.GetFileNameWithoutExtension(str));
            }
            if (selectedIndex < this.comboBoxProfiles.Items.Count)
            {
                this.comboBoxProfiles.SelectedIndex = selectedIndex;
            }
            this.comboBoxProfiles.EndUpdate();
        }

        private void SetSequenceTime()
        {
            this.textBoxTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", this.m_sequence.Time / 0xea60, (this.m_sequence.Time % 0xea60) / 0x3e8, this.m_sequence.Time % 0x3e8);
        }

        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (!this.m_back)
            {
                this.m_history.Push(e.TabPageIndex);
                this.buttonPrev.Enabled = true;
            }
            if (this.m_skip)
            {
                return;
            }
            string text = string.Empty;
            switch (e.TabPageIndex)
            {
                case 1:
                {
                    int num = 0;
                    try
                    {
                        num = Convert.ToInt32(this.textBoxEventPeriod.Text);
                        if (num < 0x19)
                        {
                            text = "While possible, event periods less than 25 ms aren't realistic or practical.";
                            goto Label_0339;
                        }
                    }
                    catch
                    {
                        text = this.textBoxChannelCount.Text + " is not a valid number for the event period length";
                        goto Label_0339;
                    }
                    this.m_sequence.EventPeriod = num;
                    goto Label_0339;
                }
                case 2:
                    if (this.comboBoxProfiles.SelectedIndex == 0)
                    {
                        this.m_sequence.Profile = null;
                    }
                    else
                    {
                        this.m_sequence.Profile = new Profile(Path.Combine(Paths.ProfilePath, this.comboBoxProfiles.SelectedItem.ToString() + ".pro"));
                    }
                    goto Label_0339;

                case 3:
                {
                    int num2 = 0;
                    try
                    {
                        num2 = Convert.ToInt32(this.textBoxChannelCount.Text);
                        if (num2 < 1)
                        {
                            text = "The channel count must be 1 or more";
                            goto Label_0339;
                        }
                    }
                    catch
                    {
                        text = this.textBoxChannelCount.Text + " is not a valid number for the channel count";
                        goto Label_0339;
                    }
                    if ((num2 > 0x400) && (MessageBox.Show(string.Format("Are you sure you really want {0} channels?", num2), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
                    {
                        this.tabControl.TabIndex = 1;
                    }
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            this.m_sequence.ChannelCount = num2;
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                    goto Label_0339;
                }
                case 4:
                    if (this.textBoxChannelNames.Lines.Length == Convert.ToInt32(this.textBoxChannelCount.Text))
                    {
                        foreach (string str2 in this.textBoxChannelNames.Lines)
                        {
                            if (str2.Trim() == string.Empty)
                            {
                                text = "Channel names cannot be blank";
                                break;
                            }
                        }
                        break;
                    }
                    text = "There must be an equal number of channel names as there are channels";
                    goto Label_0339;

                case 7:
                {
                    int num4 = this.ParseTimeString(this.textBoxTime.Text);
                    if (num4 != 0)
                    {
                        try
                        {
                            this.m_sequence.Time = num4;
                        }
                        catch (Exception exception)
                        {
                            text = exception.Message;
                        }
                    }
                    goto Label_0339;
                }
                default:
                    goto Label_0339;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                for (int i = 0; i < this.m_sequence.ChannelCount; i++)
                {
                    this.m_sequence.Channels[i].Name = this.textBoxChannelNames.Lines[i];
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        Label_0339:
            if (text != string.Empty)
            {
                e.Cancel = true;
                MessageBox.Show(text, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 1:
                    this.textBoxEventPeriod.Text = this.m_sequence.EventPeriod.ToString();
                    break;

                case 2:
                    if (this.m_sequence.Profile != null)
                    {
                        this.comboBoxProfiles.SelectedIndex = this.comboBoxProfiles.Items.IndexOf(this.m_sequence.Profile.Name);
                        break;
                    }
                    this.comboBoxProfiles.SelectedIndex = 0;
                    break;

                case 4:
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        string[] strArray;
                        int num = Convert.ToInt32(this.textBoxChannelCount.Text);
                        int length = this.textBoxChannelNames.Lines.Length;
                        if (length < num)
                        {
                            strArray = new string[num];
                            this.textBoxChannelNames.Lines.CopyTo(strArray, 0);
                            while (length < num)
                            {
                                strArray[length] = string.Format("Channel {0}", length + 1);
                                length++;
                            }
                            this.textBoxChannelNames.Lines = strArray;
                        }
                        else if (length > num)
                        {
                            strArray = new string[num];
                            for (int i = 0; i < num; i++)
                            {
                                strArray[i] = this.textBoxChannelNames.Lines[i];
                            }
                            this.textBoxChannelNames.Lines = strArray;
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                    break;

                case 7:
                    this.SetSequenceTime();
                    break;
            }
            this.UpdateExplanations(e.TabPageIndex);
            this.buttonSkip.Enabled = this.buttonNext.Enabled = e.TabPageIndex < (this.tabControl.TabCount - 1);
            base.AcceptButton = (e.TabPageIndex == 4) ? null : (this.buttonNext.Enabled ? ((IButtonControl) this.buttonNext) : ((IButtonControl) this.buttonOK));
        }

        private void textBoxChannelNames_TextChanged(object sender, EventArgs e)
        {
            this.labelNamesChannels.Text = string.Format("{0} names / {1} channels", this.textBoxChannelNames.Lines.Length, this.textBoxChannelCount.Text);
        }

        private void UpdateExplanations(int pageIndex)
        {
            this.labelWhat.Enabled = this.m_explanations[pageIndex, 0] != string.Empty;
            this.labelWhy.Enabled = this.m_explanations[pageIndex, 1] != string.Empty;
            this.labelEffect.Enabled = this.m_explanations[pageIndex, 2] != string.Empty;
            this.labelNotes.Enabled = this.m_explanations[pageIndex, 3] != string.Empty;
            this.labelExplanation.Text = this.m_explanations[pageIndex, 0];
        }

        public EventSequence Sequence
        {
            get
            {
                return this.m_sequence;
            }
        }
    }
}

