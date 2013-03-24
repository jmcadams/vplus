using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class EventAverageDialog
	{
		#region Windows Form Designer generated code
		private Button buttonDone;
private ColumnHeader columnHeader1;
private ColumnHeader columnHeader2;
private ColumnHeader columnHeader3;
private ListView lvData;

		private void InitializeComponent()
        {
            this.lvData = new ListView();
            this.buttonDone = new Button();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            base.SuspendLayout();
            this.lvData.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.lvData.Dock = DockStyle.Top;
            this.lvData.FullRowSelect = true;
            this.lvData.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvData.Location = new Point(0, 0);
            this.lvData.MultiSelect = false;
            this.lvData.Name = "listView";
            this.lvData.Size = new Size(0x1ba, 0xcf);
            this.lvData.TabIndex = 0;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = View.Details;
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new Point(0x163, 0xd5);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.columnHeader1.Text = "Plugin";
            this.columnHeader2.Text = "Channels";
            this.columnHeader3.Text = "Average duration (ms)";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ba, 0xf8);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.lvData);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "EventAverageDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Plugin Event Averages";
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
