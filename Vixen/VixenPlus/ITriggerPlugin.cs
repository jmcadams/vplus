namespace Vixen
{
	public interface ITriggerPlugin : ILoadable, IPlugIn
	{
		string InterfaceTypeName { get; }

		int TriggerCount { get; }
		void Setup();
	}
}