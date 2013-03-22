using System;
using System.Reflection;

namespace Vixen
{
    internal partial class AboutDialog : System.Windows.Forms.Form
    {
        public AboutDialog()
        {
            this.InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.labelVersion.Text = string.Format("v. {0}", version);
            this.label1.Text = Vendor.ProductName;
        }
    }
}

