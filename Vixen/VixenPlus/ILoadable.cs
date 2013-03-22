namespace Vixen
{
    using System;
    using System.Xml;

    public interface ILoadable : IPlugIn
    {
        void Loading(XmlNode dataNode);
        void Unloading();

        LoadableDataLocation DataLocationPreference { get; }
    }
}

