namespace RemoteClient {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;

	internal partial class ClientControlDialog : Form {
		private ControlClient m_controlClient;
		private const string m_unableToExecute = "Client was unable to execute the command";

		public ClientControlDialog(ControlClient controlClient) {
			this.InitializeComponent();
			controlClient.ServerData += new ControlClient.OnServerData(this.controlClient_ServerData);
			this.m_controlClient = controlClient;
			this.GetClientList();
			this.textBoxPassword.Text = this.m_controlClient.Password;
			this.Authenticate(this.m_controlClient.Password);
		}

		private bool Authenticate(string password) {
			bool flag = false;
			this.Cursor = Cursors.WaitCursor;
			try {
				if (flag = this.m_controlClient.Authenticate(password)) {
					this.groupBoxClients.Enabled = true;
					return flag;
				}
				this.groupBoxClients.Enabled = false;
				this.groupBoxChannels.Enabled = false;
				this.groupBoxExecutionControl.Enabled = false;
			}
			finally {
				this.Cursor = Cursors.Default;
			}
			return flag;
		}

		private void buttonAllOff_Click(object sender, EventArgs e) {
			this.m_controlClient.ClientChannelOff(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID, -1);
			int topIndex = this.listBoxChannels.TopIndex;
			this.listBoxChannels.BeginUpdate();
			for (int i = 0; i < this.listBoxChannels.Items.Count; i++) {
				this.listBoxChannels.SetSelected(i, false);
			}
			this.listBoxChannels.TopIndex = topIndex;
			this.listBoxChannels.EndUpdate();
		}

		private void buttonAllOn_Click(object sender, EventArgs e) {
			this.m_controlClient.ClientChannelOn(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID, -1);
			int topIndex = this.listBoxChannels.TopIndex;
			this.listBoxChannels.BeginUpdate();
			for (int i = 0; i < this.listBoxChannels.Items.Count; i++) {
				this.listBoxChannels.SetSelected(i, true);
			}
			this.listBoxChannels.TopIndex = topIndex;
			this.listBoxChannels.EndUpdate();
		}

		private void buttonAllToggle_Click(object sender, EventArgs e) {
			this.m_controlClient.ClientChannelToggle(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID, -1);
			int topIndex = this.listBoxChannels.TopIndex;
			this.listBoxChannels.BeginUpdate();
			for (int i = 0; i < this.listBoxChannels.Items.Count; i++) {
				this.listBoxChannels.SetSelected(i, !this.listBoxChannels.GetSelected(i));
			}
			this.listBoxChannels.TopIndex = topIndex;
			this.listBoxChannels.EndUpdate();
		}

