using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
    internal partial class AboutDialog
    {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button btnOkay;
        private Label lblName;
        private Label lblDescription;
        private Label lblVersion;

        private void InitializeComponent()
        {
            this.btnOkay = new Button();
            this.lblName = new Label();
            this.lblDescription = new Label();
            this.lblVersion = new Label();
            this.llblURL = new LinkLabel();
            this.lblCredits = new Label();
            this.pbIcon = new PictureBox();
            this.btnCredits = new Button();
            ((ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = DialogResult.OK;
            this.btnOkay.Location = new Point(407, 135);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new Size(75, 23);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "Done";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = Color.Red;
            this.lblName.Location = new Point(147, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(201, 45);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "ProductName";
            this.lblName.TextAlign = ContentAlignment.MiddleCenter;
            this.lblName.Click += new EventHandler(this.AboutDialog_MouseClick);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new Point(16, 54);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(466, 25);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "PreoductDescripton";
            this.lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = Color.Green;
            this.lblVersion.Location = new Point(12, 79);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(470, 25);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "ProductVersion";
            this.lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // llblURL
            // 
            this.llblURL.AutoSize = true;
            this.llblURL.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.llblURL.Location = new Point(12, 132);
            this.llblURL.Name = "llblURL";
            this.llblURL.Size = new Size(121, 24);
            this.llblURL.TabIndex = 7;
            this.llblURL.TabStop = true;
            this.llblURL.Text = "ProductURL";
            this.llblURL.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llblURL_LinkClicked);
            // 
            // lblCredits
            // 
            this.lblCredits.BackColor = Color.Transparent;
            this.lblCredits.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.Location = new Point(12, 0);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new Size(470, 21);
            this.lblCredits.TabIndex = 8;
            this.lblCredits.Text = "label1";
            this.lblCredits.TextAlign = ContentAlignment.TopCenter;
            this.lblCredits.Visible = false;
            this.lblCredits.Click += new EventHandler(this.lblCredits_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new Point(13, 13);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new Size(64, 64);
            this.pbIcon.TabIndex = 9;
            this.pbIcon.TabStop = false;
            // 
            // btnCredits
            // 
            this.btnCredits.Location = new Point(326, 135);
            this.btnCredits.Name = "btnCredits";
            this.btnCredits.Size = new Size(75, 23);
            this.btnCredits.TabIndex = 10;
            this.btnCredits.Text = "Credits";
            this.btnCredits.UseVisualStyleBackColor = true;
            this.btnCredits.Click += new EventHandler(this.AboutDialog_MouseClick);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.Gainsboro;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new Size(494, 168);
            this.ControlBox = false;
            this.Controls.Add(this.btnCredits);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.llblURL);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOkay);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About";
            this.FormClosing += new FormClosingEventHandler(this.AboutDialog_FormClosing);
            ((ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private LinkLabel llblURL;
        private Label lblCredits;
        private PictureBox pbIcon;
        private Button btnCredits;
    }
}
