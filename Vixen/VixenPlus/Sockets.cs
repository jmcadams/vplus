using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Vixen
{
	public class Sockets
	{
		public static TcpClient ConnectTo(IPAddress[] hostAddresses, int port)
		{
			var client = new TcpClient();
			client.SendTimeout = 0x1388;
			client.ReceiveTimeout = 0x1388;
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
			var client = new TcpClient();
			client.SendTimeout = 0x1388;
			client.ReceiveTimeout = 0x1388;
			try
			{
				client.Connect(hostAddress, port);
			}
			catch
			{
				throw new Exception(string.Format("Timeout trying to connect to the host ({0})", hostAddress));
			}
			return client;
		}

		public static byte GetSocketByte(Socket socket)
		{
			return GetSocketBytes(socket, 1)[0];
		}

		public static byte[] GetSocketBytes(Socket socket, int byteCount)
		{
			int num;
			if (byteCount == 0)
			{
				return new byte[0];
			}
			int offset = 0;
			var buffer = new byte[byteCount];
			do
			{
				num = socket.Receive(buffer, offset, byteCount - offset, SocketFlags.None);
				offset += num;
			} while ((offset < byteCount) && (num > 0));
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

		public static string GetSocketLongString(Socket socket)
		{
			int byteCount = GetSocketInt32(socket);
			return Encoding.ASCII.GetString(GetSocketBytes(socket, byteCount));
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

		public static void SendSocketLongString(Socket socket, string str)
		{
			SendSocketInt32(socket, str.Length);
			socket.Send(Encoding.ASCII.GetBytes(str));
		}

		public static void SendSocketString(Socket socket, string str)
		{
			var array = new byte[str.Length + 1];
			array[0] = (byte) str.Length;
			Encoding.ASCII.GetBytes(str).CopyTo(array, 1);
			socket.Send(array);
		}
	}
}