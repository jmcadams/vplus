using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Keyboard
{
	internal partial class SetupDialog {

		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private Label label1;
private Label label2;
private Label label3;
private Label labelKeyNotAvailable;
private PictureBox pictureBox1;
private PictureBox pictureBox2;
private PictureBox pictureBox3;
private PictureBox pictureBoxKeyboard;
private Timer timerPulse;

		private void InitializeComponent()
        {
            this.components = new Container();
			this.pictureBoxKeyboard = new PictureBox();
            this.pictureBox1 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.labelKeyNotAvailable = new Label();
            this.timerPulse = new Timer(this.components);
            this.label3 = new Label();
            this.pictureBox3 = new PictureBox();
            ((ISupportInitialize) this.pictureBoxKeyboard).BeginInit();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            ((ISupportInitialize) this.pictureBox2).BeginInit();
            ((ISupportInitialize) this.pictureBox3).BeginInit();
            base.SuspendLayout();
            this.pictureBoxKeyboard.Location = new Point(0, 0);
            this.pictureBoxKeyboard.Name = "pictureBoxKeyboard";
            this.pictureBoxKeyboard.Size = new Size(900, 0x120);
            this.pictureBoxKeyboard.TabIndex = 0;
            this.pictureBoxKeyboard.TabStop = false;
            this.pictureBoxKeyboard.Paint += new PaintEventHandler(this.pictureBoxKeyboard_Paint);
            this.pictureBox1.BackColor = Color.Red;
            this.pictureBox1.Location = new Point(9, 0x126);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x10, 0x10);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox2.BackColor = Color.Blue;
            this.pictureBox2.Location = new Point(9, 0x13a);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(0x10, 0x10);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x26, 0x126);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Momentary input";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x26, 0x13a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4a, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Latching input";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x2d6, 0x141);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.TabStop = false;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x327, 0x141);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.TabStop = false;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.labelKeyNotAvailable.AutoSize = true;
            this.labelKeyNotAvailable.Font = new Font("Arial", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelKeyNotAvailable.Location = new Point(0x153, 0x13f);
            this.labelKeyNotAvailable.Name = "labelKeyNotAvailable";
            this.labelKeyNotAvailable.Size = new Size(0xd8, 22);
            this.labelKeyNotAvailable.TabIndex = 7;
            this.labelKeyNotAvailable.Text = "Key not available for use";
            this.labelKeyNotAvailable.Visible = false;
            this.timerPulse.Interval = 50;
            this.timerPulse.Tick += new EventHandler(this.timerPulse_Tick);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x26, 0x14e);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x81, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Available, not yet an input";
            this.pictureBox3.Location = new Point(9, 0x14e);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Size(0x10, 0x10);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Paint += new PaintEventHandler(this.pictureBox3_Paint);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x37e, 0x164);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.pictureBox3);
            base.Controls.Add(this.labelKeyNotAvailable);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.pictureBox2);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.pictureBoxKeyboard);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.Name = "SetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            base.KeyDown += new KeyEventHandler(this.SetupDialog_KeyDown);
            ((ISupportInitialize) this.pictureBoxKeyboard).EndInit();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            ((ISupportInitialize) this.pictureBox2).EndInit();
            ((ISupportInitialize) this.pictureBox3).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
            this.m_blackPen.Dispose();
            this.m_redPen.Dispose();
            this.m_bluePen.Dispose();
        }
	}
}
