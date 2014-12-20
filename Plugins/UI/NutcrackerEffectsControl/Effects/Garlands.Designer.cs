using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Garlands {
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
            this.tbGarlandType = new TrackBar();
            this.lblGarlandType = new Label();
            this.tbSpacing = new TrackBar();
            this.lblSpacing = new Label();
            ((ISupportInitialize)(this.tbGarlandType)).BeginInit();
            ((ISupportInitialize)(this.tbSpacing)).BeginInit();
            this.SuspendLayout();
            // 
            // tbGarlandType
            // 
            this.tbGarlandType.AutoSize = false;
            this.tbGarlandType.Location = new Point(90, 3);
            this.tbGarlandType.Maximum = 4;
            this.tbGarlandType.Minimum = 1;
            this.tbGarlandType.Name = "tbGarlandType";
            this.tbGarlandType.Size = new Size(139, 25);
            this.tbGarlandType.TabIndex = 3;
            this.tbGarlandType.TickStyle = TickStyle.None;
            this.tbGarlandType.Value = 1;
            this.tbGarlandType.ValueChanged += new EventHandler(this.Garlands_ControlChanged);
            // 
            // lblGarlandType
            // 
            this.lblGarlandType.AutoSize = true;
            this.lblGarlandType.Location = new Point(4, 9);
            this.lblGarlandType.Name = "lblGarlandType";
            this.lblGarlandType.Size = new Size(71, 13);
            this.lblGarlandType.TabIndex = 2;
            this.lblGarlandType.Text = "Garland Type";
            // 
            // tbSpacing
            // 
            this.tbSpacing.AutoSize = false;
            this.tbSpacing.Location = new Point(90, 34);
            this.tbSpacing.Maximum = 100;
            this.tbSpacing.Name = "tbSpacing";
            this.tbSpacing.Size = new Size(139, 25);
            this.tbSpacing.TabIndex = 5;
            this.tbSpacing.TickStyle = TickStyle.None;
            this.tbSpacing.ValueChanged += new EventHandler(this.Garlands_ControlChanged);
            // 
            // lblSpacing
            // 
            this.lblSpacing.AutoSize = true;
            this.lblSpacing.Location = new Point(29, 40);
            this.lblSpacing.Name = "lblSpacing";
            this.lblSpacing.Size = new Size(46, 13);
            this.lblSpacing.TabIndex = 4;
            this.lblSpacing.Text = "Spacing";
            // 
            // Garlands
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tbSpacing);
            this.Controls.Add(this.lblSpacing);
            this.Controls.Add(this.tbGarlandType);
            this.Controls.Add(this.lblGarlandType);
            this.Name = "Garlands";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbGarlandType)).EndInit();
            ((ISupportInitialize)(this.tbSpacing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbGarlandType;
        private Label lblGarlandType;
        private TrackBar tbSpacing;
        private Label lblSpacing;
    }
}
