namespace DMXUSBPro
{
    using System;

    internal class Message
    {
        private byte[] _data;
        private ushort _dataLength;
        private readonly MessageType _messageType;

        public Message(MessageType type)
        {
            _messageType = type;
        }

        public byte[] Data
        {
            set
            {
                _data = value;
                _dataLength = (ushort) Math.Min(value.Length, 600);
            }
        }

        public byte[] Packet
        {
            get
            {
                var array = new byte[5 + _dataLength];
                array[0] = 0x7e;
                array[1] = (byte) _messageType;
                array[2] = (byte) _dataLength;
                array[3] = (byte) (_dataLength >> 8);
                array[4 + _dataLength] = 0xe7;
                if (_data != null)
                {
                    _data.CopyTo(array, 4);
                }
                return array;
            }
        }
    }
}

