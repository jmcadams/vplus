using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

namespace Controllers.GenericSerial {
    [UsedImplicitly]
    public class GenericSerial : IEventDrivenOutputPlugIn {
        private byte[] _footer;
        private byte[] _header;
        private byte[] _packet;

        private Control _dialog;

        private SerialPort _serialPort;
        private SetupData _setupData;
        private XmlNode _setupNode;

        private const string PortNode = "name";
        private const string BaudNode = "baud";
        private const string ParityNode = "partity";
        private const string DataNode = "data";
        private const string StopNode = "stop";

        public void Event(byte[] channelValues) {
            channelValues.CopyTo(_packet, _header.Length);
            _serialPort.Write(_packet, 0, _packet.Length);
        }


        private byte[] GetBytes(string nodeName) {
            var nodeAlways = Xml.GetNodeAlways(_setupNode, nodeName);
            if (nodeAlways.Attributes != null &&
                ((nodeAlways.Attributes["checked"] != null) && (nodeAlways.Attributes["checked"].Value == bool.TrueString))) {
                return Encoding.ASCII.GetBytes(nodeAlways.InnerText);
            }
            return new byte[0];
        }


        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
            _setupData = setupData;
            _setupNode = setupNode;
            SetPort();
            _packet = new byte[(_header.Length + executableObject.Channels.Count) + _footer.Length];
            _header.CopyTo(_packet, 0);
            _footer.CopyTo(_packet, _packet.Length - _footer.Length);
        }


        private void SetPort() {
            _serialPort = new SerialPort {
                PortName = _setupData.GetString(_setupNode, "Name", "COM1"), BaudRate = _setupData.GetInteger(_setupNode, "Baud", 57600),
                DataBits = _setupData.GetInteger(_setupNode, "DataBits", 8),
                Parity = (Parity)Enum.Parse(typeof(Parity), _setupData.GetString(_setupNode, "Parity", Parity.None.ToString())),
                StopBits = (StopBits) Enum.Parse(typeof (StopBits), _setupData.GetString(_setupNode, "StopBits", StopBits.One.ToString())),
                Handshake = Handshake.None, Encoding = Encoding.UTF8
            };
            _header = GetBytes("Header");
            _footer = GetBytes("Footer");
        }


        public Control Setup() {
            return _dialog ?? (_dialog = new GenericSerialSetupDialog { SelectedPort = _serialPort });

            //using (var dialog = new DialogSerialSetup(_setupNode)) {
            //    if (dialog.ShowDialog() == DialogResult.OK) {
            //        SetPort();
            //    }
            //}

            //return null;
        }


        public void GetSetup() {
        }


        public void CloseSetup() {
            throw new NotImplementedException();
        }


        public bool SupportsLiveSetup() {
            return false;
        }


        public bool SettingsValid() {
            throw new NotImplementedException();
        }


        public void Shutdown() {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
        }


        public void Startup() {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
            _serialPort.Open();
        }


        public override string ToString() {
            return Name;
        }


        [UsedImplicitly]
        public string Author {
            get { return "Vixen and VixenPlus Developers"; }
        }

        [UsedImplicitly]
        public string Description {
            get { return "Generic serial output plugin"; }
        }

        public HardwareMap[] HardwareMap {
            get { return new[] {new HardwareMap("Serial", int.Parse(_serialPort.PortName.Substring(3)))}; }
        }

        public string Name {
            get { return "Generic serial"; }
        }
    }
}