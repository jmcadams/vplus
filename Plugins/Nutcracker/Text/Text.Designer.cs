namespace Text {
    partial class Text {
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
            this.txtBoxLine1 = new System.Windows.Forms.TextBox();
            this.lblLine1 = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.txtBoxLine2 = new System.Windows.Forms.TextBox();
            this.tbThickness = new System.Windows.Forms.TrackBar();
            this.lblTop = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBoxLine1
            // 
            this.txtBoxLine1.Location = new System.Drawing.Point(46, 0);
            this.txtBoxLine1.Name = "txtBoxLine1";
            this.txtBoxLine1.Size = new System.Drawing.Size(186, 20);
            this.txtBoxLine1.TabIndex = 0;
            this.txtBoxLine1.TextChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // lblLine1
            // 
            this.lblLine1.AutoSize = true;
            this.lblLine1.Location = new System.Drawing.Point(4, 4);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(36, 13);
            this.lblLine1.TabIndex = 1;
            this.lblLine1.Text = "Line 1";
            // 
            // lblLine2
            // 
            this.lblLine2.AutoSize = true;
            this.lblLine2.Location = new System.Drawing.Point(4, 30);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(36, 13);
            this.lblLine2.TabIndex = 3;
            this.lblLine2.Text = "Line 2";
            // 
            // txtBoxLine2
            // 
            this.txtBoxLine2.Location = new System.Drawing.Point(46, 26);
            this.txtBoxLine2.Name = "txtBoxLine2";
            this.txtBoxLine2.Size = new System.Drawing.Size(186, 20);
            this.txtBoxLine2.TabIndex = 2;
            this.txtBoxLine2.TextChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // tbThickness
            // 
            this.tbThickness.AutoSize = false;
            this.tbThickness.Location = new System.Drawing.Point(46, 52);
            this.tbThickness.Maximum = 100;
            this.tbThickness.Name = "tbThickness";
            this.tbThickness.Size = new System.Drawing.Size(186, 25);
            this.tbThickness.TabIndex = 31;
            this.tbThickness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbThickness.ValueChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(14, 58);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(26, 13);
            this.lblTop.TabIndex = 30;
            this.lblTop.Text = "Top";
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(4, 75);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(75, 23);
            this.btnFont.TabIndex = 32;
            this.btnFont.Text = "Font";
            this.btnFont.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(85, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "label1";
            this.label1.TextChanged += new System.EventHandler(this.Text_ControlChanged);
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
            this.cbDirection.Location = new System.Drawing.Point(90, 101);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(139, 21);
            this.cbDirection.TabIndex = 35;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Direction";
            // 
            // Text
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.tbThickness);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.txtBoxLine2);
            this.Controls.Add(this.lblLine1);
            this.Controls.Add(this.txtBoxLine1);
            this.Name = "Text";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbThickness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxLine1;
        private System.Windows.Forms.Label lblLine1;
        private System.Windows.Forms.Label lblLine2;
        private System.Windows.Forms.TextBox txtBoxLine2;
        private System.Windows.Forms.TrackBar tbThickness;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label label2;
    }
}
