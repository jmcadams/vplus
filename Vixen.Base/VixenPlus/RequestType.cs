namespace VixenPlus
{
	public enum RequestType
    {
        Ack = 0x13,
        Authenticate = 0x15,
        ChannelCount = 0x1a,
        ChannelOff = 12,
        ChannelOn = 11,
        ChannelToggle = 0x11,
        ClientEcho = 0x18,
        ClientEnumeration = 9,
        ClientStatus = 0x17,
        CommitSequence = 0x1c,
        CurrentPosition = 0x1d,
        DownloadSequence = 0x1b,
        Echo = 0x12,
        Execute = 2,
        Information = 10,
        ListLocal = 14,
        ListRemote = 6,
        LoadProgram = 1,
        LoadSequence = 13,
        LocalClientName = 0x19,
        Nack = 20,
        Pause = 4,
        RegisterClient = 7,
        Remove = 5,
        Retrieve = 0x16,
        RetrieveLocal = 15,
        RetrieveRemote = 0x10,
        Stop = 3,
        TimerCount = 0x1f,
        TimerStatus = 0x20,
        UnregisterClient = 8,
        UpdateEventValues = 30
    }
}

