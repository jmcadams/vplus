using System.Resources;

namespace AppUpdate.Properties {
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [CompilerGenerated]
    internal class Resources {
        private static ResourceManager _resourceMan;


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (!ReferenceEquals(_resourceMan, null)) {
                    return _resourceMan;
                }
                var manager = new ResourceManager("AppUpdate.Properties.Resources", typeof (Resources).Assembly);
                _resourceMan = manager;
                return _resourceMan;
            }
        }
    }
}
