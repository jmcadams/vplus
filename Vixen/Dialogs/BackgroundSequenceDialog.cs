using System;
using System.IO;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs
{
    internal partial class BackgroundSequenceDialog : Form
    {
        private readonly string _sequencesPath;


        public BackgroundSequenceDialog(string sequenceFileName, string sequencePath)
        {
            InitializeComponent();
            BackgroundSequenceFileName = sequenceFileName;
            _sequencesPath = sequencePath;
            labelSequenceName.Text = (sequenceFileName == string.Empty)
                                         ? Resources.None
                                         : Path.GetFileNameWithoutExtension(sequenceFileName);
        }


        public string BackgroundSequenceFileName { get; private set; }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            labelSequenceName.Text = Resources.None;
            BackgroundSequenceFileName = string.Empty;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = _sequencesPath;
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            EventSequence sequence;
            try
            {
                sequence = new EventSequence(openFileDialog.FileName);
            }
            catch
            {
                MessageBox.Show(Resources.NotAValidSequence, Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Hand);
                return;
            }
            if (sequence.EngineType != EngineType.Procedural)
            {
                MessageBox.Show(
                    Resources.ScripedSequenceOnly,
                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                labelSequenceName.Text = sequence.Name;
                BackgroundSequenceFileName = openFileDialog.FileName;
                sequence.Dispose();
            }
        }
    }
}