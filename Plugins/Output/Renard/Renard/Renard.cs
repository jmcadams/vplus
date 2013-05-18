using System.Collections.Generic;

namespace Renard {
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    using VixenPlus;

    public class Renard : IEventDrivenOutputPlugIn {
        private byte[] _channelValues;
        private AutoResetEvent _eventTrigger;
        private bool _holdPort;
        private byte[] _p1Packet = new byte[1];
        private SerialPort _selectedPort;
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
                if (!_selectedPort.IsOpen) {
                    _selectedPort.Open();
                }
                FireEvent();
                _selectedPort.Close();
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
            _selectedPort = new SerialPort(_setupData.GetString(_setupNode, "name", "COM1"),
                                           _setupData.GetInteger(_setupNode, "baud", 19200),
                                           (Parity)
                                           Enum.Parse(typeof (Parity),
                                                      _setupData.GetString(_setupNode, "parity", Parity.None.ToString())),
                                           _setupData.GetInteger(_setupNode, "data", 8),
                                           (StopBits)
                                           Enum.Parse(typeof (StopBits),
                                                      _setupData.GetString(_setupNode, "stop", StopBits.One.ToString())));
            _holdPort = _setupData.GetBoolean(_setupNode, "HoldPort", true);
            _selectedPort.WriteTimeout = 500;
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
            while ((_selectedPort.WriteBufferSize - _selectedPort.BytesToWrite) <= count) {
                Thread.Sleep(5);
            }
            _selectedPort.Write(_p1Packet, 0, count);
        }


        public void Setup() {
            using (var dialog = new SetupDialog(_selectedPort, _holdPort)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _selectedPort = dialog.SelectedPort;
                _setupData.SetString(_setupNode, "name", _selectedPort.PortName);
                _setupData.SetInteger(_setupNode, "baud", _selectedPort.BaudRate);
                _setupData.SetString(_setupNode, "parity", _selectedPort.Parity.ToString());
                _setupData.SetInteger(_setupNode, "data", _selectedPort.DataBits);
                _setupData.SetString(_setupNode, "stop", _selectedPort.StopBits.ToString());
                _holdPort = dialog.HoldPort;
                _setupData.SetBoolean(_setupNode, "HoldPort", _holdPort);
            }
        }


        public void Shutdown() {
            if (State != RunState.Running) {
                return;
            }

            State = RunState.Stopping;
            while (State != RunState.Stopped) {
                Thread.Sleep(5);
            }
            if (_selectedPort.IsOpen) {
                _selectedPort.Close();
            }
        }


        public void Startup() {
            if (!(!_holdPort || _selectedPort.IsOpen)) {
                _selectedPort.Open();
            }
            _selectedPort.Handshake = Handshake.None;
            _selectedPort.Encoding = Encoding.UTF8;
            _selectedPort.RtsEnable = true;
            _selectedPort.DtrEnable = true;
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


        public string Author {
            get { return "Vixen and Vixen+ Developers"; }
        }

        public string Description {
            get { return "Renard Protocol Output"; }
        }

        public HardwareMap[] HardwareMap {
            get { return new[] {new HardwareMap("Serial", int.Parse(_selectedPort.PortName.Substring(3)))}; }
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
