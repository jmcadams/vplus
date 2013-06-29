namespace Matrix2 {
    partial class Matrix {
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
            this.gbOrientation = new System.Windows.Forms.GroupBox();
            this.rbVertical = new System.Windows.Forms.RadioButton();
            this.rbHorizontal = new System.Windows.Forms.RadioButton();
            this.nudStrandCount = new System.Windows.Forms.NumericUpDown();
            this.nudNodeCount = new System.Windows.Forms.NumericUpDown();
            this.lblNodeCount = new System.Windows.Forms.Label();
            this.lblStandCount = new System.Windows.Forms.Label();
            this.nudStringCount = new System.Windows.Forms.NumericUpDown();
            this.lblStringCount = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOrientation
            // 
            this.gbOrientation.Controls.Add(this.rbVertical);
            this.gbOrientation.Controls.Add(this.rbHorizontal);
            this.gbOrientation.Location = new System.Drawing.Point(240, 125);
            this.gbOrientation.Name = "gbOrientation";
            this.gbOrientation.Size = new System.Drawing.Size(107, 67);
            this.gbOrientation.TabIndex = 19;
            this.gbOrientation.TabStop = false;
            this.gbOrientation.Text = "Orientation";
            // 
            // rbVertical
            // 
            this.rbVertical.AutoSize = true;
            this.rbVertical.Location = new System.Drawing.Point(7, 43);
            this.rbVertical.Name = "rbVertical";
            this.rbVertical.Size = new System.Drawing.Size(60, 17);
            this.rbVertical.TabIndex = 1;
            this.rbVertical.Text = "Vertical";
            this.rbVertical.UseVisualStyleBackColor = true;
            // 
            // rbHorizontal
            // 
            this.rbHorizontal.AutoSize = true;
            this.rbHorizontal.Location = new System.Drawing.Point(7, 20);
            this.rbHorizontal.Name = "rbHorizontal";
            this.rbHorizontal.Size = new System.Drawing.Size(72, 17);
            this.rbHorizontal.TabIndex = 0;
            this.rbHorizontal.Text = "Horizontal";
            this.rbHorizontal.UseVisualStyleBackColor = true;
            // 
            // nudStrandCount
            // 
            this.nudStrandCount.Location = new System.Drawing.Point(276, 99);
            this.nudStrandCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudStrandCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStrandCount.Name = "nudStrandCount";
            this.nudStrandCount.Size = new System.Drawing.Size(71, 20);
            this.nudStrandCount.TabIndex = 18;
            this.nudStrandCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            // 
            // lblNodeCount
            // 
            this.lblNodeCount.AutoSize = true;
            this.lblNodeCount.Location = new System.Drawing.Point(209, 43);
            this.lblNodeCount.Name = "lblNodeCount";
            this.lblNodeCount.Size = new System.Drawing.Size(138, 13);
            this.lblNodeCount.TabIndex = 21;
            this.lblNodeCount.Text = "Number of Nodes per String";
            // 
            // lblStandCount
            // 
            this.lblStandCount.AutoSize = true;
            this.lblStandCount.Location = new System.Drawing.Point(204, 82);
            this.lblStandCount.Name = "lblStandCount";
            this.lblStandCount.Size = new System.Drawing.Size(143, 13);
            this.lblStandCount.TabIndex = 22;
            this.lblStandCount.Text = "Number of Strands per String";
            // 
            // nudStringCount
            // 
            this.nudStringCount.Location = new System.Drawing.Point(276, 20);
            this.nudStringCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStringCount.Name = "nudStringCount";
            this.nudStringCount.Size = new System.Drawing.Size(71, 20);
            this.nudStringCount.TabIndex = 16;
            this.nudStringCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStringCount
            // 
            this.lblStringCount.AutoSize = true;
            this.lblStringCount.Location = new System.Drawing.Point(223, 4);
            this.lblStringCount.Name = "lblStringCount";
            this.lblStringCount.Size = new System.Drawing.Size(124, 13);
            this.lblStringCount.TabIndex = 20;
            this.lblStringCount.Text = "Actual Number of Strings";
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
            // Matrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOrientation);
            this.Controls.Add(this.nudStrandCount);
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.lblStandCount);
            this.Controls.Add(this.nudStringCount);
            this.Controls.Add(this.lblStringCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Matrix";
            this.Size = new System.Drawing.Size(350, 250);
            this.gbOrientation.ResumeLayout(false);
            this.gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOrientation;
        private System.Windows.Forms.RadioButton rbVertical;
        private System.Windows.Forms.RadioButton rbHorizontal;
        private System.Windows.Forms.NumericUpDown nudStrandCount;
        private System.Windows.Forms.NumericUpDown nudNodeCount;
        private System.Windows.Forms.Label lblNodeCount;
        private System.Windows.Forms.Label lblStandCount;
        private System.Windows.Forms.NumericUpDown nudStringCount;
        private System.Windows.Forms.Label lblStringCount;
        private System.Windows.Forms.PictureBox pbPreview;
    }
}
