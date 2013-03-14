namespace VixenServerUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Form1 : Form
    {
        private Button buttonClearLog;
        private Button buttonStart;
        private Button buttonStop;
        private CheckBox checkBoxLogging;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip;
        private GroupBox groupBox1;
        private Label labelServerStatus;
        private ListBox listBoxLog;
        private List<string> m_argList;
        private XmlDocument m_configDoc;
        private Server m_server = null;
        private string m_serverDirectory;
        private MenuStrip menuStrip;
        private NotifyIcon notifyIcon;
        private ToolStripMenuItem passwordToolStripMenuItem;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;

        public Form1(string[] args)
        {
            this.InitializeComponent();
            this.m_serverDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string path = Path.Combine(this.m_serverDirectory, "server-config");
            if (File.Exists(path))
            {
                this.m_configDoc = new XmlDocument();
                this.m_configDoc.Load(path);
            }
            else
            {
                this.m_configDoc = VixenServerUI.Xml.CreateXmlDocument("Server");
                VixenServerUI.Xml.SetValue(this.m_configDoc.DocumentElement, "Password", string.Empty);
            }
            try
            {
                this.m_server = new Server();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                base.Close();
            }
            this.m_server.Password = this.m_configDoc.SelectSingleNode("//Server/Password").InnerText;
            this.m_server.ServerNotify += new Server.ServerNotifyEvent(this.m_server_ServerNotify);
            this.m_argList = new List<string>(args);
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            this.listBoxLog.BeginUpdate();
            this.listBoxLog.Items.Clear();
            this.listBoxLog.EndUpdate();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.StartServer();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.StopServer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_server.IsRunning)
            {
                this.m_server.Stop();
            }
            this.m_server.Shutdown();
            this.m_configDoc.Save(Path.Combine(this.m_serverDirectory, "server-config"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.m_argList.Contains("start"))
            {
                this.StartServer();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.Hide();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new GroupBox();
            this.buttonClearLog = new Button();
            this.checkBoxLogging = new CheckBox();
            this.listBoxLog = new ListBox();
            this.labelServerStatus = new Label();
            this.buttonStop = new Button();
            this.buttonStart = new Button();
            this.notifyIcon = new NotifyIcon(this.components);
            this.contextMenuStrip = new ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new ToolStripMenuItem();
            this.stopToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip = new MenuStrip();
            this.passwordToolStripMenuItem = new ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonClearLog);
            this.groupBox1.Controls.Add(this.checkBoxLogging);
            this.groupBox1.Controls.Add(this.listBoxLog);
            this.groupBox1.Controls.Add(this.labelServerStatus);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new Point(12, 0x1b);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x170, 0x14c);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            this.buttonClearLog.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonClearLog.Location = new Point(0x10a, 0x126);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new Size(0x4b, 0x17);
            this.buttonClearLog.TabIndex = 7;
            this.buttonClearLog.Text = "Clear";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new EventHandler(this.buttonClearLog_Click);
            this.checkBoxLogging.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Checked = true;
            this.checkBoxLogging.CheckState = CheckState.Checked;
            this.checkBoxLogging.Location = new Point(0x16, 0x12a);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new Size(0x60, 0x11);
            this.checkBoxLogging.TabIndex = 6;
            this.checkBoxLogging.Text = "Enable logging";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.listBoxLog.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.Location = new Point(0x16, 0x57);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new Size(0x13f, 0xc7);
            this.listBoxLog.TabIndex = 5;
            this.labelServerStatus.AutoSize = true;
            this.labelServerStatus.Location = new Point(0x1d, 0x37);
            this.labelServerStatus.Name = "labelServerStatus";
            this.labelServerStatus.Size = new Size(0, 13);
            this.labelServerStatus.TabIndex = 4;
            this.buttonStop.Location = new Point(0x67, 0x19);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new Size(0x4b, 0x17);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop server";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
            this.buttonStart.Location = new Point(0x16, 0x19);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(0x4b, 0x17);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start server";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            this.notifyIcon.Text = "Vixen Server (stopped)";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new EventHandler(this.notifyIcon_DoubleClick);
            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.startToolStripMenuItem, this.stopToolStripMenuItem });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(110, 0x30);
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new Size(0x6d, 0x16);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new EventHandler(this.startToolStripMenuItem_Click);
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new Size(0x6d, 0x16);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new EventHandler(this.stopToolStripMenuItem_Click);
            this.menuStrip.Items.AddRange(new ToolStripItem[] { this.passwordToolStripMenuItem });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(0x188, 0x18);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            this.passwordToolStripMenuItem.Name = "passwordToolStripMenuItem";
            this.passwordToolStripMenuItem.Size = new Size(0x41, 20);
            this.passwordToolStripMenuItem.Text = "Password";
            this.passwordToolStripMenuItem.Click += new EventHandler(this.passwordToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x188, 0x173);
            base.Controls.Add(this.menuStrip);
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
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void m_server_ServerNotify(string message)
        {
            this.labelServerStatus.BeginInvoke(new ServerNotifyDelegate(this.UpdateStatus), new object[] { message });
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            base.Show();
            base.WindowState = FormWindowState.Normal;
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_configDoc.SelectSingleNode("//Server/Password");
            PasswordDialog dialog = new PasswordDialog(node.InnerText);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                node.InnerText = dialog.Password;
            }
        }

        private void StartServer()
        {
            this.m_server.Start();
            this.notifyIcon.Text = "Vixen Server (started)";
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartServer();
        }

        private void StopServer()
        {
            this.m_server.Stop();
            this.notifyIcon.Text = "Vixen Server (stopped)";
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StopServer();
        }

        private void UpdateStatus(string message)
        {
            this.labelServerStatus.Text = message;
            if (this.checkBoxLogging.Checked)
            {
                this.listBoxLog.Items.Insert(0, message);
            }
        }

        private delegate void ServerNotifyDelegate(string message);
    }
}

