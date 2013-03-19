namespace RemoteClient {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	internal partial class ClientUI : Form {
		private ControlClient m_controlClient;
		private XmlNode m_dataNode;
		private ExecutionClient m_executionClient;
		private bool m_internalUpdate = false;
		private LocalClient m_localClient;
		private const string m_startFailure = "Client failed to start.";
		private const string m_startSuccess = "Client started successfully.";

		public ClientUI(ControlClient controlClient, ExecutionClient executionClient, LocalClient localClient, XmlNode dataNode) {
			this.InitializeComponent();
			this.m_controlClient = controlClient;
			this.m_executionClient = executionClient;
			this.m_localClient = localClient;
			this.m_dataNode = dataNode;
			Xml.GetNodeAlways(this.m_dataNode, "Execution");
			Xml.GetNodeAlways(this.m_dataNode, "Control");
		}

		private void buttonControlClientControlPanel_Click(object sender, EventArgs e) {
			this.m_controlClient.ShowControlPanel();
		}

		private void buttonExecutionClientControlPanel_Click(object sender, EventArgs e) {
			this.m_executionClient.ShowControlPanel();
		}

		private void buttonRefresh_Click(object sender, EventArgs e) {
			StringBuilder builder = new StringBuilder();
			if (this.AllowLocal && this.m_localClient.Running) {
				this.Cursor = Cursors.WaitCursor;
				this.m_localClient.Stop();
				if (this.m_localClient.Start()) {
					builder.AppendLine("Local: Successful restart");
				}
				else {
					builder.AppendLine("Local: Failed restart");
				}
				this.Cursor = Cursors.Default;
			}
			if (this.AllowRemote && this.m_executionClient.Running) {
				this.m_executionClient.Stop();
				this.Cursor = Cursors.WaitCursor;
				if (this.m_executionClient.Start()) {
					builder.AppendLine("Remote: Successful restart");
				}
				else {
					builder.AppendLine("Remote: Failed restart");
				}
				this.Cursor = Cursors.Default;
			}
			if (builder.Length > 0) {
				MessageBox.Show(builder.ToString(), "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void buttonTest_Click(object sender, EventArgs e) {
			Xml.GetNodeAlways(this.m_dataNode, "Server").InnerText = this.textBoxServer.Text;
			if (this.ValidServer(this.textBoxServer.Text)) {
				this.EnableRemoteClientUI();
				this.m_controlClient.Server = this.textBoxServer.Text;
				this.m_executionClient.Server = this.textBoxServer.Text;
				MessageBox.Show("Found the server!", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else {
				this.DisableRemoteClientUI();
				MessageBox.Show("Server did not respond appropriately.\nThe server may have something other than Vixen running on this port, or there may not be a server at this address or by this name.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void checkBoxLocal_CheckedChanged(object sender, EventArgs e) {
			if (this.checkBoxLocal.Checked) {
				this.Cursor = Cursors.WaitCursor;
				if (this.m_localClient.Start()) {
					if (!this.m_internalUpdate) {
						MessageBox.Show("Local client: Client started successfully.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				else {
					bool internalUpdate = this.m_internalUpdate;
					this.m_internalUpdate = true;
					this.checkBoxLocal.Checked = false;
					this.m_internalUpdate = internalUpdate;
					MessageBox.Show("Local client: Client failed to start.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				this.Cursor = Cursors.Default;
			}
			else {
				this.m_localClient.Stop();
			}
			this.buttonRefresh.Enabled = this.checkBoxLocal.Checked | this.checkBoxRemote.Checked;
		}

		private void checkBoxRemote_CheckedChanged(object sender, EventArgs e) {
			if (this.checkBoxRemote.Checked) {
				this.Cursor = Cursors.WaitCursor;
				if (this.m_executionClient.Start()) {
					if (!this.m_internalUpdate) {
						MessageBox.Show("Remote client: Client started successfully.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				else {
					bool internalUpdate = this.m_internalUpdate;
					this.m_internalUpdate = true;
					this.checkBoxRemote.Checked = false;
					this.m_internalUpdate = internalUpdate;
					MessageBox.Show("Remote client: Client failed to start.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				this.Cursor = Cursors.Default;
			}
			else {
				this.m_executionClient.Stop();
			}
			this.groupBoxExecutionClient.Enabled = this.checkBoxRemote.Checked;
			this.buttonRefresh.Enabled = this.checkBoxLocal.Checked | this.checkBoxRemote.Checked;
		}

		private void ClientUI_Load(object sender, EventArgs e) {
			this.m_internalUpdate = true;
			this.textBoxServer.Text = this.m_controlClient.Server;
			if (this.ValidServer(this.textBoxServer.Text)) {
				this.EnableRemoteClientUI();
				this.checkBoxRemote.Checked = (this.m_executionClient != null) && this.m_executionClient.Running;
			}
			else {
				this.DisableRemoteClientUI();
			}
			this.checkBoxLocal.Checked = this.m_localClient.Running;
			this.m_internalUpdate = false;
		}

		private void DisableRemoteClientUI() {
			this.groupBoxControlClient.Enabled = false;
			this.groupBoxExecutionClient.Enabled = false;
			this.checkBoxRemote.Checked = false;
			this.checkBoxRemote.Enabled = false;
		}



		private void EnableRemoteClientUI() {
			this.groupBoxControlClient.Enabled = true;
			this.checkBoxRemote.Enabled = true;
			this.groupBoxExecutionClient.Enabled = this.checkBoxRemote.Checked;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(ClientUI));
		//this.toolTip.SetToolTip(this.label2, manager.GetString("label2.ToolTip"));



		private void toolTip_Popup(object sender, PopupEventArgs e) {
			if (e.AssociatedControl == this.label2) {
				this.toolTip.ToolTipTitle = "Execution Client";
			}
			else if (e.AssociatedControl == this.label4) {
				this.toolTip.ToolTipTitle = "Control Client";
			}
		}

		private bool ValidServer(string serverNameAddress) {
			IPAddress address;
			if (serverNameAddress.Length == 0) {
				return false;
			}
			this.Cursor = Cursors.WaitCursor;
			if (char.IsDigit(serverNameAddress[0])) {
				int num;
				string ipString = ((num = serverNameAddress.IndexOf(':')) > -1) ? serverNameAddress.Substring(0, num) : serverNameAddress;
				address = IPAddress.Parse(ipString);
			}
			else {
				address = Sockets.GetIPV4AddressFor(serverNameAddress);
			}
			try {
				TcpClient client = Sockets.ConnectTo(address, 0xa1b9);
				client.Client.Send(new byte[] { 0x12 });
				byte[] buffer = new byte[1];
				client.Client.Receive(buffer, 1, SocketFlags.None);
				client.Close();
				this.Cursor = Cursors.Default;
				return (buffer[0] == 0x12);
			}
			catch (Exception exception) {
				this.Cursor = Cursors.Default;
				ErrorLog.Log(exception.Message);
				return false;
			}
		}

		public bool AllowLocal {
			get {
				return this.checkBoxLocal.Checked;
			}
		}

		public bool AllowRemote {
			get {
				return this.checkBoxRemote.Checked;
			}
		}
	}
}