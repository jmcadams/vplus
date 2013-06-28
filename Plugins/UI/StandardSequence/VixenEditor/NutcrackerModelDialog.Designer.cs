namespace VixenEditor.VixenEditor {
    partial class NutcrackerModelDialog {
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
            this.cbColorLayout = new System.Windows.Forms.ComboBox();
            this.lblColorLayout = new System.Windows.Forms.Label();
            this.chkBoxUseGroup = new System.Windows.Forms.CheckBox();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbColorLayout
            // 
            this.cbColorLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorLayout.FormattingEnabled = true;
            this.cbColorLayout.Items.AddRange(new object[] {
            "RGB",
            "RBG",
            "GRB",
            "GBR",
            "BRG",
            "BGR",
            "Static - Red Values",
            "Static - Green Values",
            "Static - Blue Values"});
            this.cbColorLayout.Location = new System.Drawing.Point(192, 346);
            this.cbColorLayout.Name = "cbColorLayout";
            this.cbColorLayout.Size = new System.Drawing.Size(171, 21);
            this.cbColorLayout.TabIndex = 23;
            // 
            // lblColorLayout
            // 
            this.lblColorLayout.AutoSize = true;
            this.lblColorLayout.Location = new System.Drawing.Point(84, 349);
            this.lblColorLayout.Name = "lblColorLayout";
            this.lblColorLayout.Size = new System.Drawing.Size(102, 13);
            this.lblColorLayout.TabIndex = 22;
            this.lblColorLayout.Text = "Channel Color Order";
            // 
            // chkBoxUseGroup
            // 
            this.chkBoxUseGroup.AutoSize = true;
            this.chkBoxUseGroup.Checked = true;
            this.chkBoxUseGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxUseGroup.Location = new System.Drawing.Point(109, 321);
            this.chkBoxUseGroup.Name = "chkBoxUseGroup";
            this.chkBoxUseGroup.Size = new System.Drawing.Size(77, 17);
            this.chkBoxUseGroup.TabIndex = 25;
            this.chkBoxUseGroup.Text = "Use Group";
            this.chkBoxUseGroup.UseVisualStyleBackColor = true;
            // 
            // cbGroups
            // 
            this.cbGroups.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new System.Drawing.Point(192, 319);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(171, 21);
            this.cbGroups.TabIndex = 24;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 419);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(207, 419);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(255, 396);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Part of my display";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(249, 373);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(54, 17);
            this.radioButton1.TabIndex = 29;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "L to R";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(309, 373);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(54, 17);
            this.radioButton2.TabIndex = 30;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "R to L";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(13, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 274);
            this.panel1.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 322);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Start Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 375);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Start Channel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(186, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Model Name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(263, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 35;
            // 
            // NutcrackerModelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 454);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkBoxUseGroup);
            this.Controls.Add(this.cbGroups);
            this.Controls.Add(this.cbColorLayout);
            this.Controls.Add(this.lblColorLayout);
            this.Name = "NutcrackerModelDialog";
            this.Text = "NutcrackerModelDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbColorLayout;
        private System.Windows.Forms.Label lblColorLayout;
        private System.Windows.Forms.CheckBox chkBoxUseGroup;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
    }
}