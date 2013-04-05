namespace VixenEditor.Properties
{
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager _resourceManager;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(_resourceManager, null))
                {
                    var manager = new ResourceManager("VixenEditor.Properties.Resources", typeof(Resources).Assembly);
                    _resourceManager = manager;
                }
                return _resourceManager;
            }
        }
    }
}

