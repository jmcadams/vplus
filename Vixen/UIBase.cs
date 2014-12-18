using System;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

namespace VixenPlus {
    public partial class UIBase : Form, IUIPlugIn {
        private bool _isDirty;


        protected UIBase() {
            InitializeComponent();
            Icon = Resources.VixenPlus;
        }


        public virtual EventSequence New() {
            throw new NotImplementedException();
        }


        public virtual EventSequence New(EventSequence seedSequence) {
            throw new NotImplementedException();
        }


        public virtual void Notify(Notification notification, object data) {
            throw new NotImplementedException();
        }


        public virtual EventSequence Open(string filePath) {
            throw new NotImplementedException();
        }


        public virtual DialogResult RunWizard(ref EventSequence resultSequence) {
            throw new NotImplementedException();
        }


        public virtual void SaveTo() {
            throw new NotImplementedException();
        }


        void IUIPlugIn.Show() {
            Show();
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public virtual string FileExtension { get; set; }

        public virtual string FileTypeDescription { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public bool IsDirty {
            get { return _isDirty; }
            set {
                _isDirty = value;
                OnDirtyChanged(EventArgs.Empty);
            }
        }

        public virtual EventSequence Sequence { get; set; }
        public event EventHandler DirtyChanged;


        protected virtual void OnDirtyChanged(EventArgs e) {
            if (DirtyChanged != null) {
                DirtyChanged(this, e);
            }
        }
    }
}