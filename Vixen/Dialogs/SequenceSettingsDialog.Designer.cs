using System.Windows.Forms;

namespace VixenPlus.Dialogs{
    public partial class SequenceSettingsDialog{
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSequenceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMinimum = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaximum = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxEventPeriodLength = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sequence name";
            this.label1.Visible = false;
            // 
            // textBoxSequenceName
            // 
            this.textBoxSequenceName.Location = new System.Drawing.Point(101, 115);
            this.textBoxSequenceName.Name = "textBoxSequenceName";
            this.textBoxSequenceName.Size = new System.Drawing.Size(161, 20);
            this.textBoxSequenceName.TabIndex = 1;
            this.textBoxSequenceName.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Minimum illumination level (0-255)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Maximum illumination level (0-255)";
            // 
            // numericUpDownMinimum
            // 
            this.numericUpDownMinimum.Location = new System.Drawing.Point(213, 24);
            this.numericUpDownMinimum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMinimum.Name = "numericUpDownMinimum";
            this.numericUpDownMinimum.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownMinimum.TabIndex = 3;
            // 
            // numericUpDownMaximum
            // 
            this.numericUpDownMaximum.Location = new System.Drawing.Point(213, 57);
            this.numericUpDownMaximum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaximum.Name = "numericUpDownMaximum";
            this.numericUpDownMaximum.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownMaximum.TabIndex = 5;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(124, 162);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(205, 162);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Event period length (ms)";
            // 
            // textBoxEventPeriodLength
            // 
            this.textBoxEventPeriodLength.Location = new System.Drawing.Point(213, 89);
            this.textBoxEventPeriodLength.Name = "textBoxEventPeriodLength";
            this.textBoxEventPeriodLength.Size = new System.Drawing.Size(45, 20);
            this.textBoxEventPeriodLength.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxEventPeriodLength);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxSequenceName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownMinimum);
            this.groupBox1.Controls.Add(this.numericUpDownMaximum);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 144);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // SequenceSettingsDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(292, 197);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SequenceSettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sequence Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximum)).EndInit();
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
