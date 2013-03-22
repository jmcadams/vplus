namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public partial class AllChannelsColorDialog : Form {
		private SolidBrush m_brush;
        private Dictionary<int, Color> m_colorsInUse;
        private Color m_dragColor;
        private Preference2 m_preferences;

        public AllChannelsColorDialog(List<Channel> channels)
        {
            this.InitializeComponent();
            foreach (Channel channel in channels)
            {
                this.listBoxChannels.Items.Add(channel.Clone());
            }
            this.m_colorsInUse = new Dictionary<int, Color>();
            this.m_brush = new SolidBrush(Color.White);
            foreach (Channel channel in channels)
            {
                if (!this.m_colorsInUse.ContainsKey(channel.Color.ToArgb()))
                {
                    this.listBoxColorsInUse.Items.Add(channel.Color);
                    this.m_colorsInUse.Add(channel.Color.ToArgb(), channel.Color);
                }
            }
            this.m_preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
            string[] strArray = this.m_preferences.GetString("CustomColors").Split(new char[] { ',' });
            int[] numArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = int.Parse(strArray[i]);
            }
            this.colorDialog.CustomColors = numArray;
        }

        private void AllChannelsColorDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HelpDialog dialog = new HelpDialog("Drag colors from the color list onto the channel list.\n\nIf you have one channel selected, the color will apply to whatever channel you drop it on.\n\nIf you have multiple channels selected, the color will apply to all channels selected.");
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonNewColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.m_colorsInUse.ContainsKey(this.colorDialog.Color.ToArgb()))
                {
                    MessageBox.Show("Color already exists in the list.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.listBoxColorsInUse.Items.Add(this.colorDialog.Color);
                    this.m_colorsInUse.Add(this.colorDialog.Color.ToArgb(), this.colorDialog.Color);
                }
                string[] strArray = new string[this.colorDialog.CustomColors.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = this.colorDialog.CustomColors[i].ToString();
                }
                this.m_preferences.SetString("CustomColors", string.Join(",", strArray));
            }
        }

        

        

        private void listBoxChannels_DragDrop(object sender, DragEventArgs e)
        {
            Color data = (Color) e.Data.GetData(typeof(Color));
            if (this.listBoxChannels.SelectedItems.Count > 1)
            {
                foreach (Channel channel in this.listBoxChannels.SelectedItems)
                {
                    channel.Color = data;
                }
            }
            else
            {
                Point p = this.listBoxChannels.PointToClient(new Point(e.X, e.Y));
                ((Channel) this.listBoxChannels.Items[this.listBoxChannels.IndexFromPoint(p)]).Color = data;
            }
            this.listBoxChannels.Refresh();
        }

        private void listBoxChannels_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Color)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listBoxChannels_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Channel channel = (Channel) this.listBoxChannels.Items[e.Index];
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                Rectangle rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Height - 4, e.Bounds.Height - 4);
                if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                {
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
                else
                {
                    e.Graphics.DrawRectangle(Pens.White, rect);
                }
                rect.X += 2;
                rect.Y += 2;
                rect.Width -= 3;
                rect.Height -= 3;
                e.Graphics.FillRectangle(channel.Brush, rect);
                if (((channel.Color.R + channel.Color.G) + channel.Color.B) < 100)
                {
                    this.m_brush.Color = Color.White;
                }
                else
                {
                    this.m_brush.Color = Color.Black;
                }
                e.Graphics.DrawString(channel.Name, this.Font, this.m_brush, (float) ((e.Bounds.X + e.Bounds.Height) + 2), (float) (e.Bounds.Y + 3));
            }
        }

        private void listBoxColorsInUse_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                this.m_brush.Color = (Color) this.listBoxColorsInUse.Items[e.Index];
                e.Graphics.FillRectangle(this.m_brush, e.Bounds);
            }
        }

        private void listBoxColorsInUse_MouseDown(object sender, MouseEventArgs e)
        {
            int num = this.listBoxColorsInUse.IndexFromPoint(e.Location);
            if (num != -1)
            {
                this.m_dragColor = (Color) this.listBoxColorsInUse.Items[num];
            }
            else
            {
                this.m_dragColor = Color.Empty;
            }
        }

        private void listBoxColorsInUse_MouseMove(object sender, MouseEventArgs e)
        {
            if (((Control.MouseButtons & MouseButtons.Left) != MouseButtons.None) && (this.m_dragColor != Color.Empty))
            {
                this.listBoxColorsInUse.DoDragDrop(this.m_dragColor, DragDropEffects.Copy);
            }
        }

        public List<Color> ChannelColors
        {
            get
            {
                List<Color> list = new List<Color>();
                foreach (Channel channel in this.listBoxChannels.Items)
                {
                    list.Add(channel.Color);
                }
                return list;
            }
        }
    }
}

