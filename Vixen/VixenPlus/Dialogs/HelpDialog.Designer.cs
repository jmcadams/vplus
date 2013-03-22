namespace Vixen.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class HelpDialog{
		private IContainer components;

		#region Windows Form Designer generated code
		private LinkLabel linkLabelClose;

		private void InitializeComponent()
		{
			this.linkLabelClose = new LinkLabel();
			base.SuspendLayout();
			this.linkLabelClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.linkLabelClose.AutoSize = true;
			this.linkLabelClose.LinkBehavior = LinkBehavior.NeverUnderline;
			this.linkLabelClose.Location = new Point(0x1c0, 230);
			this.linkLabelClose.Name = "linkLabelClose";
			this.linkLabelClose.Size = new Size(0x21, 13);
			this.linkLabelClose.TabIndex = 0;
			this.linkLabelClose.TabStop = true;
			this.linkLabelClose.Text = "Close";
			this.linkLabelClose.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelClose_LinkClicked);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.ClientSize = new Size(0x1ed, 0xfc);
			base.ControlBox = false;
			base.Controls.Add(this.linkLabelClose);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.KeyPreview = true;
			base.Name = "HelpDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "HelpDialog";
			base.KeyPress += new KeyPressEventHandler(this.HelpDialog_KeyPress);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			if (this.m_bigFont != null)
			{
				this.m_bigFont.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
