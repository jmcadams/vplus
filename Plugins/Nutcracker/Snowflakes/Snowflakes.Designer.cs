namespace Snowflakes {
    partial class Snowflakes {
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
            this.tbMaxFlakes = new System.Windows.Forms.TrackBar();
            this.lblMaxFlakes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxFlakes)).BeginInit();
            this.SuspendLayout();
            // 
            // tbType
            // 
            this.tbType.AutoSize = false;
            this.tbType.Location = new System.Drawing.Point(90, 34);
            this.tbType.Maximum = 4;
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(139, 25);
            this.tbType.TabIndex = 13;
            this.tbType.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbType.ValueChanged += new System.EventHandler(this.Snowflakes_ControlChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(44, 40);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "Type";
            // 
            // tbMaxFlakes
            // 
            this.tbMaxFlakes.AutoSize = false;
            this.tbMaxFlakes.Location = new System.Drawing.Point(90, 3);
            this.tbMaxFlakes.Maximum = 20;
            this.tbMaxFlakes.Minimum = 1;
            this.tbMaxFlakes.Name = "tbMaxFlakes";
            this.tbMaxFlakes.Size = new System.Drawing.Size(139, 25);
            this.tbMaxFlakes.TabIndex = 11;
            this.tbMaxFlakes.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbMaxFlakes.Value = 1;
            this.tbMaxFlakes.ValueChanged += new System.EventHandler(this.Snowflakes_ControlChanged);
            // 
            // lblMaxFlakes
            // 
            this.lblMaxFlakes.AutoSize = true;
            this.lblMaxFlakes.Location = new System.Drawing.Point(14, 9);
            this.lblMaxFlakes.Name = "lblMaxFlakes";
            this.lblMaxFlakes.Size = new System.Drawing.Size(61, 13);
            this.lblMaxFlakes.TabIndex = 10;
            this.lblMaxFlakes.Text = "Max Flakes";
            // 
            // Snowflakes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbMaxFlakes);
            this.Controls.Add(this.lblMaxFlakes);
            this.Name = "Snowflakes";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxFlakes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TrackBar tbMaxFlakes;
        private System.Windows.Forms.Label lblMaxFlakes;
    }
}
