using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Fireworks {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.lblExplosionCount = new Label();
            this.tbExplosionCount = new TrackBar();
            this.tbExplosionParticles = new TrackBar();
            this.lblExplosionParticles = new Label();
            this.tbParticleVelocity = new TrackBar();
            this.lblParticleVelocity = new Label();
            this.tbParticleFade = new TrackBar();
            this.lblParticleFade = new Label();
            this.cbMutliColor = new CheckBox();
            ((ISupportInitialize)(this.tbExplosionCount)).BeginInit();
            ((ISupportInitialize)(this.tbExplosionParticles)).BeginInit();
            ((ISupportInitialize)(this.tbParticleVelocity)).BeginInit();
            ((ISupportInitialize)(this.tbParticleFade)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExplosionCount
            // 
            this.lblExplosionCount.AutoSize = true;
            this.lblExplosionCount.Location = new Point(26, 6);
            this.lblExplosionCount.Name = "lblExplosionCount";
            this.lblExplosionCount.Size = new Size(57, 13);
            this.lblExplosionCount.TabIndex = 0;
            this.lblExplosionCount.Text = "# of Shells";
            // 
            // tbExplosionCount
            // 
            this.tbExplosionCount.AutoSize = false;
            this.tbExplosionCount.Location = new Point(90, 0);
            this.tbExplosionCount.Maximum = 95;
            this.tbExplosionCount.Minimum = 1;
            this.tbExplosionCount.Name = "tbExplosionCount";
            this.tbExplosionCount.Size = new Size(139, 25);
            this.tbExplosionCount.TabIndex = 1;
            this.tbExplosionCount.TickStyle = TickStyle.None;
            this.tbExplosionCount.Value = 10;
            this.tbExplosionCount.ValueChanged += new EventHandler(this.Fireworks_ControlChanged);
            // 
            // tbExplosionParticles
            // 
            this.tbExplosionParticles.AutoSize = false;
            this.tbExplosionParticles.Location = new Point(90, 25);
            this.tbExplosionParticles.Maximum = 100;
            this.tbExplosionParticles.Minimum = 1;
            this.tbExplosionParticles.Name = "tbExplosionParticles";
            this.tbExplosionParticles.Size = new Size(139, 25);
            this.tbExplosionParticles.TabIndex = 3;
            this.tbExplosionParticles.TickStyle = TickStyle.None;
            this.tbExplosionParticles.Value = 50;
            this.tbExplosionParticles.ValueChanged += new EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblExplosionParticles
            // 
            this.lblExplosionParticles.AutoSize = true;
            this.lblExplosionParticles.Location = new Point(10, 31);
            this.lblExplosionParticles.Name = "lblExplosionParticles";
            this.lblExplosionParticles.Size = new Size(73, 13);
            this.lblExplosionParticles.TabIndex = 2;
            this.lblExplosionParticles.Text = "Expl. Particles";
            // 
            // tbParticleVelocity
            // 
            this.tbParticleVelocity.AutoSize = false;
            this.tbParticleVelocity.Location = new Point(90, 50);
            this.tbParticleVelocity.Minimum = 1;
            this.tbParticleVelocity.Name = "tbParticleVelocity";
            this.tbParticleVelocity.Size = new Size(139, 25);
            this.tbParticleVelocity.TabIndex = 5;
            this.tbParticleVelocity.TickStyle = TickStyle.None;
            this.tbParticleVelocity.Value = 2;
            this.tbParticleVelocity.ValueChanged += new EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblParticleVelocity
            // 
            this.lblParticleVelocity.AutoSize = true;
            this.lblParticleVelocity.Location = new Point(1, 56);
            this.lblParticleVelocity.Name = "lblParticleVelocity";
            this.lblParticleVelocity.Size = new Size(82, 13);
            this.lblParticleVelocity.TabIndex = 4;
            this.lblParticleVelocity.Text = "Particle Velocity";
            // 
            // tbParticleFade
            // 
            this.tbParticleFade.AutoSize = false;
            this.tbParticleFade.Location = new Point(90, 75);
            this.tbParticleFade.Maximum = 100;
            this.tbParticleFade.Minimum = 1;
            this.tbParticleFade.Name = "tbParticleFade";
            this.tbParticleFade.Size = new Size(139, 25);
            this.tbParticleFade.TabIndex = 7;
            this.tbParticleFade.TickStyle = TickStyle.None;
            this.tbParticleFade.Value = 50;
            this.tbParticleFade.ValueChanged += new EventHandler(this.Fireworks_ControlChanged);
            // 
            // lblParticleFade
            // 
            this.lblParticleFade.AutoSize = true;
            this.lblParticleFade.Location = new Point(14, 81);
            this.lblParticleFade.Name = "lblParticleFade";
            this.lblParticleFade.Size = new Size(69, 13);
            this.lblParticleFade.TabIndex = 6;
            this.lblParticleFade.Text = "Particle Fade";
            // 
            // cbMutliColor
            // 
            this.cbMutliColor.AutoSize = true;
            this.cbMutliColor.Location = new Point(90, 106);
            this.cbMutliColor.Name = "cbMutliColor";
            this.cbMutliColor.Size = new Size(106, 17);
            this.cbMutliColor.TabIndex = 8;
            this.cbMutliColor.Text = "Multi-Color Shells";
            this.cbMutliColor.UseVisualStyleBackColor = true;
            this.cbMutliColor.CheckedChanged += new EventHandler(this.Fireworks_ControlChanged);
            // 
            // Fireworks
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
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
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbExplosionCount)).EndInit();
            ((ISupportInitialize)(this.tbExplosionParticles)).EndInit();
            ((ISupportInitialize)(this.tbParticleVelocity)).EndInit();
            ((ISupportInitialize)(this.tbParticleFade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblExplosionCount;
        private TrackBar tbExplosionCount;
        private TrackBar tbExplosionParticles;
        private Label lblExplosionParticles;
        private TrackBar tbParticleVelocity;
        private Label lblParticleVelocity;
        private TrackBar tbParticleFade;
        private Label lblParticleFade;
        private CheckBox cbMutliColor;
    }
}
