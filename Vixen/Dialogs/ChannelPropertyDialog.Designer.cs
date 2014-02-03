using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public partial class ChannelPropertyDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonClose;
        private Button buttonColor;
        private Button buttonDimmingCurve;
        private Button buttonNext;
        private Button buttonPrev;
        private CheckBox checkBoxEnabled;
        private ColorDialog colorDialog;
        private ComboBox comboBoxChannels;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelOutputChannel;
        private TextBox textBoxName;


        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDimmingCurve = new System.Windows.Forms.Button();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.labelOutputChannel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.comboBoxChannels = new System.Windows.Forms.ComboBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonDimmingCurve);
            this.groupBox1.Controls.Add(this.checkBoxEnabled);
            this.groupBox1.Controls.Add(this.labelOutputChannel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonColor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 192);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel";
            // 
            // buttonDimmingCurve
            // 
            this.buttonDimmingCurve.Location = new System.Drawing.Point(17, 149);
            this.buttonDimmingCurve.Name = "buttonDimmingCurve";
            this.buttonDimmingCurve.Size = new System.Drawing.Size(106, 23);
            this.buttonDimmingCurve.TabIndex = 7;
            this.buttonDimmingCurve.Text = "Dimming Curve";
            this.buttonDimmingCurve.UseVisualStyleBackColor = true;
            this.buttonDimmingCurve.Click += new System.EventHandler(this.buttonDimmingCurve_Click);
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Location = new System.Drawing.Point(112, 105);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxEnabled.TabIndex = 6;
            this.checkBoxEnabled.Text = "Enabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            // 
            // labelOutputChannel
            // 
            this.labelOutputChannel.AutoSize = true;
            this.labelOutputChannel.Location = new System.Drawing.Point(109, 83);
            this.labelOutputChannel.Name = "labelOutputChannel";
            this.labelOutputChannel.Size = new System.Drawing.Size(100, 13);
            this.labelOutputChannel.TabIndex = 5;
            this.labelOutputChannel.Text = "labelOutputChannel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output channel:";
            // 
            // buttonColor
            // 
            this.buttonColor.Location = new System.Drawing.Point(112, 51);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(75, 23);
            this.buttonColor.TabIndex = 3;
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Color:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(112, 25);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(150, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(205, 239);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(12, 12);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(23, 23);
            this.buttonPrev.TabIndex = 0;
            this.buttonPrev.Text = "<";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(41, 12);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(23, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = ">";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // comboBoxChannels
            // 
            this.comboBoxChannels.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannels.FormattingEnabled = true;
            this.comboBoxChannels.Location = new System.Drawing.Point(70, 14);
            this.comboBoxChannels.Name = "comboBoxChannels";
            this.comboBoxChannels.Size = new System.Drawing.Size(198, 21);
            this.comboBoxChannels.TabIndex = 2;
            this.comboBoxChannels.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxChannels_DrawItem);
            this.comboBoxChannels.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannels_SelectedIndexChanged);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            // 
            // ChannelPropertyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(292, 274);
            this.Controls.Add(this.comboBoxChannels);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelPropertyDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Channel Properties";
            this.Load += new System.EventHandler(this.ChannelPropertyDialog_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChannelPropertyDialog_KeyPress);
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
