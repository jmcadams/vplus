using System.Collections.Generic;

namespace VixenPlus {
    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        IEnumerable<HardwareMap> HardwareMap { get; }
        void Shutdown();
        void Startup();
    }
}