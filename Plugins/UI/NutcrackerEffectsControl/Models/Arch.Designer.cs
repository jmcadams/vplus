using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Models {
    partial class Arch {
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
            this.nudNodeCount = new NumericUpDown();
            this.lblNodeCount = new Label();
            this.nudArchCount = new NumericUpDown();
            this.lblArchCount = new Label();
            this.pbPreview = new PictureBox();
            ((ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((ISupportInitialize)(this.nudArchCount)).BeginInit();
            ((ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNodeCount
            // 
            this.nudNodeCount.Location = new Point(276, 59);
            this.nudNodeCount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudNodeCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNodeCount.Name = "nudNodeCount";
            this.nudNodeCount.Size = new Size(71, 20);
            this.nudNodeCount.TabIndex = 17;
            this.nudNodeCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudNodeCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblNodeCount
            // 
            this.lblNodeCount.AutoSize = true;
            this.lblNodeCount.Location = new Point(214, 43);
            this.lblNodeCount.Name = "lblNodeCount";
            this.lblNodeCount.Size = new Size(133, 13);
            this.lblNodeCount.TabIndex = 21;
            this.lblNodeCount.Text = "Number of Nodes per Arch";
            // 
            // nudArchCount
            // 
            this.nudArchCount.Location = new Point(276, 20);
            this.nudArchCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudArchCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchCount.Name = "nudArchCount";
            this.nudArchCount.Size = new Size(71, 20);
            this.nudArchCount.TabIndex = 16;
            this.nudArchCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblArchCount
            // 
            this.lblArchCount.AutoSize = true;
            this.lblArchCount.Location = new Point(255, 4);
            this.lblArchCount.Name = "lblArchCount";
            this.lblArchCount.Size = new Size(92, 13);
            this.lblArchCount.TabIndex = 20;
            this.lblArchCount.Text = "Number of Arches";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = Color.Black;
            this.pbPreview.Location = new Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new Size(172, 244);
            this.pbPreview.TabIndex = 23;
            this.pbPreview.TabStop = false;
            this.pbPreview.Paint += new PaintEventHandler(this.pbPreview_Paint);
            // 
            // Arch
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.nudArchCount);
            this.Controls.Add(this.lblArchCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Arch";
            this.Size = new Size(350, 250);
            ((ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((ISupportInitialize)(this.nudArchCount)).EndInit();
            ((ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown nudNodeCount;
        private Label lblNodeCount;
        private NumericUpDown nudArchCount;
        private Label lblArchCount;
        private PictureBox pbPreview;
    }
}
