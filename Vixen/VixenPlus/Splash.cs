using System.Threading;
using System.Windows.Forms;

namespace VixenPlus
{
    internal partial class Splash : Form
    {
        private const int FadeMS = 40;

        public Splash()
        {
            InitializeComponent();

        }

        public void FadeIn() {
            if (!Visible) {
                Show();
                var big = Screen.AllScreens[2];
                Left = big.Bounds.X + (big.WorkingArea.Width - Width) /2;
                Top = big.Bounds.Y + (big.WorkingArea.Height - Height) / 2;
            }
            for (var opacity = 0d; opacity <= 1d; opacity += 0.1d) {
                Opacity = opacity;
                Refresh();
                Thread.Sleep(FadeMS);
            }
        }

        public void FadeOut() {
            for (var opacity = 1d; opacity > 0d; opacity -= 0.1d) {
                Opacity = opacity;
                Refresh();
                Thread.Sleep(FadeMS);
            }
            if (Visible) Hide();
        }
    }
}
