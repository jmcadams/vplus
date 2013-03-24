namespace RemoteClient {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus;

	public partial class ExecutionClientUI : Form {
		private ClientContext m_clientContext;
		private ExecutionClient m_executionClient;
		private Preference2 m_preferences;

		internal ExecutionClientUI(ExecutionClient executionClient, ClientContext clientContext) {
			object obj2;
			this.components = null;
			this.InitializeComponent();
			this.m_executionClient = executionClient;
			this.m_clientContext = clientContext;
			if (this.m_clientContext.ContextObject != null) {
				this.labelCurrentlyLoaded.Text = this.m_clientContext.ContextObject.Name;
			}
			if (Interfaces.Available.TryGetValue("ISystem", out obj2)) {
				this.m_preferences = ((ISystem)obj2).UserPreferences;
			}
		}

		private void buttonRefreshProgramList_Click(object sender, EventArgs e) {
			this.listBoxPrograms.BeginUpdate();
			try {
				this.listBoxPrograms.Items.Clear();
				this.listBoxPrograms.Items.AddRange(this.m_executionClient.RequestRemoteProgramList());
			}
			finally {
				this.listBoxPrograms.EndUpdate();
			}
		}

		private void buttonRefreshSequenceList_Click(object sender, EventArgs e) {
			this.listBoxSequences.BeginUpdate();
			try {
				this.listBoxSequences.Items.Clear();
				this.listBoxSequences.Items.AddRange(this.m_executionClient.RequestRemoteSequenceList());
			}
			finally {
				this.listBoxSequences.EndUpdate();
			}
		}

		private void buttonRetrieveProgram_Click(object sender, EventArgs e) {
			this.Cursor = Cursors.WaitCursor;
			try {
				SequenceProgram program = this.m_executionClient.RetrieveRemoteProgram((string)this.listBoxPrograms.SelectedItem);
				this.m_clientContext.ContextObject = program;
				this.labelCurrentlyLoaded.Text = program.Name;
				MessageBox.Show("Download complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void buttonRetrieveSequence_Click(object sender, EventArgs e) {
			this.Cursor = Cursors.WaitCursor;
			try {
				EventSequence sequence = this.m_executionClient.RetrieveRemoteSequence((string)this.listBoxSequences.SelectedItem);
				this.m_clientContext.ContextObject = sequence;
				this.labelCurrentlyLoaded.Text = sequence.Name;
				MessageBox.Show("Download complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void checkBoxStartClient_CheckedChanged(object sender, EventArgs e) {
		}

		private void listBoxPrograms_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonRetrieveProgram.Enabled = this.listBoxPrograms.SelectedIndex != -1;
		}

		private void listBoxSequences_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonRetrieveSequence.Enabled = this.listBoxSequences.SelectedIndex != -1;
		}
	}
}