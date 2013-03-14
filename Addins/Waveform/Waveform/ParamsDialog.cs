namespace Waveform
{
    using FMOD;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Vixen;

    public class ParamsDialog : Form
    {
        private Button buttonStart;
        private CheckBox checkBoxAutoScale;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label labelScale;
        private ListBox listBoxChannels;
        private int m_bits = 0;
        private int m_channels = 0;
        private bool m_completed = false;
        private int m_highestValue = 0;
        private uint m_len1 = 0;
        private uint m_len2 = 0;
        private uint m_lenbytes = 0;
        private uint m_lenMilliseconds = 0;
        private short[] m_levels;
        private IntPtr m_ptr1 = IntPtr.Zero;
        private IntPtr m_ptr2 = IntPtr.Zero;
        private float m_rate = 0f;
        private RunState m_run = RunState.Stopped;
        private float m_scale;
        private EventSequence m_sequence;
        private Sound m_sound = null;
        private MethodInvoker m_stepInvoker;
        private _System m_system = null;
        private ProgressBar progressBar1;
        private RadioButton radioButtonChannelRange;
        private RadioButton radioButtonSingleChannel;
        private TextBox textBoxScale;

        public ParamsDialog(EventSequence sequence)
        {
            this.InitializeComponent();
            this.m_sequence = sequence;
            this.m_stepInvoker = new MethodInvoker(this.progressBar1.PerformStep);
            this.listBoxChannels.Items.AddRange(sequence.Channels.ToArray());
            Factory.System_Create(ref this.m_system);
            this.m_system.init(2, INITFLAG.NORMAL, IntPtr.Zero);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.buttonStart.Text == "Start")
            {
                if (this.listBoxChannels.SelectedItems.Count == 0)
                {
                    MessageBox.Show("There are no channels selected", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if ((!this.radioButtonChannelRange.Checked || (this.listBoxChannels.SelectedItems.Count != 1)) || (MessageBox.Show("You selected to use multiple channels but only selected one channel.\nIs this really what you want?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No))
                {
                    RESULT result;
                    this.buttonStart.Enabled = false;
                    SOUND_TYPE rAW = SOUND_TYPE.RAW;
                    SOUND_FORMAT nONE = SOUND_FORMAT.NONE;
                    float volume = 0f;
                    float pan = 0f;
                    int priority = 0;
                    this.label1.Text = "Loading audio, please wait...";
                    this.label1.Refresh();
                    if ((result = this.m_system.createSound(Path.Combine(Paths.AudioPath, this.m_sequence.Audio.FileName), MODE.ACCURATETIME | MODE._2D | MODE.HARDWARE | MODE.CREATESAMPLE, ref this.m_sound)) != RESULT.OK)
                    {
                        MessageBox.Show(string.Format("Sound system error ({0})\n\n{1}", result, Error.String(result)), "Waveform", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.buttonStart.Enabled = true;
                        this.label1.Text = string.Empty;
                    }
                    else
                    {
                        this.m_sound.getFormat(ref rAW, ref nONE, ref this.m_channels, ref this.m_bits);
                        this.m_sound.getDefaults(ref this.m_rate, ref volume, ref pan, ref priority);
                        this.m_sound.getLength(ref this.m_lenbytes, TIMEUNIT.PCMBYTES);
                        this.m_sound.getLength(ref this.m_lenMilliseconds, TIMEUNIT.MS);
                        this.buttonStart.Enabled = true;
                        this.progressBar1.Maximum = (int) (((ulong) this.m_lenMilliseconds) / ((long) this.m_sequence.EventPeriod));
                        this.progressBar1.Value = this.progressBar1.Minimum;
                        this.progressBar1.Visible = true;
                        this.label1.Text = "Calculating...";
                        this.buttonStart.Text = "Stop";
                        this.m_highestValue = 0;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ParsePCM));
                    }
                }
            }
            else
            {
                this.RequestStop();
            }
        }

        private void checkBoxAutoScale_CheckedChanged(object sender, EventArgs e)
        {
            this.labelScale.Enabled = this.textBoxScale.Enabled = !this.checkBoxAutoScale.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Done()
        {
            this.ReleaseAllocations();
            if (!this.checkBoxAutoScale.Checked)
            {
                try
                {
                    this.m_scale = Convert.ToSingle(this.textBoxScale.Text);
                }
                catch
                {
                    MessageBox.Show(this.textBoxScale.Text + " is not a valid number.\nReverting to a default scale of 1.0", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.m_scale = 1f;
                }
            }
            else
            {
                this.m_scale = 32767f / ((float) this.m_highestValue);
            }
            int num = (1 << (this.m_bits & 0x1f)) >> 1;
            int num3 = 0;
            int num4 = 0;
            int selectedIndex = 0;
            int num6 = 0;
            int index = 0;
            try
            {
                float num2;
                if (this.radioButtonChannelRange.Checked)
                {
                    num3 = this.listBoxChannels.SelectedIndices[0];
                    num4 = num3;
                    selectedIndex = 1;
                    while ((selectedIndex < this.listBoxChannels.SelectedIndices.Count) && (this.listBoxChannels.SelectedIndices[selectedIndex++] == (num4 + 1)))
                    {
                        num4++;
                    }
                    num4++;
                    int num8 = num4 - num3;
                    for (index = 0; index < this.m_levels.Length; index++)
                    {
                        num2 = ((float) this.m_levels[index]) / ((float) num);
                        num2 = Math.Min((float) (num2 * this.m_scale), (float) 1f);
                        num6 = ((int) Math.Round((double) (num8 * num2), MidpointRounding.AwayFromZero)) + num3;
                        selectedIndex = num3;
                        while (selectedIndex < num6)
                        {
                            this.m_sequence.EventValues[selectedIndex, index] = this.m_sequence.MaximumLevel;
                            selectedIndex++;
                        }
                        while (selectedIndex < num4)
                        {
                            this.m_sequence.EventValues[selectedIndex, index] = this.m_sequence.MinimumLevel;
                            selectedIndex++;
                        }
                    }
                }
                else
                {
                    selectedIndex = this.listBoxChannels.SelectedIndex;
                    index = 0;
                    while (index < this.m_levels.Length)
                    {
                        num2 = ((float) this.m_levels[index]) / ((float) num);
                        num2 = Math.Min((float) (num2 * this.m_scale), (float) 1f);
                        this.m_sequence.EventValues[selectedIndex, index] = Math.Min(this.m_sequence.MaximumLevel, (byte) (num2 * 255f));
                        index++;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Exception: {0}\n\nStart channel = {1}\nEnd channel = {2}\nUpper channel = {3}, Channel index = {4}, m_levels[i] = {5}", new object[] { exception.Message, num3, num4, num6, selectedIndex, this.m_levels[index] }));
            }
            this.label1.Text = "Completed";
            this.m_completed = true;
            MessageBox.Show("Done.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private short FindHighValue(int startSample, int endSample)
        {
            int num7;
            int num8;
            short num = -32768;
            int num2 = (this.m_bits >> 3) * this.m_channels;
            int num3 = startSample * num2;
            int num4 = endSample * num2;
            int num5 = endSample - startSample;
            int length = num2 * num5;
            byte[] destination = new byte[length];
            this.m_sound.@lock((uint) num3, (uint) length, ref this.m_ptr1, ref this.m_ptr2, ref this.m_len1, ref this.m_len2);
            Marshal.Copy(this.m_ptr1, destination, 0, length);
            num3 = 0;
            if (this.m_bits == 0x10)
            {
                for (num8 = 0; num8 < num5; num8++)
                {
                    num7 = 0;
                    while (num7 < this.m_channels)
                    {
                        num = Math.Max(num, BitConverter.ToInt16(destination, num3 + (num7 * 2)));
                        num7++;
                    }
                    num3 += num2;
                }
            }
            else if (this.m_bits == 8)
            {
                for (num8 = 0; num8 < num5; num8++)
                {
                    for (num7 = 0; num7 < this.m_channels; num7++)
                    {
                        num = Math.Max(num, (sbyte) destination[num3 + num7]);
                    }
                    num3 += num2;
                }
            }
            this.m_sound.unlock(this.m_ptr1, this.m_ptr2, this.m_len1, this.m_len2);
            return num;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.textBoxScale = new TextBox();
            this.labelScale = new Label();
            this.radioButtonChannelRange = new RadioButton();
            this.radioButtonSingleChannel = new RadioButton();
            this.listBoxChannels = new ListBox();
            this.buttonStart = new Button();
            this.progressBar1 = new ProgressBar();
            this.label1 = new Label();
            this.checkBoxAutoScale = new CheckBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.checkBoxAutoScale);
            this.groupBox1.Controls.Add(this.textBoxScale);
            this.groupBox1.Controls.Add(this.labelScale);
            this.groupBox1.Controls.Add(this.radioButtonChannelRange);
            this.groupBox1.Controls.Add(this.radioButtonSingleChannel);
            this.groupBox1.Location = new Point(10, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xef, 0x89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Waveform representation";
            this.textBoxScale.Location = new Point(110, 0x4a);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new Size(0x34, 20);
            this.textBoxScale.TabIndex = 3;
            this.textBoxScale.Text = "1.0";
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new Point(40, 0x4d);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new Size(0x40, 13);
            this.labelScale.TabIndex = 2;
            this.labelScale.Text = "Scale factor";
            this.radioButtonChannelRange.AutoSize = true;
            this.radioButtonChannelRange.Checked = true;
            this.radioButtonChannelRange.Location = new Point(0x19, 0x13);
            this.radioButtonChannelRange.Name = "radioButtonChannelRange";
            this.radioButtonChannelRange.Size = new Size(0xc4, 0x11);
            this.radioButtonChannelRange.TabIndex = 0;
            this.radioButtonChannelRange.TabStop = true;
            this.radioButtonChannelRange.Text = "On/Off values over a channel range";
            this.radioButtonChannelRange.UseVisualStyleBackColor = true;
            this.radioButtonChannelRange.CheckedChanged += new EventHandler(this.radioButtonChannelRange_CheckedChanged);
            this.radioButtonSingleChannel.AutoSize = true;
            this.radioButtonSingleChannel.Location = new Point(0x19, 0x2a);
            this.radioButtonSingleChannel.Name = "radioButtonSingleChannel";
            this.radioButtonSingleChannel.Size = new Size(0xbc, 0x11);
            this.radioButtonSingleChannel.TabIndex = 1;
            this.radioButtonSingleChannel.Text = "Dimmed values in a single channel";
            this.radioButtonSingleChannel.UseVisualStyleBackColor = true;
            this.radioButtonSingleChannel.CheckedChanged += new EventHandler(this.radioButtonSingleChannel_CheckedChanged);
            this.listBoxChannels.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(0x110, 0x17);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(0xac, 0xc7);
            this.listBoxChannels.TabIndex = 2;
            this.buttonStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonStart.Location = new Point(12, 0x9d);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(0x4b, 0x17);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
            this.progressBar1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.progressBar1.Location = new Point(10, 0xc7);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0xef, 0x17);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Visible = false;
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0xb7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0, 13);
            this.label1.TabIndex = 4;
            this.checkBoxAutoScale.AutoSize = true;
            this.checkBoxAutoScale.Location = new Point(0x2b, 0x67);
            this.checkBoxAutoScale.Name = "checkBoxAutoScale";
            this.checkBoxAutoScale.Size = new Size(0x75, 0x11);
            this.checkBoxAutoScale.TabIndex = 4;
            this.checkBoxAutoScale.Text = "Auto-scale to 100%";
            this.checkBoxAutoScale.UseVisualStyleBackColor = true;
            this.checkBoxAutoScale.CheckedChanged += new EventHandler(this.checkBoxAutoScale_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1cf, 0xf2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.progressBar1);
            base.Controls.Add(this.buttonStart);
            base.Controls.Add(this.listBoxChannels);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ParamsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Waveform";
            base.FormClosing += new FormClosingEventHandler(this.ParamsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ParamsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_run != RunState.Stopped)
            {
                if (MessageBox.Show("Do you want to stop?", "Waveform", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.RequestStop();
                }
            }
            this.ReleaseAllocations();
        }

        private void ParsePCM(object obj)
        {
            double num = 1000.0 / ((double) this.m_sequence.EventPeriod);
            double num2 = (((double) this.m_lenbytes) / ((double) this.m_channels)) / ((double) (this.m_bits / 8));
            int num3 = (int) (((ulong) this.m_lenMilliseconds) / ((long) this.m_sequence.EventPeriod));
            double num4 = num2 / ((double) num3);
            this.m_levels = new short[num3];
            this.m_run = RunState.Running;
            this.m_completed = false;
            int index = 0;
            while ((index < num3) && (this.m_run == RunState.Running))
            {
                int startSample = (int) Math.Round((double) (index * num4), MidpointRounding.AwayFromZero);
                int endSample = (int) Math.Round((double) ((index + 1) * num4), MidpointRounding.AwayFromZero);
                short num7 = this.FindHighValue(startSample, endSample);
                this.m_highestValue = Math.Max(num7, this.m_highestValue);
                this.m_levels[index] = num7;
                base.BeginInvoke(this.m_stepInvoker);
                index++;
            }
            this.m_run = RunState.Stopped;
            if (index == num3)
            {
                base.BeginInvoke(new MethodInvoker(this.Done));
            }
        }

        private void radioButtonChannelRange_CheckedChanged(object sender, EventArgs e)
        {
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
        }

        private void radioButtonSingleChannel_CheckedChanged(object sender, EventArgs e)
        {
            this.listBoxChannels.SelectionMode = SelectionMode.One;
        }

        private void ReleaseAllocations()
        {
            if (this.m_sound != null)
            {
                this.m_sound.release();
                this.m_sound = null;
            }
            if (this.m_system != null)
            {
                this.m_system.close();
                this.m_system.release();
                this.m_system = null;
            }
        }

        private void RequestStop()
        {
            this.label1.Text = "Stopping...";
            this.buttonStart.Enabled = false;
            this.m_run = RunState.StopRequested;
            while (this.m_run != RunState.Stopped)
            {
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Visible = false;
            this.label1.Text = "Stopped";
            this.buttonStart.Text = "Start";
            this.buttonStart.Enabled = true;
            if (this.m_sound != null)
            {
                this.m_sound.release();
                this.m_sound = null;
            }
        }

        public bool Completed
        {
            get
            {
                return this.m_completed;
            }
        }

        private enum RunState
        {
            Stopped,
            Running,
            StopRequested
        }
    }
}

