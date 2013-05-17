using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
    public partial class RoutineSelectDialog : Form
    {
        private bool _resizing;

        public RoutineSelectDialog()
        {
            InitializeComponent();
            foreach (var str in Directory.GetFiles(Paths.RoutinePath, "*.vir"))
            {
                listBoxRoutines.Items.Add(new Routine(str));
            }
        }

        public string SelectedRoutine
        {
            get {
                return listBoxRoutines.SelectedItem == null ? null : ((Routine) listBoxRoutines.SelectedItem).FilePath;
            }
        }

        private void listBoxRoutines_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (_resizing ||
                ((((e.State & (DrawItemState.Focus | DrawItemState.Selected)) != (DrawItemState.Focus | DrawItemState.Selected)) ||
                  (e.Index != listBoxRoutines.SelectedIndex)) &&
                 ((e.State & (DrawItemState.NoFocusRect | DrawItemState.Selected)) != DrawItemState.NoFocusRect))) {
                return;
            }
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            var routine = (Routine) listBoxRoutines.Items[e.Index];
            var layoutRectangle = new RectangleF((e.Bounds.X + 5), (e.Bounds.Y + 5), ((listBoxRoutines.Width - 200) - 10),
                                                 (e.Bounds.Height - 10));
            e.Graphics.DrawString(routine.Name, listBoxRoutines.Font, Brushes.DarkSlateBlue, layoutRectangle);
            var rect = new Rectangle(e.Bounds.Width - 175, e.Bounds.Y + 10, 150, 80);
            e.Graphics.FillRectangle(Brushes.White, rect);
            e.Graphics.DrawRectangle((e.State & DrawItemState.Selected) == DrawItemState.Selected ? Pens.Black : Pens.White,
                                     (rect.X - 1), (rect.Y - 1), (rect.Width + 1), (rect.Height + 1));
            var width = 150f/(routine.PreviewBounds.Width);
            var height = 80f/(routine.PreviewBounds.Height);
            var num7 = routine.PreviewBounds.Height;
            var num8 = routine.PreviewBounds.Width;
            var brush = new SolidBrush(Color.LightBlue);
            for (var i = 0; i < num7; i++)
            {
                for (var j = 0; j < num8; j++)
                {
                    brush.Color = Color.FromArgb(routine.Preview.GetPixel(j, i).ToArgb());
                    e.Graphics.FillRectangle(brush, rect.Left + (j*width), rect.Top + (i*height), width, height);
                }
            }
            brush.Dispose();
        }

        private void listBoxRoutines_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxRoutines.SelectedIndex != -1)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void listBoxRoutines_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = listBoxRoutines.SelectedItem != null;
        }

        private void RoutineSelectDialog_ResizeBegin(object sender, EventArgs e)
        {
            _resizing = true;
        }

        private void RoutineSelectDialog_ResizeEnd(object sender, EventArgs e)
        {
            _resizing = false;
            listBoxRoutines.Refresh();
        }
    }
}