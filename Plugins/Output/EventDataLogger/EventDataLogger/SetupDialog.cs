namespace EventDataLogger {
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;

	public partial class SetupDialog : Form {

		private string m_filePath;

		public SetupDialog(string logFilePath) {
			this.InitializeComponent();
			this.m_filePath = logFilePath;
			this.buttonViewLog.Enabled = this.buttonClearLog.Enabled = File.Exists(this.m_filePath);
		}

		private void buttonClearLog_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Clear log?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				File.Delete(this.m_filePath);
				this.buttonViewLog.Enabled = this.buttonClearLog.Enabled = File.Exists(this.m_filePath);
			}
		}

		private void buttonViewLog_Click(object sender, EventArgs e) {
			Process process = new Process();
			process.StartInfo.Arguments = string.Format("\"{0}\"", this.m_filePath);
			process.StartInfo.FileName = "wordpad";
			process.Start();
		}
	}
}