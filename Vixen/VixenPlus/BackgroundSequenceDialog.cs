namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal partial class BackgroundSequenceDialog : Form
    {
        private string m_sequenceFileName;
        private string m_sequencesPath;
        

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

        public string BackgroundSequenceFileName
        {
            get
            {
                return this.m_sequenceFileName;
            }
        }
    }
}

