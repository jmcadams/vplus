namespace Meteors {
    partial class Meteors {
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
            this.tbSpacing = new System.Windows.Forms.TrackBar();
            this.lblTrailLength = new System.Windows.Forms.Label();
            this.tbGarlandType = new System.Windows.Forms.TrackBar();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSpacing
            // 
            this.tbSpacing.AutoSize = false;
            this.tbSpacing.Location = new System.Drawing.Point(90, 70);
            this.tbSpacing.Maximum = 100;
            this.tbSpacing.Name = "tbSpacing";
            this.tbSpacing.Size = new System.Drawing.Size(139, 25);
            this.tbSpacing.TabIndex = 9;
            this.tbSpacing.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblTrailLength
            // 
            this.lblTrailLength.AutoSize = true;
            this.lblTrailLength.Location = new System.Drawing.Point(12, 76);
            this.lblTrailLength.Name = "lblTrailLength";
            this.lblTrailLength.Size = new System.Drawing.Size(63, 13);
            this.lblTrailLength.TabIndex = 8;
            this.lblTrailLength.Text = "Trail Length";
            // 
            // tbGarlandType
            // 
            this.tbGarlandType.AutoSize = false;
            this.tbGarlandType.Location = new System.Drawing.Point(90, 39);
            this.tbGarlandType.Maximum = 100;
            this.tbGarlandType.Name = "tbGarlandType";
            this.tbGarlandType.Size = new System.Drawing.Size(139, 25);
            this.tbGarlandType.TabIndex = 7;
            this.tbGarlandType.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(40, 45);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 6;
            this.lblCount.Text = "Count";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(44, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 10;
            this.lblType.Text = "Type";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Rainbow",
            "Range",
            "Palette"});
            this.comboBox1.Location = new System.Drawing.Point(90, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(139, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // Meteors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbSpacing);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbGarlandType);
            this.Controls.Add(this.lblCount);
            this.Name = "Meteors";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbSpacing;
        private System.Windows.Forms.Label lblTrailLength;
        private System.Windows.Forms.TrackBar tbGarlandType;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
