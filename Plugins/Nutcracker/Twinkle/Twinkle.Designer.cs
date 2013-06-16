namespace Twinkle {
    partial class Twinkle {
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
            this.tbLightCount = new System.Windows.Forms.TrackBar();
            this.lblLightCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tbLightCount
            // 
            this.tbLightCount.AutoSize = false;
            this.tbLightCount.Location = new System.Drawing.Point(93, 0);
            this.tbLightCount.Maximum = 100;
            this.tbLightCount.Name = "tbLightCount";
            this.tbLightCount.Size = new System.Drawing.Size(139, 25);
            this.tbLightCount.TabIndex = 23;
            this.tbLightCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbLightCount.ValueChanged += new System.EventHandler(this.Twinkle_ControlChanged);
            // 
            // lblLightCount
            // 
            this.lblLightCount.AutoSize = true;
            this.lblLightCount.Location = new System.Drawing.Point(26, 5);
            this.lblLightCount.Name = "lblLightCount";
            this.lblLightCount.Size = new System.Drawing.Size(57, 13);
            this.lblLightCount.TabIndex = 22;
            this.lblLightCount.Text = "# of Lights";
            // 
            // Twinkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbLightCount);
            this.Controls.Add(this.lblLightCount);
            this.Name = "Twinkle";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbLightCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbLightCount;
        private System.Windows.Forms.Label lblLightCount;
    }
}
