using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    internal partial class SoundDeviceDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonDone;
        private Button buttonSet;
        private ComboBox comboBoxDevice;
        private GroupBox groupBox1;


        private void InitializeComponent() {
            this.groupBox1 = new GroupBox();
            this.buttonSet = new Button();
            this.comboBoxDevice = new ComboBox();
            this.buttonDone = new Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSet);
            this.groupBox1.Controls.Add(this.comboBoxDevice);
            this.groupBox1.Location = new Point(15, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(259, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available devices";
            // 
            // buttonSet
            // 
            this.buttonSet.Enabled = false;
            this.buttonSet.Location = new Point(92, 69);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new Size(75, 23);
            this.buttonSet.TabIndex = 1;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new EventHandler(this.buttonSet_Click);
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new Point(18, 30);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new Size(220, 21);
            this.comboBoxDevice.TabIndex = 0;
            this.comboBoxDevice.SelectedIndexChanged += new EventHandler(this.comboBoxDevice_SelectedIndexChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(199, 139);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(75, 23);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            // 
            // SoundDeviceDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(287, 168);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "SoundDeviceDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sound Device";
            this.Load += new EventHandler(this.SoundDeviceDialog_Load);
            this.groupBox1.ResumeLayout(false);
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
