using VixenPlus;

namespace DMXUSBPro
{
    using System;
    using System.IO;
    using System.IO.Ports;

    internal class Widget : IDisposable
    {
        private readonly string _logFile = Path.Combine(Paths.DataPath, "dmxusbpro.log");
        private readonly Message _dmxPacketMessage;
        private readonly SerialPort _serialPort;
        private readonly byte[] _statePacket;

        public Widget(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _statePacket = new byte[0x201];
            _dmxPacketMessage = new Message(MessageType.OutputOnlySendDMXPacketRequest) {Data = _statePacket};
        }

        public void Dispose()
        {
            if ((_serialPort != null) && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            GC.SuppressFinalize(this);
        }

        ~Widget()
        {
            Dispose();
        }


        public void OutputDMXPacket(byte[] channelValues) {
            if (_serialPort == null) {
                File.AppendAllText(_logFile, @"Port reference is null\n");
            }
            if (_statePacket == null) {
                File.AppendAllText(_logFile, @"State packet is null\n");
            }
            if (_dmxPacketMessage == null) {
                File.AppendAllText(_logFile, @"Packet message is null\n");
            }
            else if (_dmxPacketMessage.Packet == null) {
                File.AppendAllText(_logFile, @"Packet message : packet is null\n");
            }
            if (_serialPort != null && !_serialPort.IsOpen) {
                _serialPort.Open();
            }
            if (_statePacket != null) {
                _statePacket[0] = 0;
                Array.Copy(channelValues, 0, _statePacket, 1, Math.Min(0x200, channelValues.Length));
            }
            if (_dmxPacketMessage == null) {
                return;
            }

            var packet = _dmxPacketMessage.Packet;
            if (_serialPort != null && packet != null) {
                _serialPort.Write(packet, 0, packet.Length);
            }
        }


        public void Start()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Stop()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
    }
}

