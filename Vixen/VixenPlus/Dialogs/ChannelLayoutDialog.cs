namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Vixen;
	using VixenControls;

	internal partial class ChannelLayoutDialog : Form {
		public ChannelLayoutDialog(IExecutable executableObject) {
		}

		private string GetTerminalDirectory(string directoryPath) {
			if (directoryPath.EndsWith(@"\")) {
				directoryPath = directoryPath.TrimEnd(new char[] { '\\' });
			}
			return directoryPath.Substring(directoryPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
		}

		private void listBoxPlugins_SelectedIndexChanged(object sender, EventArgs e) {
		}

		private void vectorImageStrip1_DragDrop(object sender, DragEventArgs e) {
		}

		private void vectorImageStrip1_DragOver(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(typeof(Controller))) {
				e.Effect = DragDropEffects.Move;
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}
	}
}