namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class RouterContext : ITickSource
    {
        public byte[] EngineBuffer;
        public IExecutable ExecutableObject;
        public bool m_initialized = false;
        public List<MappedOutputPlugIn> OutputPluginList;
        public SetupData PluginData;
        public ITickSource TickSource;

        public RouterContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject, ITickSource tickSource)
        {
            this.EngineBuffer = engineBuffer;
            this.PluginData = pluginData;
            this.ExecutableObject = executableObject;
            this.OutputPluginList = new List<MappedOutputPlugIn>();
            if (tickSource == null)
            {
                this.TickSource = this;
            }
            else
            {
                this.TickSource = tickSource;
            }
            foreach (XmlNode node in this.PluginData.GetAllPluginData(SetupData.PluginType.Output, true))
            {
                MappedOutputPlugIn item = new MappedOutputPlugIn((IOutputPlugIn) OutputPlugins.FindPlugin(node.Attributes["name"].Value, true), Convert.ToInt32(node.Attributes["from"].Value), Convert.ToInt32(node.Attributes["to"].Value), true, node);
                this.OutputPluginList.Add(item);
            }
        }

        public bool Initialized
        {
            get
            {
                return this.m_initialized;
            }
            set
            {
                this.m_initialized = value;
                foreach (MappedOutputPlugIn @in in this.OutputPluginList)
                {
                    @in.ContextInitialized = value;
                }
            }
        }

        public int Milliseconds
        {
            get
            {
                return 0;
            }
        }
    }
}

