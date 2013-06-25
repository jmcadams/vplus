using System;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;

using Properties;

using VixenPlus;

namespace GenericSerial {
    internal partial class DialogSerialSetup : Form {

        private const int BaudRate115200 = 8;

        private readonly XmlNode _setupNode;

        public DialogSerialSetup(XmlNode setupNode) {
            InitializeComponent();
            // ReSharper disable CoVariantArrayConversion
            cbPort.Items.AddRange(SerialPort.GetPortNames());
            // ReSharper restore CoVariantArrayConversion

            _setupNode = setupNode;

            var portName = Xml.GetNodeAlways(_setupNode, "Name").InnerText;
            cbPort.SelectedIndex = cbPort.Items.IndexOf((portName.Length > 3) ? portName : "COM1");

            var baud = Xml.GetNodeAlways(_setupNode, "Baud").InnerText;
            cbBaud.SelectedIndex = cbBaud.Items.IndexOf(baud);

            var dataBits = Xml.GetOptionalNodeValue(_setupNode, "DataBits");
            if (string.IsNullOrEmpty(dataBits)) {
                dataBits = "8";
            }
            cdDataBits.SelectedIndex = cdDataBits.Items.IndexOf(dataBits);

            var parity = Xml.GetOptionalNodeValue(_setupNode, "Parity");
            if (string.IsNullOrEmpty(parity)) {
                parity = "None";
            }
            cbParity.SelectedIndex = cbParity.Items.IndexOf(parity);

            var stopBits = Xml.GetOptionalNodeValue(_setupNode, "StopBits");
            if (string.IsNullOrEmpty(stopBits)) {
                stopBits = "One";
            }
            cbStopBits.SelectedIndex = cbStopBits.Items.IndexOf(stopBits);

            var header = Xml.GetNodeAlways(_setupNode, "Header");
            if (header.Attributes != null && header.Attributes["checked"] != null) {
                cbHeader.Checked = bool.Parse(header.Attributes["checked"].Value);
            }
            tbHeader.Text = header.InnerText;

            var footer = Xml.GetNodeAlways(_setupNode, "Footer");
            if (footer.Attributes != null && footer.Attributes["checked"] != null) {
                cbFooter.Checked = bool.Parse(footer.Attributes["checked"].Value);
            }
            tbFooter.Text = footer.InnerText;
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            Xml.SetValue(_setupNode, "Name", cbPort.Text);
            Xml.SetValue(_setupNode, "Baud", cbBaud.Text.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(_setupNode, "DataBits", cdDataBits.Text);
            Xml.SetValue(_setupNode, "Parity", cbParity.Text);
            Xml.SetValue(_setupNode, "StopBits", cbStopBits.Text);
            Xml.SetAttribute(_setupNode, "Header", "checked", cbHeader.Checked.ToString()).InnerText = tbHeader.Text;
            Xml.SetAttribute(_setupNode, "Footer", "checked", cbFooter.Checked.ToString()).InnerText = tbFooter.Text;
        }

        private void cbBaud_SelectedIndexChanged(object sender, EventArgs e) {
            lblWarn.Text = cbBaud.SelectedIndex > BaudRate115200 ? Resources.HighBaudRateSupport : "";
        }
    }
}
