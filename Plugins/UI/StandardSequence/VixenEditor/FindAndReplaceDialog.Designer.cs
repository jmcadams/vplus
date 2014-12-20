using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

namespace VixenEditor {
    public partial class FindAndReplaceDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private Label lblHeading;
        private Label lblFind;
        private Label lblReplace;
        private NumericUpDown udFind;
        private NumericUpDown udReplace;

        private void InitializeComponent() {
            this.udReplace = new NumericUpDown();
            this.lblReplace = new Label();
            this.udFind = new NumericUpDown();
            this.lblFind = new Label();
            this.lblHeading = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            ((ISupportInitialize)(this.udReplace)).BeginInit();
            ((ISupportInitialize)(this.udFind)).BeginInit();
            this.SuspendLayout();
            // 
            // udReplace
            // 
            this.udReplace.Location = new Point(231, 26);
            this.udReplace.Name = "udReplace";
            this.udReplace.Size = new Size(52, 20);
            this.udReplace.TabIndex = 4;
            this.udReplace.Enter += new EventHandler(this.numericUpDownReplaceWith_Enter);
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new Point(153, 28);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new Size(72, 13);
            this.lblReplace.TabIndex = 3;
            this.lblReplace.Text = "Replace with:";
            // 
            // udFind
            // 
            this.udFind.Location = new Point(74, 26);
            this.udFind.Name = "udFind";
            this.udFind.Size = new Size(52, 20);
            this.udFind.TabIndex = 2;
            this.udFind.Enter += new EventHandler(this.numericUpDownFind_Enter);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new Point(12, 28);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new Size(56, 13);
            this.lblFind.TabIndex = 1;
            this.lblFind.Text = "Find what:";
            // 
            // lblHeading
            // 
            this.lblHeading.Location = new Point(12, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new Size(271, 19);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Heading";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(127, 53);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(208, 53);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FindAndReplaceDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(295, 87);
            this.Controls.Add(this.udReplace);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.udFind);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.lblHeading);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = Resources.VixenPlus;
            this.Name = "FindAndReplaceDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Find and Replace";
            ((ISupportInitialize)(this.udReplace)).EndInit();
            ((ISupportInitialize)(this.udFind)).EndInit();
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