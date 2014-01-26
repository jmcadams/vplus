using System.Xml;

public interface ILoadable : IPlugIn
{
    LoadableDataLocation DataLocationPreference { get; }
    void Loading(XmlNode dataNode);
    void Unloading();
}