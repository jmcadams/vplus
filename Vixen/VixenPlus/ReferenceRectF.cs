using System.Drawing;

namespace VixenPlus
{
    internal class ReferenceRectF
    {
        private RectangleF _rectangleF;

        public ReferenceRectF()
        {
            _rectangleF = new RectangleF();
        }

        public ReferenceRectF(float x, float y, float width, float height)
        {
            _rectangleF = new RectangleF(x, y, width, height);
        }

        public float Bottom
        {
            get { return _rectangleF.Bottom; }
        }

        public float Height
        {
            get { return _rectangleF.Height; }
            set { _rectangleF.Height = value; }
        }

        public float Left
        {
            get { return _rectangleF.Left; }
        }

        public float Right
        {
            get { return _rectangleF.Right; }
        }

        public float Top
        {
            get { return _rectangleF.Top; }
        }

        public float Width
        {
            get { return _rectangleF.Width; }
            set { _rectangleF.Width = value; }
        }

        public float X
        {
            get { return _rectangleF.X; }
            set { _rectangleF.X = value; }
        }

        public float Y
        {
            get { return _rectangleF.Y; }
            set { _rectangleF.Y = value; }
        }

        public bool Contains(Point p)
        {
            return _rectangleF.Contains(p);
        }

        public static bool Intersects(ReferenceRectF a, ReferenceRectF b)
        {
            return ((((a.Right > b.Left) && (a.Bottom > b.Top)) && (a.Left < b.Right)) && (a.Top < b.Bottom));
        }

        public RectangleF ToRectangleF()
        {
            return _rectangleF;
        }
    }
}