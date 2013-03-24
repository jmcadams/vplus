namespace VixenPlus
{
    using System;
    using System.Net.Sockets;

    public interface ObjectPacket
    {
        void CopyFrom(Socket socket);
        void CopyTo(byte[] array, int startIndex);

        int PacketBytesRequired { get; }
    }
}

