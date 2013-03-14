namespace RemoteClient
{
    using System;

    internal class FindResult
    {
        public int resultAddress;
        public int resultPort;

        public FindResult()
        {
        }

        public FindResult(byte[] packet)
        {
            this.resultAddress = BitConverter.ToInt32(packet, 0);
            this.resultPort = BitConverter.ToInt32(packet, 4);
        }

        public int SerializeToPacket(byte[] packet)
        {
            BitConverter.GetBytes(this.resultAddress).CopyTo(packet, 0);
            BitConverter.GetBytes(this.resultPort).CopyTo(packet, 4);
            return 8;
        }
    }
}

