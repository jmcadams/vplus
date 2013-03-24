using System;
using System.IO;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class BackgroundSequenceDialog : Form
	{
		private readonly string m_sequencesPath;
		private string m_sequenceFileName;


		public BackgroundSequenceDialog(string sequenceFileName, string sequencePath)
		{
			InitializeComponent();
			m_sequenceFileName = sequenceFileName;
			m_sequencesPath = sequencePath;
			labelSequenceName.Text = (sequenceFileName == string.Empty)
				                         ? "None"
				                         : Path.GetFileNameWithoutExtension(sequenceFileName);
		}

		public string BackgroundSequenceFileName
		{
			get { return m_sequenceFileName; }
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			labelSequenceName.Text = "None";
			m_sequenceFileName = string.Empty;
		}

		private void buttonSelect_Click(object sender, EventArgs e)
		{
			EventSequence sequence = null;
			openFileDialog.InitialDirectory = m_sequencesPath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					sequence = new EventSequence(openFileDialog.FileName);
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid sequence file.", Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Hand);
					return;
				}
				if (sequence.EngineType != EngineType.Procedural)
				{
					MessageBox.Show(
						"This sequence is not a scripted sequence.\nOnly a scripted sequence can be selected for background execution.",
						Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					labelSequenceName.Text = sequence.Name;
					m_sequenceFileName = openFileDialog.FileName;
					sequence.Dispose();
					sequence = null;
				}
			}
		}
	}
}