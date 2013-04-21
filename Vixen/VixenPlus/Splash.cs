using System;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

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
            if (!Visible) Show();
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
