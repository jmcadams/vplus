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
            this.tbBranchCount = new System.Windows.Forms.TrackBar();
            this.lblBranchCount = new System.Windows.Forms.Label();
            this.tbGralands = new System.Windows.Forms.TrackBar();
            this.lblGralands = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbBranchCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGralands)).BeginInit();
            this.SuspendLayout();
            // 
            // tbBranchCount
            // 
            this.tbBranchCount.AutoSize = false;
            this.tbBranchCount.Location = new System.Drawing.Point(93, 0);
            this.tbBranchCount.Minimum = 1;
            this.tbBranchCount.Name = "tbBranchCount";
            this.tbBranchCount.Size = new System.Drawing.Size(139, 25);
            this.tbBranchCount.TabIndex = 21;
            this.tbBranchCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbBranchCount.Value = 3;
            this.tbBranchCount.ValueChanged += new System.EventHandler(this.Tree_ControlChanged);
            // 
            // lblBranchCount
            // 
            this.lblBranchCount.AutoSize = true;
            this.lblBranchCount.Location = new System.Drawing.Point(0, 6);
            this.lblBranchCount.Name = "lblBranchCount";
            this.lblBranchCount.Size = new System.Drawing.Size(74, 13);
            this.lblBranchCount.TabIndex = 20;
            this.lblBranchCount.Text = "# of Branches";
            // 
            // tbGralands
            // 
            this.tbGralands.AutoSize = false;
            this.tbGralands.Location = new System.Drawing.Point(93, 33);
            this.tbGralands.Maximum = 2;
            this.tbGralands.Minimum = 1;
            this.tbGralands.Name = "tbGralands";
            this.tbGralands.Size = new System.Drawing.Size(139, 25);
            this.tbGralands.TabIndex = 23;
            this.tbGralands.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGralands.Value = 1;
            this.tbGralands.ValueChanged += new System.EventHandler(this.Tree_ControlChanged);
            // 
            // lblGralands
            // 
            this.lblGralands.Location = new System.Drawing.Point(0, 30);
            this.lblGralands.Name = "lblGralands";
            this.lblGralands.Size = new System.Drawing.Size(74, 28);
            this.lblGralands.TabIndex = 22;
            this.lblGralands.Text = "# of Garlands per branch";
            // 
            // Tree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbGralands);
            this.Controls.Add(this.lblGralands);
            this.Controls.Add(this.tbBranchCount);
            this.Controls.Add(this.lblBranchCount);
            this.Name = "Tree";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbBranchCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGralands)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbBranchCount;
        private System.Windows.Forms.Label lblBranchCount;
        private System.Windows.Forms.TrackBar tbGralands;
        private System.Windows.Forms.Label lblGralands;
    }
}
