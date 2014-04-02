namespace NutcrackerEffectsControl
{
    sealed partial class NutcrackerEffectControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.lblSpeed = new System.Windows.Forms.Label();
            this.tbSpeed = new System.Windows.Forms.TrackBar();
            this.btnPalette = new System.Windows.Forms.Button();
            this.cbEffects = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkBoxPalette1 = new System.Windows.Forms.CheckBox();
            this.chkBoxPalette2 = new System.Windows.Forms.CheckBox();
            this.chkBoxPalette3 = new System.Windows.Forms.CheckBox();
            this.chkBoxPalette4 = new System.Windows.Forms.CheckBox();
            this.chkBoxPalette5 = new System.Windows.Forms.CheckBox();
            this.chkBoxPalette6 = new System.Windows.Forms.CheckBox();
            this.palette1 = new System.Windows.Forms.Label();
            this.palette2 = new System.Windows.Forms.Label();
            this.palette3 = new System.Windows.Forms.Label();
            this.palette5 = new System.Windows.Forms.Label();
            this.palette4 = new System.Windows.Forms.Label();
            this.palette6 = new System.Windows.Forms.Label();
            this.tbNotes = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(326, 6);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(38, 13);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "Speed";
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(323, 22);
            this.tbSpeed.Maximum = 20;
            this.tbSpeed.Minimum = 1;
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbSpeed.Size = new System.Drawing.Size(45, 147);
            this.tbSpeed.TabIndex = 1;
            this.tbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbSpeed.Value = 1;
            // 
            // btnPalette
            // 
            this.btnPalette.Location = new System.Drawing.Point(242, 1);
            this.btnPalette.Name = "btnPalette";
            this.btnPalette.Size = new System.Drawing.Size(75, 23);
            this.btnPalette.TabIndex = 2;
            this.btnPalette.Text = "Palette";
            this.btnPalette.UseVisualStyleBackColor = true;
            this.btnPalette.Click += new System.EventHandler(this.btnPalette_Click);
            // 
            // cbEffects
            // 
            this.cbEffects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEffects.FormattingEnabled = true;
            this.cbEffects.Location = new System.Drawing.Point(3, 3);
            this.cbEffects.Name = "cbEffects";
            this.cbEffects.Size = new System.Drawing.Size(233, 21);
            this.cbEffects.TabIndex = 9;
            this.cbEffects.SelectedIndexChanged += new System.EventHandler(this.cbEffects_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(4, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 191);
            this.panel1.TabIndex = 10;
            // 
            // chkBoxPalette1
            // 
            this.chkBoxPalette1.AutoSize = true;
            this.chkBoxPalette1.Checked = true;
            this.chkBoxPalette1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxPalette1.Location = new System.Drawing.Point(255, 31);
            this.chkBoxPalette1.Name = "chkBoxPalette1";
            this.chkBoxPalette1.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette1.TabIndex = 11;
            this.chkBoxPalette1.UseVisualStyleBackColor = true;
            this.chkBoxPalette1.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette2
            // 
            this.chkBoxPalette2.AutoSize = true;
            this.chkBoxPalette2.Checked = true;
            this.chkBoxPalette2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxPalette2.Location = new System.Drawing.Point(255, 55);
            this.chkBoxPalette2.Name = "chkBoxPalette2";
            this.chkBoxPalette2.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette2.TabIndex = 12;
            this.chkBoxPalette2.UseVisualStyleBackColor = true;
            this.chkBoxPalette2.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette3
            // 
            this.chkBoxPalette3.AutoSize = true;
            this.chkBoxPalette3.Location = new System.Drawing.Point(255, 79);
            this.chkBoxPalette3.Name = "chkBoxPalette3";
            this.chkBoxPalette3.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette3.TabIndex = 13;
            this.chkBoxPalette3.UseVisualStyleBackColor = true;
            this.chkBoxPalette3.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette4
            // 
            this.chkBoxPalette4.AutoSize = true;
            this.chkBoxPalette4.Location = new System.Drawing.Point(255, 103);
            this.chkBoxPalette4.Name = "chkBoxPalette4";
            this.chkBoxPalette4.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette4.TabIndex = 14;
            this.chkBoxPalette4.UseVisualStyleBackColor = true;
            this.chkBoxPalette4.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette5
            // 
            this.chkBoxPalette5.AutoSize = true;
            this.chkBoxPalette5.Location = new System.Drawing.Point(255, 127);
            this.chkBoxPalette5.Name = "chkBoxPalette5";
            this.chkBoxPalette5.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette5.TabIndex = 15;
            this.chkBoxPalette5.UseVisualStyleBackColor = true;
            this.chkBoxPalette5.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // chkBoxPalette6
            // 
            this.chkBoxPalette6.AutoSize = true;
            this.chkBoxPalette6.Location = new System.Drawing.Point(254, 151);
            this.chkBoxPalette6.Name = "chkBoxPalette6";
            this.chkBoxPalette6.Size = new System.Drawing.Size(15, 14);
            this.chkBoxPalette6.TabIndex = 16;
            this.chkBoxPalette6.UseVisualStyleBackColor = true;
            this.chkBoxPalette6.CheckedChanged += new System.EventHandler(this.OnControlChanged);
            // 
            // palette1
            // 
            this.palette1.BackColor = System.Drawing.Color.Red;
            this.palette1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette1.Location = new System.Drawing.Point(277, 31);
            this.palette1.Name = "palette1";
            this.palette1.Size = new System.Drawing.Size(28, 14);
            this.palette1.TabIndex = 17;
            this.palette1.Text = "1";
            this.palette1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette1.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette1.Click += new System.EventHandler(this.palette_Click);
            // 
            // palette2
            // 
            this.palette2.BackColor = System.Drawing.Color.Lime;
            this.palette2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette2.ForeColor = System.Drawing.Color.Black;
            this.palette2.Location = new System.Drawing.Point(277, 55);
            this.palette2.Name = "palette2";
            this.palette2.Size = new System.Drawing.Size(28, 14);
            this.palette2.TabIndex = 18;
            this.palette2.Text = "2";
            this.palette2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette2.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette2.Click += new System.EventHandler(this.palette_Click);
            // 
            // palette3
            // 
            this.palette3.BackColor = System.Drawing.Color.Blue;
            this.palette3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette3.ForeColor = System.Drawing.Color.White;
            this.palette3.Location = new System.Drawing.Point(277, 79);
            this.palette3.Name = "palette3";
            this.palette3.Size = new System.Drawing.Size(28, 14);
            this.palette3.TabIndex = 19;
            this.palette3.Text = "3";
            this.palette3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette3.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette3.Click += new System.EventHandler(this.palette_Click);
            // 
            // palette5
            // 
            this.palette5.BackColor = System.Drawing.Color.White;
            this.palette5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette5.ForeColor = System.Drawing.Color.Black;
            this.palette5.Location = new System.Drawing.Point(277, 127);
            this.palette5.Name = "palette5";
            this.palette5.Size = new System.Drawing.Size(28, 14);
            this.palette5.TabIndex = 21;
            this.palette5.Text = "5";
            this.palette5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette5.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette5.Click += new System.EventHandler(this.palette_Click);
            // 
            // palette4
            // 
            this.palette4.BackColor = System.Drawing.Color.Yellow;
            this.palette4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette4.ForeColor = System.Drawing.Color.Black;
            this.palette4.Location = new System.Drawing.Point(277, 103);
            this.palette4.Name = "palette4";
            this.palette4.Size = new System.Drawing.Size(28, 14);
            this.palette4.TabIndex = 22;
            this.palette4.Text = "4";
            this.palette4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette4.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette4.Click += new System.EventHandler(this.palette_Click);
            // 
            // palette6
            // 
            this.palette6.BackColor = System.Drawing.Color.Black;
            this.palette6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.palette6.ForeColor = System.Drawing.Color.White;
            this.palette6.Location = new System.Drawing.Point(277, 151);
            this.palette6.Name = "palette6";
            this.palette6.Size = new System.Drawing.Size(28, 14);
            this.palette6.TabIndex = 23;
            this.palette6.Text = "6";
            this.palette6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.palette6.BackColorChanged += new System.EventHandler(this.OnControlChanged);
            this.palette6.Click += new System.EventHandler(this.palette_Click);
            // 
            // tbNotes
            // 
            this.tbNotes.Location = new System.Drawing.Point(243, 172);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(121, 50);
            this.tbNotes.TabIndex = 24;
            // 
            // NutcrackerEffectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
            this.Size = new System.Drawing.Size(371, 225);
            this.Load += new System.EventHandler(this.NutcrackerEffectControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar tbSpeed;
        private System.Windows.Forms.Button btnPalette;
        private System.Windows.Forms.ComboBox cbEffects;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkBoxPalette1;
        private System.Windows.Forms.CheckBox chkBoxPalette2;
        private System.Windows.Forms.CheckBox chkBoxPalette3;
        private System.Windows.Forms.CheckBox chkBoxPalette4;
        private System.Windows.Forms.CheckBox chkBoxPalette5;
        private System.Windows.Forms.CheckBox chkBoxPalette6;
        private System.Windows.Forms.Label palette1;
        private System.Windows.Forms.Label palette2;
        private System.Windows.Forms.Label palette3;
        private System.Windows.Forms.Label palette5;
        private System.Windows.Forms.Label palette4;
        private System.Windows.Forms.Label palette6;
        private System.Windows.Forms.TextBox tbNotes;
    }
}
