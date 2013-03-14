namespace Vixen
{
    using System;
    using System.Xml;

    public interface IEventlessOutputPlugIn : IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer);
    }
}

