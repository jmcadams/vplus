using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	internal partial class ChannelCopyDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCopy;
		private Button buttonDone;
		private ComboBox comboBoxDestinationChannel;
		private ComboBox comboBoxSourceChannel;
		private Label label1;
		private Label label2;

		private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSourceChannel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDestinationChannel = new System.Windows.Forms.ComboBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source channel";
            // 
            // comboBoxSourceChannel
            // 
            this.comboBoxSourceChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSourceChannel.FormattingEnabled = true;
            this.comboBoxSourceChannel.Location = new System.Drawing.Point(121, 6);
            this.comboBoxSourceChannel.Name = "comboBoxSourceChannel";
            this.comboBoxSourceChannel.Size = new System.Drawing.Size(159, 21);
            this.comboBoxSourceChannel.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destination channel";
            // 
            // comboBoxDestinationChannel
            // 
            this.comboBoxDestinationChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDestinationChannel.FormattingEnabled = true;
            this.comboBoxDestinationChannel.Location = new System.Drawing.Point(121, 32);
            this.comboBoxDestinationChannel.Name = "comboBoxDestinationChannel";
            this.comboBoxDestinationChannel.Size = new System.Drawing.Size(159, 21);
            this.comboBoxDestinationChannel.TabIndex = 3;
            // 
            // buttonCopy
            // 
            this.buttonCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCopy.Location = new System.Drawing.Point(205, 59);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 4;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonDone.Location = new System.Drawing.Point(17, 59);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 5;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // ChannelCopyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(292, 90);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.comboBoxDestinationChannel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxSourceChannel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelCopyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Channel Copy";
            this.TopMost = true;
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