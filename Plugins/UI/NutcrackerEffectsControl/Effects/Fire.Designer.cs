using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Fire {
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
            this.lblHeight = new Label();
            this.tbHeight = new TrackBar();
            this.chkBoxUsePalette = new CheckBox();
            this.textBox1 = new TextBox();
            ((ISupportInitialize)(this.tbHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new Point(4, 6);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new Size(38, 13);
            this.lblHeight.TabIndex = 0;
            this.lblHeight.Text = "Height";
            // 
            // tbHeight
            // 
            this.tbHeight.AutoSize = false;
            this.tbHeight.Location = new Point(46, 0);
            this.tbHeight.Maximum = 100;
            this.tbHeight.Minimum = 10;
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new Size(183, 25);
            this.tbHeight.TabIndex = 1;
            this.tbHeight.TickStyle = TickStyle.None;
            this.tbHeight.Value = 50;
            this.tbHeight.ValueChanged += new EventHandler(this.Fire_ControlChanged);
            // 
            // chkBoxUsePalette
            // 
            this.chkBoxUsePalette.AutoSize = true;
            this.chkBoxUsePalette.Location = new Point(7, 31);
            this.chkBoxUsePalette.Name = "chkBoxUsePalette";
            this.chkBoxUsePalette.Size = new Size(81, 17);
            this.chkBoxUsePalette.TabIndex = 2;
            this.chkBoxUsePalette.Text = "Use Palette";
            this.chkBoxUsePalette.UseVisualStyleBackColor = true;
            this.chkBoxUsePalette.CheckedChanged += new EventHandler(this.Fire_ControlChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new Point(4, 55);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(225, 76);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "NOTE: Nutcracker uses HSV color and may not have the intended effect when you use" +
                " the palette.\r\n\r\ne.g. Black and white will render red/yellow.";
            // 
            // Fire
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkBoxUsePalette);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.lblHeight);
            this.Name = "Fire";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblHeight;
        private TrackBar tbHeight;
        private CheckBox chkBoxUsePalette;
        private TextBox textBox1;
    }
}
