using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Controllers.Common;

using VixenPlus;
using VixenPlus.Annotations;

namespace Controllers.DMXUSBPro
{
    [UsedImplicitly]
    public class DmxusbPro : IEventDrivenOutputPlugIn
    {
        private SerialPort _serialPort;
        private SetupData _setupData;
        private XmlNode _setupNode;
        private Widget _widget;

        public void Event(byte[] channelValues)
        {
            _widget.OutputDMXPacket(channelValues);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            _setupNode = setupNode;
            _setupData = setupData;
            SetPort();
        }

        private void SetPort()
        {
            if ((_serialPort != null) && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _serialPort = new SerialPort(_setupData.GetString(_setupNode, "Name", "COM1"), _setupData.GetInteger(_setupNode, "Baud", 0xe100), (Parity) Enum.Parse(typeof(Parity), _setupData.GetString(_setupNode, "Parity", "None")), _setupData.GetInteger(_setupNode, "Data", 8), (StopBits) Enum.Parse(typeof(StopBits), _setupData.GetString(_setupNode, "Stop", "One")))
            {Handshake = Handshake.None, Encoding = Encoding.UTF8};
        }


        public Control Setup() {
            using (var dialog = new SerialSetupDialog(_serialPort)) {
                dialog.Show();
                //if (dialog.ShowDialog() != DialogResult.OK) {
                //    return;
                //}
                _serialPort = dialog.SelectedPort;
                _serialPort.Handshake = Handshake.None;
                _serialPort.Encoding = Encoding.UTF8;
                _setupData.SetString(_setupNode, "Name", _serialPort.PortName);
                _setupData.SetInteger(_setupNode, "Baud", _serialPort.BaudRate);
                _setupData.SetString(_setupNode, "Parity", _serialPort.Parity.ToString());
                _setupData.SetInteger(_setupNode, "Data", _serialPort.DataBits);
                _setupData.SetString(_setupNode, "Stop", _serialPort.StopBits.ToString());
            }

            return null;
        }


        public SetupData GetSetup() {
            throw new NotImplementedException();
        }


        public void CloseSetup() {
            throw new NotImplementedException();
        }


        public bool SupportsPreview() {
            return false;
        }


        public bool ValidateSettings() {
            throw new NotImplementedException();
        }


        public void Shutdown()
        {
            _widget.Stop();
        }

        public void Startup()
        {
            if (_widget != null)
            {
                _widget.Dispose();
            }
            _widget = new Widget(_serialPort);
            _widget.Start();
        }

        public override string ToString()
        {
            return Name;
        }


        [UsedImplicitly]
        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        [UsedImplicitly]
        public string Description
        {
            get
            {
                return Name;
            }
        }

        public HardwareMap[] HardwareMap
        {
            get
            {
                return new[] { new HardwareMap("Serial", int.Parse(_serialPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "Enttec DMX USB Pro";
            }
        }
    }
}

