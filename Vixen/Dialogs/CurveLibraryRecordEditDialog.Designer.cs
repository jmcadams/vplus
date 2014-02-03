using System.ComponentModel;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    internal partial class CurveLibraryRecordEditDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonColor;
        private Button buttonOK;
        private ColorDialog colorDialog;
        private GroupBox gbAll;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxController;
        private TextBox textBoxLightCount;
        private TextBox textBoxManufacturer;


        private void InitializeComponent() {
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.buttonColor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxController = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLightCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxManufacturer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.gbAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAll
            // 
            this.gbAll.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.gbAll.Controls.Add(this.buttonColor);
            this.gbAll.Controls.Add(this.label4);
            this.gbAll.Controls.Add(this.textBoxController);
            this.gbAll.Controls.Add(this.label3);
            this.gbAll.Controls.Add(this.textBoxLightCount);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Controls.Add(this.textBoxManufacturer);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Location = new System.Drawing.Point(12, 12);
            this.gbAll.Name = "gbAll";
            this.gbAll.Size = new System.Drawing.Size(436, 146);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            this.gbAll.Text = "This curve applies to the following setup...";
            // 
            // buttonColor
            // 
            this.buttonColor.BackColor = System.Drawing.Color.White;
            this.buttonColor.Location = new System.Drawing.Point(189, 83);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(97, 20);
            this.buttonColor.TabIndex = 5;
            this.buttonColor.UseVisualStyleBackColor = false;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Color";
            // 
            // textBoxController
            // 
            this.textBoxController.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxController.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxController.Location = new System.Drawing.Point(189, 109);
            this.textBoxController.Name = "textBoxController";
            this.textBoxController.Size = new System.Drawing.Size(100, 20);
            this.textBoxController.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Controller";
            // 
            // textBoxLightCount
            // 
            this.textBoxLightCount.Location = new System.Drawing.Point(189, 57);
            this.textBoxLightCount.Name = "textBoxLightCount";
            this.textBoxLightCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxLightCount.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Light count";
            // 
            // textBoxManufacturer
            // 
            this.textBoxManufacturer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxManufacturer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxManufacturer.Location = new System.Drawing.Point(189, 31);
            this.textBoxManufacturer.Name = "textBoxManufacturer";
            this.textBoxManufacturer.Size = new System.Drawing.Size(184, 20);
            this.textBoxManufacturer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Light string manufacturer";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(292, 164);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(373, 164);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            // 
            // CurveLibraryRecordEditDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(460, 199);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.gbAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurveLibraryRecordEditDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurveLibraryRecordEditDialog_FormClosing);
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
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
