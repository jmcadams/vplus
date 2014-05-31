using System.Drawing;
using System.Drawing.Imaging;

namespace Preview {
    internal static class BgImage {
        internal static Image GetImage(Image originalImage, float opacity) {
            Image image = new Bitmap(originalImage);

            using (var g = Graphics.FromImage(image)) {
                using (var attributes = new ImageAttributes()) {
                    var matrix = new ColorMatrix { Matrix40 = opacity, Matrix41 = opacity, Matrix42 = opacity };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    attributes.SetColorMatrix(matrix);
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return image;
        }
    }
}
