namespace Vixen.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class RoutineSelectDialog{
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private ListBox listBoxRoutines;

		private void InitializeComponent()
		{
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.listBoxRoutines = new ListBox();
			base.SuspendLayout();
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Enabled = false;
			this.buttonOK.Location = new Point(0x130, 0x1a6);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x181, 0x1a6);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.listBoxRoutines.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.listBoxRoutines.DrawMode = DrawMode.OwnerDrawFixed;
			this.listBoxRoutines.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.listBoxRoutines.ForeColor = Color.DarkSlateBlue;
			this.listBoxRoutines.ItemHeight = 100;
			this.listBoxRoutines.Location = new Point(12, 12);
			this.listBoxRoutines.Name = "listBoxRoutines";
			this.listBoxRoutines.ScrollAlwaysVisible = true;
			this.listBoxRoutines.Size = new Size(0x1c0, 0x194);
			this.listBoxRoutines.TabIndex = 3;
			this.listBoxRoutines.MouseDoubleClick += new MouseEventHandler(this.listBoxRoutines_MouseDoubleClick);
			this.listBoxRoutines.DrawItem += new DrawItemEventHandler(this.listBoxRoutines_DrawItem);
			this.listBoxRoutines.SelectedIndexChanged += new EventHandler(this.listBoxRoutines_SelectedIndexChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x1d8, 0x1c9);
			base.Controls.Add(this.listBoxRoutines);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Name = "RoutineSelectDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Routine Selection";
			base.ResizeBegin += new EventHandler(this.RoutineSelectDialog_ResizeBegin);
			base.ResizeEnd += new EventHandler(this.RoutineSelectDialog_ResizeEnd);
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			foreach (Routine routine in this.listBoxRoutines.Items)
			{
				routine.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
