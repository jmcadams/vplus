﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace AdjustablePreview.Properties {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    internal class Resources {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("AdjustablePreview.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channel Cleared.
        /// </summary>
        internal static string ChannelCleared {
            get {
                return ResourceManager.GetString("ChannelCleared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channel Copied.
        /// </summary>
        internal static string ChannelCopied {
            get {
                return ResourceManager.GetString("ChannelCopied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Background image saved..
        /// </summary>
        internal static string ImageSaved {
            get {
                return ResourceManager.GetString("ImageSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are not any channels to create a preview in the selected profile..
        /// </summary>
        internal static string NoChannelsInProfile {
            get {
                return ResourceManager.GetString("NoChannelsInProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nothing to copy.
        /// </summary>
        internal static string NothingToCopy {
            get {
                return ResourceManager.GetString("NothingToCopy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to save your changes?.
        /// </summary>
        internal static string SaveChanges {
            get {
                return ResourceManager.GetString("SaveChanges", resourceCulture);
            }
        }
    }
}
