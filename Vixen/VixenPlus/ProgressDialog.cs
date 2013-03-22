namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            this.InitializeComponent();
        }

        public string Message
        {
            set
            {
                this.labelMessage.Text = value;
                this.labelMessage.Refresh();
            }
        }
    }
}

