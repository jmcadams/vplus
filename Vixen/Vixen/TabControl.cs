namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class TabControl : System.Windows.Forms.TabControl
    {
        private IContainer components;
        private bool m_HideTabs;

        public TabControl()
        {
            this.m_HideTabs = false;
            this.components = null;
            this.InitializeComponent();
        }

        public TabControl(IContainer container)
        {
            this.m_HideTabs = false;
            this.components = null;
            container.Add(this);
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                int num;
                int height;
                if (this.HideTabs)
                {
                    return new Rectangle(0, 0, base.Width, base.Height);
                }
                if (base.Alignment <= TabAlignment.Bottom)
                {
                    height = base.ItemSize.Height;
                }
                else
                {
                    height = base.ItemSize.Width;
                }
                if (base.Appearance == TabAppearance.Normal)
                {
                    num = 5 + (height * base.RowCount);
                }
                else
                {
                    num = (3 + height) * base.RowCount;
                }
                switch (base.Alignment)
                {
                    case TabAlignment.Bottom:
                        return new Rectangle(4, 4, base.Width - 8, (base.Height - num) - 4);

                    case TabAlignment.Left:
                        return new Rectangle(num, 4, (base.Width - num) - 4, base.Height - 8);

                    case TabAlignment.Right:
                        return new Rectangle(4, 4, (base.Width - num) - 4, base.Height - 8);
                }
                return new Rectangle(4, num, base.Width - 8, (base.Height - num) - 4);
            }
        }

        [DefaultValue(false), RefreshProperties(RefreshProperties.All)]
        public bool HideTabs
        {
            get
            {
                return this.m_HideTabs;
            }
            set
            {
                if (this.m_HideTabs != value)
                {
                    this.m_HideTabs = value;
                    if (value)
                    {
                        this.Multiline = true;
                    }
                    base.UpdateStyles();
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public bool Multiline
        {
            get
            {
                return (this.HideTabs || base.Multiline);
            }
            set
            {
                if (this.HideTabs)
                {
                    base.Multiline = true;
                }
                else
                {
                    base.Multiline = value;
                }
            }
        }
    }
}

