namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class AutoTextOptionsDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private ErrorProvider errorProvider;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private const int m_delta = 2;
        private Point m_frameDelta = new Point();
        private Generator m_generator = null;
        private Image m_image;
        private Point m_imageLocation = new Point(0, 0);
        private int m_pause;
        private int m_pauseCountdown;
        private Rectangle m_scrollRegion = new Rectangle();
        private Point m_startPoint;
        private int m_textPositionValue = 0;
        private ScrollDirection m_textScrollDirection;
        private NumericUpDown numericUpDownTextHeight;
        private PictureBox pictureBoxImageLarge;
        private PictureBox pictureBoxImageSmall;
        private PictureBox pictureBoxSample;
        private PictureBox pictureBoxScrollDown;
        private PictureBox pictureBoxScrollLeft;
        private PictureBox pictureBoxScrollRight;
        private PictureBox pictureBoxScrollUp;
        private RadioButton radioButtonPositionBottom;
        private RadioButton radioButtonPositionPercent;
        private RadioButton radioButtonPositionPixel;
        private RadioButton radioButtonPositionTop;
        private RadioButton radioButtonScrollOn;
        private RadioButton radioButtonScrollOnOff;
        private TextBox textBoxPositionValue;
        private Timer timer;
        private ToolTip toolTip;

        public AutoTextOptionsDialog(Generator generator)
        {
            this.InitializeComponent();
            this.m_generator = generator;
            this.m_pause = 0x7d0 / this.timer.Interval;
            this.SetScroll(generator.TextScrollDirection);
            this.SetPosition(generator.TextVertPosition, generator.TextVertPositionValue);
            this.SetExtent(generator.TextScrollExtent);
            this.numericUpDownTextHeight.Maximum = generator.BoardPixelHeight;
            this.numericUpDownTextHeight.Value = generator.TextHeight;
        }

        private void AutoTextOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer.Stop();
        }

        private void AutoTextOptionsDialog_Load(object sender, EventArgs e)
        {
            this.timer.Start();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!this.ValidatePositionValue())
            {
                MessageBox.Show("Please fix the error shown before continuing.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = System.Windows.Forms.DialogResult.None;
            }
            else
            {
                this.m_generator.TextHeight = (int) this.numericUpDownTextHeight.Value;
                if (this.radioButtonPositionTop.Checked)
                {
                    this.m_generator.TextVertPosition = VertPosition.Top;
                    this.m_generator.TextVertPositionValue = 0;
                }
                else if (this.radioButtonPositionBottom.Checked)
                {
                    this.m_generator.TextVertPosition = VertPosition.Bottom;
                    this.m_generator.TextVertPositionValue = 0;
                }
                else if (this.radioButtonPositionPercent.Checked)
                {
                    this.m_generator.TextVertPosition = VertPosition.Percent;
                    if (((((this.m_textPositionValue * this.m_generator.BoardPixelHeight) / 100) + this.m_generator.TextHeight) > this.m_generator.BoardPixelHeight) && (MessageBox.Show("Text position's percent value will put the text partially off of the board.\nIs this okay?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No))
                    {
                        base.DialogResult = System.Windows.Forms.DialogResult.None;
                    }
                    this.m_generator.TextVertPositionValue = this.m_textPositionValue;
                }
                else if (this.radioButtonPositionPixel.Checked)
                {
                    this.m_generator.TextVertPosition = VertPosition.Pixel;
                    this.m_generator.TextVertPositionValue = this.m_textPositionValue;
                }
                this.m_generator.TextScrollDirection = this.m_textScrollDirection;
                this.m_generator.TextScrollExtent = this.radioButtonScrollOn.Checked ? ScrollExtent.On : ScrollExtent.OnAndOff;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_generator != null)
            {
                this.m_generator.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AutoTextOptionsDialog));
            this.toolTip = new ToolTip(this.components);
            this.pictureBoxScrollUp = new PictureBox();
            this.pictureBoxScrollDown = new PictureBox();
            this.pictureBoxScrollLeft = new PictureBox();
            this.pictureBoxScrollRight = new PictureBox();
            this.pictureBoxSample = new PictureBox();
            this.groupBox1 = new GroupBox();
            this.textBoxPositionValue = new TextBox();
            this.radioButtonPositionPercent = new RadioButton();
            this.radioButtonPositionPixel = new RadioButton();
            this.radioButtonPositionBottom = new RadioButton();
            this.radioButtonPositionTop = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.radioButtonScrollOnOff = new RadioButton();
            this.radioButtonScrollOn = new RadioButton();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.timer = new Timer(this.components);
            this.pictureBoxImageSmall = new PictureBox();
            this.pictureBoxImageLarge = new PictureBox();
            this.groupBox3 = new GroupBox();
            this.label1 = new Label();
            this.numericUpDownTextHeight = new NumericUpDown();
            this.errorProvider = new ErrorProvider(this.components);
            ((ISupportInitialize) this.pictureBoxScrollUp).BeginInit();
            ((ISupportInitialize) this.pictureBoxScrollDown).BeginInit();
            ((ISupportInitialize) this.pictureBoxScrollLeft).BeginInit();
            ((ISupportInitialize) this.pictureBoxScrollRight).BeginInit();
            ((ISupportInitialize) this.pictureBoxSample).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxImageSmall).BeginInit();
            ((ISupportInitialize) this.pictureBoxImageLarge).BeginInit();
            this.groupBox3.SuspendLayout();
            this.numericUpDownTextHeight.BeginInit();
            ((ISupportInitialize) this.errorProvider).BeginInit();
            base.SuspendLayout();
            this.pictureBoxScrollUp.Image = (Image) manager.GetObject("pictureBoxScrollUp.Image");
            this.pictureBoxScrollUp.Location = new Point(0x71, 0x17);
            this.pictureBoxScrollUp.Name = "pictureBoxScrollUp";
            this.pictureBoxScrollUp.Size = new Size(0x18, 0x18);
            this.pictureBoxScrollUp.TabIndex = 0;
            this.pictureBoxScrollUp.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxScrollUp, "Scroll from bottom to top");
            this.pictureBoxScrollUp.Click += new EventHandler(this.pictureBoxScrollUp_Click);
            this.pictureBoxScrollDown.Image = (Image) manager.GetObject("pictureBoxScrollDown.Image");
            this.pictureBoxScrollDown.Location = new Point(0x71, 0x5b);
            this.pictureBoxScrollDown.Name = "pictureBoxScrollDown";
            this.pictureBoxScrollDown.Size = new Size(0x18, 0x18);
            this.pictureBoxScrollDown.TabIndex = 1;
            this.pictureBoxScrollDown.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxScrollDown, "Scroll from top to bottom");
            this.pictureBoxScrollDown.Click += new EventHandler(this.pictureBoxScrollDown_Click);
            this.pictureBoxScrollLeft.Image = (Image) manager.GetObject("pictureBoxScrollLeft.Image");
            this.pictureBoxScrollLeft.Location = new Point(0x2f, 0x39);
            this.pictureBoxScrollLeft.Name = "pictureBoxScrollLeft";
            this.pictureBoxScrollLeft.Size = new Size(0x18, 0x18);
            this.pictureBoxScrollLeft.TabIndex = 2;
            this.pictureBoxScrollLeft.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxScrollLeft, "Scroll from right to left");
            this.pictureBoxScrollLeft.Click += new EventHandler(this.pictureBoxScrollLeft_Click);
            this.pictureBoxScrollRight.Image = (Image) manager.GetObject("pictureBoxScrollRight.Image");
            this.pictureBoxScrollRight.Location = new Point(0xb3, 0x39);
            this.pictureBoxScrollRight.Name = "pictureBoxScrollRight";
            this.pictureBoxScrollRight.Size = new Size(0x18, 0x18);
            this.pictureBoxScrollRight.TabIndex = 3;
            this.pictureBoxScrollRight.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxScrollRight, "Scroll from left to right");
            this.pictureBoxScrollRight.Click += new EventHandler(this.pictureBoxScrollRight_Click);
            this.pictureBoxSample.Location = new Point(0x4d, 0x35);
            this.pictureBoxSample.Name = "pictureBoxSample";
            this.pictureBoxSample.Size = new Size(0x60, 0x20);
            this.pictureBoxSample.TabIndex = 4;
            this.pictureBoxSample.TabStop = false;
            this.pictureBoxSample.Paint += new PaintEventHandler(this.pictureBoxSample_Paint);
            this.groupBox1.Controls.Add(this.textBoxPositionValue);
            this.groupBox1.Controls.Add(this.radioButtonPositionPercent);
            this.groupBox1.Controls.Add(this.radioButtonPositionPixel);
            this.groupBox1.Controls.Add(this.radioButtonPositionBottom);
            this.groupBox1.Controls.Add(this.radioButtonPositionTop);
            this.groupBox1.Location = new Point(12, 0xcd);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xe2, 0x7e);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            this.textBoxPositionValue.Enabled = false;
            this.textBoxPositionValue.Location = new Point(0xa7, 0x52);
            this.textBoxPositionValue.Name = "textBoxPositionValue";
            this.textBoxPositionValue.Size = new Size(0x29, 20);
            this.textBoxPositionValue.TabIndex = 4;
            this.textBoxPositionValue.Leave += new EventHandler(this.textBoxPositionValue_Leave);
            this.radioButtonPositionPercent.AutoSize = true;
            this.radioButtonPositionPercent.Location = new Point(20, 0x5f);
            this.radioButtonPositionPercent.Name = "radioButtonPositionPercent";
            this.radioButtonPositionPercent.Size = new Size(0x80, 0x11);
            this.radioButtonPositionPercent.TabIndex = 3;
            this.radioButtonPositionPercent.Text = "Percent vertical offset";
            this.radioButtonPositionPercent.UseVisualStyleBackColor = true;
            this.radioButtonPositionPercent.CheckedChanged += new EventHandler(this.radioButtonPositionPercent_CheckedChanged);
            this.radioButtonPositionPixel.AutoSize = true;
            this.radioButtonPositionPixel.Location = new Point(20, 0x48);
            this.radioButtonPositionPixel.Name = "radioButtonPositionPixel";
            this.radioButtonPositionPixel.Size = new Size(0x71, 0x11);
            this.radioButtonPositionPixel.TabIndex = 2;
            this.radioButtonPositionPixel.Text = "Pixel vertical offset";
            this.radioButtonPositionPixel.UseVisualStyleBackColor = true;
            this.radioButtonPositionPixel.CheckedChanged += new EventHandler(this.radioButtonPositionPixel_CheckedChanged);
            this.radioButtonPositionBottom.AutoSize = true;
            this.radioButtonPositionBottom.Location = new Point(20, 0x31);
            this.radioButtonPositionBottom.Name = "radioButtonPositionBottom";
            this.radioButtonPositionBottom.Size = new Size(0x3a, 0x11);
            this.radioButtonPositionBottom.TabIndex = 1;
            this.radioButtonPositionBottom.Text = "Bottom";
            this.radioButtonPositionBottom.UseVisualStyleBackColor = true;
            this.radioButtonPositionBottom.CheckedChanged += new EventHandler(this.radioButtonPositionBottom_CheckedChanged);
            this.radioButtonPositionTop.AutoSize = true;
            this.radioButtonPositionTop.Checked = true;
            this.radioButtonPositionTop.Location = new Point(20, 0x1a);
            this.radioButtonPositionTop.Name = "radioButtonPositionTop";
            this.radioButtonPositionTop.Size = new Size(0x2c, 0x11);
            this.radioButtonPositionTop.TabIndex = 0;
            this.radioButtonPositionTop.TabStop = true;
            this.radioButtonPositionTop.Text = "Top";
            this.radioButtonPositionTop.UseVisualStyleBackColor = true;
            this.radioButtonPositionTop.CheckedChanged += new EventHandler(this.radioButtonPositionTop_CheckedChanged);
            this.groupBox2.Controls.Add(this.radioButtonScrollOnOff);
            this.groupBox2.Controls.Add(this.radioButtonScrollOn);
            this.groupBox2.Location = new Point(12, 0x15b);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xe2, 0x48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "How to scroll";
            this.radioButtonScrollOnOff.AutoSize = true;
            this.radioButtonScrollOnOff.Checked = true;
            this.radioButtonScrollOnOff.Location = new Point(20, 0x13);
            this.radioButtonScrollOnOff.Name = "radioButtonScrollOnOff";
            this.radioButtonScrollOnOff.Size = new Size(0x66, 0x11);
            this.radioButtonScrollOnOff.TabIndex = 0;
            this.radioButtonScrollOnOff.TabStop = true;
            this.radioButtonScrollOnOff.Text = "Scroll on and off";
            this.radioButtonScrollOnOff.UseVisualStyleBackColor = true;
            this.radioButtonScrollOnOff.CheckedChanged += new EventHandler(this.RespondToChange);
            this.radioButtonScrollOn.AutoSize = true;
            this.radioButtonScrollOn.Location = new Point(20, 0x2a);
            this.radioButtonScrollOn.Name = "radioButtonScrollOn";
            this.radioButtonScrollOn.Size = new Size(0x6d, 0x11);
            this.radioButtonScrollOn.TabIndex = 1;
            this.radioButtonScrollOn.Text = "Scroll and stay on";
            this.radioButtonScrollOn.UseVisualStyleBackColor = true;
            this.radioButtonScrollOn.CheckedChanged += new EventHandler(this.RespondToChange);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x52, 0x1b2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xa3, 0x1b2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.timer.Interval = 50;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.pictureBoxImageSmall.Image = (Image) manager.GetObject("pictureBoxImageSmall.Image");
            this.pictureBoxImageSmall.Location = new Point(0x1f, 0x17);
            this.pictureBoxImageSmall.Name = "pictureBoxImageSmall";
            this.pictureBoxImageSmall.Size = new Size(0x4c, 14);
            this.pictureBoxImageSmall.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxImageSmall.TabIndex = 9;
            this.pictureBoxImageSmall.TabStop = false;
            this.pictureBoxImageSmall.Visible = false;
            this.pictureBoxImageLarge.Image = (Image) manager.GetObject("pictureBoxImageLarge.Image");
            this.pictureBoxImageLarge.Location = new Point(0x1f, 0x5b);
            this.pictureBoxImageLarge.Name = "pictureBoxImageLarge";
            this.pictureBoxImageLarge.Size = new Size(0x4c, 0x1c);
            this.pictureBoxImageLarge.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxImageLarge.TabIndex = 10;
            this.pictureBoxImageLarge.TabStop = false;
            this.pictureBoxImageLarge.Visible = false;
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.numericUpDownTextHeight);
            this.groupBox3.Location = new Point(12, 0x8d);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xe1, 0x35);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Text height";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x77, 0x17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "pixels";
            this.numericUpDownTextHeight.Location = new Point(0x48, 0x15);
            int[] bits = new int[4];
            bits[0] = 8;
            this.numericUpDownTextHeight.Minimum = new decimal(bits);
            this.numericUpDownTextHeight.Name = "numericUpDownTextHeight";
            this.numericUpDownTextHeight.Size = new Size(0x29, 20);
            this.numericUpDownTextHeight.TabIndex = 0;
            bits = new int[4];
            bits[0] = 8;
            this.numericUpDownTextHeight.Value = new decimal(bits);
            this.errorProvider.ContainerControl = this;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(250, 0x1d5);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.pictureBoxImageLarge);
            base.Controls.Add(this.pictureBoxImageSmall);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.pictureBoxSample);
            base.Controls.Add(this.pictureBoxScrollRight);
            base.Controls.Add(this.pictureBoxScrollLeft);
            base.Controls.Add(this.pictureBoxScrollDown);
            base.Controls.Add(this.pictureBoxScrollUp);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AutoTextOptionsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Auto Text Options";
            base.FormClosing += new FormClosingEventHandler(this.AutoTextOptionsDialog_FormClosing);
            base.Load += new EventHandler(this.AutoTextOptionsDialog_Load);
            ((ISupportInitialize) this.pictureBoxScrollUp).EndInit();
            ((ISupportInitialize) this.pictureBoxScrollDown).EndInit();
            ((ISupportInitialize) this.pictureBoxScrollLeft).EndInit();
            ((ISupportInitialize) this.pictureBoxScrollRight).EndInit();
            ((ISupportInitialize) this.pictureBoxSample).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((ISupportInitialize) this.pictureBoxImageSmall).EndInit();
            ((ISupportInitialize) this.pictureBoxImageLarge).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.numericUpDownTextHeight.EndInit();
            ((ISupportInitialize) this.errorProvider).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void pictureBoxSample_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, this.pictureBoxSample.ClientRectangle);
            e.Graphics.DrawImage(this.m_image, this.m_imageLocation);
        }

        private void pictureBoxScrollDown_Click(object sender, EventArgs e)
        {
            this.SetScroll(ScrollDirection.Down);
        }

        private void pictureBoxScrollLeft_Click(object sender, EventArgs e)
        {
            this.SetScroll(ScrollDirection.Left);
        }

        private void pictureBoxScrollRight_Click(object sender, EventArgs e)
        {
            this.SetScroll(ScrollDirection.Right);
        }

        private void pictureBoxScrollUp_Click(object sender, EventArgs e)
        {
            this.SetScroll(ScrollDirection.Up);
        }

        private void radioButtonPositionBottom_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPositionValue.Enabled = false;
            this.errorProvider.SetError(this.textBoxPositionValue, string.Empty);
        }

        private void radioButtonPositionPercent_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPositionValue.Enabled = true;
            this.textBoxPositionValue.SelectAll();
            base.ActiveControl = this.textBoxPositionValue;
            if (this.textBoxPositionValue.Text.Length > 0)
            {
                this.ValidatePositionValue();
            }
        }

        private void radioButtonPositionPixel_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPositionValue.Enabled = true;
            this.textBoxPositionValue.SelectAll();
            base.ActiveControl = this.textBoxPositionValue;
            if (this.textBoxPositionValue.Text.Length > 0)
            {
                this.ValidatePositionValue();
            }
        }

        private void radioButtonPositionTop_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPositionValue.Enabled = false;
            this.errorProvider.SetError(this.textBoxPositionValue, string.Empty);
        }

        private void ResetAnimation()
        {
            this.m_image = this.pictureBoxImageLarge.Image;
            switch (this.m_textScrollDirection)
            {
                case ScrollDirection.Left:
                    if (!this.radioButtonScrollOn.Checked)
                    {
                        this.m_scrollRegion.X = -this.m_image.Width;
                        this.m_scrollRegion.Width = (this.pictureBoxSample.Width * 2) + this.m_image.Width;
                    }
                    else
                    {
                        this.m_scrollRegion.X = 0;
                        this.m_scrollRegion.Width = this.pictureBoxSample.Width + this.m_image.Width;
                    }
                    this.m_scrollRegion.Y = 0;
                    this.m_scrollRegion.Height = this.m_image.Height;
                    this.StartAnimation(new Point(this.m_scrollRegion.Right - this.m_image.Width, this.m_scrollRegion.Y), new Point(-2, 0));
                    goto Label_032D;

                case ScrollDirection.Right:
                    this.m_scrollRegion.X = -this.m_image.Width;
                    if (!this.radioButtonScrollOn.Checked)
                    {
                        this.m_scrollRegion.Width = (this.m_image.Width * 2) + this.pictureBoxSample.Width;
                        break;
                    }
                    this.m_scrollRegion.Width = this.m_image.Width * 2;
                    break;

                case ScrollDirection.Up:
                    this.m_scrollRegion.X = 0;
                    if (!this.radioButtonScrollOn.Checked)
                    {
                        this.m_scrollRegion.Y = -this.m_image.Height;
                        this.m_scrollRegion.Height = (this.m_image.Height * 2) + this.pictureBoxSample.Height;
                    }
                    else
                    {
                        this.m_scrollRegion.Y = 0;
                        this.m_scrollRegion.Height = this.pictureBoxSample.Height + this.m_image.Height;
                    }
                    this.StartAnimation(new Point(this.m_scrollRegion.X, this.m_scrollRegion.Bottom - this.m_image.Height), new Point(0, -2));
                    goto Label_032D;

                case ScrollDirection.Down:
                    this.m_scrollRegion.X = 0;
                    this.m_scrollRegion.Y = -this.m_image.Height;
                    if (!this.radioButtonScrollOn.Checked)
                    {
                        this.m_scrollRegion.Height = (this.m_image.Height * 2) + this.pictureBoxSample.Height;
                    }
                    else
                    {
                        this.m_scrollRegion.Height = this.m_image.Height * 2;
                    }
                    this.StartAnimation(this.m_scrollRegion.Location, new Point(0, 2));
                    goto Label_032D;

                default:
                    goto Label_032D;
            }
            this.m_scrollRegion.Y = 0;
            this.m_scrollRegion.Height = this.m_image.Height;
            this.StartAnimation(this.m_scrollRegion.Location, new Point(2, 0));
        Label_032D:
            if (this.m_scrollRegion.Width == 0)
            {
                this.m_scrollRegion.Width = this.m_image.Width;
            }
        }

        private void RespondToChange(object sender, EventArgs e)
        {
            if (((RadioButton) sender).Checked)
            {
                this.ResetAnimation();
            }
        }

        private void SetExtent(ScrollExtent extent)
        {
            switch (extent)
            {
                case ScrollExtent.On:
                    this.radioButtonScrollOn.Checked = true;
                    break;

                case ScrollExtent.OnAndOff:
                    this.radioButtonScrollOnOff.Checked = true;
                    break;
            }
        }

        private void SetPosition(VertPosition position, int positionValue)
        {
            switch (position)
            {
                case VertPosition.Top:
                    this.textBoxPositionValue.Enabled = false;
                    this.radioButtonPositionTop.Checked = true;
                    break;

                case VertPosition.Bottom:
                    this.textBoxPositionValue.Enabled = false;
                    this.radioButtonPositionBottom.Checked = true;
                    break;

                case VertPosition.Pixel:
                    this.textBoxPositionValue.Text = positionValue.ToString();
                    this.textBoxPositionValue.Enabled = true;
                    this.radioButtonPositionPixel.Checked = true;
                    break;

                case VertPosition.Percent:
                    this.textBoxPositionValue.Text = positionValue.ToString();
                    this.textBoxPositionValue.Enabled = true;
                    this.radioButtonPositionPercent.Checked = true;
                    break;
            }
        }

        private void SetScroll(ScrollDirection direction)
        {
            this.m_textScrollDirection = direction;
            this.ResetAnimation();
        }

        private void StartAnimation(Point start, Point delta)
        {
            this.m_startPoint = start;
            this.m_frameDelta = delta;
            this.m_pauseCountdown = this.m_pause;
            this.m_imageLocation = this.m_startPoint;
            this.pictureBoxSample.Refresh();
        }

        private void textBoxPositionValue_Leave(object sender, EventArgs e)
        {
            this.ValidatePositionValue();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(this.m_imageLocation.X, this.m_imageLocation.Y, this.m_image.Width, this.m_image.Height);
            if (this.m_scrollRegion.Contains(rect))
            {
                this.m_imageLocation.Offset(this.m_frameDelta);
                this.pictureBoxSample.Refresh();
            }
            else if (this.m_pauseCountdown == 0)
            {
                this.StartAnimation(this.m_startPoint, this.m_frameDelta);
            }
            else
            {
                this.m_pauseCountdown--;
            }
        }

        private bool ValidatePositionValue()
        {
            if (this.radioButtonPositionBottom.Checked || this.radioButtonPositionTop.Checked)
            {
                return true;
            }
            bool flag = false;
            if (!int.TryParse(this.textBoxPositionValue.Text, out this.m_textPositionValue))
            {
                base.ActiveControl = this.textBoxPositionValue;
                this.textBoxPositionValue.SelectAll();
                this.errorProvider.SetError(this.textBoxPositionValue, "Position value must be a valid number.");
                flag = true;
            }
            if (this.radioButtonPositionPercent.Checked)
            {
                if ((this.m_textPositionValue < 0) || (this.m_textPositionValue > 100))
                {
                    this.m_textPositionValue = 0;
                    base.ActiveControl = this.textBoxPositionValue;
                    this.textBoxPositionValue.SelectAll();
                    this.errorProvider.SetError(this.textBoxPositionValue, "Text position's percent value must be between 0 and 100.");
                    flag = true;
                }
            }
            else if (this.radioButtonPositionPixel.Checked && (this.m_textPositionValue > this.m_generator.BoardPixelHeight))
            {
                this.m_textPositionValue = 0;
                base.ActiveControl = this.textBoxPositionValue;
                this.textBoxPositionValue.SelectAll();
                this.errorProvider.SetError(this.textBoxPositionValue, "Text position's value will put the text completely off of the board.");
                flag = true;
            }
            if (!flag)
            {
                this.errorProvider.SetError(this.textBoxPositionValue, string.Empty);
            }
            return !flag;
        }
    }
}

