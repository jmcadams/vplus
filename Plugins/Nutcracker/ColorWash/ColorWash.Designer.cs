namespace ColorWash {
    partial class ColorWash {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lblCount = new System.Windows.Forms.Label();
            this.chkBoxHFade = new System.Windows.Forms.CheckBox();
            this.chkBoxVFade = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(125, 3);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 25);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(84, 9);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "Count";
            // 
            // chkBoxHFade
            // 
            this.chkBoxHFade.AutoSize = true;
            this.chkBoxHFade.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxHFade.Location = new System.Drawing.Point(3, 37);
            this.chkBoxHFade.Name = "chkBoxHFade";
            this.chkBoxHFade.Size = new System.Drawing.Size(100, 17);
            this.chkBoxHFade.TabIndex = 2;
            this.chkBoxHFade.Text = "Horizontal Fade";
            this.chkBoxHFade.UseVisualStyleBackColor = true;
            // 
            // chkBoxVFade
            // 
            this.chkBoxVFade.AutoSize = true;
            this.chkBoxVFade.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxVFade.Location = new System.Drawing.Point(125, 37);
            this.chkBoxVFade.Name = "chkBoxVFade";
            this.chkBoxVFade.Size = new System.Drawing.Size(88, 17);
            this.chkBoxVFade.TabIndex = 3;
            this.chkBoxVFade.Text = "Vertical Fade";
            this.chkBoxVFade.UseVisualStyleBackColor = true;
            // 
            // ColorWash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxVFade);
            this.Controls.Add(this.chkBoxHFade);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.trackBar1);
            this.Name = "ColorWash";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.CheckBox chkBoxHFade;
        private System.Windows.Forms.CheckBox chkBoxVFade;
    }
}
