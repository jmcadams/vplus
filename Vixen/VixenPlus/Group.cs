using System.Collections.Generic;

namespace VixenPlus
{
    internal class Group
    {
        private readonly List<Channel> _mirrorChannels;

        public Group(Channel primaryChannel)
        {
            PrimaryChannel = null;
            _mirrorChannels = null;
            PrimaryChannel = primaryChannel;
            _mirrorChannels = new List<Channel>();
        }

        public Group(string primaryChannelName, IEnumerable<string> mirrorChannelNames, List<Channel> channels)
        {
            PrimaryChannel = null;
            _mirrorChannels = null;
            PrimaryChannel = FindChannel(primaryChannelName, channels);
            _mirrorChannels = new List<Channel>();
            foreach (string str in mirrorChannelNames)
            {
                _mirrorChannels.Add(FindChannel(str, channels));
            }
        }

        public List<Channel> MirrorChannels
        {
            get { return _mirrorChannels; }
        }

        public Channel PrimaryChannel { get; set; }

        private Channel FindChannel(string channelName, IEnumerable<Channel> channels)
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
    }
}