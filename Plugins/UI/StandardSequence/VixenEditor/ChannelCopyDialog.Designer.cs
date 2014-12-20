using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

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
            this.label1 = new Label();
            this.comboBoxSourceChannel = new ComboBox();
            this.label2 = new Label();
            this.comboBoxDestinationChannel = new ComboBox();
            this.buttonCopy = new Button();
            this.buttonDone = new Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(33, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source channel";
            // 
            // comboBoxSourceChannel
            // 
            this.comboBoxSourceChannel.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxSourceChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSourceChannel.FormattingEnabled = true;
            this.comboBoxSourceChannel.Location = new Point(121, 6);
            this.comboBoxSourceChannel.Name = "comboBoxSourceChannel";
            this.comboBoxSourceChannel.Size = new Size(159, 21);
            this.comboBoxSourceChannel.TabIndex = 1;
            this.comboBoxSourceChannel.DrawItem += new DrawItemEventHandler(this.comboBox_DrawItem);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(14, 35);
            this.label2.Name = "label2";
            this.label2.Size = new Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destination channel";
            // 
            // comboBoxDestinationChannel
            // 
            this.comboBoxDestinationChannel.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBoxDestinationChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestinationChannel.FormattingEnabled = true;
            this.comboBoxDestinationChannel.Location = new Point(121, 32);
            this.comboBoxDestinationChannel.Name = "comboBoxDestinationChannel";
            this.comboBoxDestinationChannel.Size = new Size(159, 21);
            this.comboBoxDestinationChannel.TabIndex = 3;
            this.comboBoxDestinationChannel.DrawItem += new DrawItemEventHandler(this.comboBox_DrawItem);
            // 
            // buttonCopy
            // 
            this.buttonCopy.DialogResult = DialogResult.OK;
            this.buttonCopy.Location = new Point(205, 59);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(75, 23);
            this.buttonCopy.TabIndex = 4;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.DialogResult = DialogResult.Cancel;
            this.buttonDone.Location = new Point(17, 59);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(75, 23);
            this.buttonDone.TabIndex = 5;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
            // 
            // ChannelCopyDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new Size(292, 90);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.comboBoxDestinationChannel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxSourceChannel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelCopyDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
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