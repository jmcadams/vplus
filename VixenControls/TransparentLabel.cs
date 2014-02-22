using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VixenPlusCommon {
    public partial class TransparentLabel: Label
    {	
        public TransparentLabel()
        {
            InitializeComponent();
        }

        public TransparentLabel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void WndProc(ref Message m) {
            const int wmNchittest = 0x0084;
            const int httransparent = (-1);

            if (m.Msg == wmNchittest) {
                m.Result = (IntPtr)httransparent;
            }
            else {
                base.WndProc(ref m);
            }
        }
    }
}