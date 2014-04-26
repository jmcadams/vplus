using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
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
        private byte[] _p1Packet = new byte[1];
        private Control _dialog;
        private SerialPort _serialPort;
        private SetupData _setupData;
        private XmlNode _setupNode;
        private RunState _state = RunState.Stopped;

        private const int ProtocolHeaderSize = 2;
        private const byte ReplacementValue = 0x7c;
        private const byte PacketIgnoreValue = 0x7d;
        private const byte StreamStartValue = 0x7e;
        private const byte PacketEndValue = 0x7f;
        private const byte PacketStartValue = 0x80;

        public Renard() {
            _p1Packet[0] = StreamStartValue;
        }


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
            _serialPort = new SerialPort(_setupData.GetString(_setupNode, "name", "COM1"),
                                           _setupData.GetInteger(_setupNode, "baud", 19200),
                                           (Parity)
                                           Enum.Parse(typeof (Parity),
                                                      _setupData.GetString(_setupNode, "parity", Parity.None.ToString())),
                                           _setupData.GetInteger(_setupNode, "data", 8),
                                           (StopBits)
                                           Enum.Parse(typeof (StopBits),
                                                      _setupData.GetString(_setupNode, "stop", StopBits.One.ToString())));
            _holdPort = _setupData.GetBoolean(_setupNode, "HoldPort", true);
            _serialPort.WriteTimeout = 500;
        }


        private void DoEvent(IList<byte> channelValues) {
            var length = channelValues.Count;
            var count = ProtocolHeaderSize;
            var desiredPacketLength = (ProtocolHeaderSize + length);
            desiredPacketLength += (desiredPacketLength / 100);
            if (_p1Packet.Length < desiredPacketLength) {
                _p1Packet = new byte[desiredPacketLength];
            }
            _p1Packet[0] = StreamStartValue;
            _p1Packet[1] = PacketStartValue;
            for (var i = 0; i < length; i++) {
                switch (channelValues[i]) {
                    case PacketIgnoreValue:
                        _p1Packet[count++] = ReplacementValue;
                        break;
                    case StreamStartValue:
                        _p1Packet[count++] = ReplacementValue;
                        break;
                    case PacketEndValue:
                        _p1Packet[count++] = PacketStartValue;
                        break;
                    default:
                        _p1Packet[count++] = channelValues[i];
                        break;
                }
                if ((count%100) == 0) {
                    _p1Packet[count++] = PacketIgnoreValue;
                }
            }
            while ((_serialPort.WriteBufferSize - _serialPort.BytesToWrite) <= count) {
                Thread.Sleep(5);
            }
            _serialPort.Write(_p1Packet, 0, count);
        }


        public Control Setup() {
            return _dialog ?? (_dialog = new SetupDialog {SelectedPort = _serialPort});
        }


        public void GetSetup() {
            if (null != _dialog) {
                _serialPort = ((SetupDialog) _dialog).SelectedPort;
            }

            while (_setupNode.ChildNodes.Count > 0) {
                _setupNode.RemoveChild(_setupNode.ChildNodes[0]);
            }

            AppendChild("name", _serialPort.PortName);
            AppendChild("baud", _serialPort.BaudRate.ToString(CultureInfo.InvariantCulture));
            AppendChild("parity", _serialPort.Parity.ToString());
            AppendChild("data", _serialPort.DataBits.ToString(CultureInfo.InvariantCulture));
            AppendChild("stop", _serialPort.StopBits.ToString());
            AppendChild("HoldPort", _holdPort.ToString());
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
                Thread.Sleep(5);
            }
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
        }


        public void Startup() {
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

            new Thread(EventThread).Start();
            while (State != RunState.Running) {
                Thread.Sleep(1);
            }
        }


        public override string ToString() {
            return Name;
        }


        [UsedImplicitly]
        public bool SupportsLiveSetup() {
            return true;
        }


        public bool SettingsValid() {
            return ((SetupDialog) _dialog).ValidateSettings();
        }


        public HardwareMap[] HardwareMap {
            get {
                int port;
                return int.TryParse(_serialPort.PortName.Substring(3), out port) 
                    ? new[] {new HardwareMap(String.Format("Serial: {0}, {1}, {2}, {3}, {4}",
                        _serialPort.PortName, 
                        _serialPort.BaudRate, 
                        _serialPort.DataBits,
                        _serialPort.Parity,
                        _serialPort.StopBits), port)} 
                    : new[] {new HardwareMap("None", 0)};
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
