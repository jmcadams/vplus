using System.Xml;

namespace VixenPlus {
    public interface IEventlessOutputPlugIn : IOutputPlugIn
    {
        void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer);
    }
}