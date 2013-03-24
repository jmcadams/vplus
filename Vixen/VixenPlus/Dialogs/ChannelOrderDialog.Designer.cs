namespace VixenPlus.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class ChannelOrderDialog{
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private Panel panel1;
private PictureBox pictureBoxChannels;
private VScrollBar vScrollBar;

		private void InitializeComponent()
		{
			this.panel1 = new Panel();
			this.pictureBoxChannels = new PictureBox();
			this.vScrollBar = new VScrollBar();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.panel1.SuspendLayout();
			((ISupportInitialize) this.pictureBoxChannels).BeginInit();
			base.SuspendLayout();
			this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.pictureBoxChannels);
			this.panel1.Controls.Add(this.vScrollBar);
			this.panel1.Location = new Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(680, 0x201);
			this.panel1.TabIndex = 0;
			this.pictureBoxChannels.BackColor = Color.White;
			this.pictureBoxChannels.Dock = DockStyle.Fill;
			this.pictureBoxChannels.Location = new Point(0, 0);
			this.pictureBoxChannels.Name = "pictureBoxChannels";
			this.pictureBoxChannels.Size = new Size(0x295, 0x1ff);
			this.pictureBoxChannels.TabIndex = 2;
			this.pictureBoxChannels.TabStop = false;
			this.pictureBoxChannels.MouseDown += new MouseEventHandler(this.pictureBoxChannels_MouseDown);
			this.pictureBoxChannels.MouseMove += new MouseEventHandler(this.pictureBoxChannels_MouseMove);
			this.pictureBoxChannels.Paint += new PaintEventHandler(this.pictureBoxChannels_Paint);
			this.pictureBoxChannels.MouseDoubleClick += new MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
			this.pictureBoxChannels.Resize += new EventHandler(this.pictureBoxChannels_Resize);
			this.pictureBoxChannels.MouseUp += new MouseEventHandler(this.pictureBoxChannels_MouseUp);
			this.vScrollBar.Dock = DockStyle.Right;
			this.vScrollBar.Location = new Point(0x295, 0);
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new Size(0x11, 0x1ff);
			this.vScrollBar.TabIndex = 1;
			this.vScrollBar.ValueChanged += new EventHandler(this.vScrollBar_ValueChanged);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x218, 0x213);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x269, 0x213);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x2c0, 0x236);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.panel1);
			base.HelpButton = true;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ChannelOrderDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Channel Order";
			base.HelpButtonClicked += new CancelEventHandler(this.ChannelOrderDialog_HelpButtonClicked);
			base.KeyUp += new KeyEventHandler(this.ChannelOrderDialog_KeyUp);
			base.KeyDown += new KeyEventHandler(this.ChannelOrderDialog_KeyDown);
			this.panel1.ResumeLayout(false);
			((ISupportInitialize) this.pictureBoxChannels).EndInit();
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
