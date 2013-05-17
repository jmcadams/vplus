using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using Properties;

namespace VixenPlus
{
    internal partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            Text = Resources.About + Vendor.ProductName;

            lblName.Text = Vendor.ProductName;
            lblDescription.Text = Vendor.ProductDescription;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            labelVersion.Text = string.Format(Resources.FormattedVersion, version);

            llblURL.Text = Vendor.ProductURL;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void llblURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llblURL.LinkVisited = true;
            Process.Start(Vendor.ProductURL);
        }
    }
}