namespace NutcrackerEffects {
    partial class Bars {
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
            this.lblPaletteRep = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lblDirection = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cbHighlight = new System.Windows.Forms.CheckBox();
            this.cb3D = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPaletteRep
            // 
            this.lblPaletteRep.AutoSize = true;
            this.lblPaletteRep.Location = new System.Drawing.Point(2, 8);
            this.lblPaletteRep.Name = "lblPaletteRep";
            this.lblPaletteRep.Size = new System.Drawing.Size(78, 13);
            this.lblPaletteRep.TabIndex = 0;
            this.lblPaletteRep.Text = "Palette Repeat";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 2;
            this.trackBar1.Location = new System.Drawing.Point(79, 2);
            this.trackBar1.Maximum = 4;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(150, 25);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TabStop = false;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(31, 39);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(49, 13);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Expand",
            "Compress"});
            this.comboBox1.Location = new System.Drawing.Point(86, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // cbHighlight
            // 
            this.cbHighlight.AutoSize = true;
            this.cbHighlight.Location = new System.Drawing.Point(13, 66);
            this.cbHighlight.Name = "cbHighlight";
            this.cbHighlight.Size = new System.Drawing.Size(67, 17);
            this.cbHighlight.TabIndex = 4;
            this.cbHighlight.Text = "Highlight";
            this.cbHighlight.UseVisualStyleBackColor = true;
            // 
            // cb3D
            // 
            this.cb3D.AutoSize = true;
            this.cb3D.Location = new System.Drawing.Point(13, 89);
            this.cb3D.Name = "cb3D";
            this.cb3D.Size = new System.Drawing.Size(40, 17);
            this.cb3D.TabIndex = 5;
            this.cb3D.Text = "3D";
            this.cb3D.UseVisualStyleBackColor = true;
            // 
            // Bars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cb3D);
            this.Controls.Add(this.cbHighlight);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblDirection);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lblPaletteRep);
            this.Name = "Bars";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPaletteRep;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox cbHighlight;
        private System.Windows.Forms.CheckBox cb3D;
    }
}
