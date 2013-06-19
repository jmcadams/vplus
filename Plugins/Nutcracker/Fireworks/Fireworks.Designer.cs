namespace Fireworks {
    partial class Fireworks {
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
            this.lblExplosionCount = new System.Windows.Forms.Label();
            this.tbExplosionCount = new System.Windows.Forms.TrackBar();
            this.tbExplosionParticles = new System.Windows.Forms.TrackBar();
            this.lblExplosionParticles = new System.Windows.Forms.Label();
            this.tbParticleVelocity = new System.Windows.Forms.TrackBar();
            this.lblParticleVelocity = new System.Windows.Forms.Label();
            this.tbParticleFade = new System.Windows.Forms.TrackBar();
            this.lblParticleFade = new System.Windows.Forms.Label();
            this.cbMutliColor = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbExplosionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbExplosionParticles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParticleVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParticleFade)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExplosionCount
            // 
            this.lblExplosionCount.AutoSize = true;
            this.lblExplosionCount.Location = new System.Drawing.Point(26, 6);
            this.lblExplosionCount.Name = "lblExplosionCount";
            this.lblExplosionCount.Size = new System.Drawing.Size(57, 13);
            this.lblExplosionCount.TabIndex = 0;
            this.lblExplosionCount.Text = "# of Shells";
            // 
            // tbExplosionCount
            // 
            this.tbExplosionCount.AutoSize = false;
            this.tbExplosionCount.Location = new System.Drawing.Point(90, 0);
            this.tbExplosionCount.Maximum = 100;
            this.tbExplosionCount.Minimum = 1;
            this.tbExplosionCount.Name = "tbExplosionCount";
            this.tbExplosionCount.Size = new System.Drawing.Size(139, 25);
            this.tbExplosionCount.TabIndex = 1;
            this.tbExplosionCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbExplosionCount.Value = 1;
            this.tbExplosionCount.ValueChanged += new System.EventHandler(this.Fireworks_ControlChanged);
            // 
            // tbExplosionParticles
            // 
            this.tbExplosionParticles.AutoSize = false;
            this.tbExplosionParticles.Location = new System.Drawing.Point(90, 25);
            this.tbExplosionParticles.Maximum = 100;
            this.tbExplosionParticles.Minimum = 1;
            this.tbExplosionParticles.Name = "tbExplosionParticles";
            this.tbExplosionParticles.Size = new System.Drawing.Size(139, 25);
            this.tbExplosionParticles.TabIndex = 3;
            this.tbExplosionParticles.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbExplosionParticles.Value = 1;
            this.tbExplosionParticles.ValueChanged += new System.EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblExplosionParticles
            // 
            this.lblExplosionParticles.AutoSize = true;
            this.lblExplosionParticles.Location = new System.Drawing.Point(10, 31);
            this.lblExplosionParticles.Name = "lblExplosionParticles";
            this.lblExplosionParticles.Size = new System.Drawing.Size(73, 13);
            this.lblExplosionParticles.TabIndex = 2;
            this.lblExplosionParticles.Text = "Expl. Particles";
            // 
            // tbParticleVelocity
            // 
            this.tbParticleVelocity.AutoSize = false;
            this.tbParticleVelocity.Location = new System.Drawing.Point(90, 50);
            this.tbParticleVelocity.Maximum = 11;
            this.tbParticleVelocity.Minimum = 1;
            this.tbParticleVelocity.Name = "tbParticleVelocity";
            this.tbParticleVelocity.Size = new System.Drawing.Size(139, 25);
            this.tbParticleVelocity.TabIndex = 5;
            this.tbParticleVelocity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbParticleVelocity.Value = 1;
            this.tbParticleVelocity.ValueChanged += new System.EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblParticleVelocity
            // 
            this.lblParticleVelocity.AutoSize = true;
            this.lblParticleVelocity.Location = new System.Drawing.Point(1, 56);
            this.lblParticleVelocity.Name = "lblParticleVelocity";
            this.lblParticleVelocity.Size = new System.Drawing.Size(82, 13);
            this.lblParticleVelocity.TabIndex = 4;
            this.lblParticleVelocity.Text = "Particle Velocity";
            // 
            // tbParticleFade
            // 
            this.tbParticleFade.AutoSize = false;
            this.tbParticleFade.Location = new System.Drawing.Point(90, 75);
            this.tbParticleFade.Maximum = 100;
            this.tbParticleFade.Minimum = 1;
            this.tbParticleFade.Name = "tbParticleFade";
            this.tbParticleFade.Size = new System.Drawing.Size(139, 25);
            this.tbParticleFade.TabIndex = 7;
            this.tbParticleFade.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbParticleFade.Value = 1;
            this.tbParticleFade.ValueChanged += new System.EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblParticleFade
            // 
            this.lblParticleFade.AutoSize = true;
            this.lblParticleFade.Location = new System.Drawing.Point(14, 81);
            this.lblParticleFade.Name = "lblParticleFade";
            this.lblParticleFade.Size = new System.Drawing.Size(69, 13);
            this.lblParticleFade.TabIndex = 6;
            this.lblParticleFade.Text = "Particle Fade";
            // 
            // cbMutliColor
            // 
            this.cbMutliColor.AutoSize = true;
            this.cbMutliColor.Location = new System.Drawing.Point(90, 106);
            this.cbMutliColor.Name = "cbMutliColor";
            this.cbMutliColor.Size = new System.Drawing.Size(106, 17);
            this.cbMutliColor.TabIndex = 8;
            this.cbMutliColor.Text = "Multi-Color Shells";
            this.cbMutliColor.UseVisualStyleBackColor = true;
            // 
            // Fireworks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbMutliColor);
            this.Controls.Add(this.tbParticleFade);
            this.Controls.Add(this.lblParticleFade);
            this.Controls.Add(this.tbParticleVelocity);
            this.Controls.Add(this.lblParticleVelocity);
            this.Controls.Add(this.tbExplosionParticles);
            this.Controls.Add(this.lblExplosionParticles);
            this.Controls.Add(this.tbExplosionCount);
            this.Controls.Add(this.lblExplosionCount);
            this.Name = "Fireworks";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbExplosionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbExplosionParticles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParticleVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParticleFade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExplosionCount;
        private System.Windows.Forms.TrackBar tbExplosionCount;
        private System.Windows.Forms.TrackBar tbExplosionParticles;
        private System.Windows.Forms.Label lblExplosionParticles;
        private System.Windows.Forms.TrackBar tbParticleVelocity;
        private System.Windows.Forms.Label lblParticleVelocity;
        private System.Windows.Forms.TrackBar tbParticleFade;
        private System.Windows.Forms.Label lblParticleFade;
        private System.Windows.Forms.CheckBox cbMutliColor;
    }
}
