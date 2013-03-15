using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class CurveConflictResolutionDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private ColumnHeader columnHeader1;
private ColumnHeader columnHeader2;
private ColumnHeader columnHeader3;
private ColumnHeader columnHeader7;
private GroupBox gbAll;
private Label label1;
private ListView listView;

		private void InitializeComponent()
        {
            this.gbAll = new GroupBox();
            this.listView = new ListView();
            this.columnHeader7 = new ColumnHeader();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.gbAll.SuspendLayout();
            base.SuspendLayout();
            this.gbAll.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gbAll.Controls.Add(this.listView);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Location = new Point(12, 12);
            this.gbAll.Name = "groupBox1";
            this.gbAll.Size = new Size(0x1ab, 290);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            this.gbAll.Text = "Conflicts";
            this.listView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new ColumnHeader[] { this.columnHeader7, this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new Point(20, 0x55);
            this.listView.Name = "listView";
            this.listView.Size = new Size(0x187, 0xbb);
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = View.Details;
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 0x19;
            this.columnHeader1.Text = "Manufacturer";
            this.columnHeader1.Width = 150;
            this.columnHeader2.Text = "Count";
            this.columnHeader3.Text = "Controller";
            this.columnHeader3.Width = 130;
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(0x11, 0x19);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x18a, 0x20);
            this.label1.TabIndex = 0;
            this.label1.Text = "The target library already has definitions for these listed configurations.  Select which ones you want to overwrite with your changes.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x11b, 0x134);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x16c, 0x134);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1c3, 0x157);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.gbAll);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CurveConflictResolutionDialog";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Curve Conflict Resolution";
            this.gbAll.ResumeLayout(false);
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
