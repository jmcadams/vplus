namespace Vixen.Dialogs
{
    using System;
    using System.IO;
    using VixenControls;

    internal class HardwareObject : ToolboxItem
    {
        private VectorImage.Image m_vectorImage;

        public HardwareObject(string fileName)
        {
            base.Name = Path.GetFileNameWithoutExtension(fileName);
            this.m_vectorImage = VectorImage.Image.FromFile(fileName);
        }
    }
}

