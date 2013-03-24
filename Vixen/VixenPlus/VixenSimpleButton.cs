using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VixenPlus
{
	internal class VixenSimpleButton : PictureBox, IDisposable
	{
		private readonly SolidBrush _brush = new SolidBrush(Color.Black);
		private readonly Color _disabledColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
		private readonly Color _enabledColor = Color.FromArgb(0x80, 0x80, 0xff);
		private readonly Font _font = new Font("Arial", 13f, FontStyle.Bold);
		private readonly Color _hoverColor = Color.FromArgb(80, 80, 0xff);
		private readonly Pen _pen = new Pen(Color.Black, 2f);
		private readonly VixenSimpleButtonType _buttonType;
		private bool _isHovered;

		public VixenSimpleButton(VixenSimpleButtonType type)
		{
			_buttonType = type;
			Size = new Size(20, 20);
		}

		void IDisposable.Dispose()
		{
			_brush.Dispose();
			_font.Dispose();
			_pen.Dispose();
			GC.SuppressFinalize(this);
		}

		~VixenSimpleButton()
		{
			Dispose();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			_isHovered = true;
			Refresh();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_isHovered = false;
			Refresh();
			base.OnMouseLeave(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			Graphics graphics = pe.Graphics;
			Color color = Enabled ? (_isHovered ? _hoverColor : _enabledColor) : _disabledColor;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle clientRectangle = ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			_pen.Color = color;
			_brush.Color = color;
			graphics.FillEllipse(Brushes.White, clientRectangle);
			graphics.DrawEllipse(_pen, clientRectangle);
			switch (_buttonType)
			{
				case VixenSimpleButtonType.Add:
					graphics.DrawString("+", _font, _brush, 3f, 1f);
					break;

				case VixenSimpleButtonType.Edit:
					graphics.DrawLine(_pen, 6, 11, 11, 6);
					graphics.DrawLine(_pen, 9, 14, 14, 9);
					break;

				case VixenSimpleButtonType.Remove:
					graphics.DrawString("-", _font, _brush, 3f, 1f);
					break;
			}
		}
	}
}