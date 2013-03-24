namespace VixenPlus
{
	public interface ITriggerPlugin : ILoadable
	{
		string InterfaceTypeName { get; }

		int TriggerCount { get; }
		void Setup();
	}
}