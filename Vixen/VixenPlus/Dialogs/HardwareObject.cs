using System.IO;
using VixenControls;

namespace Vixen.Dialogs
{
	internal class HardwareObject : ToolboxItem
	{
		private VectorImage.Image m_vectorImage;

		public HardwareObject(string fileName)
		{
			base.Name = Path.GetFileNameWithoutExtension(fileName);
			m_vectorImage = VectorImage.Image.FromFile(fileName);
		}
	}
}