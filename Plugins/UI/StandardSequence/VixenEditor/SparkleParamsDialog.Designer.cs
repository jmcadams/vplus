using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class SparkleParamsDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private MethodInvoker m_refreshInvoker;
        private NumericUpDown udMax;
        private NumericUpDown udMin;
        private PictureBox pictureBoxExample;
        private TrackBar trackBarDecay;
        private TrackBar trackBarFrequency;

        private void InitializeComponent() {
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.groupBox1 = new GroupBox();
            this.udMax = new NumericUpDown();
            this.label4 = new Label();
            this.udMin = new NumericUpDown();
            this.label3 = new Label();
            this.trackBarDecay = new TrackBar();
            this.label2 = new Label();
            this.label1 = new Label();
            this.pictureBoxExample = new PictureBox();
            this.trackBarFrequency = new TrackBar();
            this.lblFreq = new Label();
            this.lblDecay = new Label();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)(this.udMax)).BeginInit();
            ((ISupportInitialize)(this.udMin)).BeginInit();
            ((ISupportInitialize)(this.trackBarDecay)).BeginInit();
            ((ISupportInitialize)(this.pictureBoxExample)).BeginInit();
            ((ISupportInitialize)(this.trackBarFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(293, 279);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(212, 279);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblDecay);
            this.groupBox1.Controls.Add(this.lblFreq);
            this.groupBox1.Controls.Add(this.udMax);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.udMin);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.trackBarDecay);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pictureBoxExample);
            this.groupBox1.Controls.Add(this.trackBarFrequency);
            this.groupBox1.Location = new Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(356, 263);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sparkle Parameters";
            // 
            // udMax
            // 
            this.udMax.Location = new Point(276, 121);
            this.udMax.Name = "udMax";
            this.udMax.Size = new Size(52, 20);
            this.udMax.TabIndex = 8;
            this.udMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udMax.ValueChanged += new EventHandler(this.numericUpDownMax_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(273, 105);
            this.label4.Name = "label4";
            this.label4.Size = new Size(51, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Maximum";
            // 
            // udMin
            // 
            this.udMin.Location = new Point(276, 66);
            this.udMin.Name = "udMin";
            this.udMin.Size = new Size(52, 20);
            this.udMin.TabIndex = 6;
            this.udMin.ValueChanged += new EventHandler(this.numericUpDownMin_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(273, 50);
            this.label3.Name = "label3";
            this.label3.Size = new Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Minimum";
            // 
            // trackBarDecay
            // 
            this.trackBarDecay.LargeChange = 1000;
            this.trackBarDecay.Location = new Point(73, 194);
            this.trackBarDecay.Maximum = 5000;
            this.trackBarDecay.Name = "trackBarDecay";
            this.trackBarDecay.Size = new Size(197, 45);
            this.trackBarDecay.SmallChange = 500;
            this.trackBarDecay.TabIndex = 4;
            this.trackBarDecay.TickFrequency = 500;
            this.trackBarDecay.TickStyle = TickStyle.Both;
            this.trackBarDecay.Scroll += new EventHandler(this.trackBarDecay_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(79, 179);
            this.label2.Name = "label2";
            this.label2.Size = new Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Decay time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Frequency";
            // 
            // pictureBoxExample
            // 
            this.pictureBoxExample.BackColor = Color.Black;
            this.pictureBoxExample.Location = new Point(82, 39);
            this.pictureBoxExample.Name = "pictureBoxExample";
            this.pictureBoxExample.Size = new Size(179, 137);
            this.pictureBoxExample.TabIndex = 1;
            this.pictureBoxExample.TabStop = false;
            this.pictureBoxExample.Paint += new PaintEventHandler(this.pictureBoxExample_Paint);
            // 
            // trackBarFrequency
            // 
            this.trackBarFrequency.Location = new Point(20, 54);
            this.trackBarFrequency.Name = "trackBarFrequency";
            this.trackBarFrequency.Orientation = Orientation.Vertical;
            this.trackBarFrequency.Size = new Size(45, 132);
            this.trackBarFrequency.TabIndex = 0;
            this.trackBarFrequency.TickStyle = TickStyle.Both;
            this.trackBarFrequency.Scroll += new EventHandler(this.trackBarFrequency_Scroll);
            // 
            // lblFreq
            // 
            this.lblFreq.Location = new Point(20, 193);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new Size(45, 19);
            this.lblFreq.TabIndex = 9;
            this.lblFreq.Text = "Freq";
            this.lblFreq.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblDecay
            // 
            this.lblDecay.Location = new Point(277, 207);
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new Size(51, 17);
            this.lblDecay.TabIndex = 10;
            this.lblDecay.Text = "Decay";
            // 
            // SparkleParamsDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(380, 312);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "SparkleParamsDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sparkle";
            this.FormClosing += new FormClosingEventHandler(this.SparkleParamsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize)(this.udMax)).EndInit();
            ((ISupportInitialize)(this.udMin)).EndInit();
            ((ISupportInitialize)(this.trackBarDecay)).EndInit();
            ((ISupportInitialize)(this.pictureBoxExample)).EndInit();
            ((ISupportInitialize)(this.trackBarFrequency)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Label lblDecay;
        private Label lblFreq;
    }
}