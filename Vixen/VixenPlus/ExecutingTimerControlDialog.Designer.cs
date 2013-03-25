using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class ExecutingTimerControlDialog
	{
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonPause;
private Button buttonPauseAll;
private Button buttonResume;
private Button buttonResumeAll;
private Button buttonStop;
private Button buttonStopAll;
private GroupBox groupBox1;
private ListBox listBoxTimers;
private System.Windows.Forms.Timer timerWatchdog;

		private void InitializeComponent()
		{
			this.components = new Container();
			this.groupBox1 = new GroupBox();
			this.listBoxTimers = new ListBox();
			this.buttonPause = new Button();
			this.buttonResume = new Button();
			this.buttonStop = new Button();
			this.buttonPauseAll = new Button();
			this.buttonResumeAll = new Button();
			this.buttonStopAll = new Button();
			this.timerWatchdog = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.listBoxTimers);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(200, 121);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Executing Timers";
			this.listBoxTimers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.listBoxTimers.FormattingEnabled = true;
			this.listBoxTimers.Location = new Point(6, 19);
			this.listBoxTimers.Name = "listBoxTimers";
			this.listBoxTimers.Size = new Size(188, 82);
			this.listBoxTimers.TabIndex = 0;
			this.listBoxTimers.SelectedIndexChanged += new EventHandler(this.listBoxTimers_SelectedIndexChanged);
			this.buttonPause.Enabled = false;
			this.buttonPause.Location = new Point(242, 31);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new Size(75, 23);
			this.buttonPause.TabIndex = 1;
			this.buttonPause.Text = "Pause";
			this.buttonPause.UseVisualStyleBackColor = true;
			this.buttonPause.Click += new EventHandler(this.buttonPause_Click);
			this.buttonResume.Enabled = false;
			this.buttonResume.Location = new Point(242, 60);
			this.buttonResume.Name = "buttonResume";
			this.buttonResume.Size = new Size(75, 23);
			this.buttonResume.TabIndex = 2;
			this.buttonResume.Text = "Resume";
			this.buttonResume.UseVisualStyleBackColor = true;
			this.buttonResume.Click += new EventHandler(this.buttonResume_Click);
			this.buttonStop.Enabled = false;
			this.buttonStop.Location = new Point(242, 89);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new Size(75, 23);
			this.buttonStop.TabIndex = 3;
			this.buttonStop.Text = "Stop";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
			this.buttonPauseAll.Location = new Point(353, 31);
			this.buttonPauseAll.Name = "buttonPauseAll";
			this.buttonPauseAll.Size = new Size(75, 23);
			this.buttonPauseAll.TabIndex = 4;
			this.buttonPauseAll.Text = "Pause all";
			this.buttonPauseAll.UseVisualStyleBackColor = true;
			this.buttonPauseAll.Click += new EventHandler(this.buttonPauseAll_Click);
			this.buttonResumeAll.Location = new Point(353, 60);
			this.buttonResumeAll.Name = "buttonResumeAll";
			this.buttonResumeAll.Size = new Size(75, 23);
			this.buttonResumeAll.TabIndex = 5;
			this.buttonResumeAll.Text = "Resume all";
			this.buttonResumeAll.UseVisualStyleBackColor = true;
			this.buttonResumeAll.Click += new EventHandler(this.buttonResumeAll_Click);
			this.buttonStopAll.Location = new Point(353, 89);
			this.buttonStopAll.Name = "buttonStopAll";
			this.buttonStopAll.Size = new Size(75, 23);
			this.buttonStopAll.TabIndex = 6;
			this.buttonStopAll.Text = "Stop all";
			this.buttonStopAll.UseVisualStyleBackColor = true;
			this.buttonStopAll.Click += new EventHandler(this.buttonStopAll_Click);
			this.timerWatchdog.Interval = 1000;
			this.timerWatchdog.Tick += new EventHandler(this.timerWatchdog_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(455, 145);
			base.Controls.Add(this.buttonStopAll);
			base.Controls.Add(this.buttonResumeAll);
			base.Controls.Add(this.buttonPauseAll);
			base.Controls.Add(this.buttonStop);
			base.Controls.Add(this.buttonResume);
			base.Controls.Add(this.buttonPause);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.Name = "ExecutingTimerControlDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Executing Timer Control";
			base.TopMost = true;
			base.VisibleChanged += new EventHandler(this.ExecutingTimerControlDialog_VisibleChanged);
			base.FormClosing += new FormClosingEventHandler(this.ExecutingTimerControlDialog_FormClosing);
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
