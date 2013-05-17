using System;
using System.Collections.Generic;
using System.Xml;

namespace VixenPlus
{
    internal class RouterContext : ITickSource
    {
        public byte[] EngineBuffer;
        public IExecutable ExecutableObject;
        public bool IsInitialized = false;
        public List<MappedOutputPlugIn> OutputPluginList;
        public SetupData PluginData;
        public ITickSource TickSource;

        public RouterContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject, ITickSource tickSource)
        {
            EngineBuffer = engineBuffer;
            PluginData = pluginData;
            ExecutableObject = executableObject;
            OutputPluginList = new List<MappedOutputPlugIn>();
            TickSource = tickSource ?? this;
            foreach (XmlNode node in PluginData.GetAllPluginData(SetupData.PluginType.Output, true))
            {
                if (node.Attributes == null) {
                    continue;
                }
                var item = new MappedOutputPlugIn((IOutputPlugIn) OutputPlugins.FindPlugin(node.Attributes["name"].Value, true),
                                                  Convert.ToInt32(node.Attributes["from"].Value),
                                                  Convert.ToInt32(node.Attributes["to"].Value), true, node);
                OutputPluginList.Add(item);
            }
        }

        public bool Initialized
        {
            get { return IsInitialized; }
            set
            {
                IsInitialized = value;
                foreach (var outputPlugIn in OutputPluginList)
                {
                    outputPlugIn.ContextInitialized = value;
                }
            }
        }

        public int Milliseconds
        {
            get { return 0; }
        }
    }
}