using System;
using System.Drawing;
using System.IO;

namespace Vixen.Dialogs
{
	internal class Routine : IDisposable
	{
		private readonly string _filePath;
		private readonly string _name;
		private readonly Rectangle _previewBounds;
		private Bitmap _preview;

		public Routine(string filePath)
		{
			_filePath = filePath;
			if (File.Exists(filePath))
			{
				string str;
				_name = Path.GetFileNameWithoutExtension(filePath);
				var stream = new FileStream(filePath, FileMode.Open);
				var reader = new StreamReader(stream);
				int width = reader.ReadLine().Split(new[] {' '}).Length - 1;
				int height = 0;
				height++;
				while (reader.ReadLine() != null)
				{
					height++;
				}
				stream.Seek(0L, SeekOrigin.Begin);
				int y = 0;
				_preview = new Bitmap(width, height);
				while ((str = reader.ReadLine()) != null)
				{
					int num4 = 0;
					foreach (string str2 in str.Split(new[] {' '}))
					{
						if (str2.Length > 0)
						{
							_preview.SetPixel(num4++, y, Color.FromArgb(Convert.ToByte(str2), Color.LightBlue));
						}
					}
					y++;
				}
				reader.Close();
				reader.Dispose();
				stream.Dispose();
				var world = GraphicsUnit.World;
				RectangleF bounds = _preview.GetBounds(ref world);
				_previewBounds = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
			}
		}

		public string FilePath
		{
			get { return _filePath; }
		}

		public string Name
		{
			get { return _name; }
		}

		public Bitmap Preview
		{
			get { return _preview; }
		}

		public Rectangle PreviewBounds
		{
			get { return _previewBounds; }
		}

		public void Dispose()
		{
			if (_preview != null)
			{
				_preview.Dispose();
				_preview = null;
			}
			GC.SuppressFinalize(this);
		}

		~Routine()
		{
			Dispose();
		}
	}
}