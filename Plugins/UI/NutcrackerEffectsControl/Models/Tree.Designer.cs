using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Models {
    partial class Tree {
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
            this.nudStrandCount = new NumericUpDown();
            this.lblStandCount = new Label();
            this.nudNodeCount = new NumericUpDown();
            this.lblNodeCount = new Label();
            this.nudStringCount = new NumericUpDown();
            this.lblStringCount = new Label();
            this.pbPreview = new PictureBox();
            this.gbDegrees = new GroupBox();
            this.rb90 = new RadioButton();
            this.rb180 = new RadioButton();
            this.rb270 = new RadioButton();
            this.rb360 = new RadioButton();
            ((ISupportInitialize)(this.nudStrandCount)).BeginInit();
            ((ISupportInitialize)(this.nudNodeCount)).BeginInit();
            ((ISupportInitialize)(this.nudStringCount)).BeginInit();
            ((ISupportInitialize)(this.pbPreview)).BeginInit();
            this.gbDegrees.SuspendLayout();
            this.SuspendLayout();
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
            this.nudStrandCount.TabIndex = 2;
            this.nudStrandCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStrandCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblStandCount
            // 
            this.lblStandCount.AutoSize = true;
            this.lblStandCount.Location = new Point(204, 82);
            this.lblStandCount.Name = "lblStandCount";
            this.lblStandCount.Size = new Size(143, 13);
            this.lblStandCount.TabIndex = 6;
            this.lblStandCount.Text = "Number of Strands per String";
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
            this.nudNodeCount.TabIndex = 1;
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
            this.lblNodeCount.TabIndex = 5;
            this.lblNodeCount.Text = "Number of Nodes per String";
            // 
            // nudStringCount
            // 
            this.nudStringCount.Location = new Point(276, 20);
            this.nudStringCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudStringCount.Name = "nudStringCount";
            this.nudStringCount.Size = new Size(71, 20);
            this.nudStringCount.TabIndex = 0;
            this.nudStringCount.Value = new decimal(new int[] {
            16,
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
            this.lblStringCount.TabIndex = 4;
            this.lblStringCount.Text = "Actual Number of Strings";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = Color.Black;
            this.pbPreview.Location = new Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new Size(172, 244);
            this.pbPreview.TabIndex = 7;
            this.pbPreview.TabStop = false;
            this.pbPreview.Paint += new PaintEventHandler(this.pbPreview_Paint);
            // 
            // gbDegrees
            // 
            this.gbDegrees.Controls.Add(this.rb90);
            this.gbDegrees.Controls.Add(this.rb180);
            this.gbDegrees.Controls.Add(this.rb270);
            this.gbDegrees.Controls.Add(this.rb360);
            this.gbDegrees.Location = new Point(240, 125);
            this.gbDegrees.Name = "gbDegrees";
            this.gbDegrees.Size = new Size(107, 67);
            this.gbDegrees.TabIndex = 3;
            this.gbDegrees.TabStop = false;
            this.gbDegrees.Text = "Visible Degrees";
            // 
            // rb90
            // 
            this.rb90.AutoSize = true;
            this.rb90.Location = new Point(56, 44);
            this.rb90.Name = "rb90";
            this.rb90.Size = new Size(37, 17);
            this.rb90.TabIndex = 3;
            this.rb90.Text = "90";
            this.rb90.UseVisualStyleBackColor = true;
            this.rb90.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // rb180
            // 
            this.rb180.AutoSize = true;
            this.rb180.Checked = true;
            this.rb180.Location = new Point(7, 43);
            this.rb180.Name = "rb180";
            this.rb180.Size = new Size(43, 17);
            this.rb180.TabIndex = 2;
            this.rb180.TabStop = true;
            this.rb180.Text = "180";
            this.rb180.UseVisualStyleBackColor = true;
            this.rb180.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // rb270
            // 
            this.rb270.AutoSize = true;
            this.rb270.Location = new Point(56, 20);
            this.rb270.Name = "rb270";
            this.rb270.Size = new Size(43, 17);
            this.rb270.TabIndex = 1;
            this.rb270.Text = "270";
            this.rb270.UseVisualStyleBackColor = true;
            this.rb270.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // rb360
            // 
            this.rb360.AutoSize = true;
            this.rb360.Location = new Point(7, 20);
            this.rb360.Name = "rb360";
            this.rb360.Size = new Size(43, 17);
            this.rb360.TabIndex = 0;
            this.rb360.Text = "360";
            this.rb360.UseVisualStyleBackColor = true;
            this.rb360.CheckedChanged += new EventHandler(this.control_ValueChanged);
            // 
            // Tree
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDegrees);
            this.Controls.Add(this.nudStrandCount);
            this.Controls.Add(this.lblStandCount);
            this.Controls.Add(this.nudNodeCount);
            this.Controls.Add(this.lblNodeCount);
            this.Controls.Add(this.nudStringCount);
            this.Controls.Add(this.lblStringCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "Tree";
            this.Size = new Size(350, 250);
            ((ISupportInitialize)(this.nudStrandCount)).EndInit();
            ((ISupportInitialize)(this.nudNodeCount)).EndInit();
            ((ISupportInitialize)(this.nudStringCount)).EndInit();
            ((ISupportInitialize)(this.pbPreview)).EndInit();
            this.gbDegrees.ResumeLayout(false);
            this.gbDegrees.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown nudStrandCount;
        private Label lblStandCount;
        private NumericUpDown nudNodeCount;
        private Label lblNodeCount;
        private NumericUpDown nudStringCount;
        private Label lblStringCount;
        private PictureBox pbPreview;
        private GroupBox gbDegrees;
        private RadioButton rb90;
        private RadioButton rb180;
        private RadioButton rb270;
        private RadioButton rb360;
    }
}
