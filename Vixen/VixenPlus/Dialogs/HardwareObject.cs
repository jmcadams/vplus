using System.IO;

using CommonControls;

namespace VixenPlus.Dialogs {
    //TODO I am not sure this is doing anything real
    internal class HardwareObject : ToolboxItem {
        public HardwareObject(string fileName) {
            Name = Path.GetFileNameWithoutExtension(fileName);
            VectorImage.Image.FromFile(fileName);
        }
    }
}
