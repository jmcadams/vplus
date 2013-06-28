namespace Tree {
    partial class Tree {
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
            this.lblStandCount = new System.Windows.Forms.Label();
            this.nudNodeCount = new System.Windows.Forms.NumericUpDown();
            this.lblNodeCount = new System.Windows.Forms.Label();
            this.nudStringCount = new System.Windows.Forms.NumericUpDown();
            this.lblStringCount = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb360 = new System.Windows.Forms.RadioButton();
            this.rb270 = new System.Windows.Forms.RadioButton();
            this.rb180 = new System.Windows.Forms.RadioButton();
            this.rb90 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.nudStrandCount.TabIndex = 2;
            this.nudStrandCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStandCount
            // 
            this.lblStandCount.AutoSize = true;
            this.lblStandCount.Location = new System.Drawing.Point(204, 82);
            this.lblStandCount.Name = "lblStandCount";
            this.lblStandCount.Size = new System.Drawing.Size(143, 13);
            this.lblStandCount.TabIndex = 6;
            this.lblStandCount.Text = "Number of Strands per String";
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
            this.nudNodeCount.TabIndex = 1;
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
            this.lblNodeCount.TabIndex = 5;
            this.lblNodeCount.Text = "Number of Nodes per String";
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
            this.nudStringCount.TabIndex = 0;
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
            this.lblStringCount.TabIndex = 4;
            this.lblStringCount.Text = "Actual Number of Strings";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(172, 244);
            this.pbPreview.TabIndex = 7;
            this.pbPreview.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb90);
            this.groupBox1.Controls.Add(this.rb180);
            this.groupBox1.Controls.Add(this.rb270);
            this.groupBox1.Controls.Add(this.rb360);
            this.groupBox1.Location = new System.Drawing.Point(240, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(107, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visible Degrees";
            // 
            // rb360
            // 
            this.rb360.AutoSize = true;
            this.rb360.Location = new System.Drawing.Point(7, 20);
            this.rb360.Name = "rb360";
            this.rb360.Size = new System.Drawing.Size(43, 17);
            this.rb360.TabIndex = 0;
            this.rb360.Text = "360";
            this.rb360.UseVisualStyleBackColor = true;
            // 
            // rb270
            // 
            this.rb270.AutoSize = true;
            this.rb270.Location = new System.Drawing.Point(56, 20);
            this.rb270.Name = "rb270";
            this.rb270.Size = new System.Drawing.Size(43, 17);
            this.rb270.TabIndex = 1;
            this.rb270.Text = "270";
            this.rb270.UseVisualStyleBackColor = true;
            // 
            // rb180
            // 
            this.rb180.AutoSize = true;
            this.rb180.Checked = true;
            this.rb180.Location = new System.Drawing.Point(7, 43);
            this.rb180.Name = "rb180";
            this.rb180.Size = new System.Drawing.Size(43, 17);
            this.rb180.TabIndex = 2;
            this.rb180.TabStop = true;
            this.rb180.Text = "180";
            this.rb180.UseVisualStyleBackColor = true;
            // 
            // rb90
            // 
            this.rb90.AutoSize = true;
            this.rb90.Location = new System.Drawing.Point(56, 44);
            this.rb90.Name = "rb90";
            this.rb90.Size = new System.Drawing.Size(37, 17);
            this.rb90.TabIndex = 3;
            this.rb90.Text = "90";
            this.rb90.UseVisualStyleBackColor = true;
            // 
            // Tree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nudStrandCount);
            this.Controls.Add(this.lblStandCount);
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.nudStringCount);
            this.Controls.Add(this.lblStringCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Tree";
            this.Size = new System.Drawing.Size(350, 250);
            ((System.ComponentModel.ISupportInitialize)(this.nudStrandCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStringCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudStrandCount;
        private System.Windows.Forms.Label lblStandCount;
        private System.Windows.Forms.NumericUpDown nudNodeCount;
        private System.Windows.Forms.Label lblNodeCount;
        private System.Windows.Forms.NumericUpDown nudStringCount;
        private System.Windows.Forms.Label lblStringCount;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb90;
        private System.Windows.Forms.RadioButton rb180;
        private System.Windows.Forms.RadioButton rb270;
        private System.Windows.Forms.RadioButton rb360;
    }
}
