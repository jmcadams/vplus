namespace Vixen
{
    using System;

    internal interface IMedia : ITickSource
    {
        string[] GetOutputDevices(int outputTypeIndex);
        int Load(string mediaFileName);

        string[] OutputTypes { get; }

        int Position { get; set; }

        bool SupportsVariableSpeeds { get; }
    }
}

