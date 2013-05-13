using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FMOD;

namespace VixenPlus
{
    internal partial class MusicPlayerDialog : Form
    {
        private readonly fmod _fmod;
        private Audio _narrativeSong;

        public MusicPlayerDialog(fmod fmod)
        {
            InitializeComponent();
            _fmod = fmod;
        }

        public Audio NarrativeSong
        {
            get { return _narrativeSong; }
            set
            {
                _narrativeSong = value;
                textBoxNarrative.Text = _narrativeSong.Name;
            }
        }

        public bool NarrativeSongEnabled
        {
            get { return checkBoxEnableNarrative.Checked; }
            set { checkBoxEnableNarrative.Checked = value; }
        }

        public int NarrativeSongInterval
        {
            get
            {
                try
                {
                    return Convert.ToInt32(textBoxNarrativeIntervalCount.Text);
                }
                catch
                {
                    return 0;
                }
            }
            set { textBoxNarrativeIntervalCount.Text = value.ToString(CultureInfo.InvariantCulture); }
        }

        public bool Shuffle
        {
            get { return checkBoxShuffle.Checked; }
            set { checkBoxShuffle.Checked = value; }
        }

        public Audio[] Songs
        {
            get
            {
                var destination = new Audio[listBoxPlaylist.Items.Count];
                listBoxPlaylist.Items.CopyTo(destination, 0);
                return destination;
            }
            set
            {
                listBoxPlaylist.BeginUpdate();
                listBoxPlaylist.Items.AddRange(value);
                listBoxPlaylist.EndUpdate();
            }
        }

        private void AddSong(string fileName)
        {
            Audio item = LoadSong(fileName);
            if (item != null)
            {
                listBoxPlaylist.Items.Add(item);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            ProgressDialog dialog = null;
            openFileDialog.InitialDirectory = Paths.AudioPath;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length > 2)
                {
                    dialog = new ProgressDialog();
                    dialog.Show();
                }
                Cursor = Cursors.WaitCursor;
                foreach (string str in openFileDialog.FileNames)
                {
                    try
                    {
                        if (dialog != null)
                        {
                            dialog.Message = "Adding " + Path.GetFileName(str);
                        }
                        AddSong(str);
                    }
                    catch
                    {
                        builder.AppendLine(str);
                    }
                }
                if (dialog != null)
                {
                    dialog.Hide();
                    dialog.Dispose();
                }
                Cursor = Cursors.Default;
            }
            if (builder.Length > 0)
            {
                MessageBox.Show("There were errors when trying to add the following songs:\n\n" + builder, Vendor.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex < (listBoxPlaylist.Items.Count - 1))
            {
                listBoxPlaylist.BeginUpdate();
                object selectedItem = listBoxPlaylist.SelectedItem;
                int selectedIndex = listBoxPlaylist.SelectedIndex;
                listBoxPlaylist.Items.RemoveAt(listBoxPlaylist.SelectedIndex);
                listBoxPlaylist.Items.Insert(++selectedIndex, selectedItem);
                listBoxPlaylist.EndUpdate();
                listBoxPlaylist.SelectedIndex = selectedIndex;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkBoxEnableNarrative.Checked)
            {
                try
                {
                    if (Convert.ToInt32(textBoxNarrativeIntervalCount.Text) >= 2)
                    {
                        if ((_narrativeSong == null) || !File.Exists(Path.Combine(Paths.AudioPath, _narrativeSong.FileName)))
                        {
                            if (
                                MessageBox.Show(
                                    "You enabled the narrative but didn't specify a narrative file.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.",
                                    Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                DialogResult = DialogResult.None;
                            }
                            else
                            {
                                checkBoxEnableNarrative.Checked = false;
                            }
                        }
                    }
                    else if (
                        MessageBox.Show(
                            "An interval count less than 2 is not usable.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.",
                            Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        DialogResult = DialogResult.None;
                    }
                    else
                    {
                        checkBoxEnableNarrative.Checked = false;
                        textBoxNarrativeIntervalCount.Text = @"0";
                    }
                }
                catch
                {
                    if (
                        MessageBox.Show(
                            "The interval count is not a valid number.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.",
                            Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        DialogResult = DialogResult.None;
                    }
                    else
                    {
                        checkBoxEnableNarrative.Checked = false;
                        textBoxNarrativeIntervalCount.Text = @"0";
                    }
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveCurrentSelection();
        }

        private void buttonSelectNarrative_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Paths.AudioPath;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _narrativeSong = LoadSong(openFileDialog.FileName);
                textBoxNarrative.Text = _narrativeSong.Name;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex > 0)
            {
                listBoxPlaylist.BeginUpdate();
                object selectedItem = listBoxPlaylist.SelectedItem;
                int selectedIndex = listBoxPlaylist.SelectedIndex;
                listBoxPlaylist.Items.RemoveAt(listBoxPlaylist.SelectedIndex);
                listBoxPlaylist.Items.Insert(--selectedIndex, selectedItem);
                listBoxPlaylist.EndUpdate();
                listBoxPlaylist.SelectedIndex = selectedIndex;
            }
        }

        private void checkBoxEnableNarrative_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxNarrative.Enabled = checkBoxEnableNarrative.Checked;
        }


        private void listBoxPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (listBoxPlaylist.SelectedIndex != -1))
            {
                RemoveCurrentSelection();
            }
        }

        private void listBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = buttonUp.Enabled = buttonDown.Enabled = listBoxPlaylist.SelectedIndex != -1;
        }

        private Audio LoadSong(string fileName)
        {
            string str = string.Empty;
            uint num = 0;
            string sourceFileName = fileName;
            fileName = Path.GetFileName(fileName);
            if (fileName != null)
            {
                string path = Path.Combine(Paths.AudioPath, fileName);
                if (!File.Exists(path))
                {
                    File.Copy(sourceFileName, path);
                }
                object[] objArray = _fmod.LoadSoundStats(path);
                if (objArray != null)
                {
                    str = (string) objArray[0];
                    num = (uint) objArray[1];
                }
                else
                {
                    MessageBox.Show("Unable to load the song.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
            }
            var audio = new Audio {FileName = fileName, Name = str, Duration = (int) num};
            return audio;
        }

        private void RemoveCurrentSelection()
        {
            listBoxPlaylist.Items.RemoveAt(listBoxPlaylist.SelectedIndex);
            buttonRemove.Enabled = buttonUp.Enabled = buttonDown.Enabled = false;
        }
    }
}