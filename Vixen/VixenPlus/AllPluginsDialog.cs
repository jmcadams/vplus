namespace Vixen
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Windows.Forms;

	[Obsolete]
	internal partial class AllPluginsDialog : Form
	{
		public AllPluginsDialog()
		{
			this.InitializeComponent();
		}

		private void AddAssembly(Assembly assembly, string relativePath, System.Type implementor)
		{
		}

		private void AllPluginsDialog_Load(object sender, EventArgs e)
		{
			string[] strArray = new string[] { Paths.AddinPath, Paths.OutputPluginPath, Paths.TriggerPluginPath, Paths.UIPluginPath };
			Assembly assembly = null;
			System.Type implementor = null;
			this.Cursor = Cursors.WaitCursor;
			try
			{
				foreach (string str2 in strArray)
				{
					string relativePath = str2.Substring(Paths.BinaryPath.Length);
					foreach (string str3 in Directory.GetFiles(str2, "*.dll", SearchOption.TopDirectoryOnly))
					{
						if (assembly != null)
						{
							this.AddAssembly(assembly, relativePath, implementor);
						}
					}
				}
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
	}
}

