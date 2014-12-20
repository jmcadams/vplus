using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Picture {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tbGifSpeed = new TrackBar();
            this.lblGifSpeed = new Label();
            this.btnFile = new Button();
            this.txtBoxFile = new TextBox();
            this.label1 = new Label();
            this.cbDirection = new ComboBox();
            ((ISupportInitialize)(this.tbGifSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // tbGifSpeed
            // 
            this.tbGifSpeed.AutoSize = false;
            this.tbGifSpeed.Location = new Point(93, 86);
            this.tbGifSpeed.Maximum = 20;
            this.tbGifSpeed.Minimum = 1;
            this.tbGifSpeed.Name = "tbGifSpeed";
            this.tbGifSpeed.Size = new Size(139, 25);
            this.tbGifSpeed.TabIndex = 11;
            this.tbGifSpeed.TickStyle = TickStyle.None;
            this.tbGifSpeed.Value = 20;
            this.tbGifSpeed.ValueChanged += new EventHandler(this.Pictures_ControlChanged);
            // 
            // lblGifSpeed
            // 
            this.lblGifSpeed.AutoSize = true;
            this.lblGifSpeed.Location = new Point(15, 92);
            this.lblGifSpeed.Name = "lblGifSpeed";
            this.lblGifSpeed.Size = new Size(56, 13);
            this.lblGifSpeed.TabIndex = 10;
            this.lblGifSpeed.Text = "GIF speed";
            // 
            // btnFile
            // 
            this.btnFile.Location = new Point(3, 3);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new Size(75, 23);
            this.btnFile.TabIndex = 12;
            this.btnFile.Text = "Choose File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new EventHandler(this.btnFile_Click);
            // 
            // txtBoxFile
            // 
            this.txtBoxFile.Enabled = false;
            this.txtBoxFile.Location = new Point(0, 33);
            this.txtBoxFile.Name = "txtBoxFile";
            this.txtBoxFile.Size = new Size(232, 20);
            this.txtBoxFile.TabIndex = 13;
            this.txtBoxFile.TextChanged += new EventHandler(this.Pictures_ControlChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(22, 62);
            this.label1.Name = "label1";
            this.label1.Size = new Size(49, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Direction";
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down",
            "None"});
            this.cbDirection.Location = new Point(93, 59);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new Size(139, 21);
            this.cbDirection.TabIndex = 15;
            this.cbDirection.SelectedIndexChanged += new EventHandler(this.Pictures_ControlChanged);
            // 
            // Picture
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxFile);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.tbGifSpeed);
            this.Controls.Add(this.lblGifSpeed);
            this.Name = "Picture";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbGifSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbGifSpeed;
        private Label lblGifSpeed;
        private Button btnFile;
        private TextBox txtBoxFile;
        private Label label1;
        private ComboBox cbDirection;

    }
}
