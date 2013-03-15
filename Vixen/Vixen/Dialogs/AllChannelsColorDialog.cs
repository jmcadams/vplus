namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class AllChannelsColorDialog : Form
    {
        private Button buttonCancel;
        private Button buttonNewColor;
        private Button buttonOK;
        private ColorDialog colorDialog;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ListBox listBoxChannels;
        private ListBox listBoxColorsInUse;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_brush.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.listBoxChannels = new ListBox();
            this.groupBox2 = new GroupBox();
            this.listBoxColorsInUse = new ListBox();
            this.buttonNewColor = new Button();
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.colorDialog = new ColorDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.listBoxChannels);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd4, 0x1ad);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            this.listBoxChannels.AllowDrop = true;
            this.listBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxChannels.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.ItemHeight = 20;
            this.listBoxChannels.Location = new Point(0x10, 0x13);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(0xb7, 0x180);
            this.listBoxChannels.TabIndex = 0;
            this.listBoxChannels.DragEnter += new DragEventHandler(this.listBoxChannels_DragEnter);
            this.listBoxChannels.DragDrop += new DragEventHandler(this.listBoxChannels_DragDrop);
            this.listBoxChannels.DrawItem += new DrawItemEventHandler(this.listBoxChannels_DrawItem);
            this.groupBox2.Controls.Add(this.listBoxColorsInUse);
            this.groupBox2.Location = new Point(0xf3, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x84, 0x187);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Colors in use";
            this.listBoxColorsInUse.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxColorsInUse.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxColorsInUse.FormattingEnabled = true;
            this.listBoxColorsInUse.ItemHeight = 20;
            this.listBoxColorsInUse.Location = new Point(15, 0x13);
            this.listBoxColorsInUse.Name = "listBoxColorsInUse";
            this.listBoxColorsInUse.ScrollAlwaysVisible = true;
            this.listBoxColorsInUse.Size = new Size(0x68, 0x158);
            this.listBoxColorsInUse.TabIndex = 0;
            this.listBoxColorsInUse.DrawItem += new DrawItemEventHandler(this.listBoxColorsInUse_DrawItem);
            this.listBoxColorsInUse.MouseMove += new MouseEventHandler(this.listBoxColorsInUse_MouseMove);
            this.listBoxColorsInUse.MouseDown += new MouseEventHandler(this.listBoxColorsInUse_MouseDown);
            this.buttonNewColor.Location = new Point(0x110, 0x1a2);
            this.buttonNewColor.Name = "buttonNewColor";
            this.buttonNewColor.Size = new Size(0x4b, 0x17);
            this.buttonNewColor.TabIndex = 3;
            this.buttonNewColor.Text = "New Color";
            this.buttonNewColor.UseVisualStyleBackColor = true;
            this.buttonNewColor.Click += new EventHandler(this.buttonNewColor_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x130, 0x1db);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0xdf, 0x1db);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x187, 510);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonNewColor);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.HelpButton = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AllChannelsColorDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Colors";
            base.HelpButtonClicked += new CancelEventHandler(this.AllChannelsColorDialog_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
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

