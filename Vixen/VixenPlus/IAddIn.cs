namespace VixenPlus
{
	public interface IAddIn : ILoadable
	{
		bool Execute(EventSequence sequence);
	}
}