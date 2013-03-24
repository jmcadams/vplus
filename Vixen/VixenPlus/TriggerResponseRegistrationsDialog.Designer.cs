namespace VixenPlus{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Collections;

	internal partial class TriggerResponseRegistrationsDialog{
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonDone;
private Button buttonRefresh;
private Button buttonRemove;
private ColumnHeader columnHeaderLine;
private ColumnHeader columnHeaderResponse;
private ColumnHeader columnHeaderTriggerInterface;
private GroupBox groupBox1;
private ListView listViewResponses;

		private void InitializeComponent()
		{
			this.groupBox1 = new GroupBox();
			this.buttonRemove = new Button();
			this.listViewResponses = new ListView();
			this.columnHeaderTriggerInterface = new ColumnHeader();
			this.columnHeaderLine = new ColumnHeader();
			this.columnHeaderResponse = new ColumnHeader();
			this.buttonRefresh = new Button();
			this.buttonDone = new Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonRemove);
			this.groupBox1.Controls.Add(this.listViewResponses);
			this.groupBox1.Controls.Add(this.buttonRefresh);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x1c4, 390);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Registered Trigger Responses";
			this.buttonRemove.Enabled = false;
			this.buttonRemove.Location = new Point(0x10, 0x169);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new Size(0xa1, 0x17);
			this.buttonRemove.TabIndex = 2;
			this.buttonRemove.Text = "Remove Selected Responses";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			this.listViewResponses.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.listViewResponses.Columns.AddRange(new ColumnHeader[] { this.columnHeaderTriggerInterface, this.columnHeaderLine, this.columnHeaderResponse });
			this.listViewResponses.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.listViewResponses.Location = new Point(0x10, 0x3d);
			this.listViewResponses.Name = "listViewResponses";
			this.listViewResponses.Size = new Size(0x1a3, 0x126);
			this.listViewResponses.TabIndex = 1;
			this.listViewResponses.UseCompatibleStateImageBehavior = false;
			this.listViewResponses.View = View.Details;
			this.listViewResponses.SelectedIndexChanged += new EventHandler(this.listViewResponses_SelectedIndexChanged);
			this.columnHeaderTriggerInterface.Text = "Trigger Interface";
			this.columnHeaderTriggerInterface.Width = 0x97;
			this.columnHeaderLine.Text = "Line";
			this.columnHeaderResponse.Text = "Response";
			this.columnHeaderResponse.Width = 0xb8;
			this.buttonRefresh.Location = new Point(0x10, 0x16);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new Size(0x4b, 0x17);
			this.buttonRefresh.TabIndex = 0;
			this.buttonRefresh.Text = "Refresh";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new EventHandler(this.buttonRefresh_Click);
			this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonDone.Location = new Point(0x185, 0x198);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new Size(0x4b, 0x17);
			this.buttonDone.TabIndex = 1;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonDone;
			base.ClientSize = new Size(0x1dc, 0x1bb);
			base.Controls.Add(this.buttonDone);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "TriggerResponseRegistrationsDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Trigger Response Registrations";
			base.Load += new EventHandler(this.TriggerResponseRegistrationsDialog_Load);
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
