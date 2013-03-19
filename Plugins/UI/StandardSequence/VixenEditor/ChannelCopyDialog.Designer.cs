using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	internal partial class ChannelCopyDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCopy;
		private Button buttonDone;
		private ComboBox comboBoxDestinationChannel;
		private ComboBox comboBoxSourceChannel;
		private Label label1;
		private Label label2;

		private void InitializeComponent() {
			this.label1 = new Label();
			this.comboBoxSourceChannel = new ComboBox();
			this.label2 = new Label();
			this.comboBoxDestinationChannel = new ComboBox();
			this.buttonCopy = new Button();
			this.buttonDone = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x16, 0x15);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x52, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source channel";
			this.comboBoxSourceChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSourceChannel.FormattingEnabled = true;
			this.comboBoxSourceChannel.Location = new Point(0x9f, 0x12);
			this.comboBoxSourceChannel.Name = "comboBoxSourceChannel";
			this.comboBoxSourceChannel.Size = new Size(0x79, 0x15);
			this.comboBoxSourceChannel.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x16, 0x30);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x65, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Destination channel";
			this.comboBoxDestinationChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationChannel.FormattingEnabled = true;
			this.comboBoxDestinationChannel.Location = new Point(0x9f, 0x2d);
			this.comboBoxDestinationChannel.Name = "comboBoxDestinationChannel";
			this.comboBoxDestinationChannel.Size = new Size(0x79, 0x15);
			this.comboBoxDestinationChannel.TabIndex = 3;
			this.buttonCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonCopy.Location = new Point(0xcd, 0x48);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new Size(0x4b, 0x17);
			this.buttonCopy.TabIndex = 4;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
			this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonDone.Location = new Point(0xcd, 0x83);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new Size(0x4b, 0x17);
			this.buttonDone.TabIndex = 5;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonDone;
			base.ClientSize = new Size(0x124, 0xa6);
			base.Controls.Add(this.buttonDone);
			base.Controls.Add(this.buttonCopy);
			base.Controls.Add(this.comboBoxDestinationChannel);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.comboBoxSourceChannel);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ChannelCopyDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Channel Copy";
			base.TopMost = true;
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