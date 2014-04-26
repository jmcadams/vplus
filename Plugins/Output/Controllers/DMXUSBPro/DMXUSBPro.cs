using System;
using System.Globalization;
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
        private Control _dialog;
        private SerialPort _serialPort;
        private SetupData _setupData;
        private XmlNode _setupNode;
        private Widget _widget;

        private const string PortNode = "Name";
        private const string BaudNode = "Baud";
        private const string ParityNode = "Partity";
        private const string DataNode = "Data";
        private const string StopNode = "Stop";

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


        private void SetPort() {
            if ((_serialPort != null) && _serialPort.IsOpen) {
                _serialPort.Close();
            }
            _serialPort = new SerialPort(_setupData.GetString(_setupNode, PortNode, "COM1"), _setupData.GetInteger(_setupNode, BaudNode, 19200),
                (Parity)Enum.Parse(typeof(Parity), _setupData.GetString(_setupNode, ParityNode, Parity.None.ToString())),
                _setupData.GetInteger(_setupNode, DataNode, 8),
                (StopBits)Enum.Parse(typeof(StopBits), _setupData.GetString(_setupNode, StopNode, StopBits.One.ToString())))
                    {Handshake = Handshake.None, Encoding = Encoding.UTF8};
        }


        public Control Setup() {
            return _dialog ?? (_dialog = new SetupDialog { SelectedPort = _serialPort });

            //using (var dialog = new SerialSetupDialog(_serialPort)) {
            //    dialog.Show();
            //    //if (dialog.ShowDialog() != DialogResult.OK) {
            //    //    return;
            //    //}
            //    _serialPort = dialog.SelectedPort;
            //    _serialPort.Handshake = Handshake.None;
            //    _serialPort.Encoding = Encoding.UTF8;
            //    _setupData.SetString(_setupNode, "Name", _serialPort.PortName);
            //    _setupData.SetInteger(_setupNode, "Baud", _serialPort.BaudRate);
            //    _setupData.SetString(_setupNode, "Parity", _serialPort.Parity.ToString());
            //    _setupData.SetInteger(_setupNode, "Data", _serialPort.DataBits);
            //    _setupData.SetString(_setupNode, "Stop", _serialPort.StopBits.ToString());
            //}

            //return null;
        }


        public void GetSetup() {
            if (null != _dialog) {
                _serialPort = ((SetupDialog)_dialog).SelectedPort;
            }

            while (_setupNode.ChildNodes.Count > 0) {
                _setupNode.RemoveChild(_setupNode.ChildNodes[0]);
            }

            AppendChild(PortNode, _serialPort.PortName);
            AppendChild(BaudNode, _serialPort.BaudRate.ToString(CultureInfo.InvariantCulture));
            AppendChild(ParityNode, _serialPort.Parity.ToString());
            AppendChild(DataNode, _serialPort.DataBits.ToString(CultureInfo.InvariantCulture));
            AppendChild(StopNode, _serialPort.StopBits.ToString());
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
            get {
                int port;
                return int.TryParse(_serialPort.PortName.Substring(3), out port)
                    ? new[] {new HardwareMap(String.Format("Serial: {0}, {1}, {2}, {3}, {4}",
                        _serialPort.PortName, 
                        _serialPort.BaudRate, 
                        _serialPort.DataBits,
                        _serialPort.Parity,
                        _serialPort.StopBits), port)}
                    : new[] { new HardwareMap("None", 0) };
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

