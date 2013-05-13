using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using VixenPlus;

namespace VixenPlusServerUI
{
    public partial class Form1 : Form
    {
        private readonly List<string> _argList;
        private readonly XmlDocument _configDoc;
        private readonly Server _server;
        private readonly string _serverDirectory;
        private MenuStrip _menuStrip;
        private NotifyIcon _notifyIcon;
        private ToolStripMenuItem _passwordToolStripMenuItem;
        private ToolStripMenuItem _startToolStripMenuItem;
        private ToolStripMenuItem _stopToolStripMenuItem;

        public Form1(IEnumerable<string> args)
        {
            InitializeComponent();
            _serverDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var path = Path.Combine(_serverDirectory, "server-config");
            if (File.Exists(path))
            {
                _configDoc = new XmlDocument();
                _configDoc.Load(path);
            }
            else
            {
                _configDoc = Xml.CreateXmlDocument("Server");
                Xml.SetValue(_configDoc.DocumentElement, "Password", string.Empty);
            }
            try
            {
                _server = new Server();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
            _server.Password = _configDoc.SelectSingleNode("//Server/Password").InnerText;
            _server.ServerNotify += m_server_ServerNotify;
            _argList = new List<string>(args);
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            listBoxLog.BeginUpdate();
            listBoxLog.Items.Clear();
            listBoxLog.EndUpdate();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_server.IsRunning)
            {
                _server.Stop();
            }
            _server.Shutdown();
            _configDoc.Save(Path.Combine(_serverDirectory, "server-config"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_argList.Contains("start"))
            {
                StartServer();
            }
        }

        //ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
        //_notifyIcon.Icon = (Icon)manager.GetObject("_notifyIcon.Icon");


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        

        private void m_server_ServerNotify(string message)
        {
            labelServerStatus.BeginInvoke(new ServerNotifyDelegate(UpdateStatus), new object[] { message });
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode node = _configDoc.SelectSingleNode("//Server/Password");
            var dialog = new PasswordDialog(node.InnerText);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                node.InnerText = dialog.Password;
            }
        }

        private void StartServer()
        {
            _server.Start();
            _notifyIcon.Text = "Vixen Server (started)";
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StopServer()
        {
            _server.Stop();
            _notifyIcon.Text = "Vixen Server (stopped)";
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void UpdateStatus(string message)
        {
            labelServerStatus.Text = message;
            if (checkBoxLogging.Checked)
            {
                listBoxLog.Items.Insert(0, message);
            }
        }

        private delegate void ServerNotifyDelegate(string message);
    }
}

