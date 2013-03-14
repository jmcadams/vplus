namespace FGDimmer
{
    using System;

    internal class Module
    {
        private bool m_enabled;
        private int m_id;
        private int m_startChannel;

        public Module(int id)
        {
            this.m_id = id;
            this.m_enabled = false;
            this.m_startChannel = 1;
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
            }
        }

        public int ID
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
            }
        }

        public int StartChannel
        {
            get
            {
                return this.m_startChannel;
            }
            set
            {
                this.m_startChannel = value;
            }
        }
    }
}

