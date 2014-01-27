using System.Collections.Generic;

public interface IHardwarePlugin : IPlugIn, ISetup
{
    IEnumerable<HardwareMap> HardwareMap { get; }
    void Shutdown();
    void Startup();
}