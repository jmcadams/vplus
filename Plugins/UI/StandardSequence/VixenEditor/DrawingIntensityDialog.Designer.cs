using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class DrawingIntensityDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonReset;
        private Label lblInfo;
        private NumericUpDown udLevel;

        private void InitializeComponent() {
            this.lblInfo = new Label();
            this.udLevel = new NumericUpDown();
            this.buttonReset = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            ((ISupportInitialize)(this.udLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new Point(12, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(203, 90);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Info Label";
            this.lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // udLevel
            // 
            this.udLevel.Location = new Point(12, 105);
            this.udLevel.Name = "udLevel";
            this.udLevel.Size = new Size(60, 20);
            this.udLevel.TabIndex = 1;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new Point(78, 102);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new Size(137, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Set to sequence\'s max";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new EventHandler(this.buttonReset_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(140, 131);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(59, 131);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // DrawingIntensityDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(223, 161);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.udLevel);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "DrawingIntensityDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Drawing Intensity";
            ((ISupportInitialize)(this.udLevel)).EndInit();
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