namespace Vixen
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Runtime.CompilerServices;
	using System.Windows.Forms;

//	[TypeDescriptionProvider(typeof(GeneralConcreteClassProvider)), ConcreteClass(typeof(UIBaseConcreteForm))]
	public partial class UIBase : Form, IUIPlugIn, VixenMDI, IPlugIn
	{
		private bool m_isDirty = false;

		public event EventHandler DirtyChanged;

		public UIBase()
		{
			this.InitializeComponent();
		}
		
		public virtual /*abstract*/ EventSequence New() { throw new NotImplementedException(); }
		public virtual /*abstract*/ EventSequence New(EventSequence seedSequence) { throw new NotImplementedException(); }
		public virtual /*abstract*/ void Notify(Notification notification, object data) { throw new NotImplementedException(); }
		public virtual void OnDirtyChanged(EventArgs e)
		{
			if (this.DirtyChanged != null)
			{
				this.DirtyChanged(this, e);
			}
		}

		public virtual /*abstract*/ EventSequence Open(string filePath) { throw new NotImplementedException(); }
		public virtual /*abstract*/ DialogResult RunWizard(ref EventSequence resultSequence) { throw new NotImplementedException(); }
		public virtual /*abstract*/ void SaveTo(string filePath) { throw new NotImplementedException(); }

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
			base.Show();
		}

		public virtual /*abstract*/ string Author { get; set; }

		public virtual /*abstract*/ string Description { get; set; }

		public virtual /*abstract*/ string FileExtension { get; set; }

		public virtual /*abstract*/ string FileTypeDescription { get; set; }

		public bool IsDirty
		{
			get
			{
				return this.m_isDirty;
			}
			set
			{
				this.m_isDirty = value;
				this.OnDirtyChanged(EventArgs.Empty);
			}
		}

		public virtual /*abstract*/ EventSequence Sequence { get; set; }
	}
}

