using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace LedTriks {
	internal partial class FrameOrganizationDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private HScrollBar hScrollBar;
		private IDataObject m_dragObject = null;
		private OpenFileDialog openFileDialog;
		private PictureBox pictureBoxEmptyBin;
		private PictureBox pictureBoxFolder;
		private PictureBox pictureBoxFullBin;
		private SaveFileDialog saveFileDialog;

		private void InitializeComponent() {
			this.hScrollBar = new HScrollBar();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.pictureBoxEmptyBin = new PictureBox();
			this.pictureBoxFullBin = new PictureBox();
			this.pictureBoxFolder = new PictureBox();
			this.saveFileDialog = new SaveFileDialog();
			this.openFileDialog = new OpenFileDialog();
			((ISupportInitialize)this.pictureBoxEmptyBin).BeginInit();
			((ISupportInitialize)this.pictureBoxFullBin).BeginInit();
			((ISupportInitialize)this.pictureBoxFolder).BeginInit();
			base.SuspendLayout();
			this.hScrollBar.Location = new Point(0x77, 0xbb);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new Size(0x11c, 0x11);
			this.hScrollBar.TabIndex = 0;
			this.hScrollBar.ValueChanged += new EventHandler(this.hScrollBar_ValueChanged);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x25d, 0x1c3);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x2ae, 0x1c3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.pictureBoxEmptyBin.Location = new Point(0x26, 0x141);
			this.pictureBoxEmptyBin.Name = "pictureBoxEmptyBin";
			this.pictureBoxEmptyBin.Size = new Size(0x30, 0x30);
			this.pictureBoxEmptyBin.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxEmptyBin.TabIndex = 3;
			this.pictureBoxEmptyBin.TabStop = false;
			this.pictureBoxEmptyBin.Visible = false;
			this.pictureBoxFullBin.Location = new Point(0x69, 0x141);
			this.pictureBoxFullBin.Name = "pictureBoxFullBin";
			this.pictureBoxFullBin.Size = new Size(0x30, 0x30);
			this.pictureBoxFullBin.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxFullBin.TabIndex = 4;
			this.pictureBoxFullBin.TabStop = false;
			this.pictureBoxFullBin.Visible = false;
			this.pictureBoxFolder.Location = new Point(0x26, 0x183);
			this.pictureBoxFolder.Name = "pictureBoxFolder";
			this.pictureBoxFolder.Size = new Size(0x30, 0x30);
			this.pictureBoxFolder.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxFolder.TabIndex = 5;
			this.pictureBoxFolder.TabStop = false;
			this.pictureBoxFolder.Visible = false;
			this.AllowDrop = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x305, 0x1e6);
			base.Controls.Add(this.pictureBoxFolder);
			base.Controls.Add(this.pictureBoxFullBin);
			base.Controls.Add(this.pictureBoxEmptyBin);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.hScrollBar);
			base.HelpButton = true;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.MinimumSize = new Size(0x30d, 520);
			base.Name = "FrameOrganizationDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Frame Organization";
			base.QueryContinueDrag += new QueryContinueDragEventHandler(this.FrameOrganizationDialog_QueryContinueDrag);
			base.ResizeBegin += new EventHandler(this.FrameOrganizationDialog_ResizeBegin);
			base.DragDrop += new DragEventHandler(this.FrameOrganizationDialog_DragDrop);
			base.HelpButtonClicked += new CancelEventHandler(this.FrameOrganizationDialog_HelpButtonClicked);
			base.Resize += new EventHandler(this.FrameOrganizationDialog_Resize);
			base.DragOver += new DragEventHandler(this.FrameOrganizationDialog_DragOver);
			base.DoubleClick += new EventHandler(this.FrameOrganizationDialog_DoubleClick);
			base.MouseUp += new MouseEventHandler(this.FrameOrganizationDialog_MouseUp);
			base.KeyUp += new KeyEventHandler(this.FrameOrganizationDialog_KeyUp);
			base.MouseMove += new MouseEventHandler(this.FrameOrganizationDialog_MouseMove);
			base.KeyDown += new KeyEventHandler(this.FrameOrganizationDialog_KeyDown);
			base.ResizeEnd += new EventHandler(this.FrameOrganizationDialog_ResizeEnd);
			base.MouseDown += new MouseEventHandler(this.FrameOrganizationDialog_MouseDown);
			base.Load += new EventHandler(this.FrameOrganizationDialog_Load);
			((ISupportInitialize)this.pictureBoxEmptyBin).EndInit();
			((ISupportInitialize)this.pictureBoxFullBin).EndInit();
			((ISupportInitialize)this.pictureBoxFolder).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			if (this.m_clipboardNumberFont != null) {
				this.m_clipboardNumberFont.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}