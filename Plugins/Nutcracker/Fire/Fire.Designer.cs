namespace Fire {
    partial class Fire {
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
            this.lblHeight = new System.Windows.Forms.Label();
            this.tbHeight = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(4, 6);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(38, 13);
            this.lblHeight.TabIndex = 0;
            this.lblHeight.Text = "Height";
            // 
            // tbHeight
            // 
            this.tbHeight.AutoSize = false;
            this.tbHeight.Location = new System.Drawing.Point(46, 0);
            this.tbHeight.Maximum = 100;
            this.tbHeight.Minimum = 1;
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(183, 25);
            this.tbHeight.TabIndex = 1;
            this.tbHeight.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbHeight.Value = 1;
            this.tbHeight.ValueChanged += new System.EventHandler(this.Fire_ControlChanged);
            // 
            // Fire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.lblHeight);
            this.Name = "Fire";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TrackBar tbHeight;
    }
}
