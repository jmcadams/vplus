using System.Windows.Forms;

namespace VixenPlus {
    public interface ISetup
    {
        Control Setup();
        void GetSetup();
        void CloseSetup();
        bool SupportsLiveSetup();
    }
}