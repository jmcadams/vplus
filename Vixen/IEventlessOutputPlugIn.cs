using System.Xml;

public interface IEventlessOutputPlugIn : IOutputPlugIn
{
    void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer);
}