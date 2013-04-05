namespace VixenEditor
{
    using System.Windows.Forms;

    internal class SelectablePictureBox : PictureBox
    {
        public SelectablePictureBox()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();
        }
    }
}

