using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class CopyChannelColorsDialog
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
			this.gbColors.Size = new Size(0x18f, 0x49);
			this.gbColors.TabIndex = 0;
			this.gbColors.TabStop = false;
			this.gbColors.Text = "Source Sequence";
			this.comboBoxSourceColors.DrawMode = DrawMode.OwnerDrawFixed;
			this.comboBoxSourceColors.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSourceColors.FormattingEnabled = true;
			this.comboBoxSourceColors.Location = new Point(0x110, 0x1f);
			this.comboBoxSourceColors.Name = "comboBoxSourceColors";
			this.comboBoxSourceColors.Size = new Size(0x79, 0x15);
			this.comboBoxSourceColors.TabIndex = 1;
			this.comboBoxSourceColors.DrawItem += new DrawItemEventHandler(this.comboBoxSourceColors_DrawItem);
			this.comboBoxSourceSequence.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSourceSequence.FormattingEnabled = true;
			this.comboBoxSourceSequence.Location = new Point(6, 0x1f);
			this.comboBoxSourceSequence.Name = "comboBoxSourceSequence";
			this.comboBoxSourceSequence.Size = new Size(260, 0x15);
			this.comboBoxSourceSequence.TabIndex = 0;
			this.comboBoxSourceSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.comboBoxDestinationColors);
			this.groupBox2.Controls.Add(this.comboBoxDestinationSequence);
			this.groupBox2.Location = new Point(12, 0x5b);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x18f, 0x49);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Destination Sequence";
			this.comboBoxDestinationColors.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationColors.FormattingEnabled = true;
			this.comboBoxDestinationColors.Location = new Point(0x110, 0x1d);
			this.comboBoxDestinationColors.Name = "comboBoxDestinationColors";
			this.comboBoxDestinationColors.Size = new Size(0x79, 0x15);
			this.comboBoxDestinationColors.TabIndex = 1;
			this.comboBoxDestinationSequence.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationSequence.FormattingEnabled = true;
			this.comboBoxDestinationSequence.Location = new Point(6, 0x1d);
			this.comboBoxDestinationSequence.Name = "comboBoxDestinationSequence";
			this.comboBoxDestinationSequence.Size = new Size(260, 0x15);
			this.comboBoxDestinationSequence.TabIndex = 0;
			this.comboBoxDestinationSequence.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationSequence_SelectedIndexChanged);
			this.buttonCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCopy.Location = new Point(0x150, 0xae);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new Size(0x4b, 0x17);
			this.buttonCopy.TabIndex = 2;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x1a7, 0xd1);
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
			if (this.m_itemBrush != null)
			{
				this.m_itemBrush.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
