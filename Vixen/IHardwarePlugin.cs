namespace VixenPlus {
    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        //If the resharper recommendation is followed, then it will break existing plugins.
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        HardwareMap[] HardwareMap { get; }
        void Shutdown();
        void Startup();
    }
}