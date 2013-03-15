namespace Vixen.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;

    public class RoutineSelectDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private ListBox listBoxRoutines;
        private bool m_resizing = false;

        public RoutineSelectDialog()
        {
            this.InitializeComponent();
            foreach (string str in Directory.GetFiles(Paths.RoutinePath, "*.vir"))
            {
                this.listBoxRoutines.Items.Add(new Routine(str));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            foreach (Routine routine in this.listBoxRoutines.Items)
            {
                routine.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.listBoxRoutines = new ListBox();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new Point(0x130, 0x1a6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x181, 0x1a6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.listBoxRoutines.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxRoutines.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxRoutines.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.listBoxRoutines.ForeColor = Color.DarkSlateBlue;
            this.listBoxRoutines.ItemHeight = 100;
            this.listBoxRoutines.Location = new Point(12, 12);
            this.listBoxRoutines.Name = "listBoxRoutines";
            this.listBoxRoutines.ScrollAlwaysVisible = true;
            this.listBoxRoutines.Size = new Size(0x1c0, 0x194);
            this.listBoxRoutines.TabIndex = 3;
            this.listBoxRoutines.MouseDoubleClick += new MouseEventHandler(this.listBoxRoutines_MouseDoubleClick);
            this.listBoxRoutines.DrawItem += new DrawItemEventHandler(this.listBoxRoutines_DrawItem);
            this.listBoxRoutines.SelectedIndexChanged += new EventHandler(this.listBoxRoutines_SelectedIndexChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1d8, 0x1c9);
            base.Controls.Add(this.listBoxRoutines);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Name = "RoutineSelectDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Routine Selection";
            base.ResizeBegin += new EventHandler(this.RoutineSelectDialog_ResizeBegin);
            base.ResizeEnd += new EventHandler(this.RoutineSelectDialog_ResizeEnd);
            base.ResumeLayout(false);
        }

        private void listBoxRoutines_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!this.m_resizing && ((((e.State & (DrawItemState.Focus | DrawItemState.Selected)) == (DrawItemState.Focus | DrawItemState.Selected)) && (e.Index == this.listBoxRoutines.SelectedIndex)) || ((e.State & (DrawItemState.NoFocusRect | DrawItemState.Selected)) == DrawItemState.NoFocusRect)))
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                Routine routine = (Routine) this.listBoxRoutines.Items[e.Index];
                RectangleF layoutRectangle = new RectangleF((float) (e.Bounds.X + 5), (float) (e.Bounds.Y + 5), (float) ((this.listBoxRoutines.Width - 200) - 10), (float) (e.Bounds.Height - 10));
                e.Graphics.DrawString(routine.Name, this.listBoxRoutines.Font, Brushes.DarkSlateBlue, layoutRectangle);
                Rectangle rect = new Rectangle(e.Bounds.Width - 0xaf, e.Bounds.Y + 10, 150, 80);
                e.Graphics.FillRectangle(Brushes.White, rect);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.DrawRectangle(Pens.Black, (int) (rect.X - 1), (int) (rect.Y - 1), (int) (rect.Width + 1), (int) (rect.Height + 1));
                }
                else
                {
                    e.Graphics.DrawRectangle(Pens.White, (int) (rect.X - 1), (int) (rect.Y - 1), (int) (rect.Width + 1), (int) (rect.Height + 1));
                }
                float width = 150f / ((float) routine.PreviewBounds.Width);
                float height = 80f / ((float) routine.PreviewBounds.Height);
                int left = e.Bounds.Left;
                int top = e.Bounds.Top;
                int num7 = routine.PreviewBounds.Height;
                int num8 = routine.PreviewBounds.Width;
                SolidBrush brush = new SolidBrush(Color.LightBlue);
                for (int i = 0; i < num7; i++)
                {
                    for (int j = 0; j < num8; j++)
                    {
                        brush.Color = Color.FromArgb(routine.Preview.GetPixel(j, i).ToArgb());
                        e.Graphics.FillRectangle(brush, rect.Left + (j * width), rect.Top + (i * height), width, height);
                    }
                }
                brush.Dispose();
            }
        }

        private void listBoxRoutines_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listBoxRoutines.SelectedIndex != -1)
            {
                base.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void listBoxRoutines_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonOK.Enabled = this.listBoxRoutines.SelectedItem != null;
        }

        private void RoutineSelectDialog_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void RoutineSelectDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
            this.listBoxRoutines.Refresh();
        }

        public string SelectedRoutine
        {
            get
            {
                if (this.listBoxRoutines.SelectedItem == null)
                {
                    return null;
                }
                return ((Routine) this.listBoxRoutines.SelectedItem).FilePath;
            }
        }
    }
}

