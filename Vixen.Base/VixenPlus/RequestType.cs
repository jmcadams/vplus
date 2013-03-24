namespace VixenPlus
{
	public enum RequestType
	{
		Ack = 19,
		Authenticate = 21,
		ChannelCount = 26,
		ChannelOff = 12,
		ChannelOn = 11,
		ChannelToggle = 17,
		ClientEcho = 24,
		ClientEnumeration = 9,
		ClientStatus = 23,
		CommitSequence = 28,
		CurrentPosition = 29,
		DownloadSequence = 27,
		Echo = 18,
		Execute = 2,
		Information = 10,
		ListLocal = 14,
		ListRemote = 6,
		LoadProgram = 1,
		LoadSequence = 13,
		LocalClientName = 25,
		Nack = 20,
		Pause = 4,
		RegisterClient = 7,
		Remove = 5,
		Retrieve = 22,
		RetrieveLocal = 15,
		RetrieveRemote = 16,
		Stop = 3,
		TimerCount = 31,
		TimerStatus = 32,
		UnregisterClient = 8,
		UpdateEventValues = 30
	}
}