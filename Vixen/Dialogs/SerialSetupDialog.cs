using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

using VixenPlus.Properties;

namespace Dialogs {
    public partial class SerialSetupDialog : Form {

        private const int BaudRate115200 = 6;

        public SerialSetupDialog(SerialPort serialPort) {
            components = null;
            InitializeComponent();
            comboBoxPortName.Items.AddRange(SerialPort.GetPortNames());
            Init(serialPort);
        }

        public SerialPort SelectedPort {
            get {
                return new SerialPort(comboBoxPortName.SelectedItem.ToString(), int.Parse(comboBoxBaudRate.SelectedItem.ToString()),
                                      (Parity) comboBoxParity.SelectedItem, int.Parse(textBoxData.Text), (StopBits) comboBoxStop.SelectedItem);
            }
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.None;
            var builder = new StringBuilder();
            if (comboBoxPortName.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_PortError);
            }
            if (comboBoxBaudRate.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_BaudError);
            }
            if (comboBoxParity.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_ParityError);
            }
            int result;
            if (!int.TryParse(textBoxData.Text, out result)) {
                builder.AppendLine(Resources.Serial_DataBitsError);
            }
            if (comboBoxStop.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_StopBitsError);
            }
            if (builder.Length > 0) {
                MessageBox.Show(Resources.Serial_Resolve + builder, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                DialogResult = DialogResult.OK;
            }
        }


        private void Init(SerialPort serialPort) {
            comboBoxParity.Items.Add(Parity.Even);
            comboBoxParity.Items.Add(Parity.Mark);
            comboBoxParity.Items.Add(Parity.None);
            comboBoxParity.Items.Add(Parity.Odd);
            comboBoxParity.Items.Add(Parity.Space);
            comboBoxStop.Items.Add(StopBits.One);
            comboBoxStop.Items.Add(StopBits.OnePointFive);
            comboBoxStop.Items.Add(StopBits.Two);
            if (serialPort == null) {
                serialPort = new SerialPort("COM1", 38400, Parity.None, 8, StopBits.One);
            }
            comboBoxPortName.SelectedIndex = comboBoxPortName.Items.IndexOf(serialPort.PortName);
            comboBoxBaudRate.SelectedItem = serialPort.BaudRate.ToString(CultureInfo.InvariantCulture);
            comboBoxParity.SelectedItem = serialPort.Parity;
            textBoxData.Text = serialPort.DataBits.ToString(CultureInfo.InvariantCulture);
            comboBoxStop.SelectedItem = serialPort.StopBits;
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e) {
            // If baud rate is faster than 115200, warn that it may not work.
            lblWarn.Text = comboBoxBaudRate.SelectedIndex > BaudRate115200 ? Resources.HighBaudRateSupport : "";
        }
    }
}
