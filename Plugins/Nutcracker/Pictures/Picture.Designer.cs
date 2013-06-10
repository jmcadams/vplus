namespace Pictures {
    partial class Picture {
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
            this.tbGifSpeed = new System.Windows.Forms.TrackBar();
            this.lblGifSpeed = new System.Windows.Forms.Label();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtBoxFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbGifSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // tbGifSpeed
            // 
            this.tbGifSpeed.AutoSize = false;
            this.tbGifSpeed.Location = new System.Drawing.Point(93, 86);
            this.tbGifSpeed.Maximum = 100;
            this.tbGifSpeed.Name = "tbGifSpeed";
            this.tbGifSpeed.Size = new System.Drawing.Size(139, 25);
            this.tbGifSpeed.TabIndex = 11;
            this.tbGifSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblGifSpeed
            // 
            this.lblGifSpeed.AutoSize = true;
            this.lblGifSpeed.Location = new System.Drawing.Point(15, 92);
            this.lblGifSpeed.Name = "lblGifSpeed";
            this.lblGifSpeed.Size = new System.Drawing.Size(56, 13);
            this.lblGifSpeed.TabIndex = 10;
            this.lblGifSpeed.Text = "GIF speed";
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(3, 3);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 12;
            this.btnFile.Text = "Choose File";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // txtBoxFile
            // 
            this.txtBoxFile.Enabled = false;
            this.txtBoxFile.Location = new System.Drawing.Point(0, 33);
            this.txtBoxFile.Name = "txtBoxFile";
            this.txtBoxFile.Size = new System.Drawing.Size(232, 20);
            this.txtBoxFile.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Direction";
            // 
            // cbDirection
            // 
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down",
            "None"});
            this.cbDirection.Location = new System.Drawing.Point(93, 59);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(139, 21);
            this.cbDirection.TabIndex = 15;
            // 
            // Picture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxFile);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.tbGifSpeed);
            this.Controls.Add(this.lblGifSpeed);
            this.Name = "Picture";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbGifSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbGifSpeed;
        private System.Windows.Forms.Label lblGifSpeed;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtBoxFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDirection;

    }
}
