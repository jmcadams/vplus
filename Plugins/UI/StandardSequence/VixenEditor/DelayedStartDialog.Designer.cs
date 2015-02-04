using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    public partial class DelayedStartDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonStartStop;
        private Label label1;
        private NumericUpDown numericUpDownDelay;
        private Timer timer;

        private void InitializeComponent() {
            this.components = new Container();
            this.label1 = new Label();
            this.numericUpDownDelay = new NumericUpDown();
            this.buttonStartStop = new Button();
            this.buttonCancel = new Button();
            this.timer = new Timer(this.components);
            ((ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(172, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Delay start for how many seconds?";
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Location = new Point(190, 7);
            this.numericUpDownDelay.Maximum = 999;
            this.numericUpDownDelay.Minimum = 1;
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new Size(50, 20);
            this.numericUpDownDelay.TabIndex = 1;
            this.numericUpDownDelay.Value = 1;
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.ForeColor = SystemColors.ControlText;
            this.buttonStartStop.Location = new Point(165, 33);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new Size(75, 23);
            this.buttonStartStop.TabIndex = 2;
            this.buttonStartStop.Text = "Start";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new EventHandler(this.buttonStartStop_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(84, 33);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            // 
            // DelayedStartDialog
            // 
            this.AcceptButton = this.buttonStartStop;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(252, 66);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownDelay);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStartStop);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "DelayedStartDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Delayed Start";
            ((ISupportInitialize)(this.numericUpDownDelay)).EndInit();
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