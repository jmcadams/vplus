namespace Spirograph {
    partial class Spirograph {
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
            this.tbThickness = new System.Windows.Forms.TrackBar();
            this.lblDistance = new System.Windows.Forms.Label();
            this.tbRotations = new System.Windows.Forms.TrackBar();
            this.lblInnerRadius = new System.Windows.Forms.Label();
            this.tbPaletteRepeat = new System.Windows.Forms.TrackBar();
            this.lblOuterRadius = new System.Windows.Forms.Label();
            this.chkBoxAnimate = new System.Windows.Forms.CheckBox();
            this.lblNote = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPaletteRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // tbThickness
            // 
            this.tbThickness.AutoSize = false;
            this.tbThickness.Location = new System.Drawing.Point(93, 56);
            this.tbThickness.Maximum = 100;
            this.tbThickness.Name = "tbThickness";
            this.tbThickness.Size = new System.Drawing.Size(139, 25);
            this.tbThickness.TabIndex = 29;
            this.tbThickness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbThickness.ValueChanged += new System.EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(29, 60);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(49, 13);
            this.lblDistance.TabIndex = 28;
            this.lblDistance.Text = "Distance";
            // 
            // tbRotations
            // 
            this.tbRotations.AutoSize = false;
            this.tbRotations.Location = new System.Drawing.Point(93, 28);
            this.tbRotations.Maximum = 100;
            this.tbRotations.Name = "tbRotations";
            this.tbRotations.Size = new System.Drawing.Size(139, 25);
            this.tbRotations.TabIndex = 27;
            this.tbRotations.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbRotations.ValueChanged += new System.EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblInnerRadius
            // 
            this.lblInnerRadius.AutoSize = true;
            this.lblInnerRadius.Location = new System.Drawing.Point(11, 33);
            this.lblInnerRadius.Name = "lblInnerRadius";
            this.lblInnerRadius.Size = new System.Drawing.Size(67, 13);
            this.lblInnerRadius.TabIndex = 26;
            this.lblInnerRadius.Text = "Inner Radius";
            // 
            // tbPaletteRepeat
            // 
            this.tbPaletteRepeat.AutoSize = false;
            this.tbPaletteRepeat.Location = new System.Drawing.Point(93, 0);
            this.tbPaletteRepeat.Maximum = 4;
            this.tbPaletteRepeat.Name = "tbPaletteRepeat";
            this.tbPaletteRepeat.Size = new System.Drawing.Size(139, 25);
            this.tbPaletteRepeat.TabIndex = 25;
            this.tbPaletteRepeat.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbPaletteRepeat.ValueChanged += new System.EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblOuterRadius
            // 
            this.lblOuterRadius.AutoSize = true;
            this.lblOuterRadius.Location = new System.Drawing.Point(9, 6);
            this.lblOuterRadius.Name = "lblOuterRadius";
            this.lblOuterRadius.Size = new System.Drawing.Size(69, 13);
            this.lblOuterRadius.TabIndex = 24;
            this.lblOuterRadius.Text = "Outer Radius";
            // 
            // chkBoxAnimate
            // 
            this.chkBoxAnimate.AutoSize = true;
            this.chkBoxAnimate.Location = new System.Drawing.Point(3, 87);
            this.chkBoxAnimate.Name = "chkBoxAnimate";
            this.chkBoxAnimate.Size = new System.Drawing.Size(109, 17);
            this.chkBoxAnimate.TabIndex = 30;
            this.chkBoxAnimate.Text = "Animate Distance";
            this.chkBoxAnimate.UseVisualStyleBackColor = true;
            this.chkBoxAnimate.CheckedChanged += new System.EventHandler(this.Spirograph_ControlChanged);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(0, 107);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(216, 13);
            this.lblNote.TabIndex = 31;
            this.lblNote.Text = "Note: Inner radius should be <= outer radius.";
            // 
            // Spirograph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.chkBoxAnimate);
            this.Controls.Add(this.tbThickness);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.tbRotations);
            this.Controls.Add(this.lblInnerRadius);
            this.Controls.Add(this.tbPaletteRepeat);
            this.Controls.Add(this.lblOuterRadius);
            this.Name = "Spirograph";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPaletteRepeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbThickness;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.TrackBar tbRotations;
        private System.Windows.Forms.Label lblInnerRadius;
        private System.Windows.Forms.TrackBar tbPaletteRepeat;
        private System.Windows.Forms.Label lblOuterRadius;
        private System.Windows.Forms.CheckBox chkBoxAnimate;
        private System.Windows.Forms.Label lblNote;
    }
}
