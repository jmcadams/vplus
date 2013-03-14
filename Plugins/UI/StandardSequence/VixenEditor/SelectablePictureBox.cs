namespace VixenEditor
{
    using System;
    using System.Windows.Forms;

    internal class SelectablePictureBox : PictureBox
    {
        public SelectablePictureBox()
        {
            base.SetStyle(ControlStyles.Selectable, true);
            base.TabStop = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return ((((keyData == Keys.Up) || (keyData == Keys.Down)) || (keyData == Keys.Left)) || (keyData == Keys.Right));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Select();
        }
    }
}

