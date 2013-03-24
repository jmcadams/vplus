namespace VixenPlus
{
    using System;
    using System.Net;

    internal class ExecutionClientStub
    {
        private System.Net.IPAddress m_ipAddress;
        private int m_msPing = 0;
        private string m_name;

        public ExecutionClientStub(string name, System.Net.IPAddress ipAddress)
        {
            this.m_name = name;
            this.m_ipAddress = ipAddress;
        }

        public System.Net.IPAddress IPAddress
        {
            get
            {
                return this.m_ipAddress;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public int Ping
        {
            get
            {
                return this.m_msPing;
            }
            set
            {
                this.m_msPing = value;
            }
        }
    }
}

