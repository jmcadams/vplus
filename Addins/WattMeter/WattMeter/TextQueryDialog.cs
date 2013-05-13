namespace WattMeter {
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class TextQueryDialog : Form {
        public TextQueryDialog(string caption, string query, string response) {
            this.InitializeComponent();
            this.Text = caption;
            this.labelQuery.Text = query;
            this.textBoxResponse.Text = response;
        }

        public string Response {
            get {
                return this.textBoxResponse.Text;
            }
        }
    }
}
