namespace VixenPlus
{
	public interface IAddIn : ILoadable, IPlugIn
	{
		bool Execute(EventSequence sequence);
	}
}