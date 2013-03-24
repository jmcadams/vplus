namespace RemoteClient
{
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using System.Runtime.CompilerServices;
	using System.Text;
	using VixenPlus;
	using System.Diagnostics;

	internal class ControlClient
	{
		private int m_clientID = Process.GetCurrentProcess().Id;
		private IPAddress m_serverAddress;
		private string m_serverIPAddrString;
		private string m_serverPassword = string.Empty;

		public event OnServerData ServerData;

		public bool Authenticate(string password)
		{
			this.m_serverPassword = password;
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[2 + password.Length];
			array[0] = 0x15;
			array[1] = (byte) password.Length;
			Encoding.ASCII.GetBytes(password).CopyTo(array, 2);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public bool ClientChannelOff(int clientID, int channel)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[9];
			array[0] = 12;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			BitConverter.GetBytes(channel).CopyTo(array, 5);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public bool ClientChannelOn(int clientID, int channel)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[9];
			array[0] = 11;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			BitConverter.GetBytes(channel).CopyTo(array, 5);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public bool ClientChannelToggle(int clientID, int channel)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[9];
			array[0] = 0x11;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			BitConverter.GetBytes(channel).CopyTo(array, 5);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public bool ClientEcho(int clientID)
		{
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[9];
			array[0] = 0x18;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			BitConverter.GetBytes(this.m_clientID).CopyTo(array, 5);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public ClientStatus ClientStatus(int clientID)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[5];
			array[0] = 0x17;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			serverConnection.Client.Send(array);
			ClientStatus status = null;
			Socket client = serverConnection.Client;
			if (Sockets.GetSocketByte(client) == 0x13)
			{
				status = new ClientStatus();
				status.ExecutionStatus = (ExecutionStatus) Sockets.GetSocketByte(client);
				if (status.ExecutionStatus != ExecutionStatus.None)
				{
					status.ProgramName = Sockets.GetSocketString(client);
					status.ProgramLength = Sockets.GetSocketInt32(client);
					status.SequenceName = Sockets.GetSocketString(client);
					status.SequenceLength = Sockets.GetSocketInt32(client);
					status.SequenceProgress = Sockets.GetSocketInt32(client);
				}
			}
			serverConnection.Close();
			return status;
		}

		public bool Execute(int clientID)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[5];
			array[0] = 2;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			serverConnection.Client.Send(array);
			if (clientID == -1)
			{
				byte[] buffer = new byte[] { 1 };
				while (buffer[0] != 0)
				{
					if (serverConnection.Available > 0)
					{
						serverConnection.Client.Receive(buffer, 1, SocketFlags.None);
						if (this.ServerData != null)
						{
							this.ServerData(buffer[0].ToString() + " clients remaining for synchronization");
						}
					}
				}
				if (this.ServerData != null)
				{
					this.ServerData("Executing");
				}
				return true;
			}
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		private TcpClient GetServerConnection()
		{
			TcpClient client = Sockets.ConnectTo(this.m_serverAddress, 0xa1b9);
			client.LingerState = new LingerOption(true, 5);
			return client;
		}

		public bool Pause(int clientID)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[5];
			array[0] = 4;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public VixenExecutionClientStub[] RequestClientEnumeration()
		{
			TcpClient serverConnection = this.GetServerConnection();
			byte[] buffer = new byte[] { 9 };
			serverConnection.Client.Send(buffer);
			Socket client = serverConnection.Client;
			int num = Sockets.GetSocketInt32(client);
			VixenExecutionClientStub[] stubArray = new VixenExecutionClientStub[num];
			for (int i = 0; i < num; i++)
			{
				stubArray[i] = new VixenExecutionClientStub(Sockets.GetSocketInt32(client), Sockets.GetSocketInt32(client), Sockets.GetSocketString(client));
			}
			serverConnection.Close();
			return stubArray;
		}

		public void SendLoadProgram(string programFileName, string sequenceDirectory)
		{
			TcpClient serverConnection = this.GetServerConnection();
			string fileName = Path.GetFileName(programFileName);
			int length = (int) new FileInfo(programFileName).Length;
			byte[] array = new byte[6 + fileName.Length];
			array[0] = 1;
			BitConverter.GetBytes(length).CopyTo(array, 1);
			array[5] = (byte) fileName.Length;
			Encoding.ASCII.GetBytes(fileName).CopyTo(array, 6);
			serverConnection.Client.Send(array);
			serverConnection.Client.SendFile(programFileName);
			serverConnection.Close();
			SequenceProgram program = new SequenceProgram(programFileName);
			foreach (string str2 in program.EventSequenceFileNames)
			{
				this.SendLoadSequence(Path.Combine(sequenceDirectory, str2), fileName, -1);
			}
		}

		public void SendLoadSequence(string sequenceFileName)
		{
			this.SendLoadSequence(sequenceFileName, string.Empty, -1);
		}

		public void SendLoadSequence(string sequenceFileName, string owningProgramName, int index)
		{
			TcpClient serverConnection = this.GetServerConnection();
			string fileName = Path.GetFileName(sequenceFileName);
			int length = (int) new FileInfo(sequenceFileName).Length;
			byte[] array = new byte[((((2 + owningProgramName.Length) + 1) + 4) + 1) + fileName.Length];
			array[0] = 13;
			BitConverter.GetBytes(owningProgramName.Length).CopyTo(array, 1);
			Encoding.ASCII.GetBytes(owningProgramName).CopyTo(array, 2);
			array[2 + owningProgramName.Length] = (byte) index;
			BitConverter.GetBytes(length).CopyTo(array, (int) ((2 + owningProgramName.Length) + 1));
			array[((2 + owningProgramName.Length) + 1) + 4] = (byte) fileName.Length;
			Encoding.ASCII.GetBytes(fileName).CopyTo(array, (int) ((((2 + owningProgramName.Length) + 1) + 4) + 1));
			serverConnection.Client.Send(array);
			serverConnection.Client.SendFile(sequenceFileName);
			serverConnection.Close();
		}

		public void ShowControlPanel()
		{
			ControlClientUI tui = new ControlClientUI(this);
			tui.ShowDialog();
			tui.Dispose();
		}

		public bool Stop(int clientID)
		{
			if (clientID == 0)
			{
				throw new Exception("Invalid client ID");
			}
			TcpClient serverConnection = this.GetServerConnection();
			byte[] array = new byte[5];
			array[0] = 3;
			BitConverter.GetBytes(clientID).CopyTo(array, 1);
			serverConnection.Client.Send(array);
			byte socketByte = Sockets.GetSocketByte(serverConnection.Client);
			serverConnection.Close();
			return (socketByte == 0x13);
		}

		public string Password
		{
			get
			{
				return this.m_serverPassword;
			}
			set
			{
				this.m_serverPassword = value;
			}
		}

		public string Server
		{
			get
			{
				return this.m_serverIPAddrString;
			}
			set
			{
				if (value.Length != 0)
				{
					if (char.IsDigit(value[0]))
					{
						string str;
						int index = value.IndexOf(':');
						if (index > -1)
						{
							str = value.Substring(0, index);
						}
						else
						{
							str = value;
						}
						this.m_serverIPAddrString = str;
						this.m_serverAddress = IPAddress.Parse(str);
					}
					else
					{
						this.m_serverAddress = Sockets.GetIPV4AddressFor(value);
						this.m_serverIPAddrString = this.m_serverAddress.ToString();
					}
				}
			}
		}

		public delegate void OnServerData(string dataString);
	}
}

