using System.Net.Sockets;

namespace VixenPlus
{
    public interface IObjectPacket
    {
        int PacketBytesRequired { get; }
        void CopyFrom(Socket socket);
        void CopyTo(byte[] array, int startIndex);
    }
}