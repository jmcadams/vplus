using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus {
    [ToolboxBitmap(typeof (System.Windows.Forms.TabControl))]
    public class TabControl : System.Windows.Forms.TabControl
    {
        private bool _hideTabs;
        private IContainer components;

        public TabControl()
        {
            _hideTabs = false;
            components = null;
            InitializeComponent();
        }

        public TabControl(IContainer container)
        {
            _hideTabs = false;
            components = null;
            container.Add(this);
            InitializeComponent();
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                int num;
                if (HideTabs)
                {
                    return new Rectangle(0, 0, Width, Height);
                }
                var height = Alignment <= TabAlignment.Bottom ? ItemSize.Height : ItemSize.Width;
                if (Appearance == TabAppearance.Normal)
                {
                    num = 5 + (height*RowCount);
                }
                else
                {
                    num = (3 + height)*RowCount;
                }
                switch (Alignment)
                {
                    case TabAlignment.Bottom:
                        return new Rectangle(4, 4, Width - 8, (Height - num) - 4);

                    case TabAlignment.Left:
                        return new Rectangle(num, 4, (Width - num) - 4, Height - 8);

                    case TabAlignment.Right:
                        return new Rectangle(4, 4, (Width - num) - 4, Height - 8);
                }
                return new Rectangle(4, num, Width - 8, (Height - num) - 4);
            }
        }

        [DefaultValue(false), RefreshProperties(RefreshProperties.All)]
        public bool HideTabs
        {
            private get { return _hideTabs; }
            set
            {
                if (_hideTabs == value) {
                    return;
                }
                _hideTabs = value;
                if (value)
                {
                    OurMultiline = true;
                }
                UpdateStyles();
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public bool OurMultiline
        {
            set { Multiline = HideTabs || value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }
    }
}