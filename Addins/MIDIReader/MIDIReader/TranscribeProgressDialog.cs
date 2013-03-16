namespace MIDIReader {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class TranscribeProgressDialog : Form {
		public TranscribeProgressDialog(int maxTracks, int maxEvents) {
			this.InitializeComponent();
			this.progressBarTrack.Maximum = maxTracks;
		}

		public void StepTrack() {
			this.progressBarTrack.Value++;
			this.Refresh();
		}
	}
}