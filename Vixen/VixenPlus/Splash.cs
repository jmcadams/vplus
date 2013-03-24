namespace Vixen {
	using System.Windows.Forms;
	using System.Reflection;

	internal partial class Splash : Form {

		public Splash() {
			InitializeComponent();
			lblAppName.Text = Vendor.ProductName;

			var version = Assembly.GetExecutingAssembly().GetName().Version;
			lblAppVersion.Text = string.Format("Version {0}.{1} Build {2}", version.Major, version.Minor, version.Build);
			lblTask.Text = "Loading...";
		}

		public string Task {
			set {
				lblTask.Text = value;
				Refresh();
			}
		}

	}
}

