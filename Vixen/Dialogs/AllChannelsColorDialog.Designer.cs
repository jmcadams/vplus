using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public partial class AllChannelsColorDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonNewColor;
        private Button buttonOK;
        private ColorDialog colorDialog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ListBox listBoxChannels;
        private ListBox listBoxColorsInUse;


        private void InitializeComponent() {
            this.groupBox1 = new GroupBox();
            this.listBoxChannels = new ListBox();
            this.groupBox2 = new GroupBox();
            this.listBoxColorsInUse = new ListBox();
            this.buttonNewColor = new Button();
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.colorDialog = new ColorDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxChannels);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(212, 429);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            // 
            // listBoxChannels
            // 
            this.listBoxChannels.AllowDrop = true;
            this.listBoxChannels.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.listBoxChannels.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.ItemHeight = 20;
            this.listBoxChannels.Location = new Point(16, 19);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(183, 384);
            this.listBoxChannels.TabIndex = 0;
            this.listBoxChannels.DrawItem += new DrawItemEventHandler(this.listBoxChannels_DrawItem);
            this.listBoxChannels.DragDrop += new DragEventHandler(this.listBoxChannels_DragDrop);
            this.listBoxChannels.DragEnter += new DragEventHandler(this.listBoxChannels_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxColorsInUse);
            this.groupBox2.Location = new Point(243, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(132, 391);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Colors in use";
            // 
            // listBoxColorsInUse
            // 
            this.listBoxColorsInUse.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.listBoxColorsInUse.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxColorsInUse.FormattingEnabled = true;
            this.listBoxColorsInUse.ItemHeight = 20;
            this.listBoxColorsInUse.Location = new Point(15, 19);
            this.listBoxColorsInUse.Name = "listBoxColorsInUse";
            this.listBoxColorsInUse.ScrollAlwaysVisible = true;
            this.listBoxColorsInUse.Size = new Size(104, 344);
            this.listBoxColorsInUse.TabIndex = 0;
            this.listBoxColorsInUse.DrawItem += new DrawItemEventHandler(this.listBoxColorsInUse_DrawItem);
            this.listBoxColorsInUse.MouseDown += new MouseEventHandler(this.listBoxColorsInUse_MouseDown);
            this.listBoxColorsInUse.MouseMove += new MouseEventHandler(this.listBoxColorsInUse_MouseMove);
            // 
            // buttonNewColor
            // 
            this.buttonNewColor.Location = new Point(272, 418);
            this.buttonNewColor.Name = "buttonNewColor";
            this.buttonNewColor.Size = new Size(75, 23);
            this.buttonNewColor.TabIndex = 3;
            this.buttonNewColor.Text = "New Color";
            this.buttonNewColor.UseVisualStyleBackColor = true;
            this.buttonNewColor.Click += new EventHandler(this.buttonNewColor_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(304, 475);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(223, 475);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            // 
            // AllChannelsColorDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(391, 510);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonNewColor);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AllChannelsColorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Colors";
            this.HelpButtonClicked += new CancelEventHandler(this.AllChannelsColorDialog_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            this._solidBrush.Dispose();
            base.Dispose(disposing);
        }
    }
}

