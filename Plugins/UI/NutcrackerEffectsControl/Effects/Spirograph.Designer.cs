using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Spirograph {
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
            this.tbDistance = new TrackBar();
            this.lblDistance = new Label();
            this.tbInnerR = new TrackBar();
            this.lblInnerRadius = new Label();
            this.tbOuterR = new TrackBar();
            this.lblOuterRadius = new Label();
            this.chkBoxAnimate = new CheckBox();
            this.lblNote = new Label();
            ((ISupportInitialize)(this.tbDistance)).BeginInit();
            ((ISupportInitialize)(this.tbInnerR)).BeginInit();
            ((ISupportInitialize)(this.tbOuterR)).BeginInit();
            this.SuspendLayout();
            // 
            // tbDistance
            // 
            this.tbDistance.AutoSize = false;
            this.tbDistance.Location = new Point(93, 56);
            this.tbDistance.Maximum = 100;
            this.tbDistance.Minimum = 1;
            this.tbDistance.Name = "tbDistance";
            this.tbDistance.Size = new Size(139, 25);
            this.tbDistance.TabIndex = 29;
            this.tbDistance.TickStyle = TickStyle.None;
            this.tbDistance.Value = 30;
            this.tbDistance.ValueChanged += new EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new Point(29, 60);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new Size(49, 13);
            this.lblDistance.TabIndex = 28;
            this.lblDistance.Text = "Distance";
            // 
            // tbInnerR
            // 
            this.tbInnerR.AutoSize = false;
            this.tbInnerR.Location = new Point(93, 28);
            this.tbInnerR.Maximum = 100;
            this.tbInnerR.Minimum = 1;
            this.tbInnerR.Name = "tbInnerR";
            this.tbInnerR.Size = new Size(139, 25);
            this.tbInnerR.TabIndex = 27;
            this.tbInnerR.TickStyle = TickStyle.None;
            this.tbInnerR.Value = 10;
            this.tbInnerR.ValueChanged += new EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblInnerRadius
            // 
            this.lblInnerRadius.AutoSize = true;
            this.lblInnerRadius.Location = new Point(11, 33);
            this.lblInnerRadius.Name = "lblInnerRadius";
            this.lblInnerRadius.Size = new Size(67, 13);
            this.lblInnerRadius.TabIndex = 26;
            this.lblInnerRadius.Text = "Inner Radius";
            // 
            // tbOuterR
            // 
            this.tbOuterR.AutoSize = false;
            this.tbOuterR.Location = new Point(93, 0);
            this.tbOuterR.Maximum = 100;
            this.tbOuterR.Minimum = 1;
            this.tbOuterR.Name = "tbOuterR";
            this.tbOuterR.Size = new Size(139, 25);
            this.tbOuterR.TabIndex = 25;
            this.tbOuterR.TickStyle = TickStyle.None;
            this.tbOuterR.Value = 20;
            this.tbOuterR.ValueChanged += new EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblOuterRadius
            // 
            this.lblOuterRadius.AutoSize = true;
            this.lblOuterRadius.Location = new Point(9, 6);
            this.lblOuterRadius.Name = "lblOuterRadius";
            this.lblOuterRadius.Size = new Size(69, 13);
            this.lblOuterRadius.TabIndex = 24;
            this.lblOuterRadius.Text = "Outer Radius";
            // 
            // chkBoxAnimate
            // 
            this.chkBoxAnimate.AutoSize = true;
            this.chkBoxAnimate.Location = new Point(3, 87);
            this.chkBoxAnimate.Name = "chkBoxAnimate";
            this.chkBoxAnimate.Size = new Size(109, 17);
            this.chkBoxAnimate.TabIndex = 30;
            this.chkBoxAnimate.Text = "Animate Distance";
            this.chkBoxAnimate.UseVisualStyleBackColor = true;
            this.chkBoxAnimate.CheckedChanged += new EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new Point(0, 107);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new Size(216, 13);
            this.lblNote.TabIndex = 31;
            this.lblNote.Text = "Note: Inner radius should be <= outer radius.";
            // 
            // Spirograph
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.chkBoxAnimate);
            this.Controls.Add(this.tbDistance);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.tbInnerR);
            this.Controls.Add(this.lblInnerRadius);
            this.Controls.Add(this.tbOuterR);
            this.Controls.Add(this.lblOuterRadius);
            this.Name = "Spirograph";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbDistance)).EndInit();
            ((ISupportInitialize)(this.tbInnerR)).EndInit();
            ((ISupportInitialize)(this.tbOuterR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbDistance;
        private Label lblDistance;
        private TrackBar tbInnerR;
        private Label lblInnerRadius;
        private TrackBar tbOuterR;
        private Label lblOuterRadius;
        private CheckBox chkBoxAnimate;
        private Label lblNote;
    }
}
