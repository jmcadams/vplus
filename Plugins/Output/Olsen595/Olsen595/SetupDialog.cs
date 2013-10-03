using System;
using System.Windows.Forms;
using System.Xml;

namespace Olsen595 {
    public partial class SetupDialog : Form {

        private readonly XmlNode _setupNode;

        public SetupDialog(XmlNode setupNode) {
            InitializeComponent();
            _setupNode = setupNode;
            SetTextFromNode("parallel1", textBoxParallel1From, textBoxParallel1To);
            SetTextFromNode("parallel2", textBoxParallel2From, textBoxParallel2To);
            SetTextFromNode("parallel3", textBoxParallel3From, textBoxParallel3To);
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            SetNodeFromText(textBoxParallel1From, textBoxParallel1To, "parallel1");
            SetNodeFromText(textBoxParallel2From, textBoxParallel2To, "parallel2");
            SetNodeFromText(textBoxParallel3From, textBoxParallel3To, "parallel3");
        }


        private void SetTextFromNode(string singleNode, Control tbFrom, Control tbTo) {
            var node = _setupNode.SelectSingleNode(singleNode);

            if (node == null || node.Attributes == null) {
                return;
            }

            tbFrom.Text = node.Attributes["from"].Value;
            tbTo.Text = node.Attributes["to"].Value;
        }


        private void SetNodeFromText(Control tbFrom, Control tbTo, string singleNode) {
            var node = _setupNode.SelectSingleNode(singleNode);

            if (node == null || node.Attributes == null) {
                return;
            }

            node.Attributes["from"].Value = tbFrom.Text;
            node.Attributes["to"].Value = tbTo.Text;
        }
    }
}