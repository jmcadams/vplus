using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class PluginEnabledFixDialog
	{
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private CheckedListBox checkedListBoxPlugIns;
private GroupBox groupBox1;
private Label label2;

		private void InitializeComponent()
		{
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.label2 = new Label();
			this.checkedListBoxPlugIns = new CheckedListBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.checkedListBoxPlugIns);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new Point(14, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x150, 0x127);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Plugin Enabled/Disabled";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0xc4, 320);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x115, 320);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.label2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.label2.Location = new Point(0x17, 0x1b);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x133, 0x3a);
			this.label2.TabIndex = 2;
			this.label2.Text = "Do you want these plugins to be enabled or disabled?\r\n\r\nCheck the ones that you want to be enabled in the sequence.";
			this.checkedListBoxPlugIns.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.checkedListBoxPlugIns.FormattingEnabled = true;
			this.checkedListBoxPlugIns.Location = new Point(0x1a, 0x54);
			this.checkedListBoxPlugIns.Name = "checkedListBoxPlugIns";
			this.checkedListBoxPlugIns.Size = new Size(0x11b, 0xb8);
			this.checkedListBoxPlugIns.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x16c, 0x163);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "PluginEnabledFixDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Enabled Flag";
			this.groupBox1.ResumeLayout(false);
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
