namespace RemoteClient {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using VixenPlus;

	internal partial class ControlClientUI : Form {
		private ControlClient m_controlClient;

		public ControlClientUI(ControlClient controlClient) {
			this.InitializeComponent();
			this.m_controlClient = controlClient;
			this.PopulateListBoxWith(this.listBoxSequences, Directory.GetFiles(Paths.SequencePath));
			this.PopulateListBoxWith(this.listBoxPrograms, Directory.GetFiles(Paths.ProgramPath));
		}

		private void buttonClientControl_Click(object sender, EventArgs e) {
			ClientControlDialog dialog = new ClientControlDialog(this.m_controlClient);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonUploadPrograms_Click(object sender, EventArgs e) {
			this.Cursor = Cursors.WaitCursor;
			try {
				foreach (string str in this.listBoxPrograms.SelectedItems) {
					this.m_controlClient.SendLoadProgram(Path.Combine(Paths.ProgramPath, str), Paths.SequencePath);
				}
				MessageBox.Show("Upload complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void buttonUploadSequences_Click(object sender, EventArgs e) {
			this.Cursor = Cursors.WaitCursor;
			try {
				foreach (string str in this.listBoxSequences.SelectedItems) {
					this.m_controlClient.SendLoadSequence(Path.Combine(Paths.SequencePath, str));
				}
				MessageBox.Show("Upload complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void listBoxPrograms_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonUploadPrograms.Enabled = this.listBoxPrograms.SelectedIndex != -1;
		}

		private void listBoxSequences_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonUploadSequences.Enabled = this.listBoxSequences.SelectedIndex != -1;
		}

		private void PopulateListBoxWith(ListBox listBox, string[] fileNames) {
			listBox.BeginUpdate();
			listBox.Items.Clear();
			foreach (string str in fileNames) {
				listBox.Items.Add(Path.GetFileName(str));
			}
			listBox.EndUpdate();
		}
	}
}