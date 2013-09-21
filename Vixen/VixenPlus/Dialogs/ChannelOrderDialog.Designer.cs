namespace VixenPlus.Dialogs{
    using System.Windows.Forms;
    using System.ComponentModel;

    public partial class ChannelOrderDialog{
        private IContainer components;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private Panel panel1;
        private PictureBox pictureBoxChannels;
        private VScrollBar vScrollBar;

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxChannels = new System.Windows.Forms.PictureBox();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxChannels);
            this.panel1.Controls.Add(this.vScrollBar);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 513);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxChannels
            // 
            this.pictureBoxChannels.BackColor = System.Drawing.Color.White;
            this.pictureBoxChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxChannels.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxChannels.Name = "pictureBoxChannels";
            this.pictureBoxChannels.Size = new System.Drawing.Size(661, 511);
            this.pictureBoxChannels.TabIndex = 2;
            this.pictureBoxChannels.TabStop = false;
            this.pictureBoxChannels.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxChannels_Paint);
            this.pictureBoxChannels.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
            this.pictureBoxChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseDown);
            this.pictureBoxChannels.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseMove);
            this.pictureBoxChannels.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseUp);
            this.pictureBoxChannels.Resize += new System.EventHandler(this.pictureBoxChannels_Resize);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(661, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 511);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(536, 531);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(617, 531);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ChannelOrderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(704, 566);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelOrderDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Channel Order";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.ChannelOrderDialog_HelpButtonClicked);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChannelOrderDialog_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChannelOrderDialog_KeyUp);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannels)).EndInit();
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
