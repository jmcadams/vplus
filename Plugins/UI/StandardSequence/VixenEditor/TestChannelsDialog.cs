namespace VixenEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    internal class TestChannelsDialog : Form
    {
        private Button buttonDone;
        private Button buttonSelectAll;
        private Button buttonUnselectAll;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label labelLevel;
        private ListBox listBoxChannels;
        private byte[] m_channelLevels;
        private List<Channel> m_channels;
        private int m_executionContextHandle;
        private IExecution m_executionInterface;
        private bool m_internal = false;
        private EventSequence m_sequence;
        private TrackBar trackBar;

        public TestChannelsDialog(EventSequence sequence, IExecution executionInterface)
        {
            this.InitializeComponent();
            this.m_executionInterface = executionInterface;
            this.m_sequence = sequence;
            this.m_channels = sequence.Channels;
            this.listBoxChannels.Items.AddRange(this.m_channels.ToArray());
            this.trackBar.Maximum = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ActualLevels") ? 0xff : 100;
            this.m_channelLevels = new byte[sequence.ChannelCount];
            this.m_executionContextHandle = this.m_executionInterface.RequestContext(false, true, null);
            this.m_executionInterface.SetAsynchronousContext(this.m_executionContextHandle, this.m_sequence);
            base.BringToFront();
            this.trackBar.Value = this.trackBar.Maximum;
        }

        private void buttonAllOff_Click(object sender, EventArgs e)
        {
            this.m_internal = true;
            this.listBoxChannels.BeginUpdate();
            for (int i = 0; i < this.listBoxChannels.Items.Count; i++)
            {
                this.listBoxChannels.SetSelected(i, false);
                this.m_channelLevels[this.m_channels[i].OutputChannel] = 0;
            }
            this.listBoxChannels.EndUpdate();
            this.m_internal = false;
            this.UpdateOutput();
        }

        private void buttonAllOn_Click(object sender, EventArgs e)
        {
            this.m_internal = true;
            this.listBoxChannels.BeginUpdate();
            for (int i = 0; i < this.listBoxChannels.Items.Count; i++)
            {
                this.listBoxChannels.SetSelected(i, true);
                this.m_channelLevels[this.m_channels[i].OutputChannel] = this.LevelFromTrackBar();
            }
            this.listBoxChannels.EndUpdate();
            this.m_internal = false;
            this.UpdateOutput();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            base.Close();
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
            this.buttonDone = new Button();
            this.groupBox1 = new GroupBox();
            this.listBoxChannels = new ListBox();
            this.labelLevel = new Label();
            this.trackBar = new TrackBar();
            this.buttonUnselectAll = new Button();
            this.buttonSelectAll = new Button();
            this.groupBox1.SuspendLayout();
            this.trackBar.BeginInit();
            base.SuspendLayout();
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0xff, 0x14b);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.listBoxChannels);
            this.groupBox1.Controls.Add(this.labelLevel);
            this.groupBox1.Controls.Add(this.trackBar);
            this.groupBox1.Controls.Add(this.buttonUnselectAll);
            this.groupBox1.Controls.Add(this.buttonSelectAll);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x13e, 0x131);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(6, 0x19);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(0xfe, 0xee);
            this.listBoxChannels.TabIndex = 8;
            this.listBoxChannels.SelectedIndexChanged += new EventHandler(this.listBoxChannels_SelectedIndexChanged);
            this.labelLevel.AutoSize = true;
            this.labelLevel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelLevel.Location = new Point(0x10c, 0x114);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new Size(0x13, 20);
            this.labelLevel.TabIndex = 7;
            this.labelLevel.Text = "0";
            this.trackBar.Location = new Point(0x10a, 12);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = Orientation.Vertical;
            this.trackBar.Size = new Size(0x2d, 0x108);
            this.trackBar.TabIndex = 6;
            this.trackBar.ValueChanged += new EventHandler(this.trackBar_ValueChanged);
            this.buttonUnselectAll.Location = new Point(0x57, 0x10d);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new Size(0x4b, 0x17);
            this.buttonUnselectAll.TabIndex = 5;
            this.buttonUnselectAll.Text = "Unselect all";
            this.buttonUnselectAll.UseVisualStyleBackColor = true;
            this.buttonUnselectAll.Click += new EventHandler(this.buttonAllOff_Click);
            this.buttonSelectAll.Location = new Point(6, 0x10d);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new Size(0x4b, 0x17);
            this.buttonSelectAll.TabIndex = 4;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new EventHandler(this.buttonAllOn_Click);
            base.AcceptButton = this.buttonDone;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonDone;
            base.ClientSize = new Size(0x156, 0x16e);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.buttonDone);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "TestChannelsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Test Channels";
            base.FormClosing += new FormClosingEventHandler(this.TestChannelsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.trackBar.EndInit();
            base.ResumeLayout(false);
        }

        private byte LevelFromTrackBar()
        {
            return ((this.trackBar.Maximum == 0xff) ? ((byte) this.trackBar.Value) : ((byte) Math.Round((double) ((this.trackBar.Value * 255f) / 100f))));
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_internal)
            {
                this.UpdateAllChannels();
            }
        }

        private void TestChannelsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            this.labelLevel.Text = this.trackBar.Value.ToString();
            byte num = (this.trackBar.Maximum == 0xff) ? ((byte) this.trackBar.Value) : ((byte) Math.Round((double) ((this.trackBar.Value * 255f) / 100f)));
            foreach (int num2 in this.listBoxChannels.SelectedIndices)
            {
                this.m_channelLevels[this.m_channels[num2].OutputChannel] = num;
            }
            this.UpdateOutput();
        }

        private void UpdateAllChannels()
        {
            for (int i = 0; i < this.m_channelLevels.Length; i++)
            {
                this.m_channelLevels[this.m_channels[i].OutputChannel] = this.listBoxChannels.GetSelected(i) ? this.LevelFromTrackBar() : ((byte) 0);
            }
            this.UpdateOutput();
        }

        private void UpdateOutput()
        {
            this.m_executionInterface.SetChannelStates(this.m_executionContextHandle, this.m_channelLevels);
        }
    }
}

