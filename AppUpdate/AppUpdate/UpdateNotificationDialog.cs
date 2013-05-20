namespace AppUpdate {
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    internal partial class UpdateNotificationDialog : Form {

        public UpdateNotificationDialog(bool restartRequired) {
            InitializeComponent();
            const string str = "There are updates available.  Would you like to download and install them now?";
            labelMessage.Text = str + (restartRequired ? "\n\n(This will cause the application to be restarted)" : string.Empty);
            Text = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName) + " Update";
        }


        public override sealed string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private const int ClassStyleNoClose = 0x200;

        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;
                createParams.ClassStyle |= ClassStyleNoClose;
                return createParams;
            }
        }
        //ComponentResourceManager manager = new ComponentResourceManager(typeof(UpdateNotificationDialog));
        //this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");
    }
}