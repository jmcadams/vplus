namespace RemoteClient
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class ExecutionClientUI : Form
    {
        private Button buttonDone;
        private Button buttonRefreshProgramList;
        private Button buttonRefreshSequenceList;
        private Button buttonRetrieveProgram;
        private Button buttonRetrieveSequence;
        private CheckBox checkBoxStartClient;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label labelCurrentlyLoaded;
        private ListBox listBoxPrograms;
        private ListBox listBoxSequences;
        private ClientContext m_clientContext;
        private ExecutionClient m_executionClient;
        private Preference2 m_preferences;

        internal ExecutionClientUI(ExecutionClient executionClient, ClientContext clientContext)
        {
            object obj2;
            this.components = null;
            this.InitializeComponent();
            this.m_executionClient = executionClient;
            this.m_clientContext = clientContext;
            if (this.m_clientContext.ContextObject != null)
            {
                this.labelCurrentlyLoaded.Text = this.m_clientContext.ContextObject.Name;
            }
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                this.m_preferences = ((ISystem) obj2).UserPreferences;
            }
        }

        private void buttonRefreshProgramList_Click(object sender, EventArgs e)
        {
            this.listBoxPrograms.BeginUpdate();
            try
            {
                this.listBoxPrograms.Items.Clear();
                this.listBoxPrograms.Items.AddRange(this.m_executionClient.RequestRemoteProgramList());
            }
            finally
            {
                this.listBoxPrograms.EndUpdate();
            }
        }

        private void buttonRefreshSequenceList_Click(object sender, EventArgs e)
        {
            this.listBoxSequences.BeginUpdate();
            try
            {
                this.listBoxSequences.Items.Clear();
                this.listBoxSequences.Items.AddRange(this.m_executionClient.RequestRemoteSequenceList());
            }
            finally
            {
                this.listBoxSequences.EndUpdate();
            }
        }

        private void buttonRetrieveProgram_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                SequenceProgram program = this.m_executionClient.RetrieveRemoteProgram((string) this.listBoxPrograms.SelectedItem);
                this.m_clientContext.ContextObject = program;
                this.labelCurrentlyLoaded.Text = program.Name;
                MessageBox.Show("Download complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonRetrieveSequence_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                EventSequence sequence = this.m_executionClient.RetrieveRemoteSequence((string) this.listBoxSequences.SelectedItem);
                this.m_clientContext.ContextObject = sequence;
                this.labelCurrentlyLoaded.Text = sequence.Name;
                MessageBox.Show("Download complete", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void checkBoxStartClient_CheckedChanged(object sender, EventArgs e)
        {
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
            this.buttonRetrieveSequence = new Button();
            this.listBoxSequences = new ListBox();
            this.buttonRefreshSequenceList = new Button();
            this.groupBox2 = new GroupBox();
            this.buttonRetrieveProgram = new Button();
            this.listBoxPrograms = new ListBox();
            this.buttonRefreshProgramList = new Button();
            this.buttonDone = new Button();
            this.label1 = new Label();
            this.labelCurrentlyLoaded = new Label();
            this.checkBoxStartClient = new CheckBox();
            this.groupBox3 = new GroupBox();
            this.label2 = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonRetrieveSequence);
            this.groupBox1.Controls.Add(this.listBoxSequences);
            this.groupBox1.Controls.Add(this.buttonRefreshSequenceList);
            this.groupBox1.Location = new Point(12, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(410, 0xbd);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sequences Available";
            this.buttonRetrieveSequence.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonRetrieveSequence.Enabled = false;
            this.buttonRetrieveSequence.Location = new Point(6, 160);
            this.buttonRetrieveSequence.Name = "buttonRetrieveSequence";
            this.buttonRetrieveSequence.Size = new Size(0x4b, 0x17);
            this.buttonRetrieveSequence.TabIndex = 2;
            this.buttonRetrieveSequence.Text = "Download";
            this.buttonRetrieveSequence.UseVisualStyleBackColor = true;
            this.buttonRetrieveSequence.Click += new EventHandler(this.buttonRetrieveSequence_Click);
            this.listBoxSequences.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxSequences.FormattingEnabled = true;
            this.listBoxSequences.Location = new Point(6, 0x30);
            this.listBoxSequences.Name = "listBoxSequences";
            this.listBoxSequences.Size = new Size(0x18e, 0x6c);
            this.listBoxSequences.TabIndex = 1;
            this.listBoxSequences.SelectedIndexChanged += new EventHandler(this.listBoxSequences_SelectedIndexChanged);
            this.buttonRefreshSequenceList.Location = new Point(6, 0x13);
            this.buttonRefreshSequenceList.Name = "buttonRefreshSequenceList";
            this.buttonRefreshSequenceList.Size = new Size(0x4b, 0x17);
            this.buttonRefreshSequenceList.TabIndex = 0;
            this.buttonRefreshSequenceList.Text = "Refresh";
            this.buttonRefreshSequenceList.UseVisualStyleBackColor = true;
            this.buttonRefreshSequenceList.Click += new EventHandler(this.buttonRefreshSequenceList_Click);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.buttonRetrieveProgram);
            this.groupBox2.Controls.Add(this.listBoxPrograms);
            this.groupBox2.Controls.Add(this.buttonRefreshProgramList);
            this.groupBox2.Location = new Point(13, 0xdb);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x199, 0xb1);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Programs Available";
            this.buttonRetrieveProgram.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonRetrieveProgram.Enabled = false;
            this.buttonRetrieveProgram.Location = new Point(6, 0x93);
            this.buttonRetrieveProgram.Name = "buttonRetrieveProgram";
            this.buttonRetrieveProgram.Size = new Size(0x4b, 0x17);
            this.buttonRetrieveProgram.TabIndex = 5;
            this.buttonRetrieveProgram.Text = "Download";
            this.buttonRetrieveProgram.UseVisualStyleBackColor = true;
            this.buttonRetrieveProgram.Click += new EventHandler(this.buttonRetrieveProgram_Click);
            this.listBoxPrograms.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxPrograms.FormattingEnabled = true;
            this.listBoxPrograms.Location = new Point(6, 0x30);
            this.listBoxPrograms.Name = "listBoxPrograms";
            this.listBoxPrograms.Size = new Size(0x18d, 0x5f);
            this.listBoxPrograms.TabIndex = 4;
            this.listBoxPrograms.SelectedIndexChanged += new EventHandler(this.listBoxPrograms_SelectedIndexChanged);
            this.buttonRefreshProgramList.Location = new Point(6, 0x13);
            this.buttonRefreshProgramList.Name = "buttonRefreshProgramList";
            this.buttonRefreshProgramList.Size = new Size(0x4b, 0x17);
            this.buttonRefreshProgramList.TabIndex = 3;
            this.buttonRefreshProgramList.Text = "Refresh";
            this.buttonRefreshProgramList.UseVisualStyleBackColor = true;
            this.buttonRefreshProgramList.Click += new EventHandler(this.buttonRefreshProgramList_Click);
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0x15b, 0x1bf);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 5;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 420);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Currently loaded:";
            this.labelCurrentlyLoaded.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.labelCurrentlyLoaded.AutoSize = true;
            this.labelCurrentlyLoaded.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCurrentlyLoaded.Location = new Point(0x6b, 420);
            this.labelCurrentlyLoaded.Name = "labelCurrentlyLoaded";
            this.labelCurrentlyLoaded.Size = new Size(0x33, 13);
            this.labelCurrentlyLoaded.TabIndex = 4;
            this.labelCurrentlyLoaded.Text = "Nothing";
            this.checkBoxStartClient.AutoSize = true;
            this.checkBoxStartClient.Enabled = false;
            this.checkBoxStartClient.Location = new Point(0x16, 0x13);
            this.checkBoxStartClient.Name = "checkBoxStartClient";
            this.checkBoxStartClient.Size = new Size(0xe5, 0x11);
            this.checkBoxStartClient.TabIndex = 0;
            this.checkBoxStartClient.Text = "Enable and register remote execution client";
            this.checkBoxStartClient.UseVisualStyleBackColor = true;
            this.checkBoxStartClient.CheckedChanged += new EventHandler(this.checkBoxStartClient_CheckedChanged);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.checkBoxStartClient);
            this.groupBox3.Location = new Point(13, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x199, 0x55);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Register/Enable";
            this.groupBox3.Visible = false;
            this.label2.Location = new Point(0x25, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x169, 0x2a);
            this.label2.TabIndex = 1;
            this.label2.Text = "This registers your client with the server and thereby enables remote execution.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1b2, 0x1e2);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.labelCurrentlyLoaded);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "ExecutionClientUI";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Execution Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRetrieveProgram.Enabled = this.listBoxPrograms.SelectedIndex != -1;
        }

        private void listBoxSequences_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRetrieveSequence.Enabled = this.listBoxSequences.SelectedIndex != -1;
        }
    }
}

