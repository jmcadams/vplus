namespace LedTriks
{
	using LedTriksUtil;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class AutoTextOptionsDialog : Form
	{
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
				MessageBox.Show("Please fix the error shown before continuing.", VixenPlus.Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					if (((((this.m_textPositionValue * this.m_generator.BoardPixelHeight) / 100) + this.m_generator.TextHeight) > this.m_generator.BoardPixelHeight) && (MessageBox.Show("Text position's percent value will put the text partially off of the board.\nIs this okay?", VixenPlus.Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No))
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

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(AutoTextOptionsDialog));
		//this.pictureBoxScrollUp.Image = (Image)manager.GetObject("pictureBoxScrollUp.Image");
		//this.pictureBoxScrollDown.Image = (Image)manager.GetObject("pictureBoxScrollDown.Image");
		//this.pictureBoxScrollLeft.Image = (Image)manager.GetObject("pictureBoxScrollLeft.Image");
		//this.pictureBoxScrollRight.Image = (Image)manager.GetObject("pictureBoxScrollRight.Image");
		//this.pictureBoxImageSmall.Image = (Image)manager.GetObject("pictureBoxImageSmall.Image");
		//this.pictureBoxImageLarge.Image = (Image)manager.GetObject("pictureBoxImageLarge.Image");

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

