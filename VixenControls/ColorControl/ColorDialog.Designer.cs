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
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.colorEditor1 = new CommonControls.ColorEditor();
            this.colorWheel1 = new CommonControls.ColorWheel();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(302, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnNone
            // 
            this.btnNone.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNone.Location = new System.Drawing.Point(221, 138);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(75, 23);
            this.btnNone.TabIndex = 3;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(140, 138);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 4;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // pbColor
            // 
            this.pbColor.Location = new System.Drawing.Point(13, 139);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(121, 23);
            this.pbColor.TabIndex = 5;
            this.pbColor.TabStop = false;
            // 
            // colorEditor1
            // 
            this.colorEditor1.Location = new System.Drawing.Point(138, 12);
            this.colorEditor1.Name = "colorEditor1";
            this.colorEditor1.Size = new System.Drawing.Size(238, 120);
            this.colorEditor1.TabIndex = 1;
            this.colorEditor1.ColorChanged += new System.EventHandler(this.colorEditor1_ColorChanged);
            // 
            // colorWheel1
            // 
            this.colorWheel1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorWheel1.Location = new System.Drawing.Point(12, 12);
            this.colorWheel1.Name = "colorWheel1";
            this.colorWheel1.Size = new System.Drawing.Size(120, 120);
            this.colorWheel1.TabIndex = 0;
            this.colorWheel1.ColorChanged += new System.EventHandler(this.colorWheel1_ColorChanged);
            // 
            // ColorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 174);
            this.Controls.Add(this.pbColor);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.colorEditor1);
            this.Controls.Add(this.colorWheel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ColorDialog";
            this.Text = "ColorDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ColorWheel colorWheel1;
        private ColorEditor colorEditor1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.PictureBox pbColor;
    }
}