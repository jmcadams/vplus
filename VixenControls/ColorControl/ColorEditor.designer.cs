namespace VixenPlusCommon
{
    sealed partial class ColorEditor
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
        this.rLabel = new System.Windows.Forms.Label();
        this.rNumericUpDown = new System.Windows.Forms.NumericUpDown();
        this.gLabel = new System.Windows.Forms.Label();
        this.gNumericUpDown = new System.Windows.Forms.NumericUpDown();
        this.bLabel = new System.Windows.Forms.Label();
        this.bNumericUpDown = new System.Windows.Forms.NumericUpDown();
        this.hexLabel = new System.Windows.Forms.Label();
        this.hexTextBox = new System.Windows.Forms.ComboBox();
        this.bColorBar = new RgbaColorSlider();
        this.gColorBar = new RgbaColorSlider();
        this.rColorBar = new RgbaColorSlider();
        ((System.ComponentModel.ISupportInitialize)(this.rNumericUpDown)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.gNumericUpDown)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).BeginInit();
        this.SuspendLayout();
        // 
        // rLabel
        // 
        this.rLabel.AutoSize = true;
        this.rLabel.Location = new System.Drawing.Point(12, 0);
        this.rLabel.Name = "rLabel";
        this.rLabel.Size = new System.Drawing.Size(18, 13);
        this.rLabel.TabIndex = 1;
        this.rLabel.Text = "R:";
        this.rLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // rNumericUpDown
        // 
        this.rNumericUpDown.Location = new System.Drawing.Point(105, -2);
        this.rNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.rNumericUpDown.Name = "rNumericUpDown";
        this.rNumericUpDown.Size = new System.Drawing.Size(58, 20);
        this.rNumericUpDown.TabIndex = 3;
        this.rNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.rNumericUpDown.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // gLabel
        // 
        this.gLabel.AutoSize = true;
        this.gLabel.Location = new System.Drawing.Point(12, 26);
        this.gLabel.Name = "gLabel";
        this.gLabel.Size = new System.Drawing.Size(18, 13);
        this.gLabel.TabIndex = 4;
        this.gLabel.Text = "G:";
        this.gLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // gNumericUpDown
        // 
        this.gNumericUpDown.Location = new System.Drawing.Point(105, 24);
        this.gNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.gNumericUpDown.Name = "gNumericUpDown";
        this.gNumericUpDown.Size = new System.Drawing.Size(58, 20);
        this.gNumericUpDown.TabIndex = 6;
        this.gNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.gNumericUpDown.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // bLabel
        // 
        this.bLabel.AutoSize = true;
        this.bLabel.Location = new System.Drawing.Point(13, 54);
        this.bLabel.Name = "bLabel";
        this.bLabel.Size = new System.Drawing.Size(17, 13);
        this.bLabel.TabIndex = 7;
        this.bLabel.Text = "B:";
        this.bLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // bNumericUpDown
        // 
        this.bNumericUpDown.Location = new System.Drawing.Point(105, 52);
        this.bNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.bNumericUpDown.Name = "bNumericUpDown";
        this.bNumericUpDown.Size = new System.Drawing.Size(58, 20);
        this.bNumericUpDown.TabIndex = 9;
        this.bNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.bNumericUpDown.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // hexLabel
        // 
        this.hexLabel.AutoSize = true;
        this.hexLabel.Location = new System.Drawing.Point(1, 81);
        this.hexLabel.Name = "hexLabel";
        this.hexLabel.Size = new System.Drawing.Size(29, 13);
        this.hexLabel.TabIndex = 10;
        this.hexLabel.Text = "Hex:";
        // 
        // hexTextBox
        // 
        this.hexTextBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        this.hexTextBox.Location = new System.Drawing.Point(105, 78);
        this.hexTextBox.Name = "hexTextBox";
        this.hexTextBox.Size = new System.Drawing.Size(58, 21);
        this.hexTextBox.TabIndex = 11;
        this.hexTextBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.hexTextBox_DrawItem);
        this.hexTextBox.SelectedIndexChanged += new System.EventHandler(this.hexTextBox_SelectedIndexChanged);
        this.hexTextBox.TextChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // bColorBar
        // 
        this.bColorBar.Channel = RgbaChannel.Blue;
        this.bColorBar.Location = new System.Drawing.Point(27, 52);
        this.bColorBar.Name = "bColorBar";
        this.bColorBar.NubColor = System.Drawing.Color.Blue;
        this.bColorBar.Size = new System.Drawing.Size(72, 20);
        this.bColorBar.TabIndex = 8;
        this.bColorBar.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // gColorBar
        // 
        this.gColorBar.Channel = RgbaChannel.Green;
        this.gColorBar.Location = new System.Drawing.Point(27, 26);
        this.gColorBar.Name = "gColorBar";
        this.gColorBar.NubColor = System.Drawing.Color.Lime;
        this.gColorBar.Size = new System.Drawing.Size(72, 20);
        this.gColorBar.TabIndex = 5;
        this.gColorBar.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // rColorBar
        // 
        this.rColorBar.Location = new System.Drawing.Point(27, 0);
        this.rColorBar.Name = "rColorBar";
        this.rColorBar.NubColor = System.Drawing.Color.Red;
        this.rColorBar.Size = new System.Drawing.Size(72, 20);
        this.rColorBar.TabIndex = 2;
        this.rColorBar.ValueChanged += new System.EventHandler(this.ValueChangedHandler);
        // 
        // ColorEditor
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.hexTextBox);
        this.Controls.Add(this.hexLabel);
        this.Controls.Add(this.bNumericUpDown);
        this.Controls.Add(this.bColorBar);
        this.Controls.Add(this.bLabel);
        this.Controls.Add(this.gNumericUpDown);
        this.Controls.Add(this.gColorBar);
        this.Controls.Add(this.gLabel);
        this.Controls.Add(this.rColorBar);
        this.Controls.Add(this.rNumericUpDown);
        this.Controls.Add(this.rLabel);
        this.Name = "ColorEditor";
        this.Size = new System.Drawing.Size(166, 102);
        ((System.ComponentModel.ISupportInitialize)(this.rNumericUpDown)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.gNumericUpDown)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label rLabel;
    private System.Windows.Forms.NumericUpDown rNumericUpDown;
    private RgbaColorSlider rColorBar;
    private System.Windows.Forms.Label gLabel;
    private RgbaColorSlider gColorBar;
    private System.Windows.Forms.NumericUpDown gNumericUpDown;
    private System.Windows.Forms.Label bLabel;
    private RgbaColorSlider bColorBar;
    private System.Windows.Forms.NumericUpDown bNumericUpDown;
    private System.Windows.Forms.Label hexLabel;
    private System.Windows.Forms.ComboBox hexTextBox;
  }
}
