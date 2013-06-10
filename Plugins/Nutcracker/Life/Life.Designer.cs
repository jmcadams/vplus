namespace Life {
    partial class Life {
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
            this.tbType = new System.Windows.Forms.TrackBar();
            this.lblType = new System.Windows.Forms.Label();
            this.tbCellsToStart = new System.Windows.Forms.TrackBar();
            this.lblCellsToStart = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCellsToStart)).BeginInit();
            this.SuspendLayout();
            // 
            // tbType
            // 
            this.tbType.AutoSize = false;
            this.tbType.Location = new System.Drawing.Point(90, 34);
            this.tbType.Maximum = 4;
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(139, 25);
            this.tbType.TabIndex = 9;
            this.tbType.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(39, 40);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type";
            // 
            // tbCellsToStart
            // 
            this.tbCellsToStart.AutoSize = false;
            this.tbCellsToStart.Location = new System.Drawing.Point(90, 3);
            this.tbCellsToStart.Maximum = 100;
            this.tbCellsToStart.Name = "tbCellsToStart";
            this.tbCellsToStart.Size = new System.Drawing.Size(139, 25);
            this.tbCellsToStart.TabIndex = 7;
            this.tbCellsToStart.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblCellsToStart
            // 
            this.lblCellsToStart.AutoSize = true;
            this.lblCellsToStart.Location = new System.Drawing.Point(4, 9);
            this.lblCellsToStart.Name = "lblCellsToStart";
            this.lblCellsToStart.Size = new System.Drawing.Size(66, 13);
            this.lblCellsToStart.TabIndex = 6;
            this.lblCellsToStart.Text = "Cells to Start";
            // 
            // Life
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbCellsToStart);
            this.Controls.Add(this.lblCellsToStart);
            this.Name = "Life";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCellsToStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TrackBar tbCellsToStart;
        private System.Windows.Forms.Label lblCellsToStart;
    }
}
