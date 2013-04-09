using System;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace VixenPlus
{
    internal partial class Splash : Form
    {
        private const int FADE_MS = 40;

        public Splash()
        {
            InitializeComponent();
        }

        public void FadeIn() {
            for (var i = 0; i <= 100; i += 10) {
                this.Opacity = (double)(i / 100d);
                this.Refresh();
                Thread.Sleep(FADE_MS);
            }
        }

        public void FadeOut() {
            for (var i = 100; i > 0; i -= 10) {
                this.Opacity = (double)(i / 100d);
                this.Refresh();
                Thread.Sleep(FADE_MS);
            }
        }
    }
}