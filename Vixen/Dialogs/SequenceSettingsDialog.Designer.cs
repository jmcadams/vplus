using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs{
    public partial class SequenceSettingsDialog{
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numericUpDownMaximum;
        private NumericUpDown numericUpDownMinimum;
        private TextBox textBoxEventPeriodLength;
        private TextBox textBoxSequenceName;

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.textBoxSequenceName = new TextBox();
            this.label2 = new Label();
            this.label3 = new Label();
            this.numericUpDownMinimum = new NumericUpDown();
            this.numericUpDownMaximum = new NumericUpDown();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.label4 = new Label();
            this.textBoxEventPeriodLength = new TextBox();
            this.groupBox1 = new GroupBox();
            ((ISupportInitialize)(this.numericUpDownMinimum)).BeginInit();
            ((ISupportInitialize)(this.numericUpDownMaximum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(10, 118);
            this.label1.Name = "label1";
            this.label1.Size = new Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sequence name";
            this.label1.Visible = false;
            // 
            // textBoxSequenceName
            // 
            this.textBoxSequenceName.Location = new Point(101, 115);
            this.textBoxSequenceName.Name = "textBoxSequenceName";
            this.textBoxSequenceName.Size = new Size(161, 20);
            this.textBoxSequenceName.TabIndex = 1;
            this.textBoxSequenceName.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new Size(163, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Minimum illumination level (0-255)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new Size(166, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Maximum illumination level (0-255)";
            // 
            // numericUpDownMinimum
            // 
            this.numericUpDownMinimum.Location = new Point(213, 24);
            this.numericUpDownMinimum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMinimum.Name = "numericUpDownMinimum";
            this.numericUpDownMinimum.Size = new Size(45, 20);
            this.numericUpDownMinimum.TabIndex = 3;
            // 
            // numericUpDownMaximum
            // 
            this.numericUpDownMaximum.Location = new Point(213, 57);
            this.numericUpDownMaximum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaximum.Name = "numericUpDownMaximum";
            this.numericUpDownMaximum.Size = new Size(45, 20);
            this.numericUpDownMaximum.TabIndex = 5;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(124, 162);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(205, 162);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 92);
            this.label4.Name = "label4";
            this.label4.Size = new Size(121, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Event period length (ms)";
            // 
            // textBoxEventPeriodLength
            // 
            this.textBoxEventPeriodLength.Location = new Point(213, 89);
            this.textBoxEventPeriodLength.Name = "textBoxEventPeriodLength";
            this.textBoxEventPeriodLength.Size = new Size(45, 20);
            this.textBoxEventPeriodLength.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxEventPeriodLength);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxSequenceName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownMinimum);
            this.groupBox1.Controls.Add(this.numericUpDownMaximum);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(268, 144);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // SequenceSettingsDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(292, 197);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SequenceSettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sequence Settings";
            ((ISupportInitialize)(this.numericUpDownMinimum)).EndInit();
            ((ISupportInitialize)(this.numericUpDownMaximum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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
