namespace WindowFrame {
    partial class WindowFrame {
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
            this.nudStrandCount = new System.Windows.Forms.NumericUpDown();
            this.nudNodeCount = new System.Windows.Forms.NumericUpDown();
            this.lblNodeCount = new System.Windows.Forms.Label();
            this.lblStandCount = new System.Windows.Forms.Label();
            this.nudStringCount = new System.Windows.Forms.NumericUpDown();
            this.lblStringCount = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
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
            this.nudStrandCount.TabIndex = 10;
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
            this.nudNodeCount.TabIndex = 9;
            this.nudNodeCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblNodeCount
            // 
            this.lblNodeCount.AutoSize = true;
            this.lblNodeCount.Location = new System.Drawing.Point(213, 43);
            this.lblNodeCount.Name = "lblNodeCount";
            this.lblNodeCount.Size = new System.Drawing.Size(134, 13);
            this.lblNodeCount.TabIndex = 13;
            this.lblNodeCount.Text = "Number of Nodes on Sides";
            // 
            // lblStandCount
            // 
            this.lblStandCount.AutoSize = true;
            this.lblStandCount.Location = new System.Drawing.Point(206, 82);
            this.lblStandCount.Name = "lblStandCount";
            this.lblStandCount.Size = new System.Drawing.Size(141, 13);
            this.lblStandCount.TabIndex = 14;
            this.lblStandCount.Text = "Number of Nodes on Bottom";
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
            this.nudStringCount.TabIndex = 8;
            this.nudStringCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStringCount
            // 
            this.lblStringCount.AutoSize = true;
            this.lblStringCount.Location = new System.Drawing.Point(220, 4);
            this.lblStringCount.Name = "lblStringCount";
            this.lblStringCount.Size = new System.Drawing.Size(127, 13);
            this.lblStringCount.TabIndex = 12;
            this.lblStringCount.Text = "Number of Nodes on Top";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(172, 244);
            this.pbPreview.TabIndex = 15;
            this.pbPreview.TabStop = false;
            // 
            // WindowFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudStrandCount);
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.lblStandCount);
            this.Controls.Add(this.nudStringCount);
            this.Controls.Add(this.lblStringCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "WindowFrame";
            this.Size = new System.Drawing.Size(350, 250);
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudStrandCount;
        private System.Windows.Forms.NumericUpDown nudNodeCount;
        private System.Windows.Forms.Label lblNodeCount;
        private System.Windows.Forms.Label lblStandCount;
        private System.Windows.Forms.NumericUpDown nudStringCount;
        private System.Windows.Forms.Label lblStringCount;
        private System.Windows.Forms.PictureBox pbPreview;
    }
}
