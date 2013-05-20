using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus {
    internal partial class ExecutingTimerControlDialog {
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


        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxTimers = new System.Windows.Forms.ListBox();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonResume = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPauseAll = new System.Windows.Forms.Button();
            this.buttonResumeAll = new System.Windows.Forms.Button();
            this.buttonStopAll = new System.Windows.Forms.Button();
            this.timerWatchdog = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.listBoxTimers);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 121);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Executing Timers";
            // 
            // listBoxTimers
            // 
            this.listBoxTimers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTimers.FormattingEnabled = true;
            this.listBoxTimers.Location = new System.Drawing.Point(6, 19);
            this.listBoxTimers.Name = "listBoxTimers";
            this.listBoxTimers.Size = new System.Drawing.Size(188, 82);
            this.listBoxTimers.TabIndex = 0;
            this.listBoxTimers.SelectedIndexChanged += new System.EventHandler(this.listBoxTimers_SelectedIndexChanged);
            // 
            // buttonPause
            // 
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new System.Drawing.Point(242, 31);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonResume
            // 
            this.buttonResume.Enabled = false;
            this.buttonResume.Location = new System.Drawing.Point(242, 60);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(75, 23);
            this.buttonResume.TabIndex = 2;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = true;
            this.buttonResume.Click += new System.EventHandler(this.buttonResume_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(242, 89);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPauseAll
            // 
            this.buttonPauseAll.Location = new System.Drawing.Point(353, 31);
            this.buttonPauseAll.Name = "buttonPauseAll";
            this.buttonPauseAll.Size = new System.Drawing.Size(75, 23);
            this.buttonPauseAll.TabIndex = 4;
            this.buttonPauseAll.Text = "Pause all";
            this.buttonPauseAll.UseVisualStyleBackColor = true;
            this.buttonPauseAll.Click += new System.EventHandler(this.buttonPauseAll_Click);
            // 
            // buttonResumeAll
            // 
            this.buttonResumeAll.Location = new System.Drawing.Point(353, 60);
            this.buttonResumeAll.Name = "buttonResumeAll";
            this.buttonResumeAll.Size = new System.Drawing.Size(75, 23);
            this.buttonResumeAll.TabIndex = 5;
            this.buttonResumeAll.Text = "Resume all";
            this.buttonResumeAll.UseVisualStyleBackColor = true;
            this.buttonResumeAll.Click += new System.EventHandler(this.buttonResumeAll_Click);
            // 
            // buttonStopAll
            // 
            this.buttonStopAll.Location = new System.Drawing.Point(353, 89);
            this.buttonStopAll.Name = "buttonStopAll";
            this.buttonStopAll.Size = new System.Drawing.Size(75, 23);
            this.buttonStopAll.TabIndex = 6;
            this.buttonStopAll.Text = "Stop all";
            this.buttonStopAll.UseVisualStyleBackColor = true;
            this.buttonStopAll.Click += new System.EventHandler(this.buttonStopAll_Click);
            // 
            // timerWatchdog
            // 
            this.timerWatchdog.Interval = 1000;
            this.timerWatchdog.Tick += new System.EventHandler(this.timerWatchdog_Tick);
            // 
            // ExecutingTimerControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 145);
            this.Controls.Add(this.buttonStopAll);
            this.Controls.Add(this.buttonResumeAll);
            this.Controls.Add(this.buttonPauseAll);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonResume);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.Name = "ExecutingTimerControlDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Executing Timer Control";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExecutingTimerControlDialog_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.ExecutingTimerControlDialog_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
