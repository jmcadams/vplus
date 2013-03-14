namespace Vixen
{
    using System;
    using System.Drawing;

    internal class ReferenceRectF
    {
        private RectangleF m_rectF;

        public ReferenceRectF()
        {
            this.m_rectF = new RectangleF();
        }

        public ReferenceRectF(float x, float y, float width, float height)
        {
            this.m_rectF = new RectangleF(x, y, width, height);
        }

        public bool Contains(Point p)
        {
            return this.m_rectF.Contains((PointF) p);
        }

        public static bool Intersects(ReferenceRectF a, ReferenceRectF b)
        {
            return ((((a.Right > b.Left) && (a.Bottom > b.Top)) && (a.Left < b.Right)) && (a.Top < b.Bottom));
        }

        public RectangleF ToRectangleF()
        {
            return this.m_rectF;
        }

        public float Bottom
        {
            get
            {
                return this.m_rectF.Bottom;
            }
        }

        public float Height
        {
            get
            {
                return this.m_rectF.Height;
            }
            set
            {
                this.m_rectF.Height = value;
            }
        }

        public float Left
        {
            get
            {
                return this.m_rectF.Left;
            }
        }

        public float Right
        {
            get
            {
                return this.m_rectF.Right;
            }
        }

        public float Top
        {
            get
            {
                return this.m_rectF.Top;
            }
        }

        public float Width
        {
            get
            {
                return this.m_rectF.Width;
            }
            set
            {
                this.m_rectF.Width = value;
            }
        }

        public float X
        {
            get
            {
                return this.m_rectF.X;
            }
            set
            {
                this.m_rectF.X = value;
            }
        }

        public float Y
        {
            get
            {
                return this.m_rectF.Y;
            }
            set
            {
                this.m_rectF.Y = value;
            }
        }
    }
}

