using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs{
    public partial class ChannelOrderDialog{
        private IContainer components;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private System.Windows.Forms.Panel panel1;
        private PictureBox pictureBoxChannels;
        private VScrollBar vScrollBar;

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.pictureBoxChannels = new PictureBox();
            this.vScrollBar = new VScrollBar();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxChannels);
            this.panel1.Controls.Add(this.vScrollBar);
            this.panel1.Location = new Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(680, 513);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxChannels
            // 
            this.pictureBoxChannels.BackColor = Color.White;
            this.pictureBoxChannels.Dock = DockStyle.Fill;
            this.pictureBoxChannels.Location = new Point(0, 0);
            this.pictureBoxChannels.Name = "pictureBoxChannels";
            this.pictureBoxChannels.Size = new Size(661, 511);
            this.pictureBoxChannels.TabIndex = 2;
            this.pictureBoxChannels.TabStop = false;
            this.pictureBoxChannels.Paint += new PaintEventHandler(this.pictureBoxChannels_Paint);
            this.pictureBoxChannels.MouseDoubleClick += new MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
            this.pictureBoxChannels.MouseDown += new MouseEventHandler(this.pictureBoxChannels_MouseDown);
            this.pictureBoxChannels.MouseMove += new MouseEventHandler(this.pictureBoxChannels_MouseMove);
            this.pictureBoxChannels.MouseUp += new MouseEventHandler(this.pictureBoxChannels_MouseUp);
            this.pictureBoxChannels.Resize += new EventHandler(this.pictureBoxChannels_Resize);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = DockStyle.Right;
            this.vScrollBar.Location = new Point(661, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new Size(17, 511);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new EventHandler(this.vScrollBar_ValueChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(536, 531);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(617, 531);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ChannelOrderDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(704, 566);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelOrderDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Order";
            this.HelpButtonClicked += new CancelEventHandler(this.ChannelOrderDialog_HelpButtonClicked);
            this.KeyDown += new KeyEventHandler(this.ChannelOrderDialog_KeyDown);
            this.KeyUp += new KeyEventHandler(this.ChannelOrderDialog_KeyUp);
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBoxChannels)).EndInit();
            this.ResumeLayout(false);

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
