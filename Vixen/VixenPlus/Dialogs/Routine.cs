namespace Vixen.Dialogs
{
    using System;
    using System.Drawing;
    using System.IO;

    internal class Routine : IDisposable
    {
        private string m_filePath;
        private string m_name;
        private Bitmap m_preview = null;
        private Rectangle m_previewBounds;

        public Routine(string filePath)
        {
            int width = 0;
            int height = 0;
            this.m_filePath = filePath;
            if (File.Exists(filePath))
            {
                string str;
                this.m_name = Path.GetFileNameWithoutExtension(filePath);
                FileStream stream = new FileStream(filePath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                width = reader.ReadLine().Split(new char[] { ' ' }).Length - 1;
                height++;
                while (reader.ReadLine() != null)
                {
                    height++;
                }
                stream.Seek(0L, SeekOrigin.Begin);
                int y = 0;
                this.m_preview = new Bitmap(width, height);
                while ((str = reader.ReadLine()) != null)
                {
                    int num4 = 0;
                    foreach (string str2 in str.Split(new char[] { ' ' }))
                    {
                        if (str2.Length > 0)
                        {
                            this.m_preview.SetPixel(num4++, y, Color.FromArgb(Convert.ToByte(str2), Color.LightBlue));
                        }
                    }
                    y++;
                }
                reader.Close();
                reader.Dispose();
                stream.Dispose();
                GraphicsUnit world = GraphicsUnit.World;
                RectangleF bounds = this.m_preview.GetBounds(ref world);
                this.m_previewBounds = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
            }
        }

        public void Dispose()
        {
            if (this.m_preview != null)
            {
                this.m_preview.Dispose();
                this.m_preview = null;
            }
            GC.SuppressFinalize(this);
        }

        ~Routine()
        {
            this.Dispose();
        }

        public string FilePath
        {
            get
            {
                return this.m_filePath;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public Bitmap Preview
        {
            get
            {
                return this.m_preview;
            }
        }

        public Rectangle PreviewBounds
        {
            get
            {
                return this.m_previewBounds;
            }
        }
    }
}

