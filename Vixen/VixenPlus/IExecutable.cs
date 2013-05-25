using System;
using System.Collections.Generic;

namespace VixenPlus
{
    public interface IExecutable : IMaskable, IDisposable
    {
        int AudioDeviceIndex { get; }

        int AudioDeviceVolume { get; }

        bool CanBePlayed { get; }

        List<Channel> Channels { get; }

        List<Channel> FullChannels { get; } 

        string FileName { get; }

        ulong Key { get; }

        string Name { get; }

        List<Channel> OutputChannels { get; }

        SetupData PlugInData { get; }

        bool TreatAsLocal { get; set; }

        object UserData { get; set; }
    }
}