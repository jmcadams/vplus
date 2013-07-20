using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
    internal partial class AboutDialog
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblVersion;

        private void InitializeComponent()
        {
            this.btnOkay = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.llblURL = new System.Windows.Forms.LinkLabel();
            this.lblCredits = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(543, 166);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(100, 28);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "Done";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Red;
            this.lblName.Location = new System.Drawing.Point(196, 11);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(268, 55);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "ProductName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblName.Click += new System.EventHandler(this.AboutDialog_MouseClick);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(21, 66);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(621, 31);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "PreoductDescripton";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.Green;
            this.lblVersion.Location = new System.Drawing.Point(16, 97);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(627, 31);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "ProductVersion";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llblURL
            // 
            this.llblURL.AutoSize = true;
            this.llblURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblURL.Location = new System.Drawing.Point(16, 162);
            this.llblURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llblURL.Name = "llblURL";
            this.llblURL.Size = new System.Drawing.Size(153, 29);
            this.llblURL.TabIndex = 7;
            this.llblURL.TabStop = true;
            this.llblURL.Text = "ProductURL";
            this.llblURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblURL_LinkClicked);
            // 
            // lblCredits
            // 
            this.lblCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.Location = new System.Drawing.Point(16, 0);
            this.lblCredits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(627, 26);
            this.lblCredits.TabIndex = 8;
            this.lblCredits.Text = "label1";
            this.lblCredits.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCredits.Visible = false;
            this.lblCredits.Click += new System.EventHandler(this.lblCredits_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(17, 16);
            this.pbIcon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(85, 79);
            this.pbIcon.TabIndex = 9;
            this.pbIcon.TabStop = false;
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(659, 207);
            this.ControlBox = false;
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.llblURL);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOkay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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

        private LinkLabel llblURL;
        private Label lblCredits;
        private PictureBox pbIcon;
    }
}
