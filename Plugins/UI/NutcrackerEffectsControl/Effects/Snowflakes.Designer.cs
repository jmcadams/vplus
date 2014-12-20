using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Snowflakes {
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
            this.tbType = new TrackBar();
            this.lblType = new Label();
            this.tbMaxFlakes = new TrackBar();
            this.lblMaxFlakes = new Label();
            ((ISupportInitialize)(this.tbType)).BeginInit();
            ((ISupportInitialize)(this.tbMaxFlakes)).BeginInit();
            this.SuspendLayout();
            // 
            // tbType
            // 
            this.tbType.AutoSize = false;
            this.tbType.Location = new Point(90, 34);
            this.tbType.Maximum = 4;
            this.tbType.Name = "tbType";
            this.tbType.Size = new Size(139, 25);
            this.tbType.TabIndex = 13;
            this.tbType.TickStyle = TickStyle.None;
            this.tbType.ValueChanged += new EventHandler(this.Snowflakes_ControlChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new Point(44, 40);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(31, 13);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "Type";
            // 
            // tbMaxFlakes
            // 
            this.tbMaxFlakes.AutoSize = false;
            this.tbMaxFlakes.Location = new Point(90, 3);
            this.tbMaxFlakes.Maximum = 20;
            this.tbMaxFlakes.Minimum = 1;
            this.tbMaxFlakes.Name = "tbMaxFlakes";
            this.tbMaxFlakes.Size = new Size(139, 25);
            this.tbMaxFlakes.TabIndex = 11;
            this.tbMaxFlakes.TickStyle = TickStyle.None;
            this.tbMaxFlakes.Value = 5;
            this.tbMaxFlakes.ValueChanged += new EventHandler(this.Snowflakes_ControlChanged);
            // 
            // lblMaxFlakes
            // 
            this.lblMaxFlakes.AutoSize = true;
            this.lblMaxFlakes.Location = new Point(14, 9);
            this.lblMaxFlakes.Name = "lblMaxFlakes";
            this.lblMaxFlakes.Size = new Size(61, 13);
            this.lblMaxFlakes.TabIndex = 10;
            this.lblMaxFlakes.Text = "Max Flakes";
            // 
            // Snowflakes
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbMaxFlakes);
            this.Controls.Add(this.lblMaxFlakes);
            this.Name = "Snowflakes";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbType)).EndInit();
            ((ISupportInitialize)(this.tbMaxFlakes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbType;
        private Label lblType;
        private TrackBar tbMaxFlakes;
        private Label lblMaxFlakes;
    }
}
