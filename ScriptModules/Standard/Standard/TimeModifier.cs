namespace Standard
{
    using System;

    public abstract class TimeModifier : ITimeModifier, IModifier
    {
        private int m_value;

        public TimeModifier(int value)
        {
            this.m_value = value;
        }

        public uint Hour
        {
            get
            {
                return (this.Type | 0x6ee80);
            }
        }

        public uint Hours
        {
            get
            {
                return (this.Type | ((uint) ((this.m_value * 0x36ee80) & 0xfffff)));
            }
        }

        public uint Millisecond
        {
            get
            {
                return (this.Type | 1);
            }
        }

        public uint Milliseconds
        {
            get
            {
                return (this.Type | ((uint) this.m_value));
            }
        }

        public uint Minute
        {
            get
            {
                return (this.Type | 0xea60);
            }
        }

        public uint Minutes
        {
            get
            {
                return (this.Type | ((uint) ((this.m_value * 0xea60) & 0xfffff)));
            }
        }

        public uint Second
        {
            get
            {
                return (this.Type | 0x3e8);
            }
        }

        public uint Seconds
        {
            get
            {
                return (this.Type | ((uint) ((this.m_value * 0x3e8) & 0xfffff)));
            }
        }

        public abstract uint Type { get; }

        public uint TypeValue
        {
            get
            {
                return (this.Type | ((uint) this.Value));
            }
        }

        public virtual int Value
        {
            get
            {
                return this.m_value;
            }
        }
    }
}

