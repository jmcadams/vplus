using System.Collections.Generic;

namespace VixenPlus
{
	internal class Group
	{
		private readonly List<Channel> m_mirrorChannels;

		public Group(Channel primaryChannel)
		{
			PrimaryChannel = null;
			m_mirrorChannels = null;
			PrimaryChannel = primaryChannel;
			m_mirrorChannels = new List<Channel>();
		}

		public Group(string primaryChannelName, List<string> mirrorChannelNames, List<Channel> channels)
		{
			PrimaryChannel = null;
			m_mirrorChannels = null;
			PrimaryChannel = FindChannel(primaryChannelName, channels);
			m_mirrorChannels = new List<Channel>();
			foreach (string str in mirrorChannelNames)
			{
				m_mirrorChannels.Add(FindChannel(str, channels));
			}
		}

		public List<Channel> MirrorChannels
		{
			get { return m_mirrorChannels; }
		}

		public Channel PrimaryChannel { get; set; }

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
	}
}