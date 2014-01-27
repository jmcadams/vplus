using System;
using System.Collections.Generic;
using System.Xml;

internal class RouterContext : ITickSource
{
    public readonly byte[] EngineBuffer;
    public readonly IExecutable ExecutableObject;
    private bool _isInitialized;
    public readonly List<MappedOutputPlugIn> OutputPluginList;
    public readonly SetupData PluginData;
    public readonly ITickSource TickSource;

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
                Convert.ToInt32(node.Attributes["to"].Value), node);
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