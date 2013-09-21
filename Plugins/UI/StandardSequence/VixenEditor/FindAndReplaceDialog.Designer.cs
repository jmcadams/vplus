using System.Windows.Forms;
using System.ComponentModel;

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
            this.udReplace = new System.Windows.Forms.NumericUpDown();
            this.lblReplace = new System.Windows.Forms.Label();
            this.udFind = new System.Windows.Forms.NumericUpDown();
            this.lblFind = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udReplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFind)).BeginInit();
            this.SuspendLayout();
            // 
            // udReplace
            // 
            this.udReplace.Location = new System.Drawing.Point(231, 26);
            this.udReplace.Name = "udReplace";
            this.udReplace.Size = new System.Drawing.Size(52, 20);
            this.udReplace.TabIndex = 4;
            this.udReplace.Enter += new System.EventHandler(this.numericUpDownReplaceWith_Enter);
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(153, 28);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(72, 13);
            this.lblReplace.TabIndex = 3;
            this.lblReplace.Text = "Replace with:";
            // 
            // udFind
            // 
            this.udFind.Location = new System.Drawing.Point(74, 26);
            this.udFind.Name = "udFind";
            this.udFind.Size = new System.Drawing.Size(52, 20);
            this.udFind.TabIndex = 2;
            this.udFind.Enter += new System.EventHandler(this.numericUpDownFind_Enter);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(12, 28);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(56, 13);
            this.lblFind.TabIndex = 1;
            this.lblFind.Text = "Find what:";
            // 
            // lblHeading
            // 
            this.lblHeading.Location = new System.Drawing.Point(12, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(271, 19);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Heading";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(127, 53);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(208, 53);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FindAndReplaceDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(295, 87);
            this.Controls.Add(this.udReplace);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.udFind);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.lblHeading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.Name = "FindAndReplaceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find and Replace";
            ((System.ComponentModel.ISupportInitialize)(this.udReplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFind)).EndInit();
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