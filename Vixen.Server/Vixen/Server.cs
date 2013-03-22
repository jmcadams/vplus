namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Timers;
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;

    public class Server
    {
        private Dictionary<int, DateTime> m_authenticatedClients;
        private const string m_authenticationError = "ERROR: Requestor does not have permission.  Please authenticate first.";
        private List<ExecutionClientStub>[] m_countdownArray;
        private int m_countdownArrayIndex;
        private System.Timers.Timer m_countdownTimer;
        private const string m_executionError = "ERROR: Client could not execute command";
        private IPAddress m_hostAddress = null;
        private string m_hostName = Dns.GetHostName();
        private const string m_invalidChannel = "ERROR: Invalid channel parameter";
        private const string m_OK = "OK";
        private string m_password = string.Empty;
        private Dictionary<string, VixenSequenceProgram> m_programs = new Dictionary<string, VixenSequenceProgram>();
        private int m_registeredClientIndex = 1;
        private List<string> m_registeredClientNames = new List<string>();
        private Dictionary<int, ExecutionClientStub> m_registeredExecutionClientsIdClientIndex = new Dictionary<int, ExecutionClientStub>();
        private Dictionary<string, int> m_registeredExecutionClientsNameIdIndex = new Dictionary<string, int>();
        private bool m_running = false;
        private Dictionary<string, byte[]> m_sequences = new Dictionary<string, byte[]>();
        private TcpListener m_serverListener;
        private int m_threadCountdown;
        private TcpListener m_webListener;

        public event ServerErrorEvent ServerError;

        public event ServerNotifyEvent ServerNotify;

        public Server()
        {
            foreach (IPAddress address in Dns.GetHostAddresses(this.m_hostName))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.m_hostAddress = address;
                    break;
                }
            }
            if (this.m_hostAddress == null)
            {
                throw new Exception("Could not find an IPV4 address to bind to.");
            }
            try
            {
                this.m_serverListener = new TcpListener(this.m_hostAddress, 0xa1b9);
            }
            catch
            {
                if (this.ServerError != null)
                {
                    this.ServerError("Server port is already in use.\nIs the server already running?");
                }
                return;
            }
            try
            {
                this.m_webListener = new TcpListener(this.m_hostAddress, 0xa1ba);
            }
            catch
            {
                if (this.ServerError != null)
                {
                    this.ServerError("Web interface port is already in use.\nIs the server already running?");
                }
                return;
            }
            this.m_authenticatedClients = new Dictionary<int, DateTime>();
            this.m_countdownTimer = new System.Timers.Timer();
            this.m_countdownTimer.Elapsed += new ElapsedEventHandler(this.countdownTimer_Elapsed);
            this.m_countdownTimer.Interval = 100.0;
        }

        private void _ClientEcho(Socket socket)
        {
            ExecutionClientStub stub;
            int key = Sockets.GetSocketInt32(socket);
            int sourceClientID = Sockets.GetSocketInt32(socket);
            if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
            {
                if (this.ClientEcho(stub, sourceClientID))
                {
                    socket.Send(new byte[] { 0x13 });
                }
                else
                {
                    socket.Send(new byte[] { 20 });
                }
            }
        }

        private string ActionClientStatus(XmlNode responseRootNode, ExecutionClientStub client)
        {
            object[] response = this.ClientStatus(client);
            if (response != null)
            {
                return this.ParseAndReturnStatus(response, responseRootNode);
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
                    object[] response = this.ClientTimerStatus(client, timerIndex);
                    if (response != null)
                    {
                        return this.ParseAndReturnStatus(response, responseRootNode);
                    }
                    return "ERROR: Client could not execute command";
                }
                if (dataRequested == "count")
                {
                    int num = 0;
                    num = this.ClientTimerCount(client);
                    if (num != -1)
                    {
                        Xml.SetNewValue(responseRootNode, "Timers", num.ToString());
                        return "OK";
                    }
                    return "ERROR: Client could not execute command";
                }
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionExecute(ExecutionClientStub client)
        {
            if (this.ClientExecute(client))
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
                strArray = this.ClientLocalList(1, client);
                if (strArray != null)
                {
                    this.AddList(strArray, "Programs", "Program", responseRootNode);
                    flag = true;
                }
            }
            else
            {
                strArray = this.ClientLocalList(0, client);
                if (strArray != null)
                {
                    this.AddList(strArray, "Sequences", "Sequence", responseRootNode);
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
            if (type != null)
            {
                type = type.ToLower();
            }
            else
            {
                type = string.Empty;
            }
            if (type == "program")
            {
                strArray = this.ServerList(1);
                if (strArray != null)
                {
                    this.AddList(strArray, "Programs", "Program", responseRootNode);
                    flag = true;
                }
            }
            else if (type == "client")
            {
                strArray = this.ServerList(2);
                if (strArray != null)
                {
                    this.AddList(strArray, "Clients", "Client", responseRootNode);
                    flag = true;
                }
            }
            else
            {
                strArray = this.ServerList(0);
                if (strArray != null)
                {
                    this.AddList(strArray, "Sequences", "Sequence", responseRootNode);
                    flag = true;
                }
            }
            if (flag)
            {
                return "OK";
            }
            return "ERROR: Server could not execute command";
        }

        private string ActionOff(XmlNode responseRootNode, ExecutionClientStub client, int channel)
        {
            if (this.ClientChannelOff(client, channel))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionOn(XmlNode responseRootNode, ExecutionClientStub client, int channel, int level)
        {
            if (this.ClientChannelOn(client, channel))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionPause(ExecutionClientStub client)
        {
            if (this.ClientPause(client))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionRetrieve(ExecutionClientStub client, string scope, string type, string fileName)
        {
            if ((fileName == null) || (fileName.Length == 0))
            {
                return "ERROR: No object file name specified";
            }
            if (this.ClientRetrieve(client, scope, type, fileName))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionStop(ExecutionClientStub client)
        {
            if (this.ClientStop(client))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private string ActionToggle(XmlNode responseRootNode, ExecutionClientStub client, int channel)
        {
            if (this.ClientChannelToggle(client, channel))
            {
                return "OK";
            }
            return "ERROR: Client could not execute command";
        }

        private void AddList(string[] items, string listElement, string listItemElement, XmlNode responseRootNode)
        {
            XmlNode contextNode = Xml.SetNewValue(responseRootNode, listElement, null);
            foreach (string str in items)
            {
                Xml.SetNewValue(contextNode, listItemElement, str);
            }
        }

        private void Authenticate(Socket socket)
        {
            if (this.Authenticate(Sockets.GetSocketString(socket), socket))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
        }

        private bool Authenticate(string password, Socket socket)
        {
            if ((this.m_password == string.Empty) || (this.m_password == password))
            {
                int hashCode = ((IPEndPoint) socket.RemoteEndPoint).Address.ToString().GetHashCode();
                if (this.m_password != string.Empty)
                {
                    this.m_authenticatedClients[hashCode] = DateTime.Now;
                }
                return true;
            }
            return false;
        }

        private bool AuthenticClient(EndPoint endPoint)
        {
            if (this.m_password == string.Empty)
            {
                return true;
            }
            int hashCode = ((IPEndPoint) endPoint).Address.ToString().GetHashCode();
            if (this.m_authenticatedClients.ContainsKey(hashCode))
            {
                DateTime time = this.m_authenticatedClients[hashCode];
                TimeSpan span = (TimeSpan) (DateTime.Now - time);
                if (span.TotalMinutes > 60.0)
                {
                    this.m_authenticatedClients.Remove(hashCode);
                    return false;
                }
                this.m_authenticatedClients[hashCode] = DateTime.Now;
                return true;
            }
            return false;
        }

        private void BroadcastExecuteRequest(Socket requestingClient)
        {
            if (this.AuthenticClient(requestingClient.RemoteEndPoint))
            {
                int num = 0x3e8;
                this.m_threadCountdown = this.m_registeredExecutionClientsIdClientIndex.Count;
                int index = 0;
                foreach (ExecutionClientStub stub in this.m_registeredExecutionClientsIdClientIndex.Values)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.PingThread), new object[] { stub, index++ });
                }
                this.ServerStatus("Waiting for clients to respond...");
                int threadCountdown = this.m_threadCountdown + 1;
                byte[] buffer = new byte[1];
                while (this.m_threadCountdown > 0)
                {
                    if ((threadCountdown != this.m_threadCountdown) && (requestingClient != null))
                    {
                        buffer[0] = (byte) this.m_threadCountdown;
                        requestingClient.Send(buffer);
                        threadCountdown = this.m_threadCountdown;
                    }
                    Thread.Sleep(100);
                }
                if ((threadCountdown > 0) && (requestingClient != null))
                {
                    buffer[0] = 0;
                    requestingClient.Send(buffer);
                }
                foreach (ExecutionClientStub stub in this.m_registeredExecutionClientsIdClientIndex.Values)
                {
                    num = Math.Max(stub.Ping, num);
                }
                this.m_countdownArray = new List<ExecutionClientStub>[num / 100];
                foreach (ExecutionClientStub stub in this.m_registeredExecutionClientsIdClientIndex.Values)
                {
                    index = stub.Ping / 100;
                    if (this.m_countdownArray[index] == null)
                    {
                        this.m_countdownArray[index] = new List<ExecutionClientStub>();
                    }
                    this.m_countdownArray[index].Add(stub);
                }
                this.ServerStatus("Executing");
                this.m_countdownArrayIndex = this.m_countdownArray.GetLength(0) - 1;
                this.m_countdownTimer.Start();
            }
        }

        private void BroadcastPacket(RequestType requestType, byte[] requestData)
        {
            foreach (ExecutionClientStub stub in this.m_registeredExecutionClientsIdClientIndex.Values)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.BroadcastThread), new object[] { stub, requestType, requestData });
            }
        }

        private void BroadcastThread(object obj)
        {
            object[] objArray = (object[]) obj;
            this.ClientRequest((ExecutionClientStub) objArray[0], (RequestType) objArray[1], (byte[]) objArray[2]);
        }

        private void ChannelOff(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                ExecutionClientStub stub;
                int key = Sockets.GetSocketInt32(socket);
                int channel = Sockets.GetSocketInt32(socket);
                if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                {
                    if (this.ClientChannelOff(stub, channel))
                    {
                        socket.Send(new byte[] { 0x13 });
                    }
                    else
                    {
                        socket.Send(new byte[] { 20 });
                    }
                }
            }
        }

        private void ChannelOn(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                ExecutionClientStub stub;
                int key = Sockets.GetSocketInt32(socket);
                int channel = Sockets.GetSocketInt32(socket);
                if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                {
                    if (this.ClientChannelOn(stub, channel))
                    {
                        socket.Send(new byte[] { 0x13 });
                    }
                    else
                    {
                        socket.Send(new byte[] { 20 });
                    }
                }
            }
        }

        private void ChannelToggle(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                ExecutionClientStub stub;
                int key = Sockets.GetSocketInt32(socket);
                int channel = Sockets.GetSocketInt32(socket);
                if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                {
                    if (this.ClientChannelToggle(stub, channel))
                    {
                        socket.Send(new byte[] { 0x13 });
                    }
                    else
                    {
                        socket.Send(new byte[] { 20 });
                    }
                }
            }
        }

        private int ClientChannelCount(ExecutionClientStub client)
        {
            Socket socket = this.SendClientRequestWithResponse(client, RequestType.ChannelCount, null).Client;
            if (Sockets.GetSocketByte(socket) == 0x13)
            {
                return Sockets.GetSocketByte(socket);
            }
            return 0;
        }

        private bool ClientChannelOff(ExecutionClientStub client, int channel)
        {
            this.ServerStatus("Received channel off request");
            byte[] array = new byte[4];
            BitConverter.GetBytes(channel).CopyTo(array, 0);
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.ChannelOff, array).Client) == 0x13);
        }

        private bool ClientChannelOn(ExecutionClientStub client, int channel)
        {
            this.ServerStatus("Received channel on request");
            byte[] array = new byte[4];
            BitConverter.GetBytes(channel).CopyTo(array, 0);
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.ChannelOn, array).Client) == 0x13);
        }

        private bool ClientChannelToggle(ExecutionClientStub client, int channel)
        {
            this.ServerStatus("Received channel toggle request");
            byte[] array = new byte[4];
            BitConverter.GetBytes(channel).CopyTo(array, 0);
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.ChannelToggle, array).Client) == 0x13);
        }

        private bool ClientEcho(ExecutionClientStub client, int sourceClientID)
        {
            byte[] array = new byte[4];
            BitConverter.GetBytes(sourceClientID).CopyTo(array, 0);
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.ClientEcho, array).Client) == 0x13);
        }

        private void ClientEnumeration(Socket socket)
        {
            this.ServerStatus("Received client enumeration request");
            Sockets.SendSocketInt32(socket, this.m_registeredExecutionClientsIdClientIndex.Count);
            Ping ping = new Ping();
            foreach (int num2 in this.m_registeredExecutionClientsIdClientIndex.Keys)
            {
                int num;
                ExecutionClientStub client = this.m_registeredExecutionClientsIdClientIndex[num2];
                Sockets.SendSocketInt32(socket, num2);
                if (ping.Send(client.IPAddress).Status == IPStatus.Success)
                {
                    num = this.ClientChannelCount(client);
                }
                else
                {
                    num = 0;
                }
                Sockets.SendSocketInt32(socket, num);
                Sockets.SendSocketString(socket, client.Name);
            }
        }

        private bool ClientExecute(ExecutionClientStub client)
        {
            this.ServerStatus("Received execute request (client)");
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.Execute, null).Client) == 0x13);
        }

        private string[] ClientLocalList(byte type, ExecutionClientStub client)
        {
            List<string> list = new List<string>();
            byte[] buffer2 = new byte[] { 0, 3, 0x2a, 0x2e, 0x2a };
            buffer2[0] = type;
            byte[] requestData = buffer2;
            Socket socket = this.SendClientRequestWithResponse(client, RequestType.ListLocal, requestData).Client;
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
                List<object> list = new List<object>();
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
            this.ServerStatus("Received pause request (client)");
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.Pause, null).Client) == 0x13);
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
            this.ServerStatus("Received retrieve request (client)");
            RequestType requestType = (scope == "local") ? RequestType.RetrieveLocal : RequestType.RetrieveRemote;
            byte[] array = new byte[2 + fileName.Length];
            array[0] = (type == "program") ? ((byte) 1) : ((byte) 0);
            array[1] = (byte) fileName.Length;
            Encoding.ASCII.GetBytes(fileName).CopyTo(array, 2);
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, requestType, array).Client) == 0x13);
        }

        private void ClientStatus(Socket socket)
        {
            ExecutionClientStub stub;
            int key = Sockets.GetSocketInt32(socket);
            if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
            {
                Socket client = this.SendClientRequestWithResponse(stub, RequestType.ClientStatus, null).Client;
                byte[] buffer = new byte[1];
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
            this.ServerStatus("Received client status request");
            Socket socket = this.SendClientRequestWithResponse(client, RequestType.ClientStatus, null).Client;
            return this.ClientObjectStatus(socket);
        }

        private bool ClientStop(ExecutionClientStub client)
        {
            this.ServerStatus("Received stop request (client)");
            return (Sockets.GetSocketByte(this.SendClientRequestWithResponse(client, RequestType.Stop, null).Client) == 0x13);
        }

        private int ClientTimerCount(ExecutionClientStub client)
        {
            this.ServerStatus("Received timer count request");
            Socket socket = this.SendClientRequestWithResponse(client, RequestType.TimerCount, null).Client;
            if (Sockets.GetSocketByte(socket) == 0x13)
            {
                return Sockets.GetSocketByte(socket);
            }
            return -1;
        }

        private object[] ClientTimerStatus(ExecutionClientStub client, int timerIndex)
        {
            this.ServerStatus("Received client timer status request");
            Socket socket = this.SendClientRequestWithResponse(client, RequestType.TimerStatus, new byte[] { (byte) timerIndex }).Client;
            return this.ClientObjectStatus(socket);
        }

        private void countdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<ExecutionClientStub> list = this.m_countdownArray[this.m_countdownArrayIndex];
            if (list != null)
            {
                foreach (ExecutionClientStub stub in list)
                {
                    this.SendClientRequest(stub, RequestType.Execute, null);
                }
            }
            if (this.m_countdownArrayIndex == 0)
            {
                this.m_countdownTimer.Stop();
            }
            this.m_countdownArrayIndex--;
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
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                int key = Sockets.GetSocketInt32(socket);
                if (key == -1)
                {
                    socket.Send(new byte[] { 0x13 });
                    this.ServerBroadcastExecute(socket);
                }
                else
                {
                    ExecutionClientStub stub;
                    if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                    {
                        if (this.ClientExecute(stub))
                        {
                            socket.Send(new byte[] { 0x13 });
                        }
                        else
                        {
                            socket.Send(new byte[] { 20 });
                        }
                    }
                }
            }
        }

        private XmlDocument HandleCommandRequest(ExecutionClientStub client, Dictionary<string, object> requestParts, Socket socket)
        {
            int num;
            string str2;
            string str3;
            Dictionary<string, string> dictionary = (Dictionary<string, string>) requestParts["Params"];
            XmlNode node = this.CreateResponse();
            XmlNode documentElement = node.OwnerDocument.DocumentElement;
            string str = string.Format("ERROR: Unhandled client command '{0}'", dictionary["action"]);
            switch (dictionary["action"])
            {
                case "on":
                    if (!dictionary.TryGetValue("channel", out str2) || !(str2.ToLower() != "all"))
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
                    if (!dictionary.TryGetValue("channel", out str2) || !(str2.ToLower() != "all"))
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
                    if (this.AuthenticClient(socket.RemoteEndPoint))
                    {
                        str = this.ActionOff(documentElement, client, num);
                    }
                    else
                    {
                        str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                    }
                    goto Label_048C;

                case "toggle":
                    if (!dictionary.TryGetValue("channel", out str2) || !(str2.ToLower() != "all"))
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
                    if (this.AuthenticClient(socket.RemoteEndPoint))
                    {
                        str = this.ActionToggle(documentElement, client, num);
                    }
                    else
                    {
                        str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                    }
                    goto Label_048C;

                case "list":
                    dictionary.TryGetValue("type", out str3);
                    str = this.ActionListLocal(documentElement, client, str3);
                    goto Label_048C;

                case "execute":
                    if (!this.AuthenticClient(socket.RemoteEndPoint))
                    {
                        str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                    }
                    else
                    {
                        str = this.ActionExecute(client);
                    }
                    goto Label_048C;

                case "pause":
                    if (!this.AuthenticClient(socket.RemoteEndPoint))
                    {
                        str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                    }
                    else
                    {
                        str = this.ActionPause(client);
                    }
                    goto Label_048C;

                case "stop":
                    if (!this.AuthenticClient(socket.RemoteEndPoint))
                    {
                        str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                    }
                    else
                    {
                        str = this.ActionStop(client);
                    }
                    goto Label_048C;

                case "retrieve":
                    if (!this.AuthenticClient(socket.RemoteEndPoint))
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
                        str = this.ActionRetrieve(client, str4, str3, str5);
                    }
                    goto Label_048C;

                case "status":
                    str = this.ActionClientStatus(documentElement, client);
                    goto Label_048C;

                case "timer":
                {
                    string str6;
                    int result = -1;
                    if (dictionary.TryGetValue("index", out str6))
                    {
                        int.TryParse(str6, out result);
                    }
                    str6 = null;
                    dictionary.TryGetValue("data", out str6);
                    str = this.ActionClientTimerData(documentElement, client, str6, result);
                    goto Label_048C;
                }
                default:
                    goto Label_048C;
            }
            int level = 100;
            if (dictionary.ContainsKey("level"))
            {
                try
                {
                    int num3 = Convert.ToInt32(dictionary["level"]);
                    if (num3 != -1)
                    {
                        level = num3;
                    }
                }
                catch
                {
                }
            }
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                str = this.ActionOn(documentElement, client, num, level);
            }
            else
            {
                str = "ERROR: Requestor does not have permission.  Please authenticate first.";
            }
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
            XmlNode node = this.CreateResponse();
            node.InnerText = builder.ToString();
            return node.OwnerDocument;
        }

        private object HandleServerRequest(Dictionary<string, object> requestParts, Socket socket)
        {
            Dictionary<string, string> dictionary = (Dictionary<string, string>) requestParts["Params"];
            XmlNode node = this.CreateResponse();
            XmlNode documentElement = node.OwnerDocument.DocumentElement;
            string str = string.Format("ERROR: Unhandled server request '{0}'", dictionary["action"]);
            string str4 = dictionary["action"];
            if (str4 != null)
            {
                if (!(str4 == "list"))
                {
                    if (str4 == "execute")
                    {
                        if (this.AuthenticClient(socket.RemoteEndPoint))
                        {
                            this.ServerBroadcastExecute(socket);
                            str = "OK";
                        }
                        else
                        {
                            str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                        }
                    }
                    else if (str4 == "pause")
                    {
                        if (this.AuthenticClient(socket.RemoteEndPoint))
                        {
                            this.ServerBroadcastPause();
                            str = "OK";
                        }
                        else
                        {
                            str = "ERROR: Requestor does not have permission.  Please authenticate first.";
                        }
                    }
                    else if (str4 == "stop")
                    {
                        if (this.AuthenticClient(socket.RemoteEndPoint))
                        {
                            this.ServerBroadcastStop();
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
                        if (dictionary.TryGetValue("value", out str3) && this.Authenticate(dictionary["value"], socket))
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
                    str = this.ActionListRemote(documentElement, str2);
                }
            }
            node.InnerText = str;
            return documentElement.OwnerDocument;
        }

        private void HandleServerSocket(object socketObject)
        {
            try
            {
                Socket socket = (Socket) socketObject;
                byte[] buffer = new byte[1];
                if (socket.Available != 0)
                {
                    socket.Receive(buffer, 1, SocketFlags.None);
                    switch (((RequestType) buffer[0]))
                    {
                        case RequestType.LoadProgram:
                            this.LoadProgram(socket);
                            return;

                        case RequestType.Execute:
                            this.ExecuteRequest(socket);
                            return;

                        case RequestType.Stop:
                            this.StopRequest(socket);
                            return;

                        case RequestType.Pause:
                            this.PauseRequest(socket);
                            return;

                        case RequestType.Remove:
                            this.Remove(socket);
                            return;

                        case RequestType.ListRemote:
                            this.ListRemote(socket);
                            return;

                        case RequestType.RegisterClient:
                            this.RegisterClient(socket);
                            return;

                        case RequestType.UnregisterClient:
                            this.UnregisterClient(socket);
                            return;

                        case RequestType.ClientEnumeration:
                            this.ClientEnumeration(socket);
                            return;

                        case RequestType.Information:
                        case RequestType.ListLocal:
                        case RequestType.RetrieveLocal:
                        case RequestType.RetrieveRemote:
                        case RequestType.Ack:
                        case RequestType.Nack:
                            return;

                        case RequestType.ChannelOn:
                            this.ChannelOn(socket);
                            return;

                        case RequestType.ChannelOff:
                            this.ChannelOff(socket);
                            return;

                        case RequestType.LoadSequence:
                            this.LoadSequence(socket);
                            return;

                        case RequestType.ChannelToggle:
                            this.ChannelToggle(socket);
                            return;

                        case RequestType.Echo:
                            this.Echo(socket);
                            return;

                        case RequestType.Authenticate:
                            this.Authenticate(socket);
                            return;

                        case RequestType.Retrieve:
                            this.Retrieve(socket);
                            return;

                        case RequestType.ClientStatus:
                            this.ClientStatus(socket);
                            return;

                        case RequestType.ClientEcho:
                            this._ClientEcho(socket);
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
            Socket socket = (Socket) socketObject;
            byte[] buffer = new byte[0x400];
            StringBuilder builder = new StringBuilder();
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
                Dictionary<string, object> requestParts = this.ParseRequest(text);
                string[] strArray = ((string) requestParts["PagePath"]).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string key = HttpUtility.UrlDecode(strArray[0]).ToLower();
                this.m_registeredExecutionClientsNameIdIndex.TryGetValue(key, out num2);
                ExecutionClientStub stub = null;
                if (num2 != 0)
                {
                    this.m_registeredExecutionClientsIdClientIndex.TryGetValue(num2, out stub);
                }
                string str3 = Path.GetFileNameWithoutExtension(strArray[strArray.Length - 1]).ToLower();
                object outerXml = string.Format("Unknown action '{0}'", str3);
                string str4 = str3;
                if (str4 != null)
                {
                    if (!(str4 == "debug"))
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
                        outerXml = this.HandleDebugRequest(stub, requestParts);
                    }
                }
                goto Label_01A7;
            Label_0169:
                if (stub == null)
                {
                    outerXml = string.Format("ERROR: Unknown client '{0}'", key);
                }
                else
                {
                    outerXml = this.HandleCommandRequest(stub, requestParts, socket);
                }
                goto Label_01A7;
            Label_019A:
                outerXml = this.HandleServerRequest(requestParts, socket);
            Label_01A7:
                if (outerXml is XmlDocument)
                {
                    outerXml = ((XmlDocument) outerXml).OuterXml;
                }
                this.SendHeader(string.Format("{0}/{1}", (string) requestParts["Protocol"], (string) requestParts["ProtocolVersion"]), ((string) outerXml).Length, "200 OK", ref socket);
                this.SendToBrowser((string) outerXml, ref socket);
                socket.Close();
            }
            catch (Exception exception)
            {
                this.SendHeader("HTTP/1.1", exception.Message.Length + 7, "200 OK", ref socket);
                this.SendToBrowser("ERROR: " + exception.Message, ref socket);
                socket.Close();
            }
        }

        private void ListenerThread(object param)
        {
            ListenerParams params = (ListenerParams) param;
            try
            {
                while (this.m_running)
                {
                    if (params.Listener.Pending())
                    {
                        Socket state = params.Listener.AcceptSocket();
                        if ((state != null) && state.Connected)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(params.Callback.Invoke), state);
                        }
                        else
                        {
                            if (state == null)
                            {
                                this.ServerStatus("Socket is null");
                            }
                            if (!state.Connected)
                            {
                                this.ServerStatus("Socket is not connected");
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
                this.ServerStatus("SOCKET EXCEPTION: " + exception.Message);
            }
        }

        private void ListRemote(Socket socket)
        {
            this.ServerStatus("Received list remote request");
            if (Sockets.GetSocketByte(socket) == 1)
            {
                socket.Send(new byte[] { (byte) this.m_programs.Count });
                foreach (string str in this.m_programs.Keys)
                {
                    Sockets.SendSocketString(socket, str);
                }
            }
            else
            {
                socket.Send(new byte[] { (byte) this.m_sequences.Count });
                foreach (string str2 in this.m_sequences.Keys)
                {
                    Sockets.SendSocketString(socket, str2);
                }
            }
        }

        private void LoadProgram(Socket socket)
        {
            this.ServerStatus("Received program load request");
            int byteCount = Sockets.GetSocketInt32(socket);
            string socketString = Sockets.GetSocketString(socket);
            byte[] socketBytes = Sockets.GetSocketBytes(socket, byteCount);
            this.ServerStatus("Received program " + socketString);
            this.m_programs[socketString] = new VixenSequenceProgram(socketBytes);
        }

        private void LoadSequence(Socket socket)
        {
            this.ServerStatus("Received sequence load request");
            string socketString = Sockets.GetSocketString(socket);
            byte num = Sockets.GetSocketBytes(socket, 1)[0];
            this.ServerStatus("Program name: \"" + socketString + "\"");
            this.ServerStatus("Index: " + num.ToString());
            this.ServerStatus("Receiving sequence");
            int byteCount = Sockets.GetSocketInt32(socket);
            string item = Sockets.GetSocketString(socket);
            byte[] socketBytes = Sockets.GetSocketBytes(socket, byteCount);
            this.ServerStatus("Received sequence " + item);
            this.m_sequences[item] = socketBytes;
            if (socketString.Length > 0)
            {
                this.ServerStatus("Adding to program");
                if (this.m_programs.ContainsKey(socketString))
                {
                    this.ServerStatus("Program exists");
                    if ((num == 0xff) || (num >= this.m_programs[socketString].Sequences.Count))
                    {
                        this.m_programs[socketString].Sequences.Add(socketBytes);
                        this.m_programs[socketString].SequenceFileNames.Add(item);
                    }
                    else
                    {
                        this.m_programs[socketString].Sequences[num] = socketBytes;
                        this.m_programs[socketString].SequenceFileNames[num] = item;
                    }
                    this.ServerStatus("Sequence assigned to program");
                }
                else
                {
                    this.ServerStatus("Program specified does not exist");
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
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string[] strArray = request.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string str = strArray[0];
            string[] strArray2 = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            dictionary["RequestType"] = strArray2[0];
            string[] strArray3 = strArray2[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            dictionary["Protocol"] = strArray3[0];
            dictionary["ProtocolVersion"] = strArray3[1];
            int index = strArray2[1].IndexOf('?');
            if (index != -1)
            {
                dictionary["PagePath"] = strArray2[1].Substring(0, index);
                Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                dictionary["Params"] = dictionary2;
                foreach (string str2 in strArray2[1].Substring(index + 1).Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] strArray4 = str2.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
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
                strArray2 = str.Substring(6).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                dictionary["HostName"] = strArray2[0];
                dictionary["Port"] = strArray2[1];
            }
            return dictionary;
        }

        private void PauseRequest(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                int key = Sockets.GetSocketInt32(socket);
                if (key == -1)
                {
                    socket.Send(new byte[] { 0x13 });
                    this.ServerBroadcastPause();
                }
                else
                {
                    ExecutionClientStub stub;
                    if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                    {
                        if (this.ClientPause(stub))
                        {
                            socket.Send(new byte[] { 0x13 });
                        }
                        else
                        {
                            socket.Send(new byte[] { 20 });
                        }
                    }
                }
            }
        }

        private void PingThread(object obj)
        {
            Ping ping = new Ping();
            ExecutionClientStub stub = (ExecutionClientStub) ((Array) obj).GetValue(0);
            int num = (int) ((Array) obj).GetValue(1);
            int num2 = 0;
            num2 += (int) (ping.Send(stub.IPAddress).RoundtripTime >> 1);
            num2 += (int) (ping.Send(stub.IPAddress).RoundtripTime >> 1);
            num2 += (int) (ping.Send(stub.IPAddress).RoundtripTime >> 1);
            num2 += (int) (ping.Send(stub.IPAddress).RoundtripTime >> 1);
            num2 = num2 >> 2;
            stub.Ping = (num2 / 100) * 100;
            this.m_threadCountdown--;
        }

        private void RegisterClient(Socket socket)
        {
            this.ServerStatus("Received registration request from " + ((IPEndPoint) socket.RemoteEndPoint).Address.ToString());
            string str = Sockets.GetSocketString(socket).ToLower();
            this.ServerStatus(string.Format("Attempting registration of client '{0}'", str));
            if (!(this.m_registeredClientNames.Contains(str) || this.m_registeredExecutionClientsNameIdIndex.ContainsKey(str)))
            {
                string ipString = socket.RemoteEndPoint.ToString();
                ipString = ipString.Substring(0, ipString.IndexOf(':'));
                ExecutionClientStub stub = new ExecutionClientStub(str, IPAddress.Parse(ipString));
                int key = this.m_registeredClientIndex++;
                this.m_registeredExecutionClientsIdClientIndex.Add(key, stub);
                this.m_registeredExecutionClientsNameIdIndex.Add(str, key);
                this.m_registeredClientNames.Add(str);
                socket.Send(BitConverter.GetBytes(key));
                this.ServerStatus("Registered");
            }
            else
            {
                this.ServerStatus("Registration failed");
                socket.Send(BitConverter.GetBytes(0));
            }
        }

        private void Remove(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                this.ServerStatus("Received remove request");
                string key = Sockets.GetSocketString(socket).ToLower();
                if (this.m_programs.ContainsKey(key))
                {
                    this.m_programs.Remove(key);
                }
                else if (this.m_sequences.ContainsKey(key))
                {
                    this.m_sequences.Remove(key);
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
                if (this.m_programs.TryGetValue(socketString, out program))
                {
                    buffer = new byte[5 + program.Program.Length];
                    buffer[0] = 0x13;
                    BitConverter.GetBytes(program.Program.Length).CopyTo(buffer, 1);
                    program.Program.CopyTo(buffer, 5);
                    socket.Send(buffer);
                    socket.Send(new byte[] { (byte) program.Sequences.Count });
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
                if (this.m_sequences.TryGetValue(socketString, out buffer2))
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
            this.ClientRequest(client, requestType, requestData).Close();
        }

        private TcpClient SendClientRequestWithResponse(ExecutionClientStub client, RequestType requestType, byte[] requestData)
        {
            TcpClient client2 = this.ClientRequest(client, requestType, requestData);
            client2.Client.ReceiveTimeout = 0x7d0;
            client2.Client.NoDelay = true;
            return client2;
        }

        private void SendHeader(string sHttpVersion, int iTotBytes, string sStatusCode, ref Socket socket)
        {
            string s = string.Format("{0} {1}\r\nContent-type: text/html\r\nAccept-Ranges: bytes\r\nContent-Length: {2}\r\n\r\n", sHttpVersion, sStatusCode, iTotBytes);
            this.SendToBrowser(Encoding.ASCII.GetBytes(s), ref socket);
        }

        private void SendToBrowser(string data, ref Socket socket)
        {
            this.SendToBrowser(Encoding.ASCII.GetBytes(data), ref socket);
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
            this.ServerStatus("Received execute request (broadcast)");
            this.BroadcastExecuteRequest(socket);
        }

        private void ServerBroadcastPause()
        {
            this.ServerStatus("Received pause request (broadcast)");
            this.BroadcastPacket(RequestType.Pause, null);
        }

        private void ServerBroadcastStop()
        {
            this.ServerStatus("Received stop request (broadcast)");
            this.BroadcastPacket(RequestType.Stop, null);
        }

        private string[] ServerList(byte type)
        {
            string[] strArray;
            int num = 0;
            if (type == 1)
            {
                strArray = new string[this.m_programs.Keys.Count];
                foreach (string str in this.m_programs.Keys)
                {
                    strArray[num++] = str;
                }
                return strArray;
            }
            if (type == 2)
            {
                return this.m_registeredClientNames.ToArray();
            }
            strArray = new string[this.m_sequences.Keys.Count];
            foreach (string str in this.m_sequences.Keys)
            {
                strArray[num++] = str;
            }
            return strArray;
        }

        private void ServerStatus(string message)
        {
            if (this.ServerNotify != null)
            {
                if (message.Length > 0x33)
                {
                    message = message.Substring(0, 0x33);
                }
                this.ServerNotify(message);
            }
        }

        public void Shutdown()
        {
            if (this.m_running)
            {
                this.Stop();
            }
        }

        public void Start()
        {
            this.m_running = true;
            this.StartServer();
            this.StartWebInterface();
            this.ServerStatus(string.Format("Server started on {0} ({1})", this.m_hostName, this.m_hostAddress));
        }

        private void StartServer()
        {
            this.m_serverListener.Start();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ListenerThread), new ListenerParams(this.m_serverListener, new WaitCallback(this.HandleServerSocket)));
        }

        private void StartWebInterface()
        {
            this.m_webListener.Start();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ListenerThread), new ListenerParams(this.m_webListener, new WaitCallback(this.HandleWebSocket)));
        }

        public void Stop()
        {
            this.m_running = false;
            this.m_serverListener.Stop();
            this.m_webListener.Stop();
            this.m_registeredClientNames.Clear();
            this.m_registeredExecutionClientsIdClientIndex.Clear();
            this.m_registeredExecutionClientsNameIdIndex.Clear();
            this.m_programs.Clear();
            this.m_sequences.Clear();
            this.m_authenticatedClients.Clear();
            this.ServerStatus("Server stopped");
        }

        private void StopRequest(Socket socket)
        {
            if (this.AuthenticClient(socket.RemoteEndPoint))
            {
                int key = Sockets.GetSocketInt32(socket);
                if (key == -1)
                {
                    socket.Send(new byte[] { 0x13 });
                    this.ServerBroadcastStop();
                }
                else
                {
                    ExecutionClientStub stub;
                    if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
                    {
                        if (this.ClientStop(stub))
                        {
                            socket.Send(new byte[] { 0x13 });
                        }
                        else
                        {
                            socket.Send(new byte[] { 20 });
                        }
                    }
                }
            }
        }

        private void UnregisterClient(Socket socket)
        {
            ExecutionClientStub stub;
            int key = Sockets.GetSocketInt32(socket);
            if (this.m_registeredExecutionClientsIdClientIndex.TryGetValue(key, out stub))
            {
                string name = stub.Name;
                this.ServerStatus("Received unregistration request from " + name);
                this.m_registeredClientNames.Remove(name);
                this.m_registeredExecutionClientsIdClientIndex.Remove(key);
                this.m_registeredExecutionClientsNameIdIndex.Remove(name);
            }
            else
            {
                this.ServerStatus("Received unregistration request from an invalid client");
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.m_running;
            }
        }

        public string Password
        {
            get
            {
                return this.m_password;
            }
            set
            {
                this.m_password = value;
                this.m_authenticatedClients.Clear();
            }
        }

        public delegate void ServerErrorEvent(string message);

        public delegate void ServerNotifyEvent(string message);
    }
}

