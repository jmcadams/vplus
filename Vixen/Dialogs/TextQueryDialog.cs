using System.Windows.Forms;

using VixenPlus.Properties;

namespace Dialogs
{
    public partial class TextQueryDialog : Form
    {
        public TextQueryDialog(string caption, string query, string response)
        {
            InitializeComponent();
            Icon = Resources.VixenPlus;
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