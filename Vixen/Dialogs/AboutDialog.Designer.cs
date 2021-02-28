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
            this.btnOkay = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.LinkLabel();
            this.lblCredits = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.btnCredits = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(407, 135);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "Done";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Red;
            this.lblName.Location = new System.Drawing.Point(147, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(201, 45);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "ProductName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblName.Click += new System.EventHandler(this.AboutDialog_MouseClick);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(16, 54);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(466, 25);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "PreoductDescripton";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.Green;
            this.lblVersion.Location = new System.Drawing.Point(12, 79);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(470, 25);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "ProductVersion";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURL.Location = new System.Drawing.Point(12, 132);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(121, 24);
            this.lblURL.TabIndex = 7;
            this.lblURL.TabStop = true;
            this.lblURL.Text = "ProductURL";
            this.lblURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblURL_LinkClicked);
            // 
            // lblCredits
            // 
            this.lblCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.Location = new System.Drawing.Point(12, 0);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(470, 21);
            this.lblCredits.TabIndex = 8;
            this.lblCredits.Text = "label1";
            this.lblCredits.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCredits.Visible = false;
            this.lblCredits.Click += new System.EventHandler(this.lblCredits_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(13, 13);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(64, 64);
            this.pbIcon.TabIndex = 9;
            this.pbIcon.TabStop = false;
            // 
            // btnCredits
            // 
            this.btnCredits.Location = new System.Drawing.Point(326, 135);
            this.btnCredits.Name = "btnCredits";
            this.btnCredits.Size = new System.Drawing.Size(75, 23);
            this.btnCredits.TabIndex = 10;
            this.btnCredits.Text = "Credits";
            this.btnCredits.UseVisualStyleBackColor = true;
            this.btnCredits.Click += new System.EventHandler(this.AboutDialog_MouseClick);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(494, 168);
            this.ControlBox = false;
            this.Controls.Add(this.btnCredits);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOkay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AboutDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
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

        private LinkLabel lblURL;
        private Label lblCredits;
        private PictureBox pbIcon;
        private Button btnCredits;
    }
}
