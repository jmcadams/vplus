namespace VixenPlus
{
    using System.Net.Sockets;
    using System.Threading;

    internal class ListenerParams
    {
        private readonly WaitCallback _callback;
        private readonly TcpListener _listener;

        public ListenerParams(TcpListener listener, WaitCallback callback)
        {
            _listener = listener;
            _callback = callback;
        }

        public WaitCallback Callback
        {
            get
            {
                return _callback;
            }
        }

        public TcpListener Listener
        {
            get
            {
                return _listener;
            }
        }
    }
}

