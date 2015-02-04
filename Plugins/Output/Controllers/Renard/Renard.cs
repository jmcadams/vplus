using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Controllers.Common;

using VixenPlus;
using VixenPlus.Annotations;

namespace Controllers.Renard {
    [UsedImplicitly]
    public class Renard : IEventDrivenOutputPlugIn {
        private byte[] _channelValues;
        private AutoResetEvent _eventTrigger;
        private bool _holdPort;
        private bool _isValidPort;
        private SetupDialog _dialog;
        private SerialPort _serialPort;
        private SetupData _setupData;
        private XmlNode _setupNode;
        private RunState _state = RunState.Stopped;

        private const byte ReplacementValue = 0x7c;
        private const byte PacketIgnoreValue = 0x7d;
        private const byte StreamStartValue = 0x7e;
        private const byte PacketEndValue = 0x7f;
        private const byte PacketStartValue = 0x80;

        private const string PortNode = "name";
        private const string BaudNode = "baud";
        private const string ParityNode = "partity";
        private const string DataNode = "data";
        private const string StopNode = "stop";
        private const string HoldNode = "HoldPort";

        public void Event(byte[] channelValues) {
            _channelValues = channelValues;
            if (_holdPort) {
                _eventTrigger.Set();
            }
            else {
                if (!_serialPort.IsOpen) {
                    _serialPort.Open();
                }
                FireEvent();
                _serialPort.Close();
            }
        }


        private void EventThread() {
            State = RunState.Running;
            _eventTrigger = new AutoResetEvent(false);
            try {
                while (State == RunState.Running) {
                    _eventTrigger.WaitOne();
                    try {
                        FireEvent();
                    }
                    catch (TimeoutException) {}
                }
            }
            catch {
                if (State == RunState.Running) {
                    State = RunState.Stopping;
                }
            }
            finally {
                State = RunState.Stopped;
            }
        }


        private void FireEvent() {
            if (State != RunState.Running) {
                return;
            }

            DoEvent(_channelValues);
        }


        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
            _setupData = setupData;
            _setupNode = setupNode;
            _serialPort = new SerialPort(_setupData.GetString(_setupNode, PortNode, "COM1"), _setupData.GetInteger(_setupNode, BaudNode, 19200),
                (Parity) Enum.Parse(typeof (Parity), _setupData.GetString(_setupNode, ParityNode, Parity.None.ToString())),
                _setupData.GetInteger(_setupNode, DataNode, 8),
                (StopBits) Enum.Parse(typeof (StopBits), _setupData.GetString(_setupNode, StopNode, StopBits.One.ToString())));
            _holdPort = _setupData.GetBoolean(_setupNode, HoldNode, true);
            _serialPort.WriteTimeout = 500;
        }


        private readonly List<byte> _packet = new List<byte>();

        private void DoEvent(IEnumerable<byte> channelValues) {
            if (!_isValidPort) return;

            _packet.Clear();
            _packet.Add(StreamStartValue);
            _packet.Add(PacketStartValue);
            foreach (var c in channelValues) {
                switch (c) {
                    case PacketIgnoreValue:
                    case StreamStartValue:
                        _packet.Add(ReplacementValue);
                        break;
                    case PacketEndValue:
                        _packet.Add(PacketStartValue);
                        break;
                    default:
                        _packet.Add(c);
                        break;
                }
                if ((_packet.Count % 100) == 0) {
                    _packet.Add(PacketIgnoreValue);
                }
            }

            _serialPort.Write(_packet.ToArray(), 0, _packet.Count);
        }


        public Control Setup() {
            return _dialog ?? (_dialog = new SetupDialog {SelectedPort = _serialPort});
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
            AppendChild(HoldNode, _holdPort.ToString());
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


        public void Shutdown() {
            if (State != RunState.Running) {
                return;
            }

            State = RunState.Stopping;
            while (State != RunState.Stopped) {
                Thread.Sleep(5);//todo replace with Task.Delay() when using 4.5
            }
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
        }


        public void Startup() {
            _isValidPort = SerialPort.GetPortNames().Contains(_serialPort.PortName);
            if (_isValidPort) {
                if (!(!_holdPort || _serialPort.IsOpen)) {
                    _serialPort.Open();
                }
                _serialPort.Handshake = Handshake.None;
                _serialPort.Encoding = Encoding.UTF8;
                _serialPort.RtsEnable = true;
                _serialPort.DtrEnable = true;
                if (!_holdPort) {
                    return;
                }
            }
            else {
                MessageBox.Show(String.Format("{0} does not exist for {1}", _serialPort.PortName, Name));
            }

            new Thread(EventThread).Start();
            while (State != RunState.Running) {
                Thread.Sleep(1); //todo replace with Task.Delay() when using 4.5
            }
        }


        public override string ToString() {
            return Name;
        }


        [UsedImplicitly]
        public bool SupportsLiveSetup() {
            return true;
        }


        public string HardwareMap {
            get {
                int port;
                return int.TryParse(_serialPort.PortName.Substring(3), out port) 
                    ? String.Format("Serial: {0}, {1}, {2}, {3}, {4}",
                        _serialPort.PortName, 
                        _serialPort.BaudRate, 
                        _serialPort.DataBits,
                        _serialPort.Parity,
                        _serialPort.StopBits) 
                    : null;
            }
        }

        public string Name {
            get { return "Renard Dimmer (modified)"; }
        }

        private RunState State {
            get { return _state; }
            set {
                _state = value;
                if (value != RunState.Stopping) {
                    return;
                }
                _eventTrigger.Set();
                _eventTrigger.Close();
                _eventTrigger = null;
            }
        }

        private enum RunState {
            Running,
            Stopping,
            Stopped
        }
    }
}
