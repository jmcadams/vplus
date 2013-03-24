using System.ComponentModel;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class Splash
	{
		private IContainer components = null;

		#region Windows Form Designer generated code
		
		private void InitializeComponent()
		{
			this.lblAppName = new System.Windows.Forms.Label();
			this.lblAppVersion = new System.Windows.Forms.Label();
			this.lblTask = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblAppName
			// 
			this.lblAppName.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAppName.ForeColor = System.Drawing.Color.Red;
			this.lblAppName.Location = new System.Drawing.Point(13, 13);
			this.lblAppName.Name = "lblAppName";
			this.lblAppName.Size = new System.Drawing.Size(535, 56);
			this.lblAppName.TabIndex = 0;
			this.lblAppName.Text = "App Name Here";
			this.lblAppName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblAppVersion
			// 
			this.lblAppVersion.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAppVersion.Location = new System.Drawing.Point(13, 69);
			this.lblAppVersion.Name = "lblAppVersion";
			this.lblAppVersion.Size = new System.Drawing.Size(535, 56);
			this.lblAppVersion.TabIndex = 1;
			this.lblAppVersion.Text = "Version Here";
			this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblTask
			// 
			this.lblTask.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTask.ForeColor = System.Drawing.Color.Green;
			this.lblTask.Location = new System.Drawing.Point(13, 151);
			this.lblTask.Name = "lblTask";
			this.lblTask.Size = new System.Drawing.Size(535, 56);
			this.lblTask.TabIndex = 2;
			this.lblTask.Text = "Loading Here";
			this.lblTask.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// Splash
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(560, 216);
			this.Controls.Add(this.lblTask);
			this.Controls.Add(this.lblAppVersion);
			this.Controls.Add(this.lblAppName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Splash";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.ResumeLayout(false);
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

		private Label lblAppName;
		private Label lblAppVersion;
		private Label lblTask;
	}
}
