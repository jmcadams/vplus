using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VixenPlus.Properties
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated,
     DebuggerNonUserCode]
    internal class Resources
    {
        private static ResourceManager _resourceManager;

        //internal static Bitmap ChannelOrder
        //{
        //    get { return (Bitmap) ResourceManager.GetObject("ChannelOrder", _resourceCulture); }
        //}

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (!ReferenceEquals(_resourceManager, null)) {
                    return _resourceManager;
                }
                var manager = new ResourceManager("Vixen.Properties.Resources", typeof (Resources).Assembly);
                _resourceManager = manager;
                return _resourceManager;
            }
        }

        //internal static Bitmap ReturnToPrevious
        //{
        //    get { return (Bitmap) ResourceManager.GetObject("ReturnToPrevious", _resourceCulture); }
        //}

        //internal static Bitmap Save
        //{
        //    get { return (Bitmap) ResourceManager.GetObject("Save", _resourceCulture); }
        //}
    }
}