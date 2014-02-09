using System.Windows.Forms;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs
{
    public partial class TextQueryDialog : Form
    {
        public TextQueryDialog(string caption, string query, string response, string action = "OK")
        {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            Text = caption;
            labelQuery.Text = query;
            textBoxResponse.Text = response;
            buttonOK.Text = action;
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