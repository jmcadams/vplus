namespace VixenControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    public class VectorImage
    {
        public class Ellipse : VectorImage.Rectangle
        {
            public Ellipse(Color color, int x, int y, int width, int height) : base(color, x, y, width, height)
            {
                base.Type = VectorImage.PrimitiveType.Ellipse;
            }

            public static VectorImageElement LoadEllipseFromXml(XmlNode contextNode)
            {
                string[] strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new char[] { ',' });
                return new VectorImage.Ellipse(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(contextNode.SelectSingleNode("Width").InnerText), int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class FilledEllipse : VectorImage.Rectangle
        {
            public FilledEllipse(Color color, int x, int y, int width, int height) : base(color, x, y, width, height)
            {
                base.Type = VectorImage.PrimitiveType.FilledEllipse;
            }

            public static VectorImageElement LoadFilledEllipseFromXml(XmlNode contextNode)
            {
                string[] strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new char[] { ',' });
                return new VectorImage.FilledEllipse(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(contextNode.SelectSingleNode("Width").InnerText), int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class FilledRectangle : VectorImage.Rectangle
        {
            public FilledRectangle(Color color, int x, int y, int width, int height) : base(color, x, y, width, height)
            {
                base.Type = VectorImage.PrimitiveType.FilledRectangle;
            }

            public static VectorImageElement LoadRectangleFromXml(XmlNode contextNode)
            {
                string[] strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new char[] { ',' });
                return new VectorImage.FilledRectangle(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(contextNode.SelectSingleNode("Width").InnerText), int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class Image
        {
            internal Point Location;
            private List<VectorImageElement> m_elements = new List<VectorImageElement>();
            private System.Drawing.Size m_originalSize;
            private System.Drawing.Size m_size;

            public Image(int width, int height)
            {
                this.m_originalSize = new System.Drawing.Size(width, height);
                this.m_size = new System.Drawing.Size(width, height);
                this.Location = new Point();
            }

            public static VectorImage.Image FromFile(string filePath)
            {
                if (!File.Exists(filePath))
                {
                    return null;
                }
                XmlDocument document = new XmlDocument();
                document.Load(filePath);
                XmlNode node = document.SelectSingleNode("Image");
                VectorImage.Image image = new VectorImage.Image(int.Parse(node.Attributes["width"].Value), int.Parse(node.Attributes["height"].Value));
                VectorImageElement item = null;
                foreach (XmlNode node2 in node.SelectNodes("Elements/Element"))
                {
                    switch (((VectorImage.PrimitiveType) Enum.Parse(typeof(VectorImage.PrimitiveType), node2.Attributes["type"].Value)))
                    {
                        case VectorImage.PrimitiveType.Line:
                            item = VectorImage.Line.LoadFromXml(node2);
                            break;

                        case VectorImage.PrimitiveType.Rectangle:
                            item = VectorImage.Rectangle.LoadFromXml(node2);
                            break;

                        case VectorImage.PrimitiveType.FilledRectangle:
                            item = VectorImage.FilledRectangle.LoadRectangleFromXml(node2);
                            break;

                        case VectorImage.PrimitiveType.Ellipse:
                            item = VectorImage.Ellipse.LoadEllipseFromXml(node2);
                            break;

                        case VectorImage.PrimitiveType.FilledEllipse:
                            item = VectorImage.FilledEllipse.LoadFilledEllipseFromXml(node2);
                            break;
                    }
                    item.Color = Color.FromArgb(int.Parse(node2.Attributes["color"].Value));
                    image.Elements.Add(item);
                }
                return image;
            }

            public void ScaleTo(float scale)
            {
                this.m_size.Width = (int) (scale * this.m_originalSize.Width);
                this.m_size.Height = (int) (scale * this.m_originalSize.Height);
                foreach (VectorImageElement element in this.m_elements)
                {
                    element.ScaleTo(scale);
                }
            }

            public List<VectorImageElement> Elements
            {
                get
                {
                    return this.m_elements;
                }
            }

            public VectorImageElement this[int index]
            {
                get
                {
                    return this.m_elements[index];
                }
                set
                {
                    this.m_elements[index] = value;
                }
            }

            public System.Drawing.Size OriginalSize
            {
                get
                {
                    return this.m_originalSize;
                }
            }

            public System.Drawing.Size Size
            {
                get
                {
                    return this.m_size;
                }
            }
        }

        public class Line : VectorImageElement
        {
            private int m_originalX;
            private int m_originalX2;
            private int m_originalY;
            private int m_originalY2;
            public int X;
            public int X2;
            public int Y;
            public int Y2;

            public Line(Color color, int x, int y, int x2, int y2) : base(VectorImage.PrimitiveType.Line, color)
            {
                this.m_originalX = this.X = x;
                this.m_originalY = this.Y = y;
                this.m_originalX2 = this.X2 = x2;
                this.m_originalY2 = this.Y2 = y2;
            }

            public static VectorImageElement LoadFromXml(XmlNode contextNode)
            {
                string[] strArray = contextNode.SelectSingleNode("Point1").InnerText.Split(new char[] { ',' });
                string[] strArray2 = contextNode.SelectSingleNode("Point2").InnerText.Split(new char[] { ',' });
                return new VectorImage.Line(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray2[0]), int.Parse(strArray2[1]));
            }

            public override void SaveToXml(XmlNode contextNode)
            {
                Xml.SetValue(contextNode, "Point1", string.Format("{0},{1}", this.m_originalX, this.m_originalY));
                Xml.SetValue(contextNode, "Point2", string.Format("{0},{1}", this.m_originalX2, this.m_originalY2));
            }

            public override void ScaleTo(float scale)
            {
                this.X = (int) (this.m_originalX * scale);
                this.Y = (int) (this.m_originalY * scale);
                this.X2 = (int) (this.m_originalX2 * scale);
                this.Y2 = (int) (this.m_originalY2 * scale);
            }

            public override int Height
            {
                get
                {
                    return this.Y2;
                }
            }

            public override int OriginalHeight
            {
                get
                {
                    return this.m_originalY2;
                }
            }

            public override int OriginalWidth
            {
                get
                {
                    return this.m_originalX2;
                }
            }

            public override int Width
            {
                get
                {
                    return this.X2;
                }
            }
        }

        public enum PrimitiveType
        {
            Line,
            Rectangle,
            FilledRectangle,
            Ellipse,
            FilledEllipse
        }

        public class Rectangle : VectorImageElement
        {
            public int _Height;
            public int _Width;
            private int m_originalHeight;
            private int m_originalWidth;
            private int m_originalX;
            private int m_originalY;
            public int X;
            public int Y;

            public Rectangle(Color color, int x, int y, int width, int height) : base(VectorImage.PrimitiveType.Rectangle, color)
            {
                this.m_originalX = this.X = x;
                this.m_originalY = this.Y = y;
                this.m_originalWidth = this._Width = width;
                this.m_originalHeight = this._Height = height;
            }

            public static VectorImageElement LoadFromXml(XmlNode contextNode)
            {
                string[] strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new char[] { ',' });
                return new VectorImage.Rectangle(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(contextNode.SelectSingleNode("Width").InnerText), int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }

            public override void SaveToXml(XmlNode contextNode)
            {
                Xml.SetValue(contextNode, "Location", string.Format("{0},{1}", this.m_originalX, this.m_originalY));
                Xml.SetValue(contextNode, "Width", this.m_originalWidth.ToString());
                Xml.SetValue(contextNode, "Height", this.m_originalHeight.ToString());
            }

            public override void ScaleTo(float scale)
            {
                this.X = (int) (this.m_originalX * scale);
                this.Y = (int) (this.m_originalY * scale);
                this._Width = (int) (this.m_originalWidth * scale);
                this._Height = (int) (this.m_originalHeight * scale);
            }

            public override int Height
            {
                get
                {
                    return this._Height;
                }
            }

            public override int OriginalHeight
            {
                get
                {
                    return this.m_originalHeight;
                }
            }

            public override int OriginalWidth
            {
                get
                {
                    return this.m_originalWidth;
                }
            }

            public override int Width
            {
                get
                {
                    return this._Width;
                }
            }
        }
    }
}

