using System.Globalization;

namespace VixenPlus
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;
	using System.Timers;
	using System.Web;
	using System.Windows.Forms;
	using System.Xml;

	public class Server
	{
		private readonly Dictionary<int, DateTime> _authenticatedClients;
		//private const string m_authenticationError = "ERROR: Requestor does not have permission.  Please authenticate first.";
		private List<ExecutionClientStub>[] _countdownArray;
		private int _countdownArrayIndex;
		private readonly System.Timers.Timer _countdownTimer;
		//private const string m_executionError = "ERROR: Client could not execute command";
		private readonly IPAddress _hostAddress;
		private readonly string _hostName = Dns.GetHostName();
		//private const string m_invalidChannel = "ERROR: Invalid channel parameter";
		//private const string m_OK = "OK";
		private string _password = string.Empty;
		private readonly Dictionary<string, VixenSequenceProgram> _programs = new Dictionary<string, VixenSequenceProgram>();
		private int _registeredClientIndex = 1;
		private readonly List<string> _registeredClientNames = new List<string>();
		private readonly Dictionary<int, ExecutionClientStub> _registeredExecutionClientsIdClientIndex = new Dictionary<int, ExecutionClientStub>();
		private readonly Dictionary<string, int> _registeredExecutionClientsNameIdIndex = new Dictionary<string, int>();
		private bool _isRunning;
		private readonly Dictionary<string, byte[]> _sequences = new Dictionary<string, byte[]>();
		private readonly TcpListener _serverListener;
		private int _threadCountdown;
		private readonly TcpListener _webListener;

		public event ServerErrorEvent ServerError;

		public event ServerNotifyEvent ServerNotify;

		public Server()
		{
			foreach (var address in Dns.GetHostAddresses(_hostName))
			{
				if (address.AddressFamily == AddressFamily.InterNetwork)
				{
					_hostAddress = address;
					break;
				}
			}
			if (_hostAddress == null)
			{
				throw new Exception("Could not find an IPV4 address to bind to.");
			}
			try
			{
				_serverListener = new TcpListener(_hostAddress, 0xa1b9);
			}
			catch
			{
				if (ServerError != null)
				{
					ServerError("Server port is already in use.\nIs the server already running?");
				}
				return;
			}
			try
			{
				_webListener = new TcpListener(_hostAddress, 0xa1ba);
			}
			catch
			{
				if (ServerError != null)
				{
					ServerError("Web interface port is already in use.\nIs the server already running?");
				}
				return;
			}
			_authenticatedClients = new Dictionary<int, DateTime>();
			_countdownTimer = new System.Timers.Timer();
			_countdownTimer.Elapsed += countdownTimer_Elapsed;
			_countdownTimer.Interval = 100.0;
		}

		private void _ClientEcho(Socket socket)
		{
			ExecutionClientStub stub;
			int key = Sockets.GetSocketInt32(socket);
			int sourceClientId = Sockets.GetSocketInt32(socket);
			if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
			{
				socket.Send(ClientEcho(stub, sourceClientId) ? new byte[] {19} : new byte[] {20});
			}
		}

		private string ActionClientStatus(XmlNode responseRootNode, ExecutionClientStub client)
		{
			object[] response = ClientStatus(client);
			if (response != null)
			{
				return ParseAndReturnStatus(response, responseRootNode);
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionClientTimerData(XmlNode responseRootNode, ExecutionClientStub client, string dataRequested, int timerIndex)
		{
			if (dataRequested != null)
			{
				dataRequested = dataRequested.ToLower();
				if ((dataRequested == "status") && (timerIndex != -1))
				{
					object[] response = ClientTimerStatus(client, timerIndex);
					if (response != null)
					{
						return ParseAndReturnStatus(response, responseRootNode);
					}
					return "ERROR: Client could not execute command";
				}
				if (dataRequested == "count")
				{
					int num = ClientTimerCount(client);
					if (num != -1)
					{
						Xml.SetNewValue(responseRootNode, "Timers", num.ToString(CultureInfo.InvariantCulture));
						return "OK";
					}
					return "ERROR: Client could not execute command";
				}
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionExecute(ExecutionClientStub client)
		{
			if (ClientExecute(client))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionListLocal(XmlNode responseRootNode, ExecutionClientStub client, string type)
		{
			string[] strArray;
			bool flag = false;
			if ((type != null) && (type.ToLower() == "program"))
			{
				strArray = ClientLocalList(1, client);
				if (strArray != null)
				{
					AddList(strArray, "Programs", "Program", responseRootNode);
					flag = true;
				}
			}
			else
			{
				strArray = ClientLocalList(0, client);
				if (strArray != null)
				{
					AddList(strArray, "Sequences", "Sequence", responseRootNode);
					flag = true;
				}
			}
			if (flag)
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionListRemote(XmlNode responseRootNode, string type)
		{
			string[] strArray;
			bool flag = false;
			type = type != null ? type.ToLower() : string.Empty;
			if (type == "program")
			{
				strArray = ServerList(1);
				if (strArray != null)
				{
					AddList(strArray, "Programs", "Program", responseRootNode);
					flag = true;
				}
			}
			else if (type == "client")
			{
				strArray = ServerList(2);
				if (strArray != null)
				{
					AddList(strArray, "Clients", "Client", responseRootNode);
					flag = true;
				}
			}
			else
			{
				strArray = ServerList(0);
				if (strArray != null)
				{
					AddList(strArray, "Sequences", "Sequence", responseRootNode);
					flag = true;
				}
			}
			if (flag)
			{
				return "OK";
			}
			return "ERROR: Server could not execute command";
		}

		private string ActionOff(ExecutionClientStub client, int channel)
		{
			if (ClientChannelOff(client, channel))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionOn(ExecutionClientStub client, int channel)
		{
			if (ClientChannelOn(client, channel))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionPause(ExecutionClientStub client)
		{
			if (ClientPause(client))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionRetrieve(ExecutionClientStub client, string scope, string type, string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return "ERROR: No object file name specified";
			}
			if (ClientRetrieve(client, scope, type, fileName))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionStop(ExecutionClientStub client)
		{
			if (ClientStop(client))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private string ActionToggle(ExecutionClientStub client, int channel)
		{
			if (ClientChannelToggle(client, channel))
			{
				return "OK";
			}
			return "ERROR: Client could not execute command";
		}

		private void AddList(IEnumerable<string> items, string listElement, string listItemElement, XmlNode responseRootNode)
		{
			XmlNode contextNode = Xml.SetNewValue(responseRootNode, listElement, null);
			foreach (string str in items)
			{
				Xml.SetNewValue(contextNode, listItemElement, str);
			}
		}

		private void Authenticate(Socket socket)
		{
			socket.Send(Authenticate(Sockets.GetSocketString(socket), socket) ? new byte[] {19} : new byte[] {20});
		}

		private bool Authenticate(string password, Socket socket)
		{
			if ((_password == string.Empty) || (_password == password))
			{
				int hashCode = ((IPEndPoint) socket.RemoteEndPoint).Address.ToString().GetHashCode();
				if (_password != string.Empty)
				{
					_authenticatedClients[hashCode] = DateTime.Now;
				}
				return true;
			}
			return false;
		}

		private bool AuthenticClient(EndPoint endPoint)
		{
			if (_password == string.Empty)
			{
				return true;
			}
			int hashCode = ((IPEndPoint) endPoint).Address.ToString().GetHashCode();
			if (_authenticatedClients.ContainsKey(hashCode))
			{
				DateTime time = _authenticatedClients[hashCode];
				var span = DateTime.Now - time;
				if (span.TotalMinutes > 60.0)
				{
					_authenticatedClients.Remove(hashCode);
					return false;
				}
				_authenticatedClients[hashCode] = DateTime.Now;
				return true;
			}
			return false;
		}

		private void BroadcastExecuteRequest(Socket requestingClient)
		{
			if (AuthenticClient(requestingClient.RemoteEndPoint))
			{
				int num = 1000;
				_threadCountdown = _registeredExecutionClientsIdClientIndex.Count;
				int index = 0;
				foreach (ExecutionClientStub stub in _registeredExecutionClientsIdClientIndex.Values)
				{
					ThreadPool.QueueUserWorkItem(PingThread, new object[] { stub, index++ });
				}
				ServerStatus("Waiting for clients to respond...");
				int threadCountdown = _threadCountdown + 1;
				var buffer = new byte[1];
				while (_threadCountdown > 0)
				{
					if (threadCountdown != _threadCountdown)
					{
						buffer[0] = (byte) _threadCountdown;
						requestingClient.Send(buffer);
						threadCountdown = _threadCountdown;
					}
					Thread.Sleep(100);
				}
				if (threadCountdown > 0)
				{
					buffer[0] = 0;
					requestingClient.Send(buffer);
				}
				foreach (ExecutionClientStub stub in _registeredExecutionClientsIdClientIndex.Values)
				{
					num = Math.Max(stub.Ping, num);
				}
				_countdownArray = new List<ExecutionClientStub>[num / 100];
				foreach (ExecutionClientStub stub in _registeredExecutionClientsIdClientIndex.Values)
				{
					index = stub.Ping / 100;
					if (_countdownArray[index] == null)
					{
						_countdownArray[index] = new List<ExecutionClientStub>();
					}
					_countdownArray[index].Add(stub);
				}
				ServerStatus("Executing");
				_countdownArrayIndex = _countdownArray.GetLength(0) - 1;
				_countdownTimer.Start();
			}
		}

		private void BroadcastPacket(RequestType requestType, byte[] requestData)
		{
			foreach (ExecutionClientStub stub in _registeredExecutionClientsIdClientIndex.Values)
			{
				ThreadPool.QueueUserWorkItem(BroadcastThread, new object[] { stub, requestType, requestData });
			}
		}

		private void BroadcastThread(object obj)
		{
			var objArray = (object[]) obj;
			ClientRequest((ExecutionClientStub) objArray[0], (RequestType) objArray[1], (byte[]) objArray[2]);
		}

		private void ChannelOff(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				ExecutionClientStub stub;
				int key = Sockets.GetSocketInt32(socket);
				int channel = Sockets.GetSocketInt32(socket);
				if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
				{
					socket.Send(ClientChannelOff(stub, channel) ? new byte[] {19} : new byte[] {20});
				}
			}
		}

		private void ChannelOn(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				ExecutionClientStub stub;
				int key = Sockets.GetSocketInt32(socket);
				int channel = Sockets.GetSocketInt32(socket);
				if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
				{
					socket.Send(ClientChannelOn(stub, channel) ? new byte[] {19} : new byte[] {20});
				}
			}
		}

		private void ChannelToggle(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				ExecutionClientStub stub;
				int key = Sockets.GetSocketInt32(socket);
				int channel = Sockets.GetSocketInt32(socket);
				if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
				{
					socket.Send(ClientChannelToggle(stub, channel) ? new byte[] {19} : new byte[] {20});
				}
			}
		}

		private int ClientChannelCount(ExecutionClientStub client)
		{
			Socket socket = SendClientRequestWithResponse(client, RequestType.ChannelCount, null).Client;
			if (Sockets.GetSocketByte(socket) == 0x13)
			{
				return Sockets.GetSocketByte(socket);
			}
			return 0;
		}

		private bool ClientChannelOff(ExecutionClientStub client, int channel)
		{
			ServerStatus("Received channel off request");
			var array = new byte[4];
			BitConverter.GetBytes(channel).CopyTo(array, 0);
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.ChannelOff, array).Client) == 0x13);
		}

		private bool ClientChannelOn(ExecutionClientStub client, int channel)
		{
			ServerStatus("Received channel on request");
			var array = new byte[4];
			BitConverter.GetBytes(channel).CopyTo(array, 0);
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.ChannelOn, array).Client) == 0x13);
		}

		private bool ClientChannelToggle(ExecutionClientStub client, int channel)
		{
			ServerStatus("Received channel toggle request");
			var array = new byte[4];
			BitConverter.GetBytes(channel).CopyTo(array, 0);
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.ChannelToggle, array).Client) == 0x13);
		}

		private bool ClientEcho(ExecutionClientStub client, int sourceClientID)
		{
			var array = new byte[4];
			BitConverter.GetBytes(sourceClientID).CopyTo(array, 0);
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.ClientEcho, array).Client) == 0x13);
		}

		private void ClientEnumeration(Socket socket)
		{
			ServerStatus("Received client enumeration request");
			Sockets.SendSocketInt32(socket, _registeredExecutionClientsIdClientIndex.Count);
			var ping = new Ping();
			foreach (int num2 in _registeredExecutionClientsIdClientIndex.Keys)
			{
				ExecutionClientStub client = _registeredExecutionClientsIdClientIndex[num2];
				Sockets.SendSocketInt32(socket, num2);
				var pingReply = ping.Send(client.IPAddress);
				var num = pingReply != null && pingReply.Status == IPStatus.Success ? ClientChannelCount(client) : 0;
				Sockets.SendSocketInt32(socket, num);
				Sockets.SendSocketString(socket, client.Name);
			}
		}

		private bool ClientExecute(ExecutionClientStub client)
		{
			ServerStatus("Received execute request (client)");
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.Execute, null).Client) == 0x13);
		}

		private string[] ClientLocalList(byte type, ExecutionClientStub client)
		{
			var list = new List<string>();
			var buffer2 = new byte[] { 0, 3, 0x2a, 0x2e, 0x2a };
			buffer2[0] = type;
			var requestData = buffer2;
			var socket = SendClientRequestWithResponse(client, RequestType.ListLocal, requestData).Client;
			if (Sockets.GetSocketByte(socket) == 0x13)
			{
				int socketByte = Sockets.GetSocketByte(socket);
				while (socketByte-- > 0)
				{
					list.Add(Sockets.GetSocketString(socket));
				}
				return list.ToArray();
			}
			return null;
		}

		private object[] ClientObjectStatus(Socket socket)
		{
			if (Sockets.GetSocketByte(socket) == 0x13)
			{
				var list = new List<object>();
				byte socketByte = Sockets.GetSocketByte(socket);
				list.Add(socketByte);
				if (socketByte != 0)
				{
					list.Add(Sockets.GetSocketString(socket));
					list.Add(Sockets.GetSocketInt32(socket));
					list.Add(Sockets.GetSocketString(socket));
					list.Add(Sockets.GetSocketInt32(socket));
					list.Add(Sockets.GetSocketInt32(socket));
				}
				return list.ToArray();
			}
			return null;
		}

		private bool ClientPause(ExecutionClientStub client)
		{
			ServerStatus("Received pause request (client)");
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.Pause, null).Client) == 0x13);
		}

		private TcpClient ClientRequest(ExecutionClientStub client, RequestType requestType, byte[] requestData)
		{
			byte[] buffer;
			TcpClient client2 = Sockets.ConnectTo(client.IPAddress, 0xa1bb);
			if (requestData != null)
			{
				buffer = new byte[requestData.Length + 1];
				requestData.CopyTo(buffer, 1);
			}
			else
			{
				buffer = new byte[1];
			}
			buffer[0] = (byte) requestType;
			client2.Client.Send(buffer);
			return client2;
		}

		private bool ClientRetrieve(ExecutionClientStub client, string scope, string type, string fileName)
		{
			ServerStatus("Received retrieve request (client)");
			RequestType requestType = (scope == "local") ? RequestType.RetrieveLocal : RequestType.RetrieveRemote;
			var array = new byte[2 + fileName.Length];
			array[0] = (type == "program") ? ((byte) 1) : ((byte) 0);
			array[1] = (byte) fileName.Length;
			Encoding.ASCII.GetBytes(fileName).CopyTo(array, 2);
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, requestType, array).Client) == 0x13);
		}

		private void ClientStatus(Socket socket)
		{
			ExecutionClientStub stub;
			var key = Sockets.GetSocketInt32(socket);
			if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
			{
				Socket client = SendClientRequestWithResponse(stub, RequestType.ClientStatus, null).Client;
				var buffer = new byte[1];
				client.Receive(buffer, 1, SocketFlags.None);
				socket.Send(buffer);
				if (buffer[0] != 0)
				{
					Sockets.SendSocketString(socket, Sockets.GetSocketString(client));
					Sockets.SendSocketInt32(socket, Sockets.GetSocketInt32(client));
					Sockets.SendSocketString(socket, Sockets.GetSocketString(client));
					Sockets.SendSocketInt32(socket, Sockets.GetSocketInt32(client));
					Sockets.SendSocketInt32(socket, Sockets.GetSocketInt32(client));
				}
			}
		}

		private object[] ClientStatus(ExecutionClientStub client)
		{
			ServerStatus("Received client status request");
			Socket socket = SendClientRequestWithResponse(client, RequestType.ClientStatus, null).Client;
			return ClientObjectStatus(socket);
		}

		private bool ClientStop(ExecutionClientStub client)
		{
			ServerStatus("Received stop request (client)");
			return (Sockets.GetSocketByte(SendClientRequestWithResponse(client, RequestType.Stop, null).Client) == 0x13);
		}

		private int ClientTimerCount(ExecutionClientStub client)
		{
			ServerStatus("Received timer count request");
			Socket socket = SendClientRequestWithResponse(client, RequestType.TimerCount, null).Client;
			if (Sockets.GetSocketByte(socket) == 0x13)
			{
				return Sockets.GetSocketByte(socket);
			}
			return -1;
		}

		private object[] ClientTimerStatus(ExecutionClientStub client, int timerIndex)
		{
			ServerStatus("Received client timer status request");
			Socket socket = SendClientRequestWithResponse(client, RequestType.TimerStatus, new[] { (byte) timerIndex }).Client;
			return ClientObjectStatus(socket);
		}

		private void countdownTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			List<ExecutionClientStub> list = _countdownArray[_countdownArrayIndex];
			if (list != null)
			{
				foreach (ExecutionClientStub stub in list)
				{
					SendClientRequest(stub, RequestType.Execute, null);
				}
			}
			if (_countdownArrayIndex == 0)
			{
				_countdownTimer.Stop();
			}
			_countdownArrayIndex--;
		}

		private XmlNode CreateResponse()
		{
			return Xml.SetValue(Xml.CreateXmlDocument("Client").DocumentElement, "Response", null);
		}

		private void Echo(Socket socket)
		{
			socket.Send(new byte[] { 0x12 });
		}

		private void ExecuteRequest(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				int key = Sockets.GetSocketInt32(socket);
				if (key == -1)
				{
					socket.Send(new byte[] { 0x13 });
					ServerBroadcastExecute(socket);
				}
				else
				{
					ExecutionClientStub stub;
					if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
					{
						socket.Send(ClientExecute(stub) ? new byte[] {19} : new byte[] {20});
					}
				}
			}
		}

		private XmlDocument HandleCommandRequest(ExecutionClientStub client, Dictionary<string, object> requestParts, Socket socket)
		{
			int num;
			string str2;
			string str3;
			var dictionary = (Dictionary<string, string>) requestParts["Params"];
			var node = CreateResponse();
			XmlNode documentElement = node.OwnerDocument.DocumentElement;
			var str = string.Format("ERROR: Unhandled client command '{0}'", dictionary["action"]);
			switch (dictionary["action"])
			{
				case "on":
					if (!dictionary.TryGetValue("channel", out str2) || str2.ToLower() == "all")
					{
						num = -1;
						break;
					}
					try
					{
						num = Convert.ToInt32(str2) - 1;
					}
					catch
					{
						node.InnerText = "ERROR: Invalid channel parameter";
						return documentElement.OwnerDocument;
					}
					break;

				case "off":
					if (!dictionary.TryGetValue("channel", out str2) || str2.ToLower() == "all")
					{
						num = -1;
					}
					else
					{
						try
						{
							num = Convert.ToInt32(str2) - 1;
						}
						catch
						{
							node.InnerText = "ERROR: Invalid channel parameter";
							return documentElement.OwnerDocument;
						}
					}
					if (AuthenticClient(socket.RemoteEndPoint))
					{
						str = ActionOff(client, num);
					}
					else
					{
						str = "ERROR: Requestor does not have permission.  Please authenticate first.";
					}
					goto Label_048C;

				case "toggle":
					if (!dictionary.TryGetValue("channel", out str2) || str2.ToLower() == "all")
					{
						num = -1;
					}
					else
					{
						try
						{
							num = Convert.ToInt32(str2) - 1;
						}
						catch
						{
							node.InnerText = "ERROR: Invalid channel parameter";
							return documentElement.OwnerDocument;
						}
					}
					if (AuthenticClient(socket.RemoteEndPoint))
					{
						str = ActionToggle(client, num);
					}
					else
					{
						str = "ERROR: Requestor does not have permission.  Please authenticate first.";
					}
					goto Label_048C;

				case "list":
					dictionary.TryGetValue("type", out str3);
					str = ActionListLocal(documentElement, client, str3);
					goto Label_048C;

				case "execute":
					str = !AuthenticClient(socket.RemoteEndPoint) ? "ERROR: Requestor does not have permission.  Please authenticate first." : ActionExecute(client);
					goto Label_048C;

				case "pause":
					str = !AuthenticClient(socket.RemoteEndPoint) ? "ERROR: Requestor does not have permission.  Please authenticate first." : ActionPause(client);
					goto Label_048C;

				case "stop":
					str = !AuthenticClient(socket.RemoteEndPoint) ? "ERROR: Requestor does not have permission.  Please authenticate first." : ActionStop(client);
					goto Label_048C;

				case "retrieve":
					if (!AuthenticClient(socket.RemoteEndPoint))
					{
						str = "ERROR: Requestor does not have permission.  Please authenticate first.";
					}
					else
					{
						string str4;
						string str5;
						dictionary.TryGetValue("scope", out str4);
						dictionary.TryGetValue("type", out str3);
						dictionary.TryGetValue("filename", out str5);
						str5 = HttpUtility.UrlDecode(str5);
						str = ActionRetrieve(client, str4, str3, str5);
					}
					goto Label_048C;

				case "status":
					str = ActionClientStatus(documentElement, client);
					goto Label_048C;

				case "timer":
				{
					string str6;
					int result = -1;
					if (dictionary.TryGetValue("index", out str6))
					{
						int.TryParse(str6, out result);
					}
					dictionary.TryGetValue("data", out str6);
					str = ActionClientTimerData(documentElement, client, str6, result);
					goto Label_048C;
				}
				default:
					goto Label_048C;
			}
			if (dictionary.ContainsKey("level"))
			{
				try
				{
					int num3 = Convert.ToInt32(dictionary["level"]);
					if (num3 != -1)
					{
					}
				}
				catch
				{
				}
			}
			str = AuthenticClient(socket.RemoteEndPoint) ? ActionOn(client, num) : "ERROR: Requestor does not have permission.  Please authenticate first.";
		Label_048C:
			node.InnerText = str;
			return documentElement.OwnerDocument;
		}

		private XmlDocument HandleDebugRequest(ExecutionClientStub client, Dictionary<string, object> requestParts)
		{
			Dictionary<string, string> dictionary = (Dictionary<string, string>) requestParts["Params"];
			StringBuilder builder = new StringBuilder();
			string str = (client != null) ? client.Name : "None";
			builder.AppendFormat("Client: {0}<br/><br/>", str);
			foreach (string str2 in dictionary.Keys)
			{
				builder.AppendFormat("{0} = {1}<br/>", str2, dictionary[str2]);
			}
			XmlNode node = CreateResponse();
			node.InnerText = builder.ToString();
			return node.OwnerDocument;
		}

		private object HandleServerRequest(Dictionary<string, object> requestParts, Socket socket)
		{
			var dictionary = (Dictionary<string, string>) requestParts["Params"];
			XmlNode node = CreateResponse();
			XmlNode documentElement = node.OwnerDocument.DocumentElement;
			string str = string.Format("ERROR: Unhandled server request '{0}'", dictionary["action"]);
			string str4 = dictionary["action"];
			if (str4 != null)
			{
				if (str4 != "list")
				{
					if (str4 == "execute")
					{
						if (AuthenticClient(socket.RemoteEndPoint))
						{
							ServerBroadcastExecute(socket);
							str = "OK";
						}
						else
						{
							str = "ERROR: Requestor does not have permission.  Please authenticate first.";
						}
					}
					else if (str4 == "pause")
					{
						if (AuthenticClient(socket.RemoteEndPoint))
						{
							ServerBroadcastPause();
							str = "OK";
						}
						else
						{
							str = "ERROR: Requestor does not have permission.  Please authenticate first.";
						}
					}
					else if (str4 == "stop")
					{
						if (AuthenticClient(socket.RemoteEndPoint))
						{
							ServerBroadcastStop();
							str = "OK";
						}
						else
						{
							str = "ERROR: Requestor does not have permission.  Please authenticate first.";
						}
					}
					else if (str4 == "authenticate")
					{
						string str3;
						if (dictionary.TryGetValue("value", out str3) && Authenticate(dictionary["value"], socket))
						{
							str = "OK";
						}
						else
						{
							str = "ERROR: Invalid password";
						}
					}
				}
				else
				{
					string str2;
					dictionary.TryGetValue("type", out str2);
					str = ActionListRemote(documentElement, str2);
				}
			}
			node.InnerText = str;
			return documentElement.OwnerDocument;
		}

		private void HandleServerSocket(object socketObject)
		{
			try
			{
				var socket = (Socket) socketObject;
				var buffer = new byte[1];
				if (socket.Available != 0)
				{
					socket.Receive(buffer, 1, SocketFlags.None);
					switch (((RequestType) buffer[0]))
					{
						case RequestType.LoadProgram:
							LoadProgram(socket);
							return;

						case RequestType.Execute:
							ExecuteRequest(socket);
							return;

						case RequestType.Stop:
							StopRequest(socket);
							return;

						case RequestType.Pause:
							PauseRequest(socket);
							return;

						case RequestType.Remove:
							Remove(socket);
							return;

						case RequestType.ListRemote:
							ListRemote(socket);
							return;

						case RequestType.RegisterClient:
							RegisterClient(socket);
							return;

						case RequestType.UnregisterClient:
							UnregisterClient(socket);
							return;

						case RequestType.ClientEnumeration:
							ClientEnumeration(socket);
							return;

						case RequestType.Information:
						case RequestType.ListLocal:
						case RequestType.RetrieveLocal:
						case RequestType.RetrieveRemote:
						case RequestType.Ack:
						case RequestType.Nack:
							return;

						case RequestType.ChannelOn:
							ChannelOn(socket);
							return;

						case RequestType.ChannelOff:
							ChannelOff(socket);
							return;

						case RequestType.LoadSequence:
							LoadSequence(socket);
							return;

						case RequestType.ChannelToggle:
							ChannelToggle(socket);
							return;

						case RequestType.Echo:
							Echo(socket);
							return;

						case RequestType.Authenticate:
							Authenticate(socket);
							return;

						case RequestType.Retrieve:
							Retrieve(socket);
							return;

						case RequestType.ClientStatus:
							ClientStatus(socket);
							return;

						case RequestType.ClientEcho:
							_ClientEcho(socket);
							return;
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message + "\n\n" + exception.StackTrace, "Vixen server");
			}
		}

		private void HandleWebSocket(object socketObject)
		{
			var socket = (Socket) socketObject;
			var buffer = new byte[0x400];
			var builder = new StringBuilder();
			try
			{
				int num2;
				int count = socket.Receive(buffer, buffer.Length, SocketFlags.None);
				builder.Append(Encoding.ASCII.GetString(buffer, 0, count));
				while (count == buffer.Length)
				{
					count = socket.Receive(buffer, buffer.Length, SocketFlags.None);
					builder.Append(Encoding.ASCII.GetString(buffer, 0, count));
				}
				string text = builder.ToString();
				if (text.Contains("ShowRequest=1"))
				{
					MessageBox.Show(text);
				}
				var requestParts = ParseRequest(text);
				var strArray = ((string) requestParts["PagePath"]).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				var key = HttpUtility.UrlDecode(strArray[0]).ToLower();
				_registeredExecutionClientsNameIdIndex.TryGetValue(key, out num2);
				ExecutionClientStub stub = null;
				if (num2 != 0)
				{
					_registeredExecutionClientsIdClientIndex.TryGetValue(num2, out stub);
				}
				string str3 = Path.GetFileNameWithoutExtension(strArray[strArray.Length - 1]).ToLower();
				object outerXml = string.Format("Unknown action '{0}'", str3);
				string str4 = str3;
				if (str4 != "debug")
				{
					if (str4 == "command")
					{
						goto Label_0169;
					}
					if (str4 == "request")
					{
						goto Label_019A;
					}
				}
				else
				{
					outerXml = HandleDebugRequest(stub, requestParts);
				}
				goto Label_01A7;
			Label_0169:
				if (stub == null)
				{
					outerXml = string.Format("ERROR: Unknown client '{0}'", key);
				}
				else
				{
					outerXml = HandleCommandRequest(stub, requestParts, socket);
				}
				goto Label_01A7;
			Label_019A:
				outerXml = HandleServerRequest(requestParts, socket);
			Label_01A7:
				if (outerXml is XmlDocument)
				{
					outerXml = ((XmlDocument) outerXml).OuterXml;
				}
				SendHeader(string.Format("{0}/{1}", requestParts["Protocol"], requestParts["ProtocolVersion"]), ((string) outerXml).Length, "200 OK", ref socket);
				SendToBrowser((string) outerXml, ref socket);
				socket.Close();
			}
			catch (Exception exception)
			{
				SendHeader("HTTP/1.1", exception.Message.Length + 7, "200 OK", ref socket);
				SendToBrowser("ERROR: " + exception.Message, ref socket);
				socket.Close();
			}
		}

		private void ListenerThread(object param)
		{
			var parameters = (ListenerParams) param;
			try
			{
				while (_isRunning)
				{
					if (parameters.Listener.Pending())
					{
						Socket state = parameters.Listener.AcceptSocket();
						if (state.Connected)
						{
							ThreadPool.QueueUserWorkItem(parameters.Callback.Invoke, state);
						}
						else
						{
							if (!state.Connected)
							{
								ServerStatus("Socket is not connected");
							}
						}
					}
					else
					{
						Thread.Sleep(500);
					}
				}
			}
			catch (SocketException exception)
			{
				ServerStatus("SOCKET EXCEPTION: " + exception.Message);
			}
		}

		private void ListRemote(Socket socket)
		{
			ServerStatus("Received list remote request");
			if (Sockets.GetSocketByte(socket) == 1)
			{
				socket.Send(new[] { (byte) _programs.Count });
				foreach (string str in _programs.Keys)
				{
					Sockets.SendSocketString(socket, str);
				}
			}
			else
			{
				socket.Send(new[] { (byte) _sequences.Count });
				foreach (string str2 in _sequences.Keys)
				{
					Sockets.SendSocketString(socket, str2);
				}
			}
		}

		private void LoadProgram(Socket socket)
		{
			ServerStatus("Received program load request");
			int byteCount = Sockets.GetSocketInt32(socket);
			string socketString = Sockets.GetSocketString(socket);
			byte[] socketBytes = Sockets.GetSocketBytes(socket, byteCount);
			ServerStatus("Received program " + socketString);
			_programs[socketString] = new VixenSequenceProgram(socketBytes);
		}

		private void LoadSequence(Socket socket)
		{
			ServerStatus("Received sequence load request");
			string socketString = Sockets.GetSocketString(socket);
			byte num = Sockets.GetSocketBytes(socket, 1)[0];
			ServerStatus("Program name: \"" + socketString + "\"");
			ServerStatus("Index: " + num.ToString(CultureInfo.InvariantCulture));
			ServerStatus("Receiving sequence");
			int byteCount = Sockets.GetSocketInt32(socket);
			string item = Sockets.GetSocketString(socket);
			byte[] socketBytes = Sockets.GetSocketBytes(socket, byteCount);
			ServerStatus("Received sequence " + item);
			_sequences[item] = socketBytes;
			if (socketString.Length > 0)
			{
				ServerStatus("Adding to program");
				if (_programs.ContainsKey(socketString))
				{
					ServerStatus("Program exists");
					if ((num == 0xff) || (num >= _programs[socketString].Sequences.Count))
					{
						_programs[socketString].Sequences.Add(socketBytes);
						_programs[socketString].SequenceFileNames.Add(item);
					}
					else
					{
						_programs[socketString].Sequences[num] = socketBytes;
						_programs[socketString].SequenceFileNames[num] = item;
					}
					ServerStatus("Sequence assigned to program");
				}
				else
				{
					ServerStatus("Program specified does not exist");
				}
			}
		}

		private string ParseAndReturnStatus(object[] response, XmlNode responseRootNode)
		{
			XmlNode contextNode = Xml.SetNewValue(responseRootNode, "Status", null);
			string nodeValue = string.Empty;
			switch (((byte) response[0]))
			{
				case 0:
					nodeValue = "None";
					break;

				case 1:
					nodeValue = "Stopped";
					break;

				case 2:
					nodeValue = "Executing";
					break;

				case 3:
					nodeValue = "Paused";
					break;
			}
			Xml.SetNewValue(contextNode, "Execution", nodeValue);
			if (((byte) response[0]) != 0)
			{
				Xml.SetAttribute(Xml.SetNewValue(contextNode, "Program", (string) response[1]), "length", response[2].ToString());
				XmlNode node = Xml.SetNewValue(contextNode, "Sequence", (string) response[3]);
				Xml.SetAttribute(node, "length", response[4].ToString());
				Xml.SetAttribute(node, "progress", response[5].ToString());
			}
			return "OK";
		}

		private Dictionary<string, object> ParseRequest(string request)
		{
			var dictionary = new Dictionary<string, object>();
			var strArray = request.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			var str = strArray[0];
			var strArray2 = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			dictionary["RequestType"] = strArray2[0];
			var strArray3 = strArray2[2].Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			dictionary["Protocol"] = strArray3[0];
			dictionary["ProtocolVersion"] = strArray3[1];
			var index = strArray2[1].IndexOf('?');
			if (index != -1)
			{
				dictionary["PagePath"] = strArray2[1].Substring(0, index);
				var dictionary2 = new Dictionary<string, string>();
				dictionary["Params"] = dictionary2;
				foreach (string str2 in strArray2[1].Substring(index + 1).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
				{
					var strArray4 = str2.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
					dictionary2[strArray4[0]] = strArray4[1];
				}
			}
			else
			{
				dictionary["PagePath"] = strArray2[1];
			}
			int num2 = 1;
			while (num2 < strArray.Length)
			{
				if (strArray[num2].StartsWith("Host:"))
				{
					break;
				}
				num2++;
			}
			if (num2 != strArray.Length)
			{
				str = strArray[num2];
				strArray2 = str.Substring(6).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				dictionary["HostName"] = strArray2[0];
				dictionary["Port"] = strArray2[1];
			}
			return dictionary;
		}

		private void PauseRequest(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				int key = Sockets.GetSocketInt32(socket);
				if (key == -1)
				{
					socket.Send(new byte[] { 0x13 });
					ServerBroadcastPause();
				}
				else
				{
					ExecutionClientStub stub;
					if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
					{
						socket.Send(ClientPause(stub) ? new byte[] {19} : new byte[] {20});
					}
				}
			}
		}

		private void PingThread(object obj)
		{
			var ping = new Ping();
			var stub = (ExecutionClientStub) ((Array) obj).GetValue(0);

			var num2 = 0;
			var pingAddress = ping.Send(stub.IPAddress);
			if (pingAddress != null)
			{
				for (var i = 0; i < 4; i++)
				{
					num2 += (int) (pingAddress.RoundtripTime >> 1);
				}
			}

			num2 = num2 >> 2;
			stub.Ping = (num2 / 100) * 100;
			_threadCountdown--;
		}

		private void RegisterClient(Socket socket)
		{
			ServerStatus("Received registration request from " + ((IPEndPoint) socket.RemoteEndPoint).Address);
			string str = Sockets.GetSocketString(socket).ToLower();
			ServerStatus(string.Format("Attempting registration of client '{0}'", str));
			if (!(_registeredClientNames.Contains(str) || _registeredExecutionClientsNameIdIndex.ContainsKey(str)))
			{
				string ipString = socket.RemoteEndPoint.ToString();
				ipString = ipString.Substring(0, ipString.IndexOf(':'));
				var stub = new ExecutionClientStub(str, IPAddress.Parse(ipString));
				int key = _registeredClientIndex++;
				_registeredExecutionClientsIdClientIndex.Add(key, stub);
				_registeredExecutionClientsNameIdIndex.Add(str, key);
				_registeredClientNames.Add(str);
				socket.Send(BitConverter.GetBytes(key));
				ServerStatus("Registered");
			}
			else
			{
				ServerStatus("Registration failed");
				socket.Send(BitConverter.GetBytes(0));
			}
		}

		private void Remove(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				ServerStatus("Received remove request");
				string key = Sockets.GetSocketString(socket).ToLower();
				if (_programs.ContainsKey(key))
				{
					_programs.Remove(key);
				}
				else if (_sequences.ContainsKey(key))
				{
					_sequences.Remove(key);
				}
			}
		}

		private void Retrieve(Socket socket)
		{
			byte[] buffer;
			byte socketByte = Sockets.GetSocketByte(socket);
			string socketString = Sockets.GetSocketString(socket);
			if (socketByte == 1)
			{
				VixenSequenceProgram program;
				if (_programs.TryGetValue(socketString, out program))
				{
					buffer = new byte[5 + program.Program.Length];
					buffer[0] = 0x13;
					BitConverter.GetBytes(program.Program.Length).CopyTo(buffer, 1);
					program.Program.CopyTo(buffer, 5);
					socket.Send(buffer);
					socket.Send(new[] { (byte) program.Sequences.Count });
					for (int i = 0; i < program.Sequences.Count; i++)
					{
						Sockets.SendSocketString(socket, program.SequenceFileNames[i]);
						Sockets.SendSocketInt32(socket, program.Sequences[i].Length);
						socket.Send(program.Sequences[i]);
					}
				}
				else
				{
					socket.Send(new byte[] { 20 });
				}
			}
			else
			{
				byte[] buffer2;
				if (_sequences.TryGetValue(socketString, out buffer2))
				{
					buffer = new byte[5 + buffer2.Length];
					buffer[0] = 0x13;
					BitConverter.GetBytes(buffer2.Length).CopyTo(buffer, 1);
					buffer2.CopyTo(buffer, 5);
					socket.Send(buffer);
				}
				else
				{
					socket.Send(new byte[] { 20 });
				}
			}
		}

		private void SendClientRequest(ExecutionClientStub client, RequestType requestType, byte[] requestData)
		{
			ClientRequest(client, requestType, requestData).Close();
		}

		private TcpClient SendClientRequestWithResponse(ExecutionClientStub client, RequestType requestType, byte[] requestData)
		{
			TcpClient client2 = ClientRequest(client, requestType, requestData);
			client2.Client.ReceiveTimeout = 0x7d0;
			client2.Client.NoDelay = true;
			return client2;
		}

		private void SendHeader(string sHttpVersion, int iTotBytes, string sStatusCode, ref Socket socket)
		{
			string s = string.Format("{0} {1}\r\nContent-type: text/html\r\nAccept-Ranges: bytes\r\nContent-Length: {2}\r\n\r\n", sHttpVersion, sStatusCode, iTotBytes);
			SendToBrowser(Encoding.ASCII.GetBytes(s), ref socket);
		}

		private void SendToBrowser(string data, ref Socket socket)
		{
			SendToBrowser(Encoding.ASCII.GetBytes(data), ref socket);
		}

		private void SendToBrowser(byte[] sendData, ref Socket socket)
		{
			if (socket.Connected)
			{
				socket.Send(sendData, sendData.Length, SocketFlags.None);
			}
		}

		private void ServerBroadcastExecute(Socket socket)
		{
			ServerStatus("Received execute request (broadcast)");
			BroadcastExecuteRequest(socket);
		}

		private void ServerBroadcastPause()
		{
			ServerStatus("Received pause request (broadcast)");
			BroadcastPacket(RequestType.Pause, null);
		}

		private void ServerBroadcastStop()
		{
			ServerStatus("Received stop request (broadcast)");
			BroadcastPacket(RequestType.Stop, null);
		}

		private string[] ServerList(byte type)
		{
			string[] strArray;
			int num = 0;
			if (type == 1)
			{
				strArray = new string[_programs.Keys.Count];
				foreach (string str in _programs.Keys)
				{
					strArray[num++] = str;
				}
				return strArray;
			}
			if (type == 2)
			{
				return _registeredClientNames.ToArray();
			}
			strArray = new string[_sequences.Keys.Count];
			foreach (string str in _sequences.Keys)
			{
				strArray[num++] = str;
			}
			return strArray;
		}

		private void ServerStatus(string message)
		{
			if (ServerNotify != null)
			{
				if (message.Length > 0x33)
				{
					message = message.Substring(0, 0x33);
				}
				ServerNotify(message);
			}
		}

		public void Shutdown()
		{
			if (_isRunning)
			{
				Stop();
			}
		}

		public void Start()
		{
			_isRunning = true;
			StartServer();
			StartWebInterface();
			ServerStatus(string.Format("Server started on {0} ({1})", _hostName, _hostAddress));
		}

		private void StartServer()
		{
			_serverListener.Start();
			ThreadPool.QueueUserWorkItem(ListenerThread, new ListenerParams(_serverListener, HandleServerSocket));
		}

		private void StartWebInterface()
		{
			_webListener.Start();
			ThreadPool.QueueUserWorkItem(ListenerThread, new ListenerParams(_webListener, HandleWebSocket));
		}

		public void Stop()
		{
			_isRunning = false;
			_serverListener.Stop();
			_webListener.Stop();
			_registeredClientNames.Clear();
			_registeredExecutionClientsIdClientIndex.Clear();
			_registeredExecutionClientsNameIdIndex.Clear();
			_programs.Clear();
			_sequences.Clear();
			_authenticatedClients.Clear();
			ServerStatus("Server stopped");
		}

		private void StopRequest(Socket socket)
		{
			if (AuthenticClient(socket.RemoteEndPoint))
			{
				int key = Sockets.GetSocketInt32(socket);
				if (key == -1)
				{
					socket.Send(new byte[] { 0x13 });
					ServerBroadcastStop();
				}
				else
				{
					ExecutionClientStub stub;
					if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
					{
						socket.Send(ClientStop(stub) ? new byte[] {19} : new byte[] {20});
					}
				}
			}
		}

		private void UnregisterClient(Socket socket)
		{
			ExecutionClientStub stub;
			int key = Sockets.GetSocketInt32(socket);
			if (_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
			{
				string name = stub.Name;
				ServerStatus("Received unregistration request from " + name);
				_registeredClientNames.Remove(name);
				_registeredExecutionClientsIdClientIndex.Remove(key);
				_registeredExecutionClientsNameIdIndex.Remove(name);
			}
			else
			{
				ServerStatus("Received unregistration request from an invalid client");
			}
		}

		public bool IsRunning
		{
			get
			{
				return _isRunning;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				_authenticatedClients.Clear();
			}
		}

		public delegate void ServerErrorEvent(string message);

		public delegate void ServerNotifyEvent(string message);
	}
}

