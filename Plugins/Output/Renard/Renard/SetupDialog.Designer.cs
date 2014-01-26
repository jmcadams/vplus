namespace Renard {
    using System.Windows.Forms;

    public partial class SetupDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonSerialSetup;
        private CheckBox checkBoxHoldPort;
        private ComboBox comboBoxProtocolVersion;
        private GroupBox groupBox1;
        private Label label6;


        private void InitializeComponent() {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxProtocolVersion = new System.Windows.Forms.ComboBox();
            this.buttonSerialSetup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxHoldPort = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(113, 168);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(194, 168);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Protocol Version:";
            // 
            // comboBoxProtocolVersion
            // 
            this.comboBoxProtocolVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProtocolVersion.FormattingEnabled = true;
            this.comboBoxProtocolVersion.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboBoxProtocolVersion.Location = new System.Drawing.Point(115, 23);
            this.comboBoxProtocolVersion.Name = "comboBoxProtocolVersion";
            this.comboBoxProtocolVersion.Size = new System.Drawing.Size(44, 21);
            this.comboBoxProtocolVersion.TabIndex = 1;
            // 
            // buttonSerialSetup
            // 
            this.buttonSerialSetup.Location = new System.Drawing.Point(25, 107);
            this.buttonSerialSetup.Name = "buttonSerialSetup";
            this.buttonSerialSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonSerialSetup.TabIndex = 3;
            this.buttonSerialSetup.Text = "Serial Setup";
            this.buttonSerialSetup.UseVisualStyleBackColor = true;
            this.buttonSerialSetup.Click += new System.EventHandler(this.buttonSerialSetup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxHoldPort);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.buttonSerialSetup);
            this.groupBox1.Controls.Add(this.comboBoxProtocolVersion);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setup";
            // 
            // checkBoxHoldPort
            // 
            this.checkBoxHoldPort.Location = new System.Drawing.Point(25, 58);
            this.checkBoxHoldPort.Name = "checkBoxHoldPort";
            this.checkBoxHoldPort.Size = new System.Drawing.Size(226, 36);
            this.checkBoxHoldPort.TabIndex = 2;
            this.checkBoxHoldPort.Text = "Hold port open during the duration of the sequence execution.";
            this.checkBoxHoldPort.UseVisualStyleBackColor = true;
            // 
            // SetupDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(281, 203);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::VixenPlus.Properties.Resources.VixenPlus;
            this.Name = "SetupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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
