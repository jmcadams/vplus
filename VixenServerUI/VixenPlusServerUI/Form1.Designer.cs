using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlusServerUI
{
    public partial class Form1 {

        private IContainer components = null;

        #region Windows Form Designer generated code

        private GroupBox groupBox1;
        private Button buttonClearLog;
        private CheckBox checkBoxLogging;
        private ListBox listBoxLog;
        private Label labelServerStatus;
        private Button buttonStop;
        private Button buttonStart;
        private ContextMenuStrip contextMenuStrip;

        private void InitializeComponent()
        {
            this.components = new Container();
            this.groupBox1 = new GroupBox();
            this.buttonClearLog = new Button();
            this.checkBoxLogging = new CheckBox();
            this.listBoxLog = new ListBox();
            this.labelServerStatus = new Label();
            this.buttonStop = new Button();
            this.buttonStart = new Button();
            this._notifyIcon = new NotifyIcon(this.components);
            this.contextMenuStrip = new ContextMenuStrip(this.components);
            this._startToolStripMenuItem = new ToolStripMenuItem();
            this._stopToolStripMenuItem = new ToolStripMenuItem();
            this._menuStrip = new MenuStrip();
            this._passwordToolStripMenuItem = new ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this._menuStrip.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonClearLog);
            this.groupBox1.Controls.Add(this.checkBoxLogging);
            this.groupBox1.Controls.Add(this.listBoxLog);
            this.groupBox1.Controls.Add(this.labelServerStatus);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x170, 0x14c);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            this.buttonClearLog.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonClearLog.Location = new Point(0x10a, 0x126);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new Size(0x4b, 23);
            this.buttonClearLog.TabIndex = 7;
            this.buttonClearLog.Text = "Clear";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new EventHandler(this.buttonClearLog_Click);
            this.checkBoxLogging.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Checked = true;
            this.checkBoxLogging.CheckState = CheckState.Checked;
            this.checkBoxLogging.Location = new Point(22, 0x12a);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new Size(0x60, 0x11);
            this.checkBoxLogging.TabIndex = 6;
            this.checkBoxLogging.Text = "Enable logging";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.listBoxLog.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.Location = new Point(22, 0x57);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new Size(0x13f, 0xc7);
            this.listBoxLog.TabIndex = 5;
            this.labelServerStatus.AutoSize = true;
            this.labelServerStatus.Location = new Point(29, 0x37);
            this.labelServerStatus.Name = "labelServerStatus";
            this.labelServerStatus.Size = new Size(0, 13);
            this.labelServerStatus.TabIndex = 4;
            this.buttonStop.Location = new Point(0x67, 25);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new Size(0x4b, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop server";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
            this.buttonStart.Location = new Point(22, 25);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(0x4b, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start server";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
            this._notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this._notifyIcon.Text = "Vixen Server (stopped)";
            this._notifyIcon.Visible = true;
            this._notifyIcon.DoubleClick += new EventHandler(this.notifyIcon_DoubleClick);
            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this._startToolStripMenuItem, this._stopToolStripMenuItem });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(110, 0x30);
            this._startToolStripMenuItem.Name = "startToolStripMenuItem";
            this._startToolStripMenuItem.Size = new Size(0x6d, 22);
            this._startToolStripMenuItem.Text = "Start";
            this._startToolStripMenuItem.Click += new EventHandler(this.startToolStripMenuItem_Click);
            this._stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this._stopToolStripMenuItem.Size = new Size(0x6d, 22);
            this._stopToolStripMenuItem.Text = "Stop";
            this._stopToolStripMenuItem.Click += new EventHandler(this.stopToolStripMenuItem_Click);
            this._menuStrip.Items.AddRange(new ToolStripItem[] { this._passwordToolStripMenuItem });
            this._menuStrip.Location = new Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new Size(0x188, 0x18);
            this._menuStrip.TabIndex = 3;
            this._menuStrip.Text = "menuStrip1";
            this._passwordToolStripMenuItem.Name = "passwordToolStripMenuItem";
            this._passwordToolStripMenuItem.Size = new Size(0x41, 20);
            this._passwordToolStripMenuItem.Text = "Password";
            this._passwordToolStripMenuItem.Click += new EventHandler(this.passwordToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x188, 0x173);
            base.Controls.Add(this._menuStrip);
            base.Controls.Add(this.groupBox1);
            base.MaximizeBox = false;
            base.Name = "Form1";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Vixen Server";
            base.Resize += new EventHandler(this.Form1_Resize);
            base.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
            base.Load += new EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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
