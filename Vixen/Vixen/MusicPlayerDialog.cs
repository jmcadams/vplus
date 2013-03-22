namespace Vixen {
	using FMOD;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;

	internal partial class MusicPlayerDialog : Form {
		private fmod m_fmod;
		private Audio m_narrativeSong = null;

		public MusicPlayerDialog(fmod fmod) {
			this.InitializeComponent();
			this.m_fmod = fmod;
		}

		private void AddSong(string fileName) {
			Audio item = this.LoadSong(fileName);
			if (item != null) {
				this.listBoxPlaylist.Items.Add(item);
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e) {
			StringBuilder builder = new StringBuilder();
			ProgressDialog dialog = null;
			this.openFileDialog.InitialDirectory = Paths.AudioPath;
			this.openFileDialog.Multiselect = true;
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				if (this.openFileDialog.FileNames.Length > 2) {
					dialog = new ProgressDialog();
					dialog.Show();
				}
				this.Cursor = Cursors.WaitCursor;
				foreach (string str in this.openFileDialog.FileNames) {
					try {
						if (dialog != null) {
							dialog.Message = "Adding " + Path.GetFileName(str);
						}
						this.AddSong(str);
					}
					catch {
						builder.AppendLine(str);
					}
				}
				if (dialog != null) {
					dialog.Hide();
					dialog.Dispose();
				}
				this.Cursor = Cursors.Default;
			}
			if (builder.Length > 0) {
				MessageBox.Show("There were errors when trying to add the following songs:\n\n" + builder.ToString(), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonDown_Click(object sender, EventArgs e) {
			if (this.listBoxPlaylist.SelectedIndex < (this.listBoxPlaylist.Items.Count - 1)) {
				this.listBoxPlaylist.BeginUpdate();
				object selectedItem = this.listBoxPlaylist.SelectedItem;
				int selectedIndex = this.listBoxPlaylist.SelectedIndex;
				this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
				this.listBoxPlaylist.Items.Insert(++selectedIndex, selectedItem);
				this.listBoxPlaylist.EndUpdate();
				this.listBoxPlaylist.SelectedIndex = selectedIndex;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.checkBoxEnableNarrative.Checked) {
				try {
					if (Convert.ToInt32(this.textBoxNarrativeIntervalCount.Text) >= 2) {
						if ((this.m_narrativeSong == null) || !File.Exists(Path.Combine(Paths.AudioPath, this.m_narrativeSong.FileName))) {
							if (MessageBox.Show("You enabled the narrative but didn't specify a narrative file.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
								base.DialogResult = System.Windows.Forms.DialogResult.None;
							}
							else {
								this.checkBoxEnableNarrative.Checked = false;
							}
						}
					}
					else if (MessageBox.Show("An interval count less than 2 is not usable.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						base.DialogResult = System.Windows.Forms.DialogResult.None;
					}
					else {
						this.checkBoxEnableNarrative.Checked = false;
						this.textBoxNarrativeIntervalCount.Text = "0";
					}
				}
				catch {
					if (MessageBox.Show("The interval count is not a valid number.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						base.DialogResult = System.Windows.Forms.DialogResult.None;
					}
					else {
						this.checkBoxEnableNarrative.Checked = false;
						this.textBoxNarrativeIntervalCount.Text = "0";
					}
				}
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e) {
			this.RemoveCurrentSelection();
		}

		private void buttonSelectNarrative_Click(object sender, EventArgs e) {
			this.openFileDialog.InitialDirectory = Paths.AudioPath;
			this.openFileDialog.Multiselect = false;
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				this.m_narrativeSong = this.LoadSong(this.openFileDialog.FileName);
				this.textBoxNarrative.Text = this.m_narrativeSong.Name;
			}
		}

		private void buttonUp_Click(object sender, EventArgs e) {
			if (this.listBoxPlaylist.SelectedIndex > 0) {
				this.listBoxPlaylist.BeginUpdate();
				object selectedItem = this.listBoxPlaylist.SelectedItem;
				int selectedIndex = this.listBoxPlaylist.SelectedIndex;
				this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
				this.listBoxPlaylist.Items.Insert(--selectedIndex, selectedItem);
				this.listBoxPlaylist.EndUpdate();
				this.listBoxPlaylist.SelectedIndex = selectedIndex;
			}
		}

		private void checkBoxEnableNarrative_CheckedChanged(object sender, EventArgs e) {
			this.groupBoxNarrative.Enabled = this.checkBoxEnableNarrative.Checked;
		}





		private void listBoxPlaylist_KeyDown(object sender, KeyEventArgs e) {
			if ((e.KeyCode == Keys.Delete) && (this.listBoxPlaylist.SelectedIndex != -1)) {
				this.RemoveCurrentSelection();
			}
		}

		private void listBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonRemove.Enabled = this.buttonUp.Enabled = this.buttonDown.Enabled = this.listBoxPlaylist.SelectedIndex != -1;
		}

		private Audio LoadSong(string fileName) {
			string str;
			uint num;
			string sourceFileName = fileName;
			fileName = Path.GetFileName(fileName);
			string path = Path.Combine(Paths.AudioPath, fileName);
			if (!File.Exists(path)) {
				File.Copy(sourceFileName, path);
			}
			object[] objArray = this.m_fmod.LoadSoundStats(path);
			if (objArray != null) {
				str = (string)objArray[0];
				num = (uint)objArray[1];
			}
			else {
				MessageBox.Show("Unable to load the song.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
			}
			Audio audio = new Audio();
			audio.FileName = fileName;
			audio.Name = str;
			audio.Duration = (int)num;
			return audio;
		}

		private void RemoveCurrentSelection() {
			this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
			this.buttonRemove.Enabled = this.buttonUp.Enabled = this.buttonDown.Enabled = false;
		}

		public Audio NarrativeSong {
			get {
				return this.m_narrativeSong;
			}
			set {
				this.m_narrativeSong = value;
				this.textBoxNarrative.Text = this.m_narrativeSong.Name;
			}
		}

		public bool NarrativeSongEnabled {
			get {
				return this.checkBoxEnableNarrative.Checked;
			}
			set {
				this.checkBoxEnableNarrative.Checked = value;
			}
		}

		public int NarrativeSongInterval {
			get {
				try {
					return Convert.ToInt32(this.textBoxNarrativeIntervalCount.Text);
				}
				catch {
					return 0;
				}
			}
			set {
				this.textBoxNarrativeIntervalCount.Text = value.ToString();
			}
		}

		public bool Shuffle {
			get {
				return this.checkBoxShuffle.Checked;
			}
			set {
				this.checkBoxShuffle.Checked = value;
			}
		}

		public Audio[] Songs {
			get {
				Audio[] destination = new Audio[this.listBoxPlaylist.Items.Count];
				this.listBoxPlaylist.Items.CopyTo(destination, 0);
				return destination;
			}
			set {
				this.listBoxPlaylist.BeginUpdate();
				this.listBoxPlaylist.Items.AddRange(value);
				this.listBoxPlaylist.EndUpdate();
			}
		}
	}
}

