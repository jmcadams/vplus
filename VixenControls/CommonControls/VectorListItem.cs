namespace VixenControls
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class VectorListItem
    {
        public GraphicsPath BorderPath;
        public VixenControls.VectorImage.Image Image;
        public System.Drawing.Rectangle ListItemBounds;

        public VectorListItem(VixenControls.VectorImage.Image image)
        {
            this.Image = image;
            this.BorderPath = null;
            this.ListItemBounds = new System.Drawing.Rectangle();
        }
    }
}

