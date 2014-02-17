namespace CommonControls {
    partial class ColorDialog {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.pbOriginalColor = new System.Windows.Forms.PictureBox();
            this.lblOriginalColor = new System.Windows.Forms.Label();
            this.pbNewColor = new System.Windows.Forms.PictureBox();
            this.lblNewColor = new System.Windows.Forms.Label();
            this.colorEditor1 = new CommonControls.ColorEditor();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewColor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(301, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnNone
            // 
            this.btnNone.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNone.Location = new System.Drawing.Point(220, 133);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(75, 23);
            this.btnNone.TabIndex = 3;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(139, 133);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 4;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // pbOriginalColor
            // 
            this.pbOriginalColor.Location = new System.Drawing.Point(12, 25);
            this.pbOriginalColor.Name = "pbOriginalColor";
            this.pbOriginalColor.Size = new System.Drawing.Size(121, 52);
            this.pbOriginalColor.TabIndex = 5;
            this.pbOriginalColor.TabStop = false;
            // 
            // lblOriginalColor
            // 
            this.lblOriginalColor.AutoSize = true;
            this.lblOriginalColor.Location = new System.Drawing.Point(12, 9);
            this.lblOriginalColor.Name = "lblOriginalColor";
            this.lblOriginalColor.Size = new System.Drawing.Size(72, 13);
            this.lblOriginalColor.TabIndex = 6;
            this.lblOriginalColor.Text = "Original Color:";
            // 
            // pbNewColor
            // 
            this.pbNewColor.Location = new System.Drawing.Point(11, 104);
            this.pbNewColor.Name = "pbNewColor";
            this.pbNewColor.Size = new System.Drawing.Size(121, 52);
            this.pbNewColor.TabIndex = 7;
            this.pbNewColor.TabStop = false;
            // 
            // lblNewColor
            // 
            this.lblNewColor.AutoSize = true;
            this.lblNewColor.Location = new System.Drawing.Point(12, 88);
            this.lblNewColor.Name = "lblNewColor";
            this.lblNewColor.Size = new System.Drawing.Size(59, 13);
            this.lblNewColor.TabIndex = 8;
            this.lblNewColor.Text = "New Color:";
            // 
            // colorEditor1
            // 
            this.colorEditor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorEditor1.Location = new System.Drawing.Point(138, 12);
            this.colorEditor1.Name = "colorEditor1";
            this.colorEditor1.Size = new System.Drawing.Size(239, 89);
            this.colorEditor1.TabIndex = 1;
            this.colorEditor1.ColorChanged += new System.EventHandler(this.colorEditor1_ColorChanged);
            // 
            // ColorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 169);
            this.Controls.Add(this.lblNewColor);
            this.Controls.Add(this.pbNewColor);
            this.Controls.Add(this.lblOriginalColor);
            this.Controls.Add(this.pbOriginalColor);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.colorEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ColorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ColorDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorEditor colorEditor1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.PictureBox pbOriginalColor;
        private System.Windows.Forms.Label lblOriginalColor;
        private System.Windows.Forms.PictureBox pbNewColor;
        private System.Windows.Forms.Label lblNewColor;
    }
}