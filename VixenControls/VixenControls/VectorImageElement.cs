namespace VixenControls
{
    using System;
    using System.Drawing;
    using System.Xml;

    public abstract class VectorImageElement
    {
        public System.Drawing.Color Color;
        public VectorImage.PrimitiveType Type;

        public VectorImageElement(VectorImage.PrimitiveType type, System.Drawing.Color color)
        {
            this.Type = type;
            this.Color = color;
        }

        public abstract void SaveToXml(XmlNode contextNode);
        public abstract void ScaleTo(float scale);

        public abstract int Height { get; }

        public abstract int OriginalHeight { get; }

        public abstract int OriginalWidth { get; }

        public abstract int Width { get; }
    }
}

