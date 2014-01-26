using System.Xml;

public interface IEventDrivenOutputPlugIn : IOutputPlugIn
{
    void Event(byte[] channelValues);
    void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode);
}