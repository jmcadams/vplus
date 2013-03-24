using System;
using System.Windows.Forms;

namespace Vixen
{
	internal class CheckableToolStripMenuItem : ToolStripMenuItem
	{
		private bool _canRaiseEvent = true;

		protected override bool CanRaiseEvents
		{
			get { return _canRaiseEvent; }
		}

		public event EventHandler CheckClick;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			_canRaiseEvent = e.X > ((Height + Padding.Top) + Padding.Bottom);
			if (!_canRaiseEvent)
			{
				Checked = !Checked;
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