namespace VixenPlus
{
	using System.Collections.Generic;

    internal class VixenSequenceProgram
    {
        public byte[] Program;
        public List<string> SequenceFileNames = new List<string>();
        public List<byte[]> Sequences = new List<byte[]>();

        public VixenSequenceProgram(byte[] sequenceProgram)
        {
            Program = sequenceProgram;
        }
    }
}

