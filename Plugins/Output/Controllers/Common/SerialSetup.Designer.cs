using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controllers.Common {
    partial class SerialSetup {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblPort = new Label();
            this.lblWarn = new Label();
            this.cbDataBits = new ComboBox();
            this.cbPortName = new ComboBox();
            this.cbBaudRate = new ComboBox();
            this.cbStopBits = new ComboBox();
            this.cbParity = new ComboBox();
            this.label4 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label5 = new Label();
            this.SuspendLayout();
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new Point(2, 6);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new Size(50, 13);
            this.lblPort.TabIndex = 21;
            this.lblPort.Text = "Com Port";
            // 
            // lblWarn
            // 
            this.lblWarn.Location = new Point(159, 3);
            this.lblWarn.Name = "lblWarn";
            this.lblWarn.Size = new Size(146, 128);
            this.lblWarn.TabIndex = 15;
            this.lblWarn.Text = "Warning";
            // 
            // cbDataBits
            // 
            this.cbDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.cbDataBits.Location = new Point(58, 56);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new Size(95, 21);
            this.cbDataBits.TabIndex = 20;
            // 
            // cbPortName
            // 
            this.cbPortName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Location = new Point(58, 3);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new Size(95, 21);
            this.cbPortName.TabIndex = 11;
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cbBaudRate.Location = new Point(58, 29);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new Size(95, 21);
            this.cbBaudRate.TabIndex = 13;
            this.cbBaudRate.SelectedIndexChanged += new EventHandler(this.comboBoxBaudRate_SelectedIndexChanged);
            // 
            // cbStopBits
            // 
            this.cbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Location = new Point(58, 110);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new Size(95, 21);
            this.cbStopBits.TabIndex = 19;
            // 
            // cbParity
            // 
            this.cbParity.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Location = new Point(58, 83);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new Size(95, 21);
            this.cbParity.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(4, 113);
            this.label4.Name = "label4";
            this.label4.Size = new Size(48, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Stop bits";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(-1, 32);
            this.label2.Name = "label2";
            this.label2.Size = new Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Baud rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 59);
            this.label3.Name = "label3";
            this.label3.Size = new Size(49, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Data bits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(19, 86);
            this.label5.Name = "label5";
            this.label5.Size = new Size(33, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Parity";
            // 
            // SerialSetup
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblWarn);
            this.Controls.Add(this.cbDataBits);
            this.Controls.Add(this.cbPortName);
            this.Controls.Add(this.cbBaudRate);
            this.Controls.Add(this.cbStopBits);
            this.Controls.Add(this.cbParity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Name = "SerialSetup";
            this.Size = new Size(308, 140);
            this.Load += new EventHandler(this.SerialSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblPort;
        private Label lblWarn;
        private ComboBox cbDataBits;
        private ComboBox cbPortName;
        private ComboBox cbBaudRate;
        private ComboBox cbStopBits;
        private ComboBox cbParity;
        private Label label4;
        private Label label2;
        private Label label3;
        private Label label5;
    }
}
