namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ThinSelection : Form
    {
        private IContainer components = null;
        private ListBox listBox;
        private int m_selectedIndex = -1;

        public ThinSelection(string[] values)
        {
            this.InitializeComponent();
            this.listBox.Items.AddRange(values);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBox = new ListBox();
            base.SuspendLayout();
            this.listBox.Dock = DockStyle.Fill;
            this.listBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 0x11;
            this.listBox.Location = new Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new Size(0xa8, 0x103);
            this.listBox.TabIndex = 0;
            this.listBox.DrawItem += new DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.SelectedIndexChanged += new EventHandler(this.listBox_SelectedIndexChanged);
            this.listBox.DoubleClick += new EventHandler(this.listBox_DoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xa8, 260);
            base.ControlBox = false;
            base.Controls.Add(this.listBox);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ThinSelection";
            base.StartPosition = FormStartPosition.Manual;
            base.KeyDown += new KeyEventHandler(this.ThinSelection_KeyDown);
            base.ResumeLayout(false);
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectedIndex != -1)
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(this.listBox.Items[e.Index].ToString(), this.listBox.Font, ((e.State & DrawItemState.Selected) != DrawItemState.None) ? Brushes.White : Brushes.Black, (float) (e.Bounds.Left + 3), (float) (e.Bounds.Top + 2));
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_selectedIndex = this.listBox.SelectedIndex;
        }

        private void ThinSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.DialogResult = DialogResult.Cancel;
            }
            else if ((e.KeyCode == Keys.Return) && (this.m_selectedIndex != -1))
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.m_selectedIndex;
            }
        }
    }
}

