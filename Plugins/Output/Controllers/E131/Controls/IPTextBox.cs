using System.Windows.Forms;

namespace Controllers.E131.Controls
{
	//-------------------------------------------------------------
	//
	//	IPTextBox - a private class for editing IP addresses
	//
	//-------------------------------------------------------------

	public class IPTextBox : TextBox
	{
		protected override CreateParams	CreateParams
		{
			get
			{
				var cp	= base.CreateParams;
				cp.ClassName = "SysIPAddress32";
				cp.Height =	23;
				return cp;
			}
		}
	}
}
