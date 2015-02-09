using System.ComponentModel;
using System.Windows.Forms;

partial class RGBTreePreviewDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        if (this._brush != null)
            this._brush.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.pictureBox = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
        this.SuspendLayout();
        // 
        // pictureBox
        // 
        this.pictureBox.BackColor = System.Drawing.Color.Transparent;
        this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pictureBox.Location = new System.Drawing.Point(0, 0);
        this.pictureBox.Name = "pictureBox";
        this.pictureBox.Size = new System.Drawing.Size(1234, 427);
        this.pictureBox.TabIndex = 0;
        this.pictureBox.TabStop = false;
        this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
        // 
        // form_Preview
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Black;
        this.ClientSize = new System.Drawing.Size(1234, 427);
        this.Controls.Add(this.pictureBox);
        this.ForeColor = System.Drawing.SystemColors.Control;
        this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.Name = "RGBTreePreview";
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "Preview";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private PictureBox pictureBox;


}