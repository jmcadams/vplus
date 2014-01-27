using System.Windows.Forms;

namespace Dialogs
{
    public partial class TextQueryDialog : Form
    {
        public TextQueryDialog(string caption, string query, string response)
        {
            InitializeComponent();
            Text = caption;
            labelQuery.Text = query;
            textBoxResponse.Text = response;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public string Response
        {
            get { return textBoxResponse.Text; }
            set { textBoxResponse.Text = value; }
        }
    }
}