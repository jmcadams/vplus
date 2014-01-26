using System.Collections.Generic;

internal class OutputPort
{
    public int Index;
    public bool IsExpanded;
    public string Name;
    public List<IHardwarePlugin> ReferencingPlugins = new List<IHardwarePlugin>();
    public bool Shared;
    public string StringFormat;

    public OutputPort(string name, int index, bool shared, string stringFormat)
    {
        Name = name;
        Index = index;
        Shared = shared;
        StringFormat = stringFormat;
    }
}