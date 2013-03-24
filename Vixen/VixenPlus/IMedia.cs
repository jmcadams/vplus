namespace Vixen
{
	internal interface IMedia : ITickSource
	{
		string[] OutputTypes { get; }

		int Position { get; set; }

		bool SupportsVariableSpeeds { get; }
		string[] GetOutputDevices(int outputTypeIndex);
		int Load(string mediaFileName);
	}
}