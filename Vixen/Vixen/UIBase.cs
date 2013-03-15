namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [TypeDescriptionProvider(typeof(GeneralConcreteClassProvider)), ConcreteClass(typeof(UIBaseConcreteForm))]
    public abstract partial class UIBase : Form, IUIPlugIn, VixenMDI, IPlugIn
    {
        private bool m_isDirty = false;

        public event EventHandler DirtyChanged;

        public UIBase()
        {
            this.InitializeComponent();
        }

        

        

        public abstract EventSequence New();
        public abstract EventSequence New(EventSequence seedSequence);
        public abstract void Notify(Notification notification, object data);
        public virtual void OnDirtyChanged(EventArgs e)
        {
            if (this.DirtyChanged != null)
            {
                this.DirtyChanged(this, e);
            }
        }

        public abstract EventSequence Open(string filePath);
        public abstract DialogResult RunWizard(ref EventSequence resultSequence);
        public abstract void SaveTo(string filePath);
        string IPlugIn.get_Name()
        {
            return base.Name;
        }

        Form IUIPlugIn.get_MdiParent()
        {
            return base.MdiParent;
        }

        void IUIPlugIn.set_MdiParent(Form form1)
        {
            base.MdiParent = form1;
        }

        void IUIPlugIn.Show()
        {
            base.Show();
        }

        public abstract string Author { get; }

        public abstract string Description { get; }

        public abstract string FileExtension { get; }

        public abstract string FileTypeDescription { get; }

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

        public abstract EventSequence Sequence { get; set; }
    }
}

