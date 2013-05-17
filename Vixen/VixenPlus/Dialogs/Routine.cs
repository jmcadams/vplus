using System;
using System.Drawing;
using System.IO;

namespace VixenPlus.Dialogs {
    internal class Routine : IDisposable {
        public Routine(string filePath) {
            FilePath = filePath;
            if (!File.Exists(filePath)) {
                return;
            }

            Name = Path.GetFileNameWithoutExtension(filePath);
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
                    foreach (var pixels in row.Split(new[] {' '})) {
                        if (pixels.Length > 0) {
                            //todo add the routine color to preferences.
                            Preview.SetPixel(x++, y, Color.FromArgb(Convert.ToByte(pixels), Color.LightBlue));
                        }
                    }
                    y++;
                }
            }
            var world = GraphicsUnit.World;
            var bounds = Preview.GetBounds(ref world);
            PreviewBounds = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
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
