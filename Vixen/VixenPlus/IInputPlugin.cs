using System.Xml;

namespace VixenPlus
{
	internal interface IInputPlugin : IHardwarePlugin
	{
		Input[] Inputs { get; }

		bool LiveUpdate { get; }

		bool Record { get; }
		void Initialize(SetupData setupData, XmlNode setupNode);
	}
}