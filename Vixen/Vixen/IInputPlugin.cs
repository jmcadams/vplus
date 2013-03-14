namespace Vixen
{
    using System;
    using System.Xml;

    internal interface IInputPlugin : IHardwarePlugin, IPlugIn, ISetup
    {
        void Initialize(SetupData setupData, XmlNode setupNode);

        Input[] Inputs { get; }

        bool LiveUpdate { get; }

        bool Record { get; }
    }
}

