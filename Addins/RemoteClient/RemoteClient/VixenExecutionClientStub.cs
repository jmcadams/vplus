namespace RemoteClient
{
    using System;

    public class VixenExecutionClientStub
    {
        private int m_channelCount;
        private string m_clientName;
        private int m_id;

        public VixenExecutionClientStub(int clientID, int channelCount, string clientName)
        {
            this.m_id = clientID;
            this.m_channelCount = channelCount;
            this.m_clientName = clientName;
        }

        public override string ToString()
        {
            return this.m_clientName;
        }

        public int ChannelCount
        {
            get
            {
                return this.m_channelCount;
            }
        }

        public int ID
        {
            get
            {
                return this.m_id;
            }
        }
    }
}

