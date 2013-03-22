namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal partial class CopyChannelColorsDialog : Form
    {
        private EventSequence m_destinationSequence;
        private SolidBrush m_itemBrush = null;
        private EventSequence m_sourceSequence;

        public CopyChannelColorsDialog()
        {
            this.InitializeComponent();
            string[] files = Directory.GetFiles(Paths.SequencePath, "*.vix");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            this.comboBoxSourceSequence.Items.AddRange(files);
            this.comboBoxDestinationSequence.Items.AddRange(files);
            this.m_itemBrush = new SolidBrush(Color.White);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (this.comboBoxSourceSequence.SelectedIndex == this.comboBoxDestinationSequence.SelectedIndex)
            {
                MessageBox.Show("Copying a sequence's data to itself won't accomplish anything.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if ((this.comboBoxSourceSequence.SelectedIndex == -1) || (this.comboBoxDestinationSequence.SelectedIndex == -1))
            {
                MessageBox.Show("You need to select both a source and a destination sequence.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (MessageBox.Show("This will make a change to the destination sequence that you cannot undo.\nClick 'Yes' to confirm that you approve of this.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int num = Math.Min(this.m_sourceSequence.ChannelCount, this.m_destinationSequence.ChannelCount);
                for (int i = 0; i < num; i++)
                {
                    this.m_destinationSequence.Channels[i].Color = this.m_sourceSequence.Channels[i].Color;
                }
                this.m_destinationSequence.Save();
                MessageBox.Show(this.m_destinationSequence.Name + " has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void comboBoxDestinationSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxDestinationSequence.SelectedIndex != -1)
            {
                this.m_destinationSequence = new EventSequence(Path.Combine(Paths.SequencePath, (string) this.comboBoxDestinationSequence.SelectedItem) + ".vix");
                this.comboBoxDestinationColors.Items.Clear();
                this.comboBoxDestinationColors.Items.AddRange(this.m_destinationSequence.Channels.ToArray());
                this.comboBoxDestinationColors.SelectedIndex = 0;
            }
            else
            {
                this.comboBoxDestinationColors.Items.Clear();
                if (this.m_destinationSequence != null)
                {
                    this.m_destinationSequence.Dispose();
                }
                this.m_destinationSequence = null;
            }
        }

        private void comboBoxSourceColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Channel channel = (Channel) this.comboBoxSourceColors.Items[e.Index];
                e.Graphics.FillRectangle(channel.Brush, e.Bounds);
                uint num = (uint) channel.Color.ToArgb();
                if ((num == uint.MaxValue) || (num == 0xffffff00))
                {
                    this.m_itemBrush.Color = Color.Black;
                }
                else
                {
                    this.m_itemBrush.Color = Color.White;
                }
                e.Graphics.DrawString(channel.Name, e.Font, this.m_itemBrush, (PointF) e.Bounds.Location);
            }
        }

        private void comboBoxSourceSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSourceSequence.SelectedIndex != -1)
            {
                this.m_sourceSequence = new EventSequence(Path.Combine(Paths.SequencePath, (string) this.comboBoxSourceSequence.SelectedItem) + ".vix");
                this.comboBoxSourceColors.Items.Clear();
                this.comboBoxSourceColors.Items.AddRange(this.m_sourceSequence.Channels.ToArray());
                this.comboBoxSourceColors.SelectedIndex = 0;
            }
            else
            {
                this.comboBoxSourceColors.Items.Clear();
                if (this.m_sourceSequence != null)
                {
                    this.m_sourceSequence.Dispose();
                }
                this.m_sourceSequence = null;
            }
        }

        private void CopyChannelColorsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_sourceSequence != null)
            {
                this.m_sourceSequence.Dispose();
            }
            if (this.m_destinationSequence != null)
            {
                this.m_destinationSequence.Dispose();
            }
        }

        

        
    }
}

