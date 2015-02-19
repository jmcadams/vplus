using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    partial class GroupDialogMultiAdd {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GroupDialogMultiAdd));
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.lblName = new Label();
            this.tbName = new TextBox();
            this.nudCount = new NumericUpDown();
            this.label1 = new Label();
            this.textBox2 = new TextBox();
            ((ISupportInitialize)(this.nudCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(197, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(116, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(13, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(38, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new Point(57, 10);
            this.tbName.Name = "tbName";
            this.tbName.Size = new Size(215, 20);
            this.tbName.TabIndex = 0;
            // 
            // nudCount
            // 
            this.nudCount.Location = new Point(197, 38);
            this.nudCount.Name = "nudCount";
            this.nudCount.Size = new Size(75, 20);
            this.nudCount.TabIndex = 1;
            this.nudCount.Enter += new EventHandler(this.nudCount_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(142, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number of Groups to Create:";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new Point(13, 64);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(259, 157);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // GroupDialogMultiAdd
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudCount);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupDialogMultiAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "GroupDialogMultiAdd";
            this.FormClosing += new FormClosingEventHandler(this.GroupDialogMultiAdd_FormClosing);
            ((ISupportInitialize)(this.nudCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label lblName;
        private TextBox tbName;
        private NumericUpDown nudCount;
        private Label label1;
        private TextBox textBox2;
    }
}