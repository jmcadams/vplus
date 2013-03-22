namespace Vixen
{
    using System;

    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        void Shutdown();
        void Startup();

        Vixen.HardwareMap[] HardwareMap { get; }
    }
}

