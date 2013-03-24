namespace RemoteClient
{
    using System;
    using VixenPlus;

    internal class ClientContext
    {
        private int m_channelCount;
        private IExecutable m_contextObject;
        private int m_executionContextHandle;
        private IExecution m_executionInterface;
        private bool m_makeObjectsLocal;

        public ClientContext()
        {
            this.m_contextObject = null;
            this.m_executionContextHandle = 0;
            this.m_executionInterface = null;
            this.m_channelCount = 0;
            this.m_makeObjectsLocal = false;
            this.Construct();
        }

        public ClientContext(bool makeObjectsLocal)
        {
            this.m_contextObject = null;
            this.m_executionContextHandle = 0;
            this.m_executionInterface = null;
            this.m_channelCount = 0;
            this.m_makeObjectsLocal = false;
            this.Construct();
            this.m_makeObjectsLocal = makeObjectsLocal;
        }

        private void Construct()
        {
            object obj2;
            if (Interfaces.Available.TryGetValue("IExecution", out obj2))
            {
                this.m_executionInterface = (IExecution) obj2;
            }
            this.m_executionContextHandle = this.m_executionInterface.RequestContext(false, false, null);
        }

        public int ChannelCount
        {
            get
            {
                return this.m_channelCount;
            }
        }

        public IExecutable ContextObject
        {
            get
            {
                return this.m_contextObject;
            }
            set
            {
                if (this.m_executionContextHandle != 0)
                {
                    this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
                    this.m_executionContextHandle = 0;
                    this.m_contextObject = null;
                    this.m_channelCount = 0;
                }
                if (value != null)
                {
                    if (this.m_executionContextHandle == 0)
                    {
                        this.m_executionContextHandle = this.m_executionInterface.RequestContext(false, false, null);
                    }
                    if (this.m_executionContextHandle != 0)
                    {
                        this.m_contextObject = value;
                        this.m_contextObject.TreatAsLocal = this.m_makeObjectsLocal;
                        this.m_executionInterface.SetSynchronousContext(this.m_executionContextHandle, this.m_contextObject);
                        this.m_channelCount = this.m_contextObject.Channels.Count;
                    }
                }
            }
        }

        public int ExecutionContextHandle
        {
            get
            {
                return this.m_executionContextHandle;
            }
        }

        public IExecution ExecutionInterface
        {
            get
            {
                return this.m_executionInterface;
            }
        }
    }
}

