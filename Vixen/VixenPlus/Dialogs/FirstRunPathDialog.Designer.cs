namespace VixenPlus.Dialogs
{
    partial class FirstRunPathDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstRunPathDialog));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFolder = new System.Windows.Forms.Button();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbMyDocs = new System.Windows.Forms.RadioButton();
            this.rbUseAppDir = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(82, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(490, 86);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFolder);
            this.panel1.Controls.Add(this.tbFolder);
            this.panel1.Controls.Add(this.rbCustom);
            this.panel1.Controls.Add(this.rbMyDocs);
            this.panel1.Controls.Add(this.rbUseAppDir);
            this.panel1.Location = new System.Drawing.Point(82, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 101);
            this.panel1.TabIndex = 1;
            // 
            // btnFolder
            // 
            this.btnFolder.Enabled = false;
            this.btnFolder.Location = new System.Drawing.Point(326, 70);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(35, 23);
            this.btnFolder.TabIndex = 4;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // tbFolder
            // 
            this.tbFolder.Enabled = false;
            this.tbFolder.Location = new System.Drawing.Point(4, 72);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(316, 20);
            this.tbFolder.TabIndex = 3;
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(3, 49);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(243, 17);
            this.rbCustom.TabIndex = 2;
            this.rbCustom.Text = "Let me specify where my data folder is created";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMyDocs
            // 
            this.rbMyDocs.AutoSize = true;
            this.rbMyDocs.Location = new System.Drawing.Point(3, 26);
            this.rbMyDocs.Name = "rbMyDocs";
            this.rbMyDocs.Size = new System.Drawing.Size(200, 17);
            this.rbMyDocs.TabIndex = 1;
            this.rbMyDocs.Text = "Create a data folder in MyDocuments";
            this.rbMyDocs.UseVisualStyleBackColor = true;
            this.rbMyDocs.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbUseAppDir
            // 
            this.rbUseAppDir.AutoSize = true;
            this.rbUseAppDir.Checked = true;
            this.rbUseAppDir.Location = new System.Drawing.Point(3, 3);
            this.rbUseAppDir.Name = "rbUseAppDir";
            this.rbUseAppDir.Size = new System.Drawing.Size(230, 17);
            this.rbUseAppDir.TabIndex = 0;
            this.rbUseAppDir.TabStop = true;
            this.rbUseAppDir.Text = "Create a data folder in the application folder";
            this.rbUseAppDir.UseVisualStyleBackColor = true;
            this.rbUseAppDir.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(503, 183);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(12, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(64, 64);
            this.pbIcon.TabIndex = 10;
            this.pbIcon.TabStop = false;
            // 
            // FirstRunPathDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 218);
            this.ControlBox = false;
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximumSize = new System.Drawing.Size(600, 250);
            this.MinimumSize = new System.Drawing.Size(600, 250);
            this.Name = "FirstRunPathDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to Vixen+";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbMyDocs;
        private System.Windows.Forms.RadioButton rbUseAppDir;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbIcon;
    }
}