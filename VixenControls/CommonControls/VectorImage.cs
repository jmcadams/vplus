using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

namespace CommonControls {
    public class VectorImage {
        public class Ellipse : Rectangle {
            public Ellipse(Color color, int x, int y, int width, int height) : base(color, x, y, width, height) {
                Type = PrimitiveType.Ellipse;
            }


            public static VectorImageElement LoadEllipseFromXml(XmlNode contextNode) {
                var strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new[] {','});
                return new Ellipse(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]),
                                   int.Parse(contextNode.SelectSingleNode("Width").InnerText),
                                   int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class FilledEllipse : Rectangle {
            public FilledEllipse(Color color, int x, int y, int width, int height) : base(color, x, y, width, height) {
                Type = PrimitiveType.FilledEllipse;
            }


            public static VectorImageElement LoadFilledEllipseFromXml(XmlNode contextNode) {
                var strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new[] {','});
                return new FilledEllipse(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]),
                                         int.Parse(contextNode.SelectSingleNode("Width").InnerText),
                                         int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class FilledRectangle : Rectangle {
            public FilledRectangle(Color color, int x, int y, int width, int height) : base(color, x, y, width, height) {
                Type = PrimitiveType.FilledRectangle;
            }


            public static VectorImageElement LoadRectangleFromXml(XmlNode contextNode) {
                var strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new[] {','});
                return new FilledRectangle(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]),
                                           int.Parse(contextNode.SelectSingleNode("Width").InnerText),
                                           int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }
        }

        public class Image {
            internal Point Location;
            private Size _size;


            public Image(int width, int height) {
                Elements = new List<VectorImageElement>();
                OriginalSize = new Size(width, height);
                _size = new Size(width, height);
                Location = new Point();
            }


            public static Image FromFile(string filePath) {
                if (!File.Exists(filePath)) {
                    return null;
                }
                var document = new XmlDocument();
                document.Load(filePath);
                var node = document.SelectSingleNode("Image");
                var image = new Image(int.Parse(node.Attributes["width"].Value), int.Parse(node.Attributes["height"].Value));
                VectorImageElement item = null;
                foreach (XmlNode node2 in node.SelectNodes("Elements/Element")) {
                    switch (((PrimitiveType) Enum.Parse(typeof (PrimitiveType), node2.Attributes["type"].Value))) {
                        case PrimitiveType.Line:
                            item = Line.LoadFromXml(node2);
                            break;

                        case PrimitiveType.Rectangle:
                            item = Rectangle.LoadFromXml(node2);
                            break;

                        case PrimitiveType.FilledRectangle:
                            item = FilledRectangle.LoadRectangleFromXml(node2);
                            break;

                        case PrimitiveType.Ellipse:
                            item = Ellipse.LoadEllipseFromXml(node2);
                            break;

                        case PrimitiveType.FilledEllipse:
                            item = FilledEllipse.LoadFilledEllipseFromXml(node2);
                            break;
                    }
                    item.Color = Color.FromArgb(int.Parse(node2.Attributes["color"].Value));
                    image.Elements.Add(item);
                }
                return image;
            }


            public void ScaleTo(float scale) {
                _size.Width = (int) (scale * OriginalSize.Width);
                _size.Height = (int) (scale * OriginalSize.Height);
                foreach (var element in Elements) {
                    element.ScaleTo(scale);
                }
            }


            public List<VectorImageElement> Elements { get; private set; }


            public VectorImageElement this[int index] {
                get { return Elements[index]; }
                set { Elements[index] = value; }
            }


            public Size OriginalSize { get; private set; }

            public Size Size {
                get { return _size; }
            }
        }

        public class Line : VectorImageElement {
            private readonly int _originalX;
            private readonly int _originalX2;
            private readonly int _originalY;
            private readonly int _originalY2;
            public int X;
            public int X2;
            public int Y;
            public int Y2;


            public Line(Color color, int x, int y, int x2, int y2) : base(PrimitiveType.Line, color) {
                _originalX = X = x;
                _originalY = Y = y;
                _originalX2 = X2 = x2;
                _originalY2 = Y2 = y2;
            }


            public static VectorImageElement LoadFromXml(XmlNode contextNode) {
                var strArray = contextNode.SelectSingleNode("Point1").InnerText.Split(new[] {','});
                var strArray2 = contextNode.SelectSingleNode("Point2").InnerText.Split(new[] {','});
                return new Line(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray2[0]), int.Parse(strArray2[1]));
            }


            public override void SaveToXml(XmlNode contextNode) {
                Xml.SetValue(contextNode, "Point1", string.Format("{0},{1}", _originalX, _originalY));
                Xml.SetValue(contextNode, "Point2", string.Format("{0},{1}", _originalX2, _originalY2));
            }


            public override void ScaleTo(float scale) {
                X = (int) (_originalX * scale);
                Y = (int) (_originalY * scale);
                X2 = (int) (_originalX2 * scale);
                Y2 = (int) (_originalY2 * scale);
            }


            public override int Height {
                get { return Y2; }
            }

            public override int OriginalHeight {
                get { return _originalY2; }
            }

            public override int OriginalWidth {
                get { return _originalX2; }
            }

            public override int Width {
                get { return X2; }
            }
        }

        public enum PrimitiveType {
            Line,
            Rectangle,
            FilledRectangle,
            Ellipse,
            FilledEllipse
        }

        public class Rectangle : VectorImageElement {
            private int _height;
            private int _width;
            private readonly int _originalHeight;
            private readonly int _originalWidth;
            private readonly int _originalX;
            private readonly int _originalY;
            public int X;
            public int Y;


            public Rectangle(Color color, int x, int y, int width, int height) : base(PrimitiveType.Rectangle, color) {
                _originalX = X = x;
                _originalY = Y = y;
                _originalWidth = _width = width;
                _originalHeight = _height = height;
            }


            public static VectorImageElement LoadFromXml(XmlNode contextNode) {
                var strArray = contextNode.SelectSingleNode("Location").InnerText.Split(new[] {','});
                return new Rectangle(Color.Black, int.Parse(strArray[0]), int.Parse(strArray[1]),
                                     int.Parse(contextNode.SelectSingleNode("Width").InnerText),
                                     int.Parse(contextNode.SelectSingleNode("Height").InnerText));
            }


            public override void SaveToXml(XmlNode contextNode) {
                Xml.SetValue(contextNode, "Location", string.Format("{0},{1}", _originalX, _originalY));
                Xml.SetValue(contextNode, "Width", _originalWidth.ToString(CultureInfo.InvariantCulture));
                Xml.SetValue(contextNode, "Height", _originalHeight.ToString(CultureInfo.InvariantCulture));
            }


            public override void ScaleTo(float scale) {
                X = (int) (_originalX * scale);
                Y = (int) (_originalY * scale);
                _width = (int) (_originalWidth * scale);
                _height = (int) (_originalHeight * scale);
            }


            public override int Height {
                get { return _height; }
            }

            public override int OriginalHeight {
                get { return _originalHeight; }
            }

            public override int OriginalWidth {
                get { return _originalWidth; }
            }

            public override int Width {
                get { return _width; }
            }
        }
    }
}
