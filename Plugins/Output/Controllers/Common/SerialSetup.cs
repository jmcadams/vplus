using System;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Controllers.Properties;

using VixenPlusCommon;

//TODO want to add events to these, so when they change, we can update the live setup screen.
namespace Controllers.Common {
    public partial class SerialSetup : UserControl {

        private const int BaudRateWarningLevel = 6;

        private const string DefaultComPort = "COM1";
        private const int DefaultBaudRate = 57600;
        private const int DefaultDataBits = 8;

        private SerialPort _serialPort;


        public SerialSetup() {
            components = null;
            _serialPort = null;
            InitializeComponent();
        }

        public SerialPort SelectedPorts {
            get {
                return DesignMode ? null : new SerialPort(cbPortName.SelectedItem.ToString(), int.Parse(cbBaudRate.SelectedItem.ToString()),
                                      (Parity)cbParity.SelectedItem, int.Parse(cbDataBits.SelectedItem.ToString()), (StopBits)cbStopBits.SelectedItem);
            }
            set { _serialPort = value; }
        }


        public bool ValidateSettings() {
            var builder = new StringBuilder();
            if (cbPortName.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_PortError);
            }
            if (cbBaudRate.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_BaudError);
            }
            if (cbParity.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_ParityError);
            }
            int result;
            if (!int.TryParse(cbDataBits.SelectedItem.ToString(), out result)) {
                builder.AppendLine(Resources.Serial_DataBitsError);
            }
            if (cbStopBits.SelectedIndex == -1) {
                builder.AppendLine(Resources.Serial_StopBitsError);
            }
            if (builder.Length <= 0) {
                return true;
            }

            MessageBox.Show(Resources.Serial_Resolve + builder, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

            return false;
        }



        private void Init(SerialPort serialPort) {
            if (serialPort == null) {
                serialPort = new SerialPort(DefaultComPort, DefaultBaudRate, Parity.None, DefaultDataBits, StopBits.One);
            }

            cbPortName.Items.Add("None");

            if (!SerialPort.GetPortNames().Contains(serialPort.PortName)) {
                cbPortName.Items.Add(serialPort.PortName);
            }

            cbPortName.Items.AddRange(SerialPort.GetPortNames());

            cbParity.Items.Add(Parity.Even);
            cbParity.Items.Add(Parity.Mark);
            cbParity.Items.Add(Parity.None);
            cbParity.Items.Add(Parity.Odd);
            cbParity.Items.Add(Parity.Space);

            cbStopBits.Items.Add(StopBits.One);
            cbStopBits.Items.Add(StopBits.OnePointFive);
            cbStopBits.Items.Add(StopBits.Two);

            cbPortName.SelectedIndex = cbPortName.Items.IndexOf(serialPort.PortName);
            cbBaudRate.SelectedItem = serialPort.BaudRate.ToString(CultureInfo.InvariantCulture);
            cbParity.SelectedItem = serialPort.Parity;
            cbDataBits.SelectedItem = serialPort.DataBits.ToString(CultureInfo.InvariantCulture);
            cbStopBits.SelectedItem = serialPort.StopBits;
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e) {
            // If baud rate is > BaudRateWarningLevel, warn that it may not work.
            lblWarn.Text = cbBaudRate.SelectedIndex > BaudRateWarningLevel ? Resources.HighBaudRateSupport : "";
        }

        private void SerialSetup_Load(object sender, EventArgs e) {
            if (DesignMode) return;
            Init(_serialPort);
        }
    }
}
