using System;
using System.Reflection;

namespace Vixen
{
	internal partial class AboutDialog : System.Windows.Forms.Form
	{
		public AboutDialog()
		{
			InitializeComponent();

			this.Text = "About " + Vendor.ProductName;

			lblName.Text = Vendor.ProductName;
			lblDescription.Text = Vendor.ProductDescription;

			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			labelVersion.Text = string.Format("Version {0}", version);
			
			llblURL.Text = Vendor.ProductURL;
		}

		private void llblURL_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			llblURL.LinkVisited = true;
			System.Diagnostics.Process.Start(Vendor.ProductURL);
		}
	}
}

