using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker
{
    sealed partial class NutcrackerEffectControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSpeed = new Label();
            this.tbSpeed = new TrackBar();
            this.btnPalette = new Button();
            this.cbEffects = new ComboBox();
            this.panel1 = new Panel();
            this.chkBoxPalette1 = new CheckBox();
            this.chkBoxPalette2 = new CheckBox();
            this.chkBoxPalette3 = new CheckBox();
            this.chkBoxPalette4 = new CheckBox();
            this.chkBoxPalette5 = new CheckBox();
            this.chkBoxPalette6 = new CheckBox();
            this.palette1 = new Label();
            this.palette2 = new Label();
            this.palette3 = new Label();
            this.palette5 = new Label();
            this.palette4 = new Label();
            this.palette6 = new Label();
            this.tbNotes = new TextBox();
            ((ISupportInitialize)(this.tbSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new Point(326, 6);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new Size(38, 13);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "Speed";
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new Point(323, 22);
            this.tbSpeed.Maximum = 20;
            this.tbSpeed.Minimum = 1;
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Orientation = Orientation.Vertical;
            this.tbSpeed.Size = new Size(45, 147);
            this.tbSpeed.TabIndex = 1;
            this.tbSpeed.TickStyle = TickStyle.Both;
            this.tbSpeed.Value = 1;
            // 
            // btnPalette
            // 
            this.btnPalette.Location = new Point(242, 1);
            this.btnPalette.Name = "btnPalette";
            this.btnPalette.Size = new Size(75, 23);
            this.btnPalette.TabIndex = 2;
            this.btnPalette.Text = "Palette";
            this.btnPalette.UseVisualStyleBackColor = true;
            this.btnPalette.Click += new EventHandler(this.btnPalette_Click);
            // 
            // cbEffects
            // 
            this.cbEffects.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbEffects.FormattingEnabled = true;
            this.cbEffects.Location = new Point(3, 3);
            this.cbEffects.Name = "cbEffects";
            this.cbEffects.Size = new Size(233, 21);
            this.cbEffects.TabIndex = 9;
            this.cbEffects.SelectedIndexChanged += new EventHandler(this.cbEffects_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new Point(4, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(232, 191);
            this.panel1.TabIndex = 10;
            // 
            // chkBoxPalette1
            // 
            this.chkBoxPalette1.AutoSize = true;
            this.chkBoxPalette1.Checked = true;
            this.chkBoxPalette1.CheckState = CheckState.Checked;
            this.chkBoxPalette1.Location = new Point(255, 31);
            this.chkBoxPalette1.Name = "chkBoxPalette1";
            this.chkBoxPalette1.Size = new Size(15, 14);
            this.chkBoxPalette1.TabIndex = 11;
            this.chkBoxPalette1.UseVisualStyleBackColor = true;
            this.chkBoxPalette1.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette2
            // 
            this.chkBoxPalette2.AutoSize = true;
            this.chkBoxPalette2.Checked = true;
            this.chkBoxPalette2.CheckState = CheckState.Checked;
            this.chkBoxPalette2.Location = new Point(255, 55);
            this.chkBoxPalette2.Name = "chkBoxPalette2";
            this.chkBoxPalette2.Size = new Size(15, 14);
            this.chkBoxPalette2.TabIndex = 12;
            this.chkBoxPalette2.UseVisualStyleBackColor = true;
            this.chkBoxPalette2.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette3
            // 
            this.chkBoxPalette3.AutoSize = true;
            this.chkBoxPalette3.Location = new Point(255, 79);
            this.chkBoxPalette3.Name = "chkBoxPalette3";
            this.chkBoxPalette3.Size = new Size(15, 14);
            this.chkBoxPalette3.TabIndex = 13;
            this.chkBoxPalette3.UseVisualStyleBackColor = true;
            this.chkBoxPalette3.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette4
            // 
            this.chkBoxPalette4.AutoSize = true;
            this.chkBoxPalette4.Location = new Point(255, 103);
            this.chkBoxPalette4.Name = "chkBoxPalette4";
            this.chkBoxPalette4.Size = new Size(15, 14);
            this.chkBoxPalette4.TabIndex = 14;
            this.chkBoxPalette4.UseVisualStyleBackColor = true;
            this.chkBoxPalette4.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette5
            // 
            this.chkBoxPalette5.AutoSize = true;
            this.chkBoxPalette5.Location = new Point(255, 127);
            this.chkBoxPalette5.Name = "chkBoxPalette5";
            this.chkBoxPalette5.Size = new Size(15, 14);
            this.chkBoxPalette5.TabIndex = 15;
            this.chkBoxPalette5.UseVisualStyleBackColor = true;
            this.chkBoxPalette5.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette6
            // 
            this.chkBoxPalette6.AutoSize = true;
            this.chkBoxPalette6.Location = new Point(254, 151);
            this.chkBoxPalette6.Name = "chkBoxPalette6";
            this.chkBoxPalette6.Size = new Size(15, 14);
            this.chkBoxPalette6.TabIndex = 16;
            this.chkBoxPalette6.UseVisualStyleBackColor = true;
            this.chkBoxPalette6.CheckedChanged += new EventHandler(this.OnControlChanged);
            // 
            // palette1
            // 
            this.palette1.BackColor = Color.Red;
            this.palette1.BorderStyle = BorderStyle.FixedSingle;
            this.palette1.Location = new Point(277, 31);
            this.palette1.Name = "palette1";
            this.palette1.Size = new Size(28, 14);
            this.palette1.TabIndex = 17;
            this.palette1.Text = "1";
            this.palette1.TextAlign = ContentAlignment.TopCenter;
            this.palette1.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette1.Click += new EventHandler(this.palette_Click);
            // 
            // palette2
            // 
            this.palette2.BackColor = Color.Lime;
            this.palette2.BorderStyle = BorderStyle.FixedSingle;
            this.palette2.ForeColor = Color.Black;
            this.palette2.Location = new Point(277, 55);
            this.palette2.Name = "palette2";
            this.palette2.Size = new Size(28, 14);
            this.palette2.TabIndex = 18;
            this.palette2.Text = "2";
            this.palette2.TextAlign = ContentAlignment.TopCenter;
            this.palette2.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette2.Click += new EventHandler(this.palette_Click);
            // 
            // palette3
            // 
            this.palette3.BackColor = Color.Blue;
            this.palette3.BorderStyle = BorderStyle.FixedSingle;
            this.palette3.ForeColor = Color.White;
            this.palette3.Location = new Point(277, 79);
            this.palette3.Name = "palette3";
            this.palette3.Size = new Size(28, 14);
            this.palette3.TabIndex = 19;
            this.palette3.Text = "3";
            this.palette3.TextAlign = ContentAlignment.TopCenter;
            this.palette3.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette3.Click += new EventHandler(this.palette_Click);
            // 
            // palette5
            // 
            this.palette5.BackColor = Color.White;
            this.palette5.BorderStyle = BorderStyle.FixedSingle;
            this.palette5.ForeColor = Color.Black;
            this.palette5.Location = new Point(277, 127);
            this.palette5.Name = "palette5";
            this.palette5.Size = new Size(28, 14);
            this.palette5.TabIndex = 21;
            this.palette5.Text = "5";
            this.palette5.TextAlign = ContentAlignment.TopCenter;
            this.palette5.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette5.Click += new EventHandler(this.palette_Click);
            // 
            // palette4
            // 
            this.palette4.BackColor = Color.Yellow;
            this.palette4.BorderStyle = BorderStyle.FixedSingle;
            this.palette4.ForeColor = Color.Black;
            this.palette4.Location = new Point(277, 103);
            this.palette4.Name = "palette4";
            this.palette4.Size = new Size(28, 14);
            this.palette4.TabIndex = 22;
            this.palette4.Text = "4";
            this.palette4.TextAlign = ContentAlignment.TopCenter;
            this.palette4.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette4.Click += new EventHandler(this.palette_Click);
            // 
            // palette6
            // 
            this.palette6.BackColor = Color.Black;
            this.palette6.BorderStyle = BorderStyle.FixedSingle;
            this.palette6.ForeColor = Color.White;
            this.palette6.Location = new Point(277, 151);
            this.palette6.Name = "palette6";
            this.palette6.Size = new Size(28, 14);
            this.palette6.TabIndex = 23;
            this.palette6.Text = "6";
            this.palette6.TextAlign = ContentAlignment.TopCenter;
            this.palette6.BackColorChanged += new EventHandler(this.OnControlChanged);
            this.palette6.Click += new EventHandler(this.palette_Click);
            // 
            // tbNotes
            // 
            this.tbNotes.Location = new Point(243, 172);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new Size(121, 50);
            this.tbNotes.TabIndex = 24;
            // 
            // NutcrackerEffectControl
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbNotes);
            this.Controls.Add(this.palette6);
            this.Controls.Add(this.palette4);
            this.Controls.Add(this.palette5);
            this.Controls.Add(this.palette3);
            this.Controls.Add(this.palette2);
            this.Controls.Add(this.palette1);
            this.Controls.Add(this.chkBoxPalette6);
            this.Controls.Add(this.chkBoxPalette5);
            this.Controls.Add(this.chkBoxPalette4);
            this.Controls.Add(this.chkBoxPalette3);
            this.Controls.Add(this.chkBoxPalette2);
            this.Controls.Add(this.chkBoxPalette1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbEffects);
            this.Controls.Add(this.btnPalette);
            this.Controls.Add(this.tbSpeed);
            this.Controls.Add(this.lblSpeed);
            this.Name = "NutcrackerEffectControl";
            this.Size = new Size(371, 225);
            this.Load += new EventHandler(this.NutcrackerEffectControl_Load);
            ((ISupportInitialize)(this.tbSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblSpeed;
        private TrackBar tbSpeed;
        private Button btnPalette;
        private ComboBox cbEffects;
        private Panel panel1;
        private CheckBox chkBoxPalette1;
        private CheckBox chkBoxPalette2;
        private CheckBox chkBoxPalette3;
        private CheckBox chkBoxPalette4;
        private CheckBox chkBoxPalette5;
        private CheckBox chkBoxPalette6;
        private Label palette1;
        private Label palette2;
        private Label palette3;
        private Label palette5;
        private Label palette4;
        private Label palette6;
        private TextBox tbNotes;
    }
}
