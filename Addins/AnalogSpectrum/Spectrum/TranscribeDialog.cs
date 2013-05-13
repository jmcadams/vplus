namespace Spectrum {
    using System.Windows.Forms;

    internal partial class TranscribeDialog : Form {
        public TranscribeDialog(int maximum) {
            InitializeComponent();
            progressBar.Maximum = maximum;
        }

        public int Progress {
            set {
                progressBar.Value = value;
                Refresh();
            }
        }
    }
}

