namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class HelpDialog : Form {
		private Font m_bigFont = null;
		private string[] m_helpText;
		private int m_lineHeight;

		public HelpDialog(string helpText) {
			this.InitializeComponent();
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			Graphics graphics = base.CreateGraphics();
			this.m_helpText = helpText.Split(new char[] { '\n' });
			this.m_lineHeight = (int)graphics.MeasureString("Mg", this.Font).Height;
			int num = 0;
			foreach (string str in this.m_helpText) {
				num = Math.Max(num, (int)graphics.MeasureString(str, this.Font).Width);
			}
			base.Size = new Size((50 + num) + 50, (90 + (this.m_helpText.Length * this.m_lineHeight)) + 50);
			graphics.Dispose();
			this.m_bigFont = new Font("Arial", 16f, FontStyle.Bold);
		}



		private void HelpDialog_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == '\x001b') {
				base.Close();
			}
		}



		private void linkLabelClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			base.Close();
		}

		protected override void OnPaint(PaintEventArgs e) {
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
			e.Graphics.DrawString("Try this", this.m_bigFont, Brushes.DarkBlue, (float)60f, (float)30f);
			int num = 0;
			num = 90;
			for (int i = 0; i < this.m_helpText.Length; i++) {
				e.Graphics.DrawString(this.m_helpText[i], this.Font, Brushes.Black, 50f, (float)num);
				num += this.m_lineHeight;
			}
		}
	}
}

