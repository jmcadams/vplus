using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class ShutdownDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonAbort;
private Label label1;
private Label labelShutdownMessage;
private Panel panel1;
private PictureBox pictureBox1;

		private void InitializeComponent()
        {
            this.pictureBox1 = new PictureBox();
            this.label1 = new Label();
            this.panel1 = new Panel();
            this.labelShutdownMessage = new Label();
            this.buttonAbort = new Button();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.pictureBox1.Location = new Point(13, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x24, 0x24);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x53, 14);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x6d, 0x18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shutdown!";
            this.panel1.BackColor = Color.White;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new Point(0x16, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x114, 0x36);
            this.panel1.TabIndex = 2;
            this.labelShutdownMessage.Location = new Point(0x16, 0x4e);
            this.labelShutdownMessage.Name = "labelShutdownMessage";
            this.labelShutdownMessage.Size = new Size(0x113, 0x31);
            this.labelShutdownMessage.TabIndex = 3;
            this.labelShutdownMessage.Text = "Vixen is shutting down your computer in 30 seconds.\r\n\r\nYou can stop this by clicking the Abort button below.";
            this.buttonAbort.Location = new Point(0x7b, 0x87);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new Size(0x4b, 0x17);
            this.buttonAbort.TabIndex = 4;
            this.buttonAbort.Text = "Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new EventHandler(this.buttonAbort_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x141, 0xac);
            base.ControlBox = false;
            base.Controls.Add(this.buttonAbort);
            base.Controls.Add(this.labelShutdownMessage);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "ShutdownDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Shutdown!";
            base.TopMost = true;
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
