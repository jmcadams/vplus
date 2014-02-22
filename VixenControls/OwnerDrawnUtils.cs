using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

namespace CommonControls {
    public static class OwnerDrawnUtils {
        private static readonly SolidBrush GenericBrush = new SolidBrush(Color.Black);
        private const string Checkmark = "\u2714";

        // For ComboBoxes
        public static void DrawItem(this DrawItemEventArgs e, string name, Color color, bool useCheckmark = false) {
            e.DrawBackground();

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                           (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            GenericBrush.Color = color;
            e.Graphics.FillRectangle(selected && !useCheckmark ? SystemBrushes.Highlight : GenericBrush, e.Bounds);
            var contrastingBrush = selected && !useCheckmark ? SystemBrushes.HighlightText : color.GetTextColor();
            e.Graphics.DrawString(name, e.Font, contrastingBrush, new RectangleF(e.Bounds.Location, e.Bounds.Size));
            if (selected && useCheckmark) {
                e.Graphics.DrawString(Checkmark, e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
            }
            e.DrawFocusRectangle();
        }


        public static void DrawItemWide(this DrawItemEventArgs e, string name, Color color, bool useCheckmark) {
            e.DrawBackground();

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                           (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            GenericBrush.Color = color;
            e.Graphics.FillRectangle(selected && !useCheckmark ? SystemBrushes.Highlight : GenericBrush, e.Bounds);
            var contrastingBrush = selected && !useCheckmark ? SystemBrushes.HighlightText : color.GetTextColor();
            var loc = e.Bounds.Location;
            if (useCheckmark) {
                loc.Offset((int)e.Graphics.MeasureString(Checkmark, e.Font).Width + 2, 0);
            }
            e.Graphics.DrawString(name, e.Font, contrastingBrush, new RectangleF(loc, e.Graphics.MeasureString(name, e.Font)));
            if (selected && useCheckmark) {
                e.Graphics.DrawString(Checkmark, e.Font, contrastingBrush, 2, e.Bounds.Y);
            }
            e.DrawFocusRectangle();
        }


        // For List Boxes -- TODO Need to make this work with prefereneces for UseCheckmark
        public static void DrawItem(this DrawItemEventArgs e, string text, Color color, ListBox lb, bool useCheckmark) {
            e.DrawBackground();

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            GenericBrush.Color = color;
            e.Graphics.FillRectangle(selected && !useCheckmark ? SystemBrushes.Highlight : GenericBrush, e.Bounds);
            var contrastingBrush = selected && !useCheckmark ? SystemBrushes.HighlightText : color.GetTextColor();
            e.Graphics.DrawString(text, e.Font, contrastingBrush, lb.GetItemRectangle(e.Index).Location);

            if (selected && useCheckmark) {
                e.Graphics.DrawString(Checkmark, e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }


        // For Ownder Drawn TreeViews
        public static void DrawItem(this DrawTreeNodeEventArgs e, Color channelColor, TreeView treeView, bool useCheckmark) {
            if (treeView == null) {
                e.DrawDefault = true;
                return;
            }

            if (e.Bounds.Left < 0 || e.Bounds.Top < 0) {
                return;
            }

            var fillRect = new Rectangle(e.Node.Bounds.X, e.Node.Bounds.Y, treeView.Width - e.Node.Bounds.Left, e.Node.Bounds.Height);
            GenericBrush.Color = channelColor;

            bool selected;
            var view = treeView as MultiSelectTreeview;
            if (view != null) {
                selected = view.SelectedNodes.Contains(e.Node);
            }
            else {
                selected = (e.State & TreeNodeStates.Selected) != 0;
            }

            var rectBrush = selected && !useCheckmark ? SystemBrushes.Highlight : GenericBrush;
            e.Graphics.FillRectangle(rectBrush, fillRect);
            var stringBrush = selected && !useCheckmark ? SystemBrushes.HighlightText : channelColor.GetTextColor();
            e.Graphics.DrawString(e.Node.Text, treeView.Font, stringBrush, e.Bounds.Left, e.Bounds.Top);


            if (selected && useCheckmark) {
                e.Graphics.DrawString(Checkmark, treeView.Font, channelColor.GetTextColor(), fillRect.Right - 40, e.Bounds.Top);
            }
        }
    }
}
