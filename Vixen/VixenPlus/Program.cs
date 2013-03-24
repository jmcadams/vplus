using System;
using System.Windows.Forms;

namespace Vixen
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1(args));
			}
			catch (Exception exception)
			{
				MessageBox.Show(
					string.Format(
						"I'm sorry, but the application is about to die.  Please take a screenshot of this message.\n\n{0}\n\n{1}",
						exception.Message, exception.StackTrace));
			}
		}
	}
}