namespace Vixen.Dialogs
{
    using System;
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
            this.Name = name;
            this.Index = index;
            this.Shared = shared;
            this.StringFormat = stringFormat;
        }
    }
}

