namespace VixenPlus {
    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        string HardwareMap { get; }
        void Shutdown();
        void Startup();
    }
}