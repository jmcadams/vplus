namespace Vixen
{
	internal interface IObjectQuery
	{
		bool UsesInputPlugin(InputPlugin plugin);
		bool UsesOutputPlugin(IOutputPlugIn plugin);
		bool UsesProgram(SequenceProgram program);
		bool UsesSequence(EventSequence sequence);
	}
}