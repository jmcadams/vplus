using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace LedTriks {
	internal partial class BoardLayoutDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private GroupBox groupBox2;
		private Label label1;
		private PictureBox pictureBoxLayout;

		private void InitializeComponent() {
			this.groupBox2 = new GroupBox();
			this.label1 = new Label();
			this.pictureBoxLayout = new PictureBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.pictureBoxLayout).BeginInit();
			base.SuspendLayout();
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.pictureBoxLayout);
			this.groupBox2.Location = new Point(12, 0x10);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x18e, 0x10d);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Layout";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x13, 0x10);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x171, 0x1a);
			this.label1.TabIndex = 1;
			this.label1.Text = "Use your mouse to click and drag an area to select the layout of your boards.\r\nIf it turns red, you've exceeded the maximum number of boards.";
			this.pictureBoxLayout.BackColor = Color.White;
			this.pictureBoxLayout.Location = new Point(0x63, 0x37);
			this.pictureBoxLayout.Name = "pictureBoxLayout";
			this.pictureBoxLayout.Size = new Size(200, 200);
			this.pictureBoxLayout.TabIndex = 0;
			this.pictureBoxLayout.TabStop = false;
			this.pictureBoxLayout.MouseDown += new MouseEventHandler(this.pictureBoxLayout_MouseDown);
			this.pictureBoxLayout.MouseMove += new MouseEventHandler(this.pictureBoxLayout_MouseMove);
			this.pictureBoxLayout.Paint += new PaintEventHandler(this.pictureBoxLayout_Paint);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0xfe, 0x123);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x14f, 0x123);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x1a6, 0x146);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox2);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "BoardLayoutDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Board Layout";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.pictureBoxLayout).EndInit();
			base.ResumeLayout(false);
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