namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class TextListDialog : Form
    {
        private IContainer components = null;
        private Label labelCaption;
        private Graphics m_graphics;

        public TextListDialog(string caption)
        {
            this.InitializeComponent();
            this.m_graphics = base.CreateGraphics();
            this.Caption = caption;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_graphics.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelCaption = new Label();
            base.SuspendLayout();
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new Point(12, 9);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new Size(0x23, 13);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "label1";
            this.labelCaption.Click += new EventHandler(this.labelCaption_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x83, 0x20);
            base.Controls.Add(this.labelCaption);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "TextListDialog";
            base.Opacity = 0.85;
            base.ShowInTaskbar = false;
            this.Text = "TextListDialog";
            base.Click += new EventHandler(this.TextListDialog_Click);
            base.KeyDown += new KeyEventHandler(this.TextListDialog_KeyDown);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void labelCaption_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void TextListDialog_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void TextListDialog_KeyDown(object sender, KeyEventArgs e)
        {
            base.Close();
        }

        public string Caption
        {
            set
            {
                string[] strArray = value.Split(new char[] { '\n' });
                base.Height = (strArray.GetLength(0) * this.labelCaption.Height) + (this.labelCaption.Top << 1);
                int num = 0;
                foreach (string str in strArray)
                {
                    num = (int) Math.Max((float) num, this.m_graphics.MeasureString(str, this.labelCaption.Font).Width);
                }
                base.Width = num + (this.labelCaption.Left << 1);
                this.labelCaption.Text = value;
            }
        }
    }
}

