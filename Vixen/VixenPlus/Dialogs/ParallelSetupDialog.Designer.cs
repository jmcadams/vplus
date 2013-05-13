namespace VixenPlus.Dialogs{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;

    public partial class ParallelSetupDialog{
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
private Button buttonOK;
private ComboBox comboBoxPort;
private GroupBox groupBox2;
private Label label2;
private TextBox textBoxPort;

        private void InitializeComponent()
        {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox2 = new GroupBox();
            this.textBoxPort = new TextBox();
            this.comboBoxPort = new ComboBox();
            this.label2 = new Label();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(127, 112);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(208, 112);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.textBoxPort);
            this.groupBox2.Controls.Add(this.comboBoxPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(271, 94);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parallel port";
            this.textBoxPort.Enabled = false;
            this.textBoxPort.Location = new Point(204, 49);
            this.textBoxPort.MaxLength = 4;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new Size(50, 20);
            this.textBoxPort.TabIndex = 9;
            this.comboBoxPort.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Items.AddRange(new object[] { "Standard port 1 (0378)", "Standard port 2 (0278)", "Standard port 3 (03bc)", "Other..." });
            this.comboBoxPort.Location = new Point(19, 49);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new Size(162, 21);
            this.comboBoxPort.TabIndex = 8;
            this.comboBoxPort.SelectedIndexChanged += new EventHandler(this.comboBoxPort_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(16, 27);
            this.label2.Name = "label2";
            this.label2.Size = new Size(116, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Select the port address";
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(295, 147);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ParallelSetupDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
