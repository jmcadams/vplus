using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlusCommon
{
    sealed partial class ColorEditor
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
        this.rLabel = new Label();
        this.rNumericUpDown = new NumericUpDown();
        this.gLabel = new Label();
        this.gNumericUpDown = new NumericUpDown();
        this.bLabel = new Label();
        this.bNumericUpDown = new NumericUpDown();
        this.hexLabel = new Label();
        this.hexTextBox = new ComboBox();
        this.bColorBar = new RgbaColorSlider();
        this.gColorBar = new RgbaColorSlider();
        this.rColorBar = new RgbaColorSlider();
        ((ISupportInitialize)(this.rNumericUpDown)).BeginInit();
        ((ISupportInitialize)(this.gNumericUpDown)).BeginInit();
        ((ISupportInitialize)(this.bNumericUpDown)).BeginInit();
        this.SuspendLayout();
        // 
        // rLabel
        // 
        this.rLabel.AutoSize = true;
        this.rLabel.Location = new Point(12, 0);
        this.rLabel.Name = "rLabel";
        this.rLabel.Size = new Size(18, 13);
        this.rLabel.TabIndex = 1;
        this.rLabel.Text = "R:";
        this.rLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // rNumericUpDown
        // 
        this.rNumericUpDown.Location = new Point(105, -2);
        this.rNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.rNumericUpDown.Name = "rNumericUpDown";
        this.rNumericUpDown.Size = new Size(58, 20);
        this.rNumericUpDown.TabIndex = 3;
        this.rNumericUpDown.TextAlign = HorizontalAlignment.Right;
        this.rNumericUpDown.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // gLabel
        // 
        this.gLabel.AutoSize = true;
        this.gLabel.Location = new Point(12, 26);
        this.gLabel.Name = "gLabel";
        this.gLabel.Size = new Size(18, 13);
        this.gLabel.TabIndex = 4;
        this.gLabel.Text = "G:";
        this.gLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // gNumericUpDown
        // 
        this.gNumericUpDown.Location = new Point(105, 24);
        this.gNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.gNumericUpDown.Name = "gNumericUpDown";
        this.gNumericUpDown.Size = new Size(58, 20);
        this.gNumericUpDown.TabIndex = 6;
        this.gNumericUpDown.TextAlign = HorizontalAlignment.Right;
        this.gNumericUpDown.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // bLabel
        // 
        this.bLabel.AutoSize = true;
        this.bLabel.Location = new Point(13, 54);
        this.bLabel.Name = "bLabel";
        this.bLabel.Size = new Size(17, 13);
        this.bLabel.TabIndex = 7;
        this.bLabel.Text = "B:";
        this.bLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // bNumericUpDown
        // 
        this.bNumericUpDown.Location = new Point(105, 52);
        this.bNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
        this.bNumericUpDown.Name = "bNumericUpDown";
        this.bNumericUpDown.Size = new Size(58, 20);
        this.bNumericUpDown.TabIndex = 9;
        this.bNumericUpDown.TextAlign = HorizontalAlignment.Right;
        this.bNumericUpDown.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // hexLabel
        // 
        this.hexLabel.AutoSize = true;
        this.hexLabel.Location = new Point(1, 81);
        this.hexLabel.Name = "hexLabel";
        this.hexLabel.Size = new Size(29, 13);
        this.hexLabel.TabIndex = 10;
        this.hexLabel.Text = "Hex:";
        // 
        // hexTextBox
        // 
        this.hexTextBox.DrawMode = DrawMode.OwnerDrawFixed;
        this.hexTextBox.Location = new Point(105, 78);
        this.hexTextBox.Name = "hexTextBox";
        this.hexTextBox.Size = new Size(58, 21);
        this.hexTextBox.TabIndex = 11;
        this.hexTextBox.DrawItem += new DrawItemEventHandler(this.hexTextBox_DrawItem);
        this.hexTextBox.SelectedIndexChanged += new EventHandler(this.hexTextBox_SelectedIndexChanged);
        this.hexTextBox.TextChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // bColorBar
        // 
        this.bColorBar.Channel = RgbaChannel.Blue;
        this.bColorBar.Location = new Point(27, 52);
        this.bColorBar.Name = "bColorBar";
        this.bColorBar.NubColor = Color.Blue;
        this.bColorBar.Size = new Size(72, 20);
        this.bColorBar.TabIndex = 8;
        this.bColorBar.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // gColorBar
        // 
        this.gColorBar.Channel = RgbaChannel.Green;
        this.gColorBar.Location = new Point(27, 26);
        this.gColorBar.Name = "gColorBar";
        this.gColorBar.NubColor = Color.Lime;
        this.gColorBar.Size = new Size(72, 20);
        this.gColorBar.TabIndex = 5;
        this.gColorBar.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // rColorBar
        // 
        this.rColorBar.Location = new Point(27, 0);
        this.rColorBar.Name = "rColorBar";
        this.rColorBar.NubColor = Color.Red;
        this.rColorBar.Size = new Size(72, 20);
        this.rColorBar.TabIndex = 2;
        this.rColorBar.ValueChanged += new EventHandler(this.ValueChangedHandler);
        // 
        // ColorEditor
        // 
        this.AutoScaleDimensions = new SizeF(6F, 13F);
        this.AutoScaleMode = AutoScaleMode.Font;
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
        this.Size = new Size(166, 102);
        ((ISupportInitialize)(this.rNumericUpDown)).EndInit();
        ((ISupportInitialize)(this.gNumericUpDown)).EndInit();
        ((ISupportInitialize)(this.bNumericUpDown)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private Label rLabel;
    private NumericUpDown rNumericUpDown;
    private RgbaColorSlider rColorBar;
    private Label gLabel;
    private RgbaColorSlider gColorBar;
    private NumericUpDown gNumericUpDown;
    private Label bLabel;
    private RgbaColorSlider bColorBar;
    private NumericUpDown bNumericUpDown;
    private Label hexLabel;
    private ComboBox hexTextBox;
  }
}
