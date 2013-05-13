using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
    public partial class SerialSetupDialog : Form
    {
        public SerialSetupDialog(SerialPort serialPort)
        {
            components = null;
            InitializeComponent();
            comboBoxPortName.Items.AddRange(SerialPort.GetPortNames());
            Init(serialPort);
        }

        public SerialSetupDialog(SerialPort serialPort, bool allowPortEdit, bool allowBaudEdit, bool allowParityEdit,
                                 bool allowDataEdit, bool allowStopEdit)
        {
            components = null;
            InitializeComponent();
            comboBoxPortName.Items.AddRange(SerialPort.GetPortNames());
            comboBoxPortName.Enabled = allowPortEdit;
            comboBoxBaudRate.Enabled = allowBaudEdit;
            comboBoxParity.Enabled = allowParityEdit;
            textBoxData.Enabled = allowDataEdit;
            comboBoxStop.Enabled = allowStopEdit;
            Init(serialPort);
        }

        public SerialPort SelectedPort
        {
            get
            {
                return new SerialPort(comboBoxPortName.SelectedItem.ToString(), int.Parse(comboBoxBaudRate.SelectedItem.ToString()),
                                      (Parity) comboBoxParity.SelectedItem, int.Parse(textBoxData.Text),
                                      (StopBits) comboBoxStop.SelectedItem);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            var builder = new StringBuilder();
            if (comboBoxPortName.SelectedIndex == -1)
            {
                builder.AppendLine("* Port name has not been selected.");
            }
            if (comboBoxBaudRate.SelectedIndex == -1)
            {
                builder.AppendLine("* Baud rate has not been selected.");
            }
            if (comboBoxParity.SelectedIndex == -1)
            {
                builder.AppendLine("* Parity has not been selected.");
            }
            int result;
            if (!int.TryParse(textBoxData.Text, out result))
            {
                builder.AppendLine("* Invalid numeric value for data bits.");
            }
            if (comboBoxStop.SelectedIndex == -1)
            {
                builder.AppendLine("* Stop bits have not been selected.");
            }
            if (builder.Length > 0)
            {
                MessageBox.Show("The following items need to be resolved:\n\n" + builder, Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Hand);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }


        private void Init(SerialPort serialPort)
        {
            comboBoxParity.Items.Add(Parity.Even);
            comboBoxParity.Items.Add(Parity.Mark);
            comboBoxParity.Items.Add(Parity.None);
            comboBoxParity.Items.Add(Parity.Odd);
            comboBoxParity.Items.Add(Parity.Space);
            comboBoxStop.Items.Add(StopBits.None);
            comboBoxStop.Items.Add(StopBits.One);
            comboBoxStop.Items.Add(StopBits.OnePointFive);
            comboBoxStop.Items.Add(StopBits.Two);
            if (serialPort == null)
            {
                serialPort = new SerialPort("COM1", 38400, Parity.None, 8, StopBits.One);
            }
            comboBoxPortName.SelectedIndex = comboBoxPortName.Items.IndexOf(serialPort.PortName);
            comboBoxBaudRate.SelectedItem = serialPort.BaudRate.ToString(CultureInfo.InvariantCulture);
            comboBoxParity.SelectedItem = serialPort.Parity;
            textBoxData.Text = serialPort.DataBits.ToString(CultureInfo.InvariantCulture);
            comboBoxStop.SelectedItem = serialPort.StopBits;
        }
    }
}