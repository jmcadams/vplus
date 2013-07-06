using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace VixenPlus.Dialogs {
    internal class Routine : IDisposable {

        public const int DefaultHeight = 80;
        public const int DefaultWidth = 150;
        private readonly Color _routineColor;

        public Routine(string filePath) {
            FilePath = filePath;
            if (!File.Exists(filePath)) {
                return;
            }

            Name = Path.GetFileNameWithoutExtension(filePath);
            _routineColor = Color.FromArgb(Int32.Parse(Preference2.GetInstance().GetString("RoutineBitmap"))); 
            using (var stream = new FileStream(filePath, FileMode.Open))
            using (var reader = new StreamReader(stream)) {
                var line = reader.ReadLine();
                var width = 0;
                if (line != null) {
                    width = line.Split(new[] {' '}).Length - 1;
                }
                var height = 1;
                while (reader.ReadLine() != null) {
                    height++;
                }
                stream.Seek(0L, SeekOrigin.Begin);
                var y = 0;
                Preview = new Bitmap(width, height);
                string row;
                while ((row = reader.ReadLine()) != null) {
                    var x = 0;
                    foreach (var pixels in row.Split(new[] {' '}).Where(pixels => pixels.Length > 0)) {
                        Preview.SetPixel(x++, y, Color.FromArgb(Convert.ToByte(pixels), _routineColor));
                    }
                    y++;
                }
            }
            if (Preview.Width != DefaultWidth || Preview.Height != DefaultHeight) {
                Preview = ResizeImage(Preview);
            }
            PreviewBounds = new Rectangle(0, 0, DefaultWidth, DefaultHeight);
        }




        private Bitmap ResizeImage(Bitmap image) {
            var result = new Bitmap(DefaultWidth, DefaultHeight);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(result)) {
                var oldHeight = image.Height;
                var oldWidth = image.Width;
                var widthFactor = (float)DefaultWidth / (image.Width);
                var heightFactor = (float)DefaultHeight / (image.Height);

                using (var brush = new SolidBrush(_routineColor)) {
                    for (var i = 0; i < oldHeight; i++) {
                        for (var j = 0; j < oldWidth; j++) {
                            brush.Color = Color.FromArgb(image.GetPixel(j, i).ToArgb());
                            graphics.FillRectangle(brush, j * widthFactor, i * heightFactor, widthFactor, heightFactor);
                        }
                    }
                }
            }

            return result;
        }


        public string FilePath { get; private set; }

        public string Name { get; private set; }

        public Bitmap Preview { get; private set; }

        public Rectangle PreviewBounds { get; private set; }


        public void Dispose() {
            if (Preview != null) {
                Preview.Dispose();
                Preview = null;
            }
            GC.SuppressFinalize(this);
        }


        ~Routine() {
            Dispose();
        }
    }
}
