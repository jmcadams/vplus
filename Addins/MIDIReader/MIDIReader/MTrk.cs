namespace MIDIReader
{
    using System;
    using System.IO;

    internal class MTrk : Chunk
    {
        public MTrk(Stream fileStream) : base("MTrk", fileStream)
        {
        }
    }
}

