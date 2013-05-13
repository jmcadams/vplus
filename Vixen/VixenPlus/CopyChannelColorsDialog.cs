using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VixenPlus
{
    internal partial class CopyChannelColorsDialog : Form
    {
        private readonly SolidBrush _solidBrush;
        private EventSequence _destinationSequence;
        private EventSequence _sourceSequence;

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
            _solidBrush = new SolidBrush(Color.White);
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
                int num = Math.Min(_sourceSequence.ChannelCount, _destinationSequence.ChannelCount);
                for (int i = 0; i < num; i++)
                {
                    _destinationSequence.Channels[i].Color = _sourceSequence.Channels[i].Color;
                }
                _destinationSequence.Save();
                MessageBox.Show(_destinationSequence.Name + " has been updated.", Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }

        private void comboBoxDestinationSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDestinationSequence.SelectedIndex != -1)
            {
                _destinationSequence =
                    new EventSequence(Path.Combine(Paths.SequencePath, (string) comboBoxDestinationSequence.SelectedItem) + ".vix");
                comboBoxDestinationColors.Items.Clear();
                comboBoxDestinationColors.Items.AddRange(_destinationSequence.Channels.ToArray());
                comboBoxDestinationColors.SelectedIndex = 0;
            }
            else
            {
                comboBoxDestinationColors.Items.Clear();
                if (_destinationSequence != null)
                {
                    _destinationSequence.Dispose();
                }
                _destinationSequence = null;
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
                    _solidBrush.Color = Color.Black;
                }
                else
                {
                    _solidBrush.Color = Color.White;
                }
                e.Graphics.DrawString(channel.Name, e.Font, _solidBrush, e.Bounds.Location);
            }
        }

        private void comboBoxSourceSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSourceSequence.SelectedIndex != -1)
            {
                _sourceSequence =
                    new EventSequence(Path.Combine(Paths.SequencePath, (string) comboBoxSourceSequence.SelectedItem) + ".vix");
                comboBoxSourceColors.Items.Clear();
                comboBoxSourceColors.Items.AddRange(_sourceSequence.Channels.ToArray());
                comboBoxSourceColors.SelectedIndex = 0;
            }
            else
            {
                comboBoxSourceColors.Items.Clear();
                if (_sourceSequence != null)
                {
                    _sourceSequence.Dispose();
                }
                _sourceSequence = null;
            }
        }

        private void CopyChannelColorsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_sourceSequence != null)
            {
                _sourceSequence.Dispose();
            }
            if (_destinationSequence != null)
            {
                _destinationSequence.Dispose();
            }
        }
    }
}