using System.Windows.Forms;

namespace Controllers.GenericSerial {
    internal partial class DialogSerialSetup {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbFooter;
        private CheckBox cbHeader;
        private ComboBox cbBaud;
        private GroupBox gbSerialPort;
        private GroupBox gbPacketData;
        private Label label1;
        private Label label2;
        private TextBox tbFooter;
        private TextBox tbHeader;


        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSerialSetup));
            this.gbSerialPort = new System.Windows.Forms.GroupBox();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.cdDataBits = new System.Windows.Forms.ComboBox();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbPacketData = new System.Windows.Forms.GroupBox();
            this.tbFooter = new System.Windows.Forms.TextBox();
            this.cbFooter = new System.Windows.Forms.CheckBox();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.cbHeader = new System.Windows.Forms.CheckBox();
            this.lblWarn = new System.Windows.Forms.Label();
            this.gbSerialPort.SuspendLayout();
            this.gbPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSerialPort
            // 
            this.gbSerialPort.Controls.Add(this.lblWarn);
            this.gbSerialPort.Controls.Add(this.cbPort);
            this.gbSerialPort.Controls.Add(this.cdDataBits);
            this.gbSerialPort.Controls.Add(this.cbStopBits);
            this.gbSerialPort.Controls.Add(this.cbParity);
            this.gbSerialPort.Controls.Add(this.label5);
            this.gbSerialPort.Controls.Add(this.label3);
            this.gbSerialPort.Controls.Add(this.label4);
            this.gbSerialPort.Controls.Add(this.cbBaud);
            this.gbSerialPort.Controls.Add(this.label2);
            this.gbSerialPort.Controls.Add(this.label1);
            this.gbSerialPort.Location = new System.Drawing.Point(12, 12);
            this.gbSerialPort.Name = "gbSerialPort";
            this.gbSerialPort.Size = new System.Drawing.Size(288, 115);
            this.gbSerialPort.TabIndex = 0;
            this.gbSerialPort.TabStop = false;
            this.gbSerialPort.Text = "Serial Port";
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(50, 34);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(79, 21);
            this.cbPort.TabIndex = 16;
            // 
            // cdDataBits
            // 
            this.cdDataBits.FormattingEnabled = true;
            this.cdDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cdDataBits.Location = new System.Drawing.Point(190, 61);
            this.cdDataBits.Name = "cdDataBits";
            this.cdDataBits.Size = new System.Drawing.Size(79, 21);
            this.cdDataBits.TabIndex = 15;
            // 
            // cbStopBits
            // 
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Items.AddRange(new object[] {
            "None",
            "One",
            "Two",
            "OnePointFive"});
            this.cbStopBits.Location = new System.Drawing.Point(190, 88);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(79, 21);
            this.cbStopBits.TabIndex = 14;
            // 
            // cbParity
            // 
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cbParity.Location = new System.Drawing.Point(50, 88);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(79, 21);
            this.cbParity.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Parity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Data bits";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Stop bits";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbBaud
            // 
            this.cbBaud.FormattingEnabled = true;
            this.cbBaud.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cbBaud.Location = new System.Drawing.Point(50, 61);
            this.cbBaud.Name = "cbBaud";
            this.cbBaud.Size = new System.Drawing.Size(79, 21);
            this.cbBaud.TabIndex = 3;
            this.cbBaud.SelectedIndexChanged += new System.EventHandler(this.cbBaud_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(144, 268);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(225, 268);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbPacketData
            // 
            this.gbPacketData.Controls.Add(this.tbFooter);
            this.gbPacketData.Controls.Add(this.cbFooter);
            this.gbPacketData.Controls.Add(this.tbHeader);
            this.gbPacketData.Controls.Add(this.cbHeader);
            this.gbPacketData.Location = new System.Drawing.Point(12, 133);
            this.gbPacketData.Name = "gbPacketData";
            this.gbPacketData.Size = new System.Drawing.Size(288, 129);
            this.gbPacketData.TabIndex = 3;
            this.gbPacketData.TabStop = false;
            this.gbPacketData.Text = "Packet Data";
            // 
            // tbFooter
            // 
            this.tbFooter.Location = new System.Drawing.Point(32, 98);
            this.tbFooter.Name = "tbFooter";
            this.tbFooter.Size = new System.Drawing.Size(204, 20);
            this.tbFooter.TabIndex = 3;
            // 
            // cbFooter
            // 
            this.cbFooter.AutoSize = true;
            this.cbFooter.Location = new System.Drawing.Point(13, 75);
            this.cbFooter.Name = "cbFooter";
            this.cbFooter.Size = new System.Drawing.Size(176, 17);
            this.cbFooter.TabIndex = 2;
            this.cbFooter.Text = "Each packet sends this footer...";
            this.cbFooter.UseVisualStyleBackColor = true;
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(32, 49);
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new System.Drawing.Size(204, 20);
            this.tbHeader.TabIndex = 1;
            // 
            // cbHeader
            // 
            this.cbHeader.AutoSize = true;
            this.cbHeader.Location = new System.Drawing.Point(13, 26);
            this.cbHeader.Name = "cbHeader";
            this.cbHeader.Size = new System.Drawing.Size(182, 17);
            this.cbHeader.TabIndex = 0;
            this.cbHeader.Text = "Each packet sends this header...";
            this.cbHeader.UseVisualStyleBackColor = true;
            // 
            // lblWarn
            // 
            this.lblWarn.Location = new System.Drawing.Point(139, 11);
            this.lblWarn.Name = "lblWarn";
            this.lblWarn.Size = new System.Drawing.Size(143, 44);
            this.lblWarn.TabIndex = 17;
            // 
            // DialogSerialSetup
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(311, 299);
            this.ControlBox = false;
            this.Controls.Add(this.gbPacketData);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogSerialSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Settings & Setup";
            this.gbSerialPort.ResumeLayout(false);
            this.gbSerialPort.PerformLayout();
            this.gbPacketData.ResumeLayout(false);
            this.gbPacketData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        private ComboBox cdDataBits;
        private ComboBox cbStopBits;
        private ComboBox cbParity;
        private Label label5;
        private Label label3;
        private Label label4;
        private ComboBox cbPort;
        private Label lblWarn;
    }
}
