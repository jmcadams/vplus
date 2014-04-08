namespace Nutcracker.Effects {
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
            this.tbSteps = new System.Windows.Forms.TrackBar();
            this.lblSteps = new System.Windows.Forms.Label();
            this.chkBoxStrobe = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSteps)).BeginInit();
            this.SuspendLayout();
            // 
            // tbLightCount
            // 
            this.tbLightCount.AutoSize = false;
            this.tbLightCount.Location = new System.Drawing.Point(93, 0);
            this.tbLightCount.Maximum = 100;
            this.tbLightCount.Minimum = 1;
            this.tbLightCount.Name = "tbLightCount";
            this.tbLightCount.Size = new System.Drawing.Size(139, 25);
            this.tbLightCount.TabIndex = 23;
            this.tbLightCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbLightCount.Value = 20;
            this.tbLightCount.ValueChanged += new System.EventHandler(this.Twinkle_ControlChanged);
            // 
            // lblLightCount
            // 
            this.lblLightCount.AutoSize = true;
            this.lblLightCount.Location = new System.Drawing.Point(29, 3);
            this.lblLightCount.Name = "lblLightCount";
            this.lblLightCount.Size = new System.Drawing.Size(57, 13);
            this.lblLightCount.TabIndex = 22;
            this.lblLightCount.Text = "# of Lights";
            // 
            // tbSteps
            // 
            this.tbSteps.AutoSize = false;
            this.tbSteps.Location = new System.Drawing.Point(93, 31);
            this.tbSteps.Maximum = 200;
            this.tbSteps.Minimum = 1;
            this.tbSteps.Name = "tbSteps";
            this.tbSteps.Size = new System.Drawing.Size(139, 25);
            this.tbSteps.TabIndex = 25;
            this.tbSteps.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSteps.Value = 3;
            this.tbSteps.ValueChanged += new System.EventHandler(this.Twinkle_ControlChanged);
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Location = new System.Drawing.Point(12, 34);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(74, 13);
            this.lblSteps.TabIndex = 24;
            this.lblSteps.Text = "Twinkle Steps";
            // 
            // chkBoxStrobe
            // 
            this.chkBoxStrobe.AutoSize = true;
            this.chkBoxStrobe.Location = new System.Drawing.Point(29, 62);
            this.chkBoxStrobe.Name = "chkBoxStrobe";
            this.chkBoxStrobe.Size = new System.Drawing.Size(57, 17);
            this.chkBoxStrobe.TabIndex = 26;
            this.chkBoxStrobe.Text = "Strobe";
            this.chkBoxStrobe.UseVisualStyleBackColor = true;
            this.chkBoxStrobe.CheckedChanged += new System.EventHandler(this.Twinkle_ControlChanged);
            // 
            // Twinkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxStrobe);
            this.Controls.Add(this.tbSteps);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.tbLightCount);
            this.Controls.Add(this.lblLightCount);
            this.Name = "Twinkle";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbLightCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSteps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbLightCount;
        private System.Windows.Forms.Label lblLightCount;
        private System.Windows.Forms.TrackBar tbSteps;
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.CheckBox chkBoxStrobe;
    }
}
