namespace Olsen595 {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class SetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
        private Button buttonOK;
		private GroupBox groupBox1;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private TextBox textBoxParallel1From;
		private TextBox textBoxParallel1To;
		private TextBox textBoxParallel2From;
		private TextBox textBoxParallel2To;
		private TextBox textBoxParallel3From;
		private TextBox textBoxParallel3To;

		private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxParallel3To = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxParallel3From = new System.Windows.Forms.TextBox();
            this.textBoxParallel2To = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxParallel2From = new System.Windows.Forms.TextBox();
            this.textBoxParallel1To = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxParallel1From = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxParallel3To);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxParallel3From);
            this.groupBox1.Controls.Add(this.textBoxParallel2To);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxParallel2From);
            this.groupBox1.Controls.Add(this.textBoxParallel1To);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxParallel1From);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port mappings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Parallel 3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Parallel 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Parallel 1";
            // 
            // textBoxParallel3To
            // 
            this.textBoxParallel3To.Location = new System.Drawing.Point(216, 117);
            this.textBoxParallel3To.Name = "textBoxParallel3To";
            this.textBoxParallel3To.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel3To.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "to";
            // 
            // textBoxParallel3From
            // 
            this.textBoxParallel3From.Location = new System.Drawing.Point(148, 117);
            this.textBoxParallel3From.Name = "textBoxParallel3From";
            this.textBoxParallel3From.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel3From.TabIndex = 10;
            // 
            // textBoxParallel2To
            // 
            this.textBoxParallel2To.Location = new System.Drawing.Point(216, 80);
            this.textBoxParallel2To.Name = "textBoxParallel2To";
            this.textBoxParallel2To.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel2To.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "to";
            // 
            // textBoxParallel2From
            // 
            this.textBoxParallel2From.Location = new System.Drawing.Point(148, 80);
            this.textBoxParallel2From.Name = "textBoxParallel2From";
            this.textBoxParallel2From.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel2From.TabIndex = 6;
            // 
            // textBoxParallel1To
            // 
            this.textBoxParallel1To.Location = new System.Drawing.Point(216, 43);
            this.textBoxParallel1To.Name = "textBoxParallel1To";
            this.textBoxParallel1To.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel1To.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "to";
            // 
            // textBoxParallel1From
            // 
            this.textBoxParallel1From.Location = new System.Drawing.Point(148, 43);
            this.textBoxParallel1From.Name = "textBoxParallel1From";
            this.textBoxParallel1From.Size = new System.Drawing.Size(40, 20);
            this.textBoxParallel1From.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Channel Range";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(136, 192);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(217, 192);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // SetupDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(302, 227);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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