using System.Windows.Forms;

namespace VixenPlus {
    public interface ISetup
    {
        Control Setup();
        SetupData GetSetup();
        void CloseSetup();
        bool SupportsPreview();
        bool ValidateSettings();
    }
}