namespace Vixen
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class VixenSimpleButton : PictureBox, IDisposable
    {
        private SolidBrush m_brush = new SolidBrush(Color.Black);
        private Color m_disabledColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
        private Color m_enabledColor = Color.FromArgb(0x80, 0x80, 0xff);
        private Font m_font = new Font("Arial", 13f, FontStyle.Bold);
        private Color m_hoverColor = Color.FromArgb(80, 80, 0xff);
        private bool m_hovered = false;
        private Pen m_pen = new Pen(Color.Black, 2f);
        private VixenSimpleButtonType m_type;

        public VixenSimpleButton(VixenSimpleButtonType type)
        {
            this.m_type = type;
            base.Size = new Size(20, 20);
        }

        ~VixenSimpleButton()
        {
            base.Dispose();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.m_hovered = true;
            this.Refresh();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_hovered = false;
            this.Refresh();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics graphics = pe.Graphics;
            Color color = base.Enabled ? (this.m_hovered ? this.m_hoverColor : this.m_enabledColor) : this.m_disabledColor;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Inflate(-2, -2);
            this.m_pen.Color = color;
            this.m_brush.Color = color;
            graphics.FillEllipse(Brushes.White, clientRectangle);
            graphics.DrawEllipse(this.m_pen, clientRectangle);
            switch (this.m_type)
            {
                case VixenSimpleButtonType.Add:
                    graphics.DrawString("+", this.m_font, this.m_brush, (float) 3f, (float) 1f);
                    break;

                case VixenSimpleButtonType.Edit:
                    graphics.DrawLine(this.m_pen, 6, 11, 11, 6);
                    graphics.DrawLine(this.m_pen, 9, 14, 14, 9);
                    break;

                case VixenSimpleButtonType.Remove:
                    graphics.DrawString("-", this.m_font, this.m_brush, (float) 3f, (float) 1f);
                    break;
            }
        }

        void IDisposable.Dispose()
        {
            this.m_brush.Dispose();
            this.m_font.Dispose();
            this.m_pen.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

