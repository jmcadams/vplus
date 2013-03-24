using System;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
	public sealed partial class HelpDialog : Form
	{
		private readonly Font _bigFont;
		private readonly string[] _helpText;
		private readonly int _lineHeight;

		public HelpDialog(string helpText)
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			Graphics graphics = CreateGraphics();
			_helpText = helpText.Split(new[] {'\n'});
			_lineHeight = (int) graphics.MeasureString("Mg", Font).Height;
			int num = 0;
			foreach (string str in _helpText)
			{
				num = Math.Max(num, (int) graphics.MeasureString(str, Font).Width);
			}
			Size = new Size((50 + num) + 50, (90 + (_helpText.Length*_lineHeight)) + 50);
			graphics.Dispose();
			_bigFont = new Font("Arial", 16f, FontStyle.Bold);
		}


		private void HelpDialog_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\x001b')
			{
				Close();
			}
		}


		private void linkLabelClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Close();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clientRectangle = ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.MediumBlue, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.RoyalBlue, clientRectangle);
			e.Graphics.DrawRectangle(Pens.Navy, 50, 0x19, ClientRectangle.Width - 100, 0x23);
			e.Graphics.DrawString("Try this", _bigFont, Brushes.DarkBlue, 60f, 30f);
			var num = 90;
			for (var i = 0; i < _helpText.Length; i++)
			{
				e.Graphics.DrawString(_helpText[i], Font, Brushes.Black, 50f, num);
				num += _lineHeight;
			}
		}
	}
}