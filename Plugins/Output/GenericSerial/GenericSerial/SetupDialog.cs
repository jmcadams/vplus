using System;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;
using VixenPlus;

namespace GenericSerial {
    internal partial class SetupDialog : Form {
        private readonly XmlNode _setupNode;


        public SetupDialog(XmlNode setupNode) {
            InitializeComponent();
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());

            _setupNode = setupNode;

            var portName = Xml.GetNodeAlways(_setupNode, "Name").InnerText;
            comboBoxPort.SelectedIndex = comboBoxPort.Items.IndexOf((portName.Length > 3) ? portName : "COM1");

            var baud = Xml.GetNodeAlways(_setupNode, "Baud").InnerText;
            comboBoxBaud.SelectedIndex = comboBoxBaud.Items.IndexOf(baud);

            var dataBits = Xml.GetOptionalNodeValue(_setupNode, "DataBits");
            if (string.IsNullOrEmpty(dataBits)) {
                dataBits = "8";
            }
            comboBoxData.SelectedIndex = comboBoxData.Items.IndexOf(dataBits);

            var parity = Xml.GetOptionalNodeValue(_setupNode, "Parity");
            if (string.IsNullOrEmpty(parity)) {
                parity = "None";
            }
            comboBoxParity.SelectedIndex = comboBoxParity.Items.IndexOf(parity);

            var stopBits = Xml.GetOptionalNodeValue(_setupNode, "StopBits");
            if (string.IsNullOrEmpty(stopBits)) {
                stopBits = "One";
            }
            comboBoxStop.SelectedIndex = comboBoxStop.Items.IndexOf(stopBits);

            var header = Xml.GetNodeAlways(_setupNode, "Header");
            if (header.Attributes != null && header.Attributes["checked"] != null) {
                checkBoxHeader.Checked = bool.Parse(header.Attributes["checked"].Value);
            }
            textBoxHeader.Text = header.InnerText;

            var footer = Xml.GetNodeAlways(_setupNode, "Footer");
            if (footer.Attributes != null && footer.Attributes["checked"] != null) {
                checkBoxFooter.Checked = bool.Parse(footer.Attributes["checked"].Value);
            }
            textBoxFooter.Text = footer.InnerText;
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            Xml.SetValue(_setupNode, "Name", comboBoxPort.Text);
            Xml.SetValue(_setupNode, "Baud", comboBoxBaud.Text.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(_setupNode, "DataBits", comboBoxData.Text);
            Xml.SetValue(_setupNode, "Parity", comboBoxParity.Text);
            Xml.SetValue(_setupNode, "StopBits", comboBoxStop.Text);
            Xml.SetAttribute(_setupNode, "Header", "checked", checkBoxHeader.Checked.ToString()).InnerText = textBoxHeader.Text;
            Xml.SetAttribute(_setupNode, "Footer", "checked", checkBoxFooter.Checked.ToString()).InnerText = textBoxFooter.Text;
        }
    }
}
