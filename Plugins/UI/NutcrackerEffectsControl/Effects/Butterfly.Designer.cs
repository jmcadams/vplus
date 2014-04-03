namespace NutcrackerEffects.Effects {
    partial class Butterfly {
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
            this.lblColors = new System.Windows.Forms.Label();
            this.lblStyle = new System.Windows.Forms.Label();
            this.lblBgChunks = new System.Windows.Forms.Label();
            this.lblBgSkip = new System.Windows.Forms.Label();
            this.tbSkip = new System.Windows.Forms.TrackBar();
            this.tbChunks = new System.Windows.Forms.TrackBar();
            this.tbStyle = new System.Windows.Forms.TrackBar();
            this.cbColors = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbSkip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbChunks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStyle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblColors
            // 
            this.lblColors.AutoSize = true;
            this.lblColors.Location = new System.Drawing.Point(71, 3);
            this.lblColors.Name = "lblColors";
            this.lblColors.Size = new System.Drawing.Size(36, 13);
            this.lblColors.TabIndex = 0;
            this.lblColors.Text = "Colors";
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new System.Drawing.Point(77, 31);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(30, 13);
            this.lblStyle.TabIndex = 1;
            this.lblStyle.Text = "Style";
            // 
            // lblBgChunks
            // 
            this.lblBgChunks.AutoSize = true;
            this.lblBgChunks.Location = new System.Drawing.Point(3, 62);
            this.lblBgChunks.Name = "lblBgChunks";
            this.lblBgChunks.Size = new System.Drawing.Size(104, 13);
            this.lblBgChunks.TabIndex = 2;
            this.lblBgChunks.Text = "Background Chunks";
            // 
            // lblBgSkip
            // 
            this.lblBgSkip.AutoSize = true;
            this.lblBgSkip.Location = new System.Drawing.Point(17, 93);
            this.lblBgSkip.Name = "lblBgSkip";
            this.lblBgSkip.Size = new System.Drawing.Size(89, 13);
            this.lblBgSkip.TabIndex = 3;
            this.lblBgSkip.Text = "Background Skip";
            // 
            // tbSkip
            // 
            this.tbSkip.AutoSize = false;
            this.tbSkip.Location = new System.Drawing.Point(112, 87);
            this.tbSkip.Minimum = 2;
            this.tbSkip.Name = "tbSkip";
            this.tbSkip.Size = new System.Drawing.Size(117, 25);
            this.tbSkip.TabIndex = 4;
            this.tbSkip.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSkip.Value = 2;
            this.tbSkip.ValueChanged += new System.EventHandler(this.Butterfly_ControlChanged);
            // 
            // tbChunks
            // 
            this.tbChunks.AutoSize = false;
            this.tbChunks.Location = new System.Drawing.Point(112, 56);
            this.tbChunks.Minimum = 1;
            this.tbChunks.Name = "tbChunks";
            this.tbChunks.Size = new System.Drawing.Size(117, 25);
            this.tbChunks.TabIndex = 5;
            this.tbChunks.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbChunks.Value = 1;
            this.tbChunks.ValueChanged += new System.EventHandler(this.Butterfly_ControlChanged);
            // 
            // tbStyle
            // 
            this.tbStyle.AutoSize = false;
            this.tbStyle.Location = new System.Drawing.Point(112, 25);
            this.tbStyle.Maximum = 3;
            this.tbStyle.Minimum = 1;
            this.tbStyle.Name = "tbStyle";
            this.tbStyle.Size = new System.Drawing.Size(117, 25);
            this.tbStyle.TabIndex = 6;
            this.tbStyle.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbStyle.Value = 1;
            this.tbStyle.ValueChanged += new System.EventHandler(this.Butterfly_ControlChanged);
            // 
            // cbColors
            // 
            this.cbColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColors.FormattingEnabled = true;
            this.cbColors.Items.AddRange(new object[] {
            "Rainbow",
            "Palette"});
            this.cbColors.Location = new System.Drawing.Point(112, 0);
            this.cbColors.Name = "cbColors";
            this.cbColors.Size = new System.Drawing.Size(117, 21);
            this.cbColors.TabIndex = 7;
            this.cbColors.SelectedIndexChanged += new System.EventHandler(this.Butterfly_ControlChanged);
            // 
            // Butterfly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbColors);
            this.Controls.Add(this.tbStyle);
            this.Controls.Add(this.tbChunks);
            this.Controls.Add(this.tbSkip);
            this.Controls.Add(this.lblBgSkip);
            this.Controls.Add(this.lblBgChunks);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.lblColors);
            this.Name = "Butterfly";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbSkip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbChunks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStyle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblColors;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.Label lblBgChunks;
        private System.Windows.Forms.Label lblBgSkip;
        private System.Windows.Forms.TrackBar tbSkip;
        private System.Windows.Forms.TrackBar tbChunks;
        private System.Windows.Forms.TrackBar tbStyle;
        private System.Windows.Forms.ComboBox cbColors;
    }
}
