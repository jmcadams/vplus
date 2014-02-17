using System.Drawing;
using System.Windows.Forms;

namespace CommonControls {
    public partial class FadableLabel : Label {
        public FadableLabel() {
            InitializeComponent();
        }


        protected override void OnPaint(PaintEventArgs e) {
            var rc = ClientRectangle;
            var fmt = new StringFormat(StringFormat.GenericTypographic);
            using (var br = new SolidBrush(ForeColor)) {
                e.Graphics.DrawString(Text, Font, br, rc, fmt);
            }

        }
    }
}