using System.Drawing;

namespace Vixen
{
	internal class ReferenceRectF
	{
		private RectangleF m_rectF;

		public ReferenceRectF()
		{
			m_rectF = new RectangleF();
		}

		public ReferenceRectF(float x, float y, float width, float height)
		{
			m_rectF = new RectangleF(x, y, width, height);
		}

		public float Bottom
		{
			get { return m_rectF.Bottom; }
		}

		public float Height
		{
			get { return m_rectF.Height; }
			set { m_rectF.Height = value; }
		}

		public float Left
		{
			get { return m_rectF.Left; }
		}

		public float Right
		{
			get { return m_rectF.Right; }
		}

		public float Top
		{
			get { return m_rectF.Top; }
		}

		public float Width
		{
			get { return m_rectF.Width; }
			set { m_rectF.Width = value; }
		}

		public float X
		{
			get { return m_rectF.X; }
			set { m_rectF.X = value; }
		}

		public float Y
		{
			get { return m_rectF.Y; }
			set { m_rectF.Y = value; }
		}

		public bool Contains(Point p)
		{
			return m_rectF.Contains(p);
		}

		public static bool Intersects(ReferenceRectF a, ReferenceRectF b)
		{
			return ((((a.Right > b.Left) && (a.Bottom > b.Top)) && (a.Left < b.Right)) && (a.Top < b.Bottom));
		}

		public RectangleF ToRectangleF()
		{
			return m_rectF;
		}
	}
}