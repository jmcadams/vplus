using System;
using System.Windows.Forms;

namespace VixenPlus
{
//	[TypeDescriptionProvider(typeof(GeneralConcreteClassProvider)), ConcreteClass(typeof(UIBaseConcreteForm))]
	public partial class UIBase : Form, IUIPlugIn
	{
		private bool _isDirty;

		public UIBase()
		{
			InitializeComponent();
		}

		public virtual /*abstract*/ EventSequence New()
		{
			throw new NotImplementedException();
		}

		public virtual /*abstract*/ EventSequence New(EventSequence seedSequence)
		{
			throw new NotImplementedException();
		}

		public virtual /*abstract*/ void Notify(Notification notification, object data)
		{
			throw new NotImplementedException();
		}

		public virtual /*abstract*/ EventSequence Open(string filePath)
		{
			throw new NotImplementedException();
		}

		public virtual /*abstract*/ DialogResult RunWizard(ref EventSequence resultSequence)
		{
			throw new NotImplementedException();
		}

		public virtual /*abstract*/ void SaveTo(string filePath)
		{
			throw new NotImplementedException();
		}

		//string IPlugIn.get_Name()
		//{
		//    return base.Name;
		//}

		//Form IUIPlugIn.get_MdiParent()
		//{
		//    return base.MdiParent;
		//}

		//void IUIPlugIn.set_MdiParent(Form form1)
		//{
		//    base.MdiParent = form1;
		//}

		void IUIPlugIn.Show()
		{
			Show();
		}

		public virtual /*abstract*/ string Author { get; set; }

		public virtual /*abstract*/ string Description { get; set; }

		public virtual /*abstract*/ string FileExtension { get; set; }

		public virtual /*abstract*/ string FileTypeDescription { get; set; }

		public bool IsDirty
		{
			get { return _isDirty; }
			set
			{
				_isDirty = value;
				OnDirtyChanged(EventArgs.Empty);
			}
		}

		public virtual /*abstract*/ EventSequence Sequence { get; set; }
		public event EventHandler DirtyChanged;

		public virtual void OnDirtyChanged(EventArgs e)
		{
			if (DirtyChanged != null)
			{
				DirtyChanged(this, e);
			}
		}
	}
}