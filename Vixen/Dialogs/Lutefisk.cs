using System.Diagnostics;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

namespace VixenPlus.Dialogs
{
    public partial class Lutefisk: Form
    {
        public Lutefisk()
        {
            InitializeComponent();
            Icon = Resources.VixenPlus;
        }

        private void Lutefisk_Click(object sender, System.EventArgs e) {
            Process.Start(@"http://www.diychristmas.org/vb1/forumdisplay.php?85-What-s-for-supper-Grandpa");
        }
    }
}
