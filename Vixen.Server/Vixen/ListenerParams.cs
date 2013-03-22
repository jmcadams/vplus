namespace Vixen
{
    using System;
    using System.Net.Sockets;
    using System.Threading;

    internal class ListenerParams
    {
        private WaitCallback m_callback;
        private TcpListener m_listener;

        public ListenerParams(TcpListener listener, WaitCallback callback)
        {
            this.m_listener = listener;
            this.m_callback = callback;
        }

        public WaitCallback Callback
        {
            get
            {
                return this.m_callback;
            }
        }

        public TcpListener Listener
        {
            get
            {
                return this.m_listener;
            }
        }
    }
}

