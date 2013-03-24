namespace Standard
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using VixenPlus;

    public class VixenChannel : IChannelEnumerable, IEnumerable<VixenChannel>, IEnumerable
    {
        private Channel[] m_channel;
        private ChannelCollection m_channelCollection;

        public VixenChannel(Channel channel)
        {
            this.m_channel = new Channel[] { channel };
            this.m_channelCollection = new ChannelCollection();
            this.m_channelCollection.Add(this);
        }

        public IEnumerator<VixenChannel> GetEnumerator()
        {
            return this.m_channelCollection.GetEnumerator();
        }

        public static implicit operator Channel(VixenChannel channel)
        {
            return channel.m_channel[0];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_channel.GetEnumerator();
        }

        public override string ToString()
        {
            return this.m_channel[0].ToString();
        }

        public int Count
        {
            get
            {
                return 1;
            }
        }

        public VixenChannel this[int index]
        {
            get
            {
                return this;
            }
            set
            {
                this.m_channel = value.m_channel;
            }
        }

        public string Name
        {
            get
            {
                return this.m_channel[0].Name;
            }
        }

        public int OutputChannel
        {
            get
            {
                return this.m_channel[0].OutputChannel;
            }
        }
    }
}

