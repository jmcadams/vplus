using System.Xml;

namespace Vixen
{
	public interface IEventlessOutputPlugIn : IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
	{
		void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer);
	}
}