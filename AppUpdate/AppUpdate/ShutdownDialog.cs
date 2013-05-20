namespace AppUpdate {
    using System.Windows.Forms;

    internal partial class ShutdownDialog : Form {

        public ShutdownDialog() {
            InitializeComponent();
            
        }

        private const int ClassStyleNoClose = 0x200;

        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;
                createParams.ClassStyle |= ClassStyleNoClose;
                return createParams;
            }
        }
        //ComponentResourceManager manager = new ComponentResourceManager(typeof(ShutdownDialog));
        //this.pictureBox1.Image = (Image)manager.GetObject("pictureBox1.Image");
        //this.labelMessage.Text = manager.GetString("labelMessage.Text");
    }
}