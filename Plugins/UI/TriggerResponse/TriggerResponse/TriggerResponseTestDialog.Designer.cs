namespace TriggerResponse {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class TriggerResponseTestDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonTest;
		private ComboBox comboBoxTriggerResponses;
		private Label label1;
		private ITrigger m_triggerInterface;

		private void InitializeComponent() {
			this.comboBoxTriggerResponses = new ComboBox();
			this.buttonTest = new Button();
			this.label1 = new Label();
			base.SuspendLayout();
			this.comboBoxTriggerResponses.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTriggerResponses.FormattingEnabled = true;
			this.comboBoxTriggerResponses.Location = new Point(0x16, 0x2a);
			this.comboBoxTriggerResponses.Name = "comboBoxTriggerResponses";
			this.comboBoxTriggerResponses.Size = new Size(0xf4, 0x15);
			this.comboBoxTriggerResponses.TabIndex = 0;
			this.comboBoxTriggerResponses.SelectedIndexChanged += new EventHandler(this.comboBoxResponses_SelectedIndexChanged);
			this.buttonTest.Enabled = false;
			this.buttonTest.Location = new Point(0x6d, 0x45);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new Size(0x4b, 0x17);
			this.buttonTest.TabIndex = 1;
			this.buttonTest.Text = "Test";
			this.buttonTest.UseVisualStyleBackColor = true;
			this.buttonTest.Click += new EventHandler(this.buttonTest_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x13, 0x16);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x76, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Triggers with responses";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x124, 0x72);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.buttonTest);
			base.Controls.Add(this.comboBoxTriggerResponses);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.KeyPreview = true;
			base.Name = "TriggerResponseTestDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Trigger Response Test";
			base.FormClosing += new FormClosingEventHandler(this.ResponseTestDialog_FormClosing);
			base.KeyDown += new KeyEventHandler(this.ResponseTestDialog_KeyDown);
			base.ResumeLayout(false);
			base.PerformLayout();
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