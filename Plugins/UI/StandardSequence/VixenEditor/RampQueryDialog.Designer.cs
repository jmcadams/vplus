using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class RampQueryDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDownEnd;
        private NumericUpDown numericUpDownStart;

        private void InitializeComponent() {
            this.label1 = new Label();
            this.numericUpDownStart = new NumericUpDown();
            this.label2 = new Label();
            this.numericUpDownEnd = new NumericUpDown();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            ((ISupportInitialize)(this.numericUpDownStart)).BeginInit();
            ((ISupportInitialize)(this.numericUpDownEnd)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(44, 14);
            this.label1.Name = "label1";
            this.label1.Size = new Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting level:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            // 
            // numericUpDownStart
            // 
            this.numericUpDownStart.Location = new Point(121, 12);
            this.numericUpDownStart.Name = "numericUpDownStart";
            this.numericUpDownStart.Size = new Size(47, 20);
            this.numericUpDownStart.TabIndex = 1;
            this.numericUpDownStart.Enter += new EventHandler(this.numericUpDownStart_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(50, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ending level:";
            // 
            // numericUpDownEnd
            // 
            this.numericUpDownEnd.Location = new Point(121, 38);
            this.numericUpDownEnd.Name = "numericUpDownEnd";
            this.numericUpDownEnd.Size = new Size(46, 20);
            this.numericUpDownEnd.TabIndex = 3;
            this.numericUpDownEnd.Enter += new EventHandler(this.numericUpDownEnd_Enter);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(15, 64);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(93, 64);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // RampQueryDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(181, 93);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDownEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownStart);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "RampQueryDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ramp parameters";
            ((ISupportInitialize)(this.numericUpDownStart)).EndInit();
            ((ISupportInitialize)(this.numericUpDownEnd)).EndInit();
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
    }
}