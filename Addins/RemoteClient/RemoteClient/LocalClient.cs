namespace RemoteClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Vixen;

    internal class LocalClient
    {
        private ClientContext m_clientContext;
        private TcpListener m_clientListener;
        private byte[] m_findRequestBuffer = new byte[8];
        private byte[] m_findResultBuffer = new byte[8];
        private Thread m_listenerThread = null;
        private Socket m_responseSocket = null;
        private bool m_running = false;

        private void ListenForDiscoveryRequests()
        {
            EndPoint remoteEP = new IPEndPoint(0L, 0);
            this.m_responseSocket.BeginReceiveFrom(this.m_findRequestBuffer, 0, this.m_findRequestBuffer.Length, SocketFlags.None, ref remoteEP, new AsyncCallback(this.OnReceiveDiscoveryRequest), null);
        }

        private void LocalClientListener()
        {
            try
            {
                while (this.m_running)
                {
                    if (this.m_clientListener.Pending())
                    {
                        Socket state = this.m_clientListener.AcceptSocket();
                        if ((state != null) && state.Connected)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.LocalClientRequestHandler), state);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (SocketException)
            {
            }
        }

        private void LocalClientRequestHandler(object state)
        {
            Socket socket = (Socket) state;
            switch (((RequestType) Sockets.GetSocketByte(socket)))
            {
                case RequestType.Execute:
                    ClientCommandHandlers.Execute(socket, this.m_clientContext, Sockets.GetSocketInt32(socket), Sockets.GetSocketInt32(socket));
                    break;

                case RequestType.Stop:
                    ClientCommandHandlers.Stop(socket, this.m_clientContext);
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

                case RequestType.LocalClientName:
                    ClientCommandHandlers.ClientName(socket);
                    break;

                case RequestType.ChannelCount:
                    ClientCommandHandlers.ChannelCount(socket, this.m_clientContext);
                    break;

                case RequestType.DownloadSequence:
                    ClientCommandHandlers.DownloadSequence(socket, Sockets.GetSocketString(socket));
                    break;

                case RequestType.CommitSequence:
                    ClientCommandHandlers.CommitSequence(socket, Sockets.GetSocketString(socket));
                    break;

                case RequestType.CurrentPosition:
                    ClientCommandHandlers.CurrentPosition(socket, this.m_clientContext);
                    break;

                case RequestType.UpdateEventValues:
                    ClientCommandHandlers.UpdateEventValues(socket, this.m_clientContext);
                    break;
            }
        }

        private void OnReceiveDiscoveryRequest(IAsyncResult result)
        {
            EndPoint endPoint = new IPEndPoint(0L, 0);
            int num = this.m_responseSocket.EndReceiveFrom(result, ref endPoint);
            FindRequest request = new FindRequest(this.m_findRequestBuffer);
            FindResult result2 = new FindResult();
            result2.resultAddress = BitConverter.ToInt32(Sockets.GetIPV4Address().GetAddressBytes(), 0);
            result2.resultPort = 0xa1bc;
            int size = result2.SerializeToPacket(this.m_findResultBuffer);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint remoteEP = new IPEndPoint(((IPEndPoint) endPoint).Address, 0xa1bd);
            socket.SendTo(this.m_findResultBuffer, size, SocketFlags.None, remoteEP);
            socket.Close();
            this.ListenForDiscoveryRequests();
        }

        public bool Start()
        {
            if (!this.m_running)
            {
                string str;
                this.m_clientContext = new ClientContext(true);
                this.m_responseSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint localEP = new IPEndPoint(IPAddress.Any, 0xa1bc);
                this.m_responseSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                this.m_responseSocket.Bind(localEP);
                this.ListenForDiscoveryRequests();
                this.m_running = true;
                try
                {
                    this.m_clientListener = new TcpListener(new IPEndPoint(Sockets.GetIPV4Address(), 0xa1bd));
                    this.m_clientListener.Start();
                    this.m_listenerThread = new Thread(new ThreadStart(this.LocalClientListener));
                    this.m_listenerThread.Start();
                }
                catch (Exception exception)
                {
                    ErrorLog.Log(exception.Message);
                    this.Stop();
                    return false;
                }
                Preference2 userPreferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
                Profile profile = null;
                if ((str = userPreferences.GetString("DefaultProfile")).Length > 0)
                {
                    profile = new Profile(Path.Combine(Paths.ProfilePath, str + ".pro"));
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
                this.m_clientListener.Stop();
                this.m_listenerThread = null;
                this.m_clientContext.ContextObject = null;
                if ((this.m_responseSocket != null) && this.m_responseSocket.Connected)
                {
                    this.m_responseSocket.Close(1);
                }
                this.m_responseSocket = null;
            }
        }

        public bool Running
        {
            get
            {
                return this.m_running;
            }
        }
    }
}

