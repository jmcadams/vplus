using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs
{
    public partial class RoutineSelectDialog : Form
    {
        private bool _resizing;

        public RoutineSelectDialog()
        {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            foreach (var str in Directory.GetFiles(Paths.RoutinePath, Vendor.All + Vendor.RoutineExtension))
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
            if (_resizing || e.Index == -1) {
                return;
            }
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            var routine = (Routine) listBoxRoutines.Items[e.Index];
            var layoutRectangle = new RectangleF((e.Bounds.X + 5), (e.Bounds.Y + 5), ((listBoxRoutines.Width - 200) - 10),
                                                 (e.Bounds.Height - 10));
            e.Graphics.DrawString(routine.Name, listBoxRoutines.Font, Brushes.DarkSlateBlue, layoutRectangle);
            var rect = new Rectangle(e.Bounds.Width - 175, e.Bounds.Y + 10, Routine.DefaultWidth, Routine.DefaultHeight);
            e.Graphics.DrawRectangle((e.State & DrawItemState.Selected) == DrawItemState.Selected ? Pens.Black : Pens.White,
                                     (rect.X - 1), (rect.Y - 1), (rect.Width + 1), (rect.Height + 1));
            e.Graphics.DrawImage(routine.Preview,rect);
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