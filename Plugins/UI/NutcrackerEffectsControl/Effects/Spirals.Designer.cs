namespace NutcrackerEffects.Effects {
    partial class Spirals {
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
            this.tbRotations = new System.Windows.Forms.TrackBar();
            this.lblRotations = new System.Windows.Forms.Label();
            this.tbPaletteRepeat = new System.Windows.Forms.TrackBar();
            this.lblPaletteRepeat = new System.Windows.Forms.Label();
            this.tbDirection = new System.Windows.Forms.TrackBar();
            this.lblDirection = new System.Windows.Forms.Label();
            this.tbThickness = new System.Windows.Forms.TrackBar();
            this.lblThickness = new System.Windows.Forms.Label();
            this.chkBoxBlend = new System.Windows.Forms.CheckBox();
            this.chkBox3D = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPaletteRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).BeginInit();
            this.SuspendLayout();
            // 
            // tbRotations
            // 
            this.tbRotations.AutoSize = false;
            this.tbRotations.Location = new System.Drawing.Point(93, 28);
            this.tbRotations.Maximum = 50;
            this.tbRotations.Minimum = -50;
            this.tbRotations.Name = "tbRotations";
            this.tbRotations.Size = new System.Drawing.Size(139, 25);
            this.tbRotations.TabIndex = 21;
            this.tbRotations.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbRotations.Value = 20;
            this.tbRotations.ValueChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // lblRotations
            // 
            this.lblRotations.AutoSize = true;
            this.lblRotations.Location = new System.Drawing.Point(26, 33);
            this.lblRotations.Name = "lblRotations";
            this.lblRotations.Size = new System.Drawing.Size(52, 13);
            this.lblRotations.TabIndex = 20;
            this.lblRotations.Text = "Rotations";
            // 
            // tbPaletteRepeat
            // 
            this.tbPaletteRepeat.AutoSize = false;
            this.tbPaletteRepeat.Location = new System.Drawing.Point(93, 0);
            this.tbPaletteRepeat.Maximum = 5;
            this.tbPaletteRepeat.Minimum = 1;
            this.tbPaletteRepeat.Name = "tbPaletteRepeat";
            this.tbPaletteRepeat.Size = new System.Drawing.Size(139, 25);
            this.tbPaletteRepeat.TabIndex = 19;
            this.tbPaletteRepeat.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbPaletteRepeat.Value = 1;
            this.tbPaletteRepeat.ValueChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // lblPaletteRepeat
            // 
            this.lblPaletteRepeat.AutoSize = true;
            this.lblPaletteRepeat.Location = new System.Drawing.Point(0, 6);
            this.lblPaletteRepeat.Name = "lblPaletteRepeat";
            this.lblPaletteRepeat.Size = new System.Drawing.Size(78, 13);
            this.lblPaletteRepeat.TabIndex = 18;
            this.lblPaletteRepeat.Text = "Palette Repeat";
            // 
            // tbDirection
            // 
            this.tbDirection.AutoSize = false;
            this.tbDirection.Location = new System.Drawing.Point(93, 84);
            this.tbDirection.Maximum = 1;
            this.tbDirection.Minimum = -1;
            this.tbDirection.Name = "tbDirection";
            this.tbDirection.Size = new System.Drawing.Size(139, 25);
            this.tbDirection.TabIndex = 25;
            this.tbDirection.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbDirection.Value = 1;
            this.tbDirection.ValueChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(29, 87);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(49, 13);
            this.lblDirection.TabIndex = 24;
            this.lblDirection.Text = "Direction";
            // 
            // tbThickness
            // 
            this.tbThickness.AutoSize = false;
            this.tbThickness.Location = new System.Drawing.Point(93, 56);
            this.tbThickness.Maximum = 100;
            this.tbThickness.Minimum = 1;
            this.tbThickness.Name = "tbThickness";
            this.tbThickness.Size = new System.Drawing.Size(139, 25);
            this.tbThickness.TabIndex = 23;
            this.tbThickness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbThickness.Value = 50;
            this.tbThickness.ValueChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // lblThickness
            // 
            this.lblThickness.AutoSize = true;
            this.lblThickness.Location = new System.Drawing.Point(22, 60);
            this.lblThickness.Name = "lblThickness";
            this.lblThickness.Size = new System.Drawing.Size(56, 13);
            this.lblThickness.TabIndex = 22;
            this.lblThickness.Text = "Thickness";
            // 
            // chkBoxBlend
            // 
            this.chkBoxBlend.AutoSize = true;
            this.chkBoxBlend.Location = new System.Drawing.Point(18, 114);
            this.chkBoxBlend.Name = "chkBoxBlend";
            this.chkBoxBlend.Size = new System.Drawing.Size(53, 17);
            this.chkBoxBlend.TabIndex = 26;
            this.chkBoxBlend.Text = "Blend";
            this.chkBoxBlend.UseVisualStyleBackColor = true;
            this.chkBoxBlend.CheckedChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // chkBox3D
            // 
            this.chkBox3D.AutoSize = true;
            this.chkBox3D.Location = new System.Drawing.Point(93, 114);
            this.chkBox3D.Name = "chkBox3D";
            this.chkBox3D.Size = new System.Drawing.Size(40, 17);
            this.chkBox3D.TabIndex = 27;
            this.chkBox3D.Text = "3D";
            this.chkBox3D.UseVisualStyleBackColor = true;
            this.chkBox3D.CheckedChanged += new System.EventHandler(this.Spirals_ControlChanged);
            // 
            // Spirals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBox3D);
            this.Controls.Add(this.chkBoxBlend);
            this.Controls.Add(this.tbDirection);
            this.Controls.Add(this.lblDirection);
            this.Controls.Add(this.tbThickness);
            this.Controls.Add(this.lblThickness);
            this.Controls.Add(this.tbRotations);
            this.Controls.Add(this.lblRotations);
            this.Controls.Add(this.tbPaletteRepeat);
            this.Controls.Add(this.lblPaletteRepeat);
            this.Name = "Spirals";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbRotations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPaletteRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbRotations;
        private System.Windows.Forms.Label lblRotations;
        private System.Windows.Forms.TrackBar tbPaletteRepeat;
        private System.Windows.Forms.Label lblPaletteRepeat;
        private System.Windows.Forms.TrackBar tbDirection;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.TrackBar tbThickness;
        private System.Windows.Forms.Label lblThickness;
        private System.Windows.Forms.CheckBox chkBoxBlend;
        private System.Windows.Forms.CheckBox chkBox3D;
    }
}
