namespace RemoteClient
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;

    internal class ControlClientUI : Form
    {
        private Button buttonClientControl;
        private Button buttonDone;
        private Button buttonUploadPrograms;
        private Button buttonUploadSequences;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListBox listBoxPrograms;
        private ListBox listBoxSequences;
        private ControlClient m_controlClient;

        public ControlClientUI(ControlClient controlClient)
        {
            this.InitializeComponent();
            this.m_controlClient = controlClient;
            this.PopulateListBoxWith(this.listBoxSequences, Directory.GetFiles(Paths.SequencePath));
            this.PopulateListBoxWith(this.listBoxPrograms, Directory.GetFiles(Paths.ProgramPath));
        }

        private void buttonClientControl_Click(object sender, EventArgs e)
        {
            ClientControlDialog dialog = new ClientControlDialog(this.m_controlClient);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonUploadPrograms_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string str in this.listBoxPrograms.SelectedItems)
                {
                    this.m_controlClient.SendLoadProgram(Path.Combine(Paths.ProgramPath, str), Paths.SequencePath);
                }
                MessageBox.Show("Upload complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonUploadSequences_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string str in this.listBoxSequences.SelectedItems)
                {
                    this.m_controlClient.SendLoadSequence(Path.Combine(Paths.SequencePath, str));
                }
                MessageBox.Show("Upload complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
            this.label2 = new Label();
            this.buttonClientControl = new Button();
            this.groupBox2 = new GroupBox();
            this.label1 = new Label();
            this.buttonUploadSequences = new Button();
            this.listBoxSequences = new ListBox();
            this.groupBox3 = new GroupBox();
            this.label3 = new Label();
            this.buttonUploadPrograms = new Button();
            this.listBoxPrograms = new ListBox();
            this.buttonDone = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonClientControl);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x178, 0x47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client control";
            this.label2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label2.Location = new Point(0x80, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xf2, 0x22);
            this.label2.TabIndex = 5;
            this.label2.Text = "Test individual channels of a client, test execution of a specific client, or control execution of all clients.";
            this.buttonClientControl.Location = new Point(15, 0x18);
            this.buttonClientControl.Name = "buttonClientControl";
            this.buttonClientControl.Size = new Size(0x63, 0x17);
            this.buttonClientControl.TabIndex = 4;
            this.buttonClientControl.Text = "Client Control";
            this.buttonClientControl.UseVisualStyleBackColor = true;
            this.buttonClientControl.Click += new EventHandler(this.buttonClientControl_Click);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonUploadSequences);
            this.groupBox2.Controls.Add(this.listBoxSequences);
            this.groupBox2.Location = new Point(12, 0x59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x178, 0xb7);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Local sequences";
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(10, 0x17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x129, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Upload sequences to the server for other clients to download.";
            this.buttonUploadSequences.Enabled = false;
            this.buttonUploadSequences.Location = new Point(6, 0x98);
            this.buttonUploadSequences.Name = "buttonUploadSequences";
            this.buttonUploadSequences.Size = new Size(0x4b, 0x17);
            this.buttonUploadSequences.TabIndex = 1;
            this.buttonUploadSequences.Text = "Upload";
            this.buttonUploadSequences.UseVisualStyleBackColor = true;
            this.buttonUploadSequences.Click += new EventHandler(this.buttonUploadSequences_Click);
            this.listBoxSequences.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxSequences.FormattingEnabled = true;
            this.listBoxSequences.Location = new Point(6, 0x33);
            this.listBoxSequences.Name = "listBoxSequences";
            this.listBoxSequences.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxSequences.Size = new Size(0x16c, 0x5f);
            this.listBoxSequences.TabIndex = 0;
            this.listBoxSequences.SelectedIndexChanged += new EventHandler(this.listBoxSequences_SelectedIndexChanged);
            this.groupBox3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.buttonUploadPrograms);
            this.groupBox3.Controls.Add(this.listBoxPrograms);
            this.groupBox3.Location = new Point(12, 0x116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x178, 0xb7);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Local programs";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(10, 0x17);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x120, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Upload programs to the server for other clients to download.";
            this.buttonUploadPrograms.Enabled = false;
            this.buttonUploadPrograms.Location = new Point(6, 0x98);
            this.buttonUploadPrograms.Name = "buttonUploadPrograms";
            this.buttonUploadPrograms.Size = new Size(0x4b, 0x17);
            this.buttonUploadPrograms.TabIndex = 3;
            this.buttonUploadPrograms.Text = "Upload";
            this.buttonUploadPrograms.UseVisualStyleBackColor = true;
            this.buttonUploadPrograms.Click += new EventHandler(this.buttonUploadPrograms_Click);
            this.listBoxPrograms.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxPrograms.FormattingEnabled = true;
            this.listBoxPrograms.Location = new Point(6, 0x33);
            this.listBoxPrograms.Name = "listBoxPrograms";
            this.listBoxPrograms.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxPrograms.Size = new Size(0x16c, 0x5f);
            this.listBoxPrograms.TabIndex = 2;
            this.listBoxPrograms.SelectedIndexChanged += new EventHandler(this.listBoxPrograms_SelectedIndexChanged);
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0x139, 470);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonDone;
            base.ClientSize = new Size(400, 0x1f9);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "ControlClientUI";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Control Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            base.ResumeLayout(false);
        }

        private void listBoxPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonUploadPrograms.Enabled = this.listBoxPrograms.SelectedIndex != -1;
        }

        private void listBoxSequences_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonUploadSequences.Enabled = this.listBoxSequences.SelectedIndex != -1;
        }

        private void PopulateListBoxWith(ListBox listBox, string[] fileNames)
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (string str in fileNames)
            {
                listBox.Items.Add(Path.GetFileName(str));
            }
            listBox.EndUpdate();
        }
    }
}

