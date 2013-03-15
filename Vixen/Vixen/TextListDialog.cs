namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class TextListDialog : Form
    {
        private Graphics m_graphics;

        public TextListDialog(string caption)
        {
            this.InitializeComponent();
            this.m_graphics = base.CreateGraphics();
            this.Caption = caption;
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

