namespace AppUpdate {
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class ProgressDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Label lblMessage;
		private Panel panel1;

		private void InitializeComponent() {
			this.lblMessage = new Label();
			this.panel1 = new Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new Point(11, 0x1d);
			this.lblMessage.Name = "labelMessage";
			this.lblMessage.Size = new Size(0x36, 13);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Loading...";
			this.panel1.AutoSize = true;
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblMessage);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(0x14d, 0x48);
			this.panel1.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			base.ClientSize = new Size(0x14d, 0x48);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "ProgressDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			base.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
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