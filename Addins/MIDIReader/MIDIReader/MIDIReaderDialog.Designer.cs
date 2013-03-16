using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace MIDIReader
{
	public partial class MIDIReaderDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonAutoMap;
		private Button buttonCancel;
		private Button buttonOK;

		private void InitializeComponent()
        {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.buttonAutoMap = new Button();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x275, 0x19e);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(710, 0x19e);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonAutoMap.Location = new Point(13, 0x19e);
            this.buttonAutoMap.Name = "buttonAutoMap";
            this.buttonAutoMap.Size = new Size(0x4b, 0x17);
            this.buttonAutoMap.TabIndex = 1;
            this.buttonAutoMap.Text = "Auto Map";
            this.buttonAutoMap.UseVisualStyleBackColor = true;
            this.buttonAutoMap.Click += new EventHandler(this.buttonAutoMap_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x31d, 0x1c1);
            base.Controls.Add(this.buttonAutoMap);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.HelpButton = true;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MIDIReaderDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MIDI Reader";
            base.ResizeBegin += new EventHandler(this.MIDIReaderDialog_ResizeBegin);
            base.HelpButtonClicked += new CancelEventHandler(this.MIDIReaderDialog_HelpButtonClicked);
            base.Resize += new EventHandler(this.MIDIReaderDialog_Resize);
            base.MouseUp += new MouseEventHandler(this.MIDIReaderDialog_MouseUp);
            base.MouseMove += new MouseEventHandler(this.MIDIReaderDialog_MouseMove);
            base.KeyDown += new KeyEventHandler(this.MIDIReaderDialog_KeyDown);
            base.ResizeEnd += new EventHandler(this.MIDIReaderDialog_ResizeEnd);
            base.MouseDown += new MouseEventHandler(this.MIDIReaderDialog_MouseDown);
            base.ResumeLayout(false);
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_noteTextBrush.Dispose();
            this.m_textFont.Dispose();
            base.Dispose(disposing);
        }
	}
}
