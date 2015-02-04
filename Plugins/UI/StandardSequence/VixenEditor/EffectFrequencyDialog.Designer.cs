using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class EffectFrequencyDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private GroupBox groupBox1;
        private MethodInvoker m_refreshInvoker;
        private PictureBox pictureBoxExample;
        private TrackBar trackBarFrequency;

        private void InitializeComponent() {
            this.groupBox1 = new GroupBox();
            this.lblValue = new Label();
            this.pictureBoxExample = new PictureBox();
            this.trackBarFrequency = new TrackBar();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxExample)).BeginInit();
            ((ISupportInitialize)(this.trackBarFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblValue);
            this.groupBox1.Controls.Add(this.pictureBoxExample);
            this.groupBox1.Controls.Add(this.trackBarFrequency);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(268, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // lblValue
            // 
            this.lblValue.Location = new Point(12, 142);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new Size(45, 13);
            this.lblValue.TabIndex = 2;
            this.lblValue.Text = "Freq";
            this.lblValue.TextAlign = ContentAlignment.TopCenter;
            // 
            // pictureBoxExample
            // 
            this.pictureBoxExample.BackColor = Color.Black;
            this.pictureBoxExample.Location = new Point(74, 19);
            this.pictureBoxExample.Name = "pictureBoxExample";
            this.pictureBoxExample.Size = new Size(179, 137);
            this.pictureBoxExample.TabIndex = 1;
            this.pictureBoxExample.TabStop = false;
            this.pictureBoxExample.Paint += new PaintEventHandler(this.pictureBoxExample_Paint);
            // 
            // trackBarFrequency
            // 
            this.trackBarFrequency.Location = new Point(12, 19);
            this.trackBarFrequency.Name = "trackBarFrequency";
            this.trackBarFrequency.Orientation = Orientation.Vertical;
            this.trackBarFrequency.Size = new Size(45, 116);
            this.trackBarFrequency.TabIndex = 0;
            this.trackBarFrequency.TickStyle = TickStyle.Both;
            this.trackBarFrequency.Scroll += new EventHandler(this.trackBarFrequency_Scroll);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(124, 186);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(205, 186);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // EffectFrequencyDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(292, 217);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "EffectFrequencyDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "EffectFrequencyDialog";
            this.FormClosing += new FormClosingEventHandler(this.EffectFrequencyDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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

        private Label lblValue;
    }
}