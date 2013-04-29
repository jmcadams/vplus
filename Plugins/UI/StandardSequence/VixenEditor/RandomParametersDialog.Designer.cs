namespace VixenEditor {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;

    internal partial class RandomParametersDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxIntensityLevel;
        private CheckBox checkBoxUseSaturation;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label lblPctSaturation;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lblPctMin;
        private Label lblPctMax;
        private Label label9;
        private NumericUpDown udMax;
        private NumericUpDown udMin;
        private NumericUpDown udPeriods;
        private NumericUpDown udSaturation;

        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.udSaturation = new System.Windows.Forms.NumericUpDown();
            this.lblPctSaturation = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseSaturation = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.udPeriods = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblPctMax = new System.Windows.Forms.Label();
            this.udMax = new System.Windows.Forms.NumericUpDown();
            this.lblPctMin = new System.Windows.Forms.Label();
            this.udMin = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxIntensityLevel = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.udSaturation)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPeriods)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMin)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 55);
            this.label1.TabIndex = 1;
            this.label1.Text = "Adjust the saturation level to determine how many channels are on during a given " +
    "event period.  The default is 50%, meaning half of the channels will be on.";
            // 
            // udSaturation
            // 
            this.udSaturation.Location = new System.Drawing.Point(98, 94);
            this.udSaturation.Name = "udSaturation";
            this.udSaturation.Size = new System.Drawing.Size(46, 20);
            this.udSaturation.TabIndex = 2;
            this.udSaturation.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udSaturation.Enter += new System.EventHandler(this.UpDownEnter);
            // 
            // lblPctSaturation
            // 
            this.lblPctSaturation.AutoSize = true;
            this.lblPctSaturation.Location = new System.Drawing.Point(148, 96);
            this.lblPctSaturation.Name = "lblPctSaturation";
            this.lblPctSaturation.Size = new System.Drawing.Size(15, 13);
            this.lblPctSaturation.TabIndex = 3;
            this.lblPctSaturation.Text = "%";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(124, 433);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(205, 433);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxUseSaturation);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.udSaturation);
            this.groupBox1.Controls.Add(this.lblPctSaturation);
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // checkBoxUseSaturation
            // 
            this.checkBoxUseSaturation.ForeColor = System.Drawing.Color.FromArgb(0, 70, 213);
            this.checkBoxUseSaturation.Location = new System.Drawing.Point(8, -4);
            this.checkBoxUseSaturation.Name = "checkBoxUseSaturation";
            this.checkBoxUseSaturation.Size = new System.Drawing.Size(110, 24);
            this.checkBoxUseSaturation.TabIndex = 0;
            this.checkBoxUseSaturation.Text = "Ensure Saturation";
            this.checkBoxUseSaturation.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 48);
            this.label3.TabIndex = 0;
            this.label3.Text = "If you want a truly random result with no intensity variation, you can press Ente" +
    "r or click OK right now.  Or you can adjust the parameters below.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.udPeriods);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 108);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Period length";
            // 
            // udPeriods
            // 
            this.udPeriods.Location = new System.Drawing.Point(98, 67);
            this.udPeriods.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPeriods.Name = "udPeriods";
            this.udPeriods.Size = new System.Drawing.Size(46, 20);
            this.udPeriods.TabIndex = 1;
            this.udPeriods.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPeriods.Enter += new System.EventHandler(this.UpDownEnter);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 33);
            this.label4.TabIndex = 0;
            this.label4.Text = "How many event periods would you like each \'on\' event to last?";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.lblPctMax);
            this.groupBox3.Controls.Add(this.udMax);
            this.groupBox3.Controls.Add(this.lblPctMin);
            this.groupBox3.Controls.Add(this.udMin);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkBoxIntensityLevel);
            this.groupBox3.Location = new System.Drawing.Point(12, 317);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 110);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Intensity levels";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(125, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "and";
            // 
            // lblPctMax
            // 
            this.lblPctMax.AutoSize = true;
            this.lblPctMax.Location = new System.Drawing.Point(199, 75);
            this.lblPctMax.Name = "lblPctMax";
            this.lblPctMax.Size = new System.Drawing.Size(15, 13);
            this.lblPctMax.TabIndex = 7;
            this.lblPctMax.Text = "%";
            // 
            // udMax
            // 
            this.udMax.Location = new System.Drawing.Point(159, 73);
            this.udMax.Name = "udMax";
            this.udMax.Size = new System.Drawing.Size(40, 20);
            this.udMax.TabIndex = 6;
            this.udMax.Enter += new System.EventHandler(this.UpDownEnter);
            // 
            // lblPctMin
            // 
            this.lblPctMin.AutoSize = true;
            this.lblPctMin.Location = new System.Drawing.Point(111, 75);
            this.lblPctMin.Name = "lblPctMin";
            this.lblPctMin.Size = new System.Drawing.Size(15, 13);
            this.lblPctMin.TabIndex = 4;
            this.lblPctMin.Text = "%";
            // 
            // udMin
            // 
            this.udMin.Location = new System.Drawing.Point(71, 73);
            this.udMin.Name = "udMin";
            this.udMin.Size = new System.Drawing.Size(40, 20);
            this.udMin.TabIndex = 3;
            this.udMin.Enter += new System.EventHandler(this.UpDownEnter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Between";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(236, 33);
            this.label5.TabIndex = 1;
            this.label5.Text = "Randomly adjust the intensity level of the activated cells within a specified ran" +
    "ge.";
            // 
            // checkBoxIntensityLevel
            // 
            this.checkBoxIntensityLevel.AutoSize = true;
            this.checkBoxIntensityLevel.ForeColor = System.Drawing.Color.FromArgb(0, 70, 213);
            this.checkBoxIntensityLevel.Location = new System.Drawing.Point(8, 0);
            this.checkBoxIntensityLevel.Name = "checkBoxIntensityLevel";
            this.checkBoxIntensityLevel.Size = new System.Drawing.Size(123, 17);
            this.checkBoxIntensityLevel.TabIndex = 0;
            this.checkBoxIntensityLevel.Text = "Vary Intensity Levels";
            this.checkBoxIntensityLevel.UseVisualStyleBackColor = true;
            // 
            // RandomParametersDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(292, 468);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RandomParametersDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Random Parameters";
            ((System.ComponentModel.ISupportInitialize)(this.udSaturation)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udPeriods)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMin)).EndInit();
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