using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vixen
{
	[ToolboxBitmap(typeof (Panel))]
	public class AddEditRemove : Panel
	{
		private readonly IContainer components;
		private VixenSimpleButton[] _buttons;

		public AddEditRemove()
		{
			components = null;
			InitializeComponent();
			Construct();
		}

		public AddEditRemove(IContainer container)
		{
			components = null;
			container.Add(this);
			InitializeComponent();
			Construct();
		}

		[DefaultValue(true)]
		public bool AddEnabled
		{
			get { return _buttons[0].Enabled; }
			set { _buttons[0].Enabled = value; }
		}

		[DefaultValue(true)]
		public bool AddVisible
		{
			get { return _buttons[0].Visible; }
			set { _buttons[0].Visible = value; }
		}

		[DefaultValue(true)]
		public bool EditEnabled
		{
			get { return _buttons[1].Enabled; }
			set { _buttons[1].Enabled = value; }
		}

		[DefaultValue(true)]
		public bool EditVisible
		{
			get { return _buttons[1].Visible; }
			set { _buttons[1].Visible = value; }
		}

		[DefaultValue(true)]
		public bool RemoveEnabled
		{
			get { return _buttons[2].Enabled; }
			set { _buttons[2].Enabled = value; }
		}

		[DefaultValue(true)]
		public bool RemoveVisible
		{
			get { return _buttons[2].Visible; }
			set { _buttons[2].Visible = value; }
		}

		public event EventHandler AddClick;

		public event EventHandler EditClick;

		public event EventHandler RemoveClick;

		private void AddEditRemove_ButtonClick(object sender, EventArgs e)
		{
			if ((sender == _buttons[0]) && (AddClick != null))
			{
				AddClick(sender, e);
			}
			else if ((sender == _buttons[1]) && (EditClick != null))
			{
				EditClick(sender, e);
			}
			else if ((sender == _buttons[2]) && (RemoveClick != null))
			{
				RemoveClick(sender, e);
			}
		}

		private void AddEditRemove_ButtonVisibleChanged(object sender, EventArgs e)
		{
			CalcPositions();
		}

		private void CalcPositions()
		{
			int num = 0;
			if (_buttons[0].Visible)
			{
				_buttons[0].Left = num;
				num += 0x1a;
			}
			if (_buttons[1].Visible)
			{
				_buttons[1].Left = num;
				num += 0x1a;
			}
			if (_buttons[2].Visible)
			{
				_buttons[2].Left = num;
				num += 0x1a;
			}
			Width = num;
		}

		private void Construct()
		{
			Size = new Size(0x48, 20);
			_buttons = new[]
				{
					new VixenSimpleButton(VixenSimpleButtonType.Add), new VixenSimpleButton(VixenSimpleButtonType.Edit),
					new VixenSimpleButton(VixenSimpleButtonType.Remove)
				};
			_buttons[0].Parent = this;
			_buttons[1].Parent = this;
			_buttons[2].Parent = this;
			EventHandler handler = AddEditRemove_ButtonVisibleChanged;
			_buttons[0].VisibleChanged += handler;
			_buttons[1].VisibleChanged += handler;
			_buttons[2].VisibleChanged += handler;
			EventHandler handler2 = AddEditRemove_ButtonClick;
			_buttons[0].Click += handler2;
			_buttons[1].Click += handler2;
			_buttons[2].Click += handler2;
			CalcPositions();
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
		}
	}
}