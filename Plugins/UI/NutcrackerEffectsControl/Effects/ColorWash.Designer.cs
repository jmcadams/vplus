using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class ColorWash {
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
            this.tbCount = new TrackBar();
            this.lblCount = new Label();
            this.chkBoxHFade = new CheckBox();
            this.chkBoxVFade = new CheckBox();
            ((ISupportInitialize)(this.tbCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCount
            // 
            this.tbCount.AutoSize = false;
            this.tbCount.Location = new Point(125, 3);
            this.tbCount.Minimum = 1;
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new Size(104, 25);
            this.tbCount.TabIndex = 0;
            this.tbCount.TickStyle = TickStyle.None;
            this.tbCount.Value = 1;
            this.tbCount.ValueChanged += new EventHandler(this.ColorWash_ControlChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new Point(84, 9);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new Size(35, 13);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "Count";
            // 
            // chkBoxHFade
            // 
            this.chkBoxHFade.AutoSize = true;
            this.chkBoxHFade.CheckAlign = ContentAlignment.MiddleRight;
            this.chkBoxHFade.Location = new Point(3, 37);
            this.chkBoxHFade.Name = "chkBoxHFade";
            this.chkBoxHFade.Size = new Size(100, 17);
            this.chkBoxHFade.TabIndex = 2;
            this.chkBoxHFade.Text = "Horizontal Fade";
            this.chkBoxHFade.UseVisualStyleBackColor = true;
            this.chkBoxHFade.CheckedChanged += new EventHandler(this.ColorWash_ControlChanged);
            // 
            // chkBoxVFade
            // 
            this.chkBoxVFade.AutoSize = true;
            this.chkBoxVFade.CheckAlign = ContentAlignment.MiddleRight;
            this.chkBoxVFade.Location = new Point(125, 37);
            this.chkBoxVFade.Name = "chkBoxVFade";
            this.chkBoxVFade.Size = new Size(88, 17);
            this.chkBoxVFade.TabIndex = 3;
            this.chkBoxVFade.Text = "Vertical Fade";
            this.chkBoxVFade.UseVisualStyleBackColor = true;
            this.chkBoxVFade.CheckedChanged += new EventHandler(this.ColorWash_ControlChanged);
            // 
            // ColorWash
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxVFade);
            this.Controls.Add(this.chkBoxHFade);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.tbCount);
            this.Name = "ColorWash";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbCount;
        private Label lblCount;
        private CheckBox chkBoxHFade;
        private CheckBox chkBoxVFade;
    }
}
