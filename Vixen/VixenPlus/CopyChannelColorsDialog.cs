using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class CopyChannelColorsDialog : Form
	{
		private readonly SolidBrush m_itemBrush;
		private EventSequence m_destinationSequence;
		private EventSequence m_sourceSequence;

		public CopyChannelColorsDialog()
		{
			InitializeComponent();
			string[] files = Directory.GetFiles(Paths.SequencePath, "*.vix");
			for (int i = 0; i < files.Length; i++)
			{
				files[i] = Path.GetFileNameWithoutExtension(files[i]);
			}
			comboBoxSourceSequence.Items.AddRange(files);
			comboBoxDestinationSequence.Items.AddRange(files);
			m_itemBrush = new SolidBrush(Color.White);
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			if (comboBoxSourceSequence.SelectedIndex == comboBoxDestinationSequence.SelectedIndex)
			{
				MessageBox.Show("Copying a sequence's data to itself won't accomplish anything.", Vendor.ProductName,
				                MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if ((comboBoxSourceSequence.SelectedIndex == -1) || (comboBoxDestinationSequence.SelectedIndex == -1))
			{
				MessageBox.Show("You need to select both a source and a destination sequence.", Vendor.ProductName,
				                MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (
				MessageBox.Show(
					"This will make a change to the destination sequence that you cannot undo.\nClick 'Yes' to confirm that you approve of this.",
					Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				int num = Math.Min(m_sourceSequence.ChannelCount, m_destinationSequence.ChannelCount);
				for (int i = 0; i < num; i++)
				{
					m_destinationSequence.Channels[i].Color = m_sourceSequence.Channels[i].Color;
				}
				m_destinationSequence.Save();
				MessageBox.Show(m_destinationSequence.Name + " has been updated.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Asterisk);
			}
		}

		private void comboBoxDestinationSequence_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxDestinationSequence.SelectedIndex != -1)
			{
				m_destinationSequence =
					new EventSequence(Path.Combine(Paths.SequencePath, (string) comboBoxDestinationSequence.SelectedItem) + ".vix");
				comboBoxDestinationColors.Items.Clear();
				comboBoxDestinationColors.Items.AddRange(m_destinationSequence.Channels.ToArray());
				comboBoxDestinationColors.SelectedIndex = 0;
			}
			else
			{
				comboBoxDestinationColors.Items.Clear();
				if (m_destinationSequence != null)
				{
					m_destinationSequence.Dispose();
				}
				m_destinationSequence = null;
			}
		}

		private void comboBoxSourceColors_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				var channel = (Channel) comboBoxSourceColors.Items[e.Index];
				e.Graphics.FillRectangle(channel.Brush, e.Bounds);
				var num = (uint) channel.Color.ToArgb();
				if ((num == uint.MaxValue) || (num == 0xffffff00))
				{
					m_itemBrush.Color = Color.Black;
				}
				else
				{
					m_itemBrush.Color = Color.White;
				}
				e.Graphics.DrawString(channel.Name, e.Font, m_itemBrush, e.Bounds.Location);
			}
		}

		private void comboBoxSourceSequence_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxSourceSequence.SelectedIndex != -1)
			{
				m_sourceSequence =
					new EventSequence(Path.Combine(Paths.SequencePath, (string) comboBoxSourceSequence.SelectedItem) + ".vix");
				comboBoxSourceColors.Items.Clear();
				comboBoxSourceColors.Items.AddRange(m_sourceSequence.Channels.ToArray());
				comboBoxSourceColors.SelectedIndex = 0;
			}
			else
			{
				comboBoxSourceColors.Items.Clear();
				if (m_sourceSequence != null)
				{
					m_sourceSequence.Dispose();
				}
				m_sourceSequence = null;
			}
		}

		private void CopyChannelColorsDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_sourceSequence != null)
			{
				m_sourceSequence.Dispose();
			}
			if (m_destinationSequence != null)
			{
				m_destinationSequence.Dispose();
			}
		}
	}
}