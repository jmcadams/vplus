namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class ChannelOrderDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components;
        private List<Channel> m_channelMapping;
        private List<Channel> m_channelNaturalOrder;
        private bool m_controlDown;
        private bool m_initializing;
        private int m_insertIndex;
        private int m_insertionIndex;
        private const int m_lineGutter = 10;
        private const int m_lineHeight = 40;
        private bool m_mouseDown;
        private int m_selectedIndex;
        private bool m_showNaturalNumber;
        private const int m_sideGutter = 10;
        private Panel panel1;
        private PictureBox pictureBoxChannels;
        private VScrollBar vScrollBar;

        public ChannelOrderDialog(List<Channel> channelList, List<int> channelOrder)
        {
            this.m_mouseDown = false;
            this.m_selectedIndex = -1;
            this.m_insertIndex = -1;
            this.m_initializing = true;
            this.m_insertionIndex = -1;
            this.m_controlDown = false;
            this.components = null;
            this.InitializeComponent();
            this.Construct(channelList, channelOrder);
        }

        public ChannelOrderDialog(List<Channel> channelList, List<int> channelOrder, string caption)
        {
            this.m_mouseDown = false;
            this.m_selectedIndex = -1;
            this.m_insertIndex = -1;
            this.m_initializing = true;
            this.m_insertionIndex = -1;
            this.m_controlDown = false;
            this.components = null;
            this.InitializeComponent();
            this.Construct(channelList, channelOrder);
            this.Text = caption;
        }

        private void CalcScrollParams()
        {
            if (!this.m_initializing)
            {
                int num = (int) Math.Round((double) (((float) (this.pictureBoxChannels.Height - 10)) / 40f), MidpointRounding.AwayFromZero);
                this.vScrollBar.Maximum = this.m_channelMapping.Count - 1;
                this.vScrollBar.LargeChange = num;
                if ((this.vScrollBar.LargeChange + this.vScrollBar.Value) > this.vScrollBar.Maximum)
                {
                    this.vScrollBar.Value = (this.vScrollBar.Maximum - this.vScrollBar.LargeChange) + 1;
                }
            }
        }

        private void ChannelOrderDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HelpDialog dialog = new HelpDialog("Drag channels into their new positions, or\n\nDouble-click at an insertion point.  Then Ctrl+Click on channels to move to that point.\nThe insertion point will automatically move with each channel inserted.");
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void ChannelOrderDialog_KeyDown(object sender, KeyEventArgs e)
        {
            this.m_controlDown = e.Control;
        }

        private void ChannelOrderDialog_KeyUp(object sender, KeyEventArgs e)
        {
            this.m_controlDown = e.Control;
        }

        private void Construct(List<Channel> channelList, List<int> channelOrder)
        {
            this.m_initializing = false;
            this.m_channelNaturalOrder = new List<Channel>();
            this.m_channelNaturalOrder.AddRange(channelList);
            this.m_channelMapping = new List<Channel>();
            if (channelOrder == null)
            {
                this.m_channelMapping.AddRange(channelList);
            }
            else
            {
                foreach (int num in channelOrder)
                {
                    this.m_channelMapping.Add(channelList[num]);
                }
            }
            this.m_showNaturalNumber = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ShowNaturalChannelNumber");
            this.CalcScrollParams();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.pictureBoxChannels = new PictureBox();
            this.vScrollBar = new VScrollBar();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxChannels).BeginInit();
            base.SuspendLayout();
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxChannels);
            this.panel1.Controls.Add(this.vScrollBar);
            this.panel1.Location = new Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(680, 0x201);
            this.panel1.TabIndex = 0;
            this.pictureBoxChannels.BackColor = Color.White;
            this.pictureBoxChannels.Dock = DockStyle.Fill;
            this.pictureBoxChannels.Location = new Point(0, 0);
            this.pictureBoxChannels.Name = "pictureBoxChannels";
            this.pictureBoxChannels.Size = new Size(0x295, 0x1ff);
            this.pictureBoxChannels.TabIndex = 2;
            this.pictureBoxChannels.TabStop = false;
            this.pictureBoxChannels.MouseDown += new MouseEventHandler(this.pictureBoxChannels_MouseDown);
            this.pictureBoxChannels.MouseMove += new MouseEventHandler(this.pictureBoxChannels_MouseMove);
            this.pictureBoxChannels.Paint += new PaintEventHandler(this.pictureBoxChannels_Paint);
            this.pictureBoxChannels.MouseDoubleClick += new MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
            this.pictureBoxChannels.Resize += new EventHandler(this.pictureBoxChannels_Resize);
            this.pictureBoxChannels.MouseUp += new MouseEventHandler(this.pictureBoxChannels_MouseUp);
            this.vScrollBar.Dock = DockStyle.Right;
            this.vScrollBar.Location = new Point(0x295, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new Size(0x11, 0x1ff);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new EventHandler(this.vScrollBar_ValueChanged);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x218, 0x213);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x269, 0x213);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x2c0, 0x236);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.panel1);
            base.HelpButton = true;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ChannelOrderDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Order";
            base.HelpButtonClicked += new CancelEventHandler(this.ChannelOrderDialog_HelpButtonClicked);
            base.KeyUp += new KeyEventHandler(this.ChannelOrderDialog_KeyUp);
            base.KeyDown += new KeyEventHandler(this.ChannelOrderDialog_KeyDown);
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxChannels).EndInit();
            base.ResumeLayout(false);
        }

        private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.m_mouseDown = false;
            if (!this.m_controlDown)
            {
                this.m_insertionIndex = this.vScrollBar.Value + (e.Y / 40);
                this.pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_controlDown)
            {
                if (this.m_insertionIndex != -1)
                {
                    int index = this.vScrollBar.Value + (e.Y / 40);
                    if (this.m_insertionIndex != index)
                    {
                        if (this.m_insertionIndex > index)
                        {
                            this.m_insertionIndex--;
                        }
                        Channel item = this.m_channelMapping[index];
                        this.m_channelMapping.RemoveAt(index);
                        this.m_channelMapping.Insert(this.m_insertionIndex, item);
                        if (this.m_insertionIndex < this.m_channelMapping.Count)
                        {
                            this.m_insertionIndex++;
                        }
                        if (this.m_insertionIndex < index)
                        {
                            if (this.vScrollBar.Value <= (this.vScrollBar.Maximum - this.vScrollBar.LargeChange))
                            {
                                this.vScrollBar.Value++;
                            }
                            else
                            {
                                this.pictureBoxChannels.Refresh();
                            }
                        }
                        else
                        {
                            this.pictureBoxChannels.Refresh();
                        }
                    }
                }
            }
            else
            {
                this.m_mouseDown = true;
                this.m_selectedIndex = this.vScrollBar.Value + (e.Y / 40);
            }
        }

        private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.m_mouseDown && ((e.Y >= 0) && (e.Y <= this.pictureBoxChannels.Height)))
            {
                if (e.Y < 10)
                {
                    this.ScrollUp();
                }
                else if (e.Y > (this.pictureBoxChannels.Height - 10))
                {
                    this.ScrollDown();
                }
                else
                {
                    int num = this.vScrollBar.Value + (e.Y / 40);
                    if (num == this.m_insertIndex)
                    {
                        return;
                    }
                    if (this.m_selectedIndex != num)
                    {
                        this.m_insertIndex = num;
                    }
                    else
                    {
                        this.m_insertIndex = -1;
                    }
                }
                this.pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.m_insertIndex != -1)
            {
                if (this.m_insertIndex != this.m_selectedIndex)
                {
                    Channel item = this.m_channelMapping[this.m_selectedIndex];
                    this.m_channelMapping.RemoveAt(this.m_selectedIndex);
                    if (this.m_selectedIndex > this.m_insertIndex)
                    {
                        this.m_channelMapping.Insert(this.m_insertIndex, item);
                    }
                    else
                    {
                        this.m_channelMapping.Insert(this.m_insertIndex - 1, item);
                    }
                }
                this.m_mouseDown = false;
                this.m_selectedIndex = -1;
                this.m_insertIndex = -1;
                this.pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_channelMapping.Count != 0)
            {
                int num2;
                Pen pen = new Pen(Color.Black, 2f);
                SolidBrush brush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle();
                Font font = new Font("Arial", 14f, FontStyle.Bold);
                SolidBrush brush2 = new SolidBrush(Color.Black);
                rect.Width = this.pictureBoxChannels.Width - 20;
                rect.X = 10;
                rect.Height = 30;
                rect.Y = 10;
                for (int i = 0; i < this.vScrollBar.LargeChange; i++)
                {
                    Channel item = this.m_channelMapping[i + this.vScrollBar.Value];
                    if (item.Color.ToArgb() != -1)
                    {
                        pen.Color = item.Color;
                        brush.Color = Color.FromArgb(0x40, item.Color);
                    }
                    else
                    {
                        pen.Color = Color.Black;
                        brush.Color = Color.White;
                    }
                    brush2.Color = pen.Color;
                    e.Graphics.FillRectangle(brush, rect);
                    e.Graphics.DrawRectangle(pen, rect);
                    if (this.m_showNaturalNumber)
                    {
                        e.Graphics.DrawString(string.Format("{0}: {1}", this.m_channelNaturalOrder.IndexOf(item) + 1, item.Name), font, Brushes.Black, 15f, (float) (rect.Top + 5));
                    }
                    else
                    {
                        e.Graphics.DrawString(item.Name, font, Brushes.Black, 15f, (float) (rect.Top + 5));
                    }
                    rect.Y += 40;
                }
                if (this.m_insertIndex != -1)
                {
                    num2 = (this.m_insertIndex - this.vScrollBar.Value) * 40;
                    int num3 = 5;
                    Point[] points = new Point[] { new Point(5, (num2 - 5) + num3), new Point(10, num2 + num3), new Point(5, (num2 + 5) + num3) };
                    e.Graphics.DrawPolygon(Pens.Black, points);
                }
                if (this.m_insertionIndex != -1)
                {
                    num2 = ((this.m_insertionIndex - this.vScrollBar.Value) * 40) + 2;
                    e.Graphics.FillRectangle(Brushes.Gray, 0, num2, this.pictureBoxChannels.Width, 6);
                }
                font.Dispose();
                brush.Dispose();
                brush2.Dispose();
                pen.Dispose();
            }
        }

        private void pictureBoxChannels_Resize(object sender, EventArgs e)
        {
            this.RecalcAndRedraw();
        }

        private void RecalcAndRedraw()
        {
            this.CalcScrollParams();
            this.pictureBoxChannels.Refresh();
        }

        private void ScrollDown()
        {
            int y = this.pictureBoxChannels.PointToScreen(new Point(this.pictureBoxChannels.Left, this.pictureBoxChannels.Bottom - 10)).Y;
            while (Control.MousePosition.Y > y)
            {
                if (this.vScrollBar.Value > (this.vScrollBar.Maximum - this.vScrollBar.LargeChange))
                {
                    break;
                }
                this.m_insertIndex++;
                this.vScrollBar.Value++;
            }
        }

        private void ScrollUp()
        {
            int y = this.pictureBoxChannels.PointToScreen(new Point(this.pictureBoxChannels.Left, this.pictureBoxChannels.Top + 10)).Y;
            while (Control.MousePosition.Y < y)
            {
                if (this.vScrollBar.Value == 0)
                {
                    break;
                }
                this.m_insertIndex--;
                this.vScrollBar.Value--;
                this.pictureBoxChannels.Refresh();
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.pictureBoxChannels.Refresh();
        }

        public List<Channel> ChannelMapping
        {
            get
            {
                return this.m_channelMapping;
            }
        }
    }
}

