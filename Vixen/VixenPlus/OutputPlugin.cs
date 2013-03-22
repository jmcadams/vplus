namespace Vixen
{
    using System;

    public class OutputPlugin
    {
        private bool m_enabled;
        private int m_from;
        private int m_id;
        private string m_name;
        private int m_to;

        public OutputPlugin(string name, int id, bool enabled, int from, int to)
        {
            this.m_name = name;
            this.m_id = id;
            this.m_enabled = enabled;
            this.m_from = from;
            this.m_to = to;
        }

        public int ChannelFrom
        {
            get
            {
                return this.m_from;
            }
        }

        public int ChannelTo
        {
            get
            {
                return this.m_to;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
        }

        public int Id
        {
            get
            {
                return this.m_id;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }
    }
}

