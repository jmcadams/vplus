using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	public partial class DelayedStartDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonStartStop;
		private GroupBox groupBox1;
		private Label label1;
		private Label labelCountdown;
		private NumericUpDown numericUpDownDelay;
		private Timer timer;

		private void InitializeComponent() {
			this.components = new Container();
			this.label1 = new Label();
			this.numericUpDownDelay = new NumericUpDown();
			this.buttonStartStop = new Button();
			this.buttonCancel = new Button();
			this.labelCountdown = new Label();
			this.timer = new Timer(this.components);
			this.groupBox1 = new GroupBox();
			this.numericUpDownDelay.BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(6, 0x1a);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x110, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "How many seconds would you like to delay the start for?";
			this.numericUpDownDelay.Location = new Point(0x125, 0x18);
			int[] bitsMax = new int[4];
			bitsMax[0] = 0x3e7;
			this.numericUpDownDelay.Maximum = new decimal(bitsMax);
			int[] bitsMin = new int[4];
			bitsMin[0] = 1;
			this.numericUpDownDelay.Minimum = new decimal(bitsMin);
			this.numericUpDownDelay.Name = "numericUpDownDelay";
			this.numericUpDownDelay.Size = new Size(50, 20);
			this.numericUpDownDelay.TabIndex = 1;
			int[] bitsUpDown = new int[4];
			bitsUpDown[0] = 1;
			this.numericUpDownDelay.Value = new decimal(bitsUpDown);
			this.buttonStartStop.ForeColor = SystemColors.ControlText;
			this.buttonStartStop.Location = new Point(0x8b, 0x3b);
			this.buttonStartStop.Name = "buttonStartStop";
			this.buttonStartStop.Size = new Size(0x4b, 0x17);
			this.buttonStartStop.TabIndex = 2;
			this.buttonStartStop.Text = "Start";
			this.buttonStartStop.UseVisualStyleBackColor = true;
			this.buttonStartStop.Click += new EventHandler(this.buttonStartStop_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x128, 120);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.labelCountdown.AutoSize = true;
			this.labelCountdown.Font = new Font("Arial", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.labelCountdown.Location = new Point(0xb0, 0x5b);
			this.labelCountdown.Name = "labelCountdown";
			this.labelCountdown.Size = new Size(0, 0x1d);
			this.labelCountdown.TabIndex = 3;
			this.timer.Interval = 0x3e8;
			this.timer.Tick += new EventHandler(this.timer_Tick);
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.labelCountdown);
			this.groupBox1.Controls.Add(this.numericUpDownDelay);
			this.groupBox1.Controls.Add(this.buttonStartStop);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x167, 0x5e);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Delay";
			base.AcceptButton = this.buttonStartStop;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x17f, 0x9b);
			base.ControlBox = false;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "DelayedStartDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Delayed Start";
			base.Load += new EventHandler(this.DelayedStartDialog_Load);
			this.numericUpDownDelay.EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
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