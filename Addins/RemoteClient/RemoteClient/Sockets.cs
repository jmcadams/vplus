namespace RemoteClient
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class Sockets
    {
        private const int TIMEOUT = 0x7d0;

        public static TcpClient ConnectTo(IPAddress[] hostAddresses, int port)
        {
            TcpClient client = new TcpClient();
            client.SendTimeout = 0x7d0;
            client.ReceiveTimeout = 0x7d0;
            try
            {
                client.Connect(hostAddresses, port);
            }
            catch
            {
                throw new Exception("Timeout trying to connect to multiple hosts");
            }
            return client;
        }

        public static TcpClient ConnectTo(IPAddress hostAddress, int port)
        {
            TcpClient client = new TcpClient();
            client.SendTimeout = 0x7d0;
            client.ReceiveTimeout = 0x7d0;
            try
            {
                client.Connect(hostAddress, port);
            }
            catch
            {
                throw new Exception(string.Format("Timeout trying to connect to the host ({0})", hostAddress.ToString()));
            }
            return client;
        }

        public static IPAddress GetIPV4Address()
        {
            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            throw new Exception("Could not find an IPV4 address to bind to.");
        }

        public static IPAddress GetIPV4AddressFor(string host)
        {
            foreach (IPAddress address in Dns.GetHostAddresses(host))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            throw new Exception("Could not find an IPV4 address to bind to.");
        }

        public static byte GetSocketByte(Socket socket)
        {
            return GetSocketBytes(socket, 1)[0];
        }

        public static byte[] GetSocketBytes(Socket socket, int byteCount)
        {
            int offset = 0;
            byte[] buffer = new byte[byteCount];
            while (offset < byteCount)
            {
                offset += socket.Receive(buffer, offset, byteCount - offset, SocketFlags.None);
            }
            return buffer;
        }

        public static int GetSocketInt16(Socket socket)
        {
            return BitConverter.ToInt16(GetSocketBytes(socket, 2), 0);
        }

        public static int GetSocketInt32(Socket socket)
        {
            return BitConverter.ToInt32(GetSocketBytes(socket, 4), 0);
        }

        public static string GetSocketString(Socket socket)
        {
            int byteCount = GetSocketBytes(socket, 1)[0];
            return Encoding.ASCII.GetString(GetSocketBytes(socket, byteCount));
        }

        public static void SendSocketInt16(Socket socket, short value)
        {
            socket.Send(BitConverter.GetBytes(value));
        }

        public static void SendSocketInt32(Socket socket, int value)
        {
            socket.Send(BitConverter.GetBytes(value));
        }

        public static void SendSocketString(Socket socket, string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            byte[] array = new byte[bytes.Length + 1];
            array[0] = (byte) bytes.Length;
            bytes.CopyTo(array, 1);
            socket.Send(array);
        }
    }
}

