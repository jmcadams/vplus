namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class GroupDialog : Form
    {
        
        
        
        
        
        
        
        
        
        
        
        
        private bool m_canClose = true;
        private Channel m_primaryChannel = null;
        

        public GroupDialog(List<Channel> channels)
        {
            this.InitializeComponent();
            this.listBoxChannels.Items.AddRange(channels.ToArray());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.m_canClose = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.m_canClose = this.m_primaryChannel != null;
            if (!this.m_canClose)
            {
                MessageBox.Show("Make sure primary channel is specified");
            }
        }

        

        private void GroupDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !this.m_canClose;
        }

        

        private void listBoxChannels_DragDrop(object sender, DragEventArgs e)
        {
            Channel data = (Channel) e.Data.GetData(typeof(Channel));
            this.listBoxChannels.Items.Add(data);
        }

        private void listBoxChannels_MouseDown(object sender, MouseEventArgs e)
        {
            int num = (e.Y / this.listBoxChannels.ItemHeight) + this.listBoxChannels.TopIndex;
            if (num < this.listBoxChannels.Items.Count)
            {
                Channel data = (Channel) this.listBoxChannels.Items[num];
                if (data != null)
                {
                    this.listBoxChannels.DoDragDrop(data, DragDropEffects.Move);
                    this.listBoxChannels.Items.Remove(data);
                }
            }
        }

        private void listBoxSecondaryChannels_DragDrop(object sender, DragEventArgs e)
        {
            Channel data = (Channel) e.Data.GetData(typeof(Channel));
            this.listBoxMirrorChannels.Items.Add(data);
        }

        private void listBoxSecondaryChannels_MouseDown(object sender, MouseEventArgs e)
        {
            int num = (e.Y / this.listBoxMirrorChannels.ItemHeight) + this.listBoxMirrorChannels.TopIndex;
            if (num < this.listBoxMirrorChannels.Items.Count)
            {
                Channel data = (Channel) this.listBoxMirrorChannels.Items[num];
                if (data != null)
                {
                    this.listBoxMirrorChannels.DoDragDrop(data, DragDropEffects.Move);
                    this.listBoxMirrorChannels.Items.Remove(data);
                }
            }
        }

        private void textBoxPrimaryChannel_DragDrop(object sender, DragEventArgs e)
        {
            Channel data = (Channel) e.Data.GetData(typeof(Channel));
            this.textBoxPrimaryChannel.Text = data.ToString();
            this.m_primaryChannel = data;
        }

        private void textBoxPrimaryChannel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Channel)) != null)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxPrimaryChannel_MouseDown(object sender, MouseEventArgs e)
        {
            this.textBoxPrimaryChannel.DoDragDrop(this.m_primaryChannel, DragDropEffects.Move);
            this.textBoxPrimaryChannel.Clear();
        }

        public Vixen.Group Group
        {
            get
            {
                return new Vixen.Group(this.m_primaryChannel);
            }
        }
    }
}

