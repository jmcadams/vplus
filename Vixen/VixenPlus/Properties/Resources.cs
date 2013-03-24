using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Vixen.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated,
	 DebuggerNonUserCode]
	internal class Resources
	{
		private static CultureInfo resourceCulture;
		private static ResourceManager resourceMan;

		internal static Bitmap ChannelOrder
		{
			get { return (Bitmap) ResourceManager.GetObject("ChannelOrder", resourceCulture); }
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get { return resourceCulture; }
			set { resourceCulture = value; }
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (ReferenceEquals(resourceMan, null))
				{
					var manager = new ResourceManager("Vixen.Properties.Resources", typeof (Resources).Assembly);
					resourceMan = manager;
				}
				return resourceMan;
			}
		}

		internal static Bitmap ReturnToPrevious
		{
			get { return (Bitmap) ResourceManager.GetObject("ReturnToPrevious", resourceCulture); }
		}

		internal static Bitmap Save
		{
			get { return (Bitmap) ResourceManager.GetObject("Save", resourceCulture); }
		}
	}
}