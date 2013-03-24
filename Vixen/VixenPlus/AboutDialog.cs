using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class AboutDialog : Form
	{
		public AboutDialog()
		{
			InitializeComponent();

			Text = "About " + Vendor.ProductName;

			lblName.Text = Vendor.ProductName;
			lblDescription.Text = Vendor.ProductDescription;

			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			labelVersion.Text = string.Format("Version {0}", version);

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