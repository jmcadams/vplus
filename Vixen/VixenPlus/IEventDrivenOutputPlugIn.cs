using System.Xml;

namespace VixenPlus
{
	public interface IEventDrivenOutputPlugIn : IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
	{
		void Event(byte[] channelValues);
		void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode);
	}
}