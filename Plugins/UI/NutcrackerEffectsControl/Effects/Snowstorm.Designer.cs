namespace NutcrackerEffects.Effects {
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
            this.tbTailLength = new System.Windows.Forms.TrackBar();
            this.lblTrailLength = new System.Windows.Forms.Label();
            this.tbMaxFlakes = new System.Windows.Forms.TrackBar();
            this.lblMaxFlakes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbTailLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxFlakes)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTailLength
            // 
            this.tbTailLength.AutoSize = false;
            this.tbTailLength.Location = new System.Drawing.Point(90, 34);
            this.tbTailLength.Maximum = 100;
            this.tbTailLength.Name = "tbTailLength";
            this.tbTailLength.Size = new System.Drawing.Size(139, 25);
            this.tbTailLength.TabIndex = 17;
            this.tbTailLength.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbTailLength.Value = 50;
            this.tbTailLength.ValueChanged += new System.EventHandler(this.SnowStorm_ControlChanged);
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
            // tbMaxFlakes
            // 
            this.tbMaxFlakes.AutoSize = false;
            this.tbMaxFlakes.Location = new System.Drawing.Point(90, 3);
            this.tbMaxFlakes.Maximum = 100;
            this.tbMaxFlakes.Name = "tbMaxFlakes";
            this.tbMaxFlakes.Size = new System.Drawing.Size(139, 25);
            this.tbMaxFlakes.TabIndex = 15;
            this.tbMaxFlakes.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbMaxFlakes.Value = 50;
            this.tbMaxFlakes.ValueChanged += new System.EventHandler(this.SnowStorm_ControlChanged);
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
            this.Controls.Add(this.tbTailLength);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbMaxFlakes);
            this.Controls.Add(this.lblMaxFlakes);
            this.Name = "Snowstorm";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbTailLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxFlakes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbTailLength;
        private System.Windows.Forms.Label lblTrailLength;
        private System.Windows.Forms.TrackBar tbMaxFlakes;
        private System.Windows.Forms.Label lblMaxFlakes;
    }
}
