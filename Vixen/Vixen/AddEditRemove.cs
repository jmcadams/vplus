namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Panel))]
    public class AddEditRemove : Panel
    {
        private IContainer components;
        private VixenSimpleButton[] m_buttons;

        public event EventHandler AddClick;

        public event EventHandler EditClick;

        public event EventHandler RemoveClick;

        public AddEditRemove()
        {
            this.components = null;
            this.InitializeComponent();
            this.Construct();
        }

        public AddEditRemove(IContainer container)
        {
            this.components = null;
            container.Add(this);
            this.InitializeComponent();
            this.Construct();
        }

        private void AddEditRemove_ButtonClick(object sender, EventArgs e)
        {
            if ((sender == this.m_buttons[0]) && (this.AddClick != null))
            {
                this.AddClick(sender, e);
            }
            else if ((sender == this.m_buttons[1]) && (this.EditClick != null))
            {
                this.EditClick(sender, e);
            }
            else if ((sender == this.m_buttons[2]) && (this.RemoveClick != null))
            {
                this.RemoveClick(sender, e);
            }
        }

        private void AddEditRemove_ButtonVisibleChanged(object sender, EventArgs e)
        {
            this.CalcPositions();
        }

        private void CalcPositions()
        {
            int num = 0;
            if (this.m_buttons[0].Visible)
            {
                this.m_buttons[0].Left = num;
                num += 0x1a;
            }
            if (this.m_buttons[1].Visible)
            {
                this.m_buttons[1].Left = num;
                num += 0x1a;
            }
            if (this.m_buttons[2].Visible)
            {
                this.m_buttons[2].Left = num;
                num += 0x1a;
            }
            base.Width = num;
        }

        private void Construct()
        {
            base.Size = new Size(0x48, 20);
            this.m_buttons = new VixenSimpleButton[] { new VixenSimpleButton(VixenSimpleButtonType.Add), new VixenSimpleButton(VixenSimpleButtonType.Edit), new VixenSimpleButton(VixenSimpleButtonType.Remove) };
            this.m_buttons[0].Parent = this;
            this.m_buttons[1].Parent = this;
            this.m_buttons[2].Parent = this;
            EventHandler handler = new EventHandler(this.AddEditRemove_ButtonVisibleChanged);
            this.m_buttons[0].VisibleChanged += handler;
            this.m_buttons[1].VisibleChanged += handler;
            this.m_buttons[2].VisibleChanged += handler;
            EventHandler handler2 = new EventHandler(this.AddEditRemove_ButtonClick);
            this.m_buttons[0].Click += handler2;
            this.m_buttons[1].Click += handler2;
            this.m_buttons[2].Click += handler2;
            this.CalcPositions();
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
        }

        [DefaultValue(true)]
        public bool AddEnabled
        {
            get
            {
                return this.m_buttons[0].Enabled;
            }
            set
            {
                this.m_buttons[0].Enabled = value;
            }
        }

        [DefaultValue(true)]
        public bool AddVisible
        {
            get
            {
                return this.m_buttons[0].Visible;
            }
            set
            {
                this.m_buttons[0].Visible = value;
            }
        }

        [DefaultValue(true)]
        public bool EditEnabled
        {
            get
            {
                return this.m_buttons[1].Enabled;
            }
            set
            {
                this.m_buttons[1].Enabled = value;
            }
        }

        [DefaultValue(true)]
        public bool EditVisible
        {
            get
            {
                return this.m_buttons[1].Visible;
            }
            set
            {
                this.m_buttons[1].Visible = value;
            }
        }

        [DefaultValue(true)]
        public bool RemoveEnabled
        {
            get
            {
                return this.m_buttons[2].Enabled;
            }
            set
            {
                this.m_buttons[2].Enabled = value;
            }
        }

        [DefaultValue(true)]
        public bool RemoveVisible
        {
            get
            {
                return this.m_buttons[2].Visible;
            }
            set
            {
                this.m_buttons[2].Visible = value;
            }
        }
    }
}

