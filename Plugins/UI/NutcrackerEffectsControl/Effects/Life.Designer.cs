using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Life {
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
            this.tbCellsToStart = new TrackBar();
            this.lblCellsToStart = new Label();
            ((ISupportInitialize)(this.tbType)).BeginInit();
            ((ISupportInitialize)(this.tbCellsToStart)).BeginInit();
            this.SuspendLayout();
            // 
            // tbType
            // 
            this.tbType.AutoSize = false;
            this.tbType.Location = new Point(90, 34);
            this.tbType.Maximum = 4;
            this.tbType.Name = "tbType";
            this.tbType.Size = new Size(139, 25);
            this.tbType.TabIndex = 9;
            this.tbType.TickStyle = TickStyle.None;
            this.tbType.ValueChanged += new EventHandler(this.Life_ControlChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new Point(39, 40);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(31, 13);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type";
            // 
            // tbCellsToStart
            // 
            this.tbCellsToStart.AutoSize = false;
            this.tbCellsToStart.Location = new Point(90, 3);
            this.tbCellsToStart.Maximum = 100;
            this.tbCellsToStart.Minimum = 1;
            this.tbCellsToStart.Name = "tbCellsToStart";
            this.tbCellsToStart.Size = new Size(139, 25);
            this.tbCellsToStart.TabIndex = 7;
            this.tbCellsToStart.TickStyle = TickStyle.None;
            this.tbCellsToStart.Value = 50;
            this.tbCellsToStart.ValueChanged += new EventHandler(this.Life_ControlChanged);
            // 
            // lblCellsToStart
            // 
            this.lblCellsToStart.AutoSize = true;
            this.lblCellsToStart.Location = new Point(4, 9);
            this.lblCellsToStart.Name = "lblCellsToStart";
            this.lblCellsToStart.Size = new Size(66, 13);
            this.lblCellsToStart.TabIndex = 6;
            this.lblCellsToStart.Text = "Cells to Start";
            // 
            // Life
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbCellsToStart);
            this.Controls.Add(this.lblCellsToStart);
            this.Name = "Life";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbType)).EndInit();
            ((ISupportInitialize)(this.tbCellsToStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbType;
        private Label lblType;
        private TrackBar tbCellsToStart;
        private Label lblCellsToStart;
    }
}
