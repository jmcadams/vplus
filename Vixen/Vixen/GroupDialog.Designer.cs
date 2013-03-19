using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	internal partial class GroupDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private Label label1;
private Label label2;
private Label label3;
private Label label4;
private Label label5;
private Label label6;
private Label label7;
private Label label8;
private ListBox listBoxChannels;
private ListBox listBoxMirrorChannels;
private TextBox textBoxPrimaryChannel;

		private void InitializeComponent()
        {
            this.label1 = new Label();
            this.listBoxChannels = new ListBox();
            this.label2 = new Label();
            this.textBoxPrimaryChannel = new TextBox();
            this.label3 = new Label();
            this.listBoxMirrorChannels = new ListBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Available Channels";
            this.listBoxChannels.AllowDrop = true;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(12, 0x22);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.Size = new Size(0x9c, 0xd4);
            this.listBoxChannels.TabIndex = 1;
            this.listBoxChannels.DragDrop += new DragEventHandler(this.listBoxChannels_DragDrop);
            this.listBoxChannels.MouseDown += new MouseEventHandler(this.listBoxChannels_MouseDown);
            this.listBoxChannels.DragOver += new DragEventHandler(this.textBoxPrimaryChannel_DragOver);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xb7, 0x21);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Primary Channel";
            this.textBoxPrimaryChannel.AllowDrop = true;
            this.textBoxPrimaryChannel.BackColor = SystemColors.Window;
            this.textBoxPrimaryChannel.Location = new Point(0xba, 0x37);
            this.textBoxPrimaryChannel.Name = "textBoxPrimaryChannel";
            this.textBoxPrimaryChannel.ReadOnly = true;
            this.textBoxPrimaryChannel.Size = new Size(0xa8, 20);
            this.textBoxPrimaryChannel.TabIndex = 3;
            this.textBoxPrimaryChannel.DragOver += new DragEventHandler(this.textBoxPrimaryChannel_DragOver);
            this.textBoxPrimaryChannel.MouseDown += new MouseEventHandler(this.textBoxPrimaryChannel_MouseDown);
            this.textBoxPrimaryChannel.DragDrop += new DragEventHandler(this.textBoxPrimaryChannel_DragDrop);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xb7, 0x5c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mirror Channels";
            this.listBoxMirrorChannels.AllowDrop = true;
            this.listBoxMirrorChannels.FormattingEnabled = true;
            this.listBoxMirrorChannels.Location = new Point(0xba, 0x70);
            this.listBoxMirrorChannels.Name = "listBoxMirrorChannels";
            this.listBoxMirrorChannels.Size = new Size(0xa8, 0x86);
            this.listBoxMirrorChannels.TabIndex = 5;
            this.listBoxMirrorChannels.DragDrop += new DragEventHandler(this.listBoxSecondaryChannels_DragDrop);
            this.listBoxMirrorChannels.MouseDown += new MouseEventHandler(this.listBoxSecondaryChannels_MouseDown);
            this.listBoxMirrorChannels.DragOver += new DragEventHandler(this.textBoxPrimaryChannel_DragOver);
            this.label4.BackColor = SystemColors.ControlDark;
            this.label4.Location = new Point(9, 0x173);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x159, 0x31);
            this.label4.TabIndex = 6;
            this.label4.Text = "\r\n  Drag and drop channels from the list on the left to the primary channel\r\n  textbox and mirror channel listbox on the right.\r\n ";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(9, 0x105);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x66, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Primary Channel:";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(9, 0x112);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x108, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "The channel that will remain active in the program grid.";
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label7.Location = new Point(9, 0x12f);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x63, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mirror Channels:";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(9, 0x13c);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x155, 0x27);
            this.label8.TabIndex = 10;
            this.label8.Text = "All remaining channels in the group.  These channels will not be active\r\nin the program grid.  All group event activity is done through the group's\r\nprimary channel.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0xc6, 0x1b3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x117, 0x1b3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x16e, 470);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.listBoxMirrorChannels);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBoxPrimaryChannel);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.listBoxChannels);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "GroupDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Group";
            base.FormClosing += new FormClosingEventHandler(this.GroupDialog_FormClosing);
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
            base.Dispose(disposing);
        }
	}
}
