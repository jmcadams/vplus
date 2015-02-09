using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus {
    public partial class OutputPlugInUIBase : Form
    {
        public XmlNode DataNode;
        public IVixenMDI ExecutionParent;

        protected OutputPlugInUIBase()
        {
            InitializeComponent();
        }

        private const int ClassStyleNoClose = 0x200;

        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;
                createParams.ClassStyle |= ClassStyleNoClose;
                return createParams;
            }
        }

        private readonly List<Keys> _playerKeys = new List<Keys> { Keys.F5, Keys.F6, Keys.F7, Keys.F8 };

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (!_playerKeys.Contains(keyData) || null == ExecutionParent) {
                // just call the base class.
                return base.ProcessCmdKey(ref msg, keyData);
            }

            ExecutionParent.Notify(Notification.KeyDown, new KeyEventArgs(keyData));
            return true;
        }
    }
}