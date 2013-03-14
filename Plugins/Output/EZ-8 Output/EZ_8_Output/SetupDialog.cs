namespace EZ_8_Output
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    internal class SetupDialog : Form
    {
        private IContainer components = null;

        public SetupDialog()
        {
            this.InitializeComponent();
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
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "SetupDialog";
        }
    }
}

