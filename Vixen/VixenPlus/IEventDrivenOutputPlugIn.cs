using System.Xml;

namespace Vixen
{
	public interface IEventDrivenOutputPlugIn : IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
	{
		void Event(byte[] channelValues);
		void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode);
	}
}