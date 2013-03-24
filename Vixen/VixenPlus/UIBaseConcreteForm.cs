using System;
using System.Windows.Forms;

namespace Vixen
{
	internal class UIBaseConcreteForm : UIBase
	{
		public override string Author
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override string Description
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override string FileExtension
		{
			get { return string.Empty; }
		}

		public override string FileTypeDescription
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override EventSequence Sequence
		{
			get { return null; }
			set { throw new Exception("The method or operation is not implemented."); }
		}

		public override EventSequence New()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override EventSequence New(EventSequence seedSequence)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void Notify(Notification notification, object data)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override EventSequence Open(string filePath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override DialogResult RunWizard(ref EventSequence resultSequence)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void SaveTo(string filePath)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}