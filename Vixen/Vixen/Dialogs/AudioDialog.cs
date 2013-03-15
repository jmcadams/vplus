namespace Vixen.Dialogs
{
    using FMOD;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using Vixen;

    public class AudioDialog : Form
    {
        private Button buttonCancel;
        private Button buttonClear;
        private Button buttonLoad;
        private Button buttonOK;
        private Button buttonPlayPause;
        private Button buttonRemoveAudio;
        private Button buttonStop;
        private ToolStripComboBox channel0ToolStripMenuItem;
        private ToolStripComboBox channel1ToolStripMenuItem;
        private ToolStripComboBox channel2ToolStripMenuItem;
        private ToolStripComboBox channel3ToolStripMenuItem;
        private ToolStripComboBox channel4ToolStripMenuItem;
        private ToolStripComboBox channel5ToolStripMenuItem;
        private ToolStripComboBox channel6ToolStripMenuItem;
        private ToolStripComboBox channel7ToolStripMenuItem;
        private ToolStripComboBox channel8ToolStripMenuItem;
        private ToolStripComboBox channel9ToolStripMenuItem;
        private CheckBox checkBoxAutoSize;
        private ComboBox comboBoxAudioDevice;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip;
        private const int COUNTDOWN_SECONDS = 5;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label labelAudioFileName;
        private Label labelAudioLength;
        private Label labelAudioName;
        private Label labelTime;
        private Label labelTotalTime;
        private LinkLabel linkLabelAssignedKeys;
        private ListBox listBoxChannels;
        private string m_audioFilename = string.Empty;
        private DateTime m_countdownEnd;
        private fmod m_fmod;
        private int[] m_keyMap = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private bool[] m_keyStates;
        private int m_lastIndex = -1;
        private int m_maxTime;
        private byte[,] m_newEventValues;
        private Audio m_originalAudio;
        private bool m_paused = false;
        private bool m_playing = false;
        private EventSequence m_sequence = null;
        private float m_smallChange;
        private SoundChannel m_soundChannel = null;
        private Stopwatch m_stopwatch;
        private int m_timeOffset;
        private System.Timers.Timer m_timer;
        private OpenFileDialog openFileDialog1;
        private PictureBox pictureBoxPause;
        private PictureBox pictureBoxPlay;
        private PictureBox pictureBoxPlayBlue;
        private ProgressBar progressBarCountdown;
        private RadioButton radioButtonMultipleEvents;
        private RadioButton radioButtonSingleEvent;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private TrackBar trackBarPosition;

        public AudioDialog(EventSequence sequence, bool autoSize, int deviceIndex)
        {
            this.InitializeComponent();
            this.m_fmod = (deviceIndex > 0) ? fmod.GetInstance(deviceIndex) : fmod.GetInstance(-1);
            this.m_timer = new System.Timers.Timer(10.0);
            this.m_timer.Elapsed += new ElapsedEventHandler(this.m_timer_Elapsed);
            this.m_sequence = sequence;
            this.m_keyStates = new bool[this.m_sequence.ChannelCount];
            this.m_stopwatch = new Stopwatch();
            this.m_newEventValues = new byte[this.m_sequence.EventValues.GetLength(0), this.m_sequence.EventValues.GetLength(1)];
            this.listBoxChannels.Items.AddRange(this.m_sequence.Channels.ToArray());
            this.m_originalAudio = sequence.Audio;
            if (sequence.Audio != null)
            {
                if (this.LoadAudio(this.m_sequence.Audio.FileName) == null)
                {
                    sequence.Audio = null;
                    this.buttonRemoveAudio.Enabled = false;
                    this.ClearAudio();
                }
                else
                {
                    this.buttonRemoveAudio.Enabled = true;
                }
            }
            else
            {
                this.buttonRemoveAudio.Enabled = false;
                this.ClearAudio();
            }
            this.checkBoxAutoSize.Checked = autoSize;
            if (!autoSize)
            {
                this.UpdateRecordableLength();
            }
            Vixen.Channel[] items = this.m_sequence.Channels.ToArray();
            this.channel1ToolStripMenuItem.Items.AddRange(items);
            this.channel2ToolStripMenuItem.Items.AddRange(items);
            this.channel3ToolStripMenuItem.Items.AddRange(items);
            this.channel4ToolStripMenuItem.Items.AddRange(items);
            this.channel5ToolStripMenuItem.Items.AddRange(items);
            this.channel6ToolStripMenuItem.Items.AddRange(items);
            this.channel7ToolStripMenuItem.Items.AddRange(items);
            this.channel8ToolStripMenuItem.Items.AddRange(items);
            this.channel9ToolStripMenuItem.Items.AddRange(items);
            this.channel0ToolStripMenuItem.Items.AddRange(items);
            this.channel1ToolStripMenuItem.SelectedIndex = Math.Min(0, this.m_sequence.ChannelCount - 1);
            this.channel2ToolStripMenuItem.SelectedIndex = Math.Min(1, this.m_sequence.ChannelCount - 1);
            this.channel3ToolStripMenuItem.SelectedIndex = Math.Min(2, this.m_sequence.ChannelCount - 1);
            this.channel4ToolStripMenuItem.SelectedIndex = Math.Min(3, this.m_sequence.ChannelCount - 1);
            this.channel5ToolStripMenuItem.SelectedIndex = Math.Min(4, this.m_sequence.ChannelCount - 1);
            this.channel6ToolStripMenuItem.SelectedIndex = Math.Min(5, this.m_sequence.ChannelCount - 1);
            this.channel7ToolStripMenuItem.SelectedIndex = Math.Min(6, this.m_sequence.ChannelCount - 1);
            this.channel8ToolStripMenuItem.SelectedIndex = Math.Min(7, this.m_sequence.ChannelCount - 1);
            this.channel9ToolStripMenuItem.SelectedIndex = Math.Min(8, this.m_sequence.ChannelCount - 1);
            this.channel0ToolStripMenuItem.SelectedIndex = Math.Min(9, this.m_sequence.ChannelCount - 1);
            this.comboBoxAudioDevice.Items.Add("Use application's default device");
            this.comboBoxAudioDevice.Items.AddRange(fmod.GetSoundDeviceList());
            this.comboBoxAudioDevice.SelectedIndex = this.m_sequence.AudioDeviceIndex + 1;
        }

        private void AudioDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Stop();
            this.m_fmod.ReleaseSound(this.m_soundChannel);
            this.m_fmod.Shutdown();
        }

        private void AudioDialog_KeyDown(object sender, KeyEventArgs e)
        {
            this.UpdateKeyState(e, true);
        }

        private void AudioDialog_KeyUp(object sender, KeyEventArgs e)
        {
            this.UpdateKeyState(e, false);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.m_sequence.Audio = this.m_originalAudio;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.m_newEventValues.GetLength(1); i++)
            {
                this.m_newEventValues[this.listBoxChannels.SelectedIndex, i] = 0;
            }
            MessageBox.Show(string.Format("Channel \"{0}\" cleared of new events.", ((Vixen.Channel) this.listBoxChannels.SelectedItem).Name), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Paths.AudioPath;
            this.openFileDialog1.DefaultExt = "wma";
            this.openFileDialog1.Filter = "All supported formats | *.aiff;*.asf;*.flac;*.mp2;*.mp3;*.ogg;*.wav;*.wma;*.mid";
            this.openFileDialog1.FileName = string.Empty;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = Path.Combine(Paths.AudioPath, Path.GetFileName(this.openFileDialog1.FileName));
                if (!File.Exists(path))
                {
                    this.Cursor = Cursors.WaitCursor;
                    File.Copy(this.openFileDialog1.FileName, path);
                    this.Cursor = Cursors.Default;
                }
                this.m_sequence.Audio = this.LoadAudio(this.openFileDialog1.FileName);
                this.UpdateRecordableLength();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ((this.m_sequence.Audio != null) && (this.checkBoxAutoSize.Checked || (this.m_sequence.Audio.Duration > this.m_sequence.Time)))
            {
                this.m_sequence.Time = this.m_sequence.Audio.Duration;
            }
            this.m_sequence.AudioDeviceIndex = this.comboBoxAudioDevice.SelectedIndex - 1;
            int num2 = Math.Min(this.m_newEventValues.GetLength(0), this.m_sequence.EventValues.GetLength(0));
            int num3 = Math.Min(this.m_newEventValues.GetLength(1), this.m_sequence.EventValues.GetLength(1));
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num3; j++)
                {
                    byte num1 = this.m_sequence.EventValues[i, j];
                    num1[0] = (byte) (num1[0] | this.m_newEventValues[i, j]);
                }
            }
        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
            if (this.m_playing)
            {
                this.buttonPlayPause.Image = this.m_paused ? this.pictureBoxPause.Image : this.pictureBoxPlayBlue.Image;
                if (this.m_soundChannel != null)
                {
                    this.m_soundChannel.Paused = !this.m_paused;
                }
                this.m_timer.Enabled = this.m_paused;
                this.m_paused = !this.m_paused;
                if (!(this.m_paused || !this.progressBarCountdown.Visible))
                {
                    this.m_countdownEnd = DateTime.Now + TimeSpan.FromSeconds((double) ((((float) this.progressBarCountdown.Value) / 100f) * 5f));
                }
            }
            else
            {
                this.m_countdownEnd = DateTime.Now.Add(TimeSpan.FromSeconds(5.0));
                this.progressBarCountdown.Value = 100;
                this.progressBarCountdown.Visible = true;
                this.m_playing = true;
                this.labelTime.Text = "Countdown...";
                this.labelTotalTime.Text = string.Empty;
                this.trackBarPosition.Enabled = false;
                this.m_timer.Start();
                this.buttonPlayPause.Image = this.pictureBoxPause.Image;
            }
        }

        private void buttonRemoveAudio_Click(object sender, EventArgs e)
        {
            this.ClearAudio();
            this.UpdateRecordableLength();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void channelMapItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox box = sender as ToolStripComboBox;
            if (box.SelectedIndex != -1)
            {
                int index = Convert.ToInt32(box.Tag) - 1;
                this.m_keyMap[index] = box.SelectedIndex;
            }
        }

        private void checkBoxAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateRecordableLength();
        }

        private void ClearAudio()
        {
            this.m_sequence.Audio = null;
            this.labelAudioFileName.Text = "This event sequence does not have audio assigned";
            this.labelAudioLength.Text = string.Empty;
            this.labelAudioName.Text = string.Empty;
            this.m_soundChannel = null;
        }

        private void CopyArray(byte[,] source, byte[,] dest)
        {
            int num = Math.Min(source.GetLength(0), dest.GetLength(0));
            int num2 = Math.Min(source.GetLength(1), dest.GetLength(1));
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num2; j++)
                {
                    dest[i, j] = source[i, j];
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private uint GetPosition()
        {
            if ((this.m_soundChannel != null) && this.m_soundChannel.IsPlaying)
            {
                return this.m_soundChannel.Position;
            }
            return (uint) (this.m_stopwatch.ElapsedMilliseconds + this.m_timeOffset);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AudioDialog));
            this.openFileDialog1 = new OpenFileDialog();
            this.groupBox1 = new GroupBox();
            this.comboBoxAudioDevice = new ComboBox();
            this.label2 = new Label();
            this.checkBoxAutoSize = new CheckBox();
            this.buttonRemoveAudio = new Button();
            this.labelAudioLength = new Label();
            this.labelAudioFileName = new Label();
            this.labelAudioName = new Label();
            this.buttonLoad = new Button();
            this.groupBox2 = new GroupBox();
            this.trackBarPosition = new TrackBar();
            this.progressBarCountdown = new ProgressBar();
            this.linkLabelAssignedKeys = new LinkLabel();
            this.pictureBoxPlay = new PictureBox();
            this.pictureBoxPlayBlue = new PictureBox();
            this.pictureBoxPause = new PictureBox();
            this.labelTotalTime = new Label();
            this.labelTime = new Label();
            this.buttonClear = new Button();
            this.buttonStop = new Button();
            this.buttonPlayPause = new Button();
            this.label1 = new Label();
            this.listBoxChannels = new ListBox();
            this.contextMenuStrip = new ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.channel1ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem3 = new ToolStripMenuItem();
            this.channel2ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem4 = new ToolStripMenuItem();
            this.channel3ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem5 = new ToolStripMenuItem();
            this.channel4ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem6 = new ToolStripMenuItem();
            this.channel5ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem7 = new ToolStripMenuItem();
            this.channel6ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem8 = new ToolStripMenuItem();
            this.channel7ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem9 = new ToolStripMenuItem();
            this.channel8ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem10 = new ToolStripMenuItem();
            this.channel9ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem11 = new ToolStripMenuItem();
            this.channel0ToolStripMenuItem = new ToolStripComboBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox3 = new GroupBox();
            this.radioButtonMultipleEvents = new RadioButton();
            this.radioButtonSingleEvent = new RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.trackBarPosition.BeginInit();
            ((ISupportInitialize) this.pictureBoxPlay).BeginInit();
            ((ISupportInitialize) this.pictureBoxPlayBlue).BeginInit();
            ((ISupportInitialize) this.pictureBoxPause).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.openFileDialog1.DefaultExt = "wma";
            this.openFileDialog1.Filter = "Windows Media Audio | *.wma";
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.comboBoxAudioDevice);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxAutoSize);
            this.groupBox1.Controls.Add(this.buttonRemoveAudio);
            this.groupBox1.Controls.Add(this.labelAudioLength);
            this.groupBox1.Controls.Add(this.labelAudioFileName);
            this.groupBox1.Controls.Add(this.labelAudioName);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(420, 0xab);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select audio";
            this.comboBoxAudioDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxAudioDevice.FormattingEnabled = true;
            this.comboBoxAudioDevice.Location = new Point(0x84, 0x87);
            this.comboBoxAudioDevice.Name = "comboBoxAudioDevice";
            this.comboBoxAudioDevice.Size = new Size(0xce, 0x15);
            this.comboBoxAudioDevice.TabIndex = 7;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x39, 0x8a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Audio device";
            this.checkBoxAutoSize.AutoSize = true;
            this.checkBoxAutoSize.CheckAlign = ContentAlignment.TopLeft;
            this.checkBoxAutoSize.Location = new Point(30, 0x1c);
            this.checkBoxAutoSize.Name = "checkBoxAutoSize";
            this.checkBoxAutoSize.Size = new Size(0xd9, 0x11);
            this.checkBoxAutoSize.TabIndex = 0;
            this.checkBoxAutoSize.Text = "Resize the sequence to the audio length";
            this.checkBoxAutoSize.UseVisualStyleBackColor = true;
            this.checkBoxAutoSize.CheckedChanged += new EventHandler(this.checkBoxAutoSize_CheckedChanged);
            this.buttonRemoveAudio.Enabled = false;
            this.buttonRemoveAudio.Location = new Point(30, 0x5f);
            this.buttonRemoveAudio.Name = "buttonRemoveAudio";
            this.buttonRemoveAudio.Size = new Size(0x60, 0x17);
            this.buttonRemoveAudio.TabIndex = 2;
            this.buttonRemoveAudio.Text = "Remove audio";
            this.buttonRemoveAudio.UseVisualStyleBackColor = true;
            this.buttonRemoveAudio.Click += new EventHandler(this.buttonRemoveAudio_Click);
            this.labelAudioLength.AutoSize = true;
            this.labelAudioLength.Location = new Point(0x84, 0x69);
            this.labelAudioLength.Name = "labelAudioLength";
            this.labelAudioLength.Size = new Size(0x24, 13);
            this.labelAudioLength.TabIndex = 5;
            this.labelAudioLength.Text = "length";
            this.labelAudioFileName.AutoSize = true;
            this.labelAudioFileName.Location = new Point(0x84, 0x42);
            this.labelAudioFileName.Name = "labelAudioFileName";
            this.labelAudioFileName.Size = new Size(0x2e, 13);
            this.labelAudioFileName.TabIndex = 3;
            this.labelAudioFileName.Text = "filename";
            this.labelAudioName.AutoSize = true;
            this.labelAudioName.Location = new Point(0x84, 0x55);
            this.labelAudioName.Name = "labelAudioName";
            this.labelAudioName.Size = new Size(0x21, 13);
            this.labelAudioName.TabIndex = 4;
            this.labelAudioName.Text = "name";
            this.buttonLoad.Location = new Point(30, 0x42);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new Size(0x60, 0x17);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Assign audio";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new EventHandler(this.buttonLoad_Click);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBox2.Controls.Add(this.trackBarPosition);
            this.groupBox2.Controls.Add(this.progressBarCountdown);
            this.groupBox2.Controls.Add(this.linkLabelAssignedKeys);
            this.groupBox2.Controls.Add(this.pictureBoxPlay);
            this.groupBox2.Controls.Add(this.pictureBoxPlayBlue);
            this.groupBox2.Controls.Add(this.pictureBoxPause);
            this.groupBox2.Controls.Add(this.labelTotalTime);
            this.groupBox2.Controls.Add(this.labelTime);
            this.groupBox2.Controls.Add(this.buttonClear);
            this.groupBox2.Controls.Add(this.buttonStop);
            this.groupBox2.Controls.Add(this.buttonPlayPause);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.listBoxChannels);
            this.groupBox2.Location = new Point(12, 0x138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(420, 0xe1);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel events";
            this.trackBarPosition.LargeChange = 30;
            this.trackBarPosition.Location = new Point(0x83, 0xb1);
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new Size(0x113, 0x2d);
            this.trackBarPosition.TabIndex = 0x13;
            this.trackBarPosition.TickStyle = TickStyle.TopLeft;
            this.trackBarPosition.Scroll += new EventHandler(this.trackBarPosition_Scroll);
            this.progressBarCountdown.Location = new Point(0x8d, 0xa4);
            this.progressBarCountdown.Name = "progressBarCountdown";
            this.progressBarCountdown.RightToLeft = RightToLeft.Yes;
            this.progressBarCountdown.RightToLeftLayout = true;
            this.progressBarCountdown.Size = new Size(0x109, 13);
            this.progressBarCountdown.TabIndex = 0x12;
            this.progressBarCountdown.Visible = false;
            this.linkLabelAssignedKeys.LinkArea = new LinkArea(11, 11);
            this.linkLabelAssignedKeys.Location = new Point(0x8d, 0x4b);
            this.linkLabelAssignedKeys.Name = "linkLabelAssignedKeys";
            this.linkLabelAssignedKeys.Size = new Size(0xf2, 0x1d);
            this.linkLabelAssignedKeys.TabIndex = 0x10;
            this.linkLabelAssignedKeys.TabStop = true;
            this.linkLabelAssignedKeys.Text = "Or use the number keys to create the pattern for specific channels simultaneously.";
            this.linkLabelAssignedKeys.UseCompatibleTextRendering = true;
            this.linkLabelAssignedKeys.VisitedLinkColor = Color.Blue;
            this.linkLabelAssignedKeys.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelAssignedKeys_LinkClicked);
            this.pictureBoxPlay.Image = (Image) manager.GetObject("pictureBoxPlay.Image");
            this.pictureBoxPlay.Location = new Point(0x162, 0x8e);
            this.pictureBoxPlay.Name = "pictureBoxPlay";
            this.pictureBoxPlay.Size = new Size(0x10, 0x10);
            this.pictureBoxPlay.TabIndex = 15;
            this.pictureBoxPlay.TabStop = false;
            this.pictureBoxPlay.Visible = false;
            this.pictureBoxPlayBlue.Image = (Image) manager.GetObject("pictureBoxPlayBlue.Image");
            this.pictureBoxPlayBlue.Location = new Point(0x14c, 0x8e);
            this.pictureBoxPlayBlue.Name = "pictureBoxPlayBlue";
            this.pictureBoxPlayBlue.Size = new Size(0x10, 0x10);
            this.pictureBoxPlayBlue.TabIndex = 14;
            this.pictureBoxPlayBlue.TabStop = false;
            this.pictureBoxPlayBlue.Visible = false;
            this.pictureBoxPause.Image = (Image) manager.GetObject("pictureBoxPause.Image");
            this.pictureBoxPause.Location = new Point(310, 0x8e);
            this.pictureBoxPause.Name = "pictureBoxPause";
            this.pictureBoxPause.Size = new Size(0x10, 0x10);
            this.pictureBoxPause.TabIndex = 13;
            this.pictureBoxPause.TabStop = false;
            this.pictureBoxPause.Visible = false;
            this.labelTotalTime.AutoSize = true;
            this.labelTotalTime.Location = new Point(0xbf, 0x93);
            this.labelTotalTime.Name = "labelTotalTime";
            this.labelTotalTime.Size = new Size(0x3f, 13);
            this.labelTotalTime.TabIndex = 7;
            this.labelTotalTime.Text = "/ 00:00.000";
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new Point(0x8a, 0x93);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new Size(0x37, 13);
            this.labelTime.TabIndex = 6;
            this.labelTime.Text = "00:00.000";
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new Point(0xc1, 0x73);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(0x57, 0x17);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Reset channel";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            this.buttonStop.Image = (Image) manager.GetObject("buttonStop.Image");
            this.buttonStop.Location = new Point(0xa7, 0x74);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new Size(20, 20);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
            this.buttonPlayPause.Image = (Image) manager.GetObject("buttonPlayPause.Image");
            this.buttonPlayPause.Location = new Point(0x8d, 0x74);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new Size(20, 20);
            this.buttonPlayPause.TabIndex = 2;
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            this.buttonPlayPause.Click += new EventHandler(this.buttonPlayPause_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x8a, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xe1, 0x27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a channel and use the left and/or right\r\nControl keys (CTRL) to create the pattern after\r\nclicking the play button.";
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(11, 0x1a);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.Size = new Size(0x73, 0xba);
            this.listBoxChannels.TabIndex = 0;
            this.listBoxChannels.SelectedIndexChanged += new EventHandler(this.listBoxChannels_SelectedIndexChanged);
            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItem2, this.toolStripMenuItem3, this.toolStripMenuItem4, this.toolStripMenuItem5, this.toolStripMenuItem6, this.toolStripMenuItem7, this.toolStripMenuItem8, this.toolStripMenuItem9, this.toolStripMenuItem10, this.toolStripMenuItem11 });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(0x57, 0xe0);
            this.toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { this.channel1ToolStripMenuItem });
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem2.Text = "'1'";
            this.channel1ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel1ToolStripMenuItem.Name = "channel1ToolStripMenuItem";
            this.channel1ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel1ToolStripMenuItem.Tag = "1";
            this.channel1ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem3.DropDownItems.AddRange(new ToolStripItem[] { this.channel2ToolStripMenuItem });
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem3.Text = "'2'";
            this.channel2ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel2ToolStripMenuItem.Name = "channel2ToolStripMenuItem";
            this.channel2ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel2ToolStripMenuItem.Tag = "2";
            this.channel2ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] { this.channel3ToolStripMenuItem });
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem4.Text = "'3'";
            this.channel3ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel3ToolStripMenuItem.Name = "channel3ToolStripMenuItem";
            this.channel3ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel3ToolStripMenuItem.Tag = "3";
            this.channel3ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[] { this.channel4ToolStripMenuItem });
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem5.Text = "'4'";
            this.channel4ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel4ToolStripMenuItem.Name = "channel4ToolStripMenuItem";
            this.channel4ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel4ToolStripMenuItem.Tag = "4";
            this.channel4ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem6.DropDownItems.AddRange(new ToolStripItem[] { this.channel5ToolStripMenuItem });
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem6.Text = "'5'";
            this.channel5ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel5ToolStripMenuItem.Name = "channel5ToolStripMenuItem";
            this.channel5ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel5ToolStripMenuItem.Tag = "5";
            this.channel5ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem7.DropDownItems.AddRange(new ToolStripItem[] { this.channel6ToolStripMenuItem });
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem7.Text = "'6'";
            this.channel6ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel6ToolStripMenuItem.Name = "channel6ToolStripMenuItem";
            this.channel6ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel6ToolStripMenuItem.Tag = "6";
            this.channel6ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem8.DropDownItems.AddRange(new ToolStripItem[] { this.channel7ToolStripMenuItem });
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem8.Text = "'7'";
            this.channel7ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel7ToolStripMenuItem.Name = "channel7ToolStripMenuItem";
            this.channel7ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel7ToolStripMenuItem.Tag = "7";
            this.channel7ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem9.DropDownItems.AddRange(new ToolStripItem[] { this.channel8ToolStripMenuItem });
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem9.Text = "'8'";
            this.channel8ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel8ToolStripMenuItem.Name = "channel8ToolStripMenuItem";
            this.channel8ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel8ToolStripMenuItem.Tag = "8";
            this.channel8ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem10.DropDownItems.AddRange(new ToolStripItem[] { this.channel9ToolStripMenuItem });
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem10.Text = "'9'";
            this.channel9ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel9ToolStripMenuItem.Name = "channel9ToolStripMenuItem";
            this.channel9ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel9ToolStripMenuItem.Tag = "9";
            this.channel9ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.toolStripMenuItem11.DropDownItems.AddRange(new ToolStripItem[] { this.channel0ToolStripMenuItem });
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem11.Text = "'0'";
            this.channel0ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel0ToolStripMenuItem.Name = "channel0ToolStripMenuItem";
            this.channel0ToolStripMenuItem.Size = new Size(0x98, 0x17);
            this.channel0ToolStripMenuItem.Tag = "10";
            this.channel0ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x114, 0x21f);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x165, 0x21f);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.groupBox3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.radioButtonMultipleEvents);
            this.groupBox3.Controls.Add(this.radioButtonSingleEvent);
            this.groupBox3.Location = new Point(12, 0xc6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(420, 0x63);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Key behavior";
            this.radioButtonMultipleEvents.AutoSize = true;
            this.radioButtonMultipleEvents.Location = new Point(0x17, 0x3a);
            this.radioButtonMultipleEvents.Name = "radioButtonMultipleEvents";
            this.radioButtonMultipleEvents.Size = new Size(0x11b, 0x11);
            this.radioButtonMultipleEvents.TabIndex = 1;
            this.radioButtonMultipleEvents.Text = "Create events while a key is pressed, until it is released";
            this.radioButtonMultipleEvents.UseVisualStyleBackColor = true;
            this.radioButtonSingleEvent.AutoSize = true;
            this.radioButtonSingleEvent.Checked = true;
            this.radioButtonSingleEvent.Location = new Point(0x17, 0x1d);
            this.radioButtonSingleEvent.Name = "radioButtonSingleEvent";
            this.radioButtonSingleEvent.Size = new Size(0xe9, 0x11);
            this.radioButtonSingleEvent.TabIndex = 0;
            this.radioButtonSingleEvent.TabStop = true;
            this.radioButtonSingleEvent.Text = "Create a single event when a key is pressed";
            this.radioButtonSingleEvent.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1bc, 0x23e);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.groupBox2);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AudioDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Event Sequence Audio";
            base.KeyUp += new KeyEventHandler(this.AudioDialog_KeyUp);
            base.FormClosing += new FormClosingEventHandler(this.AudioDialog_FormClosing);
            base.KeyDown += new KeyEventHandler(this.AudioDialog_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.trackBarPosition.EndInit();
            ((ISupportInitialize) this.pictureBoxPlay).EndInit();
            ((ISupportInitialize) this.pictureBoxPlayBlue).EndInit();
            ((ISupportInitialize) this.pictureBoxPause).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            base.ResumeLayout(false);
        }

        private void linkLabelAssignedKeys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.contextMenuStrip.Show(this.linkLabelAssignedKeys.PointToScreen(new Point(0, this.linkLabelAssignedKeys.Height)));
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateAudioButtons();
        }

        private Audio LoadAudio(string fileName)
        {
            if (fileName == string.Empty)
            {
                return null;
            }
            try
            {
                this.m_soundChannel = this.m_fmod.LoadSound(Path.Combine(Paths.AudioPath, fileName), this.m_soundChannel);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error loading audio:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            if (this.m_soundChannel == null)
            {
                return null;
            }
            this.m_audioFilename = fileName;
            this.labelAudioFileName.Text = Path.GetFileName(this.m_audioFilename);
            Audio audio = new Audio();
            audio.FileName = this.labelAudioFileName.Text;
            audio.Name = this.m_soundChannel.SoundName;
            audio.Duration = (int) this.m_soundChannel.SoundLength;
            this.labelAudioLength.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", audio.Duration / 0xea60, (audio.Duration % 0xea60) / 0x3e8, audio.Duration % 0x3e8);
            this.labelAudioName.Text = string.Format("\"{0}\"", audio.Name);
            this.UpdateAudioButtons();
            return audio;
        }

        private void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MethodInvoker method = null;
            if (this.progressBarCountdown.Visible)
            {
                if (this.progressBarCountdown.Value <= 0)
                {
                    this.UpdateTotalTime();
                    int num = (int) (((int) base.Invoke(new TrackBarValueDelegate(this.TrackBarValue))) * this.m_smallChange);
                    this.m_timeOffset = num;
                    if ((this.m_soundChannel != null) && (num < this.m_soundChannel.SoundLength))
                    {
                        this.m_fmod.Play(this.m_soundChannel, true);
                        this.m_soundChannel.Position = (uint) num;
                        this.m_soundChannel.Paused = false;
                    }
                    this.m_stopwatch.Reset();
                    this.m_stopwatch.Start();
                    base.Invoke(new ProgressBarVisibleDelegate(this.ProgressBarVisible), new object[] { false });
                }
                else
                {
                    if (method == null)
                    {
                        method = delegate {
                            TimeSpan span = (TimeSpan) (this.m_countdownEnd - DateTime.Now);
                            this.progressBarCountdown.Value = (int) ((span.TotalMilliseconds * 100.0) / 5000.0);
                        };
                    }
                    base.BeginInvoke(method);
                }
            }
            else
            {
                MethodInvoker invoker = null;
                uint position = this.GetPosition();
                if (position >= this.m_maxTime)
                {
                    this.Stop();
                }
                else
                {
                    int num2 = (int) (((ulong) position) / ((long) this.m_sequence.EventPeriod));
                    if (num2 != this.m_lastIndex)
                    {
                        this.m_lastIndex = num2;
                        for (int i = 0; i < this.m_sequence.ChannelCount; i++)
                        {
                            if (this.m_keyStates[i])
                            {
                                this.m_newEventValues[i, num2] = this.m_sequence.MaximumLevel;
                                if (this.radioButtonSingleEvent.Checked)
                                {
                                    this.m_keyStates[i] = false;
                                }
                            }
                        }
                    }
                    if (invoker == null)
                    {
                        invoker = delegate {
                            this.labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", position / 0xea60, (position % 0xea60) / 0x3e8, position % 0x3e8);
                            this.trackBarPosition.Value = (int) (((float) position) / this.m_smallChange);
                        };
                    }
                    base.BeginInvoke(invoker);
                }
            }
        }

        private void ProgressBarVisible(bool value)
        {
            this.progressBarCountdown.Visible = value;
        }

        private void Stop()
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MethodInvoker(this.Stop));
            }
            else
            {
                this.m_timer.Stop();
                Thread.Sleep((int) this.m_timer.Interval);
                if (this.m_soundChannel != null)
                {
                    this.m_fmod.Stop(this.m_soundChannel);
                }
                this.m_stopwatch.Stop();
                this.buttonPlayPause.Image = this.pictureBoxPlay.Image;
                this.labelTime.Text = "00:00.000";
                this.m_playing = this.m_paused = false;
                this.trackBarPosition.Enabled = true;
                this.trackBarPosition.Value = 0;
                this.progressBarCountdown.Visible = false;
            }
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            int num = (int) (this.trackBarPosition.Value * this.m_smallChange);
            this.labelTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
        }

        private int TrackBarValue()
        {
            return this.trackBarPosition.Value;
        }

        private void UpdateAudioButtons()
        {
            Vixen.Channel selectedItem = null;
            if (this.listBoxChannels.SelectedItem != null)
            {
                selectedItem = (Vixen.Channel) this.listBoxChannels.SelectedItem;
            }
            if (selectedItem != null)
            {
                this.buttonClear.Enabled = selectedItem.Enabled;
            }
            else
            {
                this.buttonClear.Enabled = false;
            }
        }

        private void UpdateKeyState(KeyEventArgs e, bool state)
        {
            if (!this.progressBarCountdown.Visible)
            {
                if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9))
                {
                    if (e.KeyCode == Keys.D0)
                    {
                        this.m_keyStates[this.m_keyMap[9]] = state;
                    }
                    else
                    {
                        this.m_keyStates[this.m_keyMap[((int) e.KeyCode) - 0x31]] = state;
                    }
                }
                else if ((e.KeyCode == Keys.ControlKey) && (this.listBoxChannels.SelectedItem != null))
                {
                    this.m_keyStates[this.listBoxChannels.SelectedIndex] = state;
                }
            }
        }

        private void UpdateRecordableLength()
        {
            int num = this.UpdateTotalTime();
            this.m_maxTime = num;
            this.trackBarPosition.Maximum = num;
            this.UpdateTrackbar();
            byte[,] dest = new byte[this.m_newEventValues.GetLength(0), (int) Math.Ceiling((double) (((float) num) / ((float) this.m_sequence.EventPeriod)))];
            this.CopyArray(this.m_newEventValues, dest);
            this.m_newEventValues = dest;
        }

        private int UpdateTotalTime()
        {
            if (base.InvokeRequired)
            {
                int milliseconds = 0;
                base.Invoke(delegate {
                    milliseconds = this.UpdateTotalTime();
                });
                return milliseconds;
            }
            int num = (this.m_sequence.Audio != null) ? (this.checkBoxAutoSize.Checked ? this.m_sequence.Audio.Duration : Math.Max(this.m_sequence.Time, this.m_sequence.Audio.Duration)) : this.m_sequence.Time;
            this.labelTotalTime.Text = string.Format("/ {0:d2}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
            return num;
        }

        private void UpdateTrackbar()
        {
            if (this.trackBarPosition.Maximum < 0x7d0)
            {
                this.m_smallChange = 100f;
            }
            else if (this.trackBarPosition.Maximum < 0x4e20)
            {
                this.m_smallChange = 1000f;
            }
            else if (this.trackBarPosition.Maximum < 0xea60)
            {
                this.m_smallChange = 2000f;
            }
            else
            {
                this.m_smallChange = 5000f;
            }
            this.trackBarPosition.Maximum = (int) Math.Round((double) (((float) this.trackBarPosition.Maximum) / this.m_smallChange), MidpointRounding.AwayFromZero);
        }

        private delegate void ProgressBarVisibleDelegate(bool value);

        private delegate int TrackBarValueDelegate();
    }
}

