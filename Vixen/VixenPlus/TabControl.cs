using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus
{
	[ToolboxBitmap(typeof (System.Windows.Forms.TabControl))]
	public class TabControl : System.Windows.Forms.TabControl
	{
		private IContainer components;
		private bool m_HideTabs;

		public TabControl()
		{
			m_HideTabs = false;
			components = null;
			InitializeComponent();
		}

		public TabControl(IContainer container)
		{
			m_HideTabs = false;
			components = null;
			container.Add(this);
			InitializeComponent();
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				int num;
				int height;
				if (HideTabs)
				{
					return new Rectangle(0, 0, base.Width, base.Height);
				}
				height = base.Alignment <= TabAlignment.Bottom ? base.ItemSize.Height : base.ItemSize.Width;
				if (base.Appearance == TabAppearance.Normal)
				{
					num = 5 + (height*base.RowCount);
				}
				else
				{
					num = (3 + height)*base.RowCount;
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
			get { return m_HideTabs; }
			set
			{
				if (m_HideTabs != value)
				{
					m_HideTabs = value;
					if (value)
					{
						ourMultiline = true;
					}
					base.UpdateStyles();
				}
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		public bool ourMultiline
		{
			get { return (HideTabs || base.Multiline); }
			set {
				base.Multiline = HideTabs || value;
			}
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