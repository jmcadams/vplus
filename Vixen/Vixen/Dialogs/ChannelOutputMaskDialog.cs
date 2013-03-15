namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class ChannelOutputMaskDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private CheckedListBox checkedListBoxChannels;
        private IContainer components = null;
        private Label label1;

        public ChannelOutputMaskDialog(List<Channel> channels)
        {
            this.InitializeComponent();
            foreach (Channel channel in channels)
            {
                this.checkedListBoxChannels.Items.Add(channel, channel.Enabled);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.checkedListBoxChannels = new CheckedListBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xee, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select which channels will be enabled for output.";
            this.checkedListBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.checkedListBoxChannels.CheckOnClick = true;
            this.checkedListBoxChannels.FormattingEnabled = true;
            this.checkedListBoxChannels.Location = new Point(0x15, 0x2f);
            this.checkedListBoxChannels.Name = "checkedListBoxChannels";
            this.checkedListBoxChannels.Size = new Size(0xf9, 0x121);
            this.checkedListBoxChannels.TabIndex = 1;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0x162);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0x162);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0x185);
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

        public List<int> DisabledChannels
        {
            get
            {
                List<int> list = new List<int>();
                for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++)
                {
                    list.Add(i);
                }
                foreach (int num2 in this.checkedListBoxChannels.CheckedIndices)
                {
                    list.Remove(num2);
                }
                return list;
            }
        }
    }
}

