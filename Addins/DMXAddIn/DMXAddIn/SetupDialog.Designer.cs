using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace DMXAddIn
{
	public partial class SetupDialog {

		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private CheckBox checkBoxStartStream;
private GroupBox groupBox2;

		private void InitializeComponent()
		{
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.checkBoxStartStream = new CheckBox();
			this.groupBox2 = new GroupBox();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x4a, 0x5d);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x9b, 0x5d);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.checkBoxStartStream.AutoSize = true;
			this.checkBoxStartStream.Location = new Point(11, 28);
			this.checkBoxStartStream.Name = "checkBoxStartStream";
			this.checkBoxStartStream.Size = new Size(200, 0x11);
			this.checkBoxStartStream.TabIndex = 0;
			this.checkBoxStartStream.Text = "Start DMX stream upon Vixen startup";
			this.checkBoxStartStream.UseVisualStyleBackColor = true;
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.checkBoxStartStream);
			this.groupBox2.Location = new Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0xd9, 0x40);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "General";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xf2, 0x80);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "DMX";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
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
