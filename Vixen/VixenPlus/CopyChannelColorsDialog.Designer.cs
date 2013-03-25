using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class CopyChannelColorsDialog
	{
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCopy;
private ComboBox comboBoxDestinationColors;
private ComboBox comboBoxDestinationSequence;
private ComboBox comboBoxSourceColors;
private ComboBox comboBoxSourceSequence;
private GroupBox gbColors;
private GroupBox groupBox2;

		private void InitializeComponent()
		{
			this.gbColors = new GroupBox();
			this.comboBoxSourceColors = new ComboBox();
			this.comboBoxSourceSequence = new ComboBox();
			this.groupBox2 = new GroupBox();
			this.comboBoxDestinationColors = new ComboBox();
			this.comboBoxDestinationSequence = new ComboBox();
			this.buttonCopy = new Button();
			this.gbColors.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.gbColors.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.gbColors.Controls.Add(this.comboBoxSourceColors);
			this.gbColors.Controls.Add(this.comboBoxSourceSequence);
			this.gbColors.Location = new Point(12, 12);
			this.gbColors.Name = "groupBox1";
			this.gbColors.Size = new Size(399, 73);
			this.gbColors.TabIndex = 0;
			this.gbColors.TabStop = false;
			this.gbColors.Text = "Source Sequence";
			this.comboBoxSourceColors.DrawMode = DrawMode.OwnerDrawFixed;
			this.comboBoxSourceColors.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSourceColors.FormattingEnabled = true;
			this.comboBoxSourceColors.Location = new Point(272, 31);
			this.comboBoxSourceColors.Name = "comboBoxSourceColors";
			this.comboBoxSourceColors.Size = new Size(121, 21);
			this.comboBoxSourceColors.TabIndex = 1;
			this.comboBoxSourceColors.DrawItem += new DrawItemEventHandler(this.comboBoxSourceColors_DrawItem);
			this.comboBoxSourceSequence.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSourceSequence.FormattingEnabled = true;
			this.comboBoxSourceSequence.Location = new Point(6, 31);
			this.comboBoxSourceSequence.Name = "comboBoxSourceSequence";
			this.comboBoxSourceSequence.Size = new Size(260, 21);
			this.comboBoxSourceSequence.TabIndex = 0;
			this.comboBoxSourceSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.comboBoxDestinationColors);
			this.groupBox2.Controls.Add(this.comboBoxDestinationSequence);
			this.groupBox2.Location = new Point(12, 91);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(399, 73);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Destination Sequence";
			this.comboBoxDestinationColors.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationColors.FormattingEnabled = true;
			this.comboBoxDestinationColors.Location = new Point(272, 29);
			this.comboBoxDestinationColors.Name = "comboBoxDestinationColors";
			this.comboBoxDestinationColors.Size = new Size(121, 21);
			this.comboBoxDestinationColors.TabIndex = 1;
			this.comboBoxDestinationSequence.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationSequence.FormattingEnabled = true;
			this.comboBoxDestinationSequence.Location = new Point(6, 29);
			this.comboBoxDestinationSequence.Name = "comboBoxDestinationSequence";
			this.comboBoxDestinationSequence.Size = new Size(260, 21);
			this.comboBoxDestinationSequence.TabIndex = 0;
			this.comboBoxDestinationSequence.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationSequence_SelectedIndexChanged);
			this.buttonCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCopy.Location = new Point(336, 174);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new Size(75, 23);
			this.buttonCopy.TabIndex = 2;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(423, 209);
			base.Controls.Add(this.buttonCopy);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.gbColors);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "CopyChannelColorsDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Copy Channel Colors";
			base.FormClosing += new FormClosingEventHandler(this.CopyChannelColorsDialog_FormClosing);
			this.gbColors.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			if (this._solidBrush != null)
			{
				this._solidBrush.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
