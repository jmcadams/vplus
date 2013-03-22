namespace Vixen.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class ChannelPropertyDialog{
		private IContainer components;

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

		private void InitializeComponent()
		{
			this.groupBox1 = new GroupBox();
			this.checkBoxEnabled = new CheckBox();
			this.labelOutputChannel = new Label();
			this.label3 = new Label();
			this.buttonColor = new Button();
			this.label2 = new Label();
			this.textBoxName = new TextBox();
			this.label1 = new Label();
			this.buttonClose = new Button();
			this.buttonPrev = new Button();
			this.buttonNext = new Button();
			this.comboBoxChannels = new ComboBox();
			this.colorDialog = new ColorDialog();
			this.buttonDimmingCurve = new Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonDimmingCurve);
			this.groupBox1.Controls.Add(this.checkBoxEnabled);
			this.groupBox1.Controls.Add(this.labelOutputChannel);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.buttonColor);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBoxName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(12, 0x29);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x10c, 0xc0);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Channel";
			this.checkBoxEnabled.AutoSize = true;
			this.checkBoxEnabled.Location = new Point(0x70, 0x69);
			this.checkBoxEnabled.Name = "checkBoxEnabled";
			this.checkBoxEnabled.Size = new Size(0x41, 0x11);
			this.checkBoxEnabled.TabIndex = 6;
			this.checkBoxEnabled.Text = "Enabled";
			this.checkBoxEnabled.UseVisualStyleBackColor = true;
			this.labelOutputChannel.AutoSize = true;
			this.labelOutputChannel.Location = new Point(0x6d, 0x53);
			this.labelOutputChannel.Name = "labelOutputChannel";
			this.labelOutputChannel.Size = new Size(100, 13);
			this.labelOutputChannel.TabIndex = 5;
			this.labelOutputChannel.Text = "labelOutputChannel";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(14, 0x53);
			this.label3.Name = "label3";
			this.label3.Size = new Size(0x53, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Output channel:";
			this.buttonColor.Location = new Point(0x70, 0x33);
			this.buttonColor.Name = "buttonColor";
			this.buttonColor.Size = new Size(0x4b, 0x17);
			this.buttonColor.TabIndex = 3;
			this.buttonColor.UseVisualStyleBackColor = true;
			this.buttonColor.Click += new EventHandler(this.buttonColor_Click);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(14, 0x38);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x22, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Color:";
			this.textBoxName.Location = new Point(0x70, 0x19);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new Size(150, 20);
			this.textBoxName.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(14, 0x1c);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x26, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			this.buttonClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.Location = new Point(0xcd, 0xef);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new Size(0x4b, 0x17);
			this.buttonClose.TabIndex = 1;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
			this.buttonPrev.Location = new Point(12, 12);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new Size(0x17, 0x17);
			this.buttonPrev.TabIndex = 0;
			this.buttonPrev.Text = "<";
			this.buttonPrev.UseVisualStyleBackColor = true;
			this.buttonPrev.Click += new EventHandler(this.buttonPrev_Click);
			this.buttonNext.Location = new Point(0x29, 12);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new Size(0x17, 0x17);
			this.buttonNext.TabIndex = 1;
			this.buttonNext.Text = ">";
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new EventHandler(this.buttonNext_Click);
			this.comboBoxChannels.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannels.FormattingEnabled = true;
			this.comboBoxChannels.Location = new Point(70, 14);
			this.comboBoxChannels.Name = "comboBoxChannels";
			this.comboBoxChannels.Size = new Size(0xc6, 0x15);
			this.comboBoxChannels.TabIndex = 2;
			this.comboBoxChannels.SelectedIndexChanged += new EventHandler(this.comboBoxChannels_SelectedIndexChanged);
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			this.buttonDimmingCurve.Location = new Point(0x11, 0x95);
			this.buttonDimmingCurve.Name = "buttonDimmingCurve";
			this.buttonDimmingCurve.Size = new Size(0x6a, 0x17);
			this.buttonDimmingCurve.TabIndex = 7;
			this.buttonDimmingCurve.Text = "Dimming Curve";
			this.buttonDimmingCurve.UseVisualStyleBackColor = true;
			this.buttonDimmingCurve.Click += new EventHandler(this.buttonDimmingCurve_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonClose;
			base.ClientSize = new Size(0x124, 0x112);
			base.Controls.Add(this.comboBoxChannels);
			base.Controls.Add(this.buttonNext);
			base.Controls.Add(this.buttonClose);
			base.Controls.Add(this.buttonPrev);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.KeyPreview = true;
			base.Name = "ChannelPropertyDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Channel Properties";
			base.Load += new EventHandler(this.ChannelPropertyDialog_Load);
			base.KeyPress += new KeyPressEventHandler(this.ChannelPropertyDialog_KeyPress);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
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
