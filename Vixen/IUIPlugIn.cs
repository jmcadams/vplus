using System.Windows.Forms;

using VixenPlusCommon.Annotations;

namespace VixenPlus {
    internal interface IUIPlugIn : IVixenMDI, IPlugIn
    {
        //string FileExtension { get; }
        //string FileTypeDescription { get; }
        bool IsDirty { get;
            [UsedImplicitly]
            set; }
        Form MdiParent {set; }
        EventSequence New(); 
        EventSequence New(EventSequence seedSequence);
        //EventSequence Open(string filePath);
        DialogResult RunWizard(ref EventSequence resultSequence);
        void SaveTo();
        void Show();
    }
}