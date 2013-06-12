namespace Snowstorm {
    partial class Snowstorm {
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
            this.tbSpacing = new System.Windows.Forms.TrackBar();
            this.lblTrailLength = new System.Windows.Forms.Label();
            this.tbGarlandType = new System.Windows.Forms.TrackBar();
            this.lblMaxFlakes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSpacing
            // 
            this.tbSpacing.AutoSize = false;
            this.tbSpacing.Location = new System.Drawing.Point(90, 34);
            this.tbSpacing.Maximum = 100;
            this.tbSpacing.Name = "tbSpacing";
            this.tbSpacing.Size = new System.Drawing.Size(139, 25);
            this.tbSpacing.TabIndex = 17;
            this.tbSpacing.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblTrailLength
            // 
            this.lblTrailLength.AutoSize = true;
            this.lblTrailLength.Location = new System.Drawing.Point(12, 40);
            this.lblTrailLength.Name = "lblTrailLength";
            this.lblTrailLength.Size = new System.Drawing.Size(63, 13);
            this.lblTrailLength.TabIndex = 16;
            this.lblTrailLength.Text = "Trail Length";
            // 
            // tbGarlandType
            // 
            this.tbGarlandType.AutoSize = false;
            this.tbGarlandType.Location = new System.Drawing.Point(90, 3);
            this.tbGarlandType.Maximum = 100;
            this.tbGarlandType.Name = "tbGarlandType";
            this.tbGarlandType.Size = new System.Drawing.Size(139, 25);
            this.tbGarlandType.TabIndex = 15;
            this.tbGarlandType.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblMaxFlakes
            // 
            this.lblMaxFlakes.AutoSize = true;
            this.lblMaxFlakes.Location = new System.Drawing.Point(14, 9);
            this.lblMaxFlakes.Name = "lblMaxFlakes";
            this.lblMaxFlakes.Size = new System.Drawing.Size(61, 13);
            this.lblMaxFlakes.TabIndex = 14;
            this.lblMaxFlakes.Text = "Max Flakes";
            // 
            // Snowstorm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSpacing);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbGarlandType);
            this.Controls.Add(this.lblMaxFlakes);
            this.Name = "Snowstorm";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbSpacing;
        private System.Windows.Forms.Label lblTrailLength;
        private System.Windows.Forms.TrackBar tbGarlandType;
        private System.Windows.Forms.Label lblMaxFlakes;
    }
}
