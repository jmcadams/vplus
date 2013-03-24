using System;
using System.Windows.Forms;

namespace Vixen
{
	internal class CheckableToolStripMenuItem : ToolStripMenuItem
	{
		private bool m_canRaiseEvent = true;

		protected override bool CanRaiseEvents
		{
			get { return m_canRaiseEvent; }
		}

		public event EventHandler CheckClick;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_canRaiseEvent = e.X > ((base.Height + Padding.Top) + Padding.Bottom);
			if (!m_canRaiseEvent)
			{
				base.Checked = !base.Checked;
				if (CheckClick != null)
				{
					CheckClick(this, new EventArgs());
				}
			}
			else
			{
				base.OnMouseDown(e);
			}
		}
	}
}