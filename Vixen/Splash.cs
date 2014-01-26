using System.Threading;
using System.Windows.Forms;

internal partial class Splash : Form
{
    private const int FadeMS = 40;

    public Splash()
    {
        InitializeComponent();

    }

    public void FadeIn(Screen screen) {
        if (!Visible) {
            Left = screen.Bounds.X + (screen.WorkingArea.Width - Width) /2;
            Top = screen.Bounds.Y + (screen.WorkingArea.Height - Height) / 2;
            Show();
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