namespace CurrentExecution {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	internal partial class SetupDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonBoardLayout;
		private Button buttonCancel;
		private Button buttonOK;
		private Button buttonPortSetup;
		private Button buttonTextOptions;
		private Button buttonVirtualDisplaySetup;
		private CheckBox checkBoxVirtualHardware;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private Label label1;
		private Label label2;
		private Label labelPortAddress;
		private TextBox textBoxMessage;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.buttonBoardLayout = new Button();
			this.label1 = new Label();
			this.groupBox2 = new GroupBox();
			this.buttonTextOptions = new Button();
			this.textBoxMessage = new TextBox();
			this.label2 = new Label();
			this.groupBox3 = new GroupBox();
			this.buttonPortSetup = new Button();
			this.labelPortAddress = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.checkBoxVirtualHardware = new CheckBox();
			this.buttonVirtualDisplaySetup = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonBoardLayout);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(0x10, 15);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x1ab, 100);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Board Layout";
			this.buttonBoardLayout.Location = new Point(0x145, 0x1d);
			this.buttonBoardLayout.Name = "buttonBoardLayout";
			this.buttonBoardLayout.Size = new Size(0x54, 0x17);
			this.buttonBoardLayout.TabIndex = 1;
			this.buttonBoardLayout.Text = "Board layout";
			this.buttonBoardLayout.UseVisualStyleBackColor = true;
			this.buttonBoardLayout.Click += new EventHandler(this.buttonBoardLayout_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x13, 0x1d);
			this.label1.Name = "label1";
			this.label1.Size = new Size(270, 0x27);
			this.label1.TabIndex = 0;
			this.label1.Text = "Up to 4 LedTriks boards are possible in a single display.\r\nSelect how many boards you will be using and how they\r\nwill be laid out.";
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.buttonTextOptions);
			this.groupBox2.Controls.Add(this.textBoxMessage);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new Point(0x10, 0x79);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x1ab, 0x97);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Message Format";
			this.buttonTextOptions.Location = new Point(0x16, 0x6c);
			this.buttonTextOptions.Name = "buttonTextOptions";
			this.buttonTextOptions.Size = new Size(0x4b, 0x17);
			this.buttonTextOptions.TabIndex = 2;
			this.buttonTextOptions.Text = "Text options";
			this.buttonTextOptions.UseVisualStyleBackColor = true;
			this.buttonTextOptions.Click += new EventHandler(this.buttonTextOptions_Click);
			this.textBoxMessage.Location = new Point(0x16, 0x52);
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.Size = new Size(0x17e, 20);
			this.textBoxMessage.TabIndex = 1;
			this.textBoxMessage.Text = "[NAME]";
			this.textBoxMessage.Validating += new CancelEventHandler(this.textBoxMessage_Validating);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x13, 0x20);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x107, 0x27);
			this.label2.TabIndex = 0;
			this.label2.Text = "The name of the currently-executing sequence can be\r\ninserted into a custom message.  Insert [NAME] where\r\nyou want it to appear in your message.";
			this.groupBox3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox3.Controls.Add(this.buttonVirtualDisplaySetup);
			this.groupBox3.Controls.Add(this.checkBoxVirtualHardware);
			this.groupBox3.Controls.Add(this.buttonPortSetup);
			this.groupBox3.Controls.Add(this.labelPortAddress);
			this.groupBox3.Location = new Point(0x10, 0x116);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(0x1ab, 100);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Port Setup";
			this.buttonPortSetup.Location = new Point(0x145, 0x36);
			this.buttonPortSetup.Name = "buttonPortSetup";
			this.buttonPortSetup.Size = new Size(0x54, 0x17);
			this.buttonPortSetup.TabIndex = 3;
			this.buttonPortSetup.Text = "Port setup";
			this.buttonPortSetup.UseVisualStyleBackColor = true;
			this.buttonPortSetup.Click += new EventHandler(this.buttonPortSetup_Click);
			this.labelPortAddress.AutoSize = true;
			this.labelPortAddress.Location = new Point(0x13, 0x3b);
			this.labelPortAddress.Name = "labelPortAddress";
			this.labelPortAddress.Size = new Size(0xd3, 13);
			this.labelPortAddress.TabIndex = 2;
			this.labelPortAddress.Text = "Select the base address of the parallel port.";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x11f, 0x180);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x170, 0x180);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.checkBoxVirtualHardware.AutoSize = true;
			this.checkBoxVirtualHardware.Location = new Point(0x16, 0x1d);
			this.checkBoxVirtualHardware.Name = "checkBoxVirtualHardware";
			this.checkBoxVirtualHardware.Size = new Size(0xf5, 0x11);
			this.checkBoxVirtualHardware.TabIndex = 0;
			this.checkBoxVirtualHardware.Text = "I'm not using hardware, I want a virtual display.";
			this.checkBoxVirtualHardware.UseVisualStyleBackColor = true;
			this.checkBoxVirtualHardware.CheckedChanged += new EventHandler(this.checkBoxVirtualHardware_CheckedChanged);
			this.buttonVirtualDisplaySetup.Enabled = false;
			this.buttonVirtualDisplaySetup.Location = new Point(0x145, 0x19);
			this.buttonVirtualDisplaySetup.Name = "buttonVirtualDisplaySetup";
			this.buttonVirtualDisplaySetup.Size = new Size(0x54, 0x17);
			this.buttonVirtualDisplaySetup.TabIndex = 1;
			this.buttonVirtualDisplaySetup.Text = "Display setup";
			this.buttonVirtualDisplaySetup.UseVisualStyleBackColor = true;
			this.buttonVirtualDisplaySetup.Click += new EventHandler(this.buttonVirtualDisplaySetup_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x1c7, 0x1a3);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			base.FormClosing += new FormClosingEventHandler(this.SetupDialog_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
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