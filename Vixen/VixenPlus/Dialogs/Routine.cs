using System;
using System.Drawing;
using System.IO;

namespace Vixen.Dialogs
{
	internal class Routine : IDisposable
	{
		private readonly string m_filePath;
		private readonly string m_name;
		private readonly Rectangle m_previewBounds;
		private Bitmap m_preview;

		public Routine(string filePath)
		{
			int width = 0;
			int height = 0;
			m_filePath = filePath;
			if (File.Exists(filePath))
			{
				string str;
				m_name = Path.GetFileNameWithoutExtension(filePath);
				var stream = new FileStream(filePath, FileMode.Open);
				var reader = new StreamReader(stream);
				width = reader.ReadLine().Split(new[] {' '}).Length - 1;
				height++;
				while (reader.ReadLine() != null)
				{
					height++;
				}
				stream.Seek(0L, SeekOrigin.Begin);
				int y = 0;
				m_preview = new Bitmap(width, height);
				while ((str = reader.ReadLine()) != null)
				{
					int num4 = 0;
					foreach (string str2 in str.Split(new[] {' '}))
					{
						if (str2.Length > 0)
						{
							m_preview.SetPixel(num4++, y, Color.FromArgb(Convert.ToByte(str2), Color.LightBlue));
						}
					}
					y++;
				}
				reader.Close();
				reader.Dispose();
				stream.Dispose();
				var world = GraphicsUnit.World;
				RectangleF bounds = m_preview.GetBounds(ref world);
				m_previewBounds = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
			}
		}

		public string FilePath
		{
			get { return m_filePath; }
		}

		public string Name
		{
			get { return m_name; }
		}

		public Bitmap Preview
		{
			get { return m_preview; }
		}

		public Rectangle PreviewBounds
		{
			get { return m_previewBounds; }
		}

		public void Dispose()
		{
			if (m_preview != null)
			{
				m_preview.Dispose();
				m_preview = null;
			}
			GC.SuppressFinalize(this);
		}

		~Routine()
		{
			Dispose();
		}
	}
}