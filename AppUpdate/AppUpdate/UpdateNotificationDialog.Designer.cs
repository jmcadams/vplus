namespace AppUpdate {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;

    internal partial class UpdateNotificationDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonNo;
        private Button buttonYes;
        private Label labelMessage;
        private PictureBox pictureBox1;

        private void InitializeComponent() {
            this.labelMessage = new Label();
            this.pictureBox1 = new PictureBox();
            this.buttonYes = new Button();
            this.buttonNo = new Button();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new Point(0x41, 15);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(0x23, 13);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "label1";
            this.pictureBox1.Location = new Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x23, 0x2a);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonYes.Location = new Point(150, 0x44);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new Size(0x4b, 0x17);
            this.buttonYes.TabIndex = 2;
            this.buttonYes.Text = "&Yes";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonNo.Location = new Point(0xe7, 0x44);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new Size(0x4b, 0x17);
            this.buttonNo.TabIndex = 3;
            this.buttonNo.Text = "&No";
            this.buttonNo.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonYes;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonNo;
            base.ClientSize = new Size(0x1c6, 0x62);
            base.ControlBox = false;
            base.Controls.Add(this.buttonNo);
            base.Controls.Add(this.buttonYes);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.labelMessage);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "UpdateNotificationDialog";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "UpdateNotificationDialog";
            base.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}