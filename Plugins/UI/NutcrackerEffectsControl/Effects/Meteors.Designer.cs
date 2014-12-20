using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Meteors {
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
            this.tbTrailLength = new TrackBar();
            this.lblTrailLength = new Label();
            this.tbCount = new TrackBar();
            this.lblCount = new Label();
            this.lblType = new Label();
            this.cbType = new ComboBox();
            this.chkBoxUp = new CheckBox();
            ((ISupportInitialize)(this.tbTrailLength)).BeginInit();
            ((ISupportInitialize)(this.tbCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTrailLength
            // 
            this.tbTrailLength.AutoSize = false;
            this.tbTrailLength.Location = new Point(90, 70);
            this.tbTrailLength.Maximum = 100;
            this.tbTrailLength.Minimum = 1;
            this.tbTrailLength.Name = "tbTrailLength";
            this.tbTrailLength.Size = new Size(139, 25);
            this.tbTrailLength.TabIndex = 2;
            this.tbTrailLength.TickStyle = TickStyle.None;
            this.tbTrailLength.Value = 25;
            this.tbTrailLength.ValueChanged += new EventHandler(this.Meteors_ControlChanged);
            // 
            // lblTrailLength
            // 
            this.lblTrailLength.AutoSize = true;
            this.lblTrailLength.Location = new Point(12, 76);
            this.lblTrailLength.Name = "lblTrailLength";
            this.lblTrailLength.Size = new Size(63, 13);
            this.lblTrailLength.TabIndex = 6;
            this.lblTrailLength.Text = "Trail Length";
            // 
            // tbCount
            // 
            this.tbCount.AutoSize = false;
            this.tbCount.Location = new Point(90, 39);
            this.tbCount.Maximum = 100;
            this.tbCount.Minimum = 1;
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new Size(139, 25);
            this.tbCount.TabIndex = 1;
            this.tbCount.TickStyle = TickStyle.None;
            this.tbCount.Value = 10;
            this.tbCount.ValueChanged += new EventHandler(this.Meteors_ControlChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new Point(40, 45);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new Size(35, 13);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "Count";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new Point(44, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(31, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Rainbow",
            "Range",
            "Palette"});
            this.cbType.Location = new Point(90, 12);
            this.cbType.Name = "cbType";
            this.cbType.Size = new Size(139, 21);
            this.cbType.TabIndex = 0;
            this.cbType.SelectedIndexChanged += new EventHandler(this.Meteors_ControlChanged);
            // 
            // chkBoxUp
            // 
            this.chkBoxUp.AutoSize = true;
            this.chkBoxUp.Location = new Point(90, 102);
            this.chkBoxUp.Name = "chkBoxUp";
            this.chkBoxUp.Size = new Size(59, 17);
            this.chkBoxUp.TabIndex = 3;
            this.chkBoxUp.Text = "Fall Up";
            this.chkBoxUp.UseVisualStyleBackColor = true;
            this.chkBoxUp.CheckedChanged += new EventHandler(this.Meteors_ControlChanged);
            // 
            // Meteors
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.chkBoxUp);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.tbTrailLength);
            this.Controls.Add(this.lblTrailLength);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.lblCount);
            this.Name = "Meteors";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbTrailLength)).EndInit();
            ((ISupportInitialize)(this.tbCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBar tbTrailLength;
        private Label lblTrailLength;
        private TrackBar tbCount;
        private Label lblCount;
        private Label lblType;
        private ComboBox cbType;
        private CheckBox chkBoxUp;
    }
}
