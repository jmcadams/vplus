namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class CopyChannelColorsDialog : Form
    {
        private Button buttonCopy;
        private ComboBox comboBoxDestinationColors;
        private ComboBox comboBoxDestinationSequence;
        private ComboBox comboBoxSourceColors;
        private ComboBox comboBoxSourceSequence;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_itemBrush != null)
            {
                this.m_itemBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.comboBoxSourceColors = new ComboBox();
            this.comboBoxSourceSequence = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.comboBoxDestinationColors = new ComboBox();
            this.comboBoxDestinationSequence = new ComboBox();
            this.buttonCopy = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.comboBoxSourceColors);
            this.groupBox1.Controls.Add(this.comboBoxSourceSequence);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x18f, 0x49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Sequence";
            this.comboBoxSourceColors.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxSourceColors.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSourceColors.FormattingEnabled = true;
            this.comboBoxSourceColors.Location = new Point(0x110, 0x1f);
            this.comboBoxSourceColors.Name = "comboBoxSourceColors";
            this.comboBoxSourceColors.Size = new Size(0x79, 0x15);
            this.comboBoxSourceColors.TabIndex = 1;
            this.comboBoxSourceColors.DrawItem += new DrawItemEventHandler(this.comboBoxSourceColors_DrawItem);
            this.comboBoxSourceSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSourceSequence.FormattingEnabled = true;
            this.comboBoxSourceSequence.Location = new Point(6, 0x1f);
            this.comboBoxSourceSequence.Name = "comboBoxSourceSequence";
            this.comboBoxSourceSequence.Size = new Size(260, 0x15);
            this.comboBoxSourceSequence.TabIndex = 0;
            this.comboBoxSourceSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.comboBoxDestinationColors);
            this.groupBox2.Controls.Add(this.comboBoxDestinationSequence);
            this.groupBox2.Location = new Point(12, 0x5b);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x18f, 0x49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination Sequence";
            this.comboBoxDestinationColors.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestinationColors.FormattingEnabled = true;
            this.comboBoxDestinationColors.Location = new Point(0x110, 0x1d);
            this.comboBoxDestinationColors.Name = "comboBoxDestinationColors";
            this.comboBoxDestinationColors.Size = new Size(0x79, 0x15);
            this.comboBoxDestinationColors.TabIndex = 1;
            this.comboBoxDestinationSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestinationSequence.FormattingEnabled = true;
            this.comboBoxDestinationSequence.Location = new Point(6, 0x1d);
            this.comboBoxDestinationSequence.Name = "comboBoxDestinationSequence";
            this.comboBoxDestinationSequence.Size = new Size(260, 0x15);
            this.comboBoxDestinationSequence.TabIndex = 0;
            this.comboBoxDestinationSequence.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationSequence_SelectedIndexChanged);
            this.buttonCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCopy.Location = new Point(0x150, 0xae);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(0x4b, 0x17);
            this.buttonCopy.TabIndex = 2;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1a7, 0xd1);
            base.Controls.Add(this.buttonCopy);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "CopyChannelColorsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Copy Channel Colors";
            base.FormClosing += new FormClosingEventHandler(this.CopyChannelColorsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

