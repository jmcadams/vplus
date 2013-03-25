namespace VixenEditor
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus;

	//TODO: Convert this file
	internal class TestConsoleDialog : Form
	{
		private Button buttonDone;
		private IContainer components = null;
		private ConsoleTrackBar consoleTrackBar1;
		private ConsoleTrackBar consoleTrackBar2;
		private ConsoleTrackBar consoleTrackBar3;
		private ConsoleTrackBar consoleTrackBar4;
		private ConsoleTrackBar consoleTrackBar5;
		private ConsoleTrackBar consoleTrackBar6;
		private ConsoleTrackBar consoleTrackBar7;
		private ConsoleTrackBar consoleTrackBarMaster;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private List<ConsoleTrackBar> m_channelControls = new List<ConsoleTrackBar>();
		private byte[] m_channelLevels;
		private Channel[] m_channels;
		private int m_executionContextHandle;
		private IExecution m_executionInterface;

		public TestConsoleDialog(EventSequence sequence, IExecution executionInterface)
		{
			this.InitializeComponent();
			this.m_executionInterface = executionInterface;
			this.m_executionContextHandle = this.m_executionInterface.RequestContext(false, true, null);
			this.m_executionInterface.SetAsynchronousContext(this.m_executionContextHandle, sequence);
			this.m_channelLevels = new byte[sequence.ChannelCount];
			this.m_channels = sequence.Channels.ToArray();
			string[] strArray = new string[sequence.ChannelCount + 1];
			for (int i = 1; i <= sequence.ChannelCount; i++)
			{
				strArray[i] = sequence.Channels[i - 1].Name;
			}
			strArray[0] = " -- unassigned -- ";
			foreach (Control control in this.groupBox2.Controls)
			{
				ConsoleTrackBar item = control as ConsoleTrackBar;
				if ((item != null) && (item.Master != null))
				{
					item.TextStrings = strArray;
					item.ResetIndex = 0;
					this.m_channelControls.Add(item);
				}
			}
		}

		private void buttonDone_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void consoleTrackBar_ValueChanged(object sender)
		{
			ConsoleTrackBar bar = sender as ConsoleTrackBar;
			if (bar.SelectedTextIndex > 0)
			{
				this.UpdateChannelFrom(sender as ConsoleTrackBar);
				this.UpdateOutput();
			}
		}

		private void consoleTrackBarMaster_ValueChanged(object sender)
		{
			foreach (ConsoleTrackBar bar in this.m_channelControls)
			{
				this.UpdateChannelFrom(bar);
			}
			this.UpdateOutput();
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
			this.groupBox1 = new GroupBox();
			this.consoleTrackBarMaster = new ConsoleTrackBar();
			this.groupBox2 = new GroupBox();
			this.consoleTrackBar7 = new ConsoleTrackBar();
			this.consoleTrackBar5 = new ConsoleTrackBar();
			this.consoleTrackBar6 = new ConsoleTrackBar();
			this.consoleTrackBar3 = new ConsoleTrackBar();
			this.consoleTrackBar4 = new ConsoleTrackBar();
			this.consoleTrackBar2 = new ConsoleTrackBar();
			this.consoleTrackBar1 = new ConsoleTrackBar();
			this.buttonDone = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.consoleTrackBarMaster);
			this.groupBox1.Location = new Point(8, 0x10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(80, 0x127);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Master";
			this.consoleTrackBarMaster.AllowText = false;
			this.consoleTrackBarMaster.CascadeMasterEvents = false;
			this.consoleTrackBarMaster.Location = new Point(6, 20);
			this.consoleTrackBarMaster.Master = null;
			this.consoleTrackBarMaster.Name = "consoleTrackBarMaster";
			this.consoleTrackBarMaster.Size = new Size(0x44, 0x10b);
			this.consoleTrackBarMaster.TabIndex = 0;
			this.consoleTrackBarMaster.Value = 0;
			this.consoleTrackBarMaster.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBarMaster_ValueChanged);
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.consoleTrackBar7);
			this.groupBox2.Controls.Add(this.consoleTrackBar5);
			this.groupBox2.Controls.Add(this.consoleTrackBar6);
			this.groupBox2.Controls.Add(this.consoleTrackBar3);
			this.groupBox2.Controls.Add(this.consoleTrackBar4);
			this.groupBox2.Controls.Add(this.consoleTrackBar2);
			this.groupBox2.Controls.Add(this.consoleTrackBar1);
			this.groupBox2.Location = new Point(0x77, 0x10);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x2b5, 0x127);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Channels";
			this.consoleTrackBar7.AllowText = true;
			this.consoleTrackBar7.CascadeMasterEvents = false;
			this.consoleTrackBar7.Location = new Point(0x24c, 0x11);
			this.consoleTrackBar7.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar7.Name = "consoleTrackBar7";
			this.consoleTrackBar7.Size = new Size(90, 0x10c);
			this.consoleTrackBar7.TabIndex = 6;
			this.consoleTrackBar7.Value = 0;
			this.consoleTrackBar7.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar5.AllowText = true;
			this.consoleTrackBar5.CascadeMasterEvents = false;
			this.consoleTrackBar5.Location = new Point(0x1ec, 0x11);
			this.consoleTrackBar5.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar5.Name = "consoleTrackBar5";
			this.consoleTrackBar5.Size = new Size(90, 0x10c);
			this.consoleTrackBar5.TabIndex = 5;
			this.consoleTrackBar5.Value = 0;
			this.consoleTrackBar5.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar6.AllowText = true;
			this.consoleTrackBar6.CascadeMasterEvents = false;
			this.consoleTrackBar6.Location = new Point(0x18c, 0x12);
			this.consoleTrackBar6.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar6.Name = "consoleTrackBar6";
			this.consoleTrackBar6.Size = new Size(90, 0x10c);
			this.consoleTrackBar6.TabIndex = 4;
			this.consoleTrackBar6.Value = 0;
			this.consoleTrackBar6.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar3.AllowText = true;
			this.consoleTrackBar3.CascadeMasterEvents = false;
			this.consoleTrackBar3.Location = new Point(300, 0x12);
			this.consoleTrackBar3.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar3.Name = "consoleTrackBar3";
			this.consoleTrackBar3.Size = new Size(90, 0x10c);
			this.consoleTrackBar3.TabIndex = 3;
			this.consoleTrackBar3.Value = 0;
			this.consoleTrackBar3.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar4.AllowText = true;
			this.consoleTrackBar4.CascadeMasterEvents = false;
			this.consoleTrackBar4.Location = new Point(0xcc, 0x13);
			this.consoleTrackBar4.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar4.Name = "consoleTrackBar4";
			this.consoleTrackBar4.Size = new Size(90, 0x10c);
			this.consoleTrackBar4.TabIndex = 2;
			this.consoleTrackBar4.Value = 0;
			this.consoleTrackBar4.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar2.AllowText = true;
			this.consoleTrackBar2.CascadeMasterEvents = false;
			this.consoleTrackBar2.Location = new Point(0x6c, 0x13);
			this.consoleTrackBar2.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar2.Name = "consoleTrackBar2";
			this.consoleTrackBar2.Size = new Size(90, 0x10c);
			this.consoleTrackBar2.TabIndex = 1;
			this.consoleTrackBar2.Value = 0;
			this.consoleTrackBar2.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.consoleTrackBar1.AllowText = true;
			this.consoleTrackBar1.CascadeMasterEvents = false;
			this.consoleTrackBar1.Location = new Point(12, 20);
			this.consoleTrackBar1.Master = this.consoleTrackBarMaster;
			this.consoleTrackBar1.Name = "consoleTrackBar1";
			this.consoleTrackBar1.Size = new Size(90, 0x10c);
			this.consoleTrackBar1.TabIndex = 0;
			this.consoleTrackBar1.Value = 0;
			this.consoleTrackBar1.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
			this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonDone.Location = new Point(0x2e1, 0x14e);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new Size(0x4b, 23);
			this.buttonDone.TabIndex = 4;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonDone;
			base.ClientSize = new Size(0x338, 0x171);
			base.Controls.Add(this.buttonDone);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.Name = "TestConsoleDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Test Console";
			base.FormClosing += new FormClosingEventHandler(this.TestConsoleDialog_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void TestConsoleDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
		}

		private void UpdateChannelFrom(ConsoleTrackBar consoleTrackBar)
		{
			if ((consoleTrackBar != null) && (consoleTrackBar.SelectedTextIndex > 0))
			{
				this.UpdateChannelValue(consoleTrackBar.SelectedTextIndex - 1, (byte) consoleTrackBar.Value);
			}
		}

		private void UpdateChannelValue(int channelNaturalIndex, byte value)
		{
			this.m_channelLevels[this.m_channels[channelNaturalIndex].OutputChannel] = value;
		}

		private void UpdateOutput()
		{
			this.m_executionInterface.SetChannelStates(this.m_executionContextHandle, this.m_channelLevels);
		}
	}
}

