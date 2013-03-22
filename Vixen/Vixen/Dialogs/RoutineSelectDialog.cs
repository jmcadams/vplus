namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Vixen;

	public partial class RoutineSelectDialog : Form {
		private bool m_resizing = false;

		public RoutineSelectDialog() {
			this.InitializeComponent();
			foreach (string str in Directory.GetFiles(Paths.RoutinePath, "*.vir")) {
				this.listBoxRoutines.Items.Add(new Routine(str));
			}
		}

		private void listBoxRoutines_DrawItem(object sender, DrawItemEventArgs e) {
			if (!this.m_resizing && ((((e.State & (DrawItemState.Focus | DrawItemState.Selected)) == (DrawItemState.Focus | DrawItemState.Selected)) && (e.Index == this.listBoxRoutines.SelectedIndex)) || ((e.State & (DrawItemState.NoFocusRect | DrawItemState.Selected)) == DrawItemState.NoFocusRect))) {
				e.Graphics.FillRectangle(Brushes.White, e.Bounds);
				Routine routine = (Routine)this.listBoxRoutines.Items[e.Index];
				RectangleF layoutRectangle = new RectangleF((float)(e.Bounds.X + 5), (float)(e.Bounds.Y + 5), (float)((this.listBoxRoutines.Width - 200) - 10), (float)(e.Bounds.Height - 10));
				e.Graphics.DrawString(routine.Name, this.listBoxRoutines.Font, Brushes.DarkSlateBlue, layoutRectangle);
				Rectangle rect = new Rectangle(e.Bounds.Width - 0xaf, e.Bounds.Y + 10, 150, 80);
				e.Graphics.FillRectangle(Brushes.White, rect);
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
					e.Graphics.DrawRectangle(Pens.Black, (int)(rect.X - 1), (int)(rect.Y - 1), (int)(rect.Width + 1), (int)(rect.Height + 1));
				}
				else {
					e.Graphics.DrawRectangle(Pens.White, (int)(rect.X - 1), (int)(rect.Y - 1), (int)(rect.Width + 1), (int)(rect.Height + 1));
				}
				float width = 150f / ((float)routine.PreviewBounds.Width);
				float height = 80f / ((float)routine.PreviewBounds.Height);
				int left = e.Bounds.Left;
				int top = e.Bounds.Top;
				int num7 = routine.PreviewBounds.Height;
				int num8 = routine.PreviewBounds.Width;
				SolidBrush brush = new SolidBrush(Color.LightBlue);
				for (int i = 0; i < num7; i++) {
					for (int j = 0; j < num8; j++) {
						brush.Color = Color.FromArgb(routine.Preview.GetPixel(j, i).ToArgb());
						e.Graphics.FillRectangle(brush, rect.Left + (j * width), rect.Top + (i * height), width, height);
					}
				}
				brush.Dispose();
			}
		}

		private void listBoxRoutines_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (this.listBoxRoutines.SelectedIndex != -1) {
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}

		private void listBoxRoutines_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonOK.Enabled = this.listBoxRoutines.SelectedItem != null;
		}

		private void RoutineSelectDialog_ResizeBegin(object sender, EventArgs e) {
			this.m_resizing = true;
		}

		private void RoutineSelectDialog_ResizeEnd(object sender, EventArgs e) {
			this.m_resizing = false;
			this.listBoxRoutines.Refresh();
		}

		public string SelectedRoutine {
			get {
				if (this.listBoxRoutines.SelectedItem == null) {
					return null;
				}
				return ((Routine)this.listBoxRoutines.SelectedItem).FilePath;
			}
		}
	}
}

