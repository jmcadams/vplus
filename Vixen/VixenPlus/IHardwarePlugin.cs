namespace VixenPlus
{
    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        HardwareMap[] HardwareMap { get; }
        void Shutdown();
        void Startup();
    }
}