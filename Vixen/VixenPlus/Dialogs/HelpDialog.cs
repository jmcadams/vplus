using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class HelpDialog : Form
	{
		private readonly Font m_bigFont;
		private readonly string[] m_helpText;
		private readonly int m_lineHeight;

		public HelpDialog(string helpText)
		{
			InitializeComponent();
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			Graphics graphics = base.CreateGraphics();
			m_helpText = helpText.Split(new[] {'\n'});
			m_lineHeight = (int) graphics.MeasureString("Mg", Font).Height;
			int num = 0;
			foreach (string str in m_helpText)
			{
				num = Math.Max(num, (int) graphics.MeasureString(str, Font).Width);
			}
			base.Size = new Size((50 + num) + 50, (90 + (m_helpText.Length*m_lineHeight)) + 50);
			graphics.Dispose();
			m_bigFont = new Font("Arial", 16f, FontStyle.Bold);
		}


		private void HelpDialog_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\x001b')
			{
				base.Close();
			}
		}


		private void linkLabelClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			base.Close();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.MediumBlue, clientRectangle);
			clientRectangle.Inflate(-1, -1);
			e.Graphics.DrawRectangle(Pens.RoyalBlue, clientRectangle);
			e.Graphics.DrawRectangle(Pens.Navy, 50, 0x19, base.ClientRectangle.Width - 100, 0x23);
			e.Graphics.DrawString("Try this", m_bigFont, Brushes.DarkBlue, 60f, 30f);
			int num = 0;
			num = 90;
			for (int i = 0; i < m_helpText.Length; i++)
			{
				e.Graphics.DrawString(m_helpText[i], Font, Brushes.Black, 50f, num);
				num += m_lineHeight;
			}
		}
	}
}