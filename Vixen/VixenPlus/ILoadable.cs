using System.Xml;

namespace VixenPlus
{
    public interface ILoadable : IPlugIn
    {
        LoadableDataLocation DataLocationPreference { get; }
        void Loading(XmlNode dataNode);
        void Unloading();
    }
}