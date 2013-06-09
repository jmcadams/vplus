namespace VixenEditor
{
    partial class NutcrackerControlDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.nutcrackerEffectControl1 = new VixenEditor.NutcrackerEffectControl();
            this.nutcrackerEffectControl2 = new VixenEditor.NutcrackerEffectControl();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nutcrackerEffectControl2);
            this.groupBox2.Location = new System.Drawing.Point(788, 352);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 198);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Effect 2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nutcrackerEffectControl1);
            this.groupBox1.Location = new System.Drawing.Point(398, 352);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 198);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Effect 1";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(12, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(380, 538);
            this.pbPreview.TabIndex = 2;
            this.pbPreview.TabStop = false;
            // 
            // nutcrackerEffectControl1
            // 
            this.nutcrackerEffectControl1.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl1.Name = "nutcrackerEffectControl1";
            this.nutcrackerEffectControl1.Size = new System.Drawing.Size(371, 173);
            this.nutcrackerEffectControl1.TabIndex = 0;
            // 
            // nutcrackerEffectControl2
            // 
            this.nutcrackerEffectControl2.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl2.Name = "nutcrackerEffectControl2";
            this.nutcrackerEffectControl2.Size = new System.Drawing.Size(371, 173);
            this.nutcrackerEffectControl2.TabIndex = 0;
            // 
            // NutcrackerControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 562);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "NutcrackerControlDialog";
            this.Text = "Generate Nutcracker Effect";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbPreview;
        private NutcrackerEffectControl nutcrackerEffectControl1;
        private NutcrackerEffectControl nutcrackerEffectControl2;
    }
}