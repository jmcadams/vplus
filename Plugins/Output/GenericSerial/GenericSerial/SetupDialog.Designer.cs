namespace GenericSerial {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class SetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private CheckBox checkBoxFooter;
		private CheckBox checkBoxHeader;
		private ComboBox comboBoxBaud;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private Label label1;
        private Label label2;
		private TextBox textBoxFooter;
		private TextBox textBoxHeader;

		private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.comboBoxData = new System.Windows.Forms.ComboBox();
            this.comboBoxStop = new System.Windows.Forms.ComboBox();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxFooter = new System.Windows.Forms.TextBox();
            this.checkBoxFooter = new System.Windows.Forms.CheckBox();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.checkBoxHeader = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxPort);
            this.groupBox1.Controls.Add(this.comboBoxData);
            this.groupBox1.Controls.Add(this.comboBoxStop);
            this.groupBox1.Controls.Add(this.comboBoxParity);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxBaud);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 115);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Port";
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(52, 22);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(79, 21);
            this.comboBoxPort.TabIndex = 16;
            // 
            // comboBoxData
            // 
            this.comboBoxData.FormattingEnabled = true;
            this.comboBoxData.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.comboBoxData.Location = new System.Drawing.Point(192, 49);
            this.comboBoxData.Name = "comboBoxData";
            this.comboBoxData.Size = new System.Drawing.Size(79, 21);
            this.comboBoxData.TabIndex = 15;
            // 
            // comboBoxStop
            // 
            this.comboBoxStop.FormattingEnabled = true;
            this.comboBoxStop.Items.AddRange(new object[] {
            "None",
            "One",
            "Two",
            "OnePointFive"});
            this.comboBoxStop.Location = new System.Drawing.Point(192, 76);
            this.comboBoxStop.Name = "comboBoxStop";
            this.comboBoxStop.Size = new System.Drawing.Size(79, 21);
            this.comboBoxStop.TabIndex = 14;
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.comboBoxParity.Location = new System.Drawing.Point(52, 76);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(79, 21);
            this.comboBoxParity.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Parity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Data bits";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Stop bits";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxBaud
            // 
            this.comboBoxBaud.FormattingEnabled = true;
            this.comboBoxBaud.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "9600",
            "19200",
            "38400",
            "57600",
            "119200"});
            this.comboBoxBaud.Location = new System.Drawing.Point(52, 49);
            this.comboBoxBaud.Name = "comboBoxBaud";
            this.comboBoxBaud.Size = new System.Drawing.Size(79, 21);
            this.comboBoxBaud.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(144, 268);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(225, 268);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxFooter);
            this.groupBox2.Controls.Add(this.checkBoxFooter);
            this.groupBox2.Controls.Add(this.textBoxHeader);
            this.groupBox2.Controls.Add(this.checkBoxHeader);
            this.groupBox2.Location = new System.Drawing.Point(12, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 129);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Packet Data";
            // 
            // textBoxFooter
            // 
            this.textBoxFooter.Location = new System.Drawing.Point(32, 98);
            this.textBoxFooter.Name = "textBoxFooter";
            this.textBoxFooter.Size = new System.Drawing.Size(204, 20);
            this.textBoxFooter.TabIndex = 3;
            // 
            // checkBoxFooter
            // 
            this.checkBoxFooter.AutoSize = true;
            this.checkBoxFooter.Location = new System.Drawing.Point(13, 75);
            this.checkBoxFooter.Name = "checkBoxFooter";
            this.checkBoxFooter.Size = new System.Drawing.Size(110, 17);
            this.checkBoxFooter.TabIndex = 2;
            this.checkBoxFooter.Text = "Send a text footer";
            this.checkBoxFooter.UseVisualStyleBackColor = true;
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Location = new System.Drawing.Point(32, 49);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(204, 20);
            this.textBoxHeader.TabIndex = 1;
            // 
            // checkBoxHeader
            // 
            this.checkBoxHeader.AutoSize = true;
            this.checkBoxHeader.Location = new System.Drawing.Point(13, 26);
            this.checkBoxHeader.Name = "checkBoxHeader";
            this.checkBoxHeader.Size = new System.Drawing.Size(116, 17);
            this.checkBoxHeader.TabIndex = 0;
            this.checkBoxHeader.Text = "Send a text header";
            this.checkBoxHeader.UseVisualStyleBackColor = true;
            // 
            // SetupDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(311, 299);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetupDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

        private ComboBox comboBoxData;
        private ComboBox comboBoxStop;
        private ComboBox comboBoxParity;
        private Label label5;
        private Label label3;
        private Label label4;
        private ComboBox comboBoxPort;
	}
}