using System.Windows.Forms;

namespace VixenPlus
{
	internal interface IUIPlugIn : VixenMDI, IPlugIn
	{
		string FileExtension { get; }

		string FileTypeDescription { get; }

		bool IsDirty { get; set; }

		Form MdiParent { get; set; }
		EventSequence New();
		EventSequence New(EventSequence seedSequence);
		EventSequence Open(string filePath);
		DialogResult RunWizard(ref EventSequence resultSequence);
		void SaveTo(string filePath);
		void Show();
	}
}