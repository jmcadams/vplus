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
		private System.Windows.Forms.Label labelVersion;

		private void InitializeComponent()
		{
            this.btnOkay = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.llblURL = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(260, 135);
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
            this.lblName.Location = new System.Drawing.Point(12, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(323, 45);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "ProductName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(12, 54);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(323, 25);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "PreoductDescripton";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.Green;
            this.labelVersion.Location = new System.Drawing.Point(12, 79);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(323, 25);
            this.labelVersion.TabIndex = 6;
            this.labelVersion.Text = "ProductVersion";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llblURL
            // 
            this.llblURL.AutoSize = true;
            this.llblURL.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblURL.Location = new System.Drawing.Point(12, 132);
            this.llblURL.Name = "llblURL";
            this.llblURL.Size = new System.Drawing.Size(102, 23);
            this.llblURL.TabIndex = 7;
            this.llblURL.TabStop = true;
            this.llblURL.Text = "ProductURL";
            this.llblURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblURL_LinkClicked);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(347, 168);
            this.ControlBox = false;
            this.Controls.Add(this.llblURL);
            this.Controls.Add(this.labelVersion);
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
	}
}
