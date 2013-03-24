namespace RemoteClient
{
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;
	using VixenPlus;
	using System.Diagnostics;

	internal class ExecutionClient
	{
		private ClientContext m_clientContext = new ClientContext();
		private int m_clientNaturalID = Process.GetCurrentProcess().Id;
		private int m_clientRegisteredID;
		private TcpListener m_listener = null;
		private Thread m_listenerThread = null;
		private bool m_running = false;
		private IPAddress m_serverAddress = IPAddress.None;
		private string m_serverIPAddrString = string.Empty;

		private void DoChannelToggle(Socket socket)
		{
			if (this.m_clientContext.ExecutionInterface.ExecuteChannelToggle(this.m_clientContext.ExecutionContextHandle, Sockets.GetSocketInt32(socket)))
			{
				socket.Send(new byte[] { 0x13 });
			}
			else
			{
				socket.Send(new byte[] { 20 });
			}
		}

		private void DoClientEcho(Socket socket)
		{
			socket.Send(new byte[] { (Sockets.GetSocketInt32(socket) == this.m_clientNaturalID) ? ((byte) 20) : ((byte) 0x13) });
		}

		private void DoClientStatus(Socket socket)
		{
			socket.Send(new byte[] { 0x13 });
			this.SendExecutionStatus(socket, this.m_clientContext.ExecutionContextHandle);
		}

		private void DoClientTimerCount(Socket socket)
		{
			ISystem system = (ISystem) Interfaces.Available["ISystem"];
			socket.Send(new byte[] { 0x13 });
			socket.Send(new byte[] { (byte) system.ExecutingTimerCount });
		}

		private void DoClientTimerStatus(Socket socket, int timerIndex)
		{
			ISystem system = (ISystem) Interfaces.Available["ISystem"];
			if (timerIndex >= system.ExecutingTimerCount)
			{
				socket.Send(new byte[] { 20 });
			}
			else
			{
				socket.Send(new byte[] { 0x13 });
				this.SendExecutionStatus(socket, system.GetExecutingTimerExecutionContextHandle(timerIndex));
			}
		}

		private void ListenerThread()
		{
			this.m_running = true;
			try
			{
				while (this.m_running)
				{
					if (this.m_listener.Pending())
					{
						Socket socket = this.m_listener.AcceptSocket();
						byte[] buffer = new byte[1];
						socket.Receive(buffer, 1, SocketFlags.None);
						switch (((RequestType) buffer[0]))
						{
							case RequestType.Execute:
								ClientCommandHandlers.Execute(socket, this.m_clientContext, 0, 0);
								break;

							case RequestType.Stop:
								ClientCommandHandlers.Stop(socket, this.m_clientContext);
								break;

							case RequestType.Pause:
								ClientCommandHandlers.Pause(socket, this.m_clientContext);
								break;

							case RequestType.ChannelOn:
								ClientCommandHandlers.ChannelOn(socket, this.m_clientContext, Sockets.GetSocketInt32(socket));
								break;

							case RequestType.ChannelOff:
								ClientCommandHandlers.ChannelOff(socket, this.m_clientContext, Sockets.GetSocketInt32(socket));
								break;

							case RequestType.ListLocal:
								ClientCommandHandlers.ListLocal(socket, (ObjectType) Sockets.GetSocketByte(socket), Sockets.GetSocketString(socket));
								break;

							case RequestType.RetrieveLocal:
								ClientCommandHandlers.RetrieveLocal(socket, this.m_clientContext, (ObjectType) Sockets.GetSocketByte(socket), Sockets.GetSocketString(socket));
								break;

							case RequestType.RetrieveRemote:
								ClientCommandHandlers.RetrieveRemote(socket, this.m_serverAddress, this.m_clientContext, (ObjectType) Sockets.GetSocketByte(socket), Sockets.GetSocketString(socket));
								break;

							case RequestType.ChannelToggle:
								this.DoChannelToggle(socket);
								break;

							case RequestType.ClientStatus:
								this.DoClientStatus(socket);
								break;

							case RequestType.ClientEcho:
								this.DoClientEcho(socket);
								break;

							case RequestType.ChannelCount:
								ClientCommandHandlers.ChannelCount(socket, this.m_clientContext);
								break;

							case RequestType.TimerCount:
								this.DoClientTimerCount(socket);
								break;

							case RequestType.TimerStatus:
								this.DoClientTimerStatus(socket, Sockets.GetSocketByte(socket));
								break;

							default:
								socket.Send(new byte[] { 20 });
								break;
						}
					}
					else
					{
						Thread.Sleep(50);
					}
				}
			}
			catch (SocketException)
			{
				this.m_running = false;
			}
			this.m_listenerThread = null;
		}

		public bool Register(string clientName)
		{
			TcpClient client = null;
			try
			{
				client = Sockets.ConnectTo(this.m_serverAddress, 0xa1b9);
				int num = 2 + clientName.Length;
				byte[] array = new byte[num];
				array[0] = 7;
				array[1] = (byte) clientName.Length;
				Encoding.ASCII.GetBytes(clientName).CopyTo(array, 2);
				client.Client.Send(array);
				byte[] buffer = new byte[4];
				client.Client.Receive(buffer, 4, SocketFlags.None);
				this.m_clientRegisteredID = BitConverter.ToInt32(buffer, 0);
				client.Close();
				return (this.m_clientRegisteredID != 0);
			}
			catch (Exception exception)
			{
				this.m_clientRegisteredID = 0;
				if ((client != null) && client.Connected)
				{
					client.Close();
				}
				ErrorLog.Log(exception.Message);
				return false;
			}
		}

		private string[] RequestRemoteList(byte listType)
		{
			TcpClient client = Sockets.ConnectTo(this.m_serverAddress, 0xa1b9);
			try
			{
				byte[] buffer = new byte[] { 6, listType };
				client.Client.Send(buffer);
				byte[] buffer2 = new byte[1];
				client.Client.Receive(buffer2, 1, SocketFlags.None);
				int num = buffer2[0];
				string[] strArray = new string[num];
				for (int i = 0; i < num; i++)
				{
					strArray[i] = Sockets.GetSocketString(client.Client);
				}
				client.Close();
				return strArray;
			}
			catch
			{
				if (client.Connected)
				{
					client.Close();
				}
				return new string[0];
			}
		}

		public string[] RequestRemoteProgramList()
		{
			return this.RequestRemoteList(1);
		}

		public string[] RequestRemoteSequenceList()
		{
			return this.RequestRemoteList(0);
		}

		public SequenceProgram RetrieveRemoteProgram(string programFileName)
		{
			SequenceProgram program = null;
			try
			{
				program = ClientCommandHandlers.RetrieveRemoteProgram(programFileName, this.m_serverAddress);
			}
			catch
			{
				return null;
			}
			return program;
		}

		public EventSequence RetrieveRemoteSequence(string sequenceFileName)
		{
			EventSequence sequence = null;
			try
			{
				sequence = ClientCommandHandlers.RetrieveRemoteSequence(sequenceFileName, this.m_serverAddress);
			}
			catch
			{
				return null;
			}
			return sequence;
		}

		private void SendExecutionStatus(Socket socket, int executionContextHandle)
		{
			int num;
			socket.Send(new byte[] { (byte) (this.m_clientContext.ExecutionInterface.EngineStatus(executionContextHandle, out num) + 1) });
			string str = this.m_clientContext.ExecutionInterface.LoadedProgram(executionContextHandle);
			int num2 = this.m_clientContext.ExecutionInterface.ProgramLength(executionContextHandle);
			Sockets.SendSocketString(socket, str);
			Sockets.SendSocketInt32(socket, num2);
			str = this.m_clientContext.ExecutionInterface.LoadedSequence(executionContextHandle);
			num2 = this.m_clientContext.ExecutionInterface.SequenceLength(executionContextHandle);
			Sockets.SendSocketString(socket, str);
			Sockets.SendSocketInt32(socket, num2);
			Sockets.SendSocketInt32(socket, num);
		}

		public void ShowControlPanel()
		{
			ExecutionClientUI tui = new ExecutionClientUI(this, this.m_clientContext);
			tui.ShowDialog();
			tui.Dispose();
		}

		public bool Start()
		{
			if (!this.m_running)
			{
				string str2;
				try
				{
					this.m_listener = new TcpListener(Sockets.GetIPV4Address(), 0xa1bb);
					this.m_listener.Start();
				}
				catch
				{
					throw new Exception("Client port is already in use.\nIs the client already running?");
				}
				this.m_listenerThread = new Thread(new ThreadStart(this.ListenerThread));
				this.m_listenerThread.Start();
				Preference2 userPreferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
				string clientName = userPreferences.GetString("ClientName");
				if (!this.Register(clientName))
				{
					ErrorLog.Log(string.Format("Could not register client {0} with the server {1}", clientName, this.Server));
					this.Stop();
					return false;
				}
				Profile profile = null;
				if ((str2 = userPreferences.GetString("DefaultProfile")).Length > 0)
				{
					profile = new Profile(Path.Combine(Paths.ProfilePath, str2 + ".pro"));
				}
				if (profile != null)
				{
					this.m_clientContext.ContextObject = profile;
				}
			}
			return true;
		}

		public void Stop()
		{
			if (this.m_running)
			{
				this.m_running = false;
				this.m_listenerThread.Join(0x3e8);
				if (this.m_listener != null)
				{
					this.m_listener.Stop();
				}
				this.Unregister();
				this.m_clientContext.ContextObject = null;
			}
		}

		public bool Unregister()
		{
			try
			{
				if ((this.m_serverAddress != null) && (this.m_clientRegisteredID != 0))
				{
					TcpClient client = Sockets.ConnectTo(this.m_serverAddress, 0xa1b9);
					int num = 5;
					byte[] array = new byte[num];
					array[0] = 8;
					BitConverter.GetBytes(this.m_clientRegisteredID).CopyTo(array, 1);
					client.Client.Send(array);
					this.m_clientRegisteredID = 0;
					client.Close();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Connected
		{
			get
			{
				return (this.m_clientRegisteredID != 0);
			}
		}

		public bool Running
		{
			get
			{
				return this.m_running;
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
						this.m_serverAddress = Dns.GetHostAddresses(value)[0];
						this.m_serverIPAddrString = this.m_serverAddress.ToString();
					}
				}
			}
		}

		public IPAddress ServerAddress
		{
			get
			{
				return this.m_serverAddress;
			}
		}
	}
}

