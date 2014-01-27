using System.Collections.Generic;

internal class OutputPort
{
    public readonly int Index;
    public bool IsExpanded;
    public readonly List<IHardwarePlugin> ReferencingPlugins = new List<IHardwarePlugin>();
    public bool Shared;
    public readonly string StringFormat;

    public OutputPort(int index, bool shared, string stringFormat)
    {
        Index = index;
        Shared = shared;
        StringFormat = stringFormat;
    }
}