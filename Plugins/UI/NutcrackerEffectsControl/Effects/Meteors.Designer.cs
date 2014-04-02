namespace NutcrackerEffectsControl.Effects {
    partial class Meteors {
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
            this.tbTrailLength = new System.Windows.Forms.TrackBar();
            this.lblTrailLength = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TrackBar();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.chkBoxUp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbTrailLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTrailLength
            // 
            this.tbTrailLength.AutoSize = false;
            this.tbTrailLength.Location = new System.Drawing.Point(90, 70);
            this.tbTrailLength.Maximum = 100;
            this.tbTrailLength.Minimum = 1;
            this.tbTrailLength.Name = "tbTrailLength";
            this.tbTrailLength.Size = new System.Drawing.Size(139, 25);
            this.tbTrailLength.TabIndex = 2;
            this.tbTrailLength.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbTrailLength.Value = 25;
            this.tbTrailLength.ValueChanged += new System.EventHandler(this.Meteors_ControlChanged);
            // 
            // lblTrailLength
            // 
            this.lblTrailLength.AutoSize = true;
            this.lblTrailLength.Location = new System.Drawing.Point(12, 76);
            this.lblTrailLength.Name = "lblTrailLength";
            this.lblTrailLength.Size = new System.Drawing.Size(63, 13);
            this.lblTrailLength.TabIndex = 6;
            this.lblTrailLength.Text = "Trail Length";
            // 
            // tbCount
            // 
            this.tbCount.AutoSize = false;
            this.tbCount.Location = new System.Drawing.Point(90, 39);
            this.tbCount.Maximum = 100;
            this.tbCount.Minimum = 1;
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(139, 25);
            this.tbCount.TabIndex = 1;
            this.tbCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbCount.Value = 10;
            this.tbCount.ValueChanged += new System.EventHandler(this.Meteors_ControlChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(40, 45);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "Count";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(44, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Rainbow",
            "Range",
            "Palette"});
            this.cbType.Location = new System.Drawing.Point(90, 12);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(139, 21);
            this.cbType.TabIndex = 0;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.Meteors_ControlChanged);
            // 
            // chkBoxUp
            // 
            this.chkBoxUp.AutoSize = true;
            this.chkBoxUp.Location = new System.Drawing.Point(90, 102);
            this.chkBoxUp.Name = "chkBoxUp";
            this.chkBoxUp.Size = new System.Drawing.Size(59, 17);
            this.chkBoxUp.TabIndex = 3;
            this.chkBoxUp.Text = "Fall Up";
            this.chkBoxUp.UseVisualStyleBackColor = true;
            this.chkBoxUp.CheckedChanged += new System.EventHandler(this.Meteors_ControlChanged);
            // 
            // Meteors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxUp);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbTrailLength);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.lblCount);
            this.Name = "Meteors";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbTrailLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbTrailLength;
        private System.Windows.Forms.Label lblTrailLength;
        private System.Windows.Forms.TrackBar tbCount;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.CheckBox chkBoxUp;
    }
}
