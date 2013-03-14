namespace Vixen
{
    using System;
    using System.Collections.Generic;

    internal class Group
    {
        private List<Channel> m_mirrorChannels;
        private Channel m_primaryChannel;

        public Group(Channel primaryChannel)
        {
            this.m_primaryChannel = null;
            this.m_mirrorChannels = null;
            this.m_primaryChannel = primaryChannel;
            this.m_mirrorChannels = new List<Channel>();
        }

        public Group(string primaryChannelName, List<string> mirrorChannelNames, List<Channel> channels)
        {
            this.m_primaryChannel = null;
            this.m_mirrorChannels = null;
            this.m_primaryChannel = this.FindChannel(primaryChannelName, channels);
            this.m_mirrorChannels = new List<Channel>();
            foreach (string str in mirrorChannelNames)
            {
                this.m_mirrorChannels.Add(this.FindChannel(str, channels));
            }
        }

        private Channel FindChannel(string channelName, List<Channel> channels)
        {
            foreach (Channel channel in channels)
            {
                if (channelName == channel.Name)
                {
                    return channel;
                }
            }
            return null;
        }

        public List<Channel> MirrorChannels
        {
            get
            {
                return this.m_mirrorChannels;
            }
        }

        public Channel PrimaryChannel
        {
            get
            {
                return this.m_primaryChannel;
            }
            set
            {
                this.m_primaryChannel = value;
            }
        }
    }
}

