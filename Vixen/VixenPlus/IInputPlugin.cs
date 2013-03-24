using System.Xml;

namespace Vixen
{
	internal interface IInputPlugin : IHardwarePlugin, IPlugIn, ISetup
	{
		Input[] Inputs { get; }

		bool LiveUpdate { get; }

		bool Record { get; }
		void Initialize(SetupData setupData, XmlNode setupNode);
	}
}