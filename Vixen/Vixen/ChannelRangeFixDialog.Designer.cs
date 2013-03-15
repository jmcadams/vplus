using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class ChannelRangeFixDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private GroupBox gbAll;
private Label label1;
private Label label2;
private Label label3;
private Label label4;
private Label labelChannelCount;
private ListBox listBoxPlugIns;
private TextBox textBoxFrom;
private TextBox textBoxTo;

		private void InitializeComponent()
        {
            this.gbAll = new GroupBox();
            this.textBoxTo = new TextBox();
            this.label4 = new Label();
            this.textBoxFrom = new TextBox();
            this.label3 = new Label();
            this.listBoxPlugIns = new ListBox();
            this.labelChannelCount = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.gbAll.SuspendLayout();
            base.SuspendLayout();
            this.gbAll.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gbAll.Controls.Add(this.textBoxTo);
            this.gbAll.Controls.Add(this.label4);
            this.gbAll.Controls.Add(this.textBoxFrom);
            this.gbAll.Controls.Add(this.label3);
            this.gbAll.Controls.Add(this.listBoxPlugIns);
            this.gbAll.Controls.Add(this.labelChannelCount);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Location = new Point(12, 12);
            this.gbAll.Name = "groupBox1";
            this.gbAll.Size = new Size(0x10c, 0x111);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            this.gbAll.Text = "Channel Range";
            this.textBoxTo.Location = new Point(0xd1, 0x76);
            this.textBoxTo.MaxLength = 4;
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new Size(0x30, 20);
            this.textBoxTo.TabIndex = 7;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xb2, 0x79);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "To";
            this.textBoxFrom.Location = new Point(0xd1, 0x5c);
            this.textBoxFrom.MaxLength = 4;
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new Size(0x30, 20);
            this.textBoxFrom.TabIndex = 5;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xb2, 0x5f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From";
            this.listBoxPlugIns.FormattingEnabled = true;
            this.listBoxPlugIns.Location = new Point(13, 0x5f);
            this.listBoxPlugIns.Name = "listBoxPlugIns";
            this.listBoxPlugIns.Size = new Size(0x9f, 160);
            this.listBoxPlugIns.TabIndex = 3;
            this.listBoxPlugIns.SelectedIndexChanged += new EventHandler(this.listBoxPlugIns_SelectedIndexChanged);
            this.labelChannelCount.AutoSize = true;
            this.labelChannelCount.Location = new Point(0x92, 0x41);
            this.labelChannelCount.Name = "labelChannelCount";
            this.labelChannelCount.Size = new Size(0, 13);
            this.labelChannelCount.TabIndex = 2;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(10, 0x41);
            this.label2.Name = "label2";
            this.label2.Size = new Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sequence channel count:";
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(10, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xf7, 0x2b);
            this.label1.TabIndex = 0;
            this.label1.Text = "The channel range for a plugin should not exceed the number of channels in a sequence.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0x123);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0x123);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0x146);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.gbAll);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "ChannelRangeFixDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Range";
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
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
