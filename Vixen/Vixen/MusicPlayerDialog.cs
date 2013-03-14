namespace Vixen
{
    using FMOD;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    internal class MusicPlayerDialog : Form
    {
        private Button buttonAdd;
        private Button buttonCancel;
        private Button buttonDown;
        private Button buttonOK;
        private Button buttonRemove;
        private Button buttonSelectNarrative;
        private Button buttonUp;
        private CheckBox checkBoxEnableNarrative;
        private CheckBox checkBoxShuffle;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBoxNarrative;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListBox listBoxPlaylist;
        private fmod m_fmod;
        private Audio m_narrativeSong = null;
        private OpenFileDialog openFileDialog;
        private TextBox textBoxNarrative;
        private TextBox textBoxNarrativeIntervalCount;

        public MusicPlayerDialog(fmod fmod)
        {
            this.InitializeComponent();
            this.m_fmod = fmod;
        }

        private void AddSong(string fileName)
        {
            Audio item = this.LoadSong(fileName);
            if (item != null)
            {
                this.listBoxPlaylist.Items.Add(item);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            ProgressDialog dialog = null;
            this.openFileDialog.InitialDirectory = Paths.AudioPath;
            this.openFileDialog.Multiselect = true;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.openFileDialog.FileNames.Length > 2)
                {
                    dialog = new ProgressDialog();
                    dialog.Show();
                }
                this.Cursor = Cursors.WaitCursor;
                foreach (string str in this.openFileDialog.FileNames)
                {
                    try
                    {
                        if (dialog != null)
                        {
                            dialog.Message = "Adding " + Path.GetFileName(str);
                        }
                        this.AddSong(str);
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
                this.Cursor = Cursors.Default;
            }
            if (builder.Length > 0)
            {
                MessageBox.Show("There were errors when trying to add the following songs:\n\n" + builder.ToString(), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (this.listBoxPlaylist.SelectedIndex < (this.listBoxPlaylist.Items.Count - 1))
            {
                this.listBoxPlaylist.BeginUpdate();
                object selectedItem = this.listBoxPlaylist.SelectedItem;
                int selectedIndex = this.listBoxPlaylist.SelectedIndex;
                this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
                this.listBoxPlaylist.Items.Insert(++selectedIndex, selectedItem);
                this.listBoxPlaylist.EndUpdate();
                this.listBoxPlaylist.SelectedIndex = selectedIndex;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.checkBoxEnableNarrative.Checked)
            {
                try
                {
                    if (Convert.ToInt32(this.textBoxNarrativeIntervalCount.Text) >= 2)
                    {
                        if ((this.m_narrativeSong == null) || !File.Exists(Path.Combine(Paths.AudioPath, this.m_narrativeSong.FileName)))
                        {
                            if (MessageBox.Show("You enabled the narrative but didn't specify a narrative file.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                base.DialogResult = DialogResult.None;
                            }
                            else
                            {
                                this.checkBoxEnableNarrative.Checked = false;
                            }
                        }
                    }
                    else if (MessageBox.Show("An interval count less than 2 is not usable.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        base.DialogResult = DialogResult.None;
                    }
                    else
                    {
                        this.checkBoxEnableNarrative.Checked = false;
                        this.textBoxNarrativeIntervalCount.Text = "0";
                    }
                }
                catch
                {
                    if (MessageBox.Show("The interval count is not a valid number.  Do you want to continue?\n\nIf you choose Yes, the narrative will be disabled.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        base.DialogResult = DialogResult.None;
                    }
                    else
                    {
                        this.checkBoxEnableNarrative.Checked = false;
                        this.textBoxNarrativeIntervalCount.Text = "0";
                    }
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.RemoveCurrentSelection();
        }

        private void buttonSelectNarrative_Click(object sender, EventArgs e)
        {
            this.openFileDialog.InitialDirectory = Paths.AudioPath;
            this.openFileDialog.Multiselect = false;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.m_narrativeSong = this.LoadSong(this.openFileDialog.FileName);
                this.textBoxNarrative.Text = this.m_narrativeSong.Name;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (this.listBoxPlaylist.SelectedIndex > 0)
            {
                this.listBoxPlaylist.BeginUpdate();
                object selectedItem = this.listBoxPlaylist.SelectedItem;
                int selectedIndex = this.listBoxPlaylist.SelectedIndex;
                this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
                this.listBoxPlaylist.Items.Insert(--selectedIndex, selectedItem);
                this.listBoxPlaylist.EndUpdate();
                this.listBoxPlaylist.SelectedIndex = selectedIndex;
            }
        }

        private void checkBoxEnableNarrative_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBoxNarrative.Enabled = this.checkBoxEnableNarrative.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.buttonDown = new Button();
            this.buttonUp = new Button();
            this.checkBoxShuffle = new CheckBox();
            this.buttonRemove = new Button();
            this.buttonAdd = new Button();
            this.listBoxPlaylist = new ListBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.groupBoxNarrative = new GroupBox();
            this.label1 = new Label();
            this.textBoxNarrative = new TextBox();
            this.buttonSelectNarrative = new Button();
            this.label2 = new Label();
            this.textBoxNarrativeIntervalCount = new TextBox();
            this.label3 = new Label();
            this.checkBoxEnableNarrative = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxNarrative.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.checkBoxShuffle);
            this.groupBox1.Controls.Add(this.buttonRemove);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.listBoxPlaylist);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10c, 0xd5);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlist";
            this.buttonDown.Enabled = false;
            this.buttonDown.Font = new Font("Wingdings", 8f, FontStyle.Regular, GraphicsUnit.Point, 2);
            this.buttonDown.Location = new Point(0xe9, 0x30);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new Size(0x17, 0x17);
            this.buttonDown.TabIndex = 2;
            this.buttonDown.Text = "\x00ea";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new EventHandler(this.buttonDown_Click);
            this.buttonUp.Enabled = false;
            this.buttonUp.Font = new Font("Wingdings", 8f, FontStyle.Regular, GraphicsUnit.Point, 2);
            this.buttonUp.Location = new Point(0xe9, 0x13);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new Size(0x17, 0x17);
            this.buttonUp.TabIndex = 1;
            this.buttonUp.Text = "\x00e9";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new EventHandler(this.buttonUp_Click);
            this.checkBoxShuffle.AutoSize = true;
            this.checkBoxShuffle.Location = new Point(6, 0xbc);
            this.checkBoxShuffle.Name = "checkBoxShuffle";
            this.checkBoxShuffle.Size = new Size(0x69, 0x11);
            this.checkBoxShuffle.TabIndex = 5;
            this.checkBoxShuffle.Text = "Shuffle playback";
            this.checkBoxShuffle.UseVisualStyleBackColor = true;
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new Point(0x57, 0x9f);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x4b, 0x17);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.buttonAdd.Location = new Point(6, 0x9f);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new Size(0x4b, 0x17);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
            this.listBoxPlaylist.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxPlaylist.FormattingEnabled = true;
            this.listBoxPlaylist.Location = new Point(6, 0x13);
            this.listBoxPlaylist.Name = "listBoxPlaylist";
            this.listBoxPlaylist.Size = new Size(0xdd, 0x86);
            this.listBoxPlaylist.TabIndex = 0;
            this.listBoxPlaylist.SelectedIndexChanged += new EventHandler(this.listBoxPlaylist_SelectedIndexChanged);
            this.listBoxPlaylist.KeyDown += new KeyEventHandler(this.listBoxPlaylist_KeyDown);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0x1ab);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0x1ab);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.openFileDialog.Filter = "All supported formats | *.aiff;*.asf;*.flac;*.mp2;*.mp3;*.ogg;*.wav;*.wma;*.mid";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select a music file";
            this.groupBoxNarrative.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBoxNarrative.Controls.Add(this.label3);
            this.groupBoxNarrative.Controls.Add(this.textBoxNarrativeIntervalCount);
            this.groupBoxNarrative.Controls.Add(this.label2);
            this.groupBoxNarrative.Controls.Add(this.buttonSelectNarrative);
            this.groupBoxNarrative.Controls.Add(this.textBoxNarrative);
            this.groupBoxNarrative.Controls.Add(this.label1);
            this.groupBoxNarrative.Enabled = false;
            this.groupBoxNarrative.Location = new Point(12, 0x10c);
            this.groupBoxNarrative.Name = "groupBoxNarrative";
            this.groupBoxNarrative.Size = new Size(0x10c, 0x99);
            this.groupBoxNarrative.TabIndex = 2;
            this.groupBoxNarrative.TabStop = false;
            this.groupBoxNarrative.Text = "Narrative";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xeb, 0x1a);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can choose to have a narrative (or anything\r\nelse) play at specified intervals.";
            this.textBoxNarrative.Location = new Point(0x11, 0x3a);
            this.textBoxNarrative.Name = "textBoxNarrative";
            this.textBoxNarrative.Size = new Size(0xe8, 20);
            this.textBoxNarrative.TabIndex = 1;
            this.buttonSelectNarrative.Location = new Point(0x11, 0x54);
            this.buttonSelectNarrative.Name = "buttonSelectNarrative";
            this.buttonSelectNarrative.Size = new Size(0x4b, 0x17);
            this.buttonSelectNarrative.TabIndex = 2;
            this.buttonSelectNarrative.Text = "Select";
            this.buttonSelectNarrative.UseVisualStyleBackColor = true;
            this.buttonSelectNarrative.Click += new EventHandler(this.buttonSelectNarrative_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 0x7b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(100, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Play narrative every";
            this.textBoxNarrativeIntervalCount.Location = new Point(0x79, 120);
            this.textBoxNarrativeIntervalCount.MaxLength = 2;
            this.textBoxNarrativeIntervalCount.Name = "textBoxNarrativeIntervalCount";
            this.textBoxNarrativeIntervalCount.Size = new Size(0x1a, 20);
            this.textBoxNarrativeIntervalCount.TabIndex = 4;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x99, 0x7b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x26, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "songs.";
            this.checkBoxEnableNarrative.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxEnableNarrative.AutoSize = true;
            this.checkBoxEnableNarrative.Location = new Point(0x12, 0xf5);
            this.checkBoxEnableNarrative.Name = "checkBoxEnableNarrative";
            this.checkBoxEnableNarrative.Size = new Size(0x67, 0x11);
            this.checkBoxEnableNarrative.TabIndex = 1;
            this.checkBoxEnableNarrative.Text = "Enable narrative";
            this.checkBoxEnableNarrative.UseVisualStyleBackColor = true;
            this.checkBoxEnableNarrative.CheckedChanged += new EventHandler(this.checkBoxEnableNarrative_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0x1ce);
            base.Controls.Add(this.checkBoxEnableNarrative);
            base.Controls.Add(this.groupBoxNarrative);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "MusicPlayerDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Music Player";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxNarrative.ResumeLayout(false);
            this.groupBoxNarrative.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (this.listBoxPlaylist.SelectedIndex != -1))
            {
                this.RemoveCurrentSelection();
            }
        }

        private void listBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRemove.Enabled = this.buttonUp.Enabled = this.buttonDown.Enabled = this.listBoxPlaylist.SelectedIndex != -1;
        }

        private Audio LoadSong(string fileName)
        {
            string str;
            uint num;
            string sourceFileName = fileName;
            fileName = Path.GetFileName(fileName);
            string path = Path.Combine(Paths.AudioPath, fileName);
            if (!File.Exists(path))
            {
                File.Copy(sourceFileName, path);
            }
            object[] objArray = this.m_fmod.LoadSoundStats(path);
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
            Audio audio = new Audio();
            audio.FileName = fileName;
            audio.Name = str;
            audio.Duration = (int) num;
            return audio;
        }

        private void RemoveCurrentSelection()
        {
            this.listBoxPlaylist.Items.RemoveAt(this.listBoxPlaylist.SelectedIndex);
            this.buttonRemove.Enabled = this.buttonUp.Enabled = this.buttonDown.Enabled = false;
        }

        public Audio NarrativeSong
        {
            get
            {
                return this.m_narrativeSong;
            }
            set
            {
                this.m_narrativeSong = value;
                this.textBoxNarrative.Text = this.m_narrativeSong.Name;
            }
        }

        public bool NarrativeSongEnabled
        {
            get
            {
                return this.checkBoxEnableNarrative.Checked;
            }
            set
            {
                this.checkBoxEnableNarrative.Checked = value;
            }
        }

        public int NarrativeSongInterval
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.textBoxNarrativeIntervalCount.Text);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this.textBoxNarrativeIntervalCount.Text = value.ToString();
            }
        }

        public bool Shuffle
        {
            get
            {
                return this.checkBoxShuffle.Checked;
            }
            set
            {
                this.checkBoxShuffle.Checked = value;
            }
        }

        public Audio[] Songs
        {
            get
            {
                Audio[] destination = new Audio[this.listBoxPlaylist.Items.Count];
                this.listBoxPlaylist.Items.CopyTo(destination, 0);
                return destination;
            }
            set
            {
                this.listBoxPlaylist.BeginUpdate();
                this.listBoxPlaylist.Items.AddRange(value);
                this.listBoxPlaylist.EndUpdate();
            }
        }
    }
}

