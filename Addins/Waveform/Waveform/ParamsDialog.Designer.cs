using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Waveform {
    public partial class ParamsDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonStart;
        private CheckBox checkBoxAutoScale;
        private GroupBox groupBox1;
        private Label label1;
        private Label labelScale;
        private ListBox listBoxChannels;
        private MethodInvoker m_stepInvoker;
        private ProgressBar progressBar1;
        private RadioButton radioButtonChannelRange;
        private RadioButton radioButtonSingleChannel;
        private TextBox textBoxScale;

        private void InitializeComponent() {
            this.groupBox1 = new GroupBox();
            this.textBoxScale = new TextBox();
            this.labelScale = new Label();
            this.radioButtonChannelRange = new RadioButton();
            this.radioButtonSingleChannel = new RadioButton();
            this.listBoxChannels = new ListBox();
            this.buttonStart = new Button();
            this.progressBar1 = new ProgressBar();
            this.label1 = new Label();
            this.checkBoxAutoScale = new CheckBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.checkBoxAutoScale);
            this.groupBox1.Controls.Add(this.textBoxScale);
            this.groupBox1.Controls.Add(this.labelScale);
            this.groupBox1.Controls.Add(this.radioButtonChannelRange);
            this.groupBox1.Controls.Add(this.radioButtonSingleChannel);
            this.groupBox1.Location = new Point(10, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xef, 0x89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Waveform representation";
            this.textBoxScale.Location = new Point(110, 0x4a);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new Size(0x34, 20);
            this.textBoxScale.TabIndex = 3;
            this.textBoxScale.Text = "1.0";
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new Point(40, 0x4d);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new Size(0x40, 13);
            this.labelScale.TabIndex = 2;
            this.labelScale.Text = "Scale factor";
            this.radioButtonChannelRange.AutoSize = true;
            this.radioButtonChannelRange.Checked = true;
            this.radioButtonChannelRange.Location = new Point(0x19, 0x13);
            this.radioButtonChannelRange.Name = "radioButtonChannelRange";
            this.radioButtonChannelRange.Size = new Size(0xc4, 0x11);
            this.radioButtonChannelRange.TabIndex = 0;
            this.radioButtonChannelRange.TabStop = true;
            this.radioButtonChannelRange.Text = "On/Off values over a channel range";
            this.radioButtonChannelRange.UseVisualStyleBackColor = true;
            this.radioButtonChannelRange.CheckedChanged += new EventHandler(this.radioButtonChannelRange_CheckedChanged);
            this.radioButtonSingleChannel.AutoSize = true;
            this.radioButtonSingleChannel.Location = new Point(0x19, 0x2a);
            this.radioButtonSingleChannel.Name = "radioButtonSingleChannel";
            this.radioButtonSingleChannel.Size = new Size(0xbc, 0x11);
            this.radioButtonSingleChannel.TabIndex = 1;
            this.radioButtonSingleChannel.Text = "Dimmed values in a single channel";
            this.radioButtonSingleChannel.UseVisualStyleBackColor = true;
            this.radioButtonSingleChannel.CheckedChanged += new EventHandler(this.radioButtonSingleChannel_CheckedChanged);
            this.listBoxChannels.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(0x110, 0x17);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(0xac, 0xc7);
            this.listBoxChannels.TabIndex = 2;
            this.buttonStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonStart.Location = new Point(12, 0x9d);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(0x4b, 0x17);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
            this.progressBar1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.progressBar1.Location = new Point(10, 0xc7);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0xef, 0x17);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Visible = false;
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0xb7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0, 13);
            this.label1.TabIndex = 4;
            this.checkBoxAutoScale.AutoSize = true;
            this.checkBoxAutoScale.Location = new Point(0x2b, 0x67);
            this.checkBoxAutoScale.Name = "checkBoxAutoScale";
            this.checkBoxAutoScale.Size = new Size(0x75, 0x11);
            this.checkBoxAutoScale.TabIndex = 4;
            this.checkBoxAutoScale.Text = "Auto-scale to 100%";
            this.checkBoxAutoScale.UseVisualStyleBackColor = true;
            this.checkBoxAutoScale.CheckedChanged += new EventHandler(this.checkBoxAutoScale_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1cf, 0xf2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.progressBar1);
            base.Controls.Add(this.buttonStart);
            base.Controls.Add(this.listBoxChannels);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ParamsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Waveform";
            base.FormClosing += new FormClosingEventHandler(this.ParamsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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