namespace VixenPlus {
    public interface IHardwarePlugin : IPlugIn, ISetup
    {
        //If the resharper recommendation is followed, then it will break existing plugins.
// ReSharper disable ReturnTypeCanBeEnumerable.Global
        HardwareMap[] HardwareMap { get; }
// ReSharper restore ReturnTypeCanBeEnumerable.Global
        void Shutdown();
        void Startup();
    }
}