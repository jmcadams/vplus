namespace VixenPlus.Dialogs{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;

    public partial class ChannelOutputMaskDialog{
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
private Button buttonOK;
private CheckedListBox checkedListBoxChannels;
private Label label1;

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.checkedListBoxChannels = new CheckedListBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(238, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select which channels will be enabled for output.";
            this.checkedListBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.checkedListBoxChannels.CheckOnClick = true;
            this.checkedListBoxChannels.FormattingEnabled = true;
            this.checkedListBoxChannels.Location = new Point(21, 47);
            this.checkedListBoxChannels.Name = "checkedListBoxChannels";
            this.checkedListBoxChannels.Size = new Size(249, 289);
            this.checkedListBoxChannels.TabIndex = 1;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(124, 354);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(205, 354);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(292, 389);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.checkedListBoxChannels);
            base.Controls.Add(this.label1);
            base.Name = "ChannelOutputMaskDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Output Mask";
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
        }
    }
}
