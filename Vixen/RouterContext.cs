using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace VixenPlus {
    internal class RouterContext
    {
        public readonly byte[] EngineBuffer;
        public readonly IExecutable ExecutableObject;
        private bool _isInitialized;
        public readonly List<MappedOutputPlugIn> OutputPluginList;
        public readonly SetupData PluginData;


        public RouterContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject) {
            EngineBuffer = engineBuffer;
            PluginData = pluginData;
            ExecutableObject = executableObject;
            OutputPluginList = new List<MappedOutputPlugIn>();
            foreach (var item in from XmlNode node in PluginData.GetAllPluginData(SetupData.PluginType.Output, true)
                where node.Attributes != null
                select
                    new MappedOutputPlugIn((IOutputPlugIn) OutputPlugins.FindPlugin(node.Attributes["name"].Value, true),
                        Convert.ToInt32(node.Attributes["from"].Value), Convert.ToInt32(node.Attributes["to"].Value), node)) {
                OutputPluginList.Add(item);
            }
        }


        public bool Initialized
        {
            get { return _isInitialized; }
            set
            {
                _isInitialized = value;
                foreach (var outputPlugIn in OutputPluginList)
                {
                    outputPlugIn.ContextInitialized = value;
                }
            }
        }
    }
}