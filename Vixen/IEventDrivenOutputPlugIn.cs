using System.Xml;

namespace VixenPlus {
    public interface IEventDrivenOutputPlugIn : IHardwarePlugin
    {
        void Event(byte[] channelValues);
        void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode);
    }
}