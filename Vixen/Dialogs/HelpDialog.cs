using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dialogs
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
            var graphics = CreateGraphics();
            _helpText = helpText.Split(new[] {'\n'});
            _lineHeight = (int) graphics.MeasureString("Mg", Font).Height;
            var num = _helpText.Select(str => (int) graphics.MeasureString(str, Font).Width).Concat(new[] {0}).Max();
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
            var clientRectangle = ClientRectangle;
            clientRectangle.Width--;
            clientRectangle.Height--;
            e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
            clientRectangle.Inflate(-1, -1);
            e.Graphics.DrawRectangle(Pens.Navy, clientRectangle);
            clientRectangle.Inflate(-1, -1);
            e.Graphics.DrawRectangle(Pens.MediumBlue, clientRectangle);
            clientRectangle.Inflate(-1, -1);
            e.Graphics.DrawRectangle(Pens.RoyalBlue, clientRectangle);
            e.Graphics.DrawRectangle(Pens.Navy, 50, 25, ClientRectangle.Width - 100, 35);
            e.Graphics.DrawString("Try this", _bigFont, Brushes.DarkBlue, 60f, 30f);
            var lineHeight = 90;
            foreach (var t in _helpText) {
                e.Graphics.DrawString(t, Font, Brushes.Black, 50f, lineHeight);
                lineHeight += _lineHeight;
            }
        }
    }
}