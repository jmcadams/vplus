namespace Vixen
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class CheckableToolStripMenuItem : ToolStripMenuItem
    {
        private bool m_canRaiseEvent = true;

        public event EventHandler CheckClick;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.m_canRaiseEvent = e.X > ((base.Height + this.Padding.Top) + this.Padding.Bottom);
            if (!this.m_canRaiseEvent)
            {
                base.Checked = !base.Checked;
                if (this.CheckClick != null)
                {
                    this.CheckClick(this, new EventArgs());
                }
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        protected override bool CanRaiseEvents
        {
            get
            {
                return this.m_canRaiseEvent;
            }
        }
    }
}

