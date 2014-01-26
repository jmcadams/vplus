namespace Dialogs
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCustom = new System.Windows.Forms.Label();
            this.lblMyDocs = new System.Windows.Forms.Label();
            this.lblAppDir = new System.Windows.Forms.Label();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.btnFolder = new System.Windows.Forms.Button();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbMyDocs = new System.Windows.Forms.RadioButton();
            this.rbUseAppDir = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbPrompt = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCustom);
            this.panel1.Controls.Add(this.lblMyDocs);
            this.panel1.Controls.Add(this.lblAppDir);
            this.panel1.Controls.Add(this.tbFolder);
            this.panel1.Controls.Add(this.btnFolder);
            this.panel1.Controls.Add(this.rbCustom);
            this.panel1.Controls.Add(this.rbMyDocs);
            this.panel1.Controls.Add(this.rbUseAppDir);
            this.panel1.Location = new System.Drawing.Point(82, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 144);
            this.panel1.TabIndex = 1;
            // 
            // lblCustom
            // 
            this.lblCustom.AutoSize = true;
            this.lblCustom.Location = new System.Drawing.Point(19, 121);
            this.lblCustom.Name = "lblCustom";
            this.lblCustom.Size = new System.Drawing.Size(67, 13);
            this.lblCustom.TabIndex = 8;
            this.lblCustom.Text = "Custom Path";
            this.lblCustom.Visible = false;
            // 
            // lblMyDocs
            // 
            this.lblMyDocs.AutoSize = true;
            this.lblMyDocs.Location = new System.Drawing.Point(19, 59);
            this.lblMyDocs.Name = "lblMyDocs";
            this.lblMyDocs.Size = new System.Drawing.Size(103, 13);
            this.lblMyDocs.TabIndex = 7;
            this.lblMyDocs.Text = "My Documents Path";
            // 
            // lblAppDir
            // 
            this.lblAppDir.AutoSize = true;
            this.lblAppDir.Location = new System.Drawing.Point(19, 23);
            this.lblAppDir.Name = "lblAppDir";
            this.lblAppDir.Size = new System.Drawing.Size(50, 13);
            this.lblAppDir.TabIndex = 6;
            this.lblAppDir.Text = "App path";
            // 
            // tbFolder
            // 
            this.tbFolder.Enabled = false;
            this.tbFolder.Location = new System.Drawing.Point(4, 98);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(408, 20);
            this.tbFolder.TabIndex = 3;
            this.tbFolder.TextChanged += new System.EventHandler(this.tbFolder_TextChanged);
            // 
            // btnFolder
            // 
            this.btnFolder.Enabled = false;
            this.btnFolder.Location = new System.Drawing.Point(252, 72);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(35, 23);
            this.btnFolder.TabIndex = 4;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(3, 75);
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
            this.rbMyDocs.Location = new System.Drawing.Point(3, 39);
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
            this.btnOk.Location = new System.Drawing.Point(503, 197);
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
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(503, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            // 
            // tbPrompt
            // 
            this.tbPrompt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPrompt.Location = new System.Drawing.Point(82, 12);
            this.tbPrompt.Multiline = true;
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.Size = new System.Drawing.Size(496, 87);
            this.tbPrompt.TabIndex = 13;
            // 
            // FirstRunPathDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(590, 261);
            this.ControlBox = false;
            this.Controls.Add(this.tbPrompt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = global::VixenPlus.Properties.Resources.VixenPlus;
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

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbMyDocs;
        private System.Windows.Forms.RadioButton rbUseAppDir;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lblMyDocs;
        private System.Windows.Forms.Label lblAppDir;
        private System.Windows.Forms.Label lblCustom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbPrompt;
    }
}