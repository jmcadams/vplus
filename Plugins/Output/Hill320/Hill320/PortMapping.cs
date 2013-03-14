namespace Hill320
{
    using System;

    public class PortMapping
    {
        private ushort m_controlPort;
        private ushort m_dataPort;
        private ushort m_statusPort;

        public PortMapping(int portAddress)
        {
            this.m_dataPort = (ushort) portAddress;
            this.m_statusPort = (ushort) (portAddress + 1);
            this.m_controlPort = (ushort) (portAddress + 2);
        }

        public ushort ControlPort
        {
            get
            {
                return this.m_controlPort;
            }
        }

        public ushort DataPort
        {
            get
            {
                return this.m_dataPort;
            }
        }

        public ushort StatusPort
        {
            get
            {
                return this.m_statusPort;
            }
        }
    }
}

