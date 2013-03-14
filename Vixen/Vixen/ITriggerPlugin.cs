namespace Vixen
{
    using System;

    public interface ITriggerPlugin : ILoadable, IPlugIn
    {
        void Setup();

        string InterfaceTypeName { get; }

        int TriggerCount { get; }
    }
}

