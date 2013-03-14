namespace Spectrum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class AutoMapDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private ComboBox comboBoxStartBand;
        private ComboBox comboBoxStartChannel;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;

        public AutoMapDialog(List<Channel> channels, List<FrequencyBand> bands)
        {
            this.InitializeComponent();
            this.comboBoxStartChannel.Items.AddRange(channels.ToArray());
            this.comboBoxStartBand.Items.AddRange(bands.ToArray());
            this.comboBoxStartChannel.SelectedIndex = 0;
            this.comboBoxStartBand.SelectedIndex = 0;
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
            this.groupBox1 = new GroupBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.label1 = new Label();
            this.comboBoxStartChannel = new ComboBox();
            this.label2 = new Label();
            this.comboBoxStartBand = new ComboBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.comboBoxStartBand);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxStartChannel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10c, 0x73);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Map Parameters";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0x85);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0x85);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0x22);
            this.label1.Name = "label1";
            this.label1.Size = new Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start channel";
            this.comboBoxStartChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStartChannel.FormattingEnabled = true;
            this.comboBoxStartChannel.Location = new Point(0x52, 0x1f);
            this.comboBoxStartChannel.Name = "comboBoxStartChannel";
            this.comboBoxStartChannel.Size = new Size(180, 0x15);
            this.comboBoxStartChannel.TabIndex = 1;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x45);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start band";
            this.comboBoxStartBand.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStartBand.FormattingEnabled = true;
            this.comboBoxStartBand.Location = new Point(0x52, 0x42);
            this.comboBoxStartBand.Name = "comboBoxStartBand";
            this.comboBoxStartBand.Size = new Size(180, 0x15);
            this.comboBoxStartBand.TabIndex = 3;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0xa8);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "AutoMapDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Auto Map";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public int StartBandIndex
        {
            get
            {
                return this.comboBoxStartBand.SelectedIndex;
            }
        }

        public int StartChannelIndex
        {
            get
            {
                return this.comboBoxStartChannel.SelectedIndex;
            }
        }
    }
}

