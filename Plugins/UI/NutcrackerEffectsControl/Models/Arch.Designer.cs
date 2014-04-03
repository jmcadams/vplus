namespace NutcrackerEffects.Models {
    partial class Arch {
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
            this.nudNodeCount = new System.Windows.Forms.NumericUpDown();
            this.lblNodeCount = new System.Windows.Forms.Label();
            this.nudArchCount = new System.Windows.Forms.NumericUpDown();
            this.lblArchCount = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNodeCount
            // 
            this.nudNodeCount.Location = new System.Drawing.Point(276, 59);
            this.nudNodeCount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudNodeCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNodeCount.Name = "nudNodeCount";
            this.nudNodeCount.Size = new System.Drawing.Size(71, 20);
            this.nudNodeCount.TabIndex = 17;
            this.nudNodeCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudNodeCount.ValueChanged += new System.EventHandler(this.control_ValueChanged);
            // 
            // lblNodeCount
            // 
            this.lblNodeCount.AutoSize = true;
            this.lblNodeCount.Location = new System.Drawing.Point(214, 43);
            this.lblNodeCount.Name = "lblNodeCount";
            this.lblNodeCount.Size = new System.Drawing.Size(133, 13);
            this.lblNodeCount.TabIndex = 21;
            this.lblNodeCount.Text = "Number of Nodes per Arch";
            // 
            // nudArchCount
            // 
            this.nudArchCount.Location = new System.Drawing.Point(276, 20);
            this.nudArchCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchCount.Name = "nudArchCount";
            this.nudArchCount.Size = new System.Drawing.Size(71, 20);
            this.nudArchCount.TabIndex = 16;
            this.nudArchCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchCount.ValueChanged += new System.EventHandler(this.control_ValueChanged);
            // 
            // lblArchCount
            // 
            this.lblArchCount.AutoSize = true;
            this.lblArchCount.Location = new System.Drawing.Point(255, 4);
            this.lblArchCount.Name = "lblArchCount";
            this.lblArchCount.Size = new System.Drawing.Size(92, 13);
            this.lblArchCount.TabIndex = 20;
            this.lblArchCount.Text = "Number of Arches";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(172, 244);
            this.pbPreview.TabIndex = 23;
            this.pbPreview.TabStop = false;
            // 
            // Arch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.nudArchCount);
            this.Controls.Add(this.lblArchCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Arch";
            this.Size = new System.Drawing.Size(350, 250);
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudNodeCount;
        private System.Windows.Forms.Label lblNodeCount;
        private System.Windows.Forms.NumericUpDown nudArchCount;
        private System.Windows.Forms.Label lblArchCount;
        private System.Windows.Forms.PictureBox pbPreview;
    }
}
