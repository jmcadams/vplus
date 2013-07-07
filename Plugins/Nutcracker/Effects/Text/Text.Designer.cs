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
            this.tbTop = new System.Windows.Forms.TrackBar();
            this.lblTop = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.lblFont = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRotation = new System.Windows.Forms.TrackBar();
            this.lblRotation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotation)).BeginInit();
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
            this.lblLine1.Location = new System.Drawing.Point(4, 3);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(36, 13);
            this.lblLine1.TabIndex = 1;
            this.lblLine1.Text = "Line 1";
            // 
            // lblLine2
            // 
            this.lblLine2.AutoSize = true;
            this.lblLine2.Location = new System.Drawing.Point(4, 22);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(36, 13);
            this.lblLine2.TabIndex = 3;
            this.lblLine2.Text = "Line 2";
            // 
            // txtBoxLine2
            // 
            this.txtBoxLine2.Location = new System.Drawing.Point(46, 19);
            this.txtBoxLine2.Name = "txtBoxLine2";
            this.txtBoxLine2.Size = new System.Drawing.Size(186, 20);
            this.txtBoxLine2.TabIndex = 2;
            this.txtBoxLine2.TextChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // tbTop
            // 
            this.tbTop.AutoSize = false;
            this.tbTop.Location = new System.Drawing.Point(46, 40);
            this.tbTop.Maximum = 100;
            this.tbTop.Name = "tbTop";
            this.tbTop.Size = new System.Drawing.Size(186, 25);
            this.tbTop.TabIndex = 31;
            this.tbTop.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbTop.Value = 50;
            this.tbTop.ValueChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(14, 46);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(26, 13);
            this.lblTop.TabIndex = 30;
            this.lblTop.Text = "Top";
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(4, 63);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(75, 23);
            this.btnFont.TabIndex = 32;
            this.btnFont.Text = "Font";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // lblFont
            // 
            this.lblFont.Location = new System.Drawing.Point(85, 68);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(144, 18);
            this.lblFont.TabIndex = 33;
            this.lblFont.Text = "Font";
            this.lblFont.TextChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down",
            "None"});
            this.cbDirection.Location = new System.Drawing.Point(90, 89);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(139, 21);
            this.cbDirection.TabIndex = 35;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Direction";
            // 
            // tbRotation
            // 
            this.tbRotation.AutoSize = false;
            this.tbRotation.Location = new System.Drawing.Point(46, 108);
            this.tbRotation.Maximum = 360;
            this.tbRotation.Name = "tbRotation";
            this.tbRotation.Size = new System.Drawing.Size(186, 23);
            this.tbRotation.TabIndex = 36;
            this.tbRotation.TickFrequency = 90;
            this.tbRotation.ValueChanged += new System.EventHandler(this.Text_ControlChanged);
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(4, 115);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(39, 13);
            this.lblRotation.TabIndex = 37;
            this.lblRotation.Text = "Rotate";
            // 
            // Text
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRotation);
            this.Controls.Add(this.tbRotation);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.tbTop);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.txtBoxLine2);
            this.Controls.Add(this.lblLine1);
            this.Controls.Add(this.txtBoxLine1);
            this.Name = "Text";
            this.Size = new System.Drawing.Size(232, 134);
            ((System.ComponentModel.ISupportInitialize)(this.tbTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRotation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxLine1;
        private System.Windows.Forms.Label lblLine1;
        private System.Windows.Forms.Label lblLine2;
        private System.Windows.Forms.TextBox txtBoxLine2;
        private System.Windows.Forms.TrackBar tbTop;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tbRotation;
        private System.Windows.Forms.Label lblRotation;
    }
}
