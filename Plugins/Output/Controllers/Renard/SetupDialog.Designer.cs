using System.Windows.Forms;

namespace Controllers.Renard {
    public partial class SetupDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxHoldPort;
        private ComboBox comboBoxProtocolVersion;
        private Label label6;


        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxProtocolVersion = new System.Windows.Forms.ComboBox();
            this.checkBoxHoldPort = new System.Windows.Forms.CheckBox();
            this.serialSetup1 = new Controllers.Common.SerialSetup();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(784, 192);
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
            this.buttonCancel.Location = new System.Drawing.Point(865, 192);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(168, 19);
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
            this.comboBoxProtocolVersion.Location = new System.Drawing.Point(261, 16);
            this.comboBoxProtocolVersion.Name = "comboBoxProtocolVersion";
            this.comboBoxProtocolVersion.Size = new System.Drawing.Size(57, 21);
            this.comboBoxProtocolVersion.TabIndex = 1;
            // 
            // checkBoxHoldPort
            // 
            this.checkBoxHoldPort.Location = new System.Drawing.Point(12, 144);
            this.checkBoxHoldPort.Name = "checkBoxHoldPort";
            this.checkBoxHoldPort.Size = new System.Drawing.Size(331, 22);
            this.checkBoxHoldPort.TabIndex = 2;
            this.checkBoxHoldPort.Text = "Hold port open during the duration of the sequence execution.";
            this.checkBoxHoldPort.UseVisualStyleBackColor = true;
            // 
            // serialSetup1
            // 
            this.serialSetup1.Location = new System.Drawing.Point(13, 13);
            this.serialSetup1.Name = "serialSetup1";
            this.serialSetup1.Size = new System.Drawing.Size(308, 125);
            this.serialSetup1.TabIndex = 3;
            // 
            // SetupDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(952, 227);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxHoldPort);
            this.Controls.Add(this.comboBoxProtocolVersion);
            this.Controls.Add(this.serialSetup1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Common.SerialSetup serialSetup1;
    }
}
