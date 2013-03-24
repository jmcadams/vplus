using System.Windows.Forms;

namespace Vixen
{
	internal partial class ProgressDialog : Form
	{
		public ProgressDialog()
		{
			InitializeComponent();
		}

		public string Message
		{
			set
			{
				labelMessage.Text = value;
				labelMessage.Refresh();
			}
		}
	}
}