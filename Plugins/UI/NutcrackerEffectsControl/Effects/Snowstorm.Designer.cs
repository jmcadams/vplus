using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Snowstorm {
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
            this.tbTailLength = new TrackBar();
            this.lblTrailLength = new Label();
            this.tbMaxFlakes = new TrackBar();
            this.lblMaxFlakes = new Label();
            ((ISupportInitialize)(this.tbTailLength)).BeginInit();
            ((ISupportInitialize)(this.tbMaxFlakes)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTailLength
            // 
            this.tbTailLength.AutoSize = false;
            this.tbTailLength.Location = new Point(90, 34);
            this.tbTailLength.Maximum = 100;
            this.tbTailLength.Name = "tbTailLength";
            this.tbTailLength.Size = new Size(139, 25);
            this.tbTailLength.TabIndex = 17;
            this.tbTailLength.TickStyle = TickStyle.None;
            this.tbTailLength.Value = 50;
            this.tbTailLength.ValueChanged += new EventHandler(this.SnowStorm_ControlChanged);
            // 
            // lblTrailLength
            // 
            this.lblTrailLength.AutoSize = true;
            this.lblTrailLength.Location = new Point(12, 40);
            this.lblTrailLength.Name = "lblTrailLength";
            this.lblTrailLength.Size = new Size(63, 13);
            this.lblTrailLength.TabIndex = 16;
            this.lblTrailLength.Text = "Trail Length";
            // 
            // tbMaxFlakes
            // 
            this.tbMaxFlakes.AutoSize = false;
            this.tbMaxFlakes.Location = new Point(90, 3);
            this.tbMaxFlakes.Maximum = 100;
            this.tbMaxFlakes.Name = "tbMaxFlakes";
            this.tbMaxFlakes.Size = new Size(139, 25);
            this.tbMaxFlakes.TabIndex = 15;
            this.tbMaxFlakes.TickStyle = TickStyle.None;
            this.tbMaxFlakes.Value = 50;
            this.tbMaxFlakes.ValueChanged += new EventHandler(this.SnowStorm_ControlChanged);
            // 
            // lblMaxFlakes
            // 
            this.lblMaxFlakes.AutoSize = true;
            this.lblMaxFlakes.Location = new Point(14, 9);
            this.lblMaxFlakes.Name = "lblMaxFlakes";
            this.lblMaxFlakes.Size = new Size(61, 13);
            this.lblMaxFlakes.TabIndex = 14;
            this.lblMaxFlakes.Text = "Max Flakes";
            // 
            // Snowstorm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tbTailLength);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbMaxFlakes);
            this.Controls.Add(this.lblMaxFlakes);
            this.Name = "Snowstorm";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbTailLength)).EndInit();
            ((ISupportInitialize)(this.tbMaxFlakes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbTailLength;
        private Label lblTrailLength;
        private TrackBar tbMaxFlakes;
        private Label lblMaxFlakes;
    }
}
