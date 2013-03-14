namespace Standard
{
    using System;

    internal class TimedState
    {
        public IChannelEnumerable Channels;
        public byte[,] EventValues;

        public TimedState(byte[,] eventValues, IChannelEnumerable channels)
        {
            this.Channels = channels;
            this.EventValues = eventValues;
        }
    }
}

