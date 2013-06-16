namespace Garlands {
    partial class Garlands {
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
            this.tbGarlandType = new System.Windows.Forms.TrackBar();
            this.lblGarlandType = new System.Windows.Forms.Label();
            this.tbSpacing = new System.Windows.Forms.TrackBar();
            this.lblSpacing = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).BeginInit();
            this.SuspendLayout();
            // 
            // tbGarlandType
            // 
            this.tbGarlandType.AutoSize = false;
            this.tbGarlandType.Location = new System.Drawing.Point(90, 3);
            this.tbGarlandType.Maximum = 100;
            this.tbGarlandType.Name = "tbGarlandType";
            this.tbGarlandType.Size = new System.Drawing.Size(139, 25);
            this.tbGarlandType.TabIndex = 3;
            this.tbGarlandType.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGarlandType.ValueChanged += new System.EventHandler(this.Garlands_ControlChanged);
            // 
            // lblGarlandType
            // 
            this.lblGarlandType.AutoSize = true;
            this.lblGarlandType.Location = new System.Drawing.Point(4, 9);
            this.lblGarlandType.Name = "lblGarlandType";
            this.lblGarlandType.Size = new System.Drawing.Size(71, 13);
            this.lblGarlandType.TabIndex = 2;
            this.lblGarlandType.Text = "Garland Type";
            // 
            // tbSpacing
            // 
            this.tbSpacing.AutoSize = false;
            this.tbSpacing.Location = new System.Drawing.Point(90, 34);
            this.tbSpacing.Maximum = 100;
            this.tbSpacing.Name = "tbSpacing";
            this.tbSpacing.Size = new System.Drawing.Size(139, 25);
            this.tbSpacing.TabIndex = 5;
            this.tbSpacing.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSpacing.ValueChanged += new System.EventHandler(this.Garlands_ControlChanged);
            // 
            // lblSpacing
            // 
            this.lblSpacing.AutoSize = true;
            this.lblSpacing.Location = new System.Drawing.Point(29, 40);
            this.lblSpacing.Name = "lblSpacing";
            this.lblSpacing.Size = new System.Drawing.Size(46, 13);
            this.lblSpacing.TabIndex = 4;
            this.lblSpacing.Text = "Spacing";
            // 
            // Garlands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSpacing);
            this.Controls.Add(this.lblSpacing);
            this.Controls.Add(this.tbGarlandType);
            this.Controls.Add(this.lblGarlandType);
            this.Name = "Garlands";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbGarlandType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpacing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbGarlandType;
        private System.Windows.Forms.Label lblGarlandType;
        private System.Windows.Forms.TrackBar tbSpacing;
        private System.Windows.Forms.Label lblSpacing;
    }
}
