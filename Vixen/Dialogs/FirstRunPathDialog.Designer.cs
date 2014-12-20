using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
    partial class FirstRunPathDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.panel1 = new Panel();
            this.lblCustom = new Label();
            this.lblMyDocs = new Label();
            this.lblAppDir = new Label();
            this.tbFolder = new TextBox();
            this.btnFolder = new Button();
            this.rbCustom = new RadioButton();
            this.rbMyDocs = new RadioButton();
            this.rbUseAppDir = new RadioButton();
            this.btnOk = new Button();
            this.pbIcon = new PictureBox();
            this.btnCancel = new Button();
            this.tbPrompt = new TextBox();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)(this.pbIcon)).BeginInit();
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
            this.panel1.Location = new Point(82, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(415, 144);
            this.panel1.TabIndex = 1;
            // 
            // lblCustom
            // 
            this.lblCustom.AutoSize = true;
            this.lblCustom.Location = new Point(19, 121);
            this.lblCustom.Name = "lblCustom";
            this.lblCustom.Size = new Size(67, 13);
            this.lblCustom.TabIndex = 8;
            this.lblCustom.Text = "Custom Path";
            this.lblCustom.Visible = false;
            // 
            // lblMyDocs
            // 
            this.lblMyDocs.AutoSize = true;
            this.lblMyDocs.Location = new Point(19, 59);
            this.lblMyDocs.Name = "lblMyDocs";
            this.lblMyDocs.Size = new Size(103, 13);
            this.lblMyDocs.TabIndex = 7;
            this.lblMyDocs.Text = "My Documents Path";
            // 
            // lblAppDir
            // 
            this.lblAppDir.AutoSize = true;
            this.lblAppDir.Location = new Point(19, 23);
            this.lblAppDir.Name = "lblAppDir";
            this.lblAppDir.Size = new Size(50, 13);
            this.lblAppDir.TabIndex = 6;
            this.lblAppDir.Text = "App path";
            // 
            // tbFolder
            // 
            this.tbFolder.Enabled = false;
            this.tbFolder.Location = new Point(4, 98);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new Size(408, 20);
            this.tbFolder.TabIndex = 3;
            this.tbFolder.TextChanged += new EventHandler(this.tbFolder_TextChanged);
            // 
            // btnFolder
            // 
            this.btnFolder.Enabled = false;
            this.btnFolder.Location = new Point(252, 72);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new Size(35, 23);
            this.btnFolder.TabIndex = 4;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new EventHandler(this.btnFolder_Click);
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new Point(3, 75);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new Size(243, 17);
            this.rbCustom.TabIndex = 2;
            this.rbCustom.Text = "Let me specify where my data folder is created";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMyDocs
            // 
            this.rbMyDocs.AutoSize = true;
            this.rbMyDocs.Location = new Point(3, 39);
            this.rbMyDocs.Name = "rbMyDocs";
            this.rbMyDocs.Size = new Size(200, 17);
            this.rbMyDocs.TabIndex = 1;
            this.rbMyDocs.Text = "Create a data folder in MyDocuments";
            this.rbMyDocs.UseVisualStyleBackColor = true;
            this.rbMyDocs.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbUseAppDir
            // 
            this.rbUseAppDir.AutoSize = true;
            this.rbUseAppDir.Checked = true;
            this.rbUseAppDir.Location = new Point(3, 3);
            this.rbUseAppDir.Name = "rbUseAppDir";
            this.rbUseAppDir.Size = new Size(230, 17);
            this.rbUseAppDir.TabIndex = 0;
            this.rbUseAppDir.TabStop = true;
            this.rbUseAppDir.Text = "Create a data folder in the application folder";
            this.rbUseAppDir.UseVisualStyleBackColor = true;
            this.rbUseAppDir.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new Point(503, 197);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new Point(12, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new Size(64, 64);
            this.pbIcon.TabIndex = 10;
            this.pbIcon.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(503, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            // 
            // tbPrompt
            // 
            this.tbPrompt.BorderStyle = BorderStyle.None;
            this.tbPrompt.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.tbPrompt.Location = new Point(82, 12);
            this.tbPrompt.Multiline = true;
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.Size = new Size(496, 87);
            this.tbPrompt.TabIndex = 13;
            // 
            // FirstRunPathDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new Size(590, 261);
            this.ControlBox = false;
            this.Controls.Add(this.tbPrompt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Name = "FirstRunPathDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Welcome to Vixen+";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Button btnFolder;
        private TextBox tbFolder;
        private RadioButton rbCustom;
        private RadioButton rbMyDocs;
        private RadioButton rbUseAppDir;
        private Button btnOk;
        private PictureBox pbIcon;
        private Label lblMyDocs;
        private Label lblAppDir;
        private Label lblCustom;
        private Button btnCancel;
        private TextBox tbPrompt;
    }
}