using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Bars {
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
            this.lblPaletteRep = new Label();
            this.tbRepeat = new TrackBar();
            this.lblDirection = new Label();
            this.cbDirection = new ComboBox();
            this.cbHighlight = new CheckBox();
            this.cb3D = new CheckBox();
            ((ISupportInitialize)(this.tbRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPaletteRep
            // 
            this.lblPaletteRep.AutoSize = true;
            this.lblPaletteRep.Location = new Point(2, 8);
            this.lblPaletteRep.Name = "lblPaletteRep";
            this.lblPaletteRep.Size = new Size(78, 13);
            this.lblPaletteRep.TabIndex = 0;
            this.lblPaletteRep.Text = "Palette Repeat";
            // 
            // tbRepeat
            // 
            this.tbRepeat.AutoSize = false;
            this.tbRepeat.LargeChange = 2;
            this.tbRepeat.Location = new Point(79, 2);
            this.tbRepeat.Maximum = 5;
            this.tbRepeat.Minimum = 1;
            this.tbRepeat.Name = "tbRepeat";
            this.tbRepeat.Size = new Size(150, 25);
            this.tbRepeat.TabIndex = 1;
            this.tbRepeat.TabStop = false;
            this.tbRepeat.TickStyle = TickStyle.None;
            this.tbRepeat.Value = 1;
            this.tbRepeat.ValueChanged += new EventHandler(this.Bars_ControlChanged);
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new Point(31, 39);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new Size(49, 13);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction";
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Expand",
            "Compress"});
            this.cbDirection.Location = new Point(86, 36);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new Size(140, 21);
            this.cbDirection.TabIndex = 3;
            this.cbDirection.SelectedIndexChanged += new EventHandler(this.Bars_ControlChanged);
            // 
            // cbHighlight
            // 
            this.cbHighlight.AutoSize = true;
            this.cbHighlight.Location = new Point(13, 66);
            this.cbHighlight.Name = "cbHighlight";
            this.cbHighlight.Size = new Size(67, 17);
            this.cbHighlight.TabIndex = 4;
            this.cbHighlight.Text = "Highlight";
            this.cbHighlight.UseVisualStyleBackColor = true;
            this.cbHighlight.CheckedChanged += new EventHandler(this.Bars_ControlChanged);
            // 
            // cb3D
            // 
            this.cb3D.AutoSize = true;
            this.cb3D.Location = new Point(13, 89);
            this.cb3D.Name = "cb3D";
            this.cb3D.Size = new Size(40, 17);
            this.cb3D.TabIndex = 5;
            this.cb3D.Text = "3D";
            this.cb3D.UseVisualStyleBackColor = true;
            this.cb3D.CheckedChanged += new EventHandler(this.Bars_ControlChanged);
            // 
            // Bars
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.cb3D);
            this.Controls.Add(this.cbHighlight);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.lblDirection);
            this.Controls.Add(this.tbRepeat);
            this.Controls.Add(this.lblPaletteRep);
            this.Name = "Bars";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbRepeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblPaletteRep;
        private TrackBar tbRepeat;
        private Label lblDirection;
        private ComboBox cbDirection;
        private CheckBox cbHighlight;
        private CheckBox cb3D;
    }
}
