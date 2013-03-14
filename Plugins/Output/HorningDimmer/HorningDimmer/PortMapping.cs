namespace HorningDimmer
{
    using System;

    public class PortMapping
    {
        private short m_controlPort;
        private short m_dataPort;
        private int m_from = 0;
        private bool m_mapped = false;
        private bool m_reversed = false;
        private short m_statusPort;
        private int m_to = 0;

        public PortMapping(int parallelPortNumber)
        {
            switch (parallelPortNumber)
            {
                case 1:
                    this.m_dataPort = 0x378;
                    this.m_statusPort = 0x379;
                    this.m_controlPort = 890;
                    break;

                case 2:
                    this.m_dataPort = 0x278;
                    this.m_statusPort = 0x279;
                    this.m_controlPort = 0x27a;
                    break;

                case 3:
                    this.m_dataPort = 0x3bc;
                    this.m_statusPort = 0x3bd;
                    this.m_controlPort = 0x3be;
                    break;
            }
        }

        public short ControlPort
        {
            get
            {
                return this.m_controlPort;
            }
        }

        public short DataPort
        {
            get
            {
                return this.m_dataPort;
            }
        }

        public int From
        {
            get
            {
                return this.m_from;
            }
            set
            {
                this.m_from = value;
            }
        }

        public bool Mapped
        {
            get
            {
                return this.m_mapped;
            }
            set
            {
                this.m_mapped = value;
            }
        }

        public bool Reversed
        {
            get
            {
                return this.m_reversed;
            }
            set
            {
                this.m_reversed = value;
            }
        }

        public short StatusPort
        {
            get
            {
                return this.m_statusPort;
            }
        }

        public int To
        {
            get
            {
                return this.m_to;
            }
            set
            {
                this.m_to = value;
            }
        }
    }
}

