using System.Drawing.Drawing2D;

namespace CommonControls {
    public class VectorListItem {
        public GraphicsPath BorderPath;
        public VectorImage.Image Image;
        public System.Drawing.Rectangle ListItemBounds;


        public VectorListItem(VectorImage.Image image) {
            Image = image;
            BorderPath = null;
            ListItemBounds = new System.Drawing.Rectangle();
        }
    }
}
