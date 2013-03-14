namespace FC4_RC4
{
    using System;
    using System.IO.Ports;

    internal interface IModule
    {
        void OutputEvent(SerialPort port, byte[] channelValues);

        byte Address { set; }

        int ChannelCount { get; }

        int StartChannel { set; }
    }
}

