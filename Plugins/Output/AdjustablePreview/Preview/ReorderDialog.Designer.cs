using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon;
using VixenPlusCommon.Properties;

namespace Preview {
    internal partial class ReorderDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonClear;
        private Button buttonCopy;
        private Button buttonOK;
        private ComboBox comboBoxFrom;
        private ComboBox comboBoxTo;
        private Label label1;
        private Label label2;
        private Label label3;


        private void InitializeComponent() {
            this.components = new Container();
            this.label1 = new Label();
            this.label2 = new Label();
            this.comboBoxFrom = new ComboBox();
            this.label3 = new Label();
            this.comboBoxTo = new ComboBox();
            this.buttonClear = new Button();
            this.buttonCopy = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.timerFade = new Timer(this.components);
            this.lblChannelCopied = new FadableLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(80, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(241, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy the cells drawn from one channel to another";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new Size(30, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From";
            // 
            // comboBoxFrom
            // 
            this.comboBoxFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new Point(48, 39);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new Size(205, 21);
            this.comboBoxFrom.TabIndex = 2;
            this.comboBoxFrom.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(22, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(20, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "To";
            // 
            // comboBoxTo
            // 
            this.comboBoxTo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new Point(48, 77);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new Size(205, 21);
            this.comboBoxTo.TabIndex = 4;
            this.comboBoxTo.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new Point(259, 75);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(117, 23);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Clear this channel";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new Point(259, 37);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(117, 23);
            this.buttonCopy.TabIndex = 6;
            this.buttonCopy.Text = "Copy this channel";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(220, 111);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(301, 111);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // timerFade
            // 
            this.timerFade.Interval = 2000;
            this.timerFade.Tick += new EventHandler(this.timerFade_Tick);
            // 
            // lblChannelCopied
            // 
            this.lblChannelCopied.AutoSize = true;
            this.lblChannelCopied.Location = new Point(12, 116);
            this.lblChannelCopied.Name = "lblChannelCopied";
            this.lblChannelCopied.Size = new Size(82, 13);
            this.lblChannelCopied.TabIndex = 11;
            this.lblChannelCopied.Text = "Channel Copied";
            // 
            // ReorderDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(388, 144);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblChannelCopied);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.comboBoxFrom);
            this.Controls.Add(this.comboBoxTo);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReorderDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Copy";
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


        private Timer timerFade;
        private FadableLabel lblChannelCopied;
    }
}
