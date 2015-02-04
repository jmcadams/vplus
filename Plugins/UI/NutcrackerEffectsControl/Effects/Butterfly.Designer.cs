using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker.Effects {
    partial class Butterfly {
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
            this.lblColors = new Label();
            this.lblStyle = new Label();
            this.lblBgChunks = new Label();
            this.lblBgSkip = new Label();
            this.tbSkip = new TrackBar();
            this.tbChunks = new TrackBar();
            this.tbStyle = new TrackBar();
            this.cbColors = new ComboBox();
            ((ISupportInitialize)(this.tbSkip)).BeginInit();
            ((ISupportInitialize)(this.tbChunks)).BeginInit();
            ((ISupportInitialize)(this.tbStyle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblColors
            // 
            this.lblColors.AutoSize = true;
            this.lblColors.Location = new Point(71, 3);
            this.lblColors.Name = "lblColors";
            this.lblColors.Size = new Size(36, 13);
            this.lblColors.TabIndex = 0;
            this.lblColors.Text = "Colors";
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new Point(77, 31);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new Size(30, 13);
            this.lblStyle.TabIndex = 1;
            this.lblStyle.Text = "Style";
            // 
            // lblBgChunks
            // 
            this.lblBgChunks.AutoSize = true;
            this.lblBgChunks.Location = new Point(3, 62);
            this.lblBgChunks.Name = "lblBgChunks";
            this.lblBgChunks.Size = new Size(104, 13);
            this.lblBgChunks.TabIndex = 2;
            this.lblBgChunks.Text = "Background Chunks";
            // 
            // lblBgSkip
            // 
            this.lblBgSkip.AutoSize = true;
            this.lblBgSkip.Location = new Point(17, 93);
            this.lblBgSkip.Name = "lblBgSkip";
            this.lblBgSkip.Size = new Size(89, 13);
            this.lblBgSkip.TabIndex = 3;
            this.lblBgSkip.Text = "Background Skip";
            // 
            // tbSkip
            // 
            this.tbSkip.AutoSize = false;
            this.tbSkip.Location = new Point(112, 87);
            this.tbSkip.Minimum = 2;
            this.tbSkip.Name = "tbSkip";
            this.tbSkip.Size = new Size(117, 25);
            this.tbSkip.TabIndex = 4;
            this.tbSkip.TickStyle = TickStyle.None;
            this.tbSkip.Value = 2;
            this.tbSkip.ValueChanged += new EventHandler(this.Butterfly_ControlChanged);
            // 
            // tbChunks
            // 
            this.tbChunks.AutoSize = false;
            this.tbChunks.Location = new Point(112, 56);
            this.tbChunks.Minimum = 1;
            this.tbChunks.Name = "tbChunks";
            this.tbChunks.Size = new Size(117, 25);
            this.tbChunks.TabIndex = 5;
            this.tbChunks.TickStyle = TickStyle.None;
            this.tbChunks.Value = 1;
            this.tbChunks.ValueChanged += new EventHandler(this.Butterfly_ControlChanged);
            // 
            // tbStyle
            // 
            this.tbStyle.AutoSize = false;
            this.tbStyle.Location = new Point(112, 25);
            this.tbStyle.Maximum = 3;
            this.tbStyle.Minimum = 1;
            this.tbStyle.Name = "tbStyle";
            this.tbStyle.Size = new Size(117, 25);
            this.tbStyle.TabIndex = 6;
            this.tbStyle.TickStyle = TickStyle.None;
            this.tbStyle.Value = 1;
            this.tbStyle.ValueChanged += new EventHandler(this.Butterfly_ControlChanged);
            // 
            // cbColors
            // 
            this.cbColors.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbColors.FormattingEnabled = true;
            this.cbColors.Items.AddRange(new object[] {
            "Rainbow",
            "Palette"});
            this.cbColors.Location = new Point(112, 0);
            this.cbColors.Name = "cbColors";
            this.cbColors.Size = new Size(117, 21);
            this.cbColors.TabIndex = 7;
            this.cbColors.SelectedIndexChanged += new EventHandler(this.Butterfly_ControlChanged);
            // 
            // Butterfly
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbColors);
            this.Controls.Add(this.tbStyle);
            this.Controls.Add(this.tbChunks);
            this.Controls.Add(this.tbSkip);
            this.Controls.Add(this.lblBgSkip);
            this.Controls.Add(this.lblBgChunks);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.lblColors);
            this.Name = "Butterfly";
            this.Size = new Size(232, 134);
            ((ISupportInitialize)(this.tbSkip)).EndInit();
            ((ISupportInitialize)(this.tbChunks)).EndInit();
            ((ISupportInitialize)(this.tbStyle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblColors;
        private Label lblStyle;
        private Label lblBgChunks;
        private Label lblBgSkip;
        private TrackBar tbSkip;
        private TrackBar tbChunks;
        private TrackBar tbStyle;
        private ComboBox cbColors;
    }
}
