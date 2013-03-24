using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Vixen
{
	internal class VixenSimpleButton : PictureBox, IDisposable
	{
		private readonly SolidBrush m_brush = new SolidBrush(Color.Black);
		private readonly Color m_disabledColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
		private readonly Color m_enabledColor = Color.FromArgb(0x80, 0x80, 0xff);
		private readonly Font m_font = new Font("Arial", 13f, FontStyle.Bold);
		private readonly Color m_hoverColor = Color.FromArgb(80, 80, 0xff);
		private readonly Pen m_pen = new Pen(Color.Black, 2f);
		private readonly VixenSimpleButtonType m_type;
		private bool m_hovered;

		public VixenSimpleButton(VixenSimpleButtonType type)
		{
			m_type = type;
			base.Size = new Size(20, 20);
		}

		void IDisposable.Dispose()
		{
			m_brush.Dispose();
			m_font.Dispose();
			m_pen.Dispose();
			GC.SuppressFinalize(this);
		}

		~VixenSimpleButton()
		{
			base.Dispose();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			m_hovered = true;
			Refresh();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			m_hovered = false;
			Refresh();
			base.OnMouseLeave(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			Graphics graphics = pe.Graphics;
			Color color = base.Enabled ? (m_hovered ? m_hoverColor : m_enabledColor) : m_disabledColor;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			m_pen.Color = color;
			m_brush.Color = color;
			graphics.FillEllipse(Brushes.White, clientRectangle);
			graphics.DrawEllipse(m_pen, clientRectangle);
			switch (m_type)
			{
				case VixenSimpleButtonType.Add:
					graphics.DrawString("+", m_font, m_brush, 3f, 1f);
					break;

				case VixenSimpleButtonType.Edit:
					graphics.DrawLine(m_pen, 6, 11, 11, 6);
					graphics.DrawLine(m_pen, 9, 14, 14, 9);
					break;

				case VixenSimpleButtonType.Remove:
					graphics.DrawString("-", m_font, m_brush, 3f, 1f);
					break;
			}
		}
	}
}