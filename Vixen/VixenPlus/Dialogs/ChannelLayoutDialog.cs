using System;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	internal partial class ChannelLayoutDialog : Form
	{
		private void listBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void vectorImageStrip1_DragDrop(object sender, DragEventArgs e)
		{
		}

		private void vectorImageStrip1_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(typeof (Controller)) ? DragDropEffects.Move : DragDropEffects.None;
		}
	}
}