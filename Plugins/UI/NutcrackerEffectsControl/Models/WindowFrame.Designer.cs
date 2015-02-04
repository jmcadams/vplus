using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Nutcracker.Properties;

namespace Nutcracker.Models {
    partial class WindowFrame {
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
            this.pbLink = new PictureBox();
            this.nudBottomCount = new NumericUpDown();
            this.nudSideCount = new NumericUpDown();
            this.lblSideCount = new Label();
            this.lblBottomCount = new Label();
            this.nudTopCount = new NumericUpDown();
            this.lblTopCount = new Label();
            this.pbPreview = new PictureBox();
            this.pbLinkBottom = new PictureBox();
            this.pbLinkTop = new PictureBox();
            ((ISupportInitialize)(this.pbLink)).BeginInit();
            ((ISupportInitialize)(this.nudBottomCount)).BeginInit();
            ((ISupportInitialize)(this.nudSideCount)).BeginInit();
            ((ISupportInitialize)(this.nudTopCount)).BeginInit();
            ((ISupportInitialize)(this.pbPreview)).BeginInit();
            ((ISupportInitialize)(this.pbLinkBottom)).BeginInit();
            ((ISupportInitialize)(this.pbLinkTop)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLink
            // 
            this.pbLink.BackColor = Color.Transparent;
            this.pbLink.BackgroundImage = Resources.Link;
            this.pbLink.InitialImage = null;
            this.pbLink.Location = new Point(330, 35);
            this.pbLink.Name = "pbLink";
            this.pbLink.Size = new Size(13, 28);
            this.pbLink.TabIndex = 16;
            this.pbLink.TabStop = false;
            this.pbLink.Click += new EventHandler(this.pbLink_Click);
            // 
            // nudBottomCount
            // 
            this.nudBottomCount.Location = new Point(253, 59);
            this.nudBottomCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBottomCount.Name = "nudBottomCount";
            this.nudBottomCount.Size = new Size(71, 20);
            this.nudBottomCount.TabIndex = 10;
            this.nudBottomCount.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudBottomCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // nudSideCount
            // 
            this.nudSideCount.Location = new Point(253, 98);
            this.nudSideCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSideCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSideCount.Name = "nudSideCount";
            this.nudSideCount.Size = new Size(71, 20);
            this.nudSideCount.TabIndex = 9;
            this.nudSideCount.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudSideCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblSideCount
            // 
            this.lblSideCount.AutoSize = true;
            this.lblSideCount.Location = new Point(190, 82);
            this.lblSideCount.Name = "lblSideCount";
            this.lblSideCount.Size = new Size(134, 13);
            this.lblSideCount.TabIndex = 13;
            this.lblSideCount.Text = "Number of Nodes on Sides";
            // 
            // lblBottomCount
            // 
            this.lblBottomCount.AutoSize = true;
            this.lblBottomCount.Location = new Point(183, 42);
            this.lblBottomCount.Name = "lblBottomCount";
            this.lblBottomCount.Size = new Size(141, 13);
            this.lblBottomCount.TabIndex = 14;
            this.lblBottomCount.Text = "Number of Nodes on Bottom";
            // 
            // nudTopCount
            // 
            this.nudTopCount.Location = new Point(253, 19);
            this.nudTopCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopCount.Name = "nudTopCount";
            this.nudTopCount.Size = new Size(71, 20);
            this.nudTopCount.TabIndex = 8;
            this.nudTopCount.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudTopCount.ValueChanged += new EventHandler(this.control_ValueChanged);
            // 
            // lblTopCount
            // 
            this.lblTopCount.AutoSize = true;
            this.lblTopCount.Location = new Point(197, 3);
            this.lblTopCount.Name = "lblTopCount";
            this.lblTopCount.Size = new Size(127, 13);
            this.lblTopCount.TabIndex = 12;
            this.lblTopCount.Text = "Number of Nodes on Top";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = Color.Black;
            this.pbPreview.Location = new Point(3, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new Size(172, 244);
            this.pbPreview.TabIndex = 15;
            this.pbPreview.TabStop = false;
            this.pbPreview.Paint += new PaintEventHandler(this.pbPreview_Paint);
            // 
            // pbLinkBottom
            // 
            this.pbLinkBottom.BackColor = Color.Transparent;
            this.pbLinkBottom.BackgroundImage = Resources.LinkBottom;
            this.pbLinkBottom.Location = new Point(330, 67);
            this.pbLinkBottom.Name = "pbLinkBottom";
            this.pbLinkBottom.Size = new Size(5, 4);
            this.pbLinkBottom.TabIndex = 17;
            this.pbLinkBottom.TabStop = false;
            // 
            // pbLinkTop
            // 
            this.pbLinkTop.BackColor = Color.Transparent;
            this.pbLinkTop.BackgroundImage = Resources.LinkTop;
            this.pbLinkTop.Location = new Point(330, 28);
            this.pbLinkTop.Name = "pbLinkTop";
            this.pbLinkTop.Size = new Size(5, 4);
            this.pbLinkTop.TabIndex = 18;
            this.pbLinkTop.TabStop = false;
            // 
            // WindowFrame
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbLinkTop);
            this.Controls.Add(this.pbLinkBottom);
            this.Controls.Add(this.pbLink);
            this.Controls.Add(this.nudBottomCount);
            this.Controls.Add(this.nudSideCount);
            this.Controls.Add(this.lblSideCount);
            this.Controls.Add(this.lblBottomCount);
            this.Controls.Add(this.nudTopCount);
            this.Controls.Add(this.lblTopCount);
            this.Controls.Add(this.pbPreview);
            this.Name = "WindowFrame";
            this.Size = new Size(350, 250);
            ((ISupportInitialize)(this.pbLink)).EndInit();
            ((ISupportInitialize)(this.nudBottomCount)).EndInit();
            ((ISupportInitialize)(this.nudSideCount)).EndInit();
            ((ISupportInitialize)(this.nudTopCount)).EndInit();
            ((ISupportInitialize)(this.pbPreview)).EndInit();
            ((ISupportInitialize)(this.pbLinkBottom)).EndInit();
            ((ISupportInitialize)(this.pbLinkTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown nudBottomCount;
        private NumericUpDown nudSideCount;
        private Label lblSideCount;
        private Label lblBottomCount;
        private NumericUpDown nudTopCount;
        private Label lblTopCount;
        private PictureBox pbPreview;
        private PictureBox pbLink;
        private PictureBox pbLinkBottom;
        private PictureBox pbLinkTop;
    }
}
