using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class RandomParametersDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxIntensityLevel;
        private CheckBox checkBoxUseSaturation;
        private Label lblPctSaturation;
        private Label lblHeader;
        private Label lblBetween;
        private Label lblPctMin;
        private Label lblPctMax;
        private Label lblAnd;
        private NumericUpDown udMax;
        private NumericUpDown udMin;
        private NumericUpDown udPeriods;
        private NumericUpDown udSaturation;

        private void InitializeComponent() {
            this.udSaturation = new NumericUpDown();
            this.lblPctSaturation = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.checkBoxUseSaturation = new CheckBox();
            this.lblHeader = new Label();
            this.udPeriods = new NumericUpDown();
            this.lblAnd = new Label();
            this.lblPctMax = new Label();
            this.udMax = new NumericUpDown();
            this.lblPctMin = new Label();
            this.udMin = new NumericUpDown();
            this.lblBetween = new Label();
            this.checkBoxIntensityLevel = new CheckBox();
            this.lblPeriodLen = new Label();
            this.pbExample = new PictureBox();
            this.lblExample = new Label();
            ((ISupportInitialize)(this.udSaturation)).BeginInit();
            ((ISupportInitialize)(this.udPeriods)).BeginInit();
            ((ISupportInitialize)(this.udMax)).BeginInit();
            ((ISupportInitialize)(this.udMin)).BeginInit();
            ((ISupportInitialize)(this.pbExample)).BeginInit();
            this.SuspendLayout();
            // 
            // udSaturation
            // 
            this.udSaturation.Location = new Point(212, 86);
            this.udSaturation.Name = "udSaturation";
            this.udSaturation.Size = new Size(46, 20);
            this.udSaturation.TabIndex = 3;
            this.udSaturation.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udSaturation.ValueChanged += new EventHandler(this.ud_ValueChanged);
            this.udSaturation.Enter += new EventHandler(this.UpDownEnter);
            // 
            // lblPctSaturation
            // 
            this.lblPctSaturation.AutoSize = true;
            this.lblPctSaturation.Location = new Point(258, 88);
            this.lblPctSaturation.Name = "lblPctSaturation";
            this.lblPctSaturation.Size = new Size(15, 13);
            this.lblPctSaturation.TabIndex = 11;
            this.lblPctSaturation.Text = "%";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(124, 169);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(205, 169);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseSaturation
            // 
            this.checkBoxUseSaturation.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.checkBoxUseSaturation.Location = new Point(15, 87);
            this.checkBoxUseSaturation.Name = "checkBoxUseSaturation";
            this.checkBoxUseSaturation.Size = new Size(136, 17);
            this.checkBoxUseSaturation.TabIndex = 2;
            this.checkBoxUseSaturation.Text = "Ensure Coverage:";
            this.checkBoxUseSaturation.TextAlign = ContentAlignment.MiddleRight;
            this.checkBoxUseSaturation.UseVisualStyleBackColor = true;
            this.checkBoxUseSaturation.CheckedChanged += new EventHandler(this.checkBoxUseSaturation_CheckedChanged);
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new Point(12, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new Size(268, 48);
            this.lblHeader.TabIndex = 9;
            this.lblHeader.Text = "For standard random results without intensity variation, you can press Enter or c" +
    "lick OK, otherwise adjust the parameters below.";
            // 
            // udPeriods
            // 
            this.udPeriods.Location = new Point(212, 60);
            this.udPeriods.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPeriods.Name = "udPeriods";
            this.udPeriods.Size = new Size(46, 20);
            this.udPeriods.TabIndex = 1;
            this.udPeriods.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPeriods.ValueChanged += new EventHandler(this.ud_ValueChanged);
            this.udPeriods.Enter += new EventHandler(this.UpDownEnter);
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new Point(181, 140);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new Size(25, 13);
            this.lblAnd.TabIndex = 14;
            this.lblAnd.Text = "and";
            // 
            // lblPctMax
            // 
            this.lblPctMax.AutoSize = true;
            this.lblPctMax.Location = new Point(258, 140);
            this.lblPctMax.Name = "lblPctMax";
            this.lblPctMax.Size = new Size(15, 13);
            this.lblPctMax.TabIndex = 15;
            this.lblPctMax.Text = "%";
            // 
            // udMax
            // 
            this.udMax.Location = new Point(212, 138);
            this.udMax.Name = "udMax";
            this.udMax.Size = new Size(46, 20);
            this.udMax.TabIndex = 6;
            this.udMax.ValueChanged += new EventHandler(this.ud_ValueChanged);
            this.udMax.Enter += new EventHandler(this.UpDownEnter);
            // 
            // lblPctMin
            // 
            this.lblPctMin.AutoSize = true;
            this.lblPctMin.Location = new Point(258, 114);
            this.lblPctMin.Name = "lblPctMin";
            this.lblPctMin.Size = new Size(15, 13);
            this.lblPctMin.TabIndex = 13;
            this.lblPctMin.Text = "%";
            // 
            // udMin
            // 
            this.udMin.Location = new Point(212, 112);
            this.udMin.Name = "udMin";
            this.udMin.Size = new Size(46, 20);
            this.udMin.TabIndex = 5;
            this.udMin.ValueChanged += new EventHandler(this.ud_ValueChanged);
            this.udMin.Enter += new EventHandler(this.UpDownEnter);
            // 
            // lblBetween
            // 
            this.lblBetween.AutoSize = true;
            this.lblBetween.Location = new Point(157, 114);
            this.lblBetween.Name = "lblBetween";
            this.lblBetween.Size = new Size(49, 13);
            this.lblBetween.TabIndex = 12;
            this.lblBetween.Text = "Between";
            // 
            // checkBoxIntensityLevel
            // 
            this.checkBoxIntensityLevel.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.checkBoxIntensityLevel.Location = new Point(15, 113);
            this.checkBoxIntensityLevel.Name = "checkBoxIntensityLevel";
            this.checkBoxIntensityLevel.Size = new Size(136, 17);
            this.checkBoxIntensityLevel.TabIndex = 4;
            this.checkBoxIntensityLevel.Text = "Vary Intensity Levels:";
            this.checkBoxIntensityLevel.TextAlign = ContentAlignment.MiddleRight;
            this.checkBoxIntensityLevel.UseVisualStyleBackColor = true;
            this.checkBoxIntensityLevel.CheckedChanged += new EventHandler(this.checkBoxIntensityLevel_CheckedChanged);
            // 
            // lblPeriodLen
            // 
            this.lblPeriodLen.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.lblPeriodLen.Location = new Point(12, 60);
            this.lblPeriodLen.Margin = new Padding(3);
            this.lblPeriodLen.Name = "lblPeriodLen";
            this.lblPeriodLen.Size = new Size(136, 17);
            this.lblPeriodLen.TabIndex = 10;
            this.lblPeriodLen.Text = "Period Length:";
            this.lblPeriodLen.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pbExample
            // 
            this.pbExample.BackColor = Color.Silver;
            this.pbExample.Location = new Point(11, 198);
            this.pbExample.Name = "pbExample";
            this.pbExample.Size = new Size(271, 271);
            this.pbExample.TabIndex = 9;
            this.pbExample.TabStop = false;
            this.pbExample.Paint += new PaintEventHandler(this.pbExample_Paint);
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new Point(12, 174);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new Size(50, 13);
            this.lblExample.TabIndex = 0;
            this.lblExample.Text = "Example:";
            // 
            // RandomParametersDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(294, 481);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.pbExample);
            this.Controls.Add(this.lblPeriodLen);
            this.Controls.Add(this.udPeriods);
            this.Controls.Add(this.checkBoxUseSaturation);
            this.Controls.Add(this.udSaturation);
            this.Controls.Add(this.lblAnd);
            this.Controls.Add(this.lblPctSaturation);
            this.Controls.Add(this.lblPctMax);
            this.Controls.Add(this.udMax);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblPctMin);
            this.Controls.Add(this.udMin);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.lblBetween);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxIntensityLevel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RandomParametersDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Random Settings";
            ((ISupportInitialize)(this.udSaturation)).EndInit();
            ((ISupportInitialize)(this.udPeriods)).EndInit();
            ((ISupportInitialize)(this.udMax)).EndInit();
            ((ISupportInitialize)(this.udMin)).EndInit();
            ((ISupportInitialize)(this.pbExample)).EndInit();
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

        private Label lblPeriodLen;
        private PictureBox pbExample;
        private Label lblExample;
    }
}