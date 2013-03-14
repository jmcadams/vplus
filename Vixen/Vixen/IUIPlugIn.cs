namespace Vixen
{
    using System;
    using System.Windows.Forms;

    internal interface IUIPlugIn : VixenMDI, IPlugIn
    {
        EventSequence New();
        EventSequence New(EventSequence seedSequence);
        EventSequence Open(string filePath);
        DialogResult RunWizard(ref EventSequence resultSequence);
        void SaveTo(string filePath);
        void Show();

        string FileExtension { get; }

        string FileTypeDescription { get; }

        bool IsDirty { get; set; }

        Form MdiParent { get; set; }
    }
}

