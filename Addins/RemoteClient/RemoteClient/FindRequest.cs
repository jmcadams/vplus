namespace RemoteClient
{
    using System;

    internal class FindRequest
    {
        public int responseAddress;
        public int responsePort;

        public FindRequest()
        {
        }

        public FindRequest(byte[] packet)
        {
            this.responseAddress = BitConverter.ToInt32(packet, 0);
            this.responsePort = BitConverter.ToInt32(packet, 4);
        }

        public int SerializeToPacket(byte[] packet)
        {
            BitConverter.GetBytes(this.responseAddress).CopyTo(packet, 0);
            BitConverter.GetBytes(this.responsePort).CopyTo(packet, 4);
            return 8;
        }
    }
}

