namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class BackgroundSequenceDialog : Form
    {
        private Button buttonCancel;
        private Button buttonClear;
        private Button buttonOK;
        private Button buttonSelect;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label labelSequenceName;
        private string m_sequenceFileName;
        private string m_sequencesPath;
        private OpenFileDialog openFileDialog;

        public BackgroundSequenceDialog(string sequenceFileName, string sequencePath)
        {
            this.InitializeComponent();
            this.m_sequenceFileName = sequenceFileName;
            this.m_sequencesPath = sequencePath;
            this.labelSequenceName.Text = (sequenceFileName == string.Empty) ? "None" : Path.GetFileNameWithoutExtension(sequenceFileName);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.labelSequenceName.Text = "None";
            this.m_sequenceFileName = string.Empty;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            EventSequence sequence = null;
            this.openFileDialog.InitialDirectory = this.m_sequencesPath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sequence = new EventSequence(this.openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid sequence file.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (sequence.EngineType != EngineType.Procedural)
                {
                    MessageBox.Show("This sequence is not a scripted sequence.\nOnly a scripted sequence can be selected for background execution.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this.labelSequenceName.Text = sequence.Name;
                    this.m_sequenceFileName = this.openFileDialog.FileName;
                    sequence.Dispose();
                    sequence = null;
                }
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
            this.label1 = new Label();
            this.groupBox1 = new GroupBox();
            this.buttonClear = new Button();
            this.buttonSelect = new Button();
            this.labelSequenceName = new Label();
            this.label2 = new Label();
            this.buttonOK = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(280, 0x48);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can have a scripted sequence run in the background if there are no other sequences or programs executing.  This sequence will be interrupted by any other executing item.";
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.buttonSelect);
            this.groupBox1.Controls.Add(this.labelSequenceName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(15, 0x58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x115, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background Sequence";
            this.buttonClear.Location = new Point(0x8d, 60);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(0x4b, 0x17);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            this.buttonSelect.Location = new Point(60, 60);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new Size(0x4b, 0x17);
            this.buttonSelect.TabIndex = 2;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
            this.labelSequenceName.AutoSize = true;
            this.labelSequenceName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelSequenceName.Location = new Point(6, 0x22);
            this.labelSequenceName.Name = "labelSequenceName";
            this.labelSequenceName.Size = new Size(0x25, 13);
            this.labelSequenceName.TabIndex = 1;
            this.labelSequenceName.Text = "None";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x66, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Selected sequence:";
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x88, 0xc2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.openFileDialog.Title = "Select a Scripted Sequence";
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xd9, 0xc2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x130, 0xe1);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "BackgroundSequenceDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Background Sequence";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public string BackgroundSequenceFileName
        {
            get
            {
                return this.m_sequenceFileName;
            }
        }
    }
}

