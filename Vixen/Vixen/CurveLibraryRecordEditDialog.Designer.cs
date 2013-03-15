using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class CurveLibraryRecordEditDialog
	{
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonColor;
private Button buttonOK;
private ColorDialog colorDialog;
private GroupBox gbAll;
private Label label1;
private Label label2;
private Label label3;
private Label label4;
private TextBox textBoxController;
private TextBox textBoxLightCount;
private TextBox textBoxManufacturer;

		private void InitializeComponent()
		{
			this.gbAll = new GroupBox();
			this.buttonColor = new Button();
			this.label4 = new Label();
			this.textBoxController = new TextBox();
			this.label3 = new Label();
			this.textBoxLightCount = new TextBox();
			this.label2 = new Label();
			this.textBoxManufacturer = new TextBox();
			this.label1 = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.colorDialog = new ColorDialog();
			this.gbAll.SuspendLayout();
			base.SuspendLayout();
			this.gbAll.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.gbAll.Controls.Add(this.buttonColor);
			this.gbAll.Controls.Add(this.label4);
			this.gbAll.Controls.Add(this.textBoxController);
			this.gbAll.Controls.Add(this.label3);
			this.gbAll.Controls.Add(this.textBoxLightCount);
			this.gbAll.Controls.Add(this.label2);
			this.gbAll.Controls.Add(this.textBoxManufacturer);
			this.gbAll.Controls.Add(this.label1);
			this.gbAll.Location = new Point(12, 12);
			this.gbAll.Name = "groupBox1";
			this.gbAll.Size = new Size(0x1b4, 0x92);
			this.gbAll.TabIndex = 0;
			this.gbAll.TabStop = false;
			this.gbAll.Text = "This curve applies to the following setup...";
			this.buttonColor.BackColor = Color.White;
			this.buttonColor.Location = new Point(0xbd, 0x53);
			this.buttonColor.Name = "buttonColor";
			this.buttonColor.Size = new Size(0x61, 20);
			this.buttonColor.TabIndex = 5;
			this.buttonColor.UseVisualStyleBackColor = false;
			this.buttonColor.Click += new EventHandler(this.buttonColor_Click);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(0x31, 0x56);
			this.label4.Name = "label4";
			this.label4.Size = new Size(0x1f, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Color";
			this.textBoxController.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBoxController.AutoCompleteSource = AutoCompleteSource.CustomSource;
			this.textBoxController.Location = new Point(0xbd, 0x6d);
			this.textBoxController.Name = "textBoxController";
			this.textBoxController.Size = new Size(100, 20);
			this.textBoxController.TabIndex = 7;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(0x31, 0x70);
			this.label3.Name = "label3";
			this.label3.Size = new Size(0x33, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Controller";
			this.textBoxLightCount.Location = new Point(0xbd, 0x39);
			this.textBoxLightCount.Name = "textBoxLightCount";
			this.textBoxLightCount.Size = new Size(100, 20);
			this.textBoxLightCount.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x31, 60);
			this.label2.Name = "label2";
			this.label2.Size = new Size(60, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Light count";
			this.textBoxManufacturer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBoxManufacturer.AutoCompleteSource = AutoCompleteSource.CustomSource;
			this.textBoxManufacturer.Location = new Point(0xbd, 0x1f);
			this.textBoxManufacturer.Name = "textBoxManufacturer";
			this.textBoxManufacturer.Size = new Size(0xb8, 20);
			this.textBoxManufacturer.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x31, 0x22);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x7b, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Light string manufacturer";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x124, 0xa4);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x175, 0xa4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.colorDialog.AnyColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(460, 0xc7);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.gbAll);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CurveLibraryRecordEditDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Dimming Curve";
			base.FormClosing += new FormClosingEventHandler(this.CurveLibraryRecordEditDialog_FormClosing);
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
