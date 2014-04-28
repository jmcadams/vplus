using System;
using System.Globalization;
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

        private bool _headerChecked;
        private bool _footerChecked;

        private GenericSerialSetupDialog _dialog;

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


/*
        private byte[] GetBytes(string nodeName) {
            var nodeAlways = Xml.GetNodeAlways(_setupNode, nodeName);
            if (nodeAlways.Attributes != null &&
                ((nodeAlways.Attributes["checked"] != null) && (nodeAlways.Attributes["checked"].Value == bool.TrueString))) {
                return Encoding.ASCII.GetBytes(nodeAlways.InnerText);
            }
            return new byte[0];
        }
*/


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
            var nodeAlways = Xml.GetNodeAlways(_setupNode, "Header");
            _headerChecked = (nodeAlways.Attributes != null &&
                              ((nodeAlways.Attributes["checked"] != null) &&
                               (nodeAlways.Attributes["checked"].Value == bool.TrueString)));

            _header = Encoding.ASCII.GetBytes(nodeAlways.InnerText);

            nodeAlways = Xml.GetNodeAlways(_setupNode, "Footer");

            _footerChecked = (nodeAlways.Attributes != null &&
                              ((nodeAlways.Attributes["checked"] != null) &&
                               (nodeAlways.Attributes["checked"].Value == bool.TrueString)));

            _footer = Encoding.ASCII.GetBytes(nodeAlways.InnerText);

            //_header = GetBytes("Header");
            //_footer = GetBytes("Footer");
        }


        public Control Setup() {
            return _dialog ?? 
                   (_dialog =
                       new GenericSerialSetupDialog {
                           SelectedPort = _serialPort,
                           HeaderEnabled = _headerChecked,
                           HeaderText = Encoding.Default.GetString(_header),
                           FooterEnabled = _footerChecked,
                           FooterText = Encoding.Default.GetString(_footer)
                       });
        }


        public void GetSetup() {
            if (null != _dialog) {
                _serialPort = _dialog.SelectedPort;
            }

            while (_setupNode.ChildNodes.Count > 0) {
                _setupNode.RemoveChild(_setupNode.ChildNodes[0]);
            }

            AppendChild(PortNode, _serialPort.PortName);
            AppendChild(BaudNode, _serialPort.BaudRate.ToString(CultureInfo.InvariantCulture));
            AppendChild(ParityNode, _serialPort.Parity.ToString());
            AppendChild(DataNode, _serialPort.DataBits.ToString(CultureInfo.InvariantCulture));
            AppendChild(StopNode, _serialPort.StopBits.ToString());

            if (null == _dialog) {
                return;
            }

            _headerChecked = _dialog.HeaderEnabled;
            _header = Encoding.ASCII.GetBytes(_dialog.HeaderText);

            _footerChecked = _dialog.FooterEnabled;
            _footer = Encoding.ASCII.GetBytes(_dialog.FooterText);

            Xml.SetAttribute(_setupNode, "Header", "checked", _headerChecked.ToString()).InnerText =
                _dialog.HeaderText;
            Xml.SetAttribute(_setupNode, "Footer", "checked", _footerChecked.ToString()).InnerText =
                _dialog.FooterText;
        }



        private void AppendChild(string key, string value) {
            if (_setupNode.OwnerDocument == null) {
                return;
            }

            var newChild = _setupNode.OwnerDocument.CreateElement(key);
            newChild.InnerXml = value;
            _setupNode.AppendChild(newChild);
        }

        public void CloseSetup() {
            if (_dialog == null) {
                return;
            }

            _dialog.Dispose();
            _dialog = null;
        }


        public bool SupportsLiveSetup() {
            return true;
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
            get {
                int port;
                var map = new StringBuilder();

                if (int.TryParse(_serialPort.PortName.Substring(3), out port)) {

                    map.Append(String.Format("Serial: {0}, {1}, {2}, {3}, {4}", _serialPort.PortName,
                        _serialPort.BaudRate, _serialPort.DataBits, _serialPort.Parity, _serialPort.StopBits));
                    if (_headerChecked) {
                        map.Append("\nHeader: " + Encoding.Default.GetString(_header));
                    }
                    if (_footerChecked) {
                        map.Append("\nFooter: " + Encoding.Default.GetString(_footer));
                    }
                }
                else {
                    map.Append("none");
                }
                
                return new[] {new HardwareMap(map.ToString(), port)};
            }
        }

        public string Name {
            get { return "Generic serial"; }
        }
    }
}