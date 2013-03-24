using System;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class SequenceSettingsDialog : Form
	{
		private readonly EventSequence m_sequence;

		public SequenceSettingsDialog(EventSequence sequence)
		{
			InitializeComponent();
			m_sequence = sequence;
			numericUpDownMinimum.Value = sequence.MinimumLevel;
			numericUpDownMaximum.Value = sequence.MaximumLevel;
			textBoxEventPeriodLength.Text = sequence.EventPeriod.ToString();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (numericUpDownMinimum.Value >= numericUpDownMaximum.Value)
			{
				MessageBox.Show("Minimum must be less than the maximum.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
				base.DialogResult = DialogResult.None;
			}
			else
			{
				m_sequence.MinimumLevel = (byte) numericUpDownMinimum.Value;
				m_sequence.MaximumLevel = (byte) numericUpDownMaximum.Value;
				Cursor = Cursors.WaitCursor;
				try
				{
					int num = Convert.ToInt32(textBoxEventPeriodLength.Text);
					m_sequence.EventPeriod = num;
				}
				catch
				{
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}
	}
}