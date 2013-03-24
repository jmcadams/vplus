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
	using VixenPlus;

	public partial class Form1 : Form
	{
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

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
		//this.notifyIcon.Icon = (Icon)manager.GetObject("notifyIcon.Icon");


		private void Form1_Resize(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.Hide();
			}
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

