using System.Collections.Generic;

internal class OutputPort
{
    public readonly int Index;
    public bool IsExpanded;
    public readonly List<IHardwarePlugin> ReferencingPlugins = new List<IHardwarePlugin>();

    public OutputPort(int index)
    {
        Index = index;
    }
}