		private void buttonAuthenticate_Click(object sender, EventArgs e) {
			if (this.Authenticate(this.textBoxPassword.Text)) {
				MessageBox.Show("Authentication successful", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else {
				MessageBox.Show("Authentication failed", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonPause_Click(object sender, EventArgs e) {
			if (this.radioButtonControlAll.Checked) {
				this.m_controlClient.Pause(-1);
				this.labelServerData.Text = "Paused";
			}
			else if (this.m_controlClient.Pause(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID)) {
				this.labelServerData.Text = "Paused";
			}
			else {
				MessageBox.Show("Client was unable to execute the command", Vixen.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonPlay_Click(object sender, EventArgs e) {
			if (this.radioButtonControlAll.Checked) {
				this.m_controlClient.Execute(-1);
			}
			else if (this.m_controlClient.Execute(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID)) {
				this.labelServerData.Text = "Executing";
			}
			else {
				MessageBox.Show("Client was unable to execute the command", Vixen.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonRefresh_Click(object sender, EventArgs e) {
			this.listBoxClients.SelectedIndex = -1;
			this.listBoxClients_SelectedIndexChanged(null, null);
			this.GetClientList();
		}

		private void buttonStop_Click(object sender, EventArgs e) {
			if (this.radioButtonControlAll.Checked) {
				this.m_controlClient.Stop(-1);
				this.labelServerData.Text = "Stopped";
			}
			else if (this.m_controlClient.Stop(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID)) {
				this.labelServerData.Text = "Stopped";
			}
			else {
				MessageBox.Show("Client was unable to execute the command", Vixen.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void ClientControlDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.m_controlClient.ServerData -= new ControlClient.OnServerData(this.controlClient_ServerData);
		}

		private void controlClient_ServerData(string dataString) {
			this.labelServerData.Text = dataString;
			this.labelServerData.Refresh();
		}



		private void GetClientList() {
			this.Cursor = Cursors.WaitCursor;
			try {
				this.listBoxClients.BeginUpdate();
				this.listBoxClients.Items.Clear();
				this.listBoxClients.Items.AddRange(this.m_controlClient.RequestClientEnumeration());
				if (this.listBoxClients.Items.Count == 0) {
					this.listBoxClients.Items.Add("No clients registered");
				}
				this.listBoxClients.EndUpdate();
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}



		private void listBoxChannels_MouseDown(object sender, MouseEventArgs e) {
			int index = this.listBoxChannels.IndexFromPoint(e.X, e.Y);
			if (index != -1) {
				if (this.listBoxChannels.GetSelected(index)) {
					if (!this.m_controlClient.ClientChannelOn(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID, index)) {
						MessageBox.Show("Client was unable to execute the command", Vixen.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						this.listBoxChannels.SetSelected(index, false);
					}
				}
				else if (!this.m_controlClient.ClientChannelOff(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID, index)) {
					MessageBox.Show("Client was unable to execute the command", Vixen.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this.listBoxChannels.SetSelected(index, true);
				}
			}
		}

		private void listBoxClients_DoubleClick(object sender, EventArgs e) {
			if ((this.listBoxClients.SelectedIndex != -1) && (this.listBoxClients.SelectedItem is VixenExecutionClientStub)) {
				this.Cursor = Cursors.WaitCursor;
				VixenExecutionClientStub selectedItem = (VixenExecutionClientStub)this.listBoxClients.SelectedItem;
				ClientStatus status = this.m_controlClient.ClientStatus(selectedItem.ID);
				this.Cursor = Cursors.Default;
				if (status.ExecutionStatus == ExecutionStatus.None) {
					MessageBox.Show("Idle", selectedItem.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else {
					StringBuilder builder = new StringBuilder();
					builder.AppendLine("Status: " + status.ExecutionStatus.ToString());
					builder.AppendLine();
					string str = (status.ProgramName.Length == 0) ? "(None)" : status.ProgramName;
					builder.AppendLine(string.Format("Program: {0} [{1:d2}:{2:d2}:{3:d2}]", new object[] { str, status.ProgramLength / 0xe10, (status.ProgramLength / 60) % 60, status.ProgramLength % 60 }));
					builder.AppendLine(string.Format("Sequence: {0} [{1:d2}:{2:d2}:{3:d2}]", new object[] { status.SequenceName, status.SequenceLength / 0xe10, (status.SequenceLength / 60) % 60, status.SequenceLength % 60 }));
					builder.AppendLine(string.Format("Sequence execution progress: {0:d2}:{1:d2}:{2:d2}", status.SequenceProgress / 0xe10, (status.SequenceProgress / 60) % 60, status.SequenceProgress % 60));
					MessageBox.Show(builder.ToString(), selectedItem.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void listBoxClients_SelectedIndexChanged(object sender, EventArgs e) {
			bool flag = (this.listBoxClients.SelectedIndex != -1) && !(this.listBoxClients.Items[0] is string);
			if (flag) {
				this.Cursor = Cursors.WaitCursor;
				try {
					if (!this.m_controlClient.ClientEcho(((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ID)) {
						MessageBox.Show("You cannot control a client that is a part of the same process.\nTry starting another copy of Vixen or using a web client.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						this.listBoxClients.SelectedIndex = -1;
						return;
					}
				}
				finally {
					this.Cursor = Cursors.Default;
				}
			}
			this.groupBoxExecutionControl.Enabled = flag || this.radioButtonControlAll.Checked;
			this.groupBoxChannels.Enabled = flag;
			if (flag) {
				this.listBoxChannels.BeginUpdate();
				this.listBoxChannels.Items.Clear();
				for (int i = 1; i <= ((VixenExecutionClientStub)this.listBoxClients.SelectedItem).ChannelCount; i++) {
					this.listBoxChannels.Items.Add(i.ToString());
				}
				this.listBoxChannels.EndUpdate();
			}
		}

		private void radioButtonControlAll_CheckedChanged(object sender, EventArgs e) {
			bool flag = (this.listBoxClients.SelectedIndex != -1) && !(this.listBoxClients.Items[0] is string);
			this.groupBoxExecutionControl.Enabled = flag || this.radioButtonControlAll.Checked;
		}

		private void textBoxPassword_Enter(object sender, EventArgs e) {
			base.AcceptButton = this.buttonAuthenticate;
		}

		private void textBoxPassword_Leave(object sender, EventArgs e) {
			base.AcceptButton = null;
		}
	}
}