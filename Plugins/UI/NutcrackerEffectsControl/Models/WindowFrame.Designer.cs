namespace Nutcracker.Models {
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
            this.nudBottomCount = new System.Windows.Forms.NumericUpDown();
            this.nudSideCount = new System.Windows.Forms.NumericUpDown();
            this.lblSideCount = new System.Windows.Forms.Label();
            this.lblBottomCount = new System.Windows.Forms.Label();
            this.nudTopCount = new System.Windows.Forms.NumericUpDown();
            this.lblTopCount = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSideCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // nudBottomCount
            // 
            this.nudBottomCount.Location = new System.Drawing.Point(276, 99);
            this.nudBottomCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBottomCount.Name = "nudBottomCount";
            this.nudBottomCount.Size = new System.Drawing.Size(71, 20);
            this.nudBottomCount.TabIndex = 10;
            this.nudBottomCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBottomCount.ValueChanged += new System.EventHandler(this.control_ValueChanged);
            // 
            // nudSideCount
            // 
            this.nudSideCount.Location = new System.Drawing.Point(276, 59);
            this.nudSideCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSideCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSideCount.Name = "nudSideCount";
            this.nudSideCount.Size = new System.Drawing.Size(71, 20);
            this.nudSideCount.TabIndex = 9;
            this.nudSideCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudSideCount.ValueChanged += new System.EventHandler(this.control_ValueChanged);
            // 
            // lblSideCount
            // 
            this.lblSideCount.AutoSize = true;
            this.lblSideCount.Location = new System.Drawing.Point(213, 43);
            this.lblSideCount.Name = "lblSideCount";
            this.lblSideCount.Size = new System.Drawing.Size(134, 13);
            this.lblSideCount.TabIndex = 13;
            this.lblSideCount.Text = "Number of Nodes on Sides";
            // 
            // lblBottomCount
            // 
            this.lblBottomCount.AutoSize = true;
            this.lblBottomCount.Location = new System.Drawing.Point(206, 82);
            this.lblBottomCount.Name = "lblBottomCount";
            this.lblBottomCount.Size = new System.Drawing.Size(141, 13);
            this.lblBottomCount.TabIndex = 14;
            this.lblBottomCount.Text = "Number of Nodes on Bottom";
            // 
            // nudTopCount
            // 
            this.nudTopCount.Location = new System.Drawing.Point(276, 20);
            this.nudTopCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopCount.Name = "nudTopCount";
            this.nudTopCount.Size = new System.Drawing.Size(71, 20);
            this.nudTopCount.TabIndex = 8;
            this.nudTopCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopCount.ValueChanged += new System.EventHandler(this.control_ValueChanged);
            // 
            // lblTopCount
            // 
            this.lblTopCount.AutoSize = true;
            this.lblTopCount.Location = new System.Drawing.Point(220, 4);
            this.lblTopCount.Name = "lblTopCount";
            this.lblTopCount.Size = new System.Drawing.Size(127, 13);
            this.lblTopCount.TabIndex = 12;
            this.lblTopCount.Text = "Number of Nodes on Top";
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
            this.Controls.Add(this.nudBottomCount);
            this.Controls.Add(this.nudSideCount);
            this.Controls.Add(this.lblSideCount);
            this.Controls.Add(this.lblBottomCount);
            this.Controls.Add(this.nudTopCount);
            this.Controls.Add(this.lblTopCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "WindowFrame";
            this.Size = new System.Drawing.Size(350, 250);
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSideCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudBottomCount;
        private System.Windows.Forms.NumericUpDown nudSideCount;
        private System.Windows.Forms.Label lblSideCount;
        private System.Windows.Forms.Label lblBottomCount;
        private System.Windows.Forms.NumericUpDown nudTopCount;
        private System.Windows.Forms.Label lblTopCount;
        private System.Windows.Forms.PictureBox pbPreview;
    }
}
