using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Models {
    partial class Matrix {
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
            this.gbOrientation = new GroupBox();
            this.rbVertical = new RadioButton();
            this.rbHorizontal = new RadioButton();
            this.nudStrandCount = new NumericUpDown();
            this.nudNodeCount = new NumericUpDown();
            this.lblNodeCount = new Label();
            this.lblStandCount = new Label();
            this.nudStringCount = new NumericUpDown();
            this.lblStringCount = new Label();
            this.pbPreview = new PictureBox();
            this.gbOrientation.SuspendLayout();
            ((ISupportInitialize)(this.nudStrandCount)).BeginInit();
            ((ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((ISupportInitialize)(this.nudStringCount)).BeginInit();
            ((ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOrientation
            // 
            this.gbOrientation.Controls.Add(this.rbVertical);
            this.gbOrientation.Controls.Add(this.rbHorizontal);
            this.gbOrientation.Location = new Point(240, 125);
            this.gbOrientation.Name = "gbOrientation";
            this.gbOrientation.Size = new Size(107, 67);
            this.gbOrientation.TabIndex = 19;
            this.gbOrientation.TabStop = false;
            this.gbOrientation.Text = "Orientation";
            // 
            // rbVertical
            // 
            this.rbVertical.AutoSize = true;
            this.rbVertical.Checked = true;
            this.rbVertical.Location = new Point(7, 43);
            this.rbVertical.Name = "rbVertical";
            this.rbVertical.Size = new Size(60, 17);
            this.rbVertical.TabIndex = 1;
            this.rbVertical.TabStop = true;
            this.rbVertical.Text = "Vertical";
            this.rbVertical.UseVisualStyleBackColor = true;
            this.rbVertical.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // rbHorizontal
            // 
            this.rbHorizontal.AutoSize = true;
            this.rbHorizontal.Location = new Point(7, 20);
            this.rbHorizontal.Name = "rbHorizontal";
            this.rbHorizontal.Size = new Size(72, 17);
            this.rbHorizontal.TabIndex = 0;
            this.rbHorizontal.Text = "Horizontal";
            this.rbHorizontal.UseVisualStyleBackColor = true;
            this.rbHorizontal.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // nudStrandCount
            // 
            this.nudStrandCount.Location = new Point(276, 99);
            this.nudStrandCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudStrandCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStrandCount.Name = "nudStrandCount";
            this.nudStrandCount.Size = new Size(71, 20);
            this.nudStrandCount.TabIndex = 18;
            this.nudStrandCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStrandCount.ValueChanged += new EventHandler(this.control_ValueChanged);
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
            this.lblNodeCount.Location = new Point(209, 43);
            this.lblNodeCount.Name = "lblNodeCount";
            this.lblNodeCount.Size = new Size(138, 13);
            this.lblNodeCount.TabIndex = 21;
            this.lblNodeCount.Text = "Number of Nodes per String";
            // 
            // lblStandCount
            // 
            this.lblStandCount.AutoSize = true;
            this.lblStandCount.Location = new Point(204, 82);
            this.lblStandCount.Name = "lblStandCount";
            this.lblStandCount.Size = new Size(143, 13);
            this.lblStandCount.TabIndex = 22;
            this.lblStandCount.Text = "Number of Strands per String";
            // 
            // nudStringCount
            // 
            this.nudStringCount.Location = new Point(276, 20);
            this.nudStringCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStringCount.Name = "nudStringCount";
            this.nudStringCount.Size = new Size(71, 20);
            this.nudStringCount.TabIndex = 16;
            this.nudStringCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStringCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblStringCount
            // 
            this.lblStringCount.AutoSize = true;
            this.lblStringCount.Location = new Point(223, 4);
            this.lblStringCount.Name = "lblStringCount";
            this.lblStringCount.Size = new Size(124, 13);
            this.lblStringCount.TabIndex = 20;
            this.lblStringCount.Text = "Actual Number of Strings";
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
            // Matrix
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOrientation);
            this.Controls.Add(this.nudStrandCount);
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.lblStandCount);
            this.Controls.Add(this.nudStringCount);
            this.Controls.Add(this.lblStringCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Matrix";
            this.Size = new Size(350, 250);
            this.gbOrientation.ResumeLayout(false);
            this.gbOrientation.PerformLayout();
            ((ISupportInitialize)(this.nudStrandCount)).EndInit();
            ((ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((ISupportInitialize)(this.nudStringCount)).EndInit();
            ((ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox gbOrientation;
        private RadioButton rbVertical;
        private RadioButton rbHorizontal;
        private NumericUpDown nudStrandCount;
        private NumericUpDown nudNodeCount;
        private Label lblNodeCount;
        private Label lblStandCount;
        private NumericUpDown nudStringCount;
        private Label lblStringCount;
        private PictureBox pbPreview;
    }
}
