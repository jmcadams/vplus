using System.Windows.Forms;

namespace Dialogs {
    internal partial class MusicPlayerDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonAdd;
        private Button buttonCancel;
        private Button buttonDown;
        private Button buttonOK;
        private Button buttonRemove;
        private Button buttonSelectNarrative;
        private Button buttonUp;
        private CheckBox checkBoxEnableNarrative;
        private CheckBox checkBoxShuffle;
        private GroupBox groupBox1;
        private GroupBox groupBoxNarrative;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListBox listBoxPlaylist;
        private OpenFileDialog openFileDialog;
        private TextBox textBoxNarrative;
        private TextBox textBoxNarrativeIntervalCount;


        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.checkBoxShuffle = new System.Windows.Forms.CheckBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxPlaylist = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxNarrative = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNarrativeIntervalCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectNarrative = new System.Windows.Forms.Button();
            this.textBoxNarrative = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxEnableNarrative = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxNarrative.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.checkBoxShuffle);
            this.groupBox1.Controls.Add(this.buttonRemove);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.listBoxPlaylist);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 213);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlist";
            // 
            // buttonDown
            // 
            this.buttonDown.Enabled = false;
            this.buttonDown.Font = new System.Drawing.Font("Wingdings", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,
                                                           ((byte) (2)));
            this.buttonDown.Location = new System.Drawing.Point(233, 48);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(23, 23);
            this.buttonDown.TabIndex = 2;
            this.buttonDown.Text = "ê";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Enabled = false;
            this.buttonUp.Font = new System.Drawing.Font("Wingdings", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,
                                                         ((byte) (2)));
            this.buttonUp.Location = new System.Drawing.Point(233, 19);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(23, 23);
            this.buttonUp.TabIndex = 1;
            this.buttonUp.Text = "é";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // checkBoxShuffle
            // 
            this.checkBoxShuffle.AutoSize = true;
            this.checkBoxShuffle.Location = new System.Drawing.Point(6, 188);
            this.checkBoxShuffle.Name = "checkBoxShuffle";
            this.checkBoxShuffle.Size = new System.Drawing.Size(105, 17);
            this.checkBoxShuffle.TabIndex = 5;
            this.checkBoxShuffle.Text = "Shuffle playback";
            this.checkBoxShuffle.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(87, 159);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(6, 159);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxPlaylist
            // 
            this.listBoxPlaylist.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPlaylist.FormattingEnabled = true;
            this.listBoxPlaylist.Location = new System.Drawing.Point(6, 19);
            this.listBoxPlaylist.Name = "listBoxPlaylist";
            this.listBoxPlaylist.Size = new System.Drawing.Size(221, 134);
            this.listBoxPlaylist.TabIndex = 0;
            this.listBoxPlaylist.SelectedIndexChanged += new System.EventHandler(this.listBoxPlaylist_SelectedIndexChanged);
            this.listBoxPlaylist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxPlaylist_KeyDown);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(124, 427);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(205, 427);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All supported formats | *.aiff;*.asf;*.flac;*.mp2;*.mp3;*.ogg;*.wav;*.wma;*.mid";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select a music file";
            // 
            // groupBoxNarrative
            // 
            this.groupBoxNarrative.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNarrative.Controls.Add(this.label3);
            this.groupBoxNarrative.Controls.Add(this.textBoxNarrativeIntervalCount);
            this.groupBoxNarrative.Controls.Add(this.label2);
            this.groupBoxNarrative.Controls.Add(this.buttonSelectNarrative);
            this.groupBoxNarrative.Controls.Add(this.textBoxNarrative);
            this.groupBoxNarrative.Controls.Add(this.label1);
            this.groupBoxNarrative.Enabled = false;
            this.groupBoxNarrative.Location = new System.Drawing.Point(12, 268);
            this.groupBoxNarrative.Name = "groupBoxNarrative";
            this.groupBoxNarrative.Size = new System.Drawing.Size(268, 153);
            this.groupBoxNarrative.TabIndex = 2;
            this.groupBoxNarrative.TabStop = false;
            this.groupBoxNarrative.Text = "Narrative";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "songs.";
            // 
            // textBoxNarrativeIntervalCount
            // 
            this.textBoxNarrativeIntervalCount.Location = new System.Drawing.Point(121, 120);
            this.textBoxNarrativeIntervalCount.MaxLength = 2;
            this.textBoxNarrativeIntervalCount.Name = "textBoxNarrativeIntervalCount";
            this.textBoxNarrativeIntervalCount.Size = new System.Drawing.Size(26, 20);
            this.textBoxNarrativeIntervalCount.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Play narrative every";
            // 
            // buttonSelectNarrative
            // 
            this.buttonSelectNarrative.Location = new System.Drawing.Point(17, 84);
            this.buttonSelectNarrative.Name = "buttonSelectNarrative";
            this.buttonSelectNarrative.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectNarrative.TabIndex = 2;
            this.buttonSelectNarrative.Text = "Select";
            this.buttonSelectNarrative.UseVisualStyleBackColor = true;
            this.buttonSelectNarrative.Click += new System.EventHandler(this.buttonSelectNarrative_Click);
            // 
            // textBoxNarrative
            // 
            this.textBoxNarrative.Location = new System.Drawing.Point(17, 58);
            this.textBoxNarrative.Name = "textBoxNarrative";
            this.textBoxNarrative.Size = new System.Drawing.Size(232, 20);
            this.textBoxNarrative.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can choose to have a narrative (or anything\r\nelse) play at specified interval" + "s.";
            // 
            // checkBoxEnableNarrative
            // 
            this.checkBoxEnableNarrative.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnableNarrative.AutoSize = true;
            this.checkBoxEnableNarrative.Location = new System.Drawing.Point(18, 245);
            this.checkBoxEnableNarrative.Name = "checkBoxEnableNarrative";
            this.checkBoxEnableNarrative.Size = new System.Drawing.Size(103, 17);
            this.checkBoxEnableNarrative.TabIndex = 1;
            this.checkBoxEnableNarrative.Text = "Enable narrative";
            this.checkBoxEnableNarrative.UseVisualStyleBackColor = true;
            this.checkBoxEnableNarrative.CheckedChanged += new System.EventHandler(this.checkBoxEnableNarrative_CheckedChanged);
            // 
            // MusicPlayerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(292, 462);
            this.Controls.Add(this.checkBoxEnableNarrative);
            this.Controls.Add(this.groupBoxNarrative);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::VixenPlus.Properties.Resources.VixenPlus;
            this.Name = "MusicPlayerDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music Player";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxNarrative.ResumeLayout(false);
            this.groupBoxNarrative.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
