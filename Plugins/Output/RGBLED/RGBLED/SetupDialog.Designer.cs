namespace RGBLED {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class SetupDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonAdd;
		private Button buttonCancel;
		private Button buttonChangeID;
		private Button buttonOK;
		private Button buttonSerial;
		private GroupBox groupBox1;
		private Label label1;
		private Label label2;
		private Panel panelContainer;
		private TextBox textBoxNewID;
		private TextBox textBoxOldID;

		private void InitializeComponent() {
			this.buttonSerial = new Button();
			this.groupBox1 = new GroupBox();
			this.buttonChangeID = new Button();
			this.textBoxNewID = new TextBox();
			this.label2 = new Label();
			this.textBoxOldID = new TextBox();
			this.label1 = new Label();
			this.panelContainer = new Panel();
			this.buttonAdd = new Button();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.buttonSerial.Location = new Point(12, 12);
			this.buttonSerial.Name = "buttonSerial";
			this.buttonSerial.Size = new Size(0x4b, 0x17);
			this.buttonSerial.TabIndex = 0;
			this.buttonSerial.Text = "Serial Setup";
			this.buttonSerial.UseVisualStyleBackColor = true;
			this.buttonSerial.Click += new EventHandler(this.buttonSerial_Click);
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonChangeID);
			this.groupBox1.Controls.Add(this.textBoxNewID);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBoxOldID);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.panelContainer);
			this.groupBox1.Controls.Add(this.buttonAdd);
			this.groupBox1.Location = new Point(12, 0x36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x14b, 0x10b);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Controllers";
			this.buttonChangeID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonChangeID.Location = new Point(250, 0xe7);
			this.buttonChangeID.Name = "buttonChangeID";
			this.buttonChangeID.Size = new Size(0x4b, 0x17);
			this.buttonChangeID.TabIndex = 6;
			this.buttonChangeID.Text = "Change ID";
			this.buttonChangeID.UseVisualStyleBackColor = true;
			this.buttonChangeID.Click += new EventHandler(this.buttonChangeID_Click);
			this.textBoxNewID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.textBoxNewID.Location = new Point(0xc4, 0xe9);
			this.textBoxNewID.Name = "textBoxNewID";
			this.textBoxNewID.Size = new Size(40, 20);
			this.textBoxNewID.TabIndex = 5;
			this.label2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0xae, 0xec);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x10, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "to";
			this.textBoxOldID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.textBoxOldID.Location = new Point(0x80, 0xe9);
			this.textBoxOldID.Name = "textBoxOldID";
			this.textBoxOldID.Size = new Size(40, 20);
			this.textBoxOldID.TabIndex = 3;
			this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(6, 0xec);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x74, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Change ID of controller";
			this.panelContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.panelContainer.AutoScroll = true;
			this.panelContainer.Location = new Point(6, 0x30);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new Size(0x13f, 0xac);
			this.panelContainer.TabIndex = 1;
			this.panelContainer.ControlRemoved += new ControlEventHandler(this.RenumberControls);
			this.buttonAdd.Location = new Point(6, 0x13);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new Size(0x4b, 0x17);
			this.buttonAdd.TabIndex = 0;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0xb6, 0x14b);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x10c, 0x14b);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x163, 0x16e);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonSerial);
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
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