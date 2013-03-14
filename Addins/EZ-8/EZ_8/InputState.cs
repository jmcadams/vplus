namespace EZ_8
{
    using System;

    public class InputState
    {
        private byte m_value;

        internal InputState(byte value)
        {
            this.m_value = value;
        }

        public bool RecordPressed
        {
            get
            {
                return ((this.m_value & 0x20) != 0);
            }
        }

        public bool StartPressed
        {
            get
            {
                return ((this.m_value & 0x10) != 0);
            }
        }
    }
}

