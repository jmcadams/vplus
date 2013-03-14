namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    internal class AboutDialog : Form
    {
        private Button button1;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label labelVersion;

        public AboutDialog()
        {
            this.InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.labelVersion.Text = string.Format("v. {0}", version);
            this.label1.Text = Vendor.ProductName;
        }

        private void AboutDialog_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.DarkGray, base.ClientRectangle);
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
            this.button1 = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.labelVersion = new Label();
            base.SuspendLayout();
            this.button1.DialogResult = DialogResult.OK;
            this.button1.Location = new Point(0x54, 0xaf);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Tahoma", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x20, 0x1f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x37, 0x17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vixen";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Tahoma", 10f);
            this.label2.Location = new Point(0x21, 0x36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x84, 0x11);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sequence Authoring";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Tahoma", 8f);
            this.label3.Location = new Point(0x21, 0x67);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x7e, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "(c) 2005-2009 K.C. Oaks";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Tahoma", 8f);
            this.label4.Location = new Point(0x21, 0x49);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x6d, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "www.vixenlights.com";
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new Font("Tahoma", 8f);
            this.labelVersion.Location = new Point(90, 0x90);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new Size(0x34, 13);
            this.labelVersion.TabIndex = 6;
            this.labelVersion.Text = "v. 0.90.0";
            base.AcceptButton = this.button1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.button1;
            base.ClientSize = new Size(0xf2, 0xd6);
            base.Controls.Add(this.labelVersion);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "AboutDialog";
            base.Opacity = 0.75;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About";
            base.Paint += new PaintEventHandler(this.AboutDialog_Paint);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